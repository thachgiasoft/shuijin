// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFEvaluationSheet
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.SS.Formula;
using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel
{
  public class XSSFEvaluationSheet : IEvaluationSheet
  {
    private XSSFSheet _xs;

    public XSSFEvaluationSheet(ISheet sheet)
    {
      this._xs = (XSSFSheet) sheet;
    }

    public XSSFSheet GetXSSFSheet()
    {
      return this._xs;
    }

    public IEvaluationCell GetCell(int rowIndex, int columnIndex)
    {
      IRow row = this._xs.GetRow(rowIndex);
      if (row == null)
        return (IEvaluationCell) null;
      ICell cell = row.GetCell(columnIndex);
      if (cell == null)
        return (IEvaluationCell) null;
      return (IEvaluationCell) new XSSFEvaluationCell(cell, this);
    }
  }
}
