// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Helpers.XSSFRowShifter
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula;
using NPOI.SS.Formula.PTG;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;

namespace NPOI.XSSF.UserModel.Helpers
{
  public class XSSFRowShifter
  {
    private XSSFSheet sheet;

    public XSSFRowShifter(XSSFSheet sh)
    {
      this.sheet = sh;
    }

    public List<CellRangeAddress> ShiftMerged(int startRow, int endRow, int n)
    {
      List<CellRangeAddress> cellRangeAddressList = new List<CellRangeAddress>();
      for (int index = 0; index < this.sheet.NumMergedRegions; ++index)
      {
        CellRangeAddress mergedRegion = this.sheet.GetMergedRegion(index);
        if ((mergedRegion.FirstRow >= startRow || mergedRegion.LastRow >= startRow) && (mergedRegion.FirstRow <= endRow || mergedRegion.LastRow <= endRow) && (!XSSFRowShifter.ContainsCell(mergedRegion, startRow - 1, 0) && !XSSFRowShifter.ContainsCell(mergedRegion, endRow + 1, 0)))
        {
          mergedRegion.FirstRow += n;
          mergedRegion.LastRow += n;
          cellRangeAddressList.Add(mergedRegion);
          this.sheet.RemoveMergedRegion(index);
          --index;
        }
      }
      foreach (CellRangeAddress region in cellRangeAddressList)
        this.sheet.AddMergedRegion(region);
      return cellRangeAddressList;
    }

    private static bool ContainsCell(CellRangeAddress cr, int rowIx, int colIx)
    {
      return cr.FirstRow <= rowIx && cr.LastRow >= rowIx && (cr.FirstColumn <= colIx && cr.LastColumn >= colIx);
    }

    public void UpdateNamedRanges(FormulaShifter shifter)
    {
      IWorkbook workbook = this.sheet.Workbook;
      XSSFEvaluationWorkbook evaluationWorkbook = XSSFEvaluationWorkbook.Create(workbook);
      for (int nameIndex = 0; nameIndex < workbook.NumberOfNames; ++nameIndex)
      {
        IName nameAt = workbook.GetNameAt(nameIndex);
        string refersToFormula = nameAt.RefersToFormula;
        int sheetIndex = nameAt.SheetIndex;
        Ptg[] ptgs = FormulaParser.Parse(refersToFormula, (IFormulaParsingWorkbook) evaluationWorkbook, FormulaType.NAMEDRANGE, sheetIndex);
        if (shifter.AdjustFormula(ptgs, sheetIndex))
        {
          string formulaString = FormulaRenderer.ToFormulaString((IFormulaRenderingWorkbook) evaluationWorkbook, ptgs);
          nameAt.RefersToFormula = formulaString;
        }
      }
    }

    public void UpdateFormulas(FormulaShifter shifter)
    {
      this.UpdateSheetFormulas(this.sheet, shifter);
      foreach (XSSFSheet sh in this.sheet.Workbook)
      {
        if (this.sheet != sh)
          this.UpdateSheetFormulas(sh, shifter);
      }
    }

    private void UpdateSheetFormulas(XSSFSheet sh, FormulaShifter Shifter)
    {
      foreach (XSSFRow row in sh)
        this.updateRowFormulas(row, Shifter);
    }

    private void updateRowFormulas(XSSFRow row, FormulaShifter Shifter)
    {
      foreach (XSSFCell xssfCell in row)
      {
        CT_Cell ctCell = xssfCell.GetCTCell();
        if (ctCell.IsSetF())
        {
          CT_CellFormula f = ctCell.f;
          string formula1 = f.Value;
          if (formula1.Length > 0)
          {
            string str = XSSFRowShifter.ShiftFormula(row, formula1, Shifter);
            if (str != null)
              f.Value = str;
          }
          if (f.isSetRef())
          {
            string formula2 = f.@ref;
            string str = XSSFRowShifter.ShiftFormula(row, formula2, Shifter);
            if (str != null)
              f.@ref = str;
          }
        }
      }
    }

