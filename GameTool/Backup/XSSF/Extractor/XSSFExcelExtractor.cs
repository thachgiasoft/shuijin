// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Extractor.XSSFExcelExtractor
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.Globalization;
using System.Text;

namespace NPOI.XSSF.Extractor
{
  public class XSSFExcelExtractor : POIXMLTextExtractor, NPOI.SS.Extractor.ExcelExtractor
  {
    public static XSSFRelation[] SUPPORTED_TYPES = new XSSFRelation[5]{ XSSFRelation.WORKBOOK, XSSFRelation.MACRO_TEMPLATE_WORKBOOK, XSSFRelation.MACRO_ADDIN_WORKBOOK, XSSFRelation.TEMPLATE_WORKBOOK, XSSFRelation.MACROS_WORKBOOK };
    private bool includeSheetNames = true;
    private bool includeHeadersFooters = true;
    private XSSFWorkbook workbook;
    private bool formulasNotResults;
    private bool includeCellComments;
    private CultureInfo locale;

    public XSSFExcelExtractor(string path)
      : this(new XSSFWorkbook(path))
    {
    }

    public XSSFExcelExtractor(OPCPackage Container)
      : this(new XSSFWorkbook(Container))
    {
    }

    public XSSFExcelExtractor(XSSFWorkbook workbook)
      : base((POIXMLDocument) workbook)
    {
      this.workbook = workbook;
    }

    public void SetIncludeSheetNames(bool includeSheetNames)
    {
      this.includeSheetNames = includeSheetNames;
    }

    public void SetFormulasNotResults(bool formulasNotResults)
    {
      this.formulasNotResults = formulasNotResults;
    }

    public void SetIncludeCellComments(bool includeCellComments)
    {
      this.includeCellComments = includeCellComments;
    }

    public void SetIncludeHeadersFooters(bool includeHeadersFooters)
    {
      this.includeHeadersFooters = includeHeadersFooters;
    }

    public void SetLocale(CultureInfo locale)
    {
      this.locale = locale;
    }

    public override string Text
    {
      get
      {
        DataFormatter formatter = this.locale != null ? new DataFormatter(this.locale) : new DataFormatter();
        StringBuilder text = new StringBuilder();
        for (int index = 0; index < this.workbook.NumberOfSheets; ++index)
        {
          XSSFSheet sheetAt = (XSSFSheet) this.workbook.GetSheetAt(index);
          if (this.includeSheetNames)
            text.Append(this.workbook.GetSheetName(index) + "\n");
          if (this.includeHeadersFooters)
          {
            text.Append(this.ExtractHeaderFooter((IHeaderFooter) sheetAt.FirstHeader));
            text.Append(this.ExtractHeaderFooter((IHeaderFooter) sheetAt.OddHeader));
            text.Append(this.ExtractHeaderFooter((IHeaderFooter) sheetAt.EvenHeader));
          }
          foreach (IRow row in sheetAt)
          {
            IEnumerator enumerator = row.GetEnumerator();
            bool flag = true;
            while (enumerator.MoveNext())
            {
              if (!flag)
                text.Append("\t");
              else
                flag = false;
              ICell current = (ICell) enumerator.Current;
              if (current.CellType == CellType.FORMULA && this.formulasNotResults)
                text.Append(current.CellFormula);
              if (current.CellType == CellType.FORMULA)
              {
                if (this.formulasNotResults)
                  text.Append(current.CellFormula);
                else if (current.CachedFormulaResultType == CellType.STRING)
                  this.HandleStringCell(text, current);
                else
                  this.HandleNonStringCell(text, current, formatter);
              }
              else if (current.CellType == CellType.STRING)
                this.HandleStringCell(text, current);
              else
                this.HandleNonStringCell(text, current, formatter);
              IComment cellComment = current.CellComment;
              if (this.includeCellComments && cellComment != null)
              {
                string str = cellComment.String.String.Replace('\n', ' ');
                text.Append(" Comment by ").Append(cellComment.Author).Append(": ").Append(str);
              }
            }
            text.Append("\n");
          }
          if (this.includeHeadersFooters)
          {
            text.Append(this.ExtractHeaderFooter((IHeaderFooter) sheetAt.FirstFooter));
            text.Append(this.ExtractHeaderFooter((IHeaderFooter) sheetAt.OddFooter));
            text.Append(this.ExtractHeaderFooter((IHeaderFooter) sheetAt.EvenFooter));
          }
        }
        return text.ToString();
      }
    }

    private void HandleStringCell(StringBuilder text, ICell cell)
    {
      text.Append(cell.RichStringCellValue.String);
    }

    private void HandleNonStringCell(StringBuilder text, ICell cell, DataFormatter formatter)
    {
      CellType cellType = cell.CellType;
      if (cellType == CellType.FORMULA)
        cellType = cell.CachedFormulaResultType;
      if (cellType == CellType.NUMERIC)
      {
        ICellStyle cellStyle = cell.CellStyle;
        if (cellStyle.GetDataFormatString() != null)
        {
          text.Append(formatter.FormatRawCellContents(cell.NumericCellValue, (int) cellStyle.DataFormat, cellStyle.GetDataFormatString()));
          return;
        }
      }
      XSSFCell xssfCell = (XSSFCell) cell;
      text.Append(xssfCell.GetRawValue());
    }

    private string ExtractHeaderFooter(IHeaderFooter hf)
    {
      return NPOI.HSSF.Extractor.ExcelExtractor.ExtractHeaderFooter(hf);
    }
  }
}
