// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFPatternFormatting
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel
{
  public class XSSFPatternFormatting : IPatternFormatting
  {
    private CT_Fill _fill;

    public XSSFPatternFormatting(CT_Fill fill)
    {
      this._fill = fill;
    }

    public short FillBackgroundColor
    {
      get
      {
        if (!this._fill.IsSetPatternFill() || !this._fill.GetPatternFill().bgColor.indexedSpecified)
          return 0;
        return (short) this._fill.GetPatternFill().bgColor.indexed;
      }
      set
      {
        (this._fill.IsSetPatternFill() ? this._fill.GetPatternFill() : this._fill.AddNewPatternFill()).bgColor = new CT_Color()
        {
          indexed = (uint) value,
          indexedSpecified = true
        };
      }
    }

    public short FillForegroundColor
    {
      get
      {
        if (!this._fill.IsSetPatternFill() || !this._fill.GetPatternFill().IsSetFgColor() || !this._fill.GetPatternFill().fgColor.indexedSpecified)
          return 0;
        return (short) this._fill.GetPatternFill().fgColor.indexed;
      }
      set
      {
        (this._fill.IsSetPatternFill() ? this._fill.GetPatternFill() : this._fill.AddNewPatternFill()).fgColor = new CT_Color()
        {
          indexed = (uint) value,
          indexedSpecified = true
        };
      }
    }

    public short FillPattern
    {
      get
      {
        if (!this._fill.IsSetPatternFill() || !this._fill.GetPatternFill().IsSetPatternType())
          return 0;
        return (short) (this._fill.GetPatternFill().patternType - 1);
      }
      set
      {
        CT_PatternFill ctPatternFill = this._fill.IsSetPatternFill() ? this._fill.GetPatternFill() : this._fill.AddNewPatternFill();
        if (value == (short) 0)
          ctPatternFill.patternType = ST_PatternType.none;
        else
          ctPatternFill.patternType = (ST_PatternType) ((int) value + 1);
      }
    }
  }
}
