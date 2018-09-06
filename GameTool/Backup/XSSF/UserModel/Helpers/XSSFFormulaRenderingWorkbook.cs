// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Helpers.XSSFFormulaRenderingWorkbook
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.SS.Formula;
using NPOI.SS.Formula.PTG;

namespace NPOI.XSSF.UserModel.Helpers
{
  internal class XSSFFormulaRenderingWorkbook : IFormulaRenderingWorkbook
  {
    private XSSFEvaluationWorkbook _fpwb;
    private int _sheetIndex;
    private string _name;

    public XSSFFormulaRenderingWorkbook(XSSFEvaluationWorkbook fpwb, int sheetIndex, string name)
    {
      this._fpwb = fpwb;
      this._sheetIndex = sheetIndex;
      this._name = name;
    }

    public ExternalSheet GetExternalSheet(int externSheetIndex)
    {
      return this._fpwb.GetExternalSheet(externSheetIndex);
    }

    public string GetSheetNameByExternSheet(int externSheetIndex)
    {
      if (externSheetIndex == this._sheetIndex)
        return this._name;
      return this._fpwb.GetSheetNameByExternSheet(externSheetIndex);
    }

    public string ResolveNameXText(NameXPtg nameXPtg)
    {
      return this._fpwb.ResolveNameXText(nameXPtg);
    }

    public string GetNameText(NamePtg namePtg)
    {
      return this._fpwb.GetNameText(namePtg);
    }
  }
}
