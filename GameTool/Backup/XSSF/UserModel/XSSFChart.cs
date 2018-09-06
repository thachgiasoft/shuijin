// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFChart
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats;
using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.OpenXmlFormats.Drawing;
using NPOI.SS.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.XSSF.UserModel.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XSSF.UserModel
{
  public class XSSFChart : POIXMLDocumentPart, IChart, ManuallyPositionable, IChartAxisFactory
  {
    private XSSFGraphicFrame frame;
    private CT_ChartSpace chartSpace;
    private CT_Chart chart;
    private List<IChartAxis> axis;

    public XSSFChart()
    {
      this.axis = new List<IChartAxis>();
      this.CreateChart();
    }

    protected XSSFChart(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.chartSpace = ChartSpaceDocument.Parse(part.GetInputStream()).GetChartSpace();
      this.chart = this.chartSpace.chart;
    }

    private void CreateChart()
    {
      this.chartSpace = new CT_ChartSpace();
      this.chart = this.chartSpace.AddNewChart();
      this.chart.AddNewPlotArea().AddNewLayout();
      this.chart.AddNewPlotVisOnly().val = 1;
      CT_PrintSettings ctPrintSettings = this.chartSpace.AddNewPrintSettings();
      ctPrintSettings.AddNewHeaderFooter();
      CT_PageMargins ctPageMargins = ctPrintSettings.AddNewPageMargins();
      ctPageMargins.b = 0.75;
      ctPageMargins.l = 0.7;
      ctPageMargins.r = 0.7;
      ctPageMargins.t = 0.75;
      ctPageMargins.header = 0.3;
      ctPageMargins.footer = 0.3;
      ctPrintSettings.AddNewPageSetup();
    }

    internal CT_ChartSpace GetCTChartSpace()
    {
      return this.chartSpace;
    }

    internal CT_Chart GetCTChart()
    {
      return this.chart;
    }

    protected override void Commit()
    {
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
      namespaces.Add("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
      namespaces.Add("c", "http://schemas.openxmlformats.org/drawingml/2006/chart");
      namespaces.Add("r", ST_RelationshipId.NamespaceURI);
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.chartSpace.Save(outputStream, namespaces);
      outputStream.Close();
    }

    public XSSFGraphicFrame GetGraphicFrame()
    {
      return this.frame;
    }

    internal void SetGraphicFrame(XSSFGraphicFrame frame)
    {
      this.frame = frame;
    }

    public IChartDataFactory GetChartDataFactory()
    {
      return (IChartDataFactory) XSSFChartDataFactory.GetInstance();
    }

    public IChartAxisFactory GetChartAxisFactory()
    {
      return (IChartAxisFactory) this;
    }

    public void Plot(IChartData data, params IChartAxis[] axis)
    {
      data.FillChart((IChart) this, axis);
    }

    public IValueAxis CreateValueAxis(AxisPosition pos)
    {
      XSSFValueAxis xssfValueAxis = new XSSFValueAxis(this, (long) (this.axis.Count + 1), pos);
      if (this.axis.Count == 1)
      {
        IChartAxis axi = this.axis[0];
        axi.CrossAxis((IChartAxis) xssfValueAxis);
        xssfValueAxis.CrossAxis(axi);
      }
      this.axis.Add((IChartAxis) xssfValueAxis);
      return (IValueAxis) xssfValueAxis;
    }

    public List<IChartAxis> GetAxis()
    {
      if (this.axis.Count == 0 && this.HasAxis())
        this.ParseAxis();
      return this.axis;
    }

    public IManualLayout GetManualLayout()
    {
      return (IManualLayout) new XSSFManualLayout(this);
    }

    public bool IsPlotOnlyVisibleCells()
    {
      return this.chart.plotVisOnly.val == 1;
    }

    public void SetPlotOnlyVisibleCells(bool plotVisOnly)
    {
      this.chart.plotVisOnly.val = plotVisOnly ? 1 : 0;
    }

    public XSSFRichTextString GetTitle()
    {
      if (!this.chart.IsSetTitle())
        return (XSSFRichTextString) null;
      CT_Title title = this.chart.title;
      StringBuilder sb = new StringBuilder();
      new XmlSerializer(typeof (CT_Title)).Serialize((TextWriter) new StringWriter(sb), (object) title);
      string xml = sb.ToString();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      sb.Length = 0;
      sb.Append(xmlDocument.InnerText);
      return new XSSFRichTextString(sb.ToString());
    }

    public IChartLegend GetOrCreateLegend()
    {
      return (IChartLegend) new XSSFChartLegend(this);
    }

    public void DeleteLegend()
    {
      if (!this.chart.IsSetLegend())
        return;
      this.chart.unsetLegend();
    }

    private bool HasAxis()
    {
      CT_PlotArea plotArea = this.chart.plotArea;
      throw new NotImplementedException();
    }

    private void ParseAxis()
    {
      this.ParseValueAxis();
    }

    private void ParseValueAxis()
    {
      throw new NotImplementedException();
    }
  }
}
