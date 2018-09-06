// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFRow
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.Util;
using NPOI.XSSF.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NPOI.XSSF.UserModel
{
  public class XSSFRow : IRow, IComparable<XSSFRow>
  {
    private static POILogger _logger = POILogFactory.GetLogger(typeof (XSSFRow));
    private CT_Row _row;
    private SortedDictionary<int, ICell> _cells;
    private XSSFSheet _sheet;

    public XSSFRow(CT_Row row, XSSFSheet sheet)
    {
      this._row = row;
      this._sheet = sheet;
      this._cells = new SortedDictionary<int, ICell>();
      if (0 >= row.SizeOfCArray())
        return;
      foreach (CT_Cell cell1 in row.c)
      {
        XSSFCell cell2 = new XSSFCell(this, cell1);
        this._cells.Add(cell2.ColumnIndex, (ICell) cell2);
        sheet.OnReadCell(cell2);
      }
    }

    public ISheet Sheet
    {
      get
      {
        return (ISheet) this._sheet;
      }
    }

    public SortedDictionary<int, ICell>.ValueCollection.Enumerator CellIterator()
    {
      return this._cells.Values.GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) this.CellIterator();
    }

    public int CompareTo(XSSFRow row)
    {
      int rowNum1 = this.RowNum;
      if (row.Sheet != this.Sheet)
        throw new ArgumentException("The Compared rows must belong to the same XSSFSheet");
      int rowNum2 = row.RowNum;
      if (rowNum1 < rowNum2)
        return -1;
      return rowNum1 != rowNum2 ? 1 : 0;
    }

    public ICell CreateCell(int columnIndex)
    {
      return this.CreateCell(columnIndex, CellType.BLANK);
    }

    public ICell CreateCell(int columnIndex, CellType type)
    {
      XSSFCell xssfCell1 = this._cells.ContainsKey(columnIndex) ? (XSSFCell) this._cells[columnIndex] : (XSSFCell) null;
      CT_Cell cell;
      if (xssfCell1 != null)
      {
        cell = xssfCell1.GetCTCell();
        cell.Set(new CT_Cell());
      }
      else
        cell = this._row.AddNewC();
      XSSFCell xssfCell2 = new XSSFCell(this, cell);
      xssfCell2.SetCellNum(columnIndex);
      if (type != CellType.BLANK)
        xssfCell2.SetCellType(type);
      this._cells[columnIndex] = (ICell) xssfCell2;
      return (ICell) xssfCell2;
    }

    public ICell GetCell(int cellnum)
    {
      return this.GetCell(cellnum, this._sheet.Workbook.MissingCellPolicy);
    }

    private ICell RetrieveCell(int cellnum)
    {
      if (!this._cells.ContainsKey(cellnum))
        return (ICell) null;
      return this._cells[cellnum];
    }

    public ICell GetCell(int cellnum, MissingCellPolicy policy)
    {
      if (cellnum < 0)
        throw new ArgumentException("Cell index must be >= 0");
      XSSFCell xssfCell = (XSSFCell) this.RetrieveCell(cellnum);
      if (policy == MissingCellPolicy.RETURN_NULL_AND_BLANK)
        return (ICell) xssfCell;
      if (policy == MissingCellPolicy.RETURN_BLANK_AS_NULL)
      {
        if (xssfCell == null)
          return (ICell) xssfCell;
        if (xssfCell.CellType == CellType.BLANK)
          return (ICell) null;
        return (ICell) xssfCell;
      }
      if (policy == MissingCellPolicy.CREATE_NULL_AS_BLANK)
        return (ICell) xssfCell ?? this.CreateCell(cellnum, CellType.BLANK);
      throw new ArgumentException("Illegal policy " + (object) policy + " (" + (object) policy.id + ")");
    }

    private int GetFirstKey(SortedDictionary<int, ICell>.KeyCollection keys)
    {
      int num = 0;
      foreach (int key in keys)
      {
        if (num == 0)
          return key;
      }
      throw new ArgumentOutOfRangeException();
    }

    private int GetLastKey(SortedDictionary<int, ICell>.KeyCollection keys)
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

    public short FirstCellNum
    {
      get
      {
        return this._cells.Count == 0 ? (short) -1 : (short) this.GetFirstKey(this._cells.Keys);
      }
    }

    public short LastCellNum
    {
      get
      {
        return this._cells.Count == 0 ? (short) -1 : (short) (this.GetLastKey(this._cells.Keys) + 1);
      }
    }

    public short Height
    {
      get
      {
        return (short) ((double) this.HeightInPoints * 20.0);
      }
      set
      {
        if (value == (short) -1)
        {
          if (this._row.IsSetHt())
            this._row.unSetHt();
          if (!this._row.IsSetCustomHeight())
            return;
          this._row.unSetCustomHeight();
        }
        else
        {
          this._row.ht = (double) value / 20.0;
          this._row.customHeight = true;
        }
      }
    }

    public float HeightInPoints
    {
      get
      {
        if (this._row.IsSetHt())
          return (float) this._row.ht;
        return this._sheet.DefaultRowHeightInPoints;
      }
      set
      {
        this.Height = (double) value == -1.0 ? (short) -1 : (short) ((double) value * 20.0);
      }
    }

    public int PhysicalNumberOfCells
    {
      get
      {
        return this._cells.Count;
      }
    }

    public int RowNum
    {
      get
      {
        return (int) this._row.r - 1;
      }
      set
      {
        int lastRowIndex = SpreadsheetVersion.EXCEL2007.LastRowIndex;
        if (value < 0 || value > lastRowIndex)
          throw new ArgumentException("Invalid row number (" + (object) value + ") outside allowable range (0.." + (object) lastRowIndex + ")");
        this._row.r = (uint) (value + 1);
      }
    }

    public bool ZeroHeight
    {
      get
      {
        return this._row.hidden;
      }
      set
      {
        this._row.hidden = value;
      }
    }

    public bool IsFormatted
    {
      get
      {
        return this._row.IsSetS();
      }
    }

    public ICellStyle RowStyle
    {
      get
      {
        if (!this.IsFormatted)
          return (ICellStyle) null;
        StylesTable stylesSource = ((XSSFWorkbook) this.Sheet.Workbook).GetStylesSource();
        if (stylesSource.GetNumCellStyles() > 0)
          return (ICellStyle) stylesSource.GetStyleAt((int) this._row.s);
        return (ICellStyle) null;
      }
      set
      {
        if (value == null)
        {
          if (!this._row.IsSetS())
            return;
          this._row.unSetS();
          this._row.unSetCustomFormat();
        }
        else
        {
          StylesTable stylesSource = ((XSSFWorkbook) this.Sheet.Workbook).GetStylesSource();
          XSSFCellStyle style = (XSSFCellStyle) value;
          style.VerifyBelongsToStylesSource(stylesSource);
          this._row.s = (uint) (ulong) stylesSource.PutStyle(style);
          this._row.customFormat = true;
        }
      }
    }

    public void SetRowStyle(ICellStyle style)
    {
    }

    public void RemoveCell(ICell cell)
    {
      if (cell.Row != this)
        throw new ArgumentException("Specified cell does not belong to this row");
      XSSFCell cell1 = (XSSFCell) cell;
      if (cell1.IsPartOfArrayFormulaGroup)
        cell1.NotifyArrayFormulaChanging();
      if (cell.CellType == CellType.FORMULA)
        ((XSSFWorkbook) this._sheet.Workbook).OnDeleteFormula(cell1);
      this._cells.Remove(cell.ColumnIndex);
    }

    public CT_Row GetCTRow()
    {
      return this._row;
    }

    internal void OnDocumentWrite()
    {
      bool flag = true;
      if (this._row.SizeOfCArray() != this._cells.Count)
      {
        flag = false;
      }
      else
      {
        int num = 0;
        foreach (XSSFCell xssfCell in this._cells.Values)
        {
          CT_Cell ctCell = xssfCell.GetCTCell();
          CT_Cell carray = this._row.GetCArray(num++);
          string r1 = ctCell.r;
          string r2 = carray.r;
          if ((r1 == null ? (r2 == null ? 1 : 0) : (r1.Equals(r2) ? 1 : 0)) == 0)
          {
            flag = false;
            break;
          }
        }
      }
      if (flag)
        return;
      CT_Cell[] array = new CT_Cell[this._cells.Count];
      int num1 = 0;
      foreach (XSSFCell xssfCell in this._cells.Values)
        array[num1++] = xssfCell.GetCTCell();
      this._row.SetCArray(array);
    }

    public override string ToString()
    {
      return this._row.ToString();
    }

    internal void Shift(int n)
    {
      int pRow = this.RowNum + n;
      CalculationChain calculationChain = ((XSSFWorkbook) this._sheet.Workbook).GetCalculationChain();
      int sheetId = (int) this._sheet.sheet.sheetId;
      string msg = "Row[rownum=" + (object) this.RowNum + "] contains cell(s) included in a multi-cell array formula. You cannot change part of an array.";
      foreach (XSSFCell xssfCell in this)
      {
        if (xssfCell.IsPartOfArrayFormulaGroup)
          xssfCell.NotifyArrayFormulaChanging(msg);
        calculationChain?.RemoveItem(sheetId, xssfCell.GetReference());
        xssfCell.GetCTCell().r = new CellReference(pRow, xssfCell.ColumnIndex).FormatAsString();
      }
      this.RowNum = pRow;
    }

    public List<ICell> Cells
    {
      get
      {
        List<ICell> cellList = new List<ICell>();
        foreach (ICell cell in this._cells.Values)
          cellList.Add(cell);
        return cellList;
      }
    }

    public void MoveCell(ICell cell, int newColumn)
    {
      throw new NotImplementedException();
    }

    public IRow CopyRowTo(int targetIndex)
    {
      return this.Sheet.CopyRow(this.RowNum, targetIndex);
    }

    public ICell CopyCell(int sourceIndex, int targetIndex)
    {
      return CellUtil.CopyCell((IRow) this, sourceIndex, targetIndex);
    }
  }
}
