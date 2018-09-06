// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Model.StylesTable
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.UserModel.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace NPOI.XSSF.Model
{
  public class StylesTable : POIXMLDocumentPart
  {
    public static int FIRST_CUSTOM_STYLE_ID = 165;
    private Dictionary<int, string> numberFormats = new Dictionary<int, string>();
    private List<XSSFFont> fonts = new List<XSSFFont>();
    private List<XSSFCellFill> fills = new List<XSSFCellFill>();
    private List<XSSFCellBorder> borders = new List<XSSFCellBorder>();
    private List<CT_Xf> styleXfs = new List<CT_Xf>();
    private List<CT_Xf> xfs = new List<CT_Xf>();
    private List<CT_Dxf> dxfs = new List<CT_Dxf>();
    private StyleSheetDocument doc;
    private ThemesTable theme;

    public StylesTable()
    {
      this.doc = new StyleSheetDocument();
      this.doc.AddNewStyleSheet();
      this.Initialize();
    }

    internal StylesTable(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.ReadFrom(part.GetInputStream());
    }

    public ThemesTable GetTheme()
    {
      return this.theme;
    }

    public void SetTheme(ThemesTable theme)
    {
      this.theme = theme;
      foreach (XSSFFont font in this.fonts)
        font.SetThemesTable(theme);
      foreach (XSSFCellBorder border in this.borders)
        border.SetThemesTable(theme);
    }

    protected void ReadFrom(Stream is1)
    {
      try
      {
        this.doc = StyleSheetDocument.Parse(is1);
        CT_Stylesheet styleSheet = this.doc.GetStyleSheet();
        CT_NumFmts numFmts = styleSheet.numFmts;
        if (numFmts != null)
        {
          foreach (CT_NumFmt ctNumFmt in numFmts.numFmt)
            this.numberFormats.Add((int) ctNumFmt.numFmtId, ctNumFmt.formatCode);
        }
        CT_Fonts fonts = styleSheet.fonts;
        if (fonts != null)
        {
          int index = 0;
          foreach (CT_Font font in fonts.font)
          {
            this.fonts.Add(new XSSFFont(font, index));
            ++index;
          }
        }
        CT_Fills fills = styleSheet.fills;
        if (fills != null)
        {
          foreach (CT_Fill Fill in fills.fill)
            this.fills.Add(new XSSFCellFill(Fill));
        }
        CT_Borders borders = styleSheet.borders;
        if (borders != null)
        {
          foreach (CT_Border border in borders.border)
            this.borders.Add(new XSSFCellBorder(border));
        }
        CT_CellXfs cellXfs = styleSheet.cellXfs;
        if (cellXfs != null)
          this.xfs.AddRange((IEnumerable<CT_Xf>) cellXfs.xf);
        CT_CellStyleXfs cellStyleXfs = styleSheet.cellStyleXfs;
        if (cellStyleXfs != null)
          this.styleXfs.AddRange((IEnumerable<CT_Xf>) cellStyleXfs.xf);
        CT_Dxfs dxfs = styleSheet.dxfs;
        if (dxfs == null)
          return;
        this.dxfs.AddRange((IEnumerable<CT_Dxf>) dxfs.dxf);
      }
      catch (XmlException ex)
      {
        throw new IOException(ex.Message);
      }
    }

    public string GetNumberFormatAt(int idx)
    {
      if (this.numberFormats.ContainsKey(idx))
        return this.numberFormats[idx];
      return (string) null;
    }

    public int PutNumberFormat(string fmt)
    {
      if (this.numberFormats.ContainsValue(fmt))
      {
        foreach (int key in this.numberFormats.Keys)
        {
          if (this.numberFormats[key].Equals(fmt))
            return key;
        }
        throw new InvalidOperationException("Found the format, but couldn't figure out where - should never happen!");
      }
      int firstCustomStyleId = StylesTable.FIRST_CUSTOM_STYLE_ID;
      while (this.numberFormats.ContainsKey(firstCustomStyleId))
        ++firstCustomStyleId;
      this.numberFormats[firstCustomStyleId] = fmt;
      return firstCustomStyleId;
    }

    public XSSFFont GetFontAt(int idx)
    {
      return this.fonts[idx];
    }

    public int PutFont(XSSFFont font, bool forceRegistration)
    {
      int num = -1;
      if (!forceRegistration)
        num = this.fonts.IndexOf(font);
      if (num != -1)
        return num;
      int count = this.fonts.Count;
      this.fonts.Add(font);
      return count;
    }

    public int PutFont(XSSFFont font)
    {
      return this.PutFont(font, false);
    }

    public XSSFCellStyle GetStyleAt(int idx)
    {
      int cellStyleXfId = 0;
      if (this.xfs[idx].xfId > 0U)
        cellStyleXfId = (int) this.xfs[idx].xfId;
      return new XSSFCellStyle(idx, cellStyleXfId, this, this.theme);
    }

    public int PutStyle(XSSFCellStyle style)
    {
      CT_Xf coreXf = style.GetCoreXf();
      if (!this.xfs.Contains(coreXf))
        this.xfs.Add(coreXf);
      return this.xfs.IndexOf(coreXf);
    }

    public XSSFCellBorder GetBorderAt(int idx)
    {
      return this.borders[idx];
    }

    public int PutBorder(XSSFCellBorder border)
    {
      int num = this.borders.IndexOf(border);
      if (num != -1)
        return num;
      this.borders.Add(border);
      border.SetThemesTable(this.theme);
      return this.borders.Count - 1;
    }

    public XSSFCellFill GetFillAt(int idx)
    {
      return this.fills[idx];
    }

    public List<XSSFCellBorder> GetBorders()
    {
      return this.borders;
    }

    public ReadOnlyCollection<XSSFCellFill> GetFills()
    {
      return this.fills.AsReadOnly();
    }

    public List<XSSFFont> GetFonts()
    {
      return this.fonts;
    }

    public Dictionary<int, string> GetNumberFormats()
    {
      return this.numberFormats;
    }

    public int PutFill(XSSFCellFill fill)
    {
      int num = this.fills.IndexOf(fill);
      if (num != -1)
        return num;
      this.fills.Add(fill);
      return this.fills.Count - 1;
    }

    public CT_Xf GetCellXfAt(int idx)
    {
      return this.xfs[idx];
    }

    public int PutCellXf(CT_Xf cellXf)
    {
      this.xfs.Add(cellXf);
      return this.xfs.Count;
    }

    public void ReplaceCellXfAt(int idx, CT_Xf cellXf)
    {
      this.xfs[idx] = cellXf;
    }

    public CT_Xf GetCellStyleXfAt(int idx)
    {
      return this.styleXfs[idx];
    }

    public int PutCellStyleXf(CT_Xf cellStyleXf)
    {
      this.styleXfs.Add(cellStyleXf);
      return this.styleXfs.Count;
    }

    public void ReplaceCellStyleXfAt(int idx, CT_Xf cellStyleXf)
    {
      this.styleXfs[idx] = cellStyleXf;
    }

    public int GetNumCellStyles()
    {
      return this.xfs.Count;
    }

    public int _GetNumberFormatSize()
    {
      return this.numberFormats.Count;
    }

    public int _GetXfsSize()
    {
      return this.xfs.Count;
    }

    public int _GetStyleXfsSize()
    {
      return this.styleXfs.Count;
    }

    public CT_Stylesheet GetCTStylesheet()
    {
      return this.doc.GetStyleSheet();
    }

    public int _GetDXfsSize()
    {
      return this.dxfs.Count;
    }

    public void WriteTo(Stream out1)
    {
      CT_Stylesheet styleSheet = this.doc.GetStyleSheet();
      CT_NumFmts ctNumFmts = new CT_NumFmts();
      ctNumFmts.count = (uint) this.numberFormats.Count;
      if (ctNumFmts.count > 0U)
        ctNumFmts.countSpecified = true;
      foreach (KeyValuePair<int, string> numberFormat in this.numberFormats)
      {
        CT_NumFmt ctNumFmt = ctNumFmts.AddNewNumFmt();
        ctNumFmt.numFmtId = (uint) numberFormat.Key;
        ctNumFmt.formatCode = numberFormat.Value;
      }
      styleSheet.numFmts = ctNumFmts;
      CT_Fonts ctFonts = new CT_Fonts();
      ctFonts.count = (uint) this.fonts.Count;
      if (ctFonts.count > 0U)
        ctFonts.countSpecified = true;
      CT_Font[] array1 = new CT_Font[this.fonts.Count];
      int num = 0;
      foreach (XSSFFont font in this.fonts)
        array1[num++] = font.GetCTFont();
      ctFonts.SetFontArray(array1);
      styleSheet.fonts = ctFonts;
      List<CT_Fill> array2 = new List<CT_Fill>(this.fills.Count);
      foreach (XSSFCellFill fill in this.fills)
        array2.Add(fill.GetCTFill());
      CT_Fills ctFills = new CT_Fills();
      ctFills.SetFillArray(array2);
      ctFills.count = (uint) this.fills.Count;
      if (ctFills.count > 0U)
        ctFills.countSpecified = true;
      styleSheet.fills = ctFills;
      List<CT_Border> array3 = new List<CT_Border>(this.borders.Count);
      foreach (XSSFCellBorder border in this.borders)
        array3.Add(border.GetCTBorder());
      CT_Borders ctBorders = new CT_Borders();
      ctBorders.SetBorderArray(array3);
      ctBorders.count = (uint) array3.Count;
      if (ctBorders.count > 0U)
        ctBorders.countSpecified = true;
      styleSheet.borders = ctBorders;
      if (this.xfs.Count > 0)
      {
        CT_CellXfs ctCellXfs = new CT_CellXfs();
        ctCellXfs.count = (uint) this.xfs.Count;
        if (ctCellXfs.count > 0U)
          ctCellXfs.countSpecified = true;
        ctCellXfs.xf = this.xfs;
        styleSheet.cellXfs = ctCellXfs;
      }
      if (this.styleXfs.Count > 0)
      {
        CT_CellStyleXfs ctCellStyleXfs = new CT_CellStyleXfs();
        ctCellStyleXfs.count = (uint) this.styleXfs.Count;
        if (ctCellStyleXfs.count > 0U)
          ctCellStyleXfs.countSpecified = true;
        ctCellStyleXfs.xf = this.styleXfs;
        styleSheet.cellStyleXfs = ctCellStyleXfs;
      }
      if (this.dxfs.Count > 0)
      {
        CT_Dxfs ctDxfs = new CT_Dxfs();
        ctDxfs.count = (uint) this.dxfs.Count;
        if (ctDxfs.count > 0U)
          ctDxfs.countSpecified = true;
        ctDxfs.dxf = this.dxfs;
        styleSheet.dxfs = ctDxfs;
      }
      this.doc.Save(out1);
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.WriteTo(outputStream);
      outputStream.Close();
    }

    private void Initialize()
    {
      this.fonts.Add(StylesTable.CreateDefaultFont());
      CT_Fill[] defaultFills = StylesTable.CreateDefaultFills();
      this.fills.Add(new XSSFCellFill(defaultFills[0]));
      this.fills.Add(new XSSFCellFill(defaultFills[1]));
      this.borders.Add(new XSSFCellBorder(StylesTable.CreateDefaultBorder()));
      this.styleXfs.Add(StylesTable.CreateDefaultXf());
      CT_Xf defaultXf = StylesTable.CreateDefaultXf();
      defaultXf.xfId = 0U;
      this.xfs.Add(defaultXf);
    }

    private static CT_Xf CreateDefaultXf()
    {
      return new CT_Xf() { numFmtId = 0, fontId = 0, fillId = 0, borderId = 0 };
    }

    private static CT_Border CreateDefaultBorder()
    {
      CT_Border ctBorder = new CT_Border();
      ctBorder.AddNewLeft();
      ctBorder.AddNewRight();
      ctBorder.AddNewTop();
      ctBorder.AddNewBottom();
      ctBorder.AddNewDiagonal();
      return ctBorder;
    }

    private static CT_Fill[] CreateDefaultFills()
    {
      CT_Fill[] ctFillArray = new CT_Fill[2]{ new CT_Fill(), new CT_Fill() };
      ctFillArray[0].AddNewPatternFill().patternType = ST_PatternType.none;
      ctFillArray[1].AddNewPatternFill().patternType = ST_PatternType.darkGray;
      return ctFillArray;
    }

    private static XSSFFont CreateDefaultFont()
    {
      XSSFFont xssfFont = new XSSFFont(new CT_Font(), 0);
      xssfFont.FontHeightInPoints = XSSFFont.DEFAULT_FONT_SIZE;
      xssfFont.Color = XSSFFont.DEFAULT_FONT_COLOR;
      xssfFont.FontName = XSSFFont.DEFAULT_FONT_NAME;
      xssfFont.SetFamily(FontFamily.SWISS);
      xssfFont.SetScheme(FontScheme.MINOR);
      return xssfFont;
    }

    public CT_Dxf GetDxfAt(int idx)
    {
      return this.dxfs[idx];
    }

    public int PutDxf(CT_Dxf dxf)
    {
      this.dxfs.Add(dxf);
      return this.dxfs.Count;
    }

    public XSSFCellStyle CreateCellStyle()
    {
      CT_Xf cellXf = new CT_Xf();
      cellXf.numFmtId = 0U;
      cellXf.fontId = 0U;
      cellXf.fillId = 0U;
      cellXf.borderId = 0U;
      cellXf.xfId = 0U;
      int count = this.styleXfs.Count;
      return new XSSFCellStyle(this.PutCellXf(cellXf) - 1, count - 1, this, this.theme);
    }

    public XSSFFont FindFont(short boldWeight, short color, short fontHeight, string name, bool italic, bool strikeout, short typeOffset, byte underline)
    {
      foreach (XSSFFont font in this.fonts)
      {
        if ((int) font.Boldweight == (int) boldWeight && (int) font.Color == (int) color && ((int) font.FontHeight == (int) fontHeight && font.FontName.Equals(name)) && (font.IsItalic == italic && font.IsStrikeout == strikeout && ((int) font.TypeOffset == (int) typeOffset && (int) font.Underline == (int) underline)))
          return font;
      }
      return (XSSFFont) null;
    }
  }
}
