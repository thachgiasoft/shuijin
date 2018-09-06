// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Extensions.XSSFCellFill
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;

namespace NPOI.XSSF.UserModel.Extensions
{
  public class XSSFCellFill
  {
    private CT_Fill _fill;

    public XSSFCellFill(CT_Fill Fill)
    {
      this._fill = Fill;
    }

    public XSSFCellFill()
    {
      this._fill = new CT_Fill();
    }

    public XSSFColor GetFillBackgroundColor()
    {
      CT_PatternFill patternFill = this._fill.GetPatternFill();
      if (patternFill == null)
        return (XSSFColor) null;
      CT_Color bgColor = patternFill.bgColor;
      if (bgColor != null)
        return new XSSFColor(bgColor);
      return (XSSFColor) null;
    }

    public void SetFillBackgroundColor(int index)
    {
      CT_PatternFill ctPatternFill = this.EnsureCTPatternFill();
      CT_Color ctColor = ctPatternFill.IsSetBgColor() ? ctPatternFill.bgColor : ctPatternFill.AddNewBgColor();
      ctColor.indexed = (uint) index;
      ctColor.indexedSpecified = true;
    }

    public void SetFillBackgroundColor(XSSFColor color)
    {
      this.EnsureCTPatternFill().bgColor = color.GetCTColor();
    }

    public XSSFColor GetFillForegroundColor()
    {
      CT_PatternFill patternFill = this._fill.GetPatternFill();
      if (patternFill == null)
        return (XSSFColor) null;
      CT_Color fgColor = patternFill.fgColor;
      if (fgColor != null)
        return new XSSFColor(fgColor);
      return (XSSFColor) null;
    }

    public void SetFillForegroundColor(int index)
    {
      CT_PatternFill ctPatternFill = this.EnsureCTPatternFill();
      (ctPatternFill.IsSetFgColor() ? ctPatternFill.fgColor : ctPatternFill.AddNewFgColor()).indexed = (uint) index;
    }

    public void SetFillForegroundColor(XSSFColor color)
    {
      this.EnsureCTPatternFill().fgColor = color.GetCTColor();
    }

    public ST_PatternType GetPatternType()
    {
      CT_PatternFill patternFill = this._fill.GetPatternFill();
      if (patternFill != null)
        return patternFill.patternType;
      return ST_PatternType.none;
    }

    public void SetPatternType(ST_PatternType patternType)
    {
      this.EnsureCTPatternFill().patternType = patternType;
    }

    private CT_PatternFill EnsureCTPatternFill()
    {
      return this._fill.GetPatternFill() ?? this._fill.AddNewPatternFill();
    }

    internal CT_Fill GetCTFill()
    {
      return this._fill;
    }

    public override int GetHashCode()
    {
      return this._fill.ToString().GetHashCode();
    }

    public override bool Equals(object o)
    {
      if (!(o is XSSFCellFill))
        return false;
      return this._fill.ToString().Equals(((XSSFCellFill) o).GetCTFill().ToString());
    }
  }
}
