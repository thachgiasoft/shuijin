// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Charts.XSSFChartUtil
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.UserModel.Charts;
using System;

namespace NPOI.XSSF.UserModel.Charts
{
  internal class XSSFChartUtil
  {
    private XSSFChartUtil()
    {
    }

    public static void BuildAxDataSource<T>(CT_AxDataSource ctAxDataSource, IChartDataSource<T> dataSource)
    {
      if (dataSource.IsNumeric)
      {
        if (dataSource.IsReference)
          XSSFChartUtil.BuildNumRef<T>(ctAxDataSource.AddNewNumRef(), dataSource);
        else
          XSSFChartUtil.BuildNumLit<T>(ctAxDataSource.AddNewNumLit(), dataSource);
      }
      else if (dataSource.IsReference)
        XSSFChartUtil.BuildStrRef<T>(ctAxDataSource.AddNewStrRef(), dataSource);
      else
        XSSFChartUtil.BuildStrLit<T>(ctAxDataSource.AddNewStrLit(), dataSource);
    }

    public static void BuildNumDataSource<T>(CT_NumDataSource ctNumDataSource, IChartDataSource<T> dataSource)
    {
      if (dataSource.IsReference)
        XSSFChartUtil.BuildNumRef<T>(ctNumDataSource.AddNewNumRef(), dataSource);
      else
        XSSFChartUtil.BuildNumLit<T>(ctNumDataSource.AddNewNumLit(), dataSource);
    }

    private static void BuildNumRef<T>(CT_NumRef ctNumRef, IChartDataSource<T> dataSource)
    {
      ctNumRef.f = dataSource.FormulaString;
      XSSFChartUtil.FillNumCache<T>(ctNumRef.AddNewNumCache(), dataSource);
    }

    private static void BuildNumLit<T>(CT_NumData ctNumData, IChartDataSource<T> dataSource)
    {
      XSSFChartUtil.FillNumCache<T>(ctNumData, dataSource);
    }

    private static void BuildStrRef<T>(CT_StrRef ctStrRef, IChartDataSource<T> dataSource)
    {
      ctStrRef.f = dataSource.FormulaString;
      XSSFChartUtil.FillStringCache<T>(ctStrRef.AddNewStrCache(), dataSource);
    }

    private static void BuildStrLit<T>(CT_StrData ctStrData, IChartDataSource<T> dataSource)
    {
      XSSFChartUtil.FillStringCache<T>(ctStrData, dataSource);
    }

    private static void FillStringCache<T>(CT_StrData cache, IChartDataSource<T> dataSource)
    {
      int pointCount = dataSource.PointCount;
      cache.AddNewPtCount().val = (uint) pointCount;
      for (int index = 0; index < pointCount; ++index)
      {
        object pointAt = (object) dataSource.GetPointAt(index);
        if (pointAt != null)
        {
          CT_StrVal ctStrVal = cache.AddNewPt();
          ctStrVal.idx = (uint) index;
          ctStrVal.v = pointAt.ToString();
        }
      }
    }

    private static void FillNumCache<T>(CT_NumData cache, IChartDataSource<T> dataSource)
    {
      int pointCount = dataSource.PointCount;
      cache.AddNewPtCount().val = (uint) pointCount;
      for (int index = 0; index < pointCount; ++index)
      {
        double d = Convert.ToDouble((object) dataSource.GetPointAt(index));
        if (!double.IsNaN(d))
        {
          CT_NumVal ctNumVal = cache.AddNewPt();
          ctNumVal.idx = (uint) index;
          ctNumVal.v = d.ToString();
        }
      }
    }
  }
}
