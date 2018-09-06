// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Charts.XSSFChartDataFactory
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.SS.UserModel.Charts;

namespace NPOI.XSSF.UserModel.Charts
{
  public class XSSFChartDataFactory : IChartDataFactory
  {
    private static XSSFChartDataFactory instance;

    private XSSFChartDataFactory()
    {
    }

    public IScatterChartData<Tx, Ty> CreateScatterChartData<Tx, Ty>()
    {
      return (IScatterChartData<Tx, Ty>) new XSSFScatterChartData<Tx, Ty>();
    }

    public static XSSFChartDataFactory GetInstance()
    {
      if (XSSFChartDataFactory.instance == null)
        XSSFChartDataFactory.instance = new XSSFChartDataFactory();
      return XSSFChartDataFactory.instance;
    }
  }
}
