// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFMap
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.XSSF.Model;
using NPOI.XSSF.UserModel.Helpers;
using System.Collections.Generic;
using System.Xml;

namespace NPOI.XSSF.UserModel
{
  public class XSSFMap
  {
    private CT_Map ctMap;
    private MapInfo mapInfo;

    public XSSFMap(CT_Map ctMap, MapInfo mapInfo)
    {
      this.ctMap = ctMap;
      this.mapInfo = mapInfo;
    }

    public CT_Map GetCTMap()
    {
      return this.ctMap;
    }

    public CT_Schema GetCTSchema()
    {
      return this.mapInfo.GetCTSchemaById(this.ctMap.SchemaID);
    }

    public XmlNode GetSchema()
    {
      return (XmlNode) this.GetCTSchema().Any;
    }

    public List<XSSFSingleXmlCell> GetRelatedSingleXMLCell()
    {
      List<XSSFSingleXmlCell> xssfSingleXmlCellList = new List<XSSFSingleXmlCell>();
      int numberOfSheets = this.mapInfo.Workbook.NumberOfSheets;
      for (int index = 0; index < numberOfSheets; ++index)
      {
        foreach (POIXMLDocumentPart relation in ((POIXMLDocumentPart) this.mapInfo.Workbook.GetSheetAt(index)).GetRelations())
        {
          if (relation is SingleXmlCells)
          {
            foreach (XSSFSingleXmlCell xssfSingleXmlCell in ((SingleXmlCells) relation).GetAllSimpleXmlCell())
            {
              if (xssfSingleXmlCell.GetMapId() == (long) this.ctMap.ID)
                xssfSingleXmlCellList.Add(xssfSingleXmlCell);
            }
          }
        }
      }
      return xssfSingleXmlCellList;
    }

    public List<XSSFTable> GetRelatedTables()
    {
      List<XSSFTable> xssfTableList = new List<XSSFTable>();
      int numberOfSheets = this.mapInfo.Workbook.NumberOfSheets;
      for (int index = 0; index < numberOfSheets; ++index)
      {
        foreach (POIXMLDocumentPart relation in ((POIXMLDocumentPart) this.mapInfo.Workbook.GetSheetAt(index)).GetRelations())
        {
          if (relation.GetPackageRelationship().RelationshipType.Equals(XSSFRelation.TABLE.Relation))
          {
            XSSFTable xssfTable = (XSSFTable) relation;
            if (xssfTable.MapsTo((long) this.ctMap.ID))
              xssfTableList.Add(xssfTable);
          }
        }
      }
      return xssfTableList;
    }
  }
}
