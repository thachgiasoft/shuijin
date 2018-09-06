// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFSheet
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.HSSF.Record;
using NPOI.OpenXml4Net.Exceptions;
using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.OpenXmlFormats.Vml;
using NPOI.SS;
using NPOI.SS.Formula;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.Util;
using NPOI.XSSF.Model;
using NPOI.XSSF.UserModel.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NPOI.XSSF.UserModel
{
  public class XSSFSheet : POIXMLDocumentPart, ISheet
  {
    private static POILogger logger = POILogFactory.GetLogger(typeof (XSSFSheet));
    internal CT_Sheet sheet;
    internal CT_Worksheet worksheet;
    private SortedDictionary<int, XSSFRow> _rows;
    private List<XSSFHyperlink> hyperlinks;
    private ColumnHelper columnHelper;
    private CommentsTable sheetComments;
    private Dictionary<int, CT_CellFormula> sharedFormulas;
    private Dictionary<string, XSSFTable> tables;
    private List<CellRangeAddress> arrayFormulas;
    private XSSFDataValidationHelper dataValidationHelper;

    public XSSFSheet()
    {
      this.dataValidationHelper = new XSSFDataValidationHelper(this);
      this.OnDocumentCreate();
    }

    internal XSSFSheet(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.dataValidationHelper = new XSSFDataValidationHelper(this);
    }

    public IWorkbook Workbook
    {
      get
      {
        return (IWorkbook) this.GetParent();
      }
    }

    internal override void OnDocumentRead()
    {
      try
      {
        this.Read(this.GetPackagePart().GetInputStream());
      }
      catch (IOException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
    }

    internal virtual void Read(Stream is1)
    {
      this.worksheet = WorksheetDocument.Parse(is1).GetWorksheet();
      this.InitRows(this.worksheet);
      this.columnHelper = new ColumnHelper(this.worksheet);
      foreach (POIXMLDocumentPart relation in this.GetRelations())
      {
        if (relation is CommentsTable)
        {
          this.sheetComments = (CommentsTable) relation;
          break;
        }
        if (relation is XSSFTable)
          this.tables[relation.GetPackageRelationship().Id] = (XSSFTable) relation;
      }
      this.InitHyperlinks();
    }

    internal override void OnDocumentCreate()
    {
      this.worksheet = XSSFSheet.NewSheet();
      this.InitRows(this.worksheet);
      this.columnHelper = new ColumnHelper(this.worksheet);
      this.hyperlinks = new List<XSSFHyperlink>();
    }

    private void InitRows(CT_Worksheet worksheet)
    {
      this._rows = new SortedDictionary<int, XSSFRow>();
      this.tables = new Dictionary<string, XSSFTable>();
      this.sharedFormulas = new Dictionary<int, CT_CellFormula>();
      this.arrayFormulas = new List<CellRangeAddress>();
      if (0 >= worksheet.sheetData.SizeOfRowArray())
        return;
      foreach (CT_Row row in worksheet.sheetData.row)
      {
        XSSFRow xssfRow = new XSSFRow(row, this);
        if (!this._rows.ContainsKey(xssfRow.RowNum))
          this._rows.Add(xssfRow.RowNum, xssfRow);
      }
    }

    private void InitHyperlinks()
    {
      this.hyperlinks = new List<XSSFHyperlink>();
      if (!this.worksheet.IsSetHyperlinks())
        return;
      try
      {
        PackageRelationshipCollection relationshipsByType = this.GetPackagePart().GetRelationshipsByType(XSSFRelation.SHEET_HYPERLINKS.Relation);
        foreach (CT_Hyperlink ctHyperlink in this.worksheet.hyperlinks.hyperlink)
        {
          PackageRelationship hyperlinkRel = (PackageRelationship) null;
          if (ctHyperlink.id != null)
            hyperlinkRel = relationshipsByType.GetRelationshipByID(ctHyperlink.id);
          this.hyperlinks.Add(new XSSFHyperlink(ctHyperlink, hyperlinkRel));
        }
      }
      catch (InvalidFormatException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
    }

    private static CT_Worksheet NewSheet()
    {
      CT_Worksheet ctWorksheet = new CT_Worksheet();
      ctWorksheet.AddNewSheetFormatPr().defaultRowHeight = 15.0;
      ctWorksheet.AddNewSheetViews().AddNewSheetView().workbookViewId = 0U;
      ctWorksheet.AddNewDimension().@ref = "A1";
      ctWorksheet.AddNewSheetData();
      CT_PageMargins ctPageMargins = ctWorksheet.AddNewPageMargins();
      ctPageMargins.bottom = 0.75;
      ctPageMargins.footer = 0.3;
      ctPageMargins.header = 0.3;
      ctPageMargins.left = 0.7;
      ctPageMargins.right = 0.7;
      ctPageMargins.top = 0.75;
      return ctWorksheet;
    }

    internal CT_Worksheet GetCTWorksheet()
    {
      return this.worksheet;
    }

    public ColumnHelper GetColumnHelper()
    {
      return this.columnHelper;
    }

    public string SheetName
    {
      get
      {
        return this.sheet.name;
      }
    }

    public int AddMergedRegion(CellRangeAddress region)
    {
      region.Validate(SpreadsheetVersion.EXCEL2007);
      this.ValidateArrayFormulas(region);
      CT_MergeCells ctMergeCells = this.worksheet.IsSetMergeCells() ? this.worksheet.mergeCells : this.worksheet.AddNewMergeCells();
      ctMergeCells.AddNewMergeCell().@ref = region.FormatAsString();
      return ctMergeCells.sizeOfMergeCellArray();
    }

    private void ValidateArrayFormulas(CellRangeAddress region)
    {
      int firstRow = region.FirstRow;
      int firstColumn = region.FirstColumn;
      int lastRow = region.LastRow;
      int lastColumn = region.LastColumn;
      for (int rownum = firstRow; rownum <= lastRow; ++rownum)
      {
        for (int cellnum = firstColumn; cellnum <= lastColumn; ++cellnum)
        {
          IRow row = this.GetRow(rownum);
          if (row != null)
          {
            ICell cell = row.GetCell(cellnum);
            if (cell != null && cell.IsPartOfArrayFormulaGroup)
            {
              CellRangeAddress arrayFormulaRange = cell.ArrayFormulaRange;
              if (arrayFormulaRange.NumberOfCells > 1 && (arrayFormulaRange.IsInRange(region.FirstRow, region.FirstColumn) || arrayFormulaRange.IsInRange(region.FirstRow, region.FirstColumn)))
                throw new InvalidOperationException("The range " + region.FormatAsString() + " intersects with a multi-cell array formula. You cannot merge cells of an array.");
            }
          }
        }
      }
    }

    public void AutoSizeColumn(int column)
    {
      this.AutoSizeColumn(column, false);
    }

    public void AutoSizeColumn(int column, bool useMergedCells)
    {
      double columnWidth = SheetUtil.GetColumnWidth((ISheet) this, column, useMergedCells);
      if (columnWidth == -1.0)
        return;
      double num1 = columnWidth * 256.0;
      int num2 = 65280;
      if (num1 > (double) num2)
        num1 = (double) num2;
      this.SetColumnWidth(column, (int) num1);
      this.columnHelper.SetColBestFit((long) column, true);
    }

    public IDrawing CreateDrawingPatriarch()
    {
      XSSFDrawing xssfDrawing1 = (XSSFDrawing) null;
      CT_Drawing ctDrawing = this.GetCTDrawing();
      if (ctDrawing == null)
      {
        int idx = this.GetPackagePart().Package.GetPartsByContentType(XSSFRelation.DRAWINGS.ContentType).Count + 1;
        xssfDrawing1 = (XSSFDrawing) this.CreateRelationship((POIXMLRelation) XSSFRelation.DRAWINGS, (POIXMLFactory) XSSFFactory.GetInstance(), idx);
        this.worksheet.AddNewDrawing().id = xssfDrawing1.GetPackageRelationship().Id;
      }
      else
      {
        foreach (POIXMLDocumentPart relation in this.GetRelations())
        {
          if (relation is XSSFDrawing)
          {
            XSSFDrawing xssfDrawing2 = (XSSFDrawing) relation;
            if (xssfDrawing2.GetPackageRelationship().Id.Equals(ctDrawing.id))
            {
              xssfDrawing1 = xssfDrawing2;
              break;
            }
            break;
          }
        }
        if (xssfDrawing1 == null)
          XSSFSheet.logger.Log(7, (object) ("Can't find drawing with id=" + ctDrawing.id + " in the list of the sheet's relationships"));
      }
      return (IDrawing) xssfDrawing1;
    }

    internal XSSFVMLDrawing GetVMLDrawing(bool autoCreate)
    {
      XSSFVMLDrawing xssfvmlDrawing1 = (XSSFVMLDrawing) null;
      CT_LegacyDrawing ctLegacyDrawing = this.GetCTLegacyDrawing();
      if (ctLegacyDrawing == null)
      {
        if (autoCreate)
        {
          int idx = this.GetPackagePart().Package.GetPartsByContentType(XSSFRelation.VML_DRAWINGS.ContentType).Count + 1;
          xssfvmlDrawing1 = (XSSFVMLDrawing) this.CreateRelationship((POIXMLRelation) XSSFRelation.VML_DRAWINGS, (POIXMLFactory) XSSFFactory.GetInstance(), idx);
          this.worksheet.AddNewLegacyDrawing().id = xssfvmlDrawing1.GetPackageRelationship().Id;
        }
      }
      else
      {
        foreach (POIXMLDocumentPart relation in this.GetRelations())
        {
          if (relation is XSSFVMLDrawing)
          {
            XSSFVMLDrawing xssfvmlDrawing2 = (XSSFVMLDrawing) relation;
            if (xssfvmlDrawing2.GetPackageRelationship().Id.Equals(ctLegacyDrawing.id))
            {
              xssfvmlDrawing1 = xssfvmlDrawing2;
              break;
            }
            break;
          }
        }
        if (xssfvmlDrawing1 == null)
          XSSFSheet.logger.Log(7, (object) ("Can't find VML drawing with id=" + ctLegacyDrawing.id + " in the list of the sheet's relationships"));
      }
      return xssfvmlDrawing1;
    }

    protected virtual CT_Drawing GetCTDrawing()
    {
      return this.worksheet.drawing;
    }

    protected virtual CT_LegacyDrawing GetCTLegacyDrawing()
    {
      return this.worksheet.legacyDrawing;
    }

    public void CreateFreezePane(int colSplit, int rowSplit)
    {
      this.CreateFreezePane(colSplit, rowSplit, colSplit, rowSplit);
    }

    public void CreateFreezePane(int colSplit, int rowSplit, int leftmostColumn, int topRow)
    {
      CT_SheetView defaultSheetView = this.GetDefaultSheetView();
      if (colSplit == 0 && rowSplit == 0)
      {
        if (defaultSheetView.IsSetPane())
          defaultSheetView.UnsetPane();
        defaultSheetView.SetSelectionArray((List<CT_Selection>) null);
      }
      else
      {
        if (!defaultSheetView.IsSetPane())
          defaultSheetView.AddNewPane();
        CT_Pane pane = defaultSheetView.pane;
        if (colSplit > 0)
          pane.xSplit = (double) colSplit;
        else if (pane.IsSetXSplit())
          pane.UnsetXSplit();
        if (rowSplit > 0)
          pane.ySplit = (double) rowSplit;
        else if (pane.IsSetYSplit())
          pane.UnsetYSplit();
        pane.state = ST_PaneState.frozen;
        if (rowSplit == 0)
        {
          pane.topLeftCell = new CellReference(0, leftmostColumn).FormatAsString();
          pane.activePane = ST_Pane.topRight;
        }
        else if (colSplit == 0)
        {
          pane.topLeftCell = new CellReference(topRow, 0).FormatAsString();
          pane.activePane = ST_Pane.bottomLeft;
        }
        else
        {
          pane.topLeftCell = new CellReference(topRow, leftmostColumn).FormatAsString();
          pane.activePane = ST_Pane.bottomRight;
        }
        defaultSheetView.selection = (List<CT_Selection>) null;
        defaultSheetView.AddNewSelection().pane = pane.activePane;
      }
    }

    public IComment CreateComment()
    {
      return this.CreateDrawingPatriarch().CreateCellComment((IClientAnchor) new XSSFClientAnchor());
    }

    private int GetLastKey(SortedDictionary<int, XSSFRow>.KeyCollection keys)
    {
      int num = 0;
      foreach (int key in keys)
      {
        if (num == keys.Count - 1)
          return key;
        ++num;
      }
      throw new ArgumentOutOfRangeException();
    }

    private SortedDictionary<int, XSSFRow> HeadMap(SortedDictionary<int, XSSFRow> rows, int rownum)
    {
      SortedDictionary<int, XSSFRow> sortedDictionary = new SortedDictionary<int, XSSFRow>();
      foreach (int key in rows.Keys)
      {
        if (key < rownum)
          sortedDictionary.Add(key, rows[key]);
      }
      return sortedDictionary;
    }

    public virtual IRow CreateRow(int rownum)
    {
      XSSFRow xssfRow1 = this._rows.ContainsKey(rownum) ? this._rows[rownum] : (XSSFRow) null;
      CT_Row row;
      if (xssfRow1 != null)
      {
        row = xssfRow1.GetCTRow();
        row.Set(new CT_Row());
      }
      else
        row = this._rows.Count == 0 || rownum > this.GetLastKey(this._rows.Keys) ? this.worksheet.sheetData.AddNewRow() : this.worksheet.sheetData.InsertNewRow(this.HeadMap(this._rows, rownum).Count);
      XSSFRow xssfRow2 = new XSSFRow(row, this);
      xssfRow2.RowNum = rownum;
      this._rows[rownum] = xssfRow2;
      return (IRow) xssfRow2;
    }

    public void CreateSplitPane(int xSplitPos, int ySplitPos, int leftmostColumn, int topRow, PanePosition activePane)
    {
      this.CreateFreezePane(xSplitPos, ySplitPos, leftmostColumn, topRow);
      this.GetPane().state = ST_PaneState.split;
      this.GetPane().activePane = (ST_Pane) activePane;
    }

    public IComment GetCellComment(int row, int column)
    {
      if (this.sheetComments == null)
        return (IComment) null;
      CT_Comment ctComment = this.sheetComments.GetCTComment(new CellReference(row, column).FormatAsString());
      if (ctComment == null)
        return (IComment) null;
      XSSFVMLDrawing vmlDrawing = this.GetVMLDrawing(false);
      return (IComment) new XSSFComment(this.sheetComments, ctComment, vmlDrawing == null ? (CT_Shape) null : vmlDrawing.FindCommentShape(row, column));
    }

    public XSSFHyperlink GetHyperlink(int row, int column)
    {
      string str = new CellReference(row, column).FormatAsString();
      foreach (XSSFHyperlink hyperlink in this.hyperlinks)
      {
        if (hyperlink.GetCellRef().Equals(str))
          return hyperlink;
      }
      return (XSSFHyperlink) null;
    }

    public int[] ColumnBreaks
    {
      get
      {
        if (!this.worksheet.IsSetColBreaks() || this.worksheet.colBreaks.sizeOfBrkArray() == 0)
          return new int[0];
        List<CT_Break> brk = this.worksheet.colBreaks.brk;
        int[] numArray = new int[brk.Count];
        for (int index = 0; index < brk.Count; ++index)
        {
          CT_Break ctBreak = brk[index];
          numArray[index] = (int) ctBreak.id - 1;
        }
        return numArray;
      }
    }

    public int GetColumnWidth(int columnIndex)
    {
      CT_Col column = this.columnHelper.GetColumn((long) columnIndex, false);
      return (int) ((column == null || !column.IsSetWidth() ? (double) this.DefaultColumnWidth : column.width) * 256.0);
    }

    public int DefaultColumnWidth
    {
      get
      {
        CT_SheetFormatPr sheetFormatPr = this.worksheet.sheetFormatPr;
        if (sheetFormatPr != null)
          return (int) sheetFormatPr.baseColWidth;
        return 8;
      }
      set
      {
        this.GetSheetTypeSheetFormatPr().baseColWidth = (uint) value;
      }
    }

    public short DefaultRowHeight
    {
      get
      {
        return (short) ((Decimal) this.DefaultRowHeightInPoints * new Decimal(20));
      }
      set
      {
        this.DefaultRowHeightInPoints = (float) value / 20f;
      }
    }

    public float DefaultRowHeightInPoints
    {
      get
      {
        CT_SheetFormatPr sheetFormatPr = this.worksheet.sheetFormatPr;
        return sheetFormatPr == null ? 0.0f : (float) sheetFormatPr.defaultRowHeight;
      }
      set
      {
        CT_SheetFormatPr typeSheetFormatPr = this.GetSheetTypeSheetFormatPr();
        typeSheetFormatPr.defaultRowHeight = (double) value;
        typeSheetFormatPr.customHeight = true;
      }
    }

    private CT_SheetFormatPr GetSheetTypeSheetFormatPr()
    {
      if (!this.worksheet.IsSetSheetFormatPr())
        return this.worksheet.AddNewSheetFormatPr();
      return this.worksheet.sheetFormatPr;
    }

    public ICellStyle GetColumnStyle(int column)
    {
      int colDefaultStyle = this.columnHelper.GetColDefaultStyle((long) column);
      return this.Workbook.GetCellStyleAt(colDefaultStyle == -1 ? (short) 0 : (short) colDefaultStyle);
    }

    public bool RightToLeft
    {
      get
      {
        CT_SheetView defaultSheetView = this.GetDefaultSheetView();
        if (defaultSheetView != null)
          return defaultSheetView.rightToLeft;
        return false;
      }
      set
      {
        this.GetDefaultSheetView().rightToLeft = value;
      }
    }

    public bool DisplayGuts
    {
      get
      {
        CT_SheetPr sheetTypeSheetPr = this.GetSheetTypeSheetPr();
        return (sheetTypeSheetPr.outlinePr == null ? new CT_OutlinePr() : sheetTypeSheetPr.outlinePr).showOutlineSymbols;
      }
      set
      {
        CT_SheetPr sheetTypeSheetPr = this.GetSheetTypeSheetPr();
        (sheetTypeSheetPr.outlinePr == null ? sheetTypeSheetPr.AddNewOutlinePr() : sheetTypeSheetPr.outlinePr).showOutlineSymbols = value;
      }
    }

    public bool DisplayZeros
    {
      get
      {
        CT_SheetView defaultSheetView = this.GetDefaultSheetView();
        if (defaultSheetView != null)
          return defaultSheetView.showZeros;
        return true;
      }
      set
      {
        this.GetSheetTypeSheetView().showZeros = value;
      }
    }

    public int FirstRowNum
    {
      get
      {
        if (this._rows.Count == 0)
          return 0;
        using (SortedDictionary<int, XSSFRow>.KeyCollection.Enumerator enumerator = this._rows.Keys.GetEnumerator())
        {
          if (enumerator.MoveNext())
            return enumerator.Current;
        }
        throw new ArgumentOutOfRangeException();
      }
    }

    public bool FitToPage
    {
      get
      {
        CT_SheetPr sheetTypeSheetPr = this.GetSheetTypeSheetPr();
        return (sheetTypeSheetPr == null || !sheetTypeSheetPr.IsSetPageSetUpPr() ? new CT_PageSetUpPr() : sheetTypeSheetPr.pageSetUpPr).fitToPage;
      }
      set
      {
        this.GetSheetTypePageSetUpPr().fitToPage = value;
      }
    }

    private CT_SheetPr GetSheetTypeSheetPr()
    {
      if (this.worksheet.sheetPr == null)
        this.worksheet.sheetPr = new CT_SheetPr();
      return this.worksheet.sheetPr;
    }

    private CT_HeaderFooter GetSheetTypeHeaderFooter()
    {
      if (this.worksheet.headerFooter == null)
        this.worksheet.headerFooter = new CT_HeaderFooter();
      return this.worksheet.headerFooter;
    }

    public IFooter Footer
    {
      get
      {
        return this.OddFooter;
      }
    }

    public IHeader Header
    {
      get
      {
        return this.OddHeader;
      }
    }

    public IFooter OddFooter
    {
      get
      {
        return (IFooter) new XSSFOddFooter(this.GetSheetTypeHeaderFooter());
      }
    }

    public IFooter EvenFooter
    {
      get
      {
        return (IFooter) new XSSFEvenFooter(this.GetSheetTypeHeaderFooter());
      }
    }

    public IFooter FirstFooter
    {
      get
      {
        return (IFooter) new XSSFFirstFooter(this.GetSheetTypeHeaderFooter());
      }
    }

    public IHeader OddHeader
    {
      get
      {
        return (IHeader) new XSSFOddHeader(this.GetSheetTypeHeaderFooter());
      }
    }

    public IHeader EvenHeader
    {
      get
      {
        return (IHeader) new XSSFEvenHeader(this.GetSheetTypeHeaderFooter());
      }
    }

    public IHeader FirstHeader
    {
      get
      {
        return (IHeader) new XSSFFirstHeader(this.GetSheetTypeHeaderFooter());
      }
    }

    public bool HorizontallyCenter
    {
      get
      {
        CT_PrintOptions printOptions = this.worksheet.printOptions;
        if (printOptions != null)
          return printOptions.horizontalCentered;
        return false;
      }
      set
      {
        (this.worksheet.IsSetPrintOptions() ? this.worksheet.printOptions : this.worksheet.AddNewPrintOptions()).horizontalCentered = value;
      }
    }

    public int LastRowNum
    {
      get
      {
        if (this._rows.Count != 0)
          return this.GetLastKey(this._rows.Keys);
        return 0;
      }
    }

    public short LeftCol
    {
      get
      {
        return new CellReference(this.worksheet.sheetViews.GetSheetViewArray(0).topLeftCell).Col;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public double GetMargin(MarginType margin)
    {
      if (!this.worksheet.IsSetPageMargins())
        return 0.0;
      CT_PageMargins pageMargins = this.worksheet.pageMargins;
      switch (margin)
      {
        case MarginType.LeftMargin:
          return pageMargins.left;
        case MarginType.RightMargin:
          return pageMargins.right;
        case MarginType.TopMargin:
          return pageMargins.top;
        case MarginType.BottomMargin:
          return pageMargins.bottom;
        case MarginType.HeaderMargin:
          return pageMargins.header;
        case MarginType.FooterMargin:
          return pageMargins.footer;
        default:
          throw new ArgumentException("Unknown margin constant:  " + (object) margin);
      }
    }

    public void SetMargin(MarginType margin, double size)
    {
      CT_PageMargins ctPageMargins = this.worksheet.IsSetPageMargins() ? this.worksheet.pageMargins : this.worksheet.AddNewPageMargins();
      switch (margin)
      {
        case MarginType.LeftMargin:
          ctPageMargins.left = size;
          break;
        case MarginType.RightMargin:
          ctPageMargins.right = size;
          break;
        case MarginType.TopMargin:
          ctPageMargins.top = size;
          break;
        case MarginType.BottomMargin:
          ctPageMargins.bottom = size;
          break;
        case MarginType.HeaderMargin:
          ctPageMargins.header = size;
          break;
        case MarginType.FooterMargin:
          ctPageMargins.footer = size;
          break;
        default:
          throw new InvalidOperationException("Unknown margin constant:  " + (object) margin);
      }
    }

    public CellRangeAddress GetMergedRegion(int index)
    {
      CT_MergeCells mergeCells = this.worksheet.mergeCells;
      if (mergeCells == null)
        throw new InvalidOperationException("This worksheet does not contain merged regions");
      return CellRangeAddress.ValueOf(mergeCells.GetMergeCellArray(index).@ref);
    }

    public int NumMergedRegions
    {
      get
      {
        CT_MergeCells mergeCells = this.worksheet.mergeCells;
        if (mergeCells != null)
          return mergeCells.sizeOfMergeCellArray();
        return 0;
      }
    }

    public int NumHyperlinks
    {
      get
      {
        return this.hyperlinks.Count;
      }
    }

    public PaneInformation PaneInformation
    {
      get
      {
        CT_Pane pane = this.GetDefaultSheetView().pane;
        if (pane == null)
          return (PaneInformation) null;
        CellReference cellReference = pane.IsSetTopLeftCell() ? new CellReference(pane.topLeftCell) : (CellReference) null;
        return new PaneInformation((short) pane.xSplit, (short) pane.ySplit, cellReference == null ? (short) 0 : (short) cellReference.Row, cellReference == null ? (short) 0 : cellReference.Col, (byte) pane.activePane, pane.state == ST_PaneState.frozen);
      }
    }

    public int PhysicalNumberOfRows
    {
      get
      {
        return this._rows.Count;
      }
    }

    public IPrintSetup PrintSetup
    {
      get
      {
        return (IPrintSetup) new XSSFPrintSetup(this.worksheet);
      }
    }

    public bool Protect
    {
      get
      {
        if (this.worksheet.IsSetSheetProtection())
          return this.sheetProtectionEnabled();
        return false;
      }
    }

    public void ProtectSheet(string password)
    {
      if (password != null)
      {
        CT_SheetProtection ctSheetProtection = this.worksheet.AddNewSheetProtection();
        ctSheetProtection.password = this.StringToExcelPassword(password);
        ctSheetProtection.sheet = true;
        ctSheetProtection.scenarios = true;
        ctSheetProtection.objects = true;
      }
      else
        this.worksheet.UnsetSheetProtection();
    }

    private string StringToExcelPassword(string password)
    {
      return PasswordRecord.HashPassword(password).ToString("x");
    }

    public IRow GetRow(int rownum)
    {
      if (this._rows.ContainsKey(rownum))
        return (IRow) this._rows[rownum];
      return (IRow) null;
    }

    public int[] RowBreaks
    {
      get
      {
        if (!this.worksheet.IsSetRowBreaks() || this.worksheet.rowBreaks.sizeOfBrkArray() == 0)
          return new int[0];
        List<CT_Break> brk = this.worksheet.rowBreaks.brk;
        int[] numArray = new int[brk.Count];
        for (int index = 0; index < brk.Count; ++index)
        {
          CT_Break ctBreak = brk[index];
          numArray[index] = (int) ctBreak.id - 1;
        }
        return numArray;
      }
    }

    public bool RowSumsBelow
    {
      get
      {
        CT_SheetPr sheetPr = this.worksheet.sheetPr;
        CT_OutlinePr ctOutlinePr = sheetPr == null || !sheetPr.IsSetOutlinePr() ? (CT_OutlinePr) null : sheetPr.outlinePr;
        if (ctOutlinePr != null)
          return ctOutlinePr.summaryBelow;
        return true;
      }
      set
      {
        this.ensureOutlinePr().summaryBelow = value;
      }
    }

    public bool RowSumsRight
    {
      get
      {
        CT_SheetPr sheetPr = this.worksheet.sheetPr;
        return (sheetPr == null || !sheetPr.IsSetOutlinePr() ? new CT_OutlinePr() : sheetPr.outlinePr).summaryRight;
      }
      set
      {
        this.ensureOutlinePr().summaryRight = value;
      }
    }

    private CT_OutlinePr ensureOutlinePr()
    {
      CT_SheetPr ctSheetPr = this.worksheet.IsSetSheetPr() ? this.worksheet.sheetPr : this.worksheet.AddNewSheetPr();
      if (!ctSheetPr.IsSetOutlinePr())
        return ctSheetPr.AddNewOutlinePr();
      return ctSheetPr.outlinePr;
    }

    public bool ScenarioProtect
    {
      get
      {
        if (this.worksheet.IsSetSheetProtection())
          return this.worksheet.sheetProtection.scenarios;
        return false;
      }
    }

    public short TopRow
    {
      get
      {
        return (short) new CellReference(this.GetSheetTypeSheetView().topLeftCell).Row;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool VerticallyCenter
    {
      get
      {
        CT_PrintOptions printOptions = this.worksheet.printOptions;
        if (printOptions != null)
          return printOptions.verticalCentered;
        return false;
      }
      set
      {
        (this.worksheet.IsSetPrintOptions() ? this.worksheet.printOptions : this.worksheet.AddNewPrintOptions()).verticalCentered = value;
      }
    }

    public void GroupColumn(int fromColumn, int toColumn)
    {
      this.GroupColumn1Based(fromColumn + 1, toColumn + 1);
    }

    private void GroupColumn1Based(int fromColumn, int toColumn)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      this.columnHelper.AddCleanColIntoCols(colsArray, new CT_Col()
      {
        min = (uint) fromColumn,
        max = (uint) toColumn
      });
      CT_Col column1Based;
      for (int index = fromColumn; index <= toColumn; index = (int) column1Based.max + 1)
      {
        column1Based = this.columnHelper.GetColumn1Based((long) index, false);
        short outlineLevel = (short) column1Based.outlineLevel;
        column1Based.outlineLevel = (byte) ((uint) outlineLevel + 1U);
      }
      this.worksheet.SetColsArray(0, colsArray);
      this.SetSheetFormatPrOutlineLevelCol();
    }

    public void GroupRow(int fromRow, int toRow)
    {
      for (int rownum = fromRow; rownum <= toRow; ++rownum)
      {
        CT_Row ctRow = ((XSSFRow) this.GetRow(rownum) ?? (XSSFRow) this.CreateRow(rownum)).GetCTRow();
        short outlineLevel = (short) ctRow.outlineLevel;
        ctRow.outlineLevel = (byte) ((uint) outlineLevel + 1U);
      }
      this.SetSheetFormatPrOutlineLevelRow();
    }

    private short GetMaxOutlineLevelRows()
    {
      short num = 0;
      foreach (XSSFRow xssfRow in this._rows.Values)
        num = (int) xssfRow.GetCTRow().outlineLevel > (int) num ? (short) xssfRow.GetCTRow().outlineLevel : num;
      return num;
    }

    private short GetMaxOutlineLevelCols()
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      short num = 0;
      foreach (CT_Col ctCol in colsArray.col)
        num = (int) ctCol.outlineLevel > (int) num ? (short) ctCol.outlineLevel : num;
      return num;
    }

    public bool IsColumnBroken(int column)
    {
      foreach (int columnBreak in this.ColumnBreaks)
      {
        if (columnBreak == column)
          return true;
      }
      return false;
    }

    public bool IsColumnHidden(int columnIndex)
    {
      CT_Col column = this.columnHelper.GetColumn((long) columnIndex, false);
      if (column != null)
        return column.hidden;
      return false;
    }

    public bool DisplayFormulas
    {
      get
      {
        return this.GetSheetTypeSheetView().showFormulas;
      }
      set
      {
        this.GetSheetTypeSheetView().showFormulas = value;
      }
    }

    public bool DisplayGridlines
    {
      get
      {
        return this.GetSheetTypeSheetView().showGridLines;
      }
      set
      {
        this.GetSheetTypeSheetView().showGridLines = value;
      }
    }

    public bool DisplayRowColHeadings
    {
      get
      {
        return this.GetSheetTypeSheetView().showRowColHeaders;
      }
      set
      {
        this.GetSheetTypeSheetView().showRowColHeaders = value;
      }
    }

    public bool IsPrintGridlines
    {
      get
      {
        CT_PrintOptions printOptions = this.worksheet.printOptions;
        if (printOptions != null)
          return printOptions.gridLines;
        return false;
      }
      set
      {
        (this.worksheet.IsSetPrintOptions() ? this.worksheet.printOptions : this.worksheet.AddNewPrintOptions()).gridLines = value;
      }
    }

    public bool IsRowBroken(int row)
    {
      foreach (int rowBreak in this.RowBreaks)
      {
        if (rowBreak == row)
          return true;
      }
      return false;
    }

    public void SetRowBreak(int row)
    {
      CT_PageBreak ctPageBreak = this.worksheet.IsSetRowBreaks() ? this.worksheet.rowBreaks : this.worksheet.AddNewRowBreaks();
      if (this.IsRowBroken(row))
        return;
      CT_Break ctBreak = ctPageBreak.AddNewBrk();
      ctBreak.id = (uint) (row + 1);
      ctBreak.man = true;
      ctBreak.max = (uint) SpreadsheetVersion.EXCEL2007.LastColumnIndex;
      ctPageBreak.count = (uint) ctPageBreak.sizeOfBrkArray();
      ctPageBreak.manualBreakCount = (uint) ctPageBreak.sizeOfBrkArray();
    }

    public void RemoveColumnBreak(int column)
    {
      if (!this.worksheet.IsSetColBreaks())
        return;
      CT_PageBreak colBreaks = this.worksheet.colBreaks;
      List<CT_Break> brk = colBreaks.brk;
      for (int index = 0; index < brk.Count; ++index)
      {
        if ((long) brk[index].id == (long) (column + 1))
          colBreaks.RemoveBrk(index);
      }
    }

    public void RemoveMergedRegion(int index)
    {
      CT_MergeCells mergeCells = this.worksheet.mergeCells;
      CT_MergeCell[] array = new CT_MergeCell[mergeCells.sizeOfMergeCellArray() - 1];
      for (int index1 = 0; index1 < mergeCells.sizeOfMergeCellArray(); ++index1)
      {
        if (index1 < index)
          array[index1] = mergeCells.GetMergeCellArray(index1);
        else if (index1 > index)
          array[index1 - 1] = mergeCells.GetMergeCellArray(index1);
      }
      if (array.Length > 0)
        mergeCells.SetMergeCellArray(array);
      else
        this.worksheet.UnsetMergeCells();
    }

    public void RemoveRow(IRow row)
    {
      if (row.Sheet != this)
        throw new ArgumentException("Specified row does not belong to this sheet");
      List<XSSFCell> xssfCellList = new List<XSSFCell>();
      foreach (ICell cell in row)
        xssfCellList.Add((XSSFCell) cell);
      foreach (XSSFCell xssfCell in xssfCellList)
        row.RemoveCell((ICell) xssfCell);
      int count = this.HeadMap(this._rows, row.RowNum).Count;
      this._rows.Remove(row.RowNum);
      this.worksheet.sheetData.RemoveRow(count);
    }

    public void RemoveRowBreak(int row)
    {
      if (!this.worksheet.IsSetRowBreaks())
        return;
      CT_PageBreak rowBreaks = this.worksheet.rowBreaks;
      List<CT_Break> brk = rowBreaks.brk;
      for (int index = 0; index < brk.Count; ++index)
      {
        if ((long) brk[index].id == (long) (row + 1))
          rowBreaks.RemoveBrk(index);
      }
    }

    public bool ForceFormulaRecalculation
    {
      get
      {
        if (this.worksheet.IsSetSheetCalcPr())
          return this.worksheet.sheetCalcPr.fullCalcOnLoad;
        return false;
      }
      set
      {
        CT_CalcPr calcPr = (this.Workbook as XSSFWorkbook).GetCTWorkbook().calcPr;
        if (this.worksheet.IsSetSheetCalcPr())
          this.worksheet.sheetCalcPr.fullCalcOnLoad = value;
        else if (value)
          this.worksheet.AddNewSheetCalcPr().fullCalcOnLoad = value;
        if (!value || calcPr == null || calcPr.calcMode != ST_CalcMode.manual)
          return;
        calcPr.calcMode = ST_CalcMode.auto;
      }
    }

    public bool Autobreaks
    {
      get
      {
        CT_SheetPr sheetTypeSheetPr = this.GetSheetTypeSheetPr();
        return (sheetTypeSheetPr == null || !sheetTypeSheetPr.IsSetPageSetUpPr() ? new CT_PageSetUpPr() : sheetTypeSheetPr.pageSetUpPr).autoPageBreaks;
      }
      set
      {
        CT_SheetPr sheetTypeSheetPr = this.GetSheetTypeSheetPr();
        (sheetTypeSheetPr.IsSetPageSetUpPr() ? sheetTypeSheetPr.pageSetUpPr : sheetTypeSheetPr.AddNewPageSetUpPr()).autoPageBreaks = value;
      }
    }

    public void SetColumnBreak(int column)
    {
      if (this.IsColumnBroken(column))
        return;
      CT_PageBreak ctPageBreak = this.worksheet.IsSetColBreaks() ? this.worksheet.colBreaks : this.worksheet.AddNewColBreaks();
      CT_Break ctBreak = ctPageBreak.AddNewBrk();
      ctBreak.id = (uint) (column + 1);
      ctBreak.man = true;
      ctBreak.max = (uint) SpreadsheetVersion.EXCEL2007.LastRowIndex;
      ctPageBreak.count = (uint) ctPageBreak.sizeOfBrkArray();
      ctPageBreak.manualBreakCount = (uint) ctPageBreak.sizeOfBrkArray();
    }

    public void SetColumnGroupCollapsed(int columnNumber, bool collapsed)
    {
      if (collapsed)
        this.CollapseColumn(columnNumber);
      else
        this.ExpandColumn(columnNumber);
    }

    private void CollapseColumn(int columnNumber)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      CT_Col column = this.columnHelper.GetColumn((long) columnNumber, false);
      int indexOfColumn = this.columnHelper.GetIndexOfColumn(colsArray, column);
      if (indexOfColumn == -1)
        return;
      int columnOutlineGroup = this.FindStartOfColumnOutlineGroup(indexOfColumn);
      CT_Col colArray = colsArray.GetColArray(columnOutlineGroup);
      this.SetColumn(this.SetGroupHidden(columnOutlineGroup, (int) colArray.outlineLevel, true) + 1, new short?(), new int?(0), new int?(), new bool?(), new bool?(true));
    }

    private void SetColumn(int targetColumnIx, short? xfIndex, int? style, int? level, bool? hidden, bool? collapsed)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      CT_Col ctCol1 = (CT_Col) null;
      int num1 = 0;
      int index;
      for (index = 0; index < colsArray.sizeOfColArray(); ++index)
      {
        CT_Col colArray = colsArray.GetColArray(index);
        if ((long) colArray.min >= (long) targetColumnIx && (long) colArray.max <= (long) targetColumnIx)
        {
          ctCol1 = colArray;
          break;
        }
        if ((long) colArray.min > (long) targetColumnIx)
          break;
      }
      if (ctCol1 == null)
      {
        CT_Col ctCol2 = new CT_Col();
        ctCol2.min = (uint) targetColumnIx;
        ctCol2.max = (uint) targetColumnIx;
        this.UnsetCollapsed(collapsed.Value, ctCol2);
        this.columnHelper.AddCleanColIntoCols(colsArray, ctCol2);
      }
      else
      {
        int num2;
        if (style.HasValue)
        {
          long style1 = (long) ctCol1.style;
          int? nullable = style;
          long valueOrDefault = (long) nullable.GetValueOrDefault();
          num2 = style1 != valueOrDefault ? 1 : (!nullable.HasValue ? 1 : 0);
        }
        else
          num2 = 0;
        bool flag1 = num2 != 0;
        int num3;
        if (level.HasValue)
        {
          int outlineLevel = (int) ctCol1.outlineLevel;
          int? nullable = level;
          int valueOrDefault = nullable.GetValueOrDefault();
          num3 = outlineLevel != valueOrDefault ? 1 : (!nullable.HasValue ? 1 : 0);
        }
        else
          num3 = 0;
        bool flag2 = num3 != 0;
        int num4;
        if (hidden.HasValue)
        {
          int num5 = ctCol1.hidden ? 1 : 0;
          bool? nullable = hidden;
          int num6 = nullable.GetValueOrDefault() ? 1 : 0;
          num4 = num5 != num6 ? 1 : (!nullable.HasValue ? 1 : 0);
        }
        else
          num4 = 0;
        bool flag3 = num4 != 0;
        int num7;
        if (collapsed.HasValue)
        {
          int num5 = ctCol1.collapsed ? 1 : 0;
          bool? nullable = collapsed;
          int num6 = nullable.GetValueOrDefault() ? 1 : 0;
          num7 = num5 != num6 ? 1 : (!nullable.HasValue ? 1 : 0);
        }
        else
          num7 = 0;
        bool flag4 = num7 != 0;
        if (!flag2 && !flag3 && !flag4 && !flag1)
          return;
        if ((long) ctCol1.min == (long) targetColumnIx && (long) ctCol1.max == (long) targetColumnIx)
          this.UnsetCollapsed(collapsed.Value, ctCol1);
        else if ((long) ctCol1.min == (long) targetColumnIx || (long) ctCol1.max == (long) targetColumnIx)
        {
          if ((long) ctCol1.min == (long) targetColumnIx)
          {
            ctCol1.min = (uint) (targetColumnIx + 1);
          }
          else
          {
            ctCol1.max = (uint) (targetColumnIx - 1);
            num1 = index + 1;
          }
          CT_Col ctCol2 = this.columnHelper.CloneCol(colsArray, ctCol1);
          ctCol2.min = (uint) targetColumnIx;
          this.UnsetCollapsed(collapsed.Value, ctCol2);
          this.columnHelper.AddCleanColIntoCols(colsArray, ctCol2);
        }
        else
        {
          CT_Col ctCol2 = ctCol1;
          CT_Col ctCol3 = this.columnHelper.CloneCol(colsArray, ctCol1);
          CT_Col col = this.columnHelper.CloneCol(colsArray, ctCol1);
          int max = (int) ctCol1.max;
          ctCol2.max = (uint) (targetColumnIx - 1);
          ctCol3.min = (uint) targetColumnIx;
          ctCol3.max = (uint) targetColumnIx;
          this.UnsetCollapsed(collapsed.Value, ctCol3);
          this.columnHelper.AddCleanColIntoCols(colsArray, ctCol3);
          col.min = (uint) (targetColumnIx + 1);
          col.max = (uint) max;
          this.columnHelper.AddCleanColIntoCols(colsArray, col);
        }
      }
    }

    private void UnsetCollapsed(bool collapsed, CT_Col ci)
    {
      if (collapsed)
        ci.collapsed = collapsed;
      else
        ci.UnsetCollapsed();
    }

    private int SetGroupHidden(int pIdx, int level, bool hidden)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      int index = pIdx;
      CT_Col col = colsArray.GetColArray(index);
      for (; index < colsArray.sizeOfColArray(); ++index)
      {
        col.hidden = hidden;
        if (index + 1 < colsArray.sizeOfColArray())
        {
          CT_Col colArray = colsArray.GetColArray(index + 1);
          if (this.IsAdjacentBefore(col, colArray) && (int) colArray.outlineLevel >= level)
            col = colArray;
          else
            break;
        }
      }
      return (int) col.max;
    }

    private bool IsAdjacentBefore(CT_Col col, CT_Col other_col)
    {
      return (int) col.max == (int) other_col.min - 1;
    }

    private int FindStartOfColumnOutlineGroup(int pIdx)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      CT_Col other_col = colsArray.GetColArray(pIdx);
      int outlineLevel = (int) other_col.outlineLevel;
      int num = pIdx;
      while (num != 0)
      {
        CT_Col colArray = colsArray.GetColArray(num - 1);
        if (this.IsAdjacentBefore(colArray, other_col) && (int) colArray.outlineLevel >= outlineLevel)
        {
          --num;
          other_col = colArray;
        }
        else
          break;
      }
      return num;
    }

    private int FindEndOfColumnOutlineGroup(int colInfoIndex)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      CT_Col col = colsArray.GetColArray(colInfoIndex);
      int outlineLevel = (int) col.outlineLevel;
      int num = colInfoIndex;
      while (num < colsArray.sizeOfColArray() - 1)
      {
        CT_Col colArray = colsArray.GetColArray(num + 1);
        if (this.IsAdjacentBefore(col, colArray) && (int) colArray.outlineLevel >= outlineLevel)
        {
          ++num;
          col = colArray;
        }
        else
          break;
      }
      return num;
    }

    private void ExpandColumn(int columnIndex)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      CT_Col column = this.columnHelper.GetColumn((long) columnIndex, false);
      int indexOfColumn = this.columnHelper.GetIndexOfColumn(colsArray, column);
      int colInfoIdx = this.FindColInfoIdx((int) column.max, indexOfColumn);
      if (colInfoIdx == -1 || !this.IsColumnGroupCollapsed(colInfoIdx))
        return;
      int columnOutlineGroup1 = this.FindStartOfColumnOutlineGroup(colInfoIdx);
      int columnOutlineGroup2 = this.FindEndOfColumnOutlineGroup(colInfoIdx);
      CT_Col colArray1 = colsArray.GetColArray(columnOutlineGroup2);
      if (!this.IsColumnGroupHiddenByParent(colInfoIdx))
      {
        int outlineLevel = (int) colArray1.outlineLevel;
        bool flag = false;
        for (int index = columnOutlineGroup1; index <= columnOutlineGroup2; ++index)
        {
          CT_Col colArray2 = colsArray.GetColArray(index);
          if (outlineLevel == (int) colArray2.outlineLevel)
          {
            colArray2.UnsetHidden();
            if (flag)
            {
              flag = false;
              colArray2.collapsed = true;
            }
          }
          else
            flag = true;
        }
      }
      this.SetColumn((int) colArray1.max + 1, new short?(), new int?(), new int?(), new bool?(false), new bool?(false));
    }

    private bool IsColumnGroupHiddenByParent(int idx)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      int num1 = 0;
      bool flag1 = false;
      int columnOutlineGroup1 = this.FindEndOfColumnOutlineGroup(idx);
      if (columnOutlineGroup1 < colsArray.sizeOfColArray())
      {
        CT_Col colArray = colsArray.GetColArray(columnOutlineGroup1 + 1);
        if (this.IsAdjacentBefore(colsArray.GetColArray(columnOutlineGroup1), colArray))
        {
          num1 = (int) colArray.outlineLevel;
          flag1 = colArray.hidden;
        }
      }
      int num2 = 0;
      bool flag2 = false;
      int columnOutlineGroup2 = this.FindStartOfColumnOutlineGroup(idx);
      if (columnOutlineGroup2 > 0)
      {
        CT_Col colArray = colsArray.GetColArray(columnOutlineGroup2 - 1);
        if (this.IsAdjacentBefore(colArray, colsArray.GetColArray(columnOutlineGroup2)))
        {
          num2 = (int) colArray.outlineLevel;
          flag2 = colArray.hidden;
        }
      }
      if (num1 > num2)
        return flag1;
      return flag2;
    }

    private int FindColInfoIdx(int columnValue, int fromColInfoIdx)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      if (columnValue < 0)
        throw new ArgumentException("column parameter out of range: " + (object) columnValue);
      if (fromColInfoIdx < 0)
        throw new ArgumentException("fromIdx parameter out of range: " + (object) fromColInfoIdx);
      for (int index = fromColInfoIdx; index < colsArray.sizeOfColArray(); ++index)
      {
        CT_Col colArray = colsArray.GetColArray(index);
        if (this.ContainsColumn(colArray, columnValue))
          return index;
        if ((long) colArray.min > (long) fromColInfoIdx)
          break;
      }
      return -1;
    }

    private bool ContainsColumn(CT_Col col, int columnIndex)
    {
      if ((long) col.min <= (long) columnIndex)
        return (long) columnIndex <= (long) col.max;
      return false;
    }

    private bool IsColumnGroupCollapsed(int idx)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      int columnOutlineGroup = this.FindEndOfColumnOutlineGroup(idx);
      int index = columnOutlineGroup + 1;
      if (index >= colsArray.sizeOfColArray())
        return false;
      CT_Col colArray = colsArray.GetColArray(index);
      if (!this.IsAdjacentBefore(colsArray.GetColArray(columnOutlineGroup), colArray))
        return false;
      return colArray.collapsed;
    }

    public void SetColumnHidden(int columnIndex, bool hidden)
    {
      this.columnHelper.SetColHidden((long) columnIndex, hidden);
    }

    public void SetColumnWidth(int columnIndex, int width)
    {
      if (width > 65280)
        throw new ArgumentException("The maximum column width for an individual cell is 255 characters.");
      this.columnHelper.SetColWidth((long) columnIndex, (double) width / 256.0);
      this.columnHelper.SetCustomWidth((long) columnIndex, true);
    }

    public void SetDefaultColumnStyle(int column, ICellStyle style)
    {
      this.columnHelper.SetColDefaultStyle((long) column, style);
    }

    private CT_SheetView GetSheetTypeSheetView()
    {
      if (this.GetDefaultSheetView() == null)
        this.GetSheetTypeSheetViews().SetSheetViewArray(0, new CT_SheetView());
      return this.GetDefaultSheetView();
    }

    public void SetRowGroupCollapsed(int rowIndex, bool collapse)
    {
      if (collapse)
        this.CollapseRow(rowIndex);
      else
        this.ExpandRow(rowIndex);
    }

    private void CollapseRow(int rowIndex)
    {
      XSSFRow row = (XSSFRow) this.GetRow(rowIndex);
      if (row == null)
        return;
      int ofRowOutlineGroup = this.FindStartOfRowOutlineGroup(rowIndex);
      int rownum = this.WriteHidden(row, ofRowOutlineGroup, true);
      if (this.GetRow(rownum) != null)
        ((XSSFRow) this.GetRow(rownum)).GetCTRow().collapsed = true;
      else
        ((XSSFRow) this.CreateRow(rownum)).GetCTRow().collapsed = true;
    }

    private int FindStartOfRowOutlineGroup(int rowIndex)
    {
      int outlineLevel = (int) ((XSSFRow) this.GetRow(rowIndex)).GetCTRow().outlineLevel;
      int rownum;
      for (rownum = rowIndex; this.GetRow(rownum) != null; --rownum)
      {
        if ((int) ((XSSFRow) this.GetRow(rownum)).GetCTRow().outlineLevel < outlineLevel)
          return rownum + 1;
      }
      return rownum;
    }

    private int WriteHidden(XSSFRow xRow, int rowIndex, bool hidden)
    {
      int outlineLevel = (int) xRow.GetCTRow().outlineLevel;
      IEnumerator rowEnumerator = this.GetRowEnumerator();
      while (rowEnumerator.MoveNext())
      {
        xRow = (XSSFRow) rowEnumerator.Current;
        if ((int) xRow.GetCTRow().outlineLevel >= outlineLevel)
        {
          xRow.GetCTRow().hidden = hidden;
          ++rowIndex;
        }
      }
      return rowIndex;
    }

    private void ExpandRow(int rowNumber)
    {
      if (rowNumber == -1)
        return;
      XSSFRow row = (XSSFRow) this.GetRow(rowNumber);
      if (!row.GetCTRow().IsSetHidden())
        return;
      int ofRowOutlineGroup1 = this.FindStartOfRowOutlineGroup(rowNumber);
      int ofRowOutlineGroup2 = this.FindEndOfRowOutlineGroup(rowNumber);
      if (!this.IsRowGroupHiddenByParent(rowNumber))
      {
        for (int index = ofRowOutlineGroup1; index < ofRowOutlineGroup2; ++index)
        {
          if ((int) row.GetCTRow().outlineLevel == (int) ((XSSFRow) this.GetRow(index)).GetCTRow().outlineLevel)
            ((XSSFRow) this.GetRow(index)).GetCTRow().unsetHidden();
          else if (!this.IsRowGroupCollapsed(index))
            ((XSSFRow) this.GetRow(index)).GetCTRow().unsetHidden();
        }
      }
      ((XSSFRow) this.GetRow(ofRowOutlineGroup2)).GetCTRow().unsetCollapsed();
    }

    public int FindEndOfRowOutlineGroup(int row)
    {
      int outlineLevel = (int) ((XSSFRow) this.GetRow(row)).GetCTRow().outlineLevel;
      int rownum = row;
      while (rownum < this.LastRowNum && (this.GetRow(rownum) != null && (int) ((XSSFRow) this.GetRow(rownum)).GetCTRow().outlineLevel >= outlineLevel))
        ++rownum;
      return rownum;
    }

    private bool IsRowGroupHiddenByParent(int row)
    {
      int ofRowOutlineGroup1 = this.FindEndOfRowOutlineGroup(row);
      int num1;
      bool flag1;
      if (this.GetRow(ofRowOutlineGroup1) == null)
      {
        num1 = 0;
        flag1 = false;
      }
      else
      {
        num1 = (int) ((XSSFRow) this.GetRow(ofRowOutlineGroup1)).GetCTRow().outlineLevel;
        flag1 = ((XSSFRow) this.GetRow(ofRowOutlineGroup1)).GetCTRow().hidden;
      }
      int ofRowOutlineGroup2 = this.FindStartOfRowOutlineGroup(row);
      int num2;
      bool flag2;
      if (ofRowOutlineGroup2 < 0 || this.GetRow(ofRowOutlineGroup2) == null)
      {
        num2 = 0;
        flag2 = false;
      }
      else
      {
        num2 = (int) ((XSSFRow) this.GetRow(ofRowOutlineGroup2)).GetCTRow().outlineLevel;
        flag2 = ((XSSFRow) this.GetRow(ofRowOutlineGroup2)).GetCTRow().hidden;
      }
      if (num1 > num2)
        return flag1;
      return flag2;
    }

    private bool IsRowGroupCollapsed(int row)
    {
      int rownum = this.FindEndOfRowOutlineGroup(row) + 1;
      if (this.GetRow(rownum) == null)
        return false;
      return ((XSSFRow) this.GetRow(rownum)).GetCTRow().collapsed;
    }

    public void SetZoom(int numerator, int denominator)
    {
      this.SetZoom(100 * numerator / denominator);
    }

    public void SetZoom(int scale)
    {
      if (scale < 10 || scale > 400)
        throw new ArgumentException("Valid scale values range from 10 to 400");
      this.GetSheetTypeSheetView().zoomScale = (uint) scale;
    }

    public void ShiftRows(int startRow, int endRow, int n)
    {
      this.ShiftRows(startRow, endRow, n, false, false);
    }

    public void ShiftRows(int startRow, int endRow, int n, bool copyRowHeight, bool reSetOriginalRowHeight)
    {
      List<int> intList = new List<int>();
      foreach (KeyValuePair<int, XSSFRow> row in this._rows)
      {
        XSSFRow xssfRow = row.Value;
        int rowNum = xssfRow.RowNum;
        if (rowNum >= startRow)
        {
          if (!copyRowHeight)
            xssfRow.Height = (short) -1;
          if (this.RemoveRow(startRow, endRow, n, rowNum))
          {
            int count = this.HeadMap(this._rows, xssfRow.RowNum).Count;
            this.worksheet.sheetData.RemoveRow(n <= 0 ? count + intList.Count : count - intList.Count);
            intList.Add(row.Key);
          }
          else if (rowNum >= startRow && rowNum <= endRow)
            xssfRow.Shift(n);
          if (this.sheetComments != null)
          {
            foreach (CT_Comment ctComment in this.sheetComments.GetCTComments().commentList.comment)
            {
              CellReference cellReference1 = new CellReference(ctComment.@ref);
              if (cellReference1.Row == rowNum)
              {
                CellReference cellReference2 = new CellReference(rowNum + n, cellReference1.Col);
                ctComment.@ref = cellReference2.FormatAsString();
              }
            }
          }
        }
      }
      foreach (int key in intList)
        this._rows.Remove(key);
      XSSFRowShifter xssfRowShifter = new XSSFRowShifter(this);
      FormulaShifter forRowShift = FormulaShifter.CreateForRowShift(this.Workbook.GetSheetIndex((ISheet) this), startRow, endRow, n);
      xssfRowShifter.UpdateNamedRanges(forRowShift);
      xssfRowShifter.UpdateFormulas(forRowShift);
      xssfRowShifter.ShiftMerged(startRow, endRow, n);
      xssfRowShifter.UpdateConditionalFormatting(forRowShift);
    }

    public void ShowInPane(short toprow, short leftcol)
    {
      this.GetPane().topLeftCell = new CellReference((int) toprow, leftcol).FormatAsString();
    }

    public void UngroupColumn(int fromColumn, int toColumn)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      for (int index = fromColumn; index <= toColumn; ++index)
      {
        CT_Col column = this.columnHelper.GetColumn((long) index, false);
        if (column != null)
        {
          short outlineLevel = (short) column.outlineLevel;
          column.outlineLevel = (byte) ((uint) outlineLevel - 1U);
          index = (int) column.max;
          if (column.outlineLevel <= (byte) 0)
            this.worksheet.GetColsArray(0).RemoveCol(this.columnHelper.GetIndexOfColumn(colsArray, column));
        }
      }
      this.worksheet.SetColsArray(0, colsArray);
      this.SetSheetFormatPrOutlineLevelCol();
    }

    public void UngroupRow(int fromRow, int toRow)
    {
      for (int rownum = fromRow; rownum <= toRow; ++rownum)
      {
        XSSFRow row = (XSSFRow) this.GetRow(rownum);
        if (row != null)
        {
          CT_Row ctRow = row.GetCTRow();
          short outlineLevel = (short) ctRow.outlineLevel;
          ctRow.outlineLevel = (byte) ((uint) outlineLevel - 1U);
          if (ctRow.outlineLevel == (byte) 0 && row.FirstCellNum == (short) -1)
            this.RemoveRow((IRow) row);
        }
      }
      this.SetSheetFormatPrOutlineLevelRow();
    }

    private void SetSheetFormatPrOutlineLevelRow()
    {
      this.GetSheetTypeSheetFormatPr().outlineLevelRow = (byte) this.GetMaxOutlineLevelRows();
    }

    private void SetSheetFormatPrOutlineLevelCol()
    {
      this.GetSheetTypeSheetFormatPr().outlineLevelCol = (byte) this.GetMaxOutlineLevelCols();
    }

    private CT_SheetViews GetSheetTypeSheetViews()
    {
      if (this.worksheet.sheetViews == null)
      {
        this.worksheet.sheetViews = new CT_SheetViews();
        this.worksheet.sheetViews.AddNewSheetView();
      }
      return this.worksheet.sheetViews;
    }

    public bool IsSelected
    {
      get
      {
        CT_SheetView defaultSheetView = this.GetDefaultSheetView();
        if (defaultSheetView != null)
          return defaultSheetView.tabSelected;
        return false;
      }
      set
      {
        foreach (CT_SheetView ctSheetView in this.GetSheetTypeSheetViews().sheetView)
          ctSheetView.tabSelected = value;
      }
    }

    public static void SetCellComment(string cellRef, XSSFComment comment)
    {
      CellReference cellReference = new CellReference(cellRef);
      comment.Row = cellReference.Row;
      comment.Column = (int) cellReference.Col;
    }

    public void AddHyperlink(XSSFHyperlink hyperlink)
    {
      this.hyperlinks.Add(hyperlink);
    }

    public string GetActiveCell()
    {
      return this.GetSheetTypeSelection().activeCell;
    }

    public void SetActiveCell(string value)
    {
      CT_Selection sheetTypeSelection = this.GetSheetTypeSelection();
      sheetTypeSelection.activeCell = value;
      sheetTypeSelection.SetSqref(new string[1]{ value });
    }

    public bool HasComments
    {
      get
      {
        if (this.sheetComments == null)
          return false;
        return this.sheetComments.GetNumberOfComments() > 0;
      }
    }

    internal int NumberOfComments
    {
      get
      {
        if (this.sheetComments == null)
          return 0;
        return this.sheetComments.GetNumberOfComments();
      }
    }

    private CT_Selection GetSheetTypeSelection()
    {
      if (this.GetSheetTypeSheetView().SizeOfSelectionArray() == 0)
        this.GetSheetTypeSheetView().InsertNewSelection(0);
      return this.GetSheetTypeSheetView().GetSelectionArray(0);
    }

    private CT_SheetView GetDefaultSheetView()
    {
      CT_SheetViews sheetTypeSheetViews = this.GetSheetTypeSheetViews();
      int num = sheetTypeSheetViews == null ? 0 : sheetTypeSheetViews.sizeOfSheetViewArray();
      if (num == 0)
        return (CT_SheetView) null;
      return sheetTypeSheetViews.GetSheetViewArray(num - 1);
    }

    protected internal CommentsTable GetCommentsTable(bool create)
    {
      if (this.sheetComments == null)
      {
        if (create)
        {
          try
          {
            this.sheetComments = (CommentsTable) this.CreateRelationship((POIXMLRelation) XSSFRelation.SHEET_COMMENTS, (POIXMLFactory) XSSFFactory.GetInstance(), (int) this.sheet.sheetId);
          }
          catch (PartAlreadyExistsException ex)
          {
            this.sheetComments = (CommentsTable) this.CreateRelationship((POIXMLRelation) XSSFRelation.SHEET_COMMENTS, (POIXMLFactory) XSSFFactory.GetInstance(), -1);
          }
        }
      }
      return this.sheetComments;
    }

    private CT_PageSetUpPr GetSheetTypePageSetUpPr()
    {
      CT_SheetPr sheetTypeSheetPr = this.GetSheetTypeSheetPr();
      if (!sheetTypeSheetPr.IsSetPageSetUpPr())
        return sheetTypeSheetPr.AddNewPageSetUpPr();
      return sheetTypeSheetPr.pageSetUpPr;
    }

    private bool RemoveRow(int startRow, int endRow, int n, int rownum)
    {
      return rownum >= startRow + n && rownum <= endRow + n && (n > 0 && rownum > endRow || n < 0 && rownum < startRow);
    }

    private CT_Pane GetPane()
    {
      if (this.GetDefaultSheetView().pane == null)
        this.GetDefaultSheetView().AddNewPane();
      return this.GetDefaultSheetView().pane;
    }

    internal CT_CellFormula GetSharedFormula(int sid)
    {
      return this.sharedFormulas[sid];
    }

    internal void OnReadCell(XSSFCell cell)
    {
      CT_CellFormula f = cell.GetCTCell().f;
      if (f != null && f.t == ST_CellFormulaType.shared && (f.isSetRef() && f.Value != null))
      {
        CT_CellFormula ctCellFormula = f.Copy();
        CellRangeAddress cellRangeAddress = CellRangeAddress.ValueOf(ctCellFormula.@ref);
        CellReference cellReference = new CellReference((ICell) cell);
        if ((int) cellReference.Col > cellRangeAddress.FirstColumn || cellReference.Row > cellRangeAddress.FirstRow)
        {
          string str = new CellRangeAddress(Math.Max(cellReference.Row, cellRangeAddress.FirstRow), cellRangeAddress.LastRow, Math.Max((int) cellReference.Col, cellRangeAddress.FirstColumn), cellRangeAddress.LastColumn).FormatAsString();
          ctCellFormula.@ref = str;
        }
        this.sharedFormulas[(int) f.si] = ctCellFormula;
      }
      if (f == null || f.t != ST_CellFormulaType.array || f.@ref == null)
        return;
      this.arrayFormulas.Add(CellRangeAddress.ValueOf(f.@ref));
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.Write(outputStream);
      outputStream.Close();
    }

    internal virtual void Write(Stream out1)
    {
      if (this.worksheet.sizeOfColsArray() == 1 && this.worksheet.GetColsArray(0).sizeOfColArray() == 0)
        this.worksheet.SetColsArray((List<CT_Cols>) null);
      if (this.hyperlinks.Count > 0)
      {
        if (this.worksheet.hyperlinks == null)
          this.worksheet.AddNewHyperlinks();
        CT_Hyperlink[] array = new CT_Hyperlink[this.hyperlinks.Count];
        for (int index = 0; index < array.Length; ++index)
        {
          XSSFHyperlink hyperlink = this.hyperlinks[index];
          hyperlink.GenerateRelationIfNeeded(this.GetPackagePart());
          array[index] = hyperlink.GetCTHyperlink();
        }
        this.worksheet.hyperlinks.SetHyperlinkArray(array);
      }
      foreach (XSSFRow xssfRow in this._rows.Values)
        xssfRow.OnDocumentWrite();
      new Dictionary<string, string>()[ST_RelationshipId.NamespaceURI] = "r";
      this.worksheet.Save(out1);
    }

    public bool IsAutoFilterLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.autoFilter;
      return false;
    }

    public bool IsDeleteColumnsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.deleteColumns;
      return false;
    }

    public bool IsDeleteRowsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.deleteRows;
      return false;
    }

    public bool IsFormatCellsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.formatCells;
      return false;
    }

    public bool IsFormatColumnsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.formatColumns;
      return false;
    }

    public bool IsFormatRowsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.formatRows;
      return false;
    }

    public bool IsInsertColumnsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.insertColumns;
      return false;
    }

    public bool IsInsertHyperlinksLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.insertHyperlinks;
      return false;
    }

    public bool IsInsertRowsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.insertRows;
      return false;
    }

    public bool IsPivotTablesLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.pivotTables;
      return false;
    }

    public bool IsSortLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.sort;
      return false;
    }

    public bool IsObjectsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.objects;
      return false;
    }

    public bool IsScenariosLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.scenarios;
      return false;
    }

    public bool IsSelectLockedCellsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.selectLockedCells;
      return false;
    }

    public bool IsSelectUnlockedCellsLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.selectUnlockedCells;
      return false;
    }

    public bool IsSheetLocked()
    {
      this.CreateProtectionFieldIfNotPresent();
      if (this.sheetProtectionEnabled())
        return this.worksheet.sheetProtection.sheet;
      return false;
    }

    public void EnableLocking()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.sheet = true;
    }

    public void DisableLocking()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.sheet = false;
    }

    public void LockAutoFilter()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.autoFilter = true;
    }

    public void LockDeleteColumns()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.deleteColumns = true;
    }

    public void LockDeleteRows()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.deleteRows = true;
    }

    public void LockFormatCells()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.deleteColumns = true;
    }

    public void LockFormatColumns()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.formatColumns = true;
    }

    public void LockFormatRows()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.formatRows = true;
    }

    public void LockInsertColumns()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.insertColumns = true;
    }

    public void LockInsertHyperlinks()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.insertHyperlinks = true;
    }

    public void LockInsertRows()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.insertRows = true;
    }

    public void LockPivotTables()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.pivotTables = true;
    }

    public void LockSort()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.sort = true;
    }

    public void LockObjects()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.objects = true;
    }

    public void LockScenarios()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.scenarios = true;
    }

    public void LockSelectLockedCells()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.selectLockedCells = true;
    }

    public void LockSelectUnlockedCells()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.worksheet.sheetProtection.selectUnlockedCells = true;
    }

    private void CreateProtectionFieldIfNotPresent()
    {
      if (this.worksheet.sheetProtection != null)
        return;
      this.worksheet.sheetProtection = new CT_SheetProtection();
    }

    private bool sheetProtectionEnabled()
    {
      return this.worksheet.sheetProtection.sheet;
    }

    internal bool IsCellInArrayFormulaContext(ICell cell)
    {
      foreach (CellRangeAddressBase arrayFormula in this.arrayFormulas)
      {
        if (arrayFormula.IsInRange(cell.RowIndex, cell.ColumnIndex))
          return true;
      }
      return false;
    }

    internal XSSFCell GetFirstCellInArrayFormula(ICell cell)
    {
      foreach (CellRangeAddress arrayFormula in this.arrayFormulas)
      {
        if (arrayFormula.IsInRange(cell.RowIndex, cell.ColumnIndex))
          return (XSSFCell) this.GetRow(arrayFormula.FirstRow).GetCell(arrayFormula.FirstColumn);
      }
      return (XSSFCell) null;
    }

    private ICellRange<ICell> GetCellRange(CellRangeAddress range)
    {
      int firstRow = range.FirstRow;
      int firstColumn = range.FirstColumn;
      int lastRow = range.LastRow;
      int lastColumn = range.LastColumn;
      int height = lastRow - firstRow + 1;
      int width = lastColumn - firstColumn + 1;
      List<ICell> flattenedList = new List<ICell>(height * width);
      for (int rownum = firstRow; rownum <= lastRow; ++rownum)
      {
        for (int index = firstColumn; index <= lastColumn; ++index)
        {
          IRow row = this.GetRow(rownum) ?? this.CreateRow(rownum);
          ICell cell = row.GetCell(index) ?? row.CreateCell(index);
          flattenedList.Add(cell);
        }
      }
      return (ICellRange<ICell>) SSCellRange<ICell>.Create(firstRow, firstColumn, height, width, flattenedList, typeof (ICell));
    }

    public ICellRange<ICell> SetArrayFormula(string formula, CellRangeAddress range)
    {
      ICellRange<ICell> cellRange = this.GetCellRange(range);
      ((XSSFCell) cellRange.TopLeftCell).SetCellArrayFormula(formula, range);
      this.arrayFormulas.Add(range);
      return cellRange;
    }

    public ICellRange<ICell> RemoveArrayFormula(ICell cell)
    {
      if (cell.Sheet != this)
        throw new ArgumentException("Specified cell does not belong to this sheet.");
      foreach (CellRangeAddress arrayFormula in this.arrayFormulas)
      {
        if (arrayFormula.IsInRange(cell.RowIndex, cell.ColumnIndex))
        {
          this.arrayFormulas.Remove(arrayFormula);
          ICellRange<ICell> cellRange = this.GetCellRange(arrayFormula);
          foreach (ICell cell1 in (IEnumerable<ICell>) cellRange)
            cell1.SetCellType(CellType.BLANK);
          return cellRange;
        }
      }
      throw new ArgumentException("Cell " + ((XSSFCell) cell).GetCTCell().r + " is not part of an array formula.");
    }

    public IDataValidationHelper GetDataValidationHelper()
    {
      return (IDataValidationHelper) this.dataValidationHelper;
    }

    public List<XSSFDataValidation> GetDataValidations()
    {
      List<XSSFDataValidation> xssfDataValidationList = new List<XSSFDataValidation>();
      CT_DataValidations dataValidations = this.worksheet.dataValidations;
      if (dataValidations != null && dataValidations.count > 0U)
      {
        foreach (CT_DataValidation ctDataValidation in dataValidations.dataValidation)
        {
          CellRangeAddressList regions = new CellRangeAddressList();
          foreach (string str1 in ctDataValidation.sqref)
          {
            char[] chArray1 = new char[1]{ ' ' };
            foreach (string str2 in str1.Split(chArray1))
            {
              char[] chArray2 = new char[1]{ ':' };
              string[] strArray = str2.Split(chArray2);
              CellReference cellReference1 = new CellReference(strArray[0]);
              CellReference cellReference2 = strArray.Length > 1 ? new CellReference(strArray[1]) : cellReference1;
              CellRangeAddress cra = new CellRangeAddress(cellReference1.Row, cellReference2.Row, (int) cellReference1.Col, (int) cellReference2.Col);
              regions.AddCellRangeAddress(cra);
            }
          }
          XSSFDataValidation xssfDataValidation = new XSSFDataValidation(regions, ctDataValidation);
          xssfDataValidationList.Add(xssfDataValidation);
        }
      }
      return xssfDataValidationList;
    }

    public void AddValidationData(IDataValidation dataValidation)
    {
      XSSFDataValidation xssfDataValidation = (XSSFDataValidation) dataValidation;
      CT_DataValidations ctDataValidations = this.worksheet.dataValidations ?? this.worksheet.AddNewDataValidations();
      int num = ctDataValidations.sizeOfDataValidationArray();
      ctDataValidations.AddNewDataValidation().Set(xssfDataValidation.GetCTDataValidation());
      ctDataValidations.count = (uint) (num + 1);
    }

    public IAutoFilter SetAutoFilter(CellRangeAddress range)
    {
      (this.worksheet.autoFilter ?? this.worksheet.AddNewAutoFilter()).@ref = new CellRangeAddress(range.FirstRow, range.LastRow, range.FirstColumn, range.LastColumn).FormatAsString();
      XSSFWorkbook workbook = (XSSFWorkbook) this.Workbook;
      int sheetIndex = this.Workbook.GetSheetIndex((ISheet) this);
      if (workbook.GetBuiltInName(XSSFName.BUILTIN_FILTER_DB, sheetIndex) == null)
      {
        XSSFName builtInName = workbook.CreateBuiltInName(XSSFName.BUILTIN_FILTER_DB, sheetIndex);
        builtInName.GetCTName().hidden = true;
        string str = new CellReference(this.SheetName, range.FirstRow, range.FirstColumn, true, true).FormatAsString() + ":" + new CellReference((string) null, range.LastRow, range.LastColumn, true, true).FormatAsString();
        builtInName.RefersToFormula = str;
      }
      return (IAutoFilter) new XSSFAutoFilter(this);
    }

    public XSSFTable CreateTable()
    {
      if (!this.worksheet.IsSetTableParts())
        this.worksheet.AddNewTableParts();
      CT_TablePart ctTablePart = this.worksheet.tableParts.AddNewTablePart();
      int idx = this.GetPackagePart().Package.GetPartsByContentType(XSSFRelation.TABLE.ContentType).Count + 1;
      XSSFTable relationship = (XSSFTable) this.CreateRelationship((POIXMLRelation) XSSFRelation.TABLE, (POIXMLFactory) XSSFFactory.GetInstance(), idx);
      ctTablePart.id = relationship.GetPackageRelationship().Id;
      this.tables[ctTablePart.id] = relationship;
      return relationship;
    }

    public List<XSSFTable> GetTables()
    {
      return new List<XSSFTable>((IEnumerable<XSSFTable>) this.tables.Values);
    }

    public ISheetConditionalFormatting SheetConditionalFormatting
    {
      get
      {
        return (ISheetConditionalFormatting) new XSSFSheetConditionalFormatting(this);
      }
    }

    public void SetTabColor(int colorIndex)
    {
      (this.worksheet.sheetPr ?? this.worksheet.AddNewSheetPr()).tabColor = new CT_Color()
      {
        indexed = (uint) colorIndex
      };
    }

    public IDrawing DrawingPatriarch
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public IEnumerator GetEnumerator()
    {
      return this.GetRowEnumerator();
    }

    public IEnumerator GetRowEnumerator()
    {
      return (IEnumerator) this._rows.Values.GetEnumerator();
    }

    public bool IsActive
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

    public bool IsMergedRegion(CellRangeAddress mergedRegion)
    {
      throw new NotImplementedException();
    }

    public void SetActive(bool sel)
    {
      throw new NotImplementedException();
    }

    public void SetActiveCell(int row, int column)
    {
      throw new NotImplementedException();
    }

    public void SetActiveCellRange(List<CellRangeAddress8Bit> cellranges, int activeRange, int activeRow, int activeColumn)
    {
      throw new NotImplementedException();
    }

    public void SetActiveCellRange(int firstRow, int lastRow, int firstColumn, int lastColumn)
    {
      throw new NotImplementedException();
    }

    public short TabColorIndex
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

    public bool IsRightToLeft
    {
      get
      {
        CT_SheetView defaultSheetView = this.GetDefaultSheetView();
        if (defaultSheetView != null)
          return defaultSheetView.rightToLeft;
        return false;
      }
      set
      {
        this.GetDefaultSheetView().rightToLeft = value;
      }
    }

    public IRow CopyRow(int sourceIndex, int targetIndex)
    {
      return SheetUtil.CopyRow((ISheet) this, sourceIndex, targetIndex);
    }

    public CellRangeAddress RepeatingRows
    {
      get
      {
        return this.GetRepeatingRowsOrColums(true);
      }
      set
      {
        CellRangeAddress repeatingColumns = this.RepeatingColumns;
        this.SetRepeatingRowsAndColumns(value, repeatingColumns);
      }
    }

    public CellRangeAddress RepeatingColumns
    {
      get
      {
        return this.GetRepeatingRowsOrColums(false);
      }
      set
      {
        this.SetRepeatingRowsAndColumns(this.RepeatingRows, value);
      }
    }

    private void SetRepeatingRowsAndColumns(CellRangeAddress rowDef, CellRangeAddress colDef)
    {
      int startC = -1;
      int endC = -1;
      int startR = -1;
      int endR = -1;
      if (rowDef != null)
      {
        startR = rowDef.FirstRow;
        endR = rowDef.LastRow;
        if (startR == -1 && endR != -1 || (startR < -1 || endR < -1) || startR > endR)
          throw new ArgumentException("Invalid row range specification");
      }
      if (colDef != null)
      {
        startC = colDef.FirstColumn;
        endC = colDef.LastColumn;
        if (startC == -1 && endC != -1 || (startC < -1 || endC < -1) || startC > endC)
          throw new ArgumentException("Invalid column range specification");
      }
      int sheetIndex = this.Workbook.GetSheetIndex((ISheet) this);
      bool flag = rowDef == null && colDef == null;
      XSSFWorkbook workbook = this.Workbook as XSSFWorkbook;
      if (workbook == null)
        throw new RuntimeException("Workbook should not be null");
      XSSFName builtInName = workbook.GetBuiltInName(XSSFName.BUILTIN_PRINT_TITLE, sheetIndex);
      if (flag)
      {
        if (builtInName == null)
          return;
        workbook.RemoveName(builtInName);
      }
      else
      {
        if (builtInName == null)
          builtInName = workbook.CreateBuiltInName(XSSFName.BUILTIN_PRINT_TITLE, sheetIndex);
        string referenceBuiltInRecord = XSSFSheet.GetReferenceBuiltInRecord(builtInName.SheetName, startC, endC, startR, endR);
        builtInName.RefersToFormula = referenceBuiltInRecord;
        if (this.worksheet.IsSetPageSetup() && this.worksheet.IsSetPageMargins())
          return;
        this.PrintSetup.ValidSettings = false;
      }
    }

    private static string GetReferenceBuiltInRecord(string sheetName, int startC, int endC, int startR, int endR)
    {
      CellReference cellReference1 = new CellReference(sheetName, 0, startC, true, true);
      CellReference cellReference2 = new CellReference(sheetName, 0, endC, true, true);
      CellReference cellReference3 = new CellReference(sheetName, startR, 0, true, true);
      CellReference cellReference4 = new CellReference(sheetName, endR, 0, true, true);
      string str1 = SheetNameFormatter.Format(sheetName);
      string str2 = "";
      string str3 = "";
      if (startC != -1 || endC != -1)
        str2 = str1 + "!$" + cellReference1.CellRefParts[2] + ":$" + cellReference2.CellRefParts[2];
      if ((startR != -1 || endR != -1) && (!cellReference3.CellRefParts[1].Equals("0") && !cellReference4.CellRefParts[1].Equals("0")))
        str3 = str1 + "!$" + cellReference3.CellRefParts[1] + ":$" + cellReference4.CellRefParts[1];
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(str2);
      if (stringBuilder.Length > 0 && str3.Length > 0)
        stringBuilder.Append(',');
      stringBuilder.Append(str3);
      return stringBuilder.ToString();
    }

    private CellRangeAddress GetRepeatingRowsOrColums(bool rows)
    {
      int sheetIndex = this.Workbook.GetSheetIndex((ISheet) this);
      XSSFWorkbook workbook = this.Workbook as XSSFWorkbook;
      if (workbook == null)
        throw new RuntimeException("Workbook should not be null");
      XSSFName builtInName = workbook.GetBuiltInName(XSSFName.BUILTIN_PRINT_TITLE, sheetIndex);
      if (builtInName == null)
        return (CellRangeAddress) null;
      string refersToFormula = builtInName.RefersToFormula;
      if (refersToFormula == null)
        return (CellRangeAddress) null;
      string[] strArray = refersToFormula.Split(",".ToCharArray());
      int lastRowIndex = SpreadsheetVersion.EXCEL2007.LastRowIndex;
      int lastColumnIndex = SpreadsheetVersion.EXCEL2007.LastColumnIndex;
      foreach (string reference in strArray)
      {
        CellRangeAddress cellRangeAddress = CellRangeAddress.ValueOf(reference);
        if (cellRangeAddress.FirstColumn == 0 && cellRangeAddress.LastColumn == lastColumnIndex || cellRangeAddress.FirstColumn == -1 && cellRangeAddress.LastColumn == -1)
        {
          if (rows)
            return cellRangeAddress;
        }
        else if ((cellRangeAddress.FirstRow == 0 && cellRangeAddress.LastRow == lastRowIndex || cellRangeAddress.FirstRow == -1 && cellRangeAddress.LastRow == -1) && !rows)
          return cellRangeAddress;
      }
      return (CellRangeAddress) null;
    }
  }
}
