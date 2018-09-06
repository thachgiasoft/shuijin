// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFEvaluationCell
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.SS.Formula;
using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel
{
  public class XSSFEvaluationCell : IEvaluationCell
  {
    private IEvaluationSheet _evalSheet;
    private XSSFCell _cell;

    public XSSFEvaluationCell(ICell cell, XSSFEvaluationSheet EvaluationSheet)
    {
      this._cell = (XSSFCell) cell;
      this._evalSheet = (IEvaluationSheet) EvaluationSheet;
    }

    public XSSFEvaluationCell(ICell cell)
      : this(cell, new XSSFEvaluationSheet(cell.Sheet))
    {
    }

    public object IdentityKey
    {
      get
      {
        return (object) this._cell;
      }
    }

    public XSSFCell GetXSSFCell()
    {
      return this._cell;
    }

    public bool BooleanCellValue
    {
      get
      {
        return this._cell.BooleanCellValue;
      }
    }

    public CellType CellType
    {
      get
      {
        return this._cell.CellType;
      }
    }

    public int ColumnIndex
    {
      get
      {
        return this._cell.ColumnIndex;
      }
    }

    public int ErrorCellValue
    {
      get
      {
        return (int) this._cell.ErrorCellValue;
      }
    }

    public double NumericCellValue
    {
      get
      {
        return this._cell.NumericCellValue;
      }
    }

    public int RowIndex
    {
      get
      {
        return this._cell.RowIndex;
      }
    }

    public IEvaluationSheet Sheet
    {
      get
      {
        return this._evalSheet;
      }
    }

    public string StringCellValue
    {
      get
      {
        return this._cell.RichStringCellValue.String;
      }
    }

    public CellType CachedFormulaResultType
    {
      get
      {
        return this._cell.CachedFormulaResultType;
      }
    }
  }
}
