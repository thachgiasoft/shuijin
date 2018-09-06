// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFFont
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.Model;

namespace NPOI.XSSF.UserModel
{
  public class XSSFFont : IFont
  {
    public static string DEFAULT_FONT_NAME = "Calibri";
    public static short DEFAULT_FONT_SIZE = 11;
    public static short DEFAULT_FONT_COLOR = IndexedColors.BLACK.Index;
    private ThemesTable _themes;
    private CT_Font _ctFont;
    private short _index;

    public XSSFFont(CT_Font font)
    {
      this._ctFont = font;
      this._index = (short) 0;
    }

    public XSSFFont(CT_Font font, int index)
    {
      this._ctFont = font;
      this._index = (short) index;
    }

    public XSSFFont()
    {
      this._ctFont = new CT_Font();
      this.FontName = XSSFFont.DEFAULT_FONT_NAME;
      this.FontHeight = XSSFFont.DEFAULT_FONT_SIZE;
    }

    public CT_Font GetCTFont()
    {
      return this._ctFont;
    }

    public bool IsBold
    {
      get
      {
        CT_BooleanProperty ctBooleanProperty = this._ctFont.sizeOfBArray() == 0 ? (CT_BooleanProperty) null : this._ctFont.GetBArray(0);
        if (ctBooleanProperty != null)
          return ctBooleanProperty.val;
        return false;
      }
      set
      {
        if (value)
          (this._ctFont.sizeOfBArray() == 0 ? this._ctFont.AddNewB() : this._ctFont.GetBArray(0)).val = value;
        else
          this._ctFont.SetBArray((CT_BooleanProperty[]) null);
      }
    }

    public short Charset
    {
      get
      {
        CT_IntProperty ctIntProperty = this._ctFont.sizeOfCharsetArray() == 0 ? (CT_IntProperty) null : this._ctFont.GetCharsetArray(0);
        return ctIntProperty == null ? (short) FontCharset.ANSI.Value : (short) FontCharset.ValueOf(ctIntProperty.val).Value;
      }
      set
      {
      }
    }

    public short Color
    {
      get
      {
        CT_Color ctColor = this._ctFont.sizeOfColorArray() == 0 ? (CT_Color) null : this._ctFont.GetColorArray(0);
        if (ctColor == null || !ctColor.indexedSpecified)
          return IndexedColors.BLACK.Index;
        long indexed = (long) ctColor.indexed;
        if (indexed == (long) XSSFFont.DEFAULT_FONT_COLOR)
          return IndexedColors.BLACK.Index;
        if (indexed == (long) IndexedColors.RED.Index)
          return IndexedColors.RED.Index;
        return (short) indexed;
      }
      set
      {
        CT_Color ctColor = this._ctFont.sizeOfColorArray() == 0 ? this._ctFont.AddNewColor() : this._ctFont.GetColorArray(0);
        switch (value)
        {
          case 10:
            ctColor.indexed = (uint) IndexedColors.RED.Index;
            ctColor.indexedSpecified = true;
            break;
          case short.MaxValue:
            ctColor.indexed = (uint) XSSFFont.DEFAULT_FONT_COLOR;
            ctColor.indexedSpecified = true;
            break;
          default:
            ctColor.indexed = (uint) value;
            ctColor.indexedSpecified = true;
            break;
        }
      }
    }

    public XSSFColor GetXSSFColor()
    {
      CT_Color color1 = this._ctFont.sizeOfColorArray() == 0 ? (CT_Color) null : this._ctFont.GetColorArray(0);
      if (color1 == null)
        return (XSSFColor) null;
      XSSFColor color2 = new XSSFColor(color1);
      if (this._themes != null)
        this._themes.InheritFromThemeAsRequired(color2);
      return color2;
    }

    public short GetThemeColor()
    {
      CT_Color ctColor = this._ctFont.sizeOfColorArray() == 0 ? (CT_Color) null : this._ctFont.GetColorArray(0);
      return ctColor == null || !ctColor.themeSpecified ? (short) 0 : (short) ctColor.theme;
    }

