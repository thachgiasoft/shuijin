// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFFootnote
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace NPOI.XWPF.UserModel
{
  public class XWPFFootnote : IEnumerator<XWPFParagraph>, IDisposable, IEnumerator, IBody
  {
    private List<XWPFParagraph> paragraphs = new List<XWPFParagraph>();
    private List<XWPFTable> tables = new List<XWPFTable>();
    private List<XWPFPictureData> pictures = new List<XWPFPictureData>();
    private List<IBodyElement> bodyElements = new List<IBodyElement>();
    private CT_FtnEdn ctFtnEdn;
    private XWPFFootnotes footnotes;

    public XWPFFootnote(CT_FtnEdn note, XWPFFootnotes xFootnotes)
    {
      this.footnotes = xFootnotes;
      this.ctFtnEdn = note;
      foreach (CT_P prgrph in (IEnumerable<CT_P>) this.ctFtnEdn.GetPList())
        this.paragraphs.Add(new XWPFParagraph(prgrph, (IBody) this));
    }

    public XWPFFootnote(XWPFDocument document, CT_FtnEdn body)
    {
      if (body == null)
        return;
      foreach (CT_P prgrph in (IEnumerable<CT_P>) body.GetPList())
        this.paragraphs.Add(new XWPFParagraph(prgrph, (IBody) document));
    }

    public IList<XWPFParagraph> Paragraphs
    {
      get
      {
        return (IList<XWPFParagraph>) this.paragraphs;
      }
    }

    public IEnumerator<XWPFParagraph> GetEnumerator()
    {
      return (IEnumerator<XWPFParagraph>) this.paragraphs.GetEnumerator();
    }

    public IList<XWPFTable> GetTables()
    {
      return (IList<XWPFTable>) this.tables;
    }

    public IList<XWPFPictureData> Pictures
    {
      get
      {
        return (IList<XWPFPictureData>) this.pictures;
      }
    }

    public IList<IBodyElement> BodyElements
    {
      get
      {
        return (IList<IBodyElement>) this.bodyElements;
      }
    }

    public CT_FtnEdn GetCTFtnEdn()
    {
      return this.ctFtnEdn;
    }

    public void SetCTFtnEdn(CT_FtnEdn footnote)
    {
      this.ctFtnEdn = footnote;
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
      while (num < this.ctFtnEdn.GetTblList().Count && this.ctFtnEdn.GetTblArray(num) != table.GetCTTbl())
        ++num;
      this.tables.Insert(num, table);
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

    public XWPFTableCell GetTableCell(CT_Tc cell)
    {
      throw new NotImplementedException();
    }

    private bool IsCursorInFtn(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    public POIXMLDocumentPart GetOwner()
    {
      return (POIXMLDocumentPart) this.footnotes;
    }

    public XWPFTable insertNewTbl(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    public XWPFParagraph insertNewParagraph(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    public XWPFTable AddNewTbl(CT_Tbl table)
    {
      CT_Tbl table1 = this.ctFtnEdn.AddNewTbl();
      table1.Set(table);
      XWPFTable xwpfTable = new XWPFTable(table1, (IBody) this);
      this.tables.Add(xwpfTable);
      return xwpfTable;
    }

    public XWPFParagraph AddNewParagraph(CT_P paragraph)
    {
      XWPFParagraph xwpfParagraph = new XWPFParagraph(this.ctFtnEdn.AddNewP(paragraph), (IBody) this);
      this.paragraphs.Add(xwpfParagraph);
      return xwpfParagraph;
    }

    public XWPFDocument GetXWPFDocument()
    {
      return this.footnotes.GetXWPFDocument();
    }

    public POIXMLDocumentPart GetPart()
    {
      return (POIXMLDocumentPart) this.footnotes;
    }

    public BodyType GetPartType()
    {
      return BodyType.FOOTNOTE;
    }

    public XWPFParagraph Current
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    object IEnumerator.Current
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool MoveNext()
    {
      throw new NotImplementedException();
    }

    public void Reset()
    {
      throw new NotImplementedException();
    }
  }
}
