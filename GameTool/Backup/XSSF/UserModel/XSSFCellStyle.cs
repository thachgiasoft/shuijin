// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFCellStyle
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.Model;
using NPOI.XSSF.UserModel.Extensions;
using System;
using System.Xml;

namespace NPOI.XSSF.UserModel
{
  public class XSSFCellStyle : ICellStyle
  {
    private int _cellXfId;
    private StylesTable _stylesSource;
    private CT_Xf _cellXf;
    private CT_Xf _cellStyleXf;
    private XSSFFont _font;
    private XSSFCellAlignment _cellAlignment;
    private ThemesTable _theme;

    public XSSFCellStyle(int cellXfId, int cellStyleXfId, StylesTable stylesSource, ThemesTable theme)
    {
      this._cellXfId = cellXfId;
      this._stylesSource = stylesSource;
      this._cellXf = stylesSource.GetCellXfAt(this._cellXfId);
      this._cellStyleXf = cellStyleXfId == -1 ? (CT_Xf) null : stylesSource.GetCellStyleXfAt(cellStyleXfId);
      this._theme = theme;
    }

    public CT_Xf GetCoreXf()
    {
      return this._cellXf;
    }

    public CT_Xf GetStyleXf()
    {
      return this._cellStyleXf;
    }

    public XSSFCellStyle(StylesTable stylesSource)
    {
      this._stylesSource = stylesSource;
      this._cellXf = new CT_Xf();
      this._cellStyleXf = (CT_Xf) null;
    }

    public void VerifyBelongsToStylesSource(StylesTable src)
    {
      if (this._stylesSource != src)
        throw new ArgumentException("This Style does not belong to the supplied Workbook Stlyes Source. Are you trying to assign a style from one workbook to the cell of a differnt workbook?");
    }

    public void CloneStyleFrom(ICellStyle source)
    {
      if (!(source is XSSFCellStyle))
        throw new ArgumentException("Can only clone from one XSSFCellStyle to another, not between HSSFCellStyle and XSSFCellStyle");
      XSSFCellStyle xssfCellStyle = (XSSFCellStyle) source;
      if (xssfCellStyle._stylesSource == this._stylesSource)
      {
        this._cellXf = xssfCellStyle.GetCoreXf();
        this._cellStyleXf = xssfCellStyle.GetStyleXf();
      }
      else
      {
        try
        {
          if (this._cellXf.IsSetAlignment())
            this._cellXf.UnsetAlignment();
          if (this._cellXf.IsSetExtLst())
            this._cellXf.UnsetExtLst();
          this._cellXf = CT_Xf.Parse(xssfCellStyle.GetCoreXf().ToString());
          this._stylesSource.ReplaceCellXfAt(this._cellXfId, this._cellXf);
        }
        catch (XmlException ex)
        {
          throw new POIXMLException((Exception) ex);
        }
        this.DataFormat = new XSSFDataFormat(this._stylesSource).GetFormat(xssfCellStyle.GetDataFormatString());
        try
        {
          XSSFFont xssfFont = new XSSFFont(CT_Font.Parse(xssfCellStyle.GetFont().GetCTFont().ToString()));
          xssfFont.RegisterTo(this._stylesSource);
          this.SetFont((IFont) xssfFont);
        }
        catch (XmlException ex)
        {
          throw new POIXMLException((Exception) ex);
        }
      }
      this._font = (XSSFFont) null;
      this._cellAlignment = (XSSFCellAlignment) null;
    }

    public HorizontalAlignment Alignment
    {
      get
      {
        return this.GetAlignmentEnum();
      }
      set
      {
        this.GetCellAlignment().SetHorizontal(value);
      }
    }

    public HorizontalAlignment GetAlignmentEnum()
    {
      CT_CellAlignment alignment = this._cellXf.alignment;
      if (alignment != null && alignment.IsSetHorizontal())
        return (HorizontalAlignment) alignment.horizontal;
      return HorizontalAlignment.GENERAL;
    }

