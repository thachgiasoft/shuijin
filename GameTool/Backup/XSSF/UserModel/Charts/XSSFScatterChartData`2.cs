// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Charts.XSSFScatterChartData`2
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.UserModel;
using NPOI.SS.UserModel.Charts;
using System;
using System.Collections.Generic;

namespace NPOI.XSSF.UserModel.Charts
{
  public class XSSFScatterChartData<Tx, Ty> : IScatterChartData<Tx, Ty>, IChartData
  {
    private List<IScatterChartSerie<Tx, Ty>> series;

    public XSSFScatterChartData()
    {
      this.series = new List<IScatterChartSerie<Tx, Ty>>();
    }

    public IScatterChartSerie<Tx, Ty> AddSerie(IChartDataSource<Tx> xs, IChartDataSource<Ty> ys)
    {
      if (!ys.IsNumeric)
        throw new ArgumentException("Y axis data source must be numeric.");
      int count = this.series.Count;
      XSSFScatterChartData<Tx, Ty>.Serie serie = new XSSFScatterChartData<Tx, Ty>.Serie(count, count, xs, ys);
      this.series.Add((IScatterChartSerie<Tx, Ty>) serie);
      return (IScatterChartSerie<Tx, Ty>) serie;
    }

    public void FillChart(IChart chart, IChartAxis[] axis)
    {
      if (!(chart is XSSFChart))
        throw new ArgumentException("Chart must be instance of XSSFChart");
      CT_ScatterChart ctScatterChart = ((XSSFChart) chart).GetCTChart().plotArea.AddNewScatterChart();
      this.AddStyle(ctScatterChart);
      foreach (XSSFScatterChartData<Tx, Ty>.Serie serie in this.series)
        serie.AddToChart(ctScatterChart);
      foreach (IChartAxis axi in axis)
        ctScatterChart.AddNewAxId().val = (uint) axi.GetId();
    }

    public List<IScatterChartSerie<Tx, Ty>> GetSeries()
    {
      return this.series;
    }

    private void AddStyle(CT_ScatterChart ctScatterChart)
    {
      ctScatterChart.AddNewScatterStyle().val = ST_ScatterStyle.lineMarker;
    }

    public class Serie : IScatterChartSerie<Tx, Ty>
    {
      private int id;
      private int order;
      private bool useCache;
      private IChartDataSource<Tx> xs;
      private IChartDataSource<Ty> ys;

      internal Serie(int id, int order, IChartDataSource<Tx> xs, IChartDataSource<Ty> ys)
      {
        this.id = id;
        this.order = order;
        this.xs = xs;
        this.ys = ys;
      }

      public IChartDataSource<Tx> GetXValues()
      {
        return this.xs;
      }

      public IChartDataSource<Ty> GetYValues()
      {
        return this.ys;
      }

      public void SetUseCache(bool useCache)
      {
        this.useCache = useCache;
      }

      internal void AddToChart(CT_ScatterChart ctScatterChart)
      {
        CT_ScatterSer ctScatterSer = ctScatterChart.AddNewSer();
        ctScatterSer.AddNewIdx().val = (uint) this.id;
        ctScatterSer.AddNewOrder().val = (uint) this.order;
        XSSFChartUtil.BuildAxDataSource<Tx>(ctScatterSer.AddNewXVal(), this.xs);
        XSSFChartUtil.BuildNumDataSource<Ty>(ctScatterSer.AddNewYVal(), this.ys);
      }
    }
  }
}
