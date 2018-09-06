// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFDataValidation
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;
using System.Text;

namespace NPOI.XSSF.UserModel
{
  public class XSSFDataValidation : IDataValidation
  {
    internal static Dictionary<int, ST_DataValidationOperator> operatorTypeMappings = new Dictionary<int, ST_DataValidationOperator>();
    internal static Dictionary<ST_DataValidationOperator, int> operatorTypeReverseMappings = new Dictionary<ST_DataValidationOperator, int>();
    internal static Dictionary<int, ST_DataValidationType> validationTypeMappings = new Dictionary<int, ST_DataValidationType>();
    internal static Dictionary<ST_DataValidationType, int> validationTypeReverseMappings = new Dictionary<ST_DataValidationType, int>();
    internal static Dictionary<int, ST_DataValidationErrorStyle> errorStyleMappings = new Dictionary<int, ST_DataValidationErrorStyle>();
    private CT_DataValidation ctDdataValidation;
    private XSSFDataValidationConstraint validationConstraint;
    private CellRangeAddressList regions;

    static XSSFDataValidation()
    {
      XSSFDataValidation.errorStyleMappings[2] = ST_DataValidationErrorStyle.information;
      XSSFDataValidation.errorStyleMappings[0] = ST_DataValidationErrorStyle.stop;
      XSSFDataValidation.errorStyleMappings[1] = ST_DataValidationErrorStyle.warning;
      XSSFDataValidation.operatorTypeMappings[0] = ST_DataValidationOperator.between;
      XSSFDataValidation.operatorTypeMappings[1] = ST_DataValidationOperator.notBetween;
      XSSFDataValidation.operatorTypeMappings[2] = ST_DataValidationOperator.equal;
      XSSFDataValidation.operatorTypeMappings[3] = ST_DataValidationOperator.notEqual;
      XSSFDataValidation.operatorTypeMappings[4] = ST_DataValidationOperator.greaterThan;
      XSSFDataValidation.operatorTypeMappings[6] = ST_DataValidationOperator.greaterThanOrEqual;
      XSSFDataValidation.operatorTypeMappings[5] = ST_DataValidationOperator.lessThan;
      XSSFDataValidation.operatorTypeMappings[7] = ST_DataValidationOperator.lessThanOrEqual;
      foreach (KeyValuePair<int, ST_DataValidationOperator> operatorTypeMapping in XSSFDataValidation.operatorTypeMappings)
        XSSFDataValidation.operatorTypeReverseMappings[operatorTypeMapping.Value] = operatorTypeMapping.Key;
      XSSFDataValidation.validationTypeMappings[7] = ST_DataValidationType.custom;
      XSSFDataValidation.validationTypeMappings[4] = ST_DataValidationType.date;
      XSSFDataValidation.validationTypeMappings[2] = ST_DataValidationType.@decimal;
      XSSFDataValidation.validationTypeMappings[3] = ST_DataValidationType.list;
      XSSFDataValidation.validationTypeMappings[0] = ST_DataValidationType.none;
      XSSFDataValidation.validationTypeMappings[6] = ST_DataValidationType.textLength;
      XSSFDataValidation.validationTypeMappings[5] = ST_DataValidationType.time;
      XSSFDataValidation.validationTypeMappings[1] = ST_DataValidationType.whole;
      foreach (KeyValuePair<int, ST_DataValidationType> validationTypeMapping in XSSFDataValidation.validationTypeMappings)
        XSSFDataValidation.validationTypeReverseMappings[validationTypeMapping.Value] = validationTypeMapping.Key;
    }

    public XSSFDataValidation(CellRangeAddressList regions, CT_DataValidation ctDataValidation)
    {
      this.validationConstraint = this.GetConstraint(ctDataValidation);
      this.ctDdataValidation = ctDataValidation;
      this.regions = regions;
      this.ctDdataValidation.errorStyle = ST_DataValidationErrorStyle.stop;
      this.ctDdataValidation.allowBlank = true;
    }

    public XSSFDataValidation(XSSFDataValidationConstraint constraint, CellRangeAddressList regions, CT_DataValidation ctDataValidation)
    {
      this.validationConstraint = constraint;
      this.ctDdataValidation = ctDataValidation;
      this.regions = regions;
      this.ctDdataValidation.errorStyle = ST_DataValidationErrorStyle.stop;
      this.ctDdataValidation.allowBlank = true;
    }

    internal CT_DataValidation GetCTDataValidation()
    {
      return this.ctDdataValidation;
    }

    public void CreateErrorBox(string title, string text)
    {
      this.ctDdataValidation.errorTitle = title;
      this.ctDdataValidation.error = text;
    }

    public void CreatePromptBox(string title, string text)
    {
      this.ctDdataValidation.promptTitle = title;
      this.ctDdataValidation.prompt = text;
    }

    public bool EmptyCellAllowed
    {
      get
      {
        return this.ctDdataValidation.allowBlank;
      }
      set
      {
        this.ctDdataValidation.allowBlank = value;
      }
    }

    public string ErrorBoxText
    {
      get
      {
        return this.ctDdataValidation.error;
      }
    }

    public string ErrorBoxTitle
    {
      get
      {
        return this.ctDdataValidation.errorTitle;
      }
    }

    public int ErrorStyle
    {
      get
      {
        return (int) this.ctDdataValidation.errorStyle;
      }
      set
      {
        this.ctDdataValidation.errorStyle = XSSFDataValidation.errorStyleMappings[value];
      }
    }

    public string PromptBoxText
    {
      get
      {
        return this.ctDdataValidation.prompt;
      }
    }

    public string PromptBoxTitle
    {
      get
      {
        return this.ctDdataValidation.promptTitle;
      }
    }

    public bool ShowErrorBox
    {
      get
      {
        return this.ctDdataValidation.showErrorMessage;
      }
      set
      {
        this.ctDdataValidation.showErrorMessage = value;
      }
    }

    public bool ShowPromptBox
    {
      get
      {
        return this.ctDdataValidation.showInputMessage;
      }
      set
      {
        this.ctDdataValidation.showInputMessage = value;
      }
    }

    public bool SuppressDropDownArrow
    {
      get
      {
        return !this.ctDdataValidation.showDropDown;
      }
      set
      {
        if (this.validationConstraint.GetValidationType() != 3)
          return;
        this.ctDdataValidation.showDropDown = !value;
      }
    }

    public IDataValidationConstraint ValidationConstraint
    {
      get
      {
        return (IDataValidationConstraint) this.validationConstraint;
      }
    }

    public CellRangeAddressList Regions
    {
      get
      {
        return this.regions;
      }
    }

    public string PrettyPrint()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (CellRangeAddress cellRangeAddress in this.regions.CellRangeAddresses)
        stringBuilder.Append(cellRangeAddress.FormatAsString());
      stringBuilder.Append(" => ");
      stringBuilder.Append(this.validationConstraint.PrettyPrint());
      return stringBuilder.ToString();
    }

    private XSSFDataValidationConstraint GetConstraint(CT_DataValidation ctDataValidation)
    {
      string formula1 = ctDataValidation.formula1;
      string formula2 = ctDataValidation.formula2;
      ST_DataValidationOperator index = ctDataValidation.@operator;
      ST_DataValidationType type = ctDataValidation.type;
      return new XSSFDataValidationConstraint(XSSFDataValidation.validationTypeReverseMappings[type], XSSFDataValidation.operatorTypeReverseMappings[index], formula1, formula2);
    }
  }
}
