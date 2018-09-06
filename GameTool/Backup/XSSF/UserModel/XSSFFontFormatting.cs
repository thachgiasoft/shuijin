// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFFontFormatting
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel
{
  public class XSSFFontFormatting : IFontFormatting
  {
    private CT_Font _font;

    internal XSSFFontFormatting(CT_Font font)
    {
      this._font = font;
    }

    public FontSuperScript EscapementType
    {
      get
      {
        if (this._font.sizeOfVertAlignArray() == 0)
          return FontSuperScript.NONE;
        return (FontSuperScript) (this._font.GetVertAlignArray(0).val - 1);
      }
      set
      {
        this._font.SetVertAlignArray((CT_VerticalAlignFontProperty[]) null);
        if (value == FontSuperScript.NONE)
          return;
        this._font.AddNewVertAlign().val = (ST_VerticalAlignRun) (value + (short) 1);
      }
    }

    public short FontColorIndex
    {
      get
      {
        if (this._font.sizeOfColorArray() == 0)
          return -1;
        int num = 0;
        CT_Color colorArray = this._font.GetColorArray(0);
        if (colorArray.IsSetIndexed())
          num = (int) colorArray.indexed;
        return (short) num;
      }
      set
      {
        this._font.SetColorArray((CT_Color[]) null);
        if (value == (short) -1)
          return;
        this._font.AddNewColor().indexed = (uint) value;
        this._font.AddNewColor().indexedSpecified = true;
      }
    }

    public XSSFColor GetXSSFColor()
    {
      if (this._font.sizeOfColorArray() == 0)
        return (XSSFColor) null;
      return new XSSFColor(this._font.GetColorArray(0));
    }

    public int FontHeight
    {
      get
      {
        if (this._font.sizeOfSzArray() == 0)
          return -1;
        return (int) (short) (20.0 * this._font.GetSzArray(0).val);
      }
      set
      {
        this._font.SetSzArray((CT_FontSize[]) null);
        if (value == -1)
          return;
        this._font.AddNewSz().val = (double) value / 20.0;
      }
    }

    public FontUnderlineType UnderlineType
    {
      get
      {
        if (this._font.sizeOfUArray() == 0)
          return FontUnderlineType.NONE;
        switch (this._font.GetUArray(0).val)
        {
          case ST_UnderlineValues.single:
            return FontUnderlineType.SINGLE;
          case ST_UnderlineValues.@double:
            return FontUnderlineType.DOUBLE;
          case ST_UnderlineValues.singleAccounting:
            return FontUnderlineType.SINGLE_ACCOUNTING;
          case ST_UnderlineValues.doubleAccounting:
            return FontUnderlineType.DOUBLE_ACCOUNTING;
          default:
            return FontUnderlineType.NONE;
        }
      }
      set
      {
        this._font.SetUArray((CT_UnderlineProperty[]) null);
        if (value == FontUnderlineType.NONE)
          return;
        this._font.AddNewU().val = (ST_UnderlineValues) FontUnderline.ValueOf(value).Value;
      }
    }

    public bool IsBold
    {
      get
      {
        if (this._font.sizeOfBArray() == 1)
          return this._font.GetBArray(0).val;
        return false;
      }
    }

    public bool IsItalic
    {
      get
      {
        if (this._font.sizeOfIArray() == 1)
          return this._font.GetIArray(0).val;
        return false;
      }
    }

    public void SetFontStyle(bool italic, bool bold)
    {
      this._font.SetIArray((CT_BooleanProperty[]) null);
      this._font.SetBArray((CT_BooleanProperty[]) null);
      if (italic)
        this._font.AddNewI().val = true;
      if (!bold)
        return;
      this._font.AddNewB().val = true;
    }

    public void ResetFontStyle()
    {
      this._font = new CT_Font();
    }
  }
}
