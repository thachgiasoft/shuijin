// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFTableRow
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using System.Collections.Generic;

namespace NPOI.XWPF.UserModel
{
  public class XWPFTableRow
  {
    private CT_Row ctRow;
    private XWPFTable table;
    private List<XWPFTableCell> tableCells;

    public XWPFTableRow(CT_Row row, XWPFTable table)
    {
      this.table = table;
      this.ctRow = row;
      this.GetTableCells();
    }

    public CT_Row GetCtRow()
    {
      return this.ctRow;
    }

    public XWPFTableCell CreateCell()
    {
      XWPFTableCell xwpfTableCell = new XWPFTableCell(this.ctRow.AddNewTc(), this, this.table.Body);
      this.tableCells.Add(xwpfTableCell);
      return xwpfTableCell;
    }

    public XWPFTableCell GetCell(int pos)
    {
      if (pos >= 0 && pos < this.ctRow.SizeOfTcArray())
        return this.GetTableCells()[pos];
      return (XWPFTableCell) null;
    }

    public void RemoveCell(int pos)
    {
      if (pos < 0 || pos >= this.ctRow.SizeOfTcArray())
        return;
      this.tableCells.RemoveAt(pos);
    }

    public XWPFTableCell AddNewTableCell()
    {
      XWPFTableCell xwpfTableCell = new XWPFTableCell(this.ctRow.AddNewTc(), this, this.table.Body);
      this.tableCells.Add(xwpfTableCell);
      return xwpfTableCell;
    }

    public void SetHeight(int height)
    {
      CT_TrPr trPr = this.GetTrPr();
      (trPr.SizeOfTrHeightArray() == 0 ? trPr.AddNewTrHeight() : trPr.GetTrHeightArray(0)).val = (ulong) height;
    }

    public int GetHeight()
    {
      CT_TrPr trPr = this.GetTrPr();
      if (trPr.SizeOfTrHeightArray() != 0)
        return (int) trPr.GetTrHeightArray(0).val;
      return 0;
    }

    private CT_TrPr GetTrPr()
    {
      if (!this.ctRow.IsSetTrPr())
        return this.ctRow.AddNewTrPr();
      return this.ctRow.trPr;
    }

    public XWPFTable GetTable()
    {
      return this.table;
    }

    public List<XWPFTableCell> GetTableCells()
    {
      if (this.tableCells == null)
      {
        List<XWPFTableCell> xwpfTableCellList = new List<XWPFTableCell>();
        foreach (CT_Tc tc in (IEnumerable<CT_Tc>) this.ctRow.GetTcList())
          xwpfTableCellList.Add(new XWPFTableCell(tc, this, this.table.Body));
        this.tableCells = xwpfTableCellList;
      }
      return this.tableCells;
    }

    public XWPFTableCell GetTableCell(CT_Tc cell)
    {
      for (int index = 0; index < this.tableCells.Count; ++index)
      {
        if (this.tableCells[index].GetCTTc() == cell)
          return this.tableCells[index];
      }
      return (XWPFTableCell) null;
    }

    public void SetCantSplitRow(bool split)
    {
      this.GetTrPr().AddNewCantSplit().val = split ? ST_OnOff.on : ST_OnOff.off;
    }

    public bool IsCantSplitRow()
    {
      bool flag = false;
      CT_TrPr trPr = this.GetTrPr();
      if (trPr.SizeOfCantSplitArray() > 0)
        flag = trPr.GetCantSplitList()[0].val == ST_OnOff.on;
      return flag;
    }

    public void SetRepeatHeader(bool repeat)
    {
      this.GetTrPr().AddNewTblHeader().val = repeat ? ST_OnOff.on : ST_OnOff.off;
    }

    public bool IsRepeatHeader()
    {
      bool flag = false;
      CT_TrPr trPr = this.GetTrPr();
      if (trPr.SizeOfTblHeaderArray() > 0)
        flag = trPr.GetTblHeaderList()[0].val == ST_OnOff.on;
      return flag;
    }
  }
}