    public short FontHeight
    {
      get
      {
        CT_FontSize ctFontSize = this._ctFont.sizeOfSzArray() == 0 ? (CT_FontSize) null : this._ctFont.GetSzArray(0);
        if (ctFontSize != null)
          return (short) (ctFontSize.val * 20.0);
        return (short) ((int) XSSFFont.DEFAULT_FONT_SIZE * 20);
      }
      set
      {
        this.SetFontHeight((double) value);
      }
    }

    public void SetFontHeight(double value)
    {
      (this._ctFont.sizeOfSzArray() == 0 ? this._ctFont.AddNewSz() : this._ctFont.GetSzArray(0)).val = value;
    }

    public short FontHeightInPoints
    {
      get
      {
        return (short) ((int) this.FontHeight / 20);
      }
      set
      {
        (this._ctFont.sizeOfSzArray() == 0 ? this._ctFont.AddNewSz() : this._ctFont.GetSzArray(0)).val = (double) value;
      }
    }

    public string FontName
    {
      get
      {
        CT_FontName ctFontName = this._ctFont.sizeOfNameArray() == 0 ? (CT_FontName) null : this._ctFont.GetNameArray(0);
        if (ctFontName != null)
          return ctFontName.val;
        return XSSFFont.DEFAULT_FONT_NAME;
      }
      set
      {
        (this._ctFont.sizeOfNameArray() == 0 ? this._ctFont.AddNewName() : this._ctFont.GetNameArray(0)).val = value == null ? XSSFFont.DEFAULT_FONT_NAME : value;
      }
    }

    public bool IsItalic
    {
      get
      {
        CT_BooleanProperty ctBooleanProperty = this._ctFont.sizeOfIArray() == 0 ? (CT_BooleanProperty) null : this._ctFont.GetIArray(0);
        if (ctBooleanProperty != null)
          return ctBooleanProperty.val;
        return false;
      }
      set
      {
        if (value)
          (this._ctFont.sizeOfIArray() == 0 ? this._ctFont.AddNewI() : this._ctFont.GetIArray(0)).val = value;
        else
          this._ctFont.SetIArray((CT_BooleanProperty[]) null);
      }
    }

    public bool IsStrikeout
    {
      get
      {
        CT_BooleanProperty ctBooleanProperty = this._ctFont.sizeOfStrikeArray() == 0 ? (CT_BooleanProperty) null : this._ctFont.GetStrikeArray(0);
        if (ctBooleanProperty != null)
          return ctBooleanProperty.val;
        return false;
      }
      set
      {
        if (!value)
          this._ctFont.SetStrikeArray((CT_BooleanProperty[]) null);
        else
          (this._ctFont.sizeOfStrikeArray() == 0 ? this._ctFont.AddNewStrike() : this._ctFont.GetStrikeArray(0)).val = value;
      }
    }

    public short TypeOffset
    {
      get
      {
        CT_VerticalAlignFontProperty alignFontProperty = this._ctFont.sizeOfVertAlignArray() == 0 ? (CT_VerticalAlignFontProperty) null : this._ctFont.GetVertAlignArray(0);
        if (alignFontProperty == null)
          return 0;
        ST_VerticalAlignRun val = alignFontProperty.val;
        switch (val)
        {
          case ST_VerticalAlignRun.baseline:
            return 0;
          case ST_VerticalAlignRun.superscript:
            return 1;
          case ST_VerticalAlignRun.subscript:
            return 2;
          default:
            throw new POIXMLException("Wrong offset value " + (object) val);
        }
      }
      set
      {
        if (value == (short) 0)
        {
          this._ctFont.SetVertAlignArray((CT_VerticalAlignFontProperty[]) null);
        }
        else
        {
          CT_VerticalAlignFontProperty alignFontProperty = this._ctFont.sizeOfVertAlignArray() == 0 ? this._ctFont.AddNewVertAlign() : this._ctFont.GetVertAlignArray(0);
          switch (value)
          {
            case 0:
              alignFontProperty.val = ST_VerticalAlignRun.baseline;
              break;
            case 1:
              alignFontProperty.val = ST_VerticalAlignRun.superscript;
              break;
            case 2:
              alignFontProperty.val = ST_VerticalAlignRun.subscript;
              break;
          }
        }
      }
    }

