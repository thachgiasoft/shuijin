// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Helpers.ColumnHelper
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.Util;
using System.Collections.Generic;

namespace NPOI.XSSF.UserModel.Helpers
{
  public class ColumnHelper
  {
    private CT_Worksheet worksheet;
    private CT_Cols newCols;

    public ColumnHelper(CT_Worksheet worksheet)
    {
      this.worksheet = worksheet;
      this.CleanColumns();
    }

    public void CleanColumns()
    {
      this.newCols = new CT_Cols();
      List<CT_Cols> colsArray = this.worksheet.GetColsArray();
      if (colsArray != null)
      {
        int index1;
        for (index1 = 0; index1 < colsArray.Count; ++index1)
        {
          List<CT_Col> colArray = colsArray[index1].GetColArray();
          for (int index2 = 0; index2 < colArray.Count; ++index2)
            this.newCols = this.AddCleanColIntoCols(this.newCols, colArray[index2]);
        }
        for (int index2 = index1 - 1; index2 >= 0; --index2)
          this.worksheet.RemoveCols(index2);
      }
      this.worksheet.AddNewCols();
      this.worksheet.SetColsArray(0, this.newCols);
    }

    public static void SortColumns(CT_Cols newCols)
    {
      List<CT_Col> colArray = newCols.GetColArray();
      colArray.Sort((IComparer<CT_Col>) new CTColComparator());
      newCols.SetColArray(colArray);
    }

    public CT_Col CloneCol(CT_Cols cols, CT_Col col)
    {
      CT_Col toCol = cols.AddNewCol();
      toCol.min = col.min;
      toCol.max = col.max;
      this.SetColumnAttributes(col, toCol);
      return toCol;
    }

    public CT_Col GetColumn(long index, bool splitColumns)
    {
      return this.GetColumn1Based(index + 1L, splitColumns);
    }

    public CT_Col GetColumn1Based(long index1, bool splitColumns)
    {
      CT_Cols colsArray = this.worksheet.GetColsArray(0);
      for (int index = 0; index < colsArray.sizeOfColArray(); ++index)
      {
        CT_Col colArray = colsArray.GetColArray(index);
        if ((long) colArray.min <= index1 && (long) colArray.max >= index1)
        {
          if (splitColumns)
          {
            if ((long) colArray.min < index1)
              this.insertCol(colsArray, (long) colArray.min, index1 - 1L, new CT_Col[1]
              {
                colArray
              });
            if ((long) colArray.max > index1)
              this.insertCol(colsArray, index1 + 1L, (long) colArray.max, new CT_Col[1]
              {
                colArray
              });
            colArray.min = (uint) index1;
            colArray.max = (uint) index1;
          }
          return colArray;
        }
      }
      return (CT_Col) null;
    }

    public CT_Cols AddCleanColIntoCols(CT_Cols cols, CT_Col col)
    {
      bool flag = false;
      for (int index = 0; index < cols.sizeOfColArray(); ++index)
      {
        CT_Col colArray = cols.GetColArray(index);
        long[] range1 = new long[2]{ (long) colArray.min, (long) colArray.max };
        long[] range2 = new long[2]{ (long) col.min, (long) col.max };
        long[] overlappingRange = NumericRanges.GetOverlappingRange(range1, range2);
        int overlappingType = NumericRanges.GetOverlappingType(range1, range2);
        if (overlappingType == 0)
        {
          colArray.max = (uint) ((ulong) overlappingRange[0] - 1UL);
          this.insertCol(cols, overlappingRange[0], overlappingRange[1], new CT_Col[2]
          {
            colArray,
            col
          });
          int num = index + 1;
          this.insertCol(cols, overlappingRange[1] + 1L, (long) col.max, new CT_Col[1]
          {
            col
          });
          index = num + 1;
        }
        else if (overlappingType == 1)
        {
          colArray.min = (uint) ((ulong) overlappingRange[1] + 1UL);
          this.insertCol(cols, overlappingRange[0], overlappingRange[1], new CT_Col[2]
          {
            colArray,
            col
          });
          int num = index + 1;
          this.insertCol(cols, (long) col.min, overlappingRange[0] - 1L, new CT_Col[1]
          {
            col
          });
          index = num + 1;
        }
        else if (overlappingType == 3)
        {
          this.SetColumnAttributes(col, colArray);
          if ((int) col.min != (int) colArray.min)
          {
            this.insertCol(cols, (long) col.min, (long) (colArray.min - 1U), new CT_Col[1]
            {
              col
            });
            ++index;
          }
          if ((int) col.max != (int) colArray.max)
          {
            this.insertCol(cols, (long) (colArray.max + 1U), (long) col.max, new CT_Col[1]
            {
              col
            });
            ++index;
          }
        }
        else if (overlappingType == 2)
        {
          if ((int) col.min != (int) colArray.min)
          {
            this.insertCol(cols, (long) colArray.min, (long) (col.min - 1U), new CT_Col[1]
            {
              colArray
            });
            ++index;
          }
          if ((int) col.max != (int) colArray.max)
          {
            this.insertCol(cols, (long) (col.max + 1U), (long) colArray.max, new CT_Col[1]
            {
              colArray
            });
            ++index;
          }
          colArray.min = (uint) overlappingRange[0];
          colArray.max = (uint) overlappingRange[1];
          this.SetColumnAttributes(col, colArray);
        }
        if (overlappingType != -1)
          flag = true;
      }
      if (!flag)
        this.CloneCol(cols, col);
      ColumnHelper.SortColumns(cols);
      return cols;
    }

