// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFName
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula;
using NPOI.SS.Formula.PTG;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFName : IName
  {
    public static string BUILTIN_PRINT_AREA = "_xlnm.Print_Area";
    public static string BUILTIN_PRINT_TITLE = "_xlnm.Print_Titles";
    public static string BUILTIN_CRITERIA = "_xlnm.Criteria:";
    public static string BUILTIN_EXTRACT = "_xlnm.Extract:";
    public static string BUILTIN_FILTER_DB = "_xlnm._FilterDatabase";
    public static string BUILTIN_CONSOLIDATE_AREA = "_xlnm.Consolidate_Area";
    public static string BUILTIN_DATABASE = "_xlnm.Database";
    public static string BUILTIN_SHEET_TITLE = "_xlnm.Sheet_Title";
    private XSSFWorkbook _workbook;
    private CT_DefinedName _ctName;

    public XSSFName(CT_DefinedName name, XSSFWorkbook workbook)
    {
      this._workbook = workbook;
      this._ctName = name;
    }

    internal CT_DefinedName GetCTName()
    {
      return this._ctName;
    }

    public string NameName
    {
      get
      {
        return this._ctName.name;
      }
      set
      {
        XSSFName.validateName(value);
        int sheetIndex = this.SheetIndex;
        for (int nameIndex = 0; nameIndex < this._workbook.NumberOfNames; ++nameIndex)
        {
          IName nameAt = this._workbook.GetNameAt(nameIndex);
          if (nameAt != this && value.Equals(nameAt.NameName, StringComparison.InvariantCultureIgnoreCase) && sheetIndex == nameAt.SheetIndex)
            throw new ArgumentException("The " + (sheetIndex == -1 ? "workbook" : "sheet") + " already contains this name: " + value);
        }
        this._ctName.name = value;
      }
    }

    public string RefersToFormula
    {
      get
      {
        string str = this._ctName.Value;
        if (str == null || str.Length < 1)
          return (string) null;
        return str;
      }
      set
      {
        XSSFEvaluationWorkbook evaluationWorkbook = XSSFEvaluationWorkbook.Create((IWorkbook) this._workbook);
        FormulaParser.Parse(value, (IFormulaParsingWorkbook) evaluationWorkbook, FormulaType.NAMEDRANGE, this.SheetIndex);
        this._ctName.Value = value;
      }
    }

    public bool IsDeleted
    {
      get
      {
        string refersToFormula = this.RefersToFormula;
        if (refersToFormula == null)
          return false;
        XSSFEvaluationWorkbook evaluationWorkbook = XSSFEvaluationWorkbook.Create((IWorkbook) this._workbook);
        return Ptg.DoesFormulaReferToDeletedCell(FormulaParser.Parse(refersToFormula, (IFormulaParsingWorkbook) evaluationWorkbook, FormulaType.NAMEDRANGE, this.SheetIndex));
      }
    }

    public int SheetIndex
    {
      get
      {
        if (!this._ctName.IsSetLocalSheetId())
          return -1;
        return (int) this._ctName.localSheetId;
      }
      set
      {
        int num = this._workbook.NumberOfSheets - 1;
        if (value < -1 || value > num)
          throw new ArgumentException("Sheet index (" + (object) value + ") is out of range" + (num == -1 ? (object) "" : (object) (" (0.." + (object) num + ")")));
        if (value == -1)
        {
          if (!this._ctName.IsSetLocalSheetId())
            return;
          this._ctName.UnsetLocalSheetId();
        }
        else
        {
          this._ctName.localSheetId = (uint) value;
          this._ctName.localSheetIdSpecified = true;
        }
      }
    }

    public bool Function
    {
      get
      {
        return this._ctName.function;
      }
      set
      {
        this._ctName.function = value;
      }
    }

    public void SetFunction(bool value)
    {
      this.Function = value;
    }

    public int FunctionGroupId
    {
      get
      {
        return (int) this._ctName.functionGroupId;
      }
      set
      {
        this._ctName.functionGroupId = (uint) value;
      }
    }

    public string SheetName
    {
      get
      {
        if (this._ctName.IsSetLocalSheetId())
          return this._workbook.GetSheetName((int) this._ctName.localSheetId);
        return new AreaReference(this.RefersToFormula).FirstCell.SheetName;
      }
    }

    public bool IsFunctionName
    {
      get
      {
        return this.Function;
      }
    }

    public string Comment
    {
      get
      {
        return this._ctName.comment;
      }
      set
      {
        this._ctName.comment = value;
      }
    }

    public override int GetHashCode()
    {
      return this._ctName.ToString().GetHashCode();
    }

    public override bool Equals(object o)
    {
      if (o == this)
        return true;
      if (!(o is XSSFName))
        return false;
      XSSFName xssfName = (XSSFName) o;
      if (this._ctName.name == xssfName.GetCTName().name)
        return (int) this._ctName.localSheetId == (int) xssfName.GetCTName().localSheetId;
      return false;
    }

    private static void validateName(string name)
    {
      if (name.Length == 0)
        throw new ArgumentException("Name cannot be blank");
      char c = name[0];
      if (c != '_' && !char.IsLetter(c) || name.IndexOf(' ') != -1)
        throw new ArgumentException("Invalid name: '" + name + "'; Names must begin with a letter or underscore and not contain spaces");
    }
  }
}
