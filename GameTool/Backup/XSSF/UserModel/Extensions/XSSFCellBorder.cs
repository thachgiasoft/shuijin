// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Extensions.XSSFCellBorder
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.Model;
using System;

namespace NPOI.XSSF.UserModel.Extensions
{
  public class XSSFCellBorder
  {
    private ThemesTable _theme;
    private CT_Border border;

    public XSSFCellBorder(CT_Border border, ThemesTable theme)
      : this(border)
    {
      this._theme = theme;
    }

    public XSSFCellBorder(CT_Border border)
    {
      this.border = border;
    }

    public XSSFCellBorder()
    {
      this.border = new CT_Border();
    }

    public void SetThemesTable(ThemesTable themes)
    {
      this._theme = themes;
    }

    public CT_Border GetCTBorder()
    {
      return this.border;
    }

    public BorderStyle GetBorderStyle(BorderSide side)
    {
      CT_BorderPr border = this.GetBorder(side);
      return (BorderStyle) new ST_BorderStyle?(border == null ? ST_BorderStyle.none : border.style).Value;
    }

    public void SetBorderStyle(BorderSide side, BorderStyle style)
    {
      this.GetBorder(side, true).style = (ST_BorderStyle) Enum.GetValues(typeof (ST_BorderStyle)).GetValue((int) (style + (short) 1));
    }

    public XSSFColor GetBorderColor(BorderSide side)
    {
      CT_BorderPr border = this.GetBorder(side);
      if (border == null || !border.IsSetColor())
        return (XSSFColor) null;
      XSSFColor color = new XSSFColor(border.color);
      if (this._theme != null)
        this._theme.InheritFromThemeAsRequired(color);
      return color;
    }

    public void SetBorderColor(BorderSide side, XSSFColor color)
    {
      CT_BorderPr border = this.GetBorder(side, true);
      if (color == null)
        border.UnsetColor();
      else
        border.color = color.GetCTColor();
    }

    private CT_BorderPr GetBorder(BorderSide side)
    {
      return this.GetBorder(side, false);
    }

    private CT_BorderPr GetBorder(BorderSide side, bool ensure)
    {
      CT_BorderPr ctBorderPr;
      switch (side)
      {
        case BorderSide.TOP:
          ctBorderPr = this.border.top;
          if (ensure && ctBorderPr == null)
          {
            ctBorderPr = this.border.AddNewTop();
            break;
          }
          break;
        case BorderSide.RIGHT:
          ctBorderPr = this.border.right;
          if (ensure && ctBorderPr == null)
          {
            ctBorderPr = this.border.AddNewRight();
            break;
          }
          break;
        case BorderSide.BOTTOM:
          ctBorderPr = this.border.bottom;
          if (ensure && ctBorderPr == null)
          {
            ctBorderPr = this.border.AddNewBottom();
            break;
          }
          break;
        case BorderSide.LEFT:
          ctBorderPr = this.border.left;
          if (ensure && ctBorderPr == null)
          {
            ctBorderPr = this.border.AddNewLeft();
            break;
          }
          break;
        case BorderSide.DIAGONAL:
          ctBorderPr = this.border.diagonal;
          if (ensure && ctBorderPr == null)
          {
            ctBorderPr = this.border.AddNewDiagonal();
            break;
          }
          break;
        default:
          throw new ArgumentException("No suitable side specified for the border");
      }
      return ctBorderPr;
    }

    public override int GetHashCode()
    {
      return this.border.ToString().GetHashCode();
    }

    public override bool Equals(object o)
    {
      if (!(o is XSSFCellBorder))
        return false;
      return this.border.ToString().Equals(((XSSFCellBorder) o).GetCTBorder().ToString());
    }
  }
}
