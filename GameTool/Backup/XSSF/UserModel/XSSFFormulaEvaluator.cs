// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFFormulaEvaluator
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.HSSF.UserModel;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Eval;
using NPOI.SS.Formula.Udf;
using NPOI.SS.UserModel;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFFormulaEvaluator : IFormulaEvaluator
  {
    private WorkbookEvaluator _bookEvaluator;
    private XSSFWorkbook _book;

    public XSSFFormulaEvaluator(IWorkbook workbook)
      : this(workbook as XSSFWorkbook, (IStabilityClassifier) null, (UDFFinder) null)
    {
    }

    public XSSFFormulaEvaluator(XSSFWorkbook workbook)
      : this(workbook, (IStabilityClassifier) null, (UDFFinder) null)
    {
    }

    public XSSFFormulaEvaluator(XSSFWorkbook workbook, IStabilityClassifier stabilityClassifier)
    {
      this._bookEvaluator = new WorkbookEvaluator((IEvaluationWorkbook) XSSFEvaluationWorkbook.Create((IWorkbook) workbook), stabilityClassifier, (UDFFinder) null);
      this._book = workbook;
    }

    private XSSFFormulaEvaluator(XSSFWorkbook workbook, IStabilityClassifier stabilityClassifier, UDFFinder udfFinder)
    {
      this._bookEvaluator = new WorkbookEvaluator((IEvaluationWorkbook) XSSFEvaluationWorkbook.Create((IWorkbook) workbook), stabilityClassifier, udfFinder);
      this._book = workbook;
    }

    public static XSSFFormulaEvaluator Create(XSSFWorkbook workbook, IStabilityClassifier stabilityClassifier, UDFFinder udfFinder)
    {
      return new XSSFFormulaEvaluator(workbook, stabilityClassifier, udfFinder);
    }

    public void ClearAllCachedResultValues()
    {
      this._bookEvaluator.ClearAllCachedResultValues();
    }

    public void NotifySetFormula(ICell cell)
    {
      this._bookEvaluator.NotifyUpdateCell((IEvaluationCell) new XSSFEvaluationCell(cell));
    }

    public void NotifyDeleteCell(ICell cell)
    {
      this._bookEvaluator.NotifyDeleteCell((IEvaluationCell) new XSSFEvaluationCell(cell));
    }

    public void NotifyUpdateCell(ICell cell)
    {
      this._bookEvaluator.NotifyUpdateCell((IEvaluationCell) new XSSFEvaluationCell(cell));
    }

    public CellValue Evaluate(ICell cell)
    {
      if (cell == null)
        return (CellValue) null;
      switch (cell.CellType)
      {
        case CellType.NUMERIC:
          return new CellValue(cell.NumericCellValue);
        case CellType.STRING:
          return new CellValue(cell.RichStringCellValue.String);
        case CellType.FORMULA:
          return this.EvaluateFormulaCellValue(cell);
        case CellType.BLANK:
          return (CellValue) null;
        case CellType.BOOLEAN:
          return CellValue.ValueOf(cell.BooleanCellValue);
        case CellType.ERROR:
          return CellValue.GetError((int) cell.ErrorCellValue);
        default:
          throw new InvalidOperationException("Bad cell type (" + (object) cell.CellType + ")");
      }
    }

    public CellType EvaluateFormulaCell(ICell cell)
    {
      if (cell == null || cell.CellType != CellType.FORMULA)
        return CellType.Unknown;
      CellValue formulaCellValue = this.EvaluateFormulaCellValue(cell);
      XSSFFormulaEvaluator.SetCellValue(cell, formulaCellValue);
      return formulaCellValue.CellType;
    }

    public ICell EvaluateInCell(ICell cell)
    {
      if (cell == null)
        return (ICell) null;
      XSSFCell xssfCell = (XSSFCell) cell;
      if (cell.CellType == CellType.FORMULA)
      {
        CellValue formulaCellValue = this.EvaluateFormulaCellValue(cell);
        XSSFFormulaEvaluator.SetCellType(cell, formulaCellValue);
        XSSFFormulaEvaluator.SetCellValue(cell, formulaCellValue);
      }
      return (ICell) xssfCell;
    }

    private static void SetCellType(ICell cell, CellValue cv)
    {
      CellType cellType = cv.CellType;
      switch (cellType)
      {
        case CellType.NUMERIC:
        case CellType.STRING:
        case CellType.BOOLEAN:
        case CellType.ERROR:
          cell.SetCellType(cellType);
          break;
        default:
          throw new InvalidOperationException("Unexpected cell value type (" + (object) cellType + ")");
      }
    }

    private static void SetCellValue(ICell cell, CellValue cv)
    {
      CellType cellType = cv.CellType;
      switch (cellType)
      {
        case CellType.NUMERIC:
          cell.SetCellValue(cv.NumberValue);
          break;
        case CellType.STRING:
          cell.SetCellValue((IRichTextString) new XSSFRichTextString(cv.StringValue));
          break;
        case CellType.BOOLEAN:
          cell.SetCellValue(cv.BooleanValue);
          break;
        case CellType.ERROR:
          cell.SetCellErrorValue((byte) cv.ErrorValue);
          break;
        default:
          throw new InvalidOperationException("Unexpected cell value type (" + (object) cellType + ")");
      }
    }

    public static void EvaluateAllFormulaCells(IWorkbook wb)
    {
      HSSFFormulaEvaluator.EvaluateAllFormulaCells(wb);
    }

    public void EvaluateAll()
    {
      HSSFFormulaEvaluator.EvaluateAllFormulaCells((IWorkbook) this._book);
    }

    private CellValue EvaluateFormulaCellValue(ICell cell)
    {
      if (!(cell is XSSFCell))
        throw new ArgumentException("Unexpected type of cell: " + (object) cell.GetType() + ". Only XSSFCells can be Evaluated.");
      ValueEval valueEval = this._bookEvaluator.Evaluate((IEvaluationCell) new XSSFEvaluationCell(cell));
      if (valueEval is NumberEval)
        return new CellValue(((NumberEval) valueEval).NumberValue);
      if (valueEval is BoolEval)
        return CellValue.ValueOf(((BoolEval) valueEval).BooleanValue);
      if (valueEval is StringEval)
        return new CellValue(((StringEval) valueEval).StringValue);
      if (valueEval is ErrorEval)
        return CellValue.GetError(((ErrorEval) valueEval).ErrorCode);
      throw new Exception("Unexpected eval class (" + valueEval.GetType().Name + ")");
    }

    public bool DebugEvaluationOutputForNextEval
    {
      get
      {
        return this._bookEvaluator.DebugEvaluationOutputForNextEval;
      }
      set
      {
        this._bookEvaluator.DebugEvaluationOutputForNextEval = value;
      }
    }
  }
}
