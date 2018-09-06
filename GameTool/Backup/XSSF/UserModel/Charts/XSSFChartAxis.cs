// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Charts.XSSFChartAxis
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.UserModel.Charts;
using System;

namespace NPOI.XSSF.UserModel.Charts
{
  public abstract class XSSFChartAxis : IChartAxis
  {
    private static double MIN_LOG_BASE = 2.0;
    private static double MAX_LOG_BASE = 1000.0;
    protected XSSFChart chart;

    protected XSSFChartAxis(XSSFChart chart)
    {
      this.chart = chart;
    }

    public abstract long GetId();

    public abstract void CrossAxis(IChartAxis axis);

    public AxisPosition GetPosition()
    {
      return XSSFChartAxis.toAxisPosition(this.GetCTAxPos());
    }

    public void SetPosition(AxisPosition position)
    {
      this.GetCTAxPos().val = XSSFChartAxis.fromAxisPosition(position);
    }

    public void SetNumberFormat(string format)
    {
      this.GetCTNumFmt().formatCode = format;
      this.GetCTNumFmt().sourceLinked = true;
    }

    public string GetNumberFormat()
    {
      return this.GetCTNumFmt().formatCode;
    }

    public bool IsSetLogBase()
    {
      return this.GetCTScaling().IsSetLogBase();
    }

    public void SetLogBase(double logBase)
    {
      if (logBase < XSSFChartAxis.MIN_LOG_BASE || XSSFChartAxis.MAX_LOG_BASE < logBase)
        throw new ArgumentException("Axis log base must be between 2 and 1000 (inclusive), got: " + (object) logBase);
      CT_Scaling ctScaling = this.GetCTScaling();
      if (ctScaling.IsSetLogBase())
        ctScaling.logBase.val = logBase;
      else
        ctScaling.AddNewLogBase().val = logBase;
    }

    public double GetLogBase()
    {
      CT_LogBase logBase = this.GetCTScaling().logBase;
      if (logBase != null)
        return logBase.val;
      return 0.0;
    }

    public bool IsSetMinimum()
    {
      return this.GetCTScaling().IsSetMin();
    }

    public void SetMinimum(double min)
    {
      CT_Scaling ctScaling = this.GetCTScaling();
      if (ctScaling.IsSetMin())
        ctScaling.min.val = min;
      else
        ctScaling.AddNewMin().val = min;
    }

    public double GetMinimum()
    {
      CT_Scaling ctScaling = this.GetCTScaling();
      if (ctScaling.IsSetMin())
        return ctScaling.min.val;
      return 0.0;
    }

    public bool IsSetMaximum()
    {
      return this.GetCTScaling().IsSetMax();
    }

    public void SetMaximum(double max)
    {
      CT_Scaling ctScaling = this.GetCTScaling();
      if (ctScaling.IsSetMax())
        ctScaling.max.val = max;
      else
        ctScaling.AddNewMax().val = max;
    }

    public double GetMaximum()
    {
      CT_Scaling ctScaling = this.GetCTScaling();
      if (ctScaling.IsSetMax())
        return ctScaling.max.val;
      return 0.0;
    }

    public AxisOrientation GetOrientation()
    {
      return XSSFChartAxis.toAxisOrientation(this.GetCTScaling().orientation);
    }

    public void SetOrientation(AxisOrientation orientation)
    {
      CT_Scaling ctScaling = this.GetCTScaling();
      ST_Orientation stOrientation = XSSFChartAxis.fromAxisOrientation(orientation);
      if (ctScaling.IsSetOrientation())
        ctScaling.orientation.val = stOrientation;
      else
        this.GetCTScaling().AddNewOrientation().val = stOrientation;
    }

    public AxisCrosses GetCrosses()
    {
      return XSSFChartAxis.toAxisCrosses(this.GetCTCrosses());
    }

    public void SetCrosses(AxisCrosses crosses)
    {
      this.GetCTCrosses().val = XSSFChartAxis.fromAxisCrosses(crosses);
    }

    protected abstract CT_AxPos GetCTAxPos();

    protected abstract CT_NumFmt GetCTNumFmt();

    protected abstract CT_Scaling GetCTScaling();

    protected abstract CT_Crosses GetCTCrosses();

    private static ST_Orientation fromAxisOrientation(AxisOrientation orientation)
    {
      switch (orientation)
      {
        case AxisOrientation.MAX_MIN:
          return ST_Orientation.maxMin;
        case AxisOrientation.MIN_MAX:
          return ST_Orientation.minMax;
        default:
          throw new ArgumentException();
      }
    }

    private static AxisOrientation toAxisOrientation(CT_Orientation ctOrientation)
    {
      switch (ctOrientation.val)
      {
        case ST_Orientation.maxMin:
          return AxisOrientation.MAX_MIN;
        case ST_Orientation.minMax:
          return AxisOrientation.MIN_MAX;
        default:
          throw new ArgumentException();
      }
    }

    private static ST_Crosses fromAxisCrosses(AxisCrosses crosses)
    {
      switch (crosses)
      {
        case AxisCrosses.AUTO_ZERO:
          return ST_Crosses.autoZero;
        case AxisCrosses.MIN:
          return ST_Crosses.min;
        case AxisCrosses.MAX:
          return ST_Crosses.max;
        default:
          throw new ArgumentException();
      }
    }

    private static AxisCrosses toAxisCrosses(CT_Crosses ctCrosses)
    {
      switch (ctCrosses.val)
      {
        case ST_Crosses.autoZero:
          return AxisCrosses.AUTO_ZERO;
        case ST_Crosses.max:
          return AxisCrosses.MAX;
        case ST_Crosses.min:
          return AxisCrosses.MIN;
        default:
          throw new ArgumentException();
      }
    }

    private static ST_AxPos fromAxisPosition(AxisPosition position)
    {
      switch (position)
      {
        case AxisPosition.BOTTOM:
          return ST_AxPos.b;
        case AxisPosition.LEFT:
          return ST_AxPos.l;
        case AxisPosition.RIGHT:
          return ST_AxPos.r;
        case AxisPosition.TOP:
          return ST_AxPos.t;
        default:
          throw new ArgumentException();
      }
    }

    private static AxisPosition toAxisPosition(CT_AxPos ctAxPos)
    {
      switch (ctAxPos.val)
      {
        case ST_AxPos.b:
          return AxisPosition.BOTTOM;
        case ST_AxPos.l:
          return AxisPosition.LEFT;
        case ST_AxPos.r:
          return AxisPosition.RIGHT;
        case ST_AxPos.t:
          return AxisPosition.TOP;
        default:
          return AxisPosition.BOTTOM;
      }
    }
  }
}
