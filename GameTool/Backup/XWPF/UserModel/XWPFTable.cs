// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFTable
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using System.Collections.Generic;
using System.Text;

namespace NPOI.XWPF.UserModel
{
  public class XWPFTable : IBodyElement
  {
    protected StringBuilder text = new StringBuilder();
    private static Dictionary<XWPFTable.XWPFBorderType, ST_Border> xwpfBorderTypeMap = new Dictionary<XWPFTable.XWPFBorderType, ST_Border>();
    private CT_Tbl ctTbl;
    protected List<XWPFTableRow> tableRows;
    protected List<string> styleIDs;
    private static Dictionary<ST_Border, XWPFTable.XWPFBorderType> stBorderTypeMap;
    protected IBody part;

    static XWPFTable()
    {
      XWPFTable.xwpfBorderTypeMap.Add(XWPFTable.XWPFBorderType.NIL, ST_Border.nil);
      XWPFTable.xwpfBorderTypeMap.Add(XWPFTable.XWPFBorderType.NONE, ST_Border.none);
      XWPFTable.xwpfBorderTypeMap.Add(XWPFTable.XWPFBorderType.SINGLE, ST_Border.single);
      XWPFTable.xwpfBorderTypeMap.Add(XWPFTable.XWPFBorderType.THICK, ST_Border.thick);
      XWPFTable.xwpfBorderTypeMap.Add(XWPFTable.XWPFBorderType.DOUBLE, ST_Border.@double);
      XWPFTable.xwpfBorderTypeMap.Add(XWPFTable.XWPFBorderType.DOTTED, ST_Border.dotted);
      XWPFTable.xwpfBorderTypeMap.Add(XWPFTable.XWPFBorderType.DASHED, ST_Border.dashed);
      XWPFTable.xwpfBorderTypeMap.Add(XWPFTable.XWPFBorderType.DOT_DASH, ST_Border.dotDash);
      XWPFTable.stBorderTypeMap = new Dictionary<ST_Border, XWPFTable.XWPFBorderType>();
      XWPFTable.stBorderTypeMap.Add(ST_Border.nil, XWPFTable.XWPFBorderType.NIL);
      XWPFTable.stBorderTypeMap.Add(ST_Border.none, XWPFTable.XWPFBorderType.NONE);
      XWPFTable.stBorderTypeMap.Add(ST_Border.single, XWPFTable.XWPFBorderType.SINGLE);
      XWPFTable.stBorderTypeMap.Add(ST_Border.thick, XWPFTable.XWPFBorderType.THICK);
      XWPFTable.stBorderTypeMap.Add(ST_Border.@double, XWPFTable.XWPFBorderType.DOUBLE);
      XWPFTable.stBorderTypeMap.Add(ST_Border.dotted, XWPFTable.XWPFBorderType.DOTTED);
      XWPFTable.stBorderTypeMap.Add(ST_Border.dashed, XWPFTable.XWPFBorderType.DASHED);
      XWPFTable.stBorderTypeMap.Add(ST_Border.dotDash, XWPFTable.XWPFBorderType.DOT_DASH);
    }

    public XWPFTable(CT_Tbl table, IBody part, int row, int col)
      : this(table, part)
    {
      for (int pos1 = 0; pos1 < row; ++pos1)
      {
        XWPFTableRow xwpfTableRow = this.GetRow(pos1) == null ? this.CreateRow() : this.GetRow(pos1);
        for (int pos2 = 0; pos2 < col; ++pos2)
        {
          if (xwpfTableRow.GetCell(pos2) == null)
            xwpfTableRow.CreateCell();
        }
      }
    }

    public XWPFTable(CT_Tbl table, IBody part)
    {
      this.part = part;
      this.ctTbl = table;
      this.tableRows = new List<XWPFTableRow>();
      if (table.SizeOfTrArray() == 0)
        this.CreateEmptyTable(table);
      foreach (CT_Row tr in (IEnumerable<CT_Row>) table.GetTrList())
      {
        StringBuilder stringBuilder = new StringBuilder();
        tr.Table = table;
        this.tableRows.Add(new XWPFTableRow(tr, this));
        foreach (CT_Tc tc in (IEnumerable<CT_Tc>) tr.GetTcList())
        {
          tc.TableRow = tr;
          foreach (CT_P prgrph in (IEnumerable<CT_P>) tc.GetPList())
          {
            XWPFParagraph xwpfParagraph = new XWPFParagraph(prgrph, part);
            if (stringBuilder.Length > 0)
              stringBuilder.Append('\t');
            stringBuilder.Append(xwpfParagraph.GetText());
          }
        }
        if (stringBuilder.Length > 0)
        {
          this.text.Append((object) stringBuilder);
          this.text.Append('\n');
        }
      }
    }

