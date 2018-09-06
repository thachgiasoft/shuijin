// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFConnector
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.Spreadsheet;

namespace NPOI.XSSF.UserModel
{
  public class XSSFConnector : XSSFShape
  {
    private static CT_Connector prototype;
    private CT_Connector ctShape;

    public XSSFConnector(XSSFDrawing drawing, CT_Connector ctShape)
    {
      this.drawing = drawing;
      this.ctShape = ctShape;
    }

    public static CT_Connector Prototype()
    {
      if (XSSFConnector.prototype == null)
      {
        CT_Connector ctConnector = new CT_Connector();
        CT_ConnectorNonVisual connectorNonVisual = ctConnector.AddNewNvCxnSpPr();
        CT_NonVisualDrawingProps visualDrawingProps = connectorNonVisual.AddNewCNvPr();
        visualDrawingProps.id = 1U;
        visualDrawingProps.name = "Shape 1";
        connectorNonVisual.AddNewCNvCxnSpPr();
        CT_ShapeProperties ctShapeProperties = ctConnector.AddNewSpPr();
        CT_Transform2D ctTransform2D = ctShapeProperties.AddNewXfrm();
        CT_PositiveSize2D ctPositiveSize2D = ctTransform2D.AddNewExt();
        ctPositiveSize2D.cx = 0L;
        ctPositiveSize2D.cy = 0L;
        CT_Point2D ctPoint2D = ctTransform2D.AddNewOff();
        ctPoint2D.x = 0L;
        ctPoint2D.y = 0L;
        CT_PresetGeometry2D presetGeometry2D = ctShapeProperties.AddNewPrstGeom();
        presetGeometry2D.prst = ST_ShapeType.line;
        presetGeometry2D.AddNewAvLst();
        CT_ShapeStyle ctShapeStyle = ctConnector.AddNewStyle();
        ctShapeStyle.AddNewLnRef().AddNewSchemeClr().val = ST_SchemeColorVal.accent1;
        ctShapeStyle.lnRef.idx = 1U;
        CT_StyleMatrixReference styleMatrixReference1 = ctShapeStyle.AddNewFillRef();
        styleMatrixReference1.idx = 0U;
        styleMatrixReference1.AddNewSchemeClr().val = ST_SchemeColorVal.accent1;
        CT_StyleMatrixReference styleMatrixReference2 = ctShapeStyle.AddNewEffectRef();
        styleMatrixReference2.idx = 0U;
        styleMatrixReference2.AddNewSchemeClr().val = ST_SchemeColorVal.accent1;
        CT_FontReference ctFontReference = ctShapeStyle.AddNewFontRef();
        ctFontReference.idx = ST_FontCollectionIndex.minor;
        ctFontReference.AddNewSchemeClr().val = ST_SchemeColorVal.tx1;
        XSSFConnector.prototype = ctConnector;
      }
      return XSSFConnector.prototype;
    }

    public CT_Connector GetCTConnector()
    {
      return this.ctShape;
    }

    public int GetShapeType()
    {
      return (int) this.ctShape.spPr.prstGeom.prst;
    }

    public void SetShapeType(int type)
    {
      this.ctShape.spPr.prstGeom.prst = (ST_ShapeType) type;
    }

    protected internal override CT_ShapeProperties GetShapeProperties()
    {
      return this.ctShape.spPr;
    }
  }
}
