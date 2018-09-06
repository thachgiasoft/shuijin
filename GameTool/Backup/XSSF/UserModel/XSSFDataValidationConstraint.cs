// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFDataValidationConstraint
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.Util;
using System;
using System.Text;

namespace NPOI.XSSF.UserModel
{
  public class XSSFDataValidationConstraint : IDataValidationConstraint
  {
    private int validationType = -1;
    private int operator1 = -1;
    private string formula1;
    private string formula2;
    private string[] explicitListOfValues;

    public XSSFDataValidationConstraint(string[] explicitListOfValues)
    {
      if (explicitListOfValues == null || explicitListOfValues.Length == 0)
        throw new ArgumentException("List validation with explicit values must specify at least one value");
      this.validationType = 3;
      this.ExplicitListValues = explicitListOfValues;
      this.Validate();
    }

    public XSSFDataValidationConstraint(int validationType, string formula1)
    {
      this.Formula1 = formula1;
      this.validationType = validationType;
      this.Validate();
    }

    public XSSFDataValidationConstraint(int validationType, int operator1, string formula1)
    {
      this.Formula1 = formula1;
      this.validationType = validationType;
      this.operator1 = operator1;
      this.Validate();
    }

    public XSSFDataValidationConstraint(int validationType, int operator1, string formula1, string formula2)
    {
      this.Formula1 = formula1;
      this.Formula2 = formula2;
      this.validationType = validationType;
      this.operator1 = operator1;
      this.Validate();
      if (3 != validationType)
        return;
      this.explicitListOfValues = formula1.Split(',');
    }

    public string[] ExplicitListValues
    {
      get
      {
        return this.explicitListOfValues;
      }
      set
      {
        this.explicitListOfValues = value;
        if (this.explicitListOfValues == null || this.explicitListOfValues.Length <= 0)
          return;
        StringBuilder stringBuilder = new StringBuilder("\"");
        for (int index = 0; index < value.Length; ++index)
        {
          string str = value[index];
          if (stringBuilder.Length > 1)
            stringBuilder.Append(",");
          stringBuilder.Append(str);
        }
        stringBuilder.Append("\"");
        this.Formula1 = stringBuilder.ToString();
      }
    }

    public string Formula1
    {
      get
      {
        return this.formula1;
      }
      set
      {
        this.formula1 = this.RemoveLeadingEquals(value);
      }
    }

    public string Formula2
    {
      get
      {
        return this.formula2;
      }
      set
      {
        this.formula2 = this.RemoveLeadingEquals(value);
      }
    }

    public int Operator
    {
      get
      {
        return this.operator1;
      }
      set
      {
        this.operator1 = value;
      }
    }

    public int GetValidationType()
    {
      return this.validationType;
    }

    protected string RemoveLeadingEquals(string formula1)
    {
      if (!this.IsFormulaEmpty(formula1) && formula1[0] == '=')
        return formula1.Substring(1);
      return formula1;
    }

    public void Validate()
    {
      if (this.validationType == 0)
        return;
      if (this.validationType == 3)
      {
        if (this.IsFormulaEmpty(this.formula1))
          throw new ArgumentException("A valid formula or a list of values must be specified for list validation.");
      }
      else
      {
        if (this.IsFormulaEmpty(this.formula1))
          throw new ArgumentException("Formula is not specified. Formula is required for all validation types except explicit list validation.");
        if (this.validationType == 7)
          return;
        if (this.operator1 == -1)
          throw new ArgumentException("This validation type requires an operator to be specified.");
        if ((this.operator1 == 0 || this.operator1 == 1) && this.IsFormulaEmpty(this.formula2))
          throw new ArgumentException("Between and not between comparisons require two formulae to be specified.");
      }
    }

    protected bool IsFormulaEmpty(string formula1)
    {
      if (formula1 != null)
        return formula1.Trim().Length == 0;
      return true;
    }

    public string PrettyPrint()
    {
      StringBuilder stringBuilder = new StringBuilder();
      ST_DataValidationType validationTypeMapping = XSSFDataValidation.validationTypeMappings[this.validationType];
      Enum operatorTypeMapping = (Enum) XSSFDataValidation.operatorTypeMappings[this.operator1];
      stringBuilder.Append((object) validationTypeMapping);
      stringBuilder.Append(' ');
      if (this.validationType != 0)
      {
        if (this.validationType != 3 && this.validationType != 0 && this.validationType != 7)
          stringBuilder.Append(",").Append((object) operatorTypeMapping).Append(", ");
        string str = "";
        if (this.validationType == 3 && this.explicitListOfValues != null)
          stringBuilder.Append(str).Append((object) Arrays.AsList((Array) this.explicitListOfValues)).Append(str).Append(' ');
        else
          stringBuilder.Append(str).Append(this.formula1).Append(str).Append(' ');
        if (this.formula2 != null)
          stringBuilder.Append(str).Append(this.formula2).Append(str).Append(' ');
      }
      return stringBuilder.ToString();
    }
  }
}