    private static string ShiftFormula(XSSFRow row, string formula, FormulaShifter Shifter)
    {
      ISheet sheet = row.Sheet;
      IWorkbook workbook = sheet.Workbook;
      int sheetIndex = workbook.GetSheetIndex(sheet);
      XSSFEvaluationWorkbook evaluationWorkbook = XSSFEvaluationWorkbook.Create(workbook);
      Ptg[] ptgs = FormulaParser.Parse(formula, (IFormulaParsingWorkbook) evaluationWorkbook, FormulaType.CELL, sheetIndex);
      string str = (string) null;
      if (Shifter.AdjustFormula(ptgs, sheetIndex))
        str = FormulaRenderer.ToFormulaString((IFormulaRenderingWorkbook) evaluationWorkbook, ptgs);
      return str;
    }

    public void UpdateConditionalFormatting(FormulaShifter Shifter)
    {
      IWorkbook workbook = this.sheet.Workbook;
      int sheetIndex = workbook.GetSheetIndex((ISheet) this.sheet);
      XSSFEvaluationWorkbook evaluationWorkbook = XSSFEvaluationWorkbook.Create(workbook);
      List<CT_ConditionalFormatting> conditionalFormatting1 = this.sheet.GetCTWorksheet().conditionalFormatting;
      for (int index1 = 0; conditionalFormatting1 != null && index1 < conditionalFormatting1.Count; ++index1)
      {
        CT_ConditionalFormatting conditionalFormatting2 = conditionalFormatting1[index1];
        List<CellRangeAddress> cellRangeAddressList1 = new List<CellRangeAddress>();
        foreach (object obj in conditionalFormatting2.sqref)
        {
          string str = obj.ToString();
          char[] chArray = new char[1]{ ' ' };
          foreach (string reference in str.Split(chArray))
            cellRangeAddressList1.Add(CellRangeAddress.ValueOf(reference));
        }
        bool flag = false;
        List<CellRangeAddress> cellRangeAddressList2 = new List<CellRangeAddress>();
        for (int index2 = 0; index2 < cellRangeAddressList1.Count; ++index2)
        {
          CellRangeAddress cra = cellRangeAddressList1[index2];
          CellRangeAddress cellRangeAddress = XSSFRowShifter.ShiftRange(Shifter, cra, sheetIndex);
          if (cellRangeAddress == null)
          {
            flag = true;
          }
          else
          {
            cellRangeAddressList2.Add(cellRangeAddress);
            if (cellRangeAddress != cra)
              flag = true;
          }
        }
        if (flag)
        {
          if (cellRangeAddressList2.Count == 0)
          {
            conditionalFormatting1.RemoveAt(index1);
            continue;
          }
          List<string> stringList = new List<string>();
          foreach (CellRangeAddress cellRangeAddress in cellRangeAddressList2)
            stringList.Add(cellRangeAddress.FormatAsString());
          conditionalFormatting2.sqref = stringList;
        }
        foreach (CT_CfRule ctCfRule in conditionalFormatting2.cfRule)
        {
          List<string> formula = ctCfRule.formula;
          for (int index2 = 0; index2 < formula.Count; ++index2)
          {
            Ptg[] ptgs = FormulaParser.Parse(formula[index2], (IFormulaParsingWorkbook) evaluationWorkbook, FormulaType.CELL, sheetIndex);
            if (Shifter.AdjustFormula(ptgs, sheetIndex))
            {
              string formulaString = FormulaRenderer.ToFormulaString((IFormulaRenderingWorkbook) evaluationWorkbook, ptgs);
              formula[index2] = formulaString;
            }
          }
        }
      }
    }

    private static CellRangeAddress ShiftRange(FormulaShifter Shifter, CellRangeAddress cra, int currentExternSheetIx)
    {
      Ptg[] ptgs = new Ptg[1]{ (Ptg) new AreaPtg(cra.FirstRow, cra.LastRow, cra.FirstColumn, cra.LastColumn, false, false, false, false) };
      if (!Shifter.AdjustFormula(ptgs, currentExternSheetIx))
        return cra;
      Ptg ptg = ptgs[0];
      if (ptg is AreaPtg)
      {
        AreaPtg areaPtg = (AreaPtg) ptg;
        return new CellRangeAddress(areaPtg.FirstRow, areaPtg.LastRow, areaPtg.FirstColumn, areaPtg.LastColumn);
      }
      if (ptg is AreaErrPtg)
        return (CellRangeAddress) null;
      throw new InvalidOperationException("Unexpected Shifted ptg class (" + ptg.GetType().Name + ")");
    }
  }
}