    private void CreateEmptyTable(CT_Tbl table)
    {
      table.AddNewTr().AddNewTc().AddNewP();
      CT_TblPr ctTblPr = table.AddNewTblPr();
      if (!ctTblPr.IsSetTblW())
        ctTblPr.AddNewTblW().w = "0";
      ctTblPr.tblW.type = ST_TblWidth.auto;
      CT_TblBorders ctTblBorders = ctTblPr.AddNewTblBorders();
      ctTblBorders.AddNewBottom().val = ST_Border.single;
      ctTblBorders.AddNewInsideH().val = ST_Border.single;
      ctTblBorders.AddNewInsideV().val = ST_Border.single;
      ctTblBorders.AddNewLeft().val = ST_Border.single;
      ctTblBorders.AddNewRight().val = ST_Border.single;
      ctTblBorders.AddNewTop().val = ST_Border.single;
      this.GetRows();
    }

    public CT_Tbl GetCTTbl()
    {
      return this.ctTbl;
    }

    public string GetText()
    {
      return this.text.ToString();
    }

    public void AddNewRowBetween(int start, int end)
    {
    }

    public void AddNewCol()
    {
      if (this.ctTbl.SizeOfTrArray() == 0)
        this.CreateRow();
      for (int p = 0; p < this.ctTbl.SizeOfTrArray(); ++p)
        new XWPFTableRow(this.ctTbl.GetTrArray(p), this).CreateCell();
    }

    public XWPFTableRow CreateRow()
    {
      int sizeCol = this.ctTbl.SizeOfTrArray() > 0 ? this.ctTbl.GetTrArray(0).SizeOfTcArray() : 0;
      XWPFTableRow tabRow = new XWPFTableRow(this.ctTbl.AddNewTr(), this);
      this.AddColumn(tabRow, sizeCol);
      this.tableRows.Add(tabRow);
      return tabRow;
    }

    public XWPFTableRow GetRow(int pos)
    {
      if (pos >= 0 && pos < this.ctTbl.SizeOfTrArray())
        return this.GetRows()[pos];
      return (XWPFTableRow) null;
    }

    public void SetWidth(int width)
    {
      CT_TblPr trPr = this.GetTrPr();
      (trPr.IsSetTblW() ? trPr.tblW : trPr.AddNewTblW()).w = width.ToString();
    }

    public int GetWidth()
    {
      CT_TblPr trPr = this.GetTrPr();
      if (!trPr.IsSetTblW())
        return -1;
      return int.Parse(trPr.tblW.w);
    }

    public int GetNumberOfRows()
    {
      return this.ctTbl.SizeOfTrArray();
    }

    private CT_TblPr GetTrPr()
    {
      if (this.ctTbl.tblPr == null)
        return this.ctTbl.AddNewTblPr();
      return this.ctTbl.tblPr;
    }

    private void AddColumn(XWPFTableRow tabRow, int sizeCol)
    {
      if (sizeCol <= 0)
        return;
      for (int index = 0; index < sizeCol; ++index)
        tabRow.CreateCell();
    }

    public string GetStyleID()
    {
      string str = (string) null;
      CT_TblPr tblPr = this.ctTbl.tblPr;
      if (tblPr != null)
      {
        CT_String tblStyle = tblPr.tblStyle;
        if (tblStyle != null)
          str = tblStyle.val;
      }
      return str;
    }

    public void SetStyleID(string styleName)
    {
      CT_TblPr trPr = this.GetTrPr();
      (trPr.tblStyle ?? trPr.AddNewTblStyle()).val = styleName;
    }

    public XWPFTable.XWPFBorderType GetInsideHBorderType()
    {
      XWPFTable.XWPFBorderType xwpfBorderType = XWPFTable.XWPFBorderType.NONE;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblBorders())
      {
        CT_TblBorders tblBorders = trPr.tblBorders;
        if (tblBorders.IsSetInsideH())
        {
          CT_Border insideH = tblBorders.insideH;
          xwpfBorderType = XWPFTable.stBorderTypeMap[insideH.val];
        }
      }
      return xwpfBorderType;
    }

