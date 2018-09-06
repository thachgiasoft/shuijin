// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFEvaluationWorkbook
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS;
using NPOI.SS.Formula;
using NPOI.SS.Formula.PTG;
using NPOI.SS.Formula.Udf;
using NPOI.SS.UserModel;
using NPOI.XSSF.Model;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFEvaluationWorkbook : IFormulaRenderingWorkbook, IEvaluationWorkbook, IFormulaParsingWorkbook
  {
    private XSSFWorkbook _uBook;

    public static XSSFEvaluationWorkbook Create(IWorkbook book)
    {
      if (book == null)
        return (XSSFEvaluationWorkbook) null;
      return new XSSFEvaluationWorkbook(book);
    }

    private XSSFEvaluationWorkbook(IWorkbook book)
    {
      this._uBook = (XSSFWorkbook) book;
    }

    private int ConvertFromExternalSheetIndex(int externSheetIndex)
    {
      return externSheetIndex;
    }

    public int ConvertFromExternSheetIndex(int externSheetIndex)
    {
      return externSheetIndex;
    }

    private int ConvertToExternalSheetIndex(int sheetIndex)
    {
      return sheetIndex;
    }

    public int GetExternalSheetIndex(string sheetName)
    {
      return this.ConvertToExternalSheetIndex(this._uBook.GetSheetIndex(sheetName));
    }

    public IEvaluationName GetName(string name, int sheetIndex)
    {
      for (int index = 0; index < this._uBook.NumberOfNames; ++index)
      {
        IName nameAt = this._uBook.GetNameAt(index);
        string nameName = nameAt.NameName;
        if (name.Equals(nameName, StringComparison.InvariantCultureIgnoreCase) && nameAt.SheetIndex == sheetIndex)
          return (IEvaluationName) new XSSFEvaluationWorkbook.Name(this._uBook.GetNameAt(index), index, (IFormulaParsingWorkbook) this);
      }
      if (sheetIndex != -1)
        return this.GetName(name, -1);
      return (IEvaluationName) null;
    }

    public int GetSheetIndex(IEvaluationSheet EvalSheet)
    {
      return this._uBook.GetSheetIndex((ISheet) ((XSSFEvaluationSheet) EvalSheet).GetXSSFSheet());
    }

    public string GetSheetName(int sheetIndex)
    {
      return this._uBook.GetSheetName(sheetIndex);
    }

    public ExternalName GetExternalName(int externSheetIndex, int externNameIndex)
    {
      throw new NotImplementedException();
    }

    public NameXPtg GetNameXPtg(string name)
    {
      IndexedUDFFinder udfFinder = (IndexedUDFFinder) this.GetUDFFinder();
      if (udfFinder.FindFunction(name) == null)
        return (NameXPtg) null;
      return new NameXPtg(0, udfFinder.GetFunctionIndex(name));
    }

    public string ResolveNameXText(NameXPtg n)
    {
      return ((IndexedUDFFinder) this.GetUDFFinder()).GetFunctionName(n.NameIndex);
    }

    public IEvaluationSheet GetSheet(int sheetIndex)
    {
      return (IEvaluationSheet) new XSSFEvaluationSheet(this._uBook.GetSheetAt(sheetIndex));
    }

    public ExternalSheet GetExternalSheet(int externSheetIndex)
    {
      return (ExternalSheet) null;
    }

    public int GetExternalSheetIndex(string workbookName, string sheetName)
    {
      throw new Exception("not implemented yet");
    }

    public int GetSheetIndex(string sheetName)
    {
      return this._uBook.GetSheetIndex(sheetName);
    }

    public string GetSheetNameByExternSheet(int externSheetIndex)
    {
      return this._uBook.GetSheetName(this.ConvertFromExternalSheetIndex(externSheetIndex));
    }

    public string GetNameText(NamePtg namePtg)
    {
      return this._uBook.GetNameAt(namePtg.Index).NameName;
    }

    public IEvaluationName GetName(NamePtg namePtg)
    {
      int index = namePtg.Index;
      return (IEvaluationName) new XSSFEvaluationWorkbook.Name(this._uBook.GetNameAt(index), index, (IFormulaParsingWorkbook) this);
    }

    public Ptg[] GetFormulaTokens(IEvaluationCell EvalCell)
    {
      XSSFCell xssfCell = ((XSSFEvaluationCell) EvalCell).GetXSSFCell();
      XSSFEvaluationWorkbook evaluationWorkbook = XSSFEvaluationWorkbook.Create((IWorkbook) this._uBook);
      return FormulaParser.Parse(xssfCell.CellFormula, (IFormulaParsingWorkbook) evaluationWorkbook, FormulaType.CELL, this._uBook.GetSheetIndex(xssfCell.Sheet));
    }

    public UDFFinder GetUDFFinder()
    {
      return this._uBook.GetUDFFinder();
    }

    private string CleanXSSFFormulaText(string text)
    {
      text = text.Replace("\\n", "").Replace("\\r", "");
      return text;
    }

    public SpreadsheetVersion GetSpreadsheetVersion()
    {
      return SpreadsheetVersion.EXCEL2007;
    }

    private class Name : IEvaluationName
    {
      private XSSFName _nameRecord;
      private int _index;
      private IFormulaParsingWorkbook _fpBook;

      public Name(IName name, int index, IFormulaParsingWorkbook fpBook)
      {
        this._nameRecord = (XSSFName) name;
        this._index = index;
        this._fpBook = fpBook;
      }

      public Ptg[] NameDefinition
      {
        get
        {
          return FormulaParser.Parse(this._nameRecord.RefersToFormula, this._fpBook, FormulaType.NAMEDRANGE, this._nameRecord.SheetIndex);
        }
      }

      public string NameText
      {
        get
        {
          return this._nameRecord.NameName;
        }
      }

      public bool HasFormula
      {
        get
        {
          CT_DefinedName ctName = this._nameRecord.GetCTName();
          string str = ctName.Value;
          if (!ctName.function && str != null)
            return str.Length > 0;
          return false;
        }
      }

      public bool IsFunctionName
      {
        get
        {
          return this._nameRecord.IsFunctionName;
        }
      }

      public bool IsRange
      {
        get
        {
          return this.HasFormula;
        }
      }

      public NamePtg CreatePtg()
      {
        return new NamePtg(this._index);
      }
    }
  }
}
