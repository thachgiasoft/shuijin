// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFChildAnchor
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFChildAnchor : XSSFAnchor
  {
    private CT_Transform2D t2d;

    public XSSFChildAnchor(int x, int y, int cx, int cy)
    {
      this.t2d = new CT_Transform2D();
      CT_Point2D ctPoint2D = this.t2d.AddNewOff();
      CT_PositiveSize2D ctPositiveSize2D = this.t2d.AddNewExt();
      ctPoint2D.x = (long) x;
      ctPoint2D.y = (long) y;
      ctPositiveSize2D.cx = (long) Math.Abs(cx - x);
      ctPositiveSize2D.cy = (long) Math.Abs(cy - y);
      if (x > cx)
        this.t2d.flipH = true;
      if (y <= cy)
        return;
      this.t2d.flipV = true;
    }

    public XSSFChildAnchor(CT_Transform2D t2d)
    {
      this.t2d = t2d;
    }

    public CT_Transform2D GetCTTransform2D()
    {
      return this.t2d;
    }

    public override int Dx1
    {
      get
      {
        return (int) this.t2d.off.x;
      }
      set
      {
        this.t2d.off.y = (long) value;
      }
    }

    public override int Dy1
    {
      get
      {
        return (int) this.t2d.off.y;
      }
      set
      {
        this.t2d.off.y = (long) value;
      }
    }

    public override int Dy2
    {
      get
      {
        return (int) ((long) this.Dy1 + this.t2d.ext.cy);
      }
      set
      {
        this.t2d.ext.cy = (long) (value - this.Dy1);
      }
    }

    public override int Dx2
    {
      get
      {
        return (int) ((long) this.Dx1 + this.t2d.ext.cx);
      }
      set
      {
        this.t2d.ext.cx = (long) (value - this.Dx1);
      }
    }
  }
}
