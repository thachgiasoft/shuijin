// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFSheetConditionalFormatting
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.HSSF.Record.CF;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;

namespace NPOI.XSSF.UserModel
{
  public class XSSFSheetConditionalFormatting : ISheetConditionalFormatting
  {
    private XSSFSheet _sheet;

    internal XSSFSheetConditionalFormatting(XSSFSheet sheet)
    {
      this._sheet = sheet;
    }

    public IConditionalFormattingRule CreateConditionalFormattingRule(ComparisonOperator comparisonOperation, string formula1, string formula2)
    {
      XSSFConditionalFormattingRule conditionalFormattingRule = new XSSFConditionalFormattingRule(this._sheet);
      CT_CfRule ctCfRule = conditionalFormattingRule.GetCTCfRule();
      ctCfRule.AddFormula(formula1);
      if (formula2 != null)
        ctCfRule.AddFormula(formula2);
      ctCfRule.type = ST_CfType.cellIs;
      ST_ConditionalFormattingOperator formattingOperator;
      switch (comparisonOperation)
      {
        case ComparisonOperator.BETWEEN:
          formattingOperator = ST_ConditionalFormattingOperator.between;
          break;
        case ComparisonOperator.NOT_BETWEEN:
          formattingOperator = ST_ConditionalFormattingOperator.notBetween;
          break;
        case ComparisonOperator.EQUAL:
          formattingOperator = ST_ConditionalFormattingOperator.equal;
          break;
        case ComparisonOperator.NOT_EQUAL:
          formattingOperator = ST_ConditionalFormattingOperator.notEqual;
          break;
        case ComparisonOperator.GT:
          formattingOperator = ST_ConditionalFormattingOperator.greaterThan;
          break;
        case ComparisonOperator.LT:
          formattingOperator = ST_ConditionalFormattingOperator.lessThan;
          break;
        case ComparisonOperator.GE:
          formattingOperator = ST_ConditionalFormattingOperator.greaterThanOrEqual;
          break;
        case ComparisonOperator.LE:
          formattingOperator = ST_ConditionalFormattingOperator.lessThanOrEqual;
          break;
        default:
          throw new ArgumentException("Unknown comparison operator: " + (object) comparisonOperation);
      }
      ctCfRule.@operator = formattingOperator;
      return (IConditionalFormattingRule) conditionalFormattingRule;
    }

    public IConditionalFormattingRule CreateConditionalFormattingRule(ComparisonOperator comparisonOperation, string formula)
    {
      return this.CreateConditionalFormattingRule(comparisonOperation, formula, (string) null);
    }

    public IConditionalFormattingRule CreateConditionalFormattingRule(string formula)
    {
      XSSFConditionalFormattingRule conditionalFormattingRule = new XSSFConditionalFormattingRule(this._sheet);
      CT_CfRule ctCfRule = conditionalFormattingRule.GetCTCfRule();
      ctCfRule.AddFormula(formula);
      ctCfRule.type = ST_CfType.expression;
      return (IConditionalFormattingRule) conditionalFormattingRule;
    }

