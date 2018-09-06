// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFRelation
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.Util;
using NPOI.XSSF.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace NPOI.XSSF.UserModel
{
  public class XSSFRelation : POIXMLRelation
  {
    private static POILogger log = POILogFactory.GetLogger(typeof (XSSFRelation));
    protected static Dictionary<string, XSSFRelation> _table = new Dictionary<string, XSSFRelation>();
    public static XSSFRelation WORKBOOK = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/workbook", "/xl/workbook.xml", (Type) null);
    public static XSSFRelation MACROS_WORKBOOK = new XSSFRelation("application/vnd.ms-excel.sheet.macroEnabled.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "/xl/workbook.xml", (Type) null);
    public static XSSFRelation TEMPLATE_WORKBOOK = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.template.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "/xl/workbook.xml", (Type) null);
    public static XSSFRelation MACRO_TEMPLATE_WORKBOOK = new XSSFRelation("application/vnd.ms-excel.template.macroEnabled.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "/xl/workbook.xml", (Type) null);
    public static XSSFRelation MACRO_ADDIN_WORKBOOK = new XSSFRelation("application/vnd.ms-excel.Addin.macroEnabled.main+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "/xl/workbook.xml", (Type) null);
    public static XSSFRelation WORKSHEET = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet", "/xl/worksheets/sheet#.xml", typeof (XSSFSheet));
    public static XSSFRelation CHARTSHEET = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.chartsheet+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartsheet", "/xl/chartsheets/sheet#.xml", typeof (XSSFChartSheet));
    public static XSSFRelation SHARED_STRINGS = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings", "/xl/sharedStrings.xml", typeof (SharedStringsTable));
    public static XSSFRelation STYLES = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", "/xl/styles.xml", typeof (StylesTable));
    public static XSSFRelation DRAWINGS = new XSSFRelation("application/vnd.openxmlformats-officedocument.drawing+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing", "/xl/drawings/drawing#.xml", typeof (XSSFDrawing));
    public static XSSFRelation VML_DRAWINGS = new XSSFRelation("application/vnd.openxmlformats-officedocument.vmlDrawing", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing", "/xl/drawings/vmlDrawing#.vml", typeof (XSSFVMLDrawing));
    public static XSSFRelation CHART = new XSSFRelation("application/vnd.openxmlformats-officedocument.drawingml.chart+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart", "/xl/charts/chart#.xml", typeof (XSSFChart));
    public static XSSFRelation CUSTOM_XML_MAPPINGS = new XSSFRelation("application/xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/xmlMaps", "/xl/xmlMaps.xml", typeof (MapInfo));
    public static XSSFRelation SINGLE_XML_CELLS = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.tableSingleCells+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/tableSingleCells", "/xl/tables/tableSingleCells#.xml", typeof (SingleXmlCells));
    public static XSSFRelation TABLE = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.table+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/table", "/xl/tables/table#.xml", typeof (XSSFTable));
    public static XSSFRelation IMAGES = new XSSFRelation((string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", (string) null, typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_EMF = new XSSFRelation("image/x-emf", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.emf", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_WMF = new XSSFRelation("image/x-wmf", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.wmf", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_PICT = new XSSFRelation("image/pict", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.pict", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_JPEG = new XSSFRelation("image/jpeg", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.jpeg", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_PNG = new XSSFRelation("image/png", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.png", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_DIB = new XSSFRelation("image/dib", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.dib", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_GIF = new XSSFRelation("image/gif", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.gif", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_TIFF = new XSSFRelation("image/tiff", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.tiff", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_EPS = new XSSFRelation("image/x-eps", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.eps", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_BMP = new XSSFRelation("image/x-ms-bmp", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.bmp", typeof (XSSFPictureData));
    public static XSSFRelation IMAGE_WPG = new XSSFRelation("image/x-wpg", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", "/xl/media/image#.wpg", typeof (XSSFPictureData));
    public static XSSFRelation SHEET_COMMENTS = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.comments+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/comments", "/xl/comments#.xml", typeof (CommentsTable));
    public static XSSFRelation SHEET_HYPERLINKS = new XSSFRelation((string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink", (string) null, (Type) null);
    public static XSSFRelation OLEEMBEDDINGS = new XSSFRelation((string) null, POIXMLDocument.OLE_OBJECT_REL_TYPE, (string) null, (Type) null);
    public static XSSFRelation PACKEMBEDDINGS = new XSSFRelation((string) null, POIXMLDocument.PACK_OBJECT_REL_TYPE, (string) null, (Type) null);
    public static XSSFRelation VBA_MACROS = new XSSFRelation("application/vnd.ms-office.vbaProject", "http://schemas.microsoft.com/office/2006/relationships/vbaProject", "/xl/vbaProject.bin", (Type) null);
    public static XSSFRelation ACTIVEX_CONTROLS = new XSSFRelation("application/vnd.ms-office.activeX+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/control", "/xl/activeX/activeX#.xml", (Type) null);
    public static XSSFRelation ACTIVEX_BINS = new XSSFRelation("application/vnd.ms-office.activeX", "http://schemas.microsoft.com/office/2006/relationships/activeXControlBinary", "/xl/activeX/activeX#.bin", (Type) null);
    public static XSSFRelation THEME = new XSSFRelation("application/vnd.openxmlformats-officedocument.theme+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme", "/xl/theme/theme#.xml", typeof (ThemesTable));
    public static XSSFRelation CALC_CHAIN = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.calcChain+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/calcChain", "/xl/calcChain.xml", typeof (CalculationChain));
    public static XSSFRelation PRINTER_SETTINGS = new XSSFRelation("application/vnd.openxmlformats-officedocument.spreadsheetml.printerSettings", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/printerSettings", "/xl/printerSettings/printerSettings#.bin", (Type) null);

    private XSSFRelation(string type, string rel, string defaultName, Type cls)
      : base(type, rel, defaultName, cls)
    {
      if (cls == null || XSSFRelation._table.ContainsKey(rel))
        return;
      XSSFRelation._table.Add(rel, this);
    }

    public Stream GetContents(PackagePart corePart)
    {
      IEnumerator<PackageRelationship> enumerator = corePart.GetRelationshipsByType(this._relation).GetEnumerator();
      if (enumerator.MoveNext())
      {
        PackagePartName partName = PackagingUriHelper.CreatePartName(enumerator.Current.TargetUri);
        return corePart.Package.GetPart(partName).GetInputStream();
      }
      XSSFRelation.log.Log(5, (object) ("No part " + this._defaultName + " found"));
      return (Stream) null;
    }

    public static XSSFRelation GetInstance(string rel)
    {
      if (XSSFRelation._table.ContainsKey(rel))
        return XSSFRelation._table[rel];
      return (XSSFRelation) null;
    }

    public static void RemoveRelation(XSSFRelation relation)
    {
      if (!XSSFRelation._table.ContainsKey(relation._relation))
        return;
      XSSFRelation._table.Remove(relation._relation);
    }

    internal static void AddRelation(XSSFRelation relation)
    {
      if (relation._type == null || XSSFRelation._table.ContainsKey(relation._relation))
        return;
      XSSFRelation._table.Add(relation._relation, relation);
    }
  }
}
