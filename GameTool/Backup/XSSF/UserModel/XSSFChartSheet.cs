// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFChartSheet
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NPOI.XSSF.UserModel
{
  public class XSSFChartSheet : XSSFSheet
  {
    private static byte[] BLANK_WORKSHEET = XSSFChartSheet.blankWorksheet();
    protected CT_Chartsheet chartsheet;

    protected XSSFChartSheet(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
    }

    internal override void Read(Stream is1)
    {
      base.Read((Stream) new MemoryStream(XSSFChartSheet.BLANK_WORKSHEET));
      try
      {
        this.chartsheet = ChartsheetDocument.Parse(is1).GetChartsheet();
      }
      catch (XmlException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
    }

    public CT_Chartsheet GetCTChartsheet()
    {
      return this.chartsheet;
    }

    protected override CT_Drawing GetCTDrawing()
    {
      return this.chartsheet.drawing;
    }

    protected override CT_LegacyDrawing GetCTLegacyDrawing()
    {
      return this.chartsheet.legacyDrawing;
    }

    internal override void Write(Stream out1)
    {
      new Dictionary<string, string>()[ST_RelationshipId.NamespaceURI] = "r";
      this.chartsheet.Save(out1);
    }

    private static byte[] blankWorksheet()
    {
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        new XSSFSheet().Write((Stream) memoryStream);
      }
      catch (IOException ex)
      {
        throw new RuntimeException((Exception) ex);
      }
      return memoryStream.ToArray();
    }
  }
}