    private CT_Col insertCol(CT_Cols cols, long min, long max, CT_Col[] colsWithAttributes)
    {
      if (this.columnExists(cols, min, max))
        return (CT_Col) null;
      CT_Col toCol = cols.InsertNewCol(0);
      toCol.min = (uint) min;
      toCol.max = (uint) max;
      foreach (CT_Col colsWithAttribute in colsWithAttributes)
        this.SetColumnAttributes(colsWithAttribute, toCol);
      return toCol;
    }

    public bool columnExists(CT_Cols cols, long index)
    {
      return this.columnExists1Based(cols, index + 1L);
    }

    private bool columnExists1Based(CT_Cols cols, long index1)
    {
      for (int index = 0; index < cols.sizeOfColArray(); ++index)
      {
        if ((long) cols.GetColArray(index).min == index1)
          return true;
      }
      return false;
    }

    public void SetColumnAttributes(CT_Col fromCol, CT_Col toCol)
    {
      if (fromCol.IsSetBestFit())
        toCol.bestFit = fromCol.bestFit;
      if (fromCol.IsSetCustomWidth())
        toCol.customWidth = fromCol.customWidth;
      if (fromCol.IsSetHidden())
        toCol.hidden = fromCol.hidden;
      if (fromCol.IsSetStyle())
      {
        toCol.style = fromCol.style;
        toCol.styleSpecified = true;
      }
      if (fromCol.IsSetWidth())
        toCol.width = fromCol.width;
      if (fromCol.IsSetCollapsed())
        toCol.collapsed = fromCol.collapsed;
      if (fromCol.IsSetPhonetic())
        toCol.phonetic = fromCol.phonetic;
      if (fromCol.IsSetOutlineLevel())
        toCol.outlineLevel = fromCol.outlineLevel;
      if (!fromCol.IsSetCollapsed())
        return;
      toCol.collapsed = fromCol.collapsed;
    }

    public void SetColBestFit(long index, bool bestFit)
    {
      this.GetOrCreateColumn1Based(index + 1L, false).bestFit = bestFit;
    }

    public void SetCustomWidth(long index, bool width)
    {
      this.GetOrCreateColumn1Based(index + 1L, true).customWidth = width;
    }

    public void SetColWidth(long index, double width)
    {
      this.GetOrCreateColumn1Based(index + 1L, true).width = width;
    }

    public void SetColHidden(long index, bool hidden)
    {
      this.GetOrCreateColumn1Based(index + 1L, true).hidden = hidden;
    }

    internal CT_Col GetOrCreateColumn1Based(long index1, bool splitColumns)
    {
      CT_Col ctCol = this.GetColumn1Based(index1, splitColumns);
      if (ctCol == null)
      {
        ctCol = this.worksheet.GetColsArray(0).AddNewCol();
        ctCol.min = (uint) index1;
        ctCol.max = (uint) index1;
      }
      return ctCol;
    }

    public void SetColDefaultStyle(long index, ICellStyle style)
    {
      this.SetColDefaultStyle(index, (int) style.Index);
    }

    public void SetColDefaultStyle(long index, int styleId)
    {
      CT_Col column1Based = this.GetOrCreateColumn1Based(index + 1L, true);
      column1Based.style = (uint) styleId;
      column1Based.styleSpecified = true;
    }

    public int GetColDefaultStyle(long index)
    {
      if (this.GetColumn(index, false) != null)
        return (int) this.GetColumn(index, false).style;
      return -1;
    }

    private bool columnExists(CT_Cols cols, long min, long max)
    {
      for (int index = 0; index < cols.sizeOfColArray(); ++index)
      {
        if ((long) cols.GetColArray(index).min == min && (long) cols.GetColArray(index).max == max)
          return true;
      }
      return false;
    }

    public int GetIndexOfColumn(CT_Cols cols, CT_Col col)
    {
      for (int index = 0; index < cols.sizeOfColArray(); ++index)
      {
        if ((int) cols.GetColArray(index).min == (int) col.min && (int) cols.GetColArray(index).max == (int) col.max)
          return index;
      }
      return -1;
    }
  }
}
