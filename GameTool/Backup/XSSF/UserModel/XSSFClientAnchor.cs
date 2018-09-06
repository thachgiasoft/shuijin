// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFClientAnchor
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml.Spreadsheet;
using NPOI.SS.UserModel;

namespace NPOI.XSSF.UserModel
{
  public class XSSFClientAnchor : XSSFAnchor, IClientAnchor
  {
    private int anchorType;
    private CT_Marker cell1;
    private CT_Marker cell2;

    public XSSFClientAnchor()
    {
      this.cell1 = new CT_Marker();
      this.cell1.col = 0;
      this.cell1.colOff = 0L;
      this.cell1.row = 0;
      this.cell1.rowOff = 0L;
      this.cell2 = new CT_Marker();
      this.cell2.col = 0;
      this.cell2.colOff = 0L;
      this.cell2.row = 0;
      this.cell2.rowOff = 0L;
    }

    public XSSFClientAnchor(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2)
      : this()
    {
      this.cell1.col = col1;
      this.cell1.colOff = (long) dx1;
      this.cell1.row = row1;
      this.cell1.rowOff = (long) dy1;
      this.cell2.col = col2;
      this.cell2.colOff = (long) dx2;
      this.cell2.row = row2;
      this.cell2.rowOff = (long) dy2;
    }

    protected XSSFClientAnchor(CT_Marker cell1, CT_Marker cell2)
    {
      this.cell1 = cell1;
      this.cell2 = cell2;
    }

    public override bool Equals(object o)
    {
      if (o == null || !(o is XSSFClientAnchor))
        return false;
      XSSFClientAnchor xssfClientAnchor = (XSSFClientAnchor) o;
      if (this.Dx1 == xssfClientAnchor.Dx1 && this.Dx2 == xssfClientAnchor.Dx2 && (this.Dy1 == xssfClientAnchor.Dy1 && this.Dy2 == xssfClientAnchor.Dy2) && (this.Col1 == xssfClientAnchor.Col1 && this.Col2 == xssfClientAnchor.Col2 && this.Row1 == xssfClientAnchor.Row1))
        return this.Row2 == xssfClientAnchor.Row2;
      return false;
    }

    public override string ToString()
    {
      return "from : " + this.cell1.ToString() + "; to: " + this.cell2.ToString();
    }

    internal CT_Marker GetFrom()
    {
      return this.cell1;
    }

    internal void SetFrom(CT_Marker from)
    {
      this.cell1 = from;
    }

    internal CT_Marker GetTo()
    {
      return this.cell2;
    }

    internal void SetTo(CT_Marker to)
    {
      this.cell2 = to;
    }

    internal bool IsSet()
    {
      if (this.cell1.col == 0 && this.cell2.col == 0 && this.cell1.row == 0)
        return this.cell2.row != 0;
      return true;
    }

    public override int Dx1
    {
      get
      {
        return (int) this.cell1.colOff;
      }
      set
      {
        this.cell1.colOff = (long) value;
      }
    }

    public override int Dy1
    {
      get
      {
        return (int) this.cell1.rowOff;
      }
      set
      {
        this.cell1.rowOff = (long) value;
      }
    }

    public override int Dy2
    {
      get
      {
        return (int) this.cell2.rowOff;
      }
      set
      {
        this.cell2.rowOff = (long) value;
      }
    }

    public override int Dx2
    {
      get
      {
        return (int) this.cell2.colOff;
      }
      set
      {
        this.cell2.colOff = (long) value;
      }
    }

    public int AnchorType
    {
      get
      {
        return this.anchorType;
      }
      set
      {
        this.anchorType = value;
      }
    }

    public int Col1
    {
      get
      {
        return this.cell1.col;
      }
      set
      {
        this.cell1.col = value;
      }
    }

    public int Col2
    {
      get
      {
        return this.cell2.col;
      }
      set
      {
        this.cell2.col = value;
      }
    }

    public int Row1
    {
      get
      {
        return this.cell1.row;
      }
      set
      {
        this.cell1.row = value;
      }
    }

    public int Row2
    {
      get
      {
        return this.cell2.row;
      }
      set
      {
        this.cell2.row = value;
      }
    }
  }
}
