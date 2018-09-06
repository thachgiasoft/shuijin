// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Charts.XSSFValueAxis
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.UserModel.Charts;
using System;

namespace NPOI.XSSF.UserModel.Charts
{
  public class XSSFValueAxis : XSSFChartAxis, IValueAxis, IChartAxis
  {
    private CT_ValAx ctValAx;

    public XSSFValueAxis(XSSFChart chart, long id, AxisPosition pos)
      : base(chart)
    {
      this.CreateAxis(id, pos);
    }

    public XSSFValueAxis(XSSFChart chart, CT_ValAx ctValAx)
      : base(chart)
    {
      this.ctValAx = ctValAx;
    }

    public override long GetId()
    {
      return (long) this.ctValAx.axId.val;
    }

    public void SetCrossBetween(AxisCrossBetween crossBetween)
    {
      this.ctValAx.crossBetween.val = XSSFValueAxis.fromCrossBetween(crossBetween);
    }

    public AxisCrossBetween GetCrossBetween()
    {
      return XSSFValueAxis.ToCrossBetween(this.ctValAx.crossBetween.val);
    }

    protected override CT_AxPos GetCTAxPos()
    {
      return this.ctValAx.axPos;
    }

    protected override CT_NumFmt GetCTNumFmt()
    {
      if (this.ctValAx.IsSetNumFmt())
        return this.ctValAx.numFmt;
      return this.ctValAx.AddNewNumFmt();
    }

    protected override CT_Scaling GetCTScaling()
    {
      return this.ctValAx.scaling;
    }

    protected override CT_Crosses GetCTCrosses()
    {
      return this.ctValAx.crosses;
    }

    public override void CrossAxis(IChartAxis axis)
    {
      this.ctValAx.crossAx.val = (uint) axis.GetId();
    }

    private void CreateAxis(long id, AxisPosition pos)
    {
      this.ctValAx = this.chart.GetCTChart().plotArea.AddNewValAx();
      this.ctValAx.AddNewAxId().val = (uint) id;
      this.ctValAx.AddNewAxPos();
      this.ctValAx.AddNewScaling();
      this.ctValAx.AddNewCrossBetween();
      this.ctValAx.AddNewCrosses();
      this.ctValAx.AddNewCrossAx();
      this.ctValAx.AddNewTickLblPos().val = ST_TickLblPos.nextTo;
      this.SetPosition(pos);
      this.SetOrientation(AxisOrientation.MIN_MAX);
      this.SetCrossBetween(AxisCrossBetween.MIDPOINT_CATEGORY);
      this.SetCrosses(AxisCrosses.AUTO_ZERO);
    }

    private static ST_CrossBetween fromCrossBetween(AxisCrossBetween crossBetween)
    {
      switch (crossBetween)
      {
        case AxisCrossBetween.BETWEEN:
          return ST_CrossBetween.between;
        case AxisCrossBetween.MIDPOINT_CATEGORY:
          return ST_CrossBetween.midCat;
        default:
          throw new ArgumentException();
      }
    }

    private static AxisCrossBetween ToCrossBetween(ST_CrossBetween ctCrossBetween)
    {
      switch (ctCrossBetween)
      {
        case ST_CrossBetween.between:
          return AxisCrossBetween.BETWEEN;
        case ST_CrossBetween.midCat:
          return AxisCrossBetween.MIDPOINT_CATEGORY;
        default:
          throw new ArgumentException();
      }
    }
  }
}
