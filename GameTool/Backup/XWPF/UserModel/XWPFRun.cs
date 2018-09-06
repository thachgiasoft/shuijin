// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFRun
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.Picture;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XWPF.UserModel
{
  public class XWPFRun
  {
    private CT_R run;
    private string pictureText;
    private XWPFParagraph paragraph;
    private List<XWPFPicture> pictures;

    public XWPFRun(CT_R r, XWPFParagraph p)
    {
      this.run = r;
      this.paragraph = p;
      IList<CT_Drawing> drawingList = r.GetDrawingList();
      foreach (CT_Drawing ctDrawing in (IEnumerable<CT_Drawing>) drawingList)
      {
        foreach (CT_Anchor anchor in ctDrawing.GetAnchorList())
        {
          if (anchor.docPr != null)
            this.GetDocument().GetDrawingIdManager().Reserve((long) anchor.docPr.id);
        }
        foreach (CT_Inline inline in ctDrawing.GetInlineList())
        {
          if (inline.docPr != null)
            this.GetDocument().GetDrawingIdManager().Reserve((long) inline.docPr.id);
        }
      }
      StringBuilder stringBuilder = new StringBuilder();
      List<object> objectList = new List<object>();
      foreach (NPOI.OpenXmlFormats.Wordprocessing.CT_Picture pict in (IEnumerable<NPOI.OpenXmlFormats.Wordprocessing.CT_Picture>) r.GetPictList())
        objectList.Add((object) pict);
      foreach (CT_Drawing ctDrawing in (IEnumerable<CT_Drawing>) drawingList)
        objectList.Add((object) ctDrawing);
      foreach (object obj in objectList)
        ;
      this.pictureText = stringBuilder.ToString();
      this.pictures = new List<XWPFPicture>();
      foreach (object o in objectList)
      {
        foreach (NPOI.OpenXmlFormats.Dml.Picture.CT_Picture ctPicture in this.GetCTPictures(o))
          this.pictures.Add(new XWPFPicture(ctPicture, this));
      }
    }

    private List<NPOI.OpenXmlFormats.Dml.Picture.CT_Picture> GetCTPictures(object o)
    {
      List<NPOI.OpenXmlFormats.Dml.Picture.CT_Picture> pictures = new List<NPOI.OpenXmlFormats.Dml.Picture.CT_Picture>();
      if (o is CT_Drawing)
      {
        foreach (object obj in (o as CT_Drawing).Items)
        {
          if (obj is CT_Inline)
            this.GetPictures((obj as CT_Inline).graphic.graphicData, pictures);
        }
      }
      else if (o is CT_GraphicalObjectData)
        this.GetPictures(o as CT_GraphicalObjectData, pictures);
      return pictures;
    }

    private void GetPictures(CT_GraphicalObjectData god, List<NPOI.OpenXmlFormats.Dml.Picture.CT_Picture> pictures)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (NPOI.OpenXmlFormats.Dml.Picture.CT_Picture));
      foreach (XmlNode xmlNode in god.Any)
      {
        StringReader stringReader = new StringReader(xmlNode.OuterXml);
        NPOI.OpenXmlFormats.Dml.Picture.CT_Picture ctPicture = xmlSerializer.Deserialize(XmlReader.Create((TextReader) stringReader)) as NPOI.OpenXmlFormats.Dml.Picture.CT_Picture;
        pictures.Add(ctPicture);
      }
    }

    public CT_R GetCTR()
    {
      return this.run;
    }

    public XWPFParagraph GetParagraph()
    {
      return this.paragraph;
    }

    public XWPFDocument GetDocument()
    {
      if (this.paragraph != null)
        return this.paragraph.GetDocument();
      return (XWPFDocument) null;
    }

    private bool IsCTOnOff(CT_OnOff onoff)
    {
      return !onoff.IsSetVal() || onoff.val == ST_OnOff.on || onoff.val == ST_OnOff.True;
    }

    public bool IsBold()
    {
      CT_RPr rPr = this.run.rPr;
      if (rPr == null || !rPr.IsSetB())
        return false;
      return this.IsCTOnOff(rPr.b);
    }

    public void SetBold(bool value)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.IsSetB() ? ctRpr.b : ctRpr.AddNewB()).val = value ? ST_OnOff.True : ST_OnOff.False;
    }

    public string GetColor()
    {
      string str = (string) null;
      if (this.run.IsSetRPr())
      {
        CT_RPr rPr = this.run.rPr;
        if (rPr.IsSetColor())
          str = rPr.color.val;
      }
      return str;
    }

    public void SetColor(string rgbStr)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.IsSetColor() ? ctRpr.color : ctRpr.AddNewColor()).val = rgbStr;
    }

    public string GetText(int pos)
    {
      if (this.run.SizeOfTArray() != 0)
        return this.run.GetTArray(pos).Value;
      return (string) null;
    }

    public string GetPictureText()
    {
      return this.pictureText;
    }

    public void SetText(string value)
    {
      this.SetText(value, this.run.GetTList().Count);
    }

    public void SetText(string value, int pos)
    {
      int num = this.run.SizeOfTArray();
      if (pos > num)
        throw new IndexOutOfRangeException("Value too large for the parameter position in XWPFrun.Text=(String value,int pos)");
      CT_Text xs = pos >= num || pos < 0 ? this.run.AddNewT() : this.run.GetTArray(pos);
      xs.Value = value;
      XWPFRun.preserveSpaces(xs);
    }

    public bool IsItalic()
    {
      CT_RPr rPr = this.run.rPr;
      if (rPr == null || !rPr.IsSetI())
        return false;
      return this.IsCTOnOff(rPr.i);
    }

    public void SetItalic(bool value)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.IsSetI() ? ctRpr.i : ctRpr.AddNewI()).val = value ? ST_OnOff.True : ST_OnOff.False;
    }

    public UnderlinePatterns GetUnderline()
    {
      CT_RPr rPr = this.run.rPr;
      if (rPr == null || !rPr.IsSetU())
        return UnderlinePatterns.None;
      return EnumConverter.ValueOf<UnderlinePatterns, ST_Underline>(rPr.u.val);
    }

    public void SetUnderline(UnderlinePatterns value)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.u == null ? ctRpr.AddNewU() : ctRpr.u).val = EnumConverter.ValueOf<ST_Underline, UnderlinePatterns>(value);
    }

    public bool IsStrike()
    {
      CT_RPr rPr = this.run.rPr;
      if (rPr == null || !rPr.IsSetStrike())
        return false;
      return this.IsCTOnOff(rPr.strike);
    }

    public void SetStrike(bool value)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.IsSetStrike() ? ctRpr.strike : ctRpr.AddNewStrike()).val = value ? ST_OnOff.True : ST_OnOff.False;
    }

    public VerticalAlign GetSubscript()
    {
      CT_RPr rPr = this.run.rPr;
      if (rPr == null || !rPr.IsSetVertAlign())
        return VerticalAlign.BASELINE;
      return EnumConverter.ValueOf<VerticalAlign, ST_VerticalAlignRun>(rPr.vertAlign.val);
    }

    public void SetSubscript(VerticalAlign valign)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.IsSetVertAlign() ? ctRpr.vertAlign : ctRpr.AddNewVertAlign()).val = EnumConverter.ValueOf<ST_VerticalAlignRun, VerticalAlign>(valign);
    }

    public string GetFontFamily()
    {
      CT_RPr rPr = this.run.rPr;
      if (rPr == null || !rPr.IsSetRFonts())
        return (string) null;
      return rPr.rFonts.ascii;
    }

    public void SetFontFamily(string fontFamily)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.IsSetRFonts() ? ctRpr.rFonts : ctRpr.AddNewRFonts()).ascii = fontFamily;
    }

    public int GetFontSize()
    {
      CT_RPr rPr = this.run.rPr;
      if (rPr == null || !rPr.IsSetSz())
        return -1;
      return (int) rPr.sz.val / 2;
    }

    public void SetFontSize(int size)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.IsSetSz() ? ctRpr.sz : ctRpr.AddNewSz()).val = (ulong) size * 2UL;
    }

    public int GetTextPosition()
    {
      CT_RPr rPr = this.run.rPr;
      if (rPr == null || !rPr.IsSetPosition())
        return -1;
      return int.Parse(rPr.position.val);
    }

    public void SetTextPosition(int val)
    {
      CT_RPr ctRpr = this.run.IsSetRPr() ? this.run.rPr : this.run.AddNewRPr();
      (ctRpr.IsSetPosition() ? ctRpr.position : ctRpr.AddNewPosition()).val = val.ToString();
    }

    public void RemoveBreak()
    {
    }

    public void AddBreak()
    {
      this.run.AddNewBr();
    }

    public void AddBreak(BreakType type)
    {
      this.run.AddNewBr().type = EnumConverter.ValueOf<ST_BrType, BreakType>(type);
    }

    public void AddBreak(BreakClear Clear)
    {
      CT_Br ctBr = this.run.AddNewBr();
      ctBr.type = EnumConverter.ValueOf<ST_BrType, BreakType>(BreakType.TEXTWRAPPING);
      ctBr.clear = EnumConverter.ValueOf<ST_BrClear, BreakClear>(Clear);
    }

    public void AddCarriageReturn()
    {
      this.run.AddNewCr();
    }

    public void RemoveCarriageReturn()
    {
    }

    public XWPFPicture AddPicture(Stream pictureData, int pictureType, string filename, int width, int height)
    {
      XWPFDocument document = this.paragraph.GetDocument();
      string id = document.AddPictureData(pictureData, pictureType);
      XWPFPictureData relationById = (XWPFPictureData) document.GetRelationById(id);
      try
      {
        CT_Inline ctInline = this.run.AddNewDrawing().AddNewInline();
        XmlElement element = new XmlDocument().CreateElement("pic", "pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");
        ctInline.graphic = new CT_GraphicalObject();
        ctInline.graphic.graphicData = new CT_GraphicalObjectData();
        ctInline.graphic.graphicData.AddPicElement((XmlElement) element.Clone());
        ctInline.distT = 0U;
        ctInline.distR = 0U;
        ctInline.distB = 0U;
        ctInline.distL = 0U;
        CT_NonVisualDrawingProps visualDrawingProps1 = ctInline.AddNewDocPr();
        long num = this.GetParagraph().GetDocument().GetDrawingIdManager().ReserveNew();
        visualDrawingProps1.id = (uint) num;
        visualDrawingProps1.name = "Drawing " + (object) num;
        visualDrawingProps1.descr = filename;
        CT_PositiveSize2D ctPositiveSize2D1 = ctInline.AddNewExtent();
        ctPositiveSize2D1.cx = (long) width;
        ctPositiveSize2D1.cy = (long) height;
        NPOI.OpenXmlFormats.Dml.Picture.CT_Picture ctPicture = this.GetCTPictures((object) ctInline.graphic.graphicData)[0];
        CT_PictureNonVisual pictureNonVisual = ctPicture.AddNewNvPicPr();
        CT_NonVisualDrawingProps visualDrawingProps2 = pictureNonVisual.AddNewCNvPr();
        visualDrawingProps2.id = 0U;
        visualDrawingProps2.name = "Picture " + (object) num;
        visualDrawingProps2.descr = filename;
        pictureNonVisual.AddNewCNvPicPr().AddNewPicLocks().noChangeAspect = true;
        CT_BlipFillProperties blipFillProperties = ctPicture.AddNewBlipFill();
        blipFillProperties.AddNewBlip().embed = relationById.GetPackageRelationship().Id;
        blipFillProperties.AddNewStretch().AddNewFillRect();
        CT_ShapeProperties ctShapeProperties = ctPicture.AddNewSpPr();
        CT_Transform2D ctTransform2D = ctShapeProperties.AddNewXfrm();
        CT_Point2D ctPoint2D = ctTransform2D.AddNewOff();
        ctPoint2D.x = 0L;
        ctPoint2D.y = 0L;
        CT_PositiveSize2D ctPositiveSize2D2 = ctTransform2D.AddNewExt();
        ctPositiveSize2D2.cx = (long) width;
        ctPositiveSize2D2.cy = (long) height;
        CT_PresetGeometry2D presetGeometry2D = ctShapeProperties.AddNewPrstGeom();
        presetGeometry2D.prst = ST_ShapeType.rect;
        presetGeometry2D.AddNewAvLst();
        XWPFPicture xwpfPicture = new XWPFPicture(ctPicture, this);
        this.pictures.Add(xwpfPicture);
        return xwpfPicture;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("", ex);
      }
    }

    public List<XWPFPicture> GetEmbeddedPictures()
    {
      return this.pictures;
    }

    private static void preserveSpaces(CT_Text xs)
    {
      string str = xs.Value;
      if (str == null || !str.StartsWith(" ") && !str.EndsWith(" "))
        return;
      xs.space = "preserve";
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      int index = 0;
      foreach (object obj in this.run.Items)
      {
        if (obj is CT_Text && this.run.ItemsElementName[index] != RunItemsChoiceType.instrText)
          stringBuilder.Append(((CT_Text) obj).Value);
        if (obj is CT_PTab)
          stringBuilder.Append("\t");
        if (obj is CT_Br)
          stringBuilder.Append("\n");
        if (obj is CT_Empty)
        {
          if (this.run.ItemsElementName[index] == RunItemsChoiceType.tab)
            stringBuilder.Append("\t");
          if (this.run.ItemsElementName[index] == RunItemsChoiceType.br)
            stringBuilder.Append("\n");
          if (this.run.ItemsElementName[index] == RunItemsChoiceType.cr)
            stringBuilder.Append("\n");
        }
        ++index;
      }
      if (this.pictureText != null && this.pictureText.Length > 0)
        stringBuilder.Append("\n").Append(this.pictureText);
      return stringBuilder.ToString();
    }
  }
}
