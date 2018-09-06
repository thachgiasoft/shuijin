// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Helpers.XSSFSingleXmlCell
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.Model;

namespace NPOI.XSSF.UserModel.Helpers
{
  public class XSSFSingleXmlCell
  {
    private CT_SingleXmlCell SingleXmlCell;
    private SingleXmlCells parent;

    public XSSFSingleXmlCell(CT_SingleXmlCell SingleXmlCell, SingleXmlCells parent)
    {
      this.SingleXmlCell = SingleXmlCell;
      this.parent = parent;
    }

    public ICell GetReferencedCell()
    {
      CellReference cellReference = new CellReference(this.SingleXmlCell.r);
      IRow row = this.parent.GetXSSFSheet().GetRow(cellReference.Row) ?? this.parent.GetXSSFSheet().CreateRow(cellReference.Row);
      return row.GetCell((int) cellReference.Col) ?? row.CreateCell((int) cellReference.Col);
    }

    public string GetXpath()
    {
      return this.SingleXmlCell.xmlCellPr.xmlPr.xpath;
    }

    public long GetMapId()
    {
      return (long) this.SingleXmlCell.xmlCellPr.xmlPr.mapId;
    }

    public ST_XmlDataType GetXmlDataType()
    {
      return this.SingleXmlCell.xmlCellPr.xmlPr.xmlDataType;
    }
  }
}
