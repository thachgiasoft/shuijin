// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFConditionalFormattingRule
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.Model;

namespace NPOI.XSSF.UserModel
{
  public class XSSFConditionalFormattingRule : IConditionalFormattingRule
  {
    private CT_CfRule _cfRule;
    private XSSFSheet _sh;

    internal XSSFConditionalFormattingRule(XSSFSheet sh)
    {
      this._cfRule = new CT_CfRule();
      this._sh = sh;
    }

    internal XSSFConditionalFormattingRule(XSSFSheet sh, CT_CfRule cfRule)
    {
      this._cfRule = cfRule;
      this._sh = sh;
    }

    internal CT_CfRule GetCTCfRule()
    {
      return this._cfRule;
    }

    internal CT_Dxf GetDxf(bool create)
    {
      StylesTable stylesSource = ((XSSFWorkbook) this._sh.Workbook).GetStylesSource();
      CT_Dxf dxf = (CT_Dxf) null;
      if (stylesSource._GetDXfsSize() > 0 && this._cfRule.IsSetDxfId())
      {
        int dxfId = (int) this._cfRule.dxfId;
        dxf = stylesSource.GetDxfAt(dxfId);
      }
      if (create && dxf == null)
      {
        dxf = new CT_Dxf();
        this._cfRule.dxfId = (uint) (stylesSource.PutDxf(dxf) - 1);
      }
      return dxf;
    }

    public IBorderFormatting CreateBorderFormatting()
    {
      CT_Dxf dxf = this.GetDxf(true);
      return (IBorderFormatting) new XSSFBorderFormatting(dxf.IsSetBorder() ? dxf.border : dxf.AddNewBorder());
    }

    public IBorderFormatting GetBorderFormatting()
    {
      CT_Dxf dxf = this.GetDxf(false);
      if (dxf == null || !dxf.IsSetBorder())
        return (IBorderFormatting) null;
      return (IBorderFormatting) new XSSFBorderFormatting(dxf.border);
    }

    public IFontFormatting CreateFontFormatting()
    {
      CT_Dxf dxf = this.GetDxf(true);
      return (IFontFormatting) new XSSFFontFormatting(dxf.IsSetFont() ? dxf.font : dxf.AddNewFont());
    }

    public IFontFormatting GetFontFormatting()
    {
      CT_Dxf dxf = this.GetDxf(false);
      if (dxf == null || !dxf.IsSetFont())
        return (IFontFormatting) null;
      return (IFontFormatting) new XSSFFontFormatting(dxf.font);
    }

    public IPatternFormatting CreatePatternFormatting()
    {
      CT_Dxf dxf = this.GetDxf(true);
      return (IPatternFormatting) new XSSFPatternFormatting(dxf.IsSetFill() ? dxf.fill : dxf.AddNewFill());
    }

    public IPatternFormatting GetPatternFormatting()
    {
      CT_Dxf dxf = this.GetDxf(false);
      if (dxf == null || !dxf.IsSetFill())
        return (IPatternFormatting) null;
      return (IPatternFormatting) new XSSFPatternFormatting(dxf.fill);
    }

    public ConditionType ConditionType
    {
      get
      {
        switch (this._cfRule.type)
        {
          case ST_CfType.expression:
            return ConditionType.FORMULA;
          case ST_CfType.cellIs:
            return ConditionType.CELL_VALUE_IS;
          default:
            return ConditionType.None;
        }
      }
    }

    public ComparisonOperator ComparisonOperation
    {
      get
      {
        switch (this._cfRule.@operator)
        {
          case ST_ConditionalFormattingOperator.lessThan:
            return ComparisonOperator.LT;
          case ST_ConditionalFormattingOperator.lessThanOrEqual:
            return ComparisonOperator.LE;
          case ST_ConditionalFormattingOperator.equal:
            return ComparisonOperator.EQUAL;
          case ST_ConditionalFormattingOperator.notEqual:
            return ComparisonOperator.NOT_EQUAL;
          case ST_ConditionalFormattingOperator.greaterThanOrEqual:
            return ComparisonOperator.GE;
          case ST_ConditionalFormattingOperator.greaterThan:
            return ComparisonOperator.GT;
          case ST_ConditionalFormattingOperator.between:
            return ComparisonOperator.BETWEEN;
          case ST_ConditionalFormattingOperator.notBetween:
            return ComparisonOperator.NOT_BETWEEN;
          default:
            return ComparisonOperator.NO_COMPARISON;
        }
      }
    }

    public string Formula1
    {
      get
      {
        if (this._cfRule.sizeOfFormulaArray() <= 0)
          return (string) null;
        return this._cfRule.GetFormulaArray(0);
      }
    }

    public string Formula2
    {
      get
      {
        if (this._cfRule.sizeOfFormulaArray() != 2)
          return (string) null;
        return this._cfRule.GetFormulaArray(1);
      }
    }
  }
}