    public int AddConditionalFormatting(CellRangeAddress[] regions, IConditionalFormattingRule[] cfRules)
    {
      if (regions == null)
        throw new ArgumentException("regions must not be null");
      foreach (CellRangeAddressBase region in regions)
        region.Validate(SpreadsheetVersion.EXCEL2007);
      if (cfRules == null)
        throw new ArgumentException("cfRules must not be null");
      if (cfRules.Length == 0)
        throw new ArgumentException("cfRules must not be empty");
      if (cfRules.Length > 3)
        throw new ArgumentException("Number of rules must not exceed 3");
      if (!(cfRules is XSSFConditionalFormattingRule[]))
      {
        XSSFConditionalFormattingRule[] conditionalFormattingRuleArray = new XSSFConditionalFormattingRule[cfRules.Length];
        Array.Copy((Array) cfRules, 0, (Array) conditionalFormattingRuleArray, 0, conditionalFormattingRuleArray.Length);
      }
      CellRangeAddress[] cellRangeAddressArray = CellRangeUtil.MergeCellRanges(regions);
      CT_ConditionalFormatting conditionalFormatting1 = this._sheet.GetCTWorksheet().AddNewConditionalFormatting();
      List<string> stringList = new List<string>();
      foreach (CellRangeAddress cellRangeAddress in cellRangeAddressArray)
        stringList.Add(cellRangeAddress.FormatAsString());
      conditionalFormatting1.sqref = stringList;
      int num = 1;
      foreach (CT_ConditionalFormatting conditionalFormatting2 in this._sheet.GetCTWorksheet().conditionalFormatting)
        num += conditionalFormatting2.sizeOfCfRuleArray();
      foreach (XSSFConditionalFormattingRule cfRule in cfRules)
      {
        cfRule.GetCTCfRule().priority = num++;
        conditionalFormatting1.AddNewCfRule().Set(cfRule.GetCTCfRule());
      }
      return this._sheet.GetCTWorksheet().SizeOfConditionalFormattingArray() - 1;
    }

    public int AddConditionalFormatting(CellRangeAddress[] regions, IConditionalFormattingRule rule1)
    {
      CellRangeAddress[] regions1 = regions;
      XSSFConditionalFormattingRule[] conditionalFormattingRuleArray;
      if (rule1 != null)
        conditionalFormattingRuleArray = new XSSFConditionalFormattingRule[1]
        {
          (XSSFConditionalFormattingRule) rule1
        };
      else
        conditionalFormattingRuleArray = (XSSFConditionalFormattingRule[]) null;
      return this.AddConditionalFormatting(regions1, (IConditionalFormattingRule[]) conditionalFormattingRuleArray);
    }

    public int AddConditionalFormatting(CellRangeAddress[] regions, IConditionalFormattingRule rule1, IConditionalFormattingRule rule2)
    {
      CellRangeAddress[] regions1 = regions;
      XSSFConditionalFormattingRule[] conditionalFormattingRuleArray;
      if (rule1 != null)
        conditionalFormattingRuleArray = new XSSFConditionalFormattingRule[2]
        {
          (XSSFConditionalFormattingRule) rule1,
          (XSSFConditionalFormattingRule) rule2
        };
      else
        conditionalFormattingRuleArray = (XSSFConditionalFormattingRule[]) null;
      return this.AddConditionalFormatting(regions1, (IConditionalFormattingRule[]) conditionalFormattingRuleArray);
    }

    public int AddConditionalFormatting(IConditionalFormatting cf)
    {
      XSSFConditionalFormatting conditionalFormatting = (XSSFConditionalFormatting) cf;
      CT_Worksheet ctWorksheet = this._sheet.GetCTWorksheet();
      ctWorksheet.AddNewConditionalFormatting().Set(conditionalFormatting.GetCTConditionalFormatting());
      return ctWorksheet.SizeOfConditionalFormattingArray() - 1;
    }

    public IConditionalFormatting GetConditionalFormattingAt(int index)
    {
      this.CheckIndex(index);
      return (IConditionalFormatting) new XSSFConditionalFormatting(this._sheet, this._sheet.GetCTWorksheet().GetConditionalFormattingArray(index));
    }

    public int NumConditionalFormattings
    {
      get
      {
        return this._sheet.GetCTWorksheet().SizeOfConditionalFormattingArray();
      }
    }

    public void RemoveConditionalFormatting(int index)
    {
      this.CheckIndex(index);
      this._sheet.GetCTWorksheet().conditionalFormatting.RemoveAt(index);
    }

    private void CheckIndex(int index)
    {
      int conditionalFormattings = this.NumConditionalFormattings;
      if (index < 0 || index >= conditionalFormattings)
        throw new ArgumentException("Specified CF index " + (object) index + " is outside the allowable range (0.." + (object) (conditionalFormattings - 1) + ")");
    }
  }
}
