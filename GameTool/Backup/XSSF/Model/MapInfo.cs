// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Model.MapInfo
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NPOI.XSSF.Model
{
  public class MapInfo : POIXMLDocumentPart
  {
    private CT_MapInfo mapInfo;
    private Dictionary<int, XSSFMap> maps;

    public MapInfo()
    {
      this.mapInfo = new CT_MapInfo();
    }

    internal MapInfo(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.ReadFrom(part.GetInputStream());
    }

    public void ReadFrom(Stream is1)
    {
      try
      {
        this.mapInfo = MapInfoDocument.Parse(is1).GetMapInfo();
        this.maps = new Dictionary<int, XSSFMap>();
        foreach (CT_Map ctMap in this.mapInfo.Map)
          this.maps[(int) ctMap.ID] = new XSSFMap(ctMap, this);
      }
      catch (XmlException ex)
      {
        throw new IOException(ex.Message);
      }
    }

    public XSSFWorkbook Workbook
    {
      get
      {
        return (XSSFWorkbook) this.GetParent();
      }
    }

    public CT_MapInfo GetCTMapInfo()
    {
      return this.mapInfo;
    }

    public CT_Schema GetCTSchemaById(string schemaId)
    {
      CT_Schema ctSchema1 = (CT_Schema) null;
      foreach (CT_Schema ctSchema2 in this.mapInfo.Schema)
      {
        if (ctSchema2.ID.Equals(schemaId))
        {
          ctSchema1 = ctSchema2;
          break;
        }
      }
      return ctSchema1;
    }

    public XSSFMap GetXSSFMapById(int id)
    {
      return this.maps[id];
    }

    public XSSFMap GetXSSFMapByName(string name)
    {
      XSSFMap xssfMap1 = (XSSFMap) null;
      foreach (XSSFMap xssfMap2 in this.maps.Values)
      {
        if (xssfMap2.GetCTMap().Name != null && xssfMap2.GetCTMap().Name.Equals(name))
          xssfMap1 = xssfMap2;
      }
      return xssfMap1;
    }

    public List<XSSFMap> GetAllXSSFMaps()
    {
      List<XSSFMap> xssfMapList = new List<XSSFMap>();
      foreach (XSSFMap xssfMap in this.maps.Values)
        xssfMapList.Add(xssfMap);
      return xssfMapList;
    }

    protected void WriteTo(Stream out1)
    {
      MapInfoDocument mapInfoDocument = new MapInfoDocument();
      mapInfoDocument.SetMapInfo(this.mapInfo);
      mapInfoDocument.Save(out1);
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.WriteTo(outputStream);
      outputStream.Close();
    }
  }
}
