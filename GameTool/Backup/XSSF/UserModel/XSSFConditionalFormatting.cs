// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFConditionalFormatting
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;

namespace NPOI.XSSF.UserModel
{
  public class XSSFConditionalFormatting : IConditionalFormatting
  {
    private CT_ConditionalFormatting _cf;
    private XSSFSheet _sh;

    internal XSSFConditionalFormatting(XSSFSheet sh)
    {
      this._cf = new CT_ConditionalFormatting();
      this._sh = sh;
    }

    internal XSSFConditionalFormatting(XSSFSheet sh, CT_ConditionalFormatting cf)
    {
      this._cf = cf;
      this._sh = sh;
    }

    internal CT_ConditionalFormatting GetCTConditionalFormatting()
    {
      return this._cf;
    }

    public CellRangeAddress[] GetFormattingRanges()
    {
      List<CellRangeAddress> cellRangeAddressList = new List<CellRangeAddress>();
      foreach (object obj in this._cf.sqref)
      {
        string str = obj.ToString();
        char[] chArray = new char[1]{ ' ' };
        foreach (string reference in str.Split(chArray))
          cellRangeAddressList.Add(CellRangeAddress.ValueOf(reference));
      }
      return cellRangeAddressList.ToArray();
    }

    public void SetRule(int idx, IConditionalFormattingRule cfRule)
    {
      XSSFConditionalFormattingRule conditionalFormattingRule = (XSSFConditionalFormattingRule) cfRule;
      this._cf.GetCfRuleArray(idx).Set(conditionalFormattingRule.GetCTCfRule());
    }

    public void AddRule(IConditionalFormattingRule cfRule)
    {
      this._cf.AddNewCfRule().Set(((XSSFConditionalFormattingRule) cfRule).GetCTCfRule());
    }

    public IConditionalFormattingRule GetRule(int idx)
    {
      return (IConditionalFormattingRule) new XSSFConditionalFormattingRule(this._sh, this._cf.GetCfRuleArray(idx));
    }

    public int NumberOfRules
    {
      get
      {
        return this._cf.sizeOfCfRuleArray();
      }
    }
  }
}
