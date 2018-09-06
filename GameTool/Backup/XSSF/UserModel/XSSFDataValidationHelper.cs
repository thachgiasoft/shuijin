// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFDataValidationHelper
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;

namespace NPOI.XSSF.UserModel
{
  public class XSSFDataValidationHelper : IDataValidationHelper
  {
    private XSSFSheet xssfSheet;

    public XSSFDataValidationHelper(XSSFSheet xssfSheet)
    {
      this.xssfSheet = xssfSheet;
    }

    public IDataValidationConstraint CreateDateConstraint(int operatorType, string formula1, string formula2, string dateFormat)
    {
      return (IDataValidationConstraint) new XSSFDataValidationConstraint(4, operatorType, formula1, formula2);
    }

    public IDataValidationConstraint CreateDecimalConstraint(int operatorType, string formula1, string formula2)
    {
      return (IDataValidationConstraint) new XSSFDataValidationConstraint(2, operatorType, formula1, formula2);
    }

    public IDataValidationConstraint CreateExplicitListConstraint(string[] listOfValues)
    {
      return (IDataValidationConstraint) new XSSFDataValidationConstraint(listOfValues);
    }

    public IDataValidationConstraint CreateFormulaListConstraint(string listFormula)
    {
      return (IDataValidationConstraint) new XSSFDataValidationConstraint(3, listFormula);
    }

    public IDataValidationConstraint CreateNumericConstraint(int validationType, int operatorType, string formula1, string formula2)
    {
      switch (validationType)
      {
        case 1:
          return this.CreateintConstraint(operatorType, formula1, formula2);
        case 2:
          return this.CreateDecimalConstraint(operatorType, formula1, formula2);
        case 6:
          return this.CreateTextLengthConstraint(operatorType, formula1, formula2);
        default:
          return (IDataValidationConstraint) null;
      }
    }

    public IDataValidationConstraint CreateintConstraint(int operatorType, string formula1, string formula2)
    {
      return (IDataValidationConstraint) new XSSFDataValidationConstraint(1, operatorType, formula1, formula2);
    }

    public IDataValidationConstraint CreateTextLengthConstraint(int operatorType, string formula1, string formula2)
    {
      return (IDataValidationConstraint) new XSSFDataValidationConstraint(6, operatorType, formula1, formula2);
    }

    public IDataValidationConstraint CreateTimeConstraint(int operatorType, string formula1, string formula2)
    {
      return (IDataValidationConstraint) new XSSFDataValidationConstraint(5, operatorType, formula1, formula2);
    }

    public IDataValidationConstraint CreateCustomConstraint(string formula)
    {
      return (IDataValidationConstraint) new XSSFDataValidationConstraint(7, formula);
    }

    public IDataValidation CreateValidation(IDataValidationConstraint constraint, CellRangeAddressList cellRangeAddressList)
    {
      XSSFDataValidationConstraint constraint1 = (XSSFDataValidationConstraint) constraint;
      CT_DataValidation ctDataValidation = new CT_DataValidation();
      int validationType = constraint.GetValidationType();
      switch (validationType)
      {
        case 0:
          ctDataValidation.type = ST_DataValidationType.none;
          break;
        case 1:
          ctDataValidation.type = ST_DataValidationType.whole;
          break;
        case 2:
          ctDataValidation.type = ST_DataValidationType.@decimal;
          break;
        case 3:
          ctDataValidation.type = ST_DataValidationType.list;
          ctDataValidation.formula1 = constraint.Formula1;
          break;
        case 4:
          ctDataValidation.type = ST_DataValidationType.date;
          break;
        case 5:
          ctDataValidation.type = ST_DataValidationType.time;
          break;
        case 6:
          ctDataValidation.type = ST_DataValidationType.textLength;
          break;
        case 7:
          ctDataValidation.type = ST_DataValidationType.custom;
          break;
        default:
          ctDataValidation.type = ST_DataValidationType.none;
          break;
      }
      if (validationType != 0 && validationType != 3)
      {
        ctDataValidation.@operator = ST_DataValidationOperator.between;
        if (XSSFDataValidation.operatorTypeMappings.ContainsKey(constraint.Operator))
          ctDataValidation.@operator = XSSFDataValidation.operatorTypeMappings[constraint.Operator];
        if (constraint.Formula1 != null)
          ctDataValidation.formula1 = constraint.Formula1;
        if (constraint.Formula2 != null)
          ctDataValidation.formula2 = constraint.Formula2;
      }
      CellRangeAddress[] cellRangeAddresses = cellRangeAddressList.CellRangeAddresses;
      List<string> stringList = new List<string>();
      for (int index = 0; index < cellRangeAddresses.Length; ++index)
      {
        CellRangeAddress cellRangeAddress = cellRangeAddresses[index];
        stringList.Add(cellRangeAddress.FormatAsString());
      }
      ctDataValidation.sqref = stringList;
      return (IDataValidation) new XSSFDataValidation(constraint1, cellRangeAddressList, ctDataValidation);
    }
  }
}
