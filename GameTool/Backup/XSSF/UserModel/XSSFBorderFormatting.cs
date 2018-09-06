// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFBorderFormatting
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel
{
  public class XSSFBorderFormatting : IBorderFormatting
  {
    private CT_Border _border;

    internal XSSFBorderFormatting(CT_Border border)
    {
      this._border = border;
    }

    public short BorderBottom
    {
      get
      {
        if (this._border.IsSetBottom())
          return 0;
        return (short) this._border.bottom.style;
      }
      set
      {
        CT_BorderPr ctBorderPr = this._border.IsSetBottom() ? this._border.bottom : this._border.AddNewBottom();
        if (value == (short) 0)
          this._border.unsetBottom();
        else
          ctBorderPr.style = (ST_BorderStyle) ((int) value + 1);
      }
    }

    public short BorderDiagonal
    {
      get
      {
        if (this._border.IsSetDiagonal())
          return 0;
        return (short) this._border.diagonal.style;
      }
      set
      {
        CT_BorderPr ctBorderPr = this._border.IsSetDiagonal() ? this._border.diagonal : this._border.AddNewDiagonal();
        if (value == (short) 0)
          this._border.unsetDiagonal();
        else
          ctBorderPr.style = (ST_BorderStyle) ((int) value + 1);
      }
    }

    public short BorderLeft
    {
      get
      {
        if (!this._border.IsSetLeft())
          return 0;
        return (short) this._border.left.style;
      }
      set
      {
        CT_BorderPr ctBorderPr = this._border.IsSetLeft() ? this._border.left : this._border.AddNewLeft();
        if (value == (short) 0)
          this._border.unsetLeft();
        else
          ctBorderPr.style = (ST_BorderStyle) ((int) value + 1);
      }
    }

    public short BorderRight
    {
      get
      {
        if (!this._border.IsSetRight())
          return 0;
        return (short) this._border.right.style;
      }
      set
      {
        CT_BorderPr ctBorderPr = this._border.IsSetRight() ? this._border.right : this._border.AddNewRight();
        if (value == (short) 0)
          this._border.unsetRight();
        else
          ctBorderPr.style = (ST_BorderStyle) ((int) value + 1);
      }
    }

    public short BorderTop
    {
      get
      {
        if (!this._border.IsSetTop())
          return 0;
        return (short) this._border.top.style;
      }
      set
      {
        CT_BorderPr ctBorderPr = this._border.IsSetTop() ? this._border.top : this._border.AddNewTop();
        if (value == (short) 0)
          this._border.unsetTop();
        else
          ctBorderPr.style = (ST_BorderStyle) ((int) value + 1);
      }
    }

    public short BottomBorderColor
    {
      get
      {
        if (!this._border.IsSetBottom())
          return 0;
        CT_BorderPr bottom = this._border.bottom;
        if (!bottom.color.indexedSpecified)
          return 0;
        return (short) bottom.color.indexed;
      }
      set
      {
        (this._border.IsSetBottom() ? this._border.bottom : this._border.AddNewBottom()).color = new CT_Color()
        {
          indexed = (uint) value,
          indexedSpecified = true
        };
      }
    }

    public short DiagonalBorderColor
    {
      get
      {
        if (!this._border.IsSetDiagonal())
          return 0;
        CT_BorderPr diagonal = this._border.diagonal;
        if (!diagonal.color.indexedSpecified)
          return 0;
        return (short) diagonal.color.indexed;
      }
      set
      {
        (this._border.IsSetDiagonal() ? this._border.diagonal : this._border.AddNewDiagonal()).color = new CT_Color()
        {
          indexed = (uint) value,
          indexedSpecified = true
        };
      }
    }

    public short LeftBorderColor
    {
      get
      {
        if (!this._border.IsSetLeft())
          return 0;
        CT_BorderPr left = this._border.left;
        if (!left.color.indexedSpecified)
          return 0;
        return (short) left.color.indexed;
      }
      set
      {
        (this._border.IsSetLeft() ? this._border.left : this._border.AddNewLeft()).color = new CT_Color()
        {
          indexed = (uint) value,
          indexedSpecified = true
        };
      }
    }

    public short RightBorderColor
    {
      get
      {
        if (!this._border.IsSetRight())
          return 0;
        CT_BorderPr right = this._border.right;
        if (!right.color.indexedSpecified)
          return 0;
        return (short) right.color.indexed;
      }
      set
      {
        (this._border.IsSetRight() ? this._border.right : this._border.AddNewRight()).color = new CT_Color()
        {
          indexed = (uint) value,
          indexedSpecified = true
        };
      }
    }

    public short TopBorderColor
    {
      get
      {
        if (!this._border.IsSetTop())
          return 0;
        CT_BorderPr top = this._border.top;
        if (!top.color.indexedSpecified)
          return 0;
        return (short) top.color.indexed;
      }
      set
      {
        (this._border.IsSetTop() ? this._border.top : this._border.AddNewTop()).color = new CT_Color()
        {
          indexed = (uint) value,
          indexedSpecified = true
        };
      }
    }
  }
}
