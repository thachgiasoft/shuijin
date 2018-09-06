// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFCell
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Eval;
using NPOI.SS.Formula.PTG;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.Util;
using NPOI.XSSF.Model;
using System;
using System.Globalization;

namespace NPOI.XSSF.UserModel
{
  public class XSSFCell : ICell
  {
    private static string FALSE_AS_STRING = "0";
    private static string TRUE_AS_STRING = "1";
    private CT_Cell _cell;
    private XSSFRow _row;
    private int _cellNum;
    private SharedStringsTable _sharedStringSource;
    private StylesTable _stylesSource;

    public XSSFCell(XSSFRow row, CT_Cell cell)
    {
      this._cell = cell;
      this._row = row;
      if (cell.r != null)
        this._cellNum = (int) new CellReference(cell.r).Col;
      this._sharedStringSource = ((XSSFWorkbook) row.Sheet.Workbook).GetSharedStringSource();
      this._stylesSource = ((XSSFWorkbook) row.Sheet.Workbook).GetStylesSource();
    }

    protected SharedStringsTable GetSharedStringSource()
    {
      return this._sharedStringSource;
    }

    protected StylesTable GetStylesSource()
    {
      return this._stylesSource;
    }

    public ISheet Sheet
    {
      get
      {
        return this._row.Sheet;
      }
    }

    public IRow Row
    {
      get
      {
        return (IRow) this._row;
      }
    }

    public bool BooleanCellValue
    {
      get
      {
        CellType cellType = this.CellType;
        switch (cellType)
        {
          case CellType.FORMULA:
            if (this._cell.IsSetV())
              return XSSFCell.TRUE_AS_STRING.Equals(this._cell.v);
            return false;
          case CellType.BLANK:
            return false;
          case CellType.BOOLEAN:
            if (this._cell.IsSetV())
              return XSSFCell.TRUE_AS_STRING.Equals(this._cell.v);
            return false;
          default:
            throw XSSFCell.TypeMismatch(CellType.BOOLEAN, cellType, false);
        }
      }
    }

    public void SetCellValue(bool value)
    {
      this._cell.t = ST_CellType.b;
      this._cell.v = value ? XSSFCell.TRUE_AS_STRING : XSSFCell.FALSE_AS_STRING;
    }

