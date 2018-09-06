// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.Model.XWPFHeaderFooterPolicy
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Vml;
using NPOI.OpenXmlFormats.Vml.Office;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XWPF.Model
{
  public class XWPFHeaderFooterPolicy
  {
    public static ST_HdrFtr DEFAULT = ST_HdrFtr.DEFAULT;
    public static ST_HdrFtr EVEN = ST_HdrFtr.EVEN;
    public static ST_HdrFtr FIRST = ST_HdrFtr.FIRST;
    private XWPFDocument doc;
    private XWPFHeader firstPageHeader;
    private XWPFFooter firstPageFooter;
    private XWPFHeader evenPageHeader;
    private XWPFFooter evenPageFooter;
    private XWPFHeader defaultHeader;
    private XWPFFooter defaultFooter;

    public XWPFHeaderFooterPolicy(XWPFDocument doc)
      : this(doc, doc.Document.body.sectPr)
    {
    }

    public XWPFHeaderFooterPolicy(XWPFDocument doc, CT_SectPr sectPr)
    {
      this.doc = doc;
      for (int i = 0; i < sectPr.SizeOfHeaderReferenceArray(); ++i)
      {
        CT_HdrFtrRef headerReferenceArray = sectPr.GetHeaderReferenceArray(i);
        POIXMLDocumentPart relationById = doc.GetRelationById(headerReferenceArray.id);
        XWPFHeader hdr = (XWPFHeader) null;
        if (relationById != null && relationById is XWPFHeader)
          hdr = (XWPFHeader) relationById;
        ST_HdrFtr type = headerReferenceArray.type;
        this.assignHeader(hdr, type);
      }
      for (int i = 0; i < sectPr.SizeOfFooterReferenceArray(); ++i)
      {
        CT_HdrFtrRef footerReferenceArray = sectPr.GetFooterReferenceArray(i);
        POIXMLDocumentPart relationById = doc.GetRelationById(footerReferenceArray.id);
        XWPFFooter ftr = (XWPFFooter) null;
        if (relationById != null && relationById is XWPFFooter)
          ftr = (XWPFFooter) relationById;
        ST_HdrFtr type = footerReferenceArray.type;
        this.assignFooter(ftr, type);
      }
    }

    private void assignFooter(XWPFFooter ftr, ST_HdrFtr type)
    {
      if (type == ST_HdrFtr.FIRST)
        this.firstPageFooter = ftr;
      else if (type == ST_HdrFtr.EVEN)
        this.evenPageFooter = ftr;
      else
        this.defaultFooter = ftr;
    }

    private void assignHeader(XWPFHeader hdr, ST_HdrFtr type)
    {
      if (type == ST_HdrFtr.FIRST)
        this.firstPageHeader = hdr;
      else if (type == ST_HdrFtr.EVEN)
        this.evenPageHeader = hdr;
      else
        this.defaultHeader = hdr;
    }

    public XWPFHeader CreateHeader(ST_HdrFtr type)
    {
      return this.CreateHeader(type, (XWPFParagraph[]) null);
    }

    public XWPFHeader CreateHeader(ST_HdrFtr type, XWPFParagraph[] pars)
    {
      XWPFRelation header = XWPFRelation.HEADER;
      string pStyle = "Header";
      int relationIndex = this.GetRelationIndex(header);
      HdrDocument hdrDocument = new HdrDocument();
      XWPFHeader relationship = (XWPFHeader) this.doc.CreateRelationship((POIXMLRelation) header, (POIXMLFactory) XWPFFactory.GetInstance(), relationIndex);
      CT_HdrFtr headerFooter = this.buildHdr(type, pStyle, (XWPFHeaderFooter) relationship, pars);
      relationship.SetHeaderFooter(headerFooter);
      Stream outputStream = relationship.GetPackagePart().GetOutputStream();
      hdrDocument.SetHdr((CT_Hdr) headerFooter);
      this.assignHeader(relationship, type);
      hdrDocument.Save(outputStream, this.Commit((XWPFHeaderFooter) relationship));
      outputStream.Close();
      return relationship;
    }

    public XWPFFooter CreateFooter(ST_HdrFtr type)
    {
      return this.CreateFooter(type, (XWPFParagraph[]) null);
    }

    public XWPFFooter CreateFooter(ST_HdrFtr type, XWPFParagraph[] pars)
    {
      XWPFRelation footer = XWPFRelation.FOOTER;
      string pStyle = "Footer";
      int relationIndex = this.GetRelationIndex(footer);
      FtrDocument ftrDocument = new FtrDocument();
      XWPFFooter relationship = (XWPFFooter) this.doc.CreateRelationship((POIXMLRelation) footer, (POIXMLFactory) XWPFFactory.GetInstance(), relationIndex);
      CT_HdrFtr headerFooter = this.buildFtr(type, pStyle, (XWPFHeaderFooter) relationship, pars);
      relationship.SetHeaderFooter(headerFooter);
      Stream outputStream = relationship.GetPackagePart().GetOutputStream();
      ftrDocument.SetFtr((CT_Ftr) headerFooter);
      this.assignFooter(relationship, type);
      ftrDocument.Save(outputStream, this.Commit((XWPFHeaderFooter) relationship));
      outputStream.Close();
      return relationship;
    }

    private int GetRelationIndex(XWPFRelation relation)
    {
      List<POIXMLDocumentPart> relations = this.doc.GetRelations();
      int num = 1;
      IEnumerator<POIXMLDocumentPart> enumerator = (IEnumerator<POIXMLDocumentPart>) relations.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (enumerator.Current.GetPackageRelationship().RelationshipType.Equals(relation.Relation))
          ++num;
      }
      return num;
    }

    private CT_HdrFtr buildFtr(ST_HdrFtr type, string pStyle, XWPFHeaderFooter wrapper, XWPFParagraph[] pars)
    {
      CT_HdrFtr ctHdrFtr = this.buildHdrFtr(pStyle, pars, wrapper);
      this.SetFooterReference(type, wrapper);
      return ctHdrFtr;
    }

    private CT_HdrFtr buildHdr(ST_HdrFtr type, string pStyle, XWPFHeaderFooter wrapper, XWPFParagraph[] pars)
    {
      CT_HdrFtr ctHdrFtr = this.buildHdrFtr(pStyle, pars, wrapper);
      this.SetHeaderReference(type, wrapper);
      return ctHdrFtr;
    }

    private CT_HdrFtr buildHdrFtr(string pStyle, XWPFParagraph[] paragraphs)
    {
      CT_HdrFtr ctHdrFtr = new CT_HdrFtr();
      if (paragraphs != null)
      {
        for (int i = 0; i < paragraphs.Length; ++i)
        {
          ctHdrFtr.AddNewP();
          ctHdrFtr.SetPArray(i, paragraphs[i].GetCTP());
        }
      }
      else
      {
        CT_P ctP = ctHdrFtr.AddNewP();
        byte[] rsidR = this.doc.Document.body.GetPArray(0).rsidR;
        byte[] rsidRdefault = this.doc.Document.body.GetPArray(0).rsidRDefault;
        ctP.rsidR = rsidR;
        ctP.rsidRDefault = rsidRdefault;
        ctP.AddNewPPr().AddNewPStyle().val = pStyle;
      }
      return ctHdrFtr;
    }

    private CT_HdrFtr buildHdrFtr(string pStyle, XWPFParagraph[] paragraphs, XWPFHeaderFooter wrapper)
    {
      CT_HdrFtr hdrFtr = wrapper._getHdrFtr();
      if (paragraphs != null)
      {
        for (int i = 0; i < paragraphs.Length; ++i)
        {
          hdrFtr.AddNewP();
          hdrFtr.SetPArray(i, paragraphs[i].GetCTP());
        }
      }
      else
      {
        CT_P ctP = hdrFtr.AddNewP();
        byte[] rsidR = this.doc.Document.body.GetPArray(0).rsidR;
        byte[] rsidRdefault = this.doc.Document.body.GetPArray(0).rsidRDefault;
        ctP.rsidP = rsidR;
        ctP.rsidRDefault = rsidRdefault;
        ctP.AddNewPPr().AddNewPStyle().val = pStyle;
      }
      return hdrFtr;
    }

    private void SetFooterReference(ST_HdrFtr type, XWPFHeaderFooter wrapper)
    {
      CT_HdrFtrRef ctHdrFtrRef = this.doc.Document.body.sectPr.AddNewFooterReference();
      ctHdrFtrRef.type = type;
      ctHdrFtrRef.id = wrapper.GetPackageRelationship().Id;
    }

    private void SetHeaderReference(ST_HdrFtr type, XWPFHeaderFooter wrapper)
    {
      CT_HdrFtrRef ctHdrFtrRef = this.doc.Document.body.sectPr.AddNewHeaderReference();
      ctHdrFtrRef.type = type;
      ctHdrFtrRef.id = wrapper.GetPackageRelationship().Id;
    }

    private XmlSerializerNamespaces Commit(XWPFHeaderFooter wrapper)
    {
      return new XmlSerializerNamespaces(new XmlQualifiedName[8]{ new XmlQualifiedName("ve", "http://schemas.openxmlformats.org/markup-compatibility/2006"), new XmlQualifiedName("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"), new XmlQualifiedName("m", "http://schemas.openxmlformats.org/officeDocument/2006/math"), new XmlQualifiedName("v", "urn:schemas-microsoft-com:vml"), new XmlQualifiedName("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"), new XmlQualifiedName("w10", "urn:schemas-microsoft-com:office:word"), new XmlQualifiedName("wne", "http://schemas.microsoft.com/office/word/2006/wordml"), new XmlQualifiedName("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") });
    }

    public XWPFHeader GetFirstPageHeader()
    {
      return this.firstPageHeader;
    }

    public XWPFFooter GetFirstPageFooter()
    {
      return this.firstPageFooter;
    }

    public XWPFHeader GetOddPageHeader()
    {
      return this.defaultHeader;
    }

    public XWPFFooter GetOddPageFooter()
    {
      return this.defaultFooter;
    }

    public XWPFHeader GetEvenPageHeader()
    {
      return this.evenPageHeader;
    }

    public XWPFFooter GetEvenPageFooter()
    {
      return this.evenPageFooter;
    }

    public XWPFHeader GetDefaultHeader()
    {
      return this.defaultHeader;
    }

    public XWPFFooter GetDefaultFooter()
    {
      return this.defaultFooter;
    }

    public XWPFHeader GetHeader(int pageNumber)
    {
      if (pageNumber == 1 && this.firstPageHeader != null)
        return this.firstPageHeader;
      if (pageNumber % 2 == 0 && this.evenPageHeader != null)
        return this.evenPageHeader;
      return this.defaultHeader;
    }

    public XWPFFooter GetFooter(int pageNumber)
    {
      if (pageNumber == 1 && this.firstPageFooter != null)
        return this.firstPageFooter;
      if (pageNumber % 2 == 0 && this.evenPageFooter != null)
        return this.evenPageFooter;
      return this.defaultFooter;
    }

    public void CreateWatermark(string text)
    {
      XWPFParagraph[] pars = new XWPFParagraph[1];
      try
      {
        pars[0] = this.GetWatermarkParagraph(text, 1);
        this.CreateHeader(XWPFHeaderFooterPolicy.DEFAULT, pars);
        pars[0] = this.GetWatermarkParagraph(text, 2);
        this.CreateHeader(XWPFHeaderFooterPolicy.FIRST, pars);
        pars[0] = this.GetWatermarkParagraph(text, 3);
        this.CreateHeader(XWPFHeaderFooterPolicy.EVEN, pars);
      }
      catch (IOException ex)
      {
        Console.Write(ex.StackTrace);
      }
    }

    private XWPFParagraph GetWatermarkParagraph(string text, int idx)
    {
      CT_P prgrph = new CT_P();
      byte[] rsidR = this.doc.Document.body.GetPArray(0).rsidR;
      byte[] rsidRdefault = this.doc.Document.body.GetPArray(0).rsidRDefault;
      prgrph.rsidP = rsidR;
      prgrph.rsidRDefault = rsidRdefault;
      prgrph.AddNewPPr().AddNewPStyle().val = "Header";
      NPOI.OpenXmlFormats.Wordprocessing.CT_R ctR = prgrph.AddNewR();
      ctR.AddNewRPr().AddNewNoProof();
      CT_Picture ctPicture = ctR.AddNewPict();
      CT_Group ctGroup = new CT_Group();
      CT_Shapetype ctShapetype = ctGroup.AddNewShapetype();
      ctShapetype.id = "_x0000_t136";
      ctShapetype.coordsize = "1600,21600";
      ctShapetype.spt = 136f;
      ctShapetype.adj = "10800";
      ctShapetype.path2 = "m@7,0l@8,0m@5,21600l@6,21600e";
      CT_Formulas ctFormulas = ctShapetype.AddNewFormulas();
      ctFormulas.AddNewF().eqn = "sum #0 0 10800";
      ctFormulas.AddNewF().eqn = "prod #0 2 1";
      ctFormulas.AddNewF().eqn = "sum 21600 0 @1";
      ctFormulas.AddNewF().eqn = "sum 0 0 @2";
      ctFormulas.AddNewF().eqn = "sum 21600 0 @3";
      ctFormulas.AddNewF().eqn = "if @0 @3 0";
      ctFormulas.AddNewF().eqn = "if @0 21600 @1";
      ctFormulas.AddNewF().eqn = "if @0 0 @2";
      ctFormulas.AddNewF().eqn = "if @0 @4 21600";
      ctFormulas.AddNewF().eqn = "mid @5 @6";
      ctFormulas.AddNewF().eqn = "mid @8 @5";
      ctFormulas.AddNewF().eqn = "mid @7 @8";
      ctFormulas.AddNewF().eqn = "mid @6 @7";
      ctFormulas.AddNewF().eqn = "sum @6 0 @5";
      CT_Path ctPath = ctShapetype.AddNewPath();
      ctPath.textpathok = ST_TrueFalse.t;
      ctPath.connecttype = ST_ConnectType.custom;
      ctPath.connectlocs = "@9,0;@10,10800;@11,21600;@12,10800";
      ctPath.connectangles = "270,180,90,0";
      CT_TextPath ctTextPath1 = ctShapetype.AddNewTextpath();
      ctTextPath1.on = ST_TrueFalse.t;
      ctTextPath1.fitshape = ST_TrueFalse.t;
      CT_H ctH = ctShapetype.AddNewHandles().AddNewH();
      ctH.position = "#0,bottomRight";
      ctH.xrange = "6629,14971";
      ctShapetype.AddNewLock().ext = ST_Ext.edit;
      CT_Shape ctShape = ctGroup.AddNewShape();
      ctShape.id = "PowerPlusWaterMarkObject" + (object) idx;
      ctShape.spid = "_x0000_s102" + (object) (4 + idx);
      ctShape.type = "#_x0000_t136";
      ctShape.style = "position:absolute;margin-left:0;margin-top:0;width:415pt;height:207.5pt;z-index:-251654144;mso-wrap-edited:f;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin";
      ctShape.wrapcoords = "616 5068 390 16297 39 16921 -39 17155 7265 17545 7186 17467 -39 17467 18904 17467 10507 17467 8710 17545 18904 17077 18787 16843 18358 16297 18279 12554 19178 12476 20701 11774 20779 11228 21131 10059 21248 8811 21248 7563 20975 6316 20935 5380 19490 5146 14022 5068 2616 5068";
      ctShape.fillcolor = "black";
      ctShape.stroked = ST_TrueFalse.@false;
      CT_TextPath ctTextPath2 = ctShape.AddNewTextpath();
      ctTextPath2.style = "font-family:&quot;Cambria&quot;;font-size:1pt";
      ctTextPath2.@string = text;
      ctPicture.Set((object) ctGroup);
      return new XWPFParagraph(prgrph, (IBody) this.doc);
    }
  }
}
