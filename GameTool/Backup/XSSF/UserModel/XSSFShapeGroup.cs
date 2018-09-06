// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFShapeGroup
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.Spreadsheet;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFShapeGroup : XSSFShape
  {
    private static CT_GroupShape prototype;
    private CT_GroupShape ctGroup;

    public XSSFShapeGroup(XSSFDrawing drawing, CT_GroupShape ctGroup)
    {
      this.drawing = drawing;
      this.ctGroup = ctGroup;
    }

    internal static CT_GroupShape Prototype()
    {
      if (XSSFShapeGroup.prototype == null)
      {
        CT_GroupShape ctGroupShape = new CT_GroupShape();
        CT_GroupShapeNonVisual groupShapeNonVisual = ctGroupShape.AddNewNvGrpSpPr();
        CT_NonVisualDrawingProps visualDrawingProps = groupShapeNonVisual.AddNewCNvPr();
        visualDrawingProps.id = 0U;
        visualDrawingProps.name = "Group 0";
        groupShapeNonVisual.AddNewCNvGrpSpPr();
        CT_GroupTransform2D groupTransform2D = ctGroupShape.AddNewGrpSpPr().AddNewXfrm();
        CT_PositiveSize2D ctPositiveSize2D1 = groupTransform2D.AddNewExt();
        ctPositiveSize2D1.cx = 0L;
        ctPositiveSize2D1.cy = 0L;
        CT_Point2D ctPoint2D1 = groupTransform2D.AddNewOff();
        ctPoint2D1.x = 0L;
        ctPoint2D1.y = 0L;
        CT_PositiveSize2D ctPositiveSize2D2 = groupTransform2D.AddNewChExt();
        ctPositiveSize2D2.cx = 0L;
        ctPositiveSize2D2.cy = 0L;
        CT_Point2D ctPoint2D2 = groupTransform2D.AddNewChOff();
        ctPoint2D2.x = 0L;
        ctPoint2D2.y = 0L;
        XSSFShapeGroup.prototype = ctGroupShape;
      }
      return XSSFShapeGroup.prototype;
    }

    public XSSFTextBox CreateTextbox(XSSFChildAnchor anchor)
    {
      CT_Shape ctShape = this.ctGroup.AddNewSp();
      ctShape.Set(XSSFSimpleShape.Prototype());
      XSSFTextBox xssfTextBox = new XSSFTextBox(this.GetDrawing(), ctShape);
      xssfTextBox.parent = this;
      xssfTextBox.anchor = (XSSFAnchor) anchor;
      xssfTextBox.GetCTShape().spPr.xfrm = anchor.GetCTTransform2D();
      return xssfTextBox;
    }

    public XSSFSimpleShape CreateSimpleShape(XSSFChildAnchor anchor)
    {
      CT_Shape ctShape = this.ctGroup.AddNewSp();
      ctShape.Set(XSSFSimpleShape.Prototype());
      XSSFSimpleShape xssfSimpleShape = new XSSFSimpleShape(this.GetDrawing(), ctShape);
      xssfSimpleShape.parent = this;
      xssfSimpleShape.anchor = (XSSFAnchor) anchor;
      xssfSimpleShape.GetCTShape().spPr.xfrm = anchor.GetCTTransform2D();
      return xssfSimpleShape;
    }

    public XSSFConnector CreateConnector(XSSFChildAnchor anchor)
    {
      CT_Connector ctShape = this.ctGroup.AddNewCxnSp();
      ctShape.Set(XSSFConnector.Prototype());
      XSSFConnector xssfConnector = new XSSFConnector(this.GetDrawing(), ctShape);
      xssfConnector.parent = this;
      xssfConnector.anchor = (XSSFAnchor) anchor;
      xssfConnector.GetCTConnector().spPr.xfrm = anchor.GetCTTransform2D();
      return xssfConnector;
    }

    public XSSFPicture CreatePicture(XSSFClientAnchor anchor, int pictureIndex)
    {
      PackageRelationship rel = this.GetDrawing().AddPictureReference(pictureIndex);
      CT_Picture ctPicture = this.ctGroup.AddNewPic();
      ctPicture.Set(XSSFPicture.Prototype());
      XSSFPicture xssfPicture = new XSSFPicture(this.GetDrawing(), ctPicture);
      xssfPicture.parent = this;
      xssfPicture.anchor = (XSSFAnchor) anchor;
      xssfPicture.SetPictureReference(rel);
      return xssfPicture;
    }

    public CT_GroupShape GetCT_GroupShape()
    {
      return this.ctGroup;
    }

    public void SetCoordinates(int x1, int y1, int x2, int y2)
    {
      CT_GroupTransform2D xfrm = this.ctGroup.grpSpPr.xfrm;
      CT_Point2D off = xfrm.off;
      off.x = (long) x1;
      off.y = (long) y1;
      CT_PositiveSize2D ext = xfrm.ext;
      ext.cx = (long) x2;
      ext.cy = (long) y2;
      CT_Point2D chOff = xfrm.chOff;
      chOff.x = (long) x1;
      chOff.y = (long) y1;
      CT_PositiveSize2D chExt = xfrm.chExt;
      chExt.cx = (long) x2;
      chExt.cy = (long) y2;
    }

    protected internal override CT_ShapeProperties GetShapeProperties()
    {
      throw new InvalidOperationException("Not supported for shape group");
    }
  }
}