    public BorderStyle BorderBottom
    {
      get
      {
        if (!this._cellXf.applyBorder)
          return BorderStyle.NONE;
        CT_Border ctBorder = this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetCTBorder();
        if (!ctBorder.IsSetBottom())
          return BorderStyle.NONE;
        return (BorderStyle) ctBorder.bottom.style;
      }
      set
      {
        CT_Border ctBorder = this.GetCTBorder();
        CT_BorderPr ctBorderPr = ctBorder.IsSetBottom() ? ctBorder.bottom : ctBorder.AddNewBottom();
        if (value == BorderStyle.NONE)
          ctBorder.unsetBottom();
        else
          ctBorderPr.style = (ST_BorderStyle) value;
        this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
        this._cellXf.applyBorder = true;
      }
    }

    public BorderStyle BorderLeft
    {
      get
      {
        if (!this._cellXf.applyBorder)
          return BorderStyle.NONE;
        CT_Border ctBorder = this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetCTBorder();
        if (!ctBorder.IsSetLeft())
          return BorderStyle.NONE;
        return (BorderStyle) ctBorder.left.style;
      }
      set
      {
        CT_Border ctBorder = this.GetCTBorder();
        CT_BorderPr ctBorderPr = ctBorder.IsSetLeft() ? ctBorder.left : ctBorder.AddNewLeft();
        if (value == BorderStyle.NONE)
          ctBorder.unsetLeft();
        else
          ctBorderPr.style = (ST_BorderStyle) value;
        this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
        this._cellXf.applyBorder = true;
      }
    }

    public BorderStyle BorderRight
    {
      get
      {
        if (!this._cellXf.applyBorder)
          return BorderStyle.NONE;
        CT_Border ctBorder = this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetCTBorder();
        if (!ctBorder.IsSetRight())
          return BorderStyle.NONE;
        return (BorderStyle) ctBorder.right.style;
      }
      set
      {
        CT_Border ctBorder = this.GetCTBorder();
        CT_BorderPr ctBorderPr = ctBorder.IsSetRight() ? ctBorder.right : ctBorder.AddNewRight();
        if (value == BorderStyle.NONE)
          ctBorder.unsetRight();
        else
          ctBorderPr.style = (ST_BorderStyle) value;
        this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
        this._cellXf.applyBorder = true;
      }
    }

    public BorderStyle BorderTop
    {
      get
      {
        if (!this._cellXf.applyBorder)
          return BorderStyle.NONE;
        CT_Border ctBorder = this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetCTBorder();
        if (!ctBorder.IsSetTop())
          return BorderStyle.NONE;
        return (BorderStyle) ctBorder.top.style;
      }
      set
      {
        CT_Border ctBorder = this.GetCTBorder();
        CT_BorderPr ctBorderPr = ctBorder.IsSetTop() ? ctBorder.top : ctBorder.AddNewTop();
        if (value == BorderStyle.NONE)
          ctBorder.unsetTop();
        else
          ctBorderPr.style = (ST_BorderStyle) value;
        this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
        this._cellXf.applyBorder = true;
      }
    }

    public short BottomBorderColor
    {
      get
      {
        XSSFColor bottomBorderXssfColor = this.GetBottomBorderXSSFColor();
        if (bottomBorderXssfColor != null)
          return bottomBorderXssfColor.Indexed;
        return IndexedColors.BLACK.Index;
      }
      set
      {
        this.SetBottomBorderColor(new XSSFColor()
        {
          Indexed = value
        });
      }
    }

    public XSSFColor GetBottomBorderXSSFColor()
    {
      if (!this._cellXf.applyBorder)
        return (XSSFColor) null;
      return this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetBorderColor(BorderSide.BOTTOM);
    }

