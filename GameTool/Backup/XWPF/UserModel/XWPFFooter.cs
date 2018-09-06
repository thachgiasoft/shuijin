// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFFooter
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XWPF.UserModel
{
  public class XWPFFooter : XWPFHeaderFooter
  {
    public XWPFFooter()
    {
      this.headerFooter = (CT_HdrFtr) new CT_Ftr();
      this.ReadHdrFtr();
    }

    public XWPFFooter(XWPFDocument doc, CT_HdrFtr hdrFtr)
      : base(doc, hdrFtr)
    {
      foreach (object obj in hdrFtr.Items)
      {
        if (obj is CT_P)
          this.paragraphs.Add(new XWPFParagraph((CT_P) obj, (IBody) this));
        if (obj is CT_Tbl)
          this.tables.Add(new XWPFTable((CT_Tbl) obj, (IBody) this));
      }
    }

    public XWPFFooter(POIXMLDocumentPart parent, PackagePart part, PackageRelationship rel)
      : base(parent, part, rel)
    {
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      FtrDocument ftrDocument = new FtrDocument((CT_Ftr) this.headerFooter);
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[8]{ new XmlQualifiedName("ve", "http://schemas.openxmlformats.org/markup-compatibility/2006"), new XmlQualifiedName("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"), new XmlQualifiedName("m", "http://schemas.openxmlformats.org/officeDocument/2006/math"), new XmlQualifiedName("v", "urn:schemas-microsoft-com:vml"), new XmlQualifiedName("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"), new XmlQualifiedName("w10", "urn:schemas-microsoft-com:office:word"), new XmlQualifiedName("wne", "http://schemas.microsoft.com/office/word/2006/wordml"), new XmlQualifiedName("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") });
      ftrDocument.Save(outputStream, namespaces);
      outputStream.Close();
    }

    internal override void OnDocumentRead()
    {
      base.OnDocumentRead();
      try
      {
        this.headerFooter = (CT_HdrFtr) FtrDocument.Parse(this.GetPackagePart().GetInputStream()).Ftr;
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
      catch (Exception ex)
      {
        throw new POIXMLException(ex);
      }
    }

    public override BodyType GetPartType()
    {
      return BodyType.FOOTER;
    }
  }
}
