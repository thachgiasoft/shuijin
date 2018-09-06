// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Charts.XSSFChartLegend
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.UserModel.Charts;
using System;

namespace NPOI.XSSF.UserModel.Charts
{
  public class XSSFChartLegend : IChartLegend, ManuallyPositionable
  {
    private CT_Legend legend;

    public XSSFChartLegend(XSSFChart chart)
    {
      CT_Chart ctChart = chart.GetCTChart();
      this.legend = ctChart.IsSetLegend() ? ctChart.legend : ctChart.AddNewLegend();
    }

    internal CT_Legend GetCTLegend()
    {
      return this.legend;
    }

    public LegendPosition Position
    {
      get
      {
        if (this.legend.IsSetLegendPos())
          return this.ToLegendPosition(this.legend.legendPos);
        return LegendPosition.RIGHT;
      }
      set
      {
        if (!this.legend.IsSetLegendPos())
          this.legend.AddNewLegendPos();
        this.legend.legendPos.val = this.FromLegendPosition(value);
        this.legend.legendPosSpecified = true;
      }
    }

    public IManualLayout GetManualLayout()
    {
      if (!this.legend.IsSetLayout())
        this.legend.AddNewLayout();
      return (IManualLayout) new XSSFManualLayout(this.legend.layout);
    }

    private ST_LegendPos FromLegendPosition(LegendPosition position)
    {
      switch (position)
      {
        case LegendPosition.BOTTOM:
          return ST_LegendPos.b;
        case LegendPosition.LEFT:
          return ST_LegendPos.l;
        case LegendPosition.RIGHT:
          return ST_LegendPos.r;
        case LegendPosition.TOP:
          return ST_LegendPos.t;
        case LegendPosition.TOP_RIGHT:
          return ST_LegendPos.tr;
        default:
          throw new ArgumentException();
      }
    }

    private LegendPosition ToLegendPosition(CT_LegendPos ctLegendPos)
    {
      switch (ctLegendPos.val)
      {
        case ST_LegendPos.b:
          return LegendPosition.BOTTOM;
        case ST_LegendPos.tr:
          return LegendPosition.TOP_RIGHT;
        case ST_LegendPos.l:
          return LegendPosition.LEFT;
        case ST_LegendPos.r:
          return LegendPosition.RIGHT;
        case ST_LegendPos.t:
          return LegendPosition.TOP;
        default:
          throw new ArgumentException();
      }
    }
  }
}
