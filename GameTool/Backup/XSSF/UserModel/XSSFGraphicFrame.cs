// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFGraphicFrame
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.Spreadsheet;

namespace NPOI.XSSF.UserModel
{
  public class XSSFGraphicFrame : XSSFShape
  {
    private static CT_GraphicalObjectFrame prototype;
    private CT_GraphicalObjectFrame graphicFrame;
    private new XSSFDrawing drawing;
    private XSSFClientAnchor anchor;

    public XSSFGraphicFrame(XSSFDrawing Drawing, CT_GraphicalObjectFrame ctGraphicFrame)
    {
      this.drawing = Drawing;
      this.graphicFrame = ctGraphicFrame;
    }

    internal CT_GraphicalObjectFrame GetCTGraphicalObjectFrame()
    {
      return this.graphicFrame;
    }

    public static CT_GraphicalObjectFrame Prototype()
    {
      if (XSSFGraphicFrame.prototype == null)
      {
        CT_GraphicalObjectFrame graphicalObjectFrame = new CT_GraphicalObjectFrame();
        CT_GraphicalObjectFrameNonVisual objectFrameNonVisual = graphicalObjectFrame.AddNewNvGraphicFramePr();
        CT_NonVisualDrawingProps visualDrawingProps = objectFrameNonVisual.AddNewCNvPr();
        visualDrawingProps.id = 0U;
        visualDrawingProps.name = "Diagramm 1";
        objectFrameNonVisual.AddNewCNvGraphicFramePr();
        CT_Transform2D ctTransform2D = graphicalObjectFrame.AddNewXfrm();
        CT_PositiveSize2D ctPositiveSize2D = ctTransform2D.AddNewExt();
        CT_Point2D ctPoint2D = ctTransform2D.AddNewOff();
        ctPositiveSize2D.cx = 0L;
        ctPositiveSize2D.cy = 0L;
        ctPoint2D.x = 0L;
        ctPoint2D.y = 0L;
        graphicalObjectFrame.AddNewGraphic();
        XSSFGraphicFrame.prototype = graphicalObjectFrame;
      }
      return XSSFGraphicFrame.prototype;
    }

    public void SetMacro(string macro)
    {
      this.graphicFrame.macro = macro;
    }

    public string Name
    {
      get
      {
        return this.GetNonVisualProperties().name;
      }
      set
      {
        this.GetNonVisualProperties().name = value;
      }
    }

    private CT_NonVisualDrawingProps GetNonVisualProperties()
    {
      return this.graphicFrame.nvGraphicFramePr.cNvPr;
    }

    public XSSFClientAnchor Anchor
    {
      get
      {
        return this.anchor;
      }
      set
      {
        this.anchor = value;
      }
    }

    internal void SetChart(XSSFChart chart, string relId)
    {
      this.AppendChartElement(this.graphicFrame.graphic.AddNewGraphicData(), relId);
      chart.SetGraphicFrame(this);
    }

    public long Id
    {
      get
      {
        return (long) this.graphicFrame.nvGraphicFramePr.cNvPr.id;
      }
      set
      {
        this.graphicFrame.nvGraphicFramePr.cNvPr.id = (uint) value;
      }
    }

    private void AppendChartElement(CT_GraphicalObjectData data, string id)
    {
      string namespaceUri = ST_RelationshipId.NamespaceURI;
    }

    protected internal override CT_ShapeProperties GetShapeProperties()
    {
      return (CT_ShapeProperties) null;
    }
  }
}