    public byte Underline
    {
      get
      {
        CT_UnderlineProperty underlineProperty = this._ctFont.sizeOfUArray() == 0 ? (CT_UnderlineProperty) null : this._ctFont.GetUArray(0);
        if (underlineProperty != null)
          return FontUnderline.ValueOf((int) underlineProperty.val).ByteValue;
        return FontUnderline.NONE.ByteValue;
      }
      set
      {
        this.SetUnderline(FontUnderline.ValueOf((int) value));
      }
    }

    public short Boldweight
    {
      get
      {
        return !this.IsBold ? (short) 400 : (short) 700;
      }
      set
      {
        this.IsBold = value == (short) 700;
      }
    }

    public void SetCharSet(byte charSet)
    {
      int charset = (int) charSet;
      if (charset < 0)
        charset += 256;
      this.SetCharSet(charset);
    }

    public void SetCharSet(int charset)
    {
      FontCharset charset1 = FontCharset.ValueOf(charset);
      if (charset1 == null)
        throw new POIXMLException("Attention: an attempt to set a type of unknow charset and charSet");
      this.SetCharSet(charset1);
    }

    public void SetCharSet(FontCharset charset)
    {
      (this._ctFont.sizeOfCharsetArray() != 0 ? this._ctFont.GetCharsetArray(0) : this._ctFont.AddNewCharset()).val = charset.Value;
    }

    public void SetColor(XSSFColor color)
    {
      if (color == null)
        this._ctFont.SetColorArray((CT_Color[]) null);
      else
        (this._ctFont.sizeOfColorArray() == 0 ? this._ctFont.AddNewColor() : this._ctFont.GetColorArray(0)).SetRgb(color.GetRgb());
    }

    public void SetThemeColor(short theme)
    {
      (this._ctFont.sizeOfColorArray() == 0 ? this._ctFont.AddNewColor() : this._ctFont.GetColorArray(0)).theme = (uint) theme;
    }

    public void SetUnderline(FontUnderline underline)
    {
      if (underline == FontUnderline.NONE && this._ctFont.sizeOfUArray() > 0)
        this._ctFont.SetUArray((CT_UnderlineProperty[]) null);
      else
        (this._ctFont.sizeOfUArray() == 0 ? this._ctFont.AddNewU() : this._ctFont.GetUArray(0)).val = (ST_UnderlineValues) underline.Value;
    }

    public override string ToString()
    {
      return this._ctFont.ToString();
    }

    public long RegisterTo(StylesTable styles)
    {
      this._themes = styles.GetTheme();
      short num = (short) styles.PutFont(this, true);
      this._index = num;
      return (long) num;
    }

    public void SetThemesTable(ThemesTable themes)
    {
      this._themes = themes;
    }

    public FontScheme GetScheme()
    {
      CT_FontScheme ctFontScheme = this._ctFont.sizeOfSchemeArray() == 0 ? (CT_FontScheme) null : this._ctFont.GetSchemeArray(0);
      if (ctFontScheme != null)
        return FontScheme.ValueOf((int) ctFontScheme.val);
      return FontScheme.NONE;
    }

    public void SetScheme(FontScheme scheme)
    {
      (this._ctFont.sizeOfSchemeArray() == 0 ? this._ctFont.AddNewScheme() : this._ctFont.GetSchemeArray(0)).val = (ST_FontScheme) scheme.Value;
    }

    public int Family
    {
      get
      {
        CT_IntProperty ctIntProperty = this._ctFont.sizeOfFamilyArray() == 0 ? this._ctFont.AddNewFamily() : this._ctFont.GetFamilyArray(0);
        if (ctIntProperty != null)
          return FontFamily.ValueOf(ctIntProperty.val).Value;
        return FontFamily.NOT_APPLICABLE.Value;
      }
      set
      {
        (this._ctFont.sizeOfFamilyArray() == 0 ? this._ctFont.AddNewFamily() : this._ctFont.GetFamilyArray(0)).val = value;
      }
    }

    public void SetFamily(FontFamily family)
    {
      this.Family = family.Value;
    }

    public short Index
    {
      get
      {
        return this._index;
      }
    }

    public override int GetHashCode()
    {
      return this._ctFont.ToString().GetHashCode();
    }

    public override bool Equals(object o)
    {
      if (!(o is XSSFFont))
        return false;
      return this._ctFont.ToString().Equals(((XSSFFont) o).GetCTFont().ToString());
    }
  }
}