    public short DataFormat
    {
      get
      {
        return (short) this._cellXf.numFmtId;
      }
      set
      {
        this._cellXf.applyNumberFormat = true;
        this._cellXf.numFmtId = (uint) value;
      }
    }

    public string GetDataFormatString()
    {
      return new XSSFDataFormat(this._stylesSource).GetFormat(this.DataFormat);
    }

    public short FillBackgroundColor
    {
      get
      {
        XSSFColor backgroundColorColor = (XSSFColor) this.FillBackgroundColorColor;
        if (backgroundColorColor != null)
          return backgroundColorColor.Indexed;
        return IndexedColors.AUTOMATIC.Index;
      }
      set
      {
        this.SetFillBackgroundColor(new XSSFColor()
        {
          Indexed = value
        });
      }
    }

    public IColor FillBackgroundColorColor
    {
      get
      {
        return (IColor) this.FillBackgroundXSSFColor;
      }
      set
      {
        this.FillBackgroundXSSFColor = (XSSFColor) value;
      }
    }

    public XSSFColor FillBackgroundXSSFColor
    {
      get
      {
        if (!this._cellXf.applyFill)
          return (XSSFColor) null;
        XSSFColor fillBackgroundColor = this._stylesSource.GetFillAt((int) this._cellXf.fillId).GetFillBackgroundColor();
        if (fillBackgroundColor != null && this._theme != null)
          this._theme.InheritFromThemeAsRequired(fillBackgroundColor);
        return fillBackgroundColor;
      }
      set
      {
        CT_Fill ctFill = this.GetCTFill();
        CT_PatternFill ctPatternFill = ctFill.patternFill;
        if (value == null)
        {
          ctPatternFill?.unsetBgColor();
        }
        else
        {
          if (ctPatternFill == null)
            ctPatternFill = ctFill.AddNewPatternFill();
          ctPatternFill.bgColor = value.GetCTColor();
        }
        this._cellXf.fillId = (uint) this._stylesSource.PutFill(new XSSFCellFill(ctFill));
        this._cellXf.applyFill = true;
      }
    }

    public short FillForegroundColor
    {
      get
      {
        XSSFColor foregroundColorColor = (XSSFColor) this.FillForegroundColorColor;
        if (foregroundColorColor != null)
          return foregroundColorColor.Indexed;
        return IndexedColors.AUTOMATIC.Index;
      }
      set
      {
        this.SetFillForegroundColor(new XSSFColor()
        {
          Indexed = value
        });
      }
    }

    public IColor FillForegroundColorColor
    {
      get
      {
        return (IColor) this.FillForegroundXSSFColor;
      }
      set
      {
        this.FillForegroundXSSFColor = (XSSFColor) value;
      }
    }

    public XSSFColor FillForegroundXSSFColor
    {
      get
      {
        if (!this._cellXf.applyFill)
          return (XSSFColor) null;
        XSSFColor fillForegroundColor = this._stylesSource.GetFillAt((int) this._cellXf.fillId).GetFillForegroundColor();
        if (fillForegroundColor != null && this._theme != null)
          this._theme.InheritFromThemeAsRequired(fillForegroundColor);
        return fillForegroundColor;
      }
      set
      {
        CT_Fill ctFill = this.GetCTFill();
        CT_PatternFill ctPatternFill = ctFill.patternFill;
        if (value == null)
        {
          ctPatternFill?.unsetFgColor();
        }
        else
        {
          if (ctPatternFill == null)
            ctPatternFill = ctFill.AddNewPatternFill();
          ctPatternFill.fgColor = value.GetCTColor();
        }
        this._cellXf.fillId = (uint) this._stylesSource.PutFill(new XSSFCellFill(ctFill));
        this._cellXf.applyFill = true;
      }
    }

