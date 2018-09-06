// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFFootnotes
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XWPF.UserModel
{
  public class XWPFFootnotes : POIXMLDocumentPart
  {
    private List<XWPFFootnote> listFootnote = new List<XWPFFootnote>();
    private CT_Footnotes ctFootnotes;
    protected XWPFDocument document;

    public XWPFFootnotes(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
    }

    public XWPFFootnotes()
    {
    }

    internal override void OnDocumentRead()
    {
      try
      {
        this.ctFootnotes = FootnotesDocument.Parse(this.GetPackagePart().GetInputStream()).Footnotes;
      }
      catch (XmlException ex)
      {
        throw new POIXMLException();
      }
      foreach (CT_FtnEdn footnote in this.ctFootnotes.FootnoteList)
        this.listFootnote.Add(new XWPFFootnote(footnote, this));
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      XmlSerializerNamespaces serializerNamespaces = new XmlSerializerNamespaces(new XmlQualifiedName[2]{ new XmlQualifiedName("w", "http://schemas.Openxmlformats.org/wordProcessingml/2006/main"), new XmlQualifiedName("r", "http://schemas.Openxmlformats.org/officeDocument/2006/relationships") });
      new FootnotesDocument(this.ctFootnotes).Save(outputStream, (XmlSerializerNamespaces) null);
      outputStream.Close();
    }

    public List<XWPFFootnote> GetFootnotesList()
    {
      return this.listFootnote;
    }

    public XWPFFootnote GetFootnoteById(int id)
    {
      foreach (XWPFFootnote xwpfFootnote in this.listFootnote)
      {
        if (xwpfFootnote.GetCTFtnEdn().id == id.ToString())
          return xwpfFootnote;
      }
      return (XWPFFootnote) null;
    }

    public void SetFootnotes(CT_Footnotes footnotes)
    {
      this.ctFootnotes = footnotes;
    }

    public void AddFootnote(XWPFFootnote footnote)
    {
      this.listFootnote.Add(footnote);
      this.ctFootnotes.AddNewFootnote().Set(footnote.GetCTFtnEdn());
    }

    public XWPFFootnote AddFootnote(CT_FtnEdn note)
    {
      CT_FtnEdn note1 = this.ctFootnotes.AddNewFootnote();
      note1.Set(note);
      XWPFFootnote xwpfFootnote = new XWPFFootnote(note1, this);
      this.listFootnote.Add(xwpfFootnote);
      return xwpfFootnote;
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
  }
}
