// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFHeaderFooter
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace NPOI.XWPF.UserModel
{
  public abstract class XWPFHeaderFooter : POIXMLDocumentPart, IBody
  {
    protected List<XWPFParagraph> paragraphs = new List<XWPFParagraph>(1);
    protected List<XWPFTable> tables = new List<XWPFTable>(1);
    protected List<XWPFPictureData> pictures = new List<XWPFPictureData>();
    protected List<IBodyElement> bodyElements = new List<IBodyElement>(1);
    protected CT_HdrFtr headerFooter;
    protected XWPFDocument document;

    public XWPFHeaderFooter(XWPFDocument doc, CT_HdrFtr hdrFtr)
    {
      if (doc == null)
        throw new NullReferenceException();
      this.document = doc;
      this.headerFooter = hdrFtr;
      this.ReadHdrFtr();
    }

    protected XWPFHeaderFooter()
    {
    }

    public XWPFHeaderFooter(POIXMLDocumentPart parent, PackagePart part, PackageRelationship rel)
      : base(parent, part, rel)
    {
      this.document = (XWPFDocument) this.GetParent();
      if (this.document == null)
        throw new NullReferenceException();
    }

    internal override void OnDocumentRead()
    {
      foreach (POIXMLDocumentPart relation in this.GetRelations())
      {
        if (relation is XWPFPictureData)
        {
          XWPFPictureData picData = (XWPFPictureData) relation;
          this.pictures.Add(picData);
          this.document.RegisterPackagePictureData(picData);
        }
      }
    }

    public CT_HdrFtr _getHdrFtr()
    {
      return this.headerFooter;
    }

    public IList<IBodyElement> BodyElements
    {
      get
      {
        return (IList<IBodyElement>) this.bodyElements.AsReadOnly();
      }
    }

    public IList<XWPFParagraph> Paragraphs
    {
      get
      {
        return (IList<XWPFParagraph>) this.paragraphs.AsReadOnly();
      }
    }

    public IList<XWPFTable> GetTables()
    {
      return (IList<XWPFTable>) this.tables.AsReadOnly();
    }

    public string GetText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.paragraphs.Count; ++index)
      {
        if (!this.paragraphs[index].IsEmpty())
        {
          string text = this.paragraphs[index].GetText();
          if (text != null && text.Length > 0)
          {
            stringBuilder.Append(text);
            stringBuilder.Append('\n');
          }
        }
      }
      IList<XWPFTable> tables = this.GetTables();
      for (int index = 0; index < tables.Count; ++index)
      {
        string text = tables[index].GetText();
        if (text != null && text.Length > 0)
        {
          stringBuilder.Append(text);
          stringBuilder.Append('\n');
        }
      }
      return stringBuilder.ToString();
    }

    public void SetHeaderFooter(CT_HdrFtr headerFooter)
    {
      this.headerFooter = headerFooter;
      this.ReadHdrFtr();
    }

    public XWPFTable GetTable(CT_Tbl ctTable)
    {
      foreach (XWPFTable table in this.tables)
      {
        if (table == null)
          return (XWPFTable) null;
        if (table.GetCTTbl().Equals((object) ctTable))
          return table;
      }
      return (XWPFTable) null;
    }

    public XWPFParagraph GetParagraph(CT_P p)
    {
      foreach (XWPFParagraph paragraph in this.paragraphs)
      {
        if (paragraph.GetCTP().Equals((object) p))
          return paragraph;
      }
      return (XWPFParagraph) null;
    }

    public XWPFParagraph GetParagraphArray(int pos)
    {
      return this.paragraphs[pos];
    }

    public List<XWPFParagraph> GetListParagraph()
    {
      return this.paragraphs;
    }

    public IList<XWPFPictureData> AllPictures
    {
      get
      {
        return (IList<XWPFPictureData>) this.pictures.AsReadOnly();
      }
    }

    public IList<XWPFPictureData> AllPackagePictures
    {
      get
      {
        return this.document.AllPackagePictures;
      }
    }

    public string AddPictureData(byte[] pictureData, int format)
    {
      XWPFPictureData packagePictureData = this.document.FindPackagePictureData(pictureData, format);
      POIXMLRelation descriptor = XWPFPictureData.RELATIONS[format];
      if (packagePictureData == null)
      {
        int nextPicNameNumber = this.document.GetNextPicNameNumber(format);
        XWPFPictureData relationship = (XWPFPictureData) this.CreateRelationship(descriptor, (POIXMLFactory) XWPFFactory.GetInstance(), nextPicNameNumber);
        PackagePart packagePart = relationship.GetPackagePart();
        Stream stream = (Stream) null;
        try
        {
          stream = packagePart.GetOutputStream();
          stream.Write(pictureData, 0, pictureData.Length);
        }
        catch (IOException ex)
        {
          throw new POIXMLException((Exception) ex);
        }
        finally
        {
          try
          {
            stream.Close();
          }
          catch (IOException ex)
          {
          }
        }
        this.document.RegisterPackagePictureData(relationship);
        this.pictures.Add(relationship);
        return this.GetRelationId((POIXMLDocumentPart) relationship);
      }
      if (this.GetRelations().Contains((POIXMLDocumentPart) packagePictureData))
        return this.GetRelationId((POIXMLDocumentPart) packagePictureData);
      PackagePart packagePart1 = packagePictureData.GetPackagePart();
      TargetMode targetMode = TargetMode.Internal;
      PackagePartName partName = packagePart1.PartName;
      string relation = descriptor.Relation;
      string id = this.GetPackagePart().AddRelationship(partName, targetMode, relation).Id;
      this.AddRelation(id, (POIXMLDocumentPart) packagePictureData);
      this.pictures.Add(packagePictureData);
      return id;
    }

    public string AddPictureData(Stream is1, int format)
    {
      return this.AddPictureData(IOUtils.ToByteArray(is1), format);
    }

    public XWPFPictureData GetPictureDataByID(string blipID)
    {
      POIXMLDocumentPart relationById = this.GetRelationById(blipID);
      if (relationById != null && relationById is XWPFPictureData)
        return (XWPFPictureData) relationById;
      return (XWPFPictureData) null;
    }

    public POIXMLDocumentPart GetOwner()
    {
      return (POIXMLDocumentPart) this;
    }

    public XWPFTable GetTableArray(int pos)
    {
      if (pos > 0 && pos < this.tables.Count)
        return this.tables[pos];
      return (XWPFTable) null;
    }

    public void insertTable(int pos, XWPFTable table)
    {
      this.bodyElements.Insert(pos, (IBodyElement) table);
      int num = 0;
      while (num < this.headerFooter.GetTblList().Count && this.headerFooter.GetTblArray(num) != table.GetCTTbl())
        ++num;
      this.tables.Insert(num, table);
    }

    public void ReadHdrFtr()
    {
      this.bodyElements = new List<IBodyElement>();
      this.paragraphs = new List<XWPFParagraph>();
      this.tables = new List<XWPFTable>();
      foreach (object obj in this.headerFooter.Items)
      {
        if (obj is CT_P)
        {
          XWPFParagraph xwpfParagraph = new XWPFParagraph((CT_P) obj, (IBody) this);
          this.paragraphs.Add(xwpfParagraph);
          this.bodyElements.Add((IBodyElement) xwpfParagraph);
        }
        if (obj is CT_Tbl)
        {
          XWPFTable xwpfTable = new XWPFTable((CT_Tbl) obj, (IBody) this);
          this.tables.Add(xwpfTable);
          this.bodyElements.Add((IBodyElement) xwpfTable);
        }
      }
    }

    public XWPFTableCell GetTableCell(CT_Tc cell)
    {
      throw new NotImplementedException();
    }

    public void SetXWPFDocument(XWPFDocument doc)
    {
      this.document = doc;
    }

    public XWPFDocument GetXWPFDocument()
    {
      if (this.document != null)
        return this.document;
      return (XWPFDocument) this.GetParent();
    }

    public POIXMLDocumentPart GetPart()
    {
      return (POIXMLDocumentPart) this;
    }

    public virtual BodyType GetPartType()
    {
      throw new NotImplementedException();
    }

    public XWPFParagraph insertNewParagraph(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    public XWPFTable insertNewTbl(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }
  }
}