    public FillPatternType FillPattern
    {
      get
      {
        if (!this._cellXf.applyFill)
          return FillPatternType.NO_FILL;
        return (FillPatternType) this._stylesSource.GetFillAt((int) this._cellXf.fillId).GetPatternType();
      }
      set
      {
        CT_Fill ctFill = this.GetCTFill();
        CT_PatternFill ctPatternFill = ctFill.IsSetPatternFill() ? ctFill.GetPatternFill() : ctFill.AddNewPatternFill();
        if (value == FillPatternType.NO_FILL && ctPatternFill.IsSetPatternType())
          ctPatternFill.unsetPatternType();
        else
          ctPatternFill.patternType = (ST_PatternType) value;
        this._cellXf.fillId = (uint) this._stylesSource.PutFill(new XSSFCellFill(ctFill));
        this._cellXf.applyFill = true;
      }
    }

    public XSSFFont GetFont()
    {
      if (this._font == null)
        this._font = this._stylesSource.GetFontAt(this.GetFontId());
      return this._font;
    }

    public short FontIndex
    {
      get
      {
        return (short) this.GetFontId();
      }
    }

    public bool IsHidden
    {
      get
      {
        if (!this._cellXf.IsSetProtection() || !this._cellXf.protection.IsSetHidden())
          return false;
        return this._cellXf.protection.hidden;
      }
      set
      {
        if (!this._cellXf.IsSetProtection())
          this._cellXf.AddNewProtection();
        this._cellXf.protection.hidden = value;
      }
    }

    public short Indention
    {
      get
      {
        CT_CellAlignment alignment = this._cellXf.alignment;
        return alignment == null ? (short) 0 : (short) alignment.indent;
      }
      set
      {
        this.GetCellAlignment().SetIndent((long) value);
      }
    }

    public short Index
    {
      get
      {
        return (short) this._cellXfId;
      }
    }

    public short LeftBorderColor
    {
      get
      {
        XSSFColor leftBorderXssfColor = this.GetLeftBorderXSSFColor();
        if (leftBorderXssfColor != null)
          return leftBorderXssfColor.Indexed;
        return IndexedColors.BLACK.Index;
      }
      set
      {
        this.SetLeftBorderColor(new XSSFColor()
        {
          Indexed = value
        });
      }
    }

    public XSSFColor GetDiagonalBorderXSSFColor()
    {
      if (!this._cellXf.applyBorder)
        return (XSSFColor) null;
      return this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetBorderColor(BorderSide.DIAGONAL);
    }

    public XSSFColor GetLeftBorderXSSFColor()
    {
      if (!this._cellXf.applyBorder)
        return (XSSFColor) null;
      return this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetBorderColor(BorderSide.LEFT);
    }

    public bool IsLocked
    {
      get
      {
        if (!this._cellXf.IsSetProtection() || !this._cellXf.protection.IsSetLocked())
          return true;
        return this._cellXf.protection.locked;
      }
      set
      {
        if (!this._cellXf.IsSetProtection())
          this._cellXf.AddNewProtection();
        this._cellXf.protection.locked = value;
      }
    }

    public short RightBorderColor
    {
      get
      {
        XSSFColor rightBorderXssfColor = this.GetRightBorderXSSFColor();
        if (rightBorderXssfColor != null)
          return rightBorderXssfColor.Indexed;
        return IndexedColors.BLACK.Index;
      }
      set
      {
        this.SetRightBorderColor(new XSSFColor()
        {
          Indexed = value
        });
      }
    }

    public XSSFColor GetRightBorderXSSFColor()
    {
      if (!this._cellXf.applyBorder)
        return (XSSFColor) null;
      return this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetBorderColor(BorderSide.RIGHT);
    }

    public short Rotation
    {
      get
      {
        CT_CellAlignment alignment = this._cellXf.alignment;
        return alignment == null ? (short) 0 : (short) alignment.textRotation;
      }
      set
      {
        this.GetCellAlignment().SetTextRotation((long) value);
      }
    }

