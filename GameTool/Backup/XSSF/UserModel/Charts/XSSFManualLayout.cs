// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Charts.XSSFManualLayout
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.UserModel.Charts;
using System;

namespace NPOI.XSSF.UserModel.Charts
{
  public class XSSFManualLayout : IManualLayout
  {
    private CT_ManualLayout layout;
    private static LayoutMode defaultLayoutMode;
    private static LayoutTarget defaultLayoutTarget;

    public XSSFManualLayout(CT_Layout ctLayout)
    {
      this.InitLayout(ctLayout);
    }

    public XSSFManualLayout(XSSFChart chart)
    {
      CT_PlotArea plotArea = chart.GetCTChart().plotArea;
      this.InitLayout(plotArea.IsSetLayout() ? plotArea.layout : plotArea.AddNewLayout());
    }

    public CT_ManualLayout GetCTManualLayout()
    {
      return this.layout;
    }

    public void SetWidthRatio(double ratio)
    {
      if (!this.layout.IsSetW())
        this.layout.AddNewW();
      this.layout.w.val = ratio;
    }

    public double GetWidthRatio()
    {
      if (!this.layout.IsSetW())
        return 0.0;
      return this.layout.w.val;
    }

    public void SetHeightRatio(double ratio)
    {
      if (!this.layout.IsSetH())
        this.layout.AddNewH();
      this.layout.h.val = ratio;
    }

    public double GetHeightRatio()
    {
      if (!this.layout.IsSetH())
        return 0.0;
      return this.layout.h.val;
    }

    public LayoutTarget GetTarget()
    {
      if (!this.layout.IsSetLayoutTarget())
        return XSSFManualLayout.defaultLayoutTarget;
      return this.toLayoutTarget(this.layout.layoutTarget);
    }

    public void SetTarget(LayoutTarget target)
    {
      if (!this.layout.IsSetLayoutTarget())
        this.layout.AddNewLayoutTarget();
      this.layout.layoutTarget.val = this.fromLayoutTarget(target);
    }

    public LayoutMode GetXMode()
    {
      if (!this.layout.IsSetXMode())
        return XSSFManualLayout.defaultLayoutMode;
      return this.toLayoutMode(this.layout.xMode);
    }

    public void SetXMode(LayoutMode mode)
    {
      if (!this.layout.IsSetXMode())
        this.layout.AddNewXMode();
      this.layout.xMode.val = this.fromLayoutMode(mode);
    }

    public LayoutMode GetYMode()
    {
      if (!this.layout.IsSetYMode())
        return XSSFManualLayout.defaultLayoutMode;
      return this.toLayoutMode(this.layout.yMode);
    }

    public void SetYMode(LayoutMode mode)
    {
      if (!this.layout.IsSetYMode())
        this.layout.AddNewYMode();
      this.layout.yMode.val = this.fromLayoutMode(mode);
    }

    public double GetX()
    {
      if (!this.layout.IsSetX())
        return 0.0;
      return this.layout.x.val;
    }

    public void SetX(double x)
    {
      if (!this.layout.IsSetX())
        this.layout.AddNewX();
      this.layout.x.val = x;
    }

    public double GetY()
    {
      if (!this.layout.IsSetY())
        return 0.0;
      return this.layout.y.val;
    }

    public void SetY(double y)
    {
      if (!this.layout.IsSetY())
        this.layout.AddNewY();
      this.layout.y.val = y;
    }

    public LayoutMode GetWidthMode()
    {
      if (!this.layout.IsSetWMode())
        return XSSFManualLayout.defaultLayoutMode;
      return this.toLayoutMode(this.layout.wMode);
    }

    public void SetWidthMode(LayoutMode mode)
    {
      if (!this.layout.IsSetWMode())
        this.layout.AddNewWMode();
      this.layout.wMode.val = this.fromLayoutMode(mode);
    }

    public LayoutMode GetHeightMode()
    {
      if (!this.layout.IsSetHMode())
        return XSSFManualLayout.defaultLayoutMode;
      return this.toLayoutMode(this.layout.hMode);
    }

    public void SetHeightMode(LayoutMode mode)
    {
      if (!this.layout.IsSetHMode())
        this.layout.AddNewHMode();
      this.layout.hMode.val = this.fromLayoutMode(mode);
    }

    private void InitLayout(CT_Layout ctLayout)
    {
      if (ctLayout.IsSetManualLayout())
        this.layout = ctLayout.manualLayout;
      else
        this.layout = ctLayout.AddNewManualLayout();
    }

    private ST_LayoutMode fromLayoutMode(LayoutMode mode)
    {
      switch (mode)
      {
        case LayoutMode.EDGE:
          return ST_LayoutMode.edge;
        case LayoutMode.FACTOR:
          return ST_LayoutMode.factor;
        default:
          throw new ArgumentException();
      }
    }

    private LayoutMode toLayoutMode(CT_LayoutMode ctLayoutMode)
    {
      switch (ctLayoutMode.val)
      {
        case ST_LayoutMode.edge:
          return LayoutMode.EDGE;
        case ST_LayoutMode.factor:
          return LayoutMode.FACTOR;
        default:
          throw new ArgumentException();
      }
    }

    private ST_LayoutTarget fromLayoutTarget(LayoutTarget target)
    {
      switch (target)
      {
        case LayoutTarget.INNER:
          return ST_LayoutTarget.inner;
        case LayoutTarget.OUTER:
          return ST_LayoutTarget.outer;
        default:
          throw new ArgumentException();
      }
    }

    private LayoutTarget toLayoutTarget(CT_LayoutTarget ctLayoutTarget)
    {
      switch (ctLayoutTarget.val)
      {
        case ST_LayoutTarget.inner:
          return LayoutTarget.INNER;
        case ST_LayoutTarget.outer:
          return LayoutTarget.OUTER;
        default:
          throw new ArgumentException();
      }
    }
  }
}
