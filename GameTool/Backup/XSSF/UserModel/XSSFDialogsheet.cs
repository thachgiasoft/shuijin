// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFDialogsheet
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NPOI.XSSF.UserModel
{
  public class XSSFDialogsheet : XSSFSheet, ISheet
  {
    protected CT_Dialogsheet dialogsheet;

    public XSSFDialogsheet(XSSFSheet sheet)
      : base(sheet.GetPackagePart(), sheet.GetPackageRelationship())
    {
      this.dialogsheet = new CT_Dialogsheet();
      this.worksheet = new CT_Worksheet();
    }

    public override IRow CreateRow(int rowNum)
    {
      return (IRow) null;
    }

    protected CT_HeaderFooter GetSheetTypeHeaderFooter()
    {
      if (this.dialogsheet.headerFooter == null)
        this.dialogsheet.headerFooter = new CT_HeaderFooter();
      return this.dialogsheet.headerFooter;
    }

    protected CT_SheetPr GetSheetTypeSheetPr()
    {
      if (this.dialogsheet.sheetPr == null)
        this.dialogsheet.sheetPr = new CT_SheetPr();
      return this.dialogsheet.sheetPr;
    }

    protected CT_PageBreak GetSheetTypeColumnBreaks()
    {
      return (CT_PageBreak) null;
    }

    protected CT_SheetFormatPr GetSheetTypeSheetFormatPr()
    {
      if (this.dialogsheet.sheetFormatPr == null)
        this.dialogsheet.sheetFormatPr = new CT_SheetFormatPr();
      return this.dialogsheet.sheetFormatPr;
    }

    protected CT_PageMargins GetSheetTypePageMargins()
    {
      if (this.dialogsheet.pageMargins == null)
        this.dialogsheet.pageMargins = new CT_PageMargins();
      return this.dialogsheet.pageMargins;
    }

    protected CT_PageBreak GetSheetTypeRowBreaks()
    {
      return (CT_PageBreak) null;
    }

    protected CT_SheetViews GetSheetTypeSheetViews()
    {
      if (this.dialogsheet.sheetViews == null)
      {
        this.dialogsheet.sheetViews = new CT_SheetViews();
        this.dialogsheet.sheetViews.AddNewSheetView();
      }
      return this.dialogsheet.sheetViews;
    }

    protected CT_PrintOptions GetSheetTypePrintOptions()
    {
      if (this.dialogsheet.printOptions == null)
        this.dialogsheet.printOptions = new CT_PrintOptions();
      return this.dialogsheet.printOptions;
    }

    protected CT_SheetProtection GetSheetTypeProtection()
    {
      if (this.dialogsheet.sheetProtection == null)
        this.dialogsheet.sheetProtection = new CT_SheetProtection();
      return this.dialogsheet.sheetProtection;
    }

    public bool GetDialog()
    {
      return true;
    }

    IRow ISheet.CreateRow(int rownum)
    {
      throw new NotImplementedException();
    }

    void ISheet.RemoveRow(IRow row)
    {
      throw new NotImplementedException();
    }

    IRow ISheet.GetRow(int rownum)
    {
      throw new NotImplementedException();
    }

    int ISheet.PhysicalNumberOfRows
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    int ISheet.FirstRowNum
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    int ISheet.LastRowNum
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.ForceFormulaRecalculation
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    void ISheet.SetColumnHidden(int columnIndex, bool hidden)
    {
      throw new NotImplementedException();
    }

    bool ISheet.IsColumnHidden(int columnIndex)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetColumnWidth(int columnIndex, int width)
    {
      throw new NotImplementedException();
    }

    int ISheet.GetColumnWidth(int columnIndex)
    {
      throw new NotImplementedException();
    }

    int ISheet.DefaultColumnWidth
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    short ISheet.DefaultRowHeight
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    float ISheet.DefaultRowHeightInPoints
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    ICellStyle ISheet.GetColumnStyle(int column)
    {
      throw new NotImplementedException();
    }

    int ISheet.AddMergedRegion(CellRangeAddress region)
    {
      throw new NotImplementedException();
    }

    bool ISheet.HorizontallyCenter
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.VerticallyCenter
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    void ISheet.RemoveMergedRegion(int index)
    {
      throw new NotImplementedException();
    }

    int ISheet.NumMergedRegions
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    CellRangeAddress ISheet.GetMergedRegion(int index)
    {
      throw new NotImplementedException();
    }

    IEnumerator ISheet.GetRowEnumerator()
    {
      throw new NotImplementedException();
    }

    IEnumerator ISheet.GetEnumerator()
    {
      throw new NotImplementedException();
    }

    bool ISheet.DisplayZeros
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.Autobreaks
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.DisplayGuts
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.FitToPage
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.RowSumsBelow
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.RowSumsRight
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.IsPrintGridlines
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    IPrintSetup ISheet.PrintSetup
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    IHeader ISheet.Header
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    IFooter ISheet.Footer
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    double ISheet.GetMargin(MarginType margin)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetMargin(MarginType margin, double size)
    {
      throw new NotImplementedException();
    }

    bool ISheet.Protect
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    void ISheet.ProtectSheet(string password)
    {
      throw new NotImplementedException();
    }

    bool ISheet.ScenarioProtect
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    short ISheet.TabColorIndex
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    IDrawing ISheet.DrawingPatriarch
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    void ISheet.SetZoom(int numerator, int denominator)
    {
      throw new NotImplementedException();
    }

    short ISheet.TopRow
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    short ISheet.LeftCol
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    void ISheet.ShowInPane(short toprow, short leftcol)
    {
      throw new NotImplementedException();
    }

    void ISheet.ShiftRows(int startRow, int endRow, int n)
    {
      throw new NotImplementedException();
    }

    void ISheet.ShiftRows(int startRow, int endRow, int n, bool copyRowHeight, bool resetOriginalRowHeight)
    {
      throw new NotImplementedException();
    }

    void ISheet.CreateFreezePane(int colSplit, int rowSplit, int leftmostColumn, int topRow)
    {
      throw new NotImplementedException();
    }

    void ISheet.CreateFreezePane(int colSplit, int rowSplit)
    {
      throw new NotImplementedException();
    }

    void ISheet.CreateSplitPane(int xSplitPos, int ySplitPos, int leftmostColumn, int topRow, PanePosition activePane)
    {
      throw new NotImplementedException();
    }

    PaneInformation ISheet.PaneInformation
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.DisplayGridlines
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.DisplayFormulas
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.DisplayRowColHeadings
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.IsActive
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.IsRowBroken(int row)
    {
      throw new NotImplementedException();
    }

    void ISheet.RemoveRowBreak(int row)
    {
      throw new NotImplementedException();
    }

    int[] ISheet.RowBreaks
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    int[] ISheet.ColumnBreaks
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    void ISheet.SetActiveCell(int row, int column)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetActiveCellRange(int firstRow, int lastRow, int firstColumn, int lastColumn)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetActiveCellRange(List<CellRangeAddress8Bit> cellranges, int activeRange, int activeRow, int activeColumn)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetColumnBreak(int column)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetRowBreak(int row)
    {
      throw new NotImplementedException();
    }

    bool ISheet.IsColumnBroken(int column)
    {
      throw new NotImplementedException();
    }

    void ISheet.RemoveColumnBreak(int column)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetColumnGroupCollapsed(int columnNumber, bool collapsed)
    {
      throw new NotImplementedException();
    }

    void ISheet.GroupColumn(int fromColumn, int toColumn)
    {
      throw new NotImplementedException();
    }

    void ISheet.UngroupColumn(int fromColumn, int toColumn)
    {
      throw new NotImplementedException();
    }

    void ISheet.GroupRow(int fromRow, int toRow)
    {
      throw new NotImplementedException();
    }

    void ISheet.UngroupRow(int fromRow, int toRow)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetRowGroupCollapsed(int row, bool collapse)
    {
      throw new NotImplementedException();
    }

    void ISheet.SetDefaultColumnStyle(int column, ICellStyle style)
    {
      throw new NotImplementedException();
    }

    void ISheet.AutoSizeColumn(int column)
    {
      throw new NotImplementedException();
    }

    void ISheet.AutoSizeColumn(int column, bool useMergedCells)
    {
      throw new NotImplementedException();
    }

    IComment ISheet.GetCellComment(int row, int column)
    {
      throw new NotImplementedException();
    }

    IDrawing ISheet.CreateDrawingPatriarch()
    {
      throw new NotImplementedException();
    }

    IWorkbook ISheet.Workbook
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    string ISheet.SheetName
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    bool ISheet.IsSelected
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    void ISheet.SetActive(bool sel)
    {
      throw new NotImplementedException();
    }

    ICellRange<ICell> ISheet.SetArrayFormula(string formula, CellRangeAddress range)
    {
      throw new NotImplementedException();
    }

    ICellRange<ICell> ISheet.RemoveArrayFormula(ICell cell)
    {
      throw new NotImplementedException();
    }

    bool ISheet.IsMergedRegion(CellRangeAddress mergedRegion)
    {
      throw new NotImplementedException();
    }

    IDataValidationHelper ISheet.GetDataValidationHelper()
    {
      throw new NotImplementedException();
    }

    void ISheet.AddValidationData(IDataValidation dataValidation)
    {
      throw new NotImplementedException();
    }

    IAutoFilter ISheet.SetAutoFilter(CellRangeAddress range)
    {
      throw new NotImplementedException();
    }

    ISheetConditionalFormatting ISheet.SheetConditionalFormatting
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    private new bool IsRightToLeft
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }
  }
}
