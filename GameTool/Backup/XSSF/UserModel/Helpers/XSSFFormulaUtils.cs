// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Helpers.XSSFFormulaUtils
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula;
using NPOI.SS.Formula.PTG;
using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel.Helpers
{
  public class XSSFFormulaUtils
  {
    private XSSFWorkbook _wb;
    private XSSFEvaluationWorkbook _fpwb;

    public XSSFFormulaUtils(XSSFWorkbook wb)
    {
      this._wb = wb;
      this._fpwb = XSSFEvaluationWorkbook.Create((IWorkbook) this._wb);
    }

    public void UpdateSheetName(int sheetIndex, string name)
    {
      IFormulaRenderingWorkbook frwb = (IFormulaRenderingWorkbook) new XSSFFormulaRenderingWorkbook(this._fpwb, sheetIndex, name);
      for (int nameIndex = 0; nameIndex < this._wb.NumberOfNames; ++nameIndex)
      {
        IName nameAt = this._wb.GetNameAt(nameIndex);
        if (nameAt.SheetIndex == -1 || nameAt.SheetIndex == sheetIndex)
          this.UpdateName(nameAt, frwb);
      }
      foreach (ISheet sheet in this._wb)
      {
        foreach (IRow row in sheet)
        {
          foreach (ICell cell in row)
          {
            if (cell.CellType == CellType.FORMULA)
              this.UpdateFormula((XSSFCell) cell, frwb);
          }
        }
      }
    }

    private void UpdateFormula(XSSFCell cell, IFormulaRenderingWorkbook frwb)
    {
      CT_CellFormula f = cell.GetCTCell().f;
      if (f == null)
        return;
      string formula = f.Value;
      if (formula == null || formula.Length <= 0)
        return;
      int sheetIndex = this._wb.GetSheetIndex(cell.Sheet);
      Ptg[] ptgs = FormulaParser.Parse(formula, (IFormulaParsingWorkbook) this._fpwb, FormulaType.CELL, sheetIndex);
      string formulaString = FormulaRenderer.ToFormulaString(frwb, ptgs);
      if (formula.Equals(formulaString))
        return;
      f.Value = formulaString;
    }

    private void UpdateName(IName name, IFormulaRenderingWorkbook frwb)
    {
      string refersToFormula = name.RefersToFormula;
      if (refersToFormula == null)
        return;
      int sheetIndex = name.SheetIndex;
      Ptg[] ptgs = FormulaParser.Parse(refersToFormula, (IFormulaParsingWorkbook) this._fpwb, FormulaType.NAMEDRANGE, sheetIndex);
      string formulaString = FormulaRenderer.ToFormulaString(frwb, ptgs);
      if (refersToFormula.Equals(formulaString))
        return;
      name.RefersToFormula = formulaString;
    }
  }
}