    public short TopBorderColor
    {
      get
      {
        XSSFColor topBorderXssfColor = this.GetTopBorderXSSFColor();
        if (topBorderXssfColor != null)
          return topBorderXssfColor.Indexed;
        return IndexedColors.BLACK.Index;
      }
      set
      {
        this.SetTopBorderColor(new XSSFColor()
        {
          Indexed = value
        });
      }
    }

    public XSSFColor GetTopBorderXSSFColor()
    {
      if (!this._cellXf.applyBorder)
        return (XSSFColor) null;
      return this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetBorderColor(BorderSide.TOP);
    }

    public VerticalAlignment VerticalAlignment
    {
      get
      {
        return this.GetVerticalAlignmentEnum();
      }
      set
      {
        this.GetCellAlignment().SetVertical(value);
      }
    }

    public VerticalAlignment GetVerticalAlignmentEnum()
    {
      CT_CellAlignment alignment = this._cellXf.alignment;
      if (alignment != null && alignment.IsSetVertical())
        return (VerticalAlignment) alignment.vertical;
      return VerticalAlignment.BOTTOM;
    }

    public bool WrapText
    {
      get
      {
        CT_CellAlignment alignment = this._cellXf.alignment;
        if (alignment != null)
          return alignment.wrapText;
        return false;
      }
      set
      {
        this.GetCellAlignment().SetWrapText(value);
      }
    }

    public void SetBottomBorderColor(XSSFColor color)
    {
      CT_Border ctBorder = this.GetCTBorder();
      if (color == null && !ctBorder.IsSetBottom())
        return;
      CT_BorderPr ctBorderPr = ctBorder.IsSetBottom() ? ctBorder.bottom : ctBorder.AddNewBottom();
      if (color != null)
        ctBorderPr.SetColor(color.GetCTColor());
      else
        ctBorderPr.UnsetColor();
      this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
      this._cellXf.applyBorder = true;
    }

    public void SetFillBackgroundColor(XSSFColor color)
    {
      CT_Fill ctFill = this.GetCTFill();
      CT_PatternFill ctPatternFill = ctFill.GetPatternFill();
      if (color == null)
      {
        ctPatternFill?.unsetBgColor();
      }
      else
      {
        if (ctPatternFill == null)
          ctPatternFill = ctFill.AddNewPatternFill();
        ctPatternFill.bgColor = color.GetCTColor();
      }
      this._cellXf.fillId = (uint) this._stylesSource.PutFill(new XSSFCellFill(ctFill));
      this._cellXf.applyFill = true;
    }

    public void SetFillForegroundColor(XSSFColor color)
    {
      CT_Fill ctFill = this.GetCTFill();
      CT_PatternFill ctPatternFill = ctFill.GetPatternFill();
      if (color == null)
      {
        ctPatternFill?.unsetFgColor();
      }
      else
      {
        if (ctPatternFill == null)
          ctPatternFill = ctFill.AddNewPatternFill();
        ctPatternFill.fgColor = color.GetCTColor();
      }
      this._cellXf.fillId = (uint) this._stylesSource.PutFill(new XSSFCellFill(ctFill));
      this._cellXf.applyFill = true;
    }

    private CT_Fill GetCTFill()
    {
      return !this._cellXf.applyFill ? new CT_Fill() : this._stylesSource.GetFillAt((int) this._cellXf.fillId).GetCTFill().Copy();
    }

    private CT_Border GetCTBorder()
    {
      CT_Border ctBorder;
      if (this._cellXf.applyBorder)
      {
        ctBorder = this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetCTBorder();
      }
      else
      {
        ctBorder = new CT_Border();
        ctBorder.AddNewLeft();
        ctBorder.AddNewRight();
        ctBorder.AddNewTop();
        ctBorder.AddNewBottom();
        ctBorder.AddNewDiagonal();
      }
      return ctBorder;
    }