    public int GetInsideHBorderSize()
    {
      int num = -1;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblBorders())
      {
        CT_TblBorders tblBorders = trPr.tblBorders;
        if (tblBorders.IsSetInsideH())
          num = (int) tblBorders.insideH.sz;
      }
      return num;
    }

    public int GetInsideHBorderSpace()
    {
      int num = -1;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblBorders())
      {
        CT_TblBorders tblBorders = trPr.tblBorders;
        if (tblBorders.IsSetInsideH())
          num = (int) tblBorders.insideH.space;
      }
      return num;
    }

    public string GetInsideHBorderColor()
    {
      string str = (string) null;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblBorders())
      {
        CT_TblBorders tblBorders = trPr.tblBorders;
        if (tblBorders.IsSetInsideH())
          str = tblBorders.insideH.color;
      }
      return str;
    }

    public XWPFTable.XWPFBorderType GetInsideVBorderType()
    {
      XWPFTable.XWPFBorderType xwpfBorderType = XWPFTable.XWPFBorderType.NONE;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblBorders())
      {
        CT_TblBorders tblBorders = trPr.tblBorders;
        if (tblBorders.IsSetInsideV())
        {
          CT_Border insideV = tblBorders.insideV;
          xwpfBorderType = XWPFTable.stBorderTypeMap[insideV.val];
        }
      }
      return xwpfBorderType;
    }

    public int GetInsideVBorderSize()
    {
      int num = -1;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblBorders())
      {
        CT_TblBorders tblBorders = trPr.tblBorders;
        if (tblBorders.IsSetInsideV())
          num = (int) tblBorders.insideV.sz;
      }
      return num;
    }

    public int GetInsideVBorderSpace()
    {
      int num = -1;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblBorders())
      {
        CT_TblBorders tblBorders = trPr.tblBorders;
        if (tblBorders.IsSetInsideV())
          num = (int) tblBorders.insideV.space;
      }
      return num;
    }

    public string GetInsideVBorderColor()
    {
      string str = (string) null;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblBorders())
      {
        CT_TblBorders tblBorders = trPr.tblBorders;
        if (tblBorders.IsSetInsideV())
          str = tblBorders.insideV.color;
      }
      return str;
    }

    public int GetRowBandSize()
    {
      int result = 0;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblStyleRowBandSize())
        int.TryParse(trPr.tblStyleRowBandSize.val, out result);
      return result;
    }

    public void SetRowBandSize(int size)
    {
      CT_TblPr trPr = this.GetTrPr();
      (trPr.IsSetTblStyleRowBandSize() ? trPr.tblStyleRowBandSize : trPr.AddNewTblStyleRowBandSize()).val = size.ToString();
    }

    public int GetColBandSize()
    {
      int result = 0;
      CT_TblPr trPr = this.GetTrPr();
      if (trPr.IsSetTblStyleColBandSize())
        int.TryParse(trPr.tblStyleColBandSize.val, out result);
      return result;
    }

    public void SetColBandSize(int size)
    {
      CT_TblPr trPr = this.GetTrPr();
      (trPr.IsSetTblStyleColBandSize() ? trPr.tblStyleColBandSize : trPr.AddNewTblStyleColBandSize()).val = size.ToString();
    }

    public void SetInsideHBorder(XWPFTable.XWPFBorderType type, int size, int space, string rgbColor)
    {
      CT_TblPr trPr = this.GetTrPr();
      CT_TblBorders ctTblBorders = trPr.IsSetTblBorders() ? trPr.tblBorders : trPr.AddNewTblBorders();
      CT_Border ctBorder = ctTblBorders.IsSetInsideH() ? ctTblBorders.insideH : ctTblBorders.AddNewInsideH();
      ctBorder.val = XWPFTable.xwpfBorderTypeMap[type];
      ctBorder.sz = (ulong) size;
      ctBorder.space = (ulong) space;
      ctBorder.color = rgbColor;
    }

    public void SetInsideVBorder(XWPFTable.XWPFBorderType type, int size, int space, string rgbColor)
    {
      CT_TblPr trPr = this.GetTrPr();
      CT_TblBorders ctTblBorders = trPr.IsSetTblBorders() ? trPr.tblBorders : trPr.AddNewTblBorders();
      CT_Border ctBorder = ctTblBorders.IsSetInsideV() ? ctTblBorders.insideV : ctTblBorders.AddNewInsideV();
      ctBorder.val = XWPFTable.xwpfBorderTypeMap[type];
      ctBorder.sz = (ulong) size;
      ctBorder.space = (ulong) space;
      ctBorder.color = rgbColor;
    }

    public int GetCellMarginTop()
    {
      int result = 0;
      CT_TblCellMar tblCellMar = this.GetTrPr().tblCellMar;
      if (tblCellMar != null)
      {
        CT_TblWidth top = tblCellMar.top;
        if (top != null)
          int.TryParse(top.w, out result);
      }
      return result;
    }

    public int GetCellMarginLeft()
    {
      int result = 0;
      CT_TblCellMar tblCellMar = this.GetTrPr().tblCellMar;
      if (tblCellMar != null)
      {
        CT_TblWidth left = tblCellMar.left;
        if (left != null)
          int.TryParse(left.w, out result);
      }
      return result;
    }

    public int GetCellMarginBottom()
    {
      int result = 0;
      CT_TblCellMar tblCellMar = this.GetTrPr().tblCellMar;
      if (tblCellMar != null)
      {
        CT_TblWidth bottom = tblCellMar.bottom;
        if (bottom != null)
          int.TryParse(bottom.w, out result);
      }
      return result;
    }

    public int GetCellMarginRight()
    {
      int result = 0;
      CT_TblCellMar tblCellMar = this.GetTrPr().tblCellMar;
      if (tblCellMar != null)
      {
        CT_TblWidth right = tblCellMar.right;
        if (right != null)
          int.TryParse(right.w, out result);
      }
      return result;
    }

    public void SetCellMargins(int top, int left, int bottom, int right)
    {
      CT_TblPr trPr = this.GetTrPr();
      CT_TblCellMar ctTblCellMar = trPr.IsSetTblCellMar() ? trPr.tblCellMar : trPr.AddNewTblCellMar();
      CT_TblWidth ctTblWidth1 = ctTblCellMar.IsSetLeft() ? ctTblCellMar.left : ctTblCellMar.AddNewLeft();
      ctTblWidth1.type = ST_TblWidth.dxa;
      ctTblWidth1.w = left.ToString();
      CT_TblWidth ctTblWidth2 = ctTblCellMar.IsSetTop() ? ctTblCellMar.top : ctTblCellMar.AddNewTop();
      ctTblWidth2.type = ST_TblWidth.dxa;
      ctTblWidth2.w = top.ToString();
      CT_TblWidth ctTblWidth3 = ctTblCellMar.IsSetBottom() ? ctTblCellMar.bottom : ctTblCellMar.AddNewBottom();
      ctTblWidth3.type = ST_TblWidth.dxa;
      ctTblWidth3.w = bottom.ToString();
      CT_TblWidth ctTblWidth4 = ctTblCellMar.IsSetRight() ? ctTblCellMar.right : ctTblCellMar.AddNewRight();
      ctTblWidth4.type = ST_TblWidth.dxa;
      ctTblWidth4.w = right.ToString();
    }

    public void AddRow(XWPFTableRow row)
    {
      this.ctTbl.AddNewTr();
      this.ctTbl.SetTrArray(this.GetNumberOfRows() - 1, row.GetCtRow());
      this.tableRows.Add(row);
    }

    public bool AddRow(XWPFTableRow row, int pos)
    {
      if (pos < 0 || pos > this.tableRows.Count)
        return false;
      this.ctTbl.InsertNewTr(pos);
      this.ctTbl.SetTrArray(pos, row.GetCtRow());
      this.tableRows.Insert(pos, row);
      return true;
    }

    public XWPFTableRow InsertNewTableRow(int pos)
    {
      if (pos < 0 || pos > this.tableRows.Count)
        return (XWPFTableRow) null;
      XWPFTableRow xwpfTableRow = new XWPFTableRow(this.ctTbl.InsertNewTr(pos), this);
      this.tableRows.Insert(pos, xwpfTableRow);
      return xwpfTableRow;
    }

    public bool RemoveRow(int pos)
    {
      if (pos < 0 || pos >= this.tableRows.Count)
        return false;
      if (this.ctTbl.SizeOfTrArray() > 0)
        this.ctTbl.RemoveTr(pos);
      this.tableRows.RemoveAt(pos);
      return true;
    }

    public List<XWPFTableRow> GetRows()
    {
      return this.tableRows;
    }

    public BodyElementType ElementType
    {
      get
      {
        return BodyElementType.TABLE;
      }
    }

    public IBody Body
    {
      get
      {
        return this.part;
      }
    }

    public POIXMLDocumentPart GetPart()
    {
      if (this.part != null)
        return this.part.GetPart();
      return (POIXMLDocumentPart) null;
    }

    public BodyType GetPartType()
    {
      return this.part.GetPartType();
    }

    public XWPFTableRow GetRow(CT_Row row)
    {
      for (int pos = 0; pos < this.GetRows().Count; ++pos)
      {
        if (this.GetRows()[pos].GetCtRow() == row)
          return this.GetRow(pos);
      }
      return (XWPFTableRow) null;
    }

    public enum XWPFBorderType
    {
      NIL,
      NONE,
      SINGLE,
      THICK,
      DOUBLE,
      DOTTED,
      DASHED,
      DOT_DASH,
    }
  }
}