    public double NumericCellValue
    {
      get
      {
        CellType cellType = this.CellType;
        switch (cellType)
        {
          case CellType.NUMERIC:
          case CellType.FORMULA:
            if (!this._cell.IsSetV())
              return 0.0;
            try
            {
              return double.Parse(this._cell.v, (IFormatProvider) CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
              throw XSSFCell.TypeMismatch(CellType.NUMERIC, CellType.STRING, false);
            }
          case CellType.BLANK:
            return 0.0;
          default:
            throw XSSFCell.TypeMismatch(CellType.NUMERIC, cellType, false);
        }
      }
    }

    public void SetCellValue(double value)
    {
      if (double.IsInfinity(value))
      {
        this._cell.t = ST_CellType.e;
        this._cell.v = FormulaError.DIV0.String;
      }
      else if (double.IsNaN(value))
      {
        this._cell.t = ST_CellType.e;
        this._cell.v = FormulaError.NUM.String;
      }
      else
      {
        this._cell.t = ST_CellType.n;
        this._cell.v = value.ToString();
      }
    }

    public string StringCellValue
    {
      get
      {
        return this.RichStringCellValue?.String;
      }
    }

    public IRichTextString RichStringCellValue
    {
      get
      {
        CellType cellType = this.CellType;
        XSSFRichTextString xssfRichTextString;
        switch (cellType)
        {
          case CellType.STRING:
            xssfRichTextString = this._cell.t != ST_CellType.inlineStr ? (this._cell.t != ST_CellType.str ? (!this._cell.IsSetV() ? new XSSFRichTextString("") : new XSSFRichTextString(this._sharedStringSource.GetEntryAt(int.Parse(this._cell.v)))) : new XSSFRichTextString(this._cell.IsSetV() ? this._cell.v : "")) : (!this._cell.IsSetIs() ? (!this._cell.IsSetV() ? new XSSFRichTextString("") : new XSSFRichTextString(this._cell.v)) : new XSSFRichTextString(this._cell.@is));
            break;
          case CellType.FORMULA:
            XSSFCell.CheckFormulaCachedValueType(CellType.STRING, this.GetBaseCellType(false));
            xssfRichTextString = new XSSFRichTextString(this._cell.IsSetV() ? this._cell.v : "");
            break;
          case CellType.BLANK:
            xssfRichTextString = new XSSFRichTextString("");
            break;
          default:
            throw XSSFCell.TypeMismatch(CellType.STRING, cellType, false);
        }
        xssfRichTextString.SetStylesTableReference(this._stylesSource);
        return (IRichTextString) xssfRichTextString;
      }
    }

    private static void CheckFormulaCachedValueType(CellType expectedTypeCode, CellType cachedValueType)
    {
      if (cachedValueType != expectedTypeCode)
        throw XSSFCell.TypeMismatch(expectedTypeCode, cachedValueType, true);
    }

    public void SetCellValue(string str)
    {
      this.SetCellValue(str == null ? (IRichTextString) null : (IRichTextString) new XSSFRichTextString(str));
    }

    public void SetCellValue(IRichTextString str)
    {
      if (str == null || str.String == null)
        this.SetCellType(CellType.BLANK);
      else if (this.CellType == CellType.FORMULA)
      {
        this._cell.v = str.String;
        this._cell.t = ST_CellType.str;
      }
      else if (this._cell.t == ST_CellType.inlineStr)
      {
        this._cell.v = str.String;
      }
      else
      {
        this._cell.t = ST_CellType.s;
        XSSFRichTextString xssfRichTextString = (XSSFRichTextString) str;
        xssfRichTextString.SetStylesTableReference(this._stylesSource);
        this._cell.v = this._sharedStringSource.AddEntry(xssfRichTextString.GetCTRst()).ToString();
      }
    }

    public string CellFormula
    {
      get
      {
        CellType cellType = this.CellType;
        if (cellType != CellType.FORMULA)
          throw XSSFCell.TypeMismatch(CellType.FORMULA, cellType, false);
        CT_CellFormula f = this._cell.f;
        if (this.IsPartOfArrayFormulaGroup && f == null)
          return ((XSSFSheet) this.Sheet).GetFirstCellInArrayFormula((ICell) this).CellFormula;
        if (f.t == ST_CellFormulaType.shared)
          return this.ConvertSharedFormula((int) f.si);
        return f.Value;
      }
      set
      {
        this.SetCellFormula(value);
      }
    }

    private string ConvertSharedFormula(int si)
    {
      XSSFSheet sheet = (XSSFSheet) this.Sheet;
      CT_CellFormula sharedFormula = sheet.GetSharedFormula(si);
      if (sharedFormula == null)
        throw new InvalidOperationException("Master cell of a shared formula with sid=" + (object) si + " was not found");
      string formula = sharedFormula.Value;
      CellRangeAddress cellRangeAddress = CellRangeAddress.ValueOf(sharedFormula.@ref);
      int sheetIndex = sheet.Workbook.GetSheetIndex((ISheet) sheet);
      XSSFEvaluationWorkbook evaluationWorkbook = XSSFEvaluationWorkbook.Create(sheet.Workbook);
      Ptg[] ptgs = new SharedFormula(SpreadsheetVersion.EXCEL2007).ConvertSharedFormulas(FormulaParser.Parse(formula, (IFormulaParsingWorkbook) evaluationWorkbook, FormulaType.CELL, sheetIndex), this.RowIndex - cellRangeAddress.FirstRow, this.ColumnIndex - cellRangeAddress.FirstColumn);
      return FormulaRenderer.ToFormulaString((IFormulaRenderingWorkbook) evaluationWorkbook, ptgs);
    }

    public void SetCellFormula(string formula)
    {
      if (this.IsPartOfArrayFormulaGroup)
        this.NotifyArrayFormulaChanging();
      this.SetFormula(formula, FormulaType.CELL);
    }

    internal void SetCellArrayFormula(string formula, CellRangeAddress range)
    {
      this.SetFormula(formula, FormulaType.ARRAY);
      CT_CellFormula f = this._cell.f;
      f.t = ST_CellFormulaType.array;
      f.@ref = range.FormatAsString();
    }

    private void SetFormula(string formula, FormulaType formulaType)
    {
      IWorkbook workbook1 = this._row.Sheet.Workbook;
      if (formula == null)
      {
        ((XSSFWorkbook) workbook1).OnDeleteFormula(this);
        if (!this._cell.IsSetF())
          return;
        this._cell.unsetF();
      }
      else
      {
        IFormulaParsingWorkbook workbook2 = (IFormulaParsingWorkbook) XSSFEvaluationWorkbook.Create(workbook1);
        FormulaParser.Parse(formula, workbook2, formulaType, workbook1.GetSheetIndex(this.Sheet));
        this._cell.f = new CT_CellFormula()
        {
          Value = formula
        };
        if (!this._cell.IsSetV())
          return;
        this._cell.unsetV();
      }
    }

    public int ColumnIndex
    {
      get
      {
        return this._cellNum;
      }
    }

    public int RowIndex
    {
      get
      {
        return this._row.RowNum;
      }
    }

    public string GetReference()
    {
      return this._cell.r;
    }

    public ICellStyle CellStyle
    {
      get
      {
        XSSFCellStyle xssfCellStyle = (XSSFCellStyle) null;
        if (this._stylesSource != null && this._stylesSource.GetNumCellStyles() > 0)
          xssfCellStyle = this._stylesSource.GetStyleAt(this._cell.IsSetS() ? (int) this._cell.s : 0);
        return (ICellStyle) xssfCellStyle;
      }
      set
      {
        if (value == null)
        {
          if (!this._cell.IsSetS())
            return;
          this._cell.unsetS();
        }
        else
        {
          XSSFCellStyle style = (XSSFCellStyle) value;
          style.VerifyBelongsToStylesSource(this._stylesSource);
          this._cell.s = (uint) (ulong) this._stylesSource.PutStyle(style);
        }
      }
    }

    public CellType CellType
    {
      get
      {
        if (this._cell.f != null || ((XSSFSheet) this.Sheet).IsCellInArrayFormulaContext((ICell) this))
          return CellType.FORMULA;
        return this.GetBaseCellType(true);
      }
    }

    public CellType CachedFormulaResultType
    {
      get
      {
        if (this._cell.f == null)
          throw new InvalidOperationException("Only formula cells have cached results");
        return this.GetBaseCellType(false);
      }
    }

    private CellType GetBaseCellType(bool blankCells)
    {
      switch (this._cell.t)
      {
        case ST_CellType.b:
          return CellType.BOOLEAN;
        case ST_CellType.n:
          return !this._cell.IsSetV() && blankCells ? CellType.BLANK : CellType.NUMERIC;
        case ST_CellType.e:
          return CellType.ERROR;
        case ST_CellType.s:
        case ST_CellType.str:
        case ST_CellType.inlineStr:
          return CellType.STRING;
        default:
          throw new InvalidOperationException("Illegal cell type: " + (object) this._cell.t);
      }
    }

    public DateTime DateCellValue
    {
      get
      {
        if (this.CellType == CellType.BLANK)
          return DateTime.MinValue;
        return DateUtil.GetJavaDate(this.NumericCellValue, ((XSSFWorkbook) this.Sheet.Workbook).IsDate1904());
      }
    }

    public void SetCellValue(DateTime value)
    {
      bool use1904windowing = ((XSSFWorkbook) this.Sheet.Workbook).IsDate1904();
      this.SetCellValue(DateUtil.GetExcelDate(value, use1904windowing));
    }

    public string ErrorCellString
    {
      get
      {
        CellType baseCellType = this.GetBaseCellType(true);
        if (baseCellType != CellType.ERROR)
          throw XSSFCell.TypeMismatch(CellType.ERROR, baseCellType, false);
        return this._cell.v;
      }
    }

    public byte ErrorCellValue
    {
      get
      {
        string errorCellString = this.ErrorCellString;
        if (errorCellString == null)
          return 0;
        return FormulaError.ForString(errorCellString).Code;
      }
    }

    public void SetCellErrorValue(byte errorCode)
    {
      this.SetCellErrorValue(FormulaError.ForInt(errorCode));
    }

    public void SetCellErrorValue(FormulaError error)
    {
      this._cell.t = ST_CellType.e;
      this._cell.v = error.String;
    }

    public void SetAsActiveCell()
    {
      ((XSSFSheet) this.Sheet).SetActiveCell(this._cell.r);
    }

    private void SetBlank()
    {
      CT_Cell cell = new CT_Cell();
      cell.r = this._cell.r;
      if (this._cell.IsSetS())
        cell.s = this._cell.s;
      this._cell.Set(cell);
    }

    internal void SetCellNum(int num)
    {
      XSSFCell.CheckBounds(num);
      this._cellNum = num;
      this._cell.r = new CellReference(this.RowIndex, this.ColumnIndex).FormatAsString();
    }

    public void SetCellType(CellType cellType)
    {
      CellType cellType1 = this.CellType;
      if (this.IsPartOfArrayFormulaGroup)
        this.NotifyArrayFormulaChanging();
      if (cellType1 == CellType.FORMULA && cellType != CellType.FORMULA)
        ((XSSFWorkbook) this.Sheet.Workbook).OnDeleteFormula(this);
      switch (cellType)
      {
        case CellType.NUMERIC:
          this._cell.t = ST_CellType.n;
          break;
        case CellType.STRING:
          if (cellType1 != CellType.STRING)
          {
            XSSFRichTextString xssfRichTextString = new XSSFRichTextString(this.ConvertCellValueToString());
            xssfRichTextString.SetStylesTableReference(this._stylesSource);
            this._cell.v = this._sharedStringSource.AddEntry(xssfRichTextString.GetCTRst()).ToString();
          }
          this._cell.t = ST_CellType.s;
          break;
        case CellType.FORMULA:
          if (!this._cell.IsSetF())
          {
            this._cell.f = new CT_CellFormula()
            {
              Value = "0"
            };
            if (this._cell.IsSetT())
            {
              this._cell.unsetT();
              break;
            }
            break;
          }
          break;
        case CellType.BLANK:
          this.SetBlank();
          break;
        case CellType.BOOLEAN:
          string str = this.ConvertCellValueToBoolean() ? XSSFCell.TRUE_AS_STRING : XSSFCell.FALSE_AS_STRING;
          this._cell.t = ST_CellType.b;
          this._cell.v = str;
          break;
        case CellType.ERROR:
          this._cell.t = ST_CellType.e;
          break;
        default:
          throw new ArgumentException("Illegal cell type: " + (object) cellType);
      }
      if (cellType == CellType.FORMULA || !this._cell.IsSetF())
        return;
      this._cell.unsetF();
    }

    public override string ToString()
    {
      switch (this.CellType)
      {
        case CellType.NUMERIC:
          if (DateUtil.IsCellDateFormatted((ICell) this))
            return new SimpleDateFormat("dd-MMM-yyyy").Format((object) this.DateCellValue, CultureInfo.CurrentCulture);
          return this.NumericCellValue.ToString() + "";
        case CellType.STRING:
          return this.RichStringCellValue.ToString();
        case CellType.FORMULA:
          return this.CellFormula;
        case CellType.BLANK:
          return "";
        case CellType.BOOLEAN:
          return !this.BooleanCellValue ? "FALSE" : "TRUE";
        case CellType.ERROR:
          return ErrorEval.GetText((int) this.ErrorCellValue);
        default:
          return "Unknown Cell Type: " + (object) this.CellType;
      }
    }

    public string GetRawValue()
    {
      return this._cell.v;
    }

    private static string GetCellTypeName(CellType cellTypeCode)
    {
      switch (cellTypeCode)
      {
        case CellType.NUMERIC:
          return "numeric";
        case CellType.STRING:
          return "text";
        case CellType.FORMULA:
          return "formula";
        case CellType.BLANK:
          return "blank";
        case CellType.BOOLEAN:
          return "bool";
        case CellType.ERROR:
          return "error";
        default:
          return "#unknown cell type (" + (object) cellTypeCode + ")#";
      }
    }

    private static Exception TypeMismatch(CellType expectedTypeCode, CellType actualTypeCode, bool IsFormulaCell)
    {
      return (Exception) new InvalidOperationException("Cannot get a " + XSSFCell.GetCellTypeName(expectedTypeCode) + " value from a " + XSSFCell.GetCellTypeName(actualTypeCode) + " " + (IsFormulaCell ? "formula " : "") + "cell");
    }

    private static void CheckBounds(int cellIndex)
    {
      SpreadsheetVersion exceL2007 = SpreadsheetVersion.EXCEL2007;
      int lastColumnIndex = SpreadsheetVersion.EXCEL2007.LastColumnIndex;
      if (cellIndex < 0 || cellIndex > lastColumnIndex)
        throw new ArgumentException("Invalid column index (" + (object) cellIndex + ").  Allowable column range for " + exceL2007.ToString() + " is (0.." + (object) lastColumnIndex + ") or ('A'..'" + exceL2007.LastColumnName + "')");
    }

    public IComment CellComment
    {
      get
      {
        return this.Sheet.GetCellComment(this._row.RowNum, this.ColumnIndex);
      }
      set
      {
        if (value == null)
        {
          this.RemoveCellComment();
        }
        else
        {
          value.Row = this.RowIndex;
          value.Column = this.ColumnIndex;
        }
      }
    }

    public void RemoveCellComment()
    {
      if (this.CellComment == null)
        return;
      string r = this._cell.r;
      XSSFSheet sheet = (XSSFSheet) this.Sheet;
      sheet.GetCommentsTable(false).RemoveComment(r);
      sheet.GetVMLDrawing(false).RemoveCommentShape(this.RowIndex, this.ColumnIndex);
    }

    public IHyperlink Hyperlink
    {
      get
      {
        return (IHyperlink) ((XSSFSheet) this.Sheet).GetHyperlink(this._row.RowNum, this._cellNum);
      }
      set
      {
        XSSFHyperlink hyperlink = (XSSFHyperlink) value;
        hyperlink.SetCellReference(new CellReference(this._row.RowNum, this._cellNum).FormatAsString());
        ((XSSFSheet) this.Sheet).AddHyperlink(hyperlink);
      }
    }

    internal CT_Cell GetCTCell()
    {
      return this._cell;
    }

    private bool ConvertCellValueToBoolean()
    {
      CellType cellType = this.CellType;
      if (cellType == CellType.FORMULA)
        cellType = this.GetBaseCellType(false);
      switch (cellType)
      {
        case CellType.NUMERIC:
          return double.Parse(this._cell.v, (IFormatProvider) CultureInfo.InvariantCulture) != 0.0;
        case CellType.STRING:
          return bool.Parse(new XSSFRichTextString(this._sharedStringSource.GetEntryAt(int.Parse(this._cell.v))).String);
        case CellType.BLANK:
        case CellType.ERROR:
          return false;
        case CellType.BOOLEAN:
          return XSSFCell.TRUE_AS_STRING.Equals(this._cell.v);
        default:
          throw new RuntimeException("Unexpected cell type (" + (object) cellType + ")");
      }
    }

    private string ConvertCellValueToString()
    {
      CellType cellType = this.CellType;
      switch (cellType)
      {
        case CellType.NUMERIC:
        case CellType.ERROR:
          return this._cell.v;
        case CellType.STRING:
          return new XSSFRichTextString(this._sharedStringSource.GetEntryAt(int.Parse(this._cell.v))).String;
        case CellType.FORMULA:
          CellType baseCellType = this.GetBaseCellType(false);
          string v = this._cell.v;
          switch (baseCellType)
          {
            case CellType.NUMERIC:
            case CellType.STRING:
            case CellType.ERROR:
              return v;
            case CellType.BOOLEAN:
              if (XSSFCell.TRUE_AS_STRING.Equals(v))
                return "TRUE";
              if (XSSFCell.FALSE_AS_STRING.Equals(v))
                return "FALSE";
              throw new InvalidOperationException("Unexpected bool cached formula value '" + v + "'.");
            default:
              throw new InvalidOperationException("Unexpected formula result type (" + (object) baseCellType + ")");
          }
        case CellType.BLANK:
          return "";
        case CellType.BOOLEAN:
          return !XSSFCell.TRUE_AS_STRING.Equals(this._cell.v) ? "FALSE" : "TRUE";
        default:
          throw new InvalidOperationException("Unexpected cell type (" + (object) cellType + ")");
      }
    }

    public CellRangeAddress ArrayFormulaRange
    {
      get
      {
        XSSFCell cellInArrayFormula = ((XSSFSheet) this.Sheet).GetFirstCellInArrayFormula((ICell) this);
        if (cellInArrayFormula == null)
          throw new InvalidOperationException("Cell " + this._cell.r + " is not part of an array formula.");
        return CellRangeAddress.ValueOf(cellInArrayFormula._cell.f.@ref);
      }
    }

    public bool IsPartOfArrayFormulaGroup
    {
      get
      {
        return ((XSSFSheet) this.Sheet).IsCellInArrayFormulaContext((ICell) this);
      }
    }

    internal void NotifyArrayFormulaChanging(string msg)
    {
      if (!this.IsPartOfArrayFormulaGroup)
        return;
      if (this.ArrayFormulaRange.NumberOfCells > 1)
        throw new InvalidOperationException(msg);
      this.Row.Sheet.RemoveArrayFormula((ICell) this);
    }

    internal void NotifyArrayFormulaChanging()
    {
      this.NotifyArrayFormulaChanging("Cell " + new CellReference((ICell) this).FormatAsString() + " is part of a multi-cell array formula. You cannot change part of an array.");
    }

    public bool IsMergedCell
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public ICell CopyCellTo(int targetIndex)
    {
      return CellUtil.CopyCell(this.Row, this.ColumnIndex, targetIndex);
    }
  }
}