    public void SetFont(IFont font)
    {
      if (font != null)
      {
        this._cellXf.fontId = (uint) (ulong) font.Index;
        this._cellXf.fontIdSpecified = true;
        this._cellXf.applyFont = true;
      }
      else
        this._cellXf.applyFont = false;
    }

    public void SetDiagonalBorderColor(XSSFColor color)
    {
      CT_Border ctBorder = this.GetCTBorder();
      if (color == null && !ctBorder.IsSetDiagonal())
        return;
      CT_BorderPr ctBorderPr = ctBorder.IsSetDiagonal() ? ctBorder.diagonal : ctBorder.AddNewDiagonal();
      if (color != null)
        ctBorderPr.color = color.GetCTColor();
      else
        ctBorderPr.UnsetColor();
      this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
      this._cellXf.applyBorder = true;
    }

    public void SetLeftBorderColor(XSSFColor color)
    {
      CT_Border ctBorder = this.GetCTBorder();
      if (color == null && !ctBorder.IsSetLeft())
        return;
      CT_BorderPr ctBorderPr = ctBorder.IsSetLeft() ? ctBorder.left : ctBorder.AddNewLeft();
      if (color != null)
        ctBorderPr.color = color.GetCTColor();
      else
        ctBorderPr.UnsetColor();
      this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
      this._cellXf.applyBorder = true;
    }

    public void SetRightBorderColor(XSSFColor color)
    {
      CT_Border ctBorder = this.GetCTBorder();
      if (color == null && !ctBorder.IsSetRight())
        return;
      CT_BorderPr ctBorderPr = ctBorder.IsSetRight() ? ctBorder.right : ctBorder.AddNewRight();
      if (color != null)
        ctBorderPr.color = color.GetCTColor();
      else
        ctBorderPr.UnsetColor();
      this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
      this._cellXf.applyBorder = true;
    }

    public void SetTopBorderColor(XSSFColor color)
    {
      CT_Border ctBorder = this.GetCTBorder();
      if (color == null && !ctBorder.IsSetTop())
        return;
      CT_BorderPr ctBorderPr = ctBorder.IsSetTop() ? ctBorder.top : ctBorder.AddNewTop();
      if (color != null)
        ctBorderPr.color = color.GetCTColor();
      else
        ctBorderPr.UnsetColor();
      this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
      this._cellXf.applyBorder = true;
    }

    public void SetVerticalAlignment(short align)
    {
      this.GetCellAlignment().SetVertical((VerticalAlignment) align);
    }

    public XSSFColor GetBorderColor(BorderSide side)
    {
      switch (side)
      {
        case BorderSide.TOP:
          return this.GetTopBorderXSSFColor();
        case BorderSide.RIGHT:
          return this.GetRightBorderXSSFColor();
        case BorderSide.BOTTOM:
          return this.GetBottomBorderXSSFColor();
        case BorderSide.LEFT:
          return this.GetLeftBorderXSSFColor();
        default:
          throw new ArgumentException("Unknown border: " + (object) side);
      }
    }

    public void SetBorderColor(BorderSide side, XSSFColor color)
    {
      switch (side)
      {
        case BorderSide.TOP:
          this.SetTopBorderColor(color);
          break;
        case BorderSide.RIGHT:
          this.SetRightBorderColor(color);
          break;
        case BorderSide.BOTTOM:
          this.SetBottomBorderColor(color);
          break;
        case BorderSide.LEFT:
          this.SetLeftBorderColor(color);
          break;
      }
    }

    private int GetFontId()
    {
      if (this._cellXf.IsSetFontId())
        return (int) this._cellXf.fontId;
      return (int) this._cellStyleXf.fontId;
    }

    internal XSSFCellAlignment GetCellAlignment()
    {
      if (this._cellAlignment == null)
        this._cellAlignment = new XSSFCellAlignment(this.GetCTCellAlignment());
      return this._cellAlignment;
    }

