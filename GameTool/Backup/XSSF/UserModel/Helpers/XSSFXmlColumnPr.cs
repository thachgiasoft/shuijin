// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Helpers.XSSFXmlColumnPr
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;

namespace NPOI.XSSF.UserModel.Helpers
{
  public class XSSFXmlColumnPr
  {
    private XSSFTable table;
    private CT_TableColumn ctTableColumn;
    private CT_XmlColumnPr ctXmlColumnPr;

    public XSSFXmlColumnPr(XSSFTable table, CT_TableColumn ctTableColum, CT_XmlColumnPr CT_XmlColumnPr)
    {
      this.table = table;
      this.ctTableColumn = ctTableColum;
      this.ctXmlColumnPr = CT_XmlColumnPr;
    }

    public long GetMapId()
    {
      return (long) this.ctXmlColumnPr.mapId;
    }

    public string GetXPath()
    {
      return this.ctXmlColumnPr.xpath;
    }

    public long GetId()
    {
      return (long) this.ctTableColumn.id;
    }

    public string GetLocalXPath()
    {
      string str = "";
      int num = this.table.GetCommonXpath().Split('/').Length - 1;
      string[] strArray = this.ctXmlColumnPr.xpath.Split('/');
      for (int index = num; index < strArray.Length; ++index)
        str = str + "/" + strArray[index];
      return str;
    }

    public ST_XmlDataType GetXmlDataType()
    {
      return this.ctXmlColumnPr.xmlDataType;
    }
  }
}