    internal CT_CellAlignment GetCTCellAlignment()
    {
      if (this._cellXf.alignment == null)
        this._cellXf.alignment = new CT_CellAlignment();
      return this._cellXf.alignment;
    }

    public override int GetHashCode()
    {
      return this._cellXf.ToString().GetHashCode();
    }

    public override bool Equals(object o)
    {
      if (o == null || !(o is XSSFCellStyle))
        return false;
      return this._cellXf.ToString().Equals(((XSSFCellStyle) o).GetCoreXf().ToString());
    }

    public object Clone()
    {
      return (object) new XSSFCellStyle(this._stylesSource.PutCellXf(this._cellXf.Copy()) - 1, this._stylesSource._GetStyleXfsSize() - 1, this._stylesSource, this._theme);
    }

    public IFont GetFont(IWorkbook parentWorkbook)
    {
      throw new NotImplementedException();
    }

    public bool ShrinkToFit
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public short BorderDiagonalColor
    {
      get
      {
        XSSFColor diagonalBorderXssfColor = this.GetDiagonalBorderXSSFColor();
        if (diagonalBorderXssfColor != null)
          return diagonalBorderXssfColor.Indexed;
        return IndexedColors.BLACK.Index;
      }
      set
      {
        this.SetDiagonalBorderColor(new XSSFColor()
        {
          Indexed = value
        });
      }
    }

    public BorderStyle BorderDiagonalLineStyle
    {
      get
      {
        if (!this._cellXf.applyBorder)
          return BorderStyle.NONE;
        CT_Border ctBorder = this._stylesSource.GetBorderAt((int) this._cellXf.borderId).GetCTBorder();
        if (!ctBorder.IsSetDiagonal())
          return BorderStyle.NONE;
        return (BorderStyle) ctBorder.diagonal.style;
      }
      set
      {
        CT_Border ctBorder = this.GetCTBorder();
        CT_BorderPr ctBorderPr = ctBorder.IsSetDiagonal() ? ctBorder.diagonal : ctBorder.AddNewDiagonal();
        if (value == BorderStyle.NONE)
          ctBorder.unsetDiagonal();
        else
          ctBorderPr.style = (ST_BorderStyle) value;
        this._cellXf.borderId = (uint) this._stylesSource.PutBorder(new XSSFCellBorder(ctBorder, this._theme));
        this._cellXf.applyBorder = true;
      }
    }

    public BorderDiagonal BorderDiagonal
    {
      get
      {
        CT_Border ctBorder = this.GetCTBorder();
        if (ctBorder.diagonalDown && ctBorder.diagonalUp)
          return BorderDiagonal.BOTH;
        if (ctBorder.diagonalDown)
          return BorderDiagonal.FORWARD;
        return ctBorder.diagonalUp ? BorderDiagonal.BACKWARD : BorderDiagonal.NONE;
      }
      set
      {
        CT_Border ctBorder = this.GetCTBorder();
        switch (value)
        {
          case BorderDiagonal.BACKWARD:
            ctBorder.diagonalDown = false;
            ctBorder.diagonalDownSpecified = false;
            ctBorder.diagonalUp = true;
            ctBorder.diagonalUpSpecified = true;
            break;
          case BorderDiagonal.FORWARD:
            ctBorder.diagonalDown = true;
            ctBorder.diagonalDownSpecified = true;
            ctBorder.diagonalUp = false;
            ctBorder.diagonalUpSpecified = false;
            break;
          case BorderDiagonal.BOTH:
            ctBorder.diagonalDown = true;
            ctBorder.diagonalDownSpecified = true;
            ctBorder.diagonalUp = true;
            ctBorder.diagonalUpSpecified = true;
            break;
          default:
            ctBorder.unsetDiagonal();
            ctBorder.diagonalDown = false;
            ctBorder.diagonalDownSpecified = false;
            ctBorder.diagonalUp = false;
            ctBorder.diagonalUpSpecified = false;
            break;
        }
      }
    }
  }
}
