// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFTableCell
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NPOI.XWPF.UserModel
{
  public class XWPFTableCell : IBody
  {
    private static Dictionary<XWPFTableCell.XWPFVertAlign, ST_VerticalJc> alignMap = new Dictionary<XWPFTableCell.XWPFVertAlign, ST_VerticalJc>();
    private CT_Tc ctTc;
    protected List<XWPFParagraph> paragraphs;
    protected List<XWPFTable> tables;
    protected List<IBodyElement> bodyElements;
    protected IBody part;
    private XWPFTableRow tableRow;
    private static Dictionary<ST_VerticalJc, XWPFTableCell.XWPFVertAlign> stVertAlignTypeMap;

    static XWPFTableCell()
    {
      XWPFTableCell.alignMap.Add(XWPFTableCell.XWPFVertAlign.TOP, ST_VerticalJc.top);
      XWPFTableCell.alignMap.Add(XWPFTableCell.XWPFVertAlign.CENTER, ST_VerticalJc.center);
      XWPFTableCell.alignMap.Add(XWPFTableCell.XWPFVertAlign.BOTH, ST_VerticalJc.both);
      XWPFTableCell.alignMap.Add(XWPFTableCell.XWPFVertAlign.BOTTOM, ST_VerticalJc.bottom);
      XWPFTableCell.stVertAlignTypeMap = new Dictionary<ST_VerticalJc, XWPFTableCell.XWPFVertAlign>();
      XWPFTableCell.stVertAlignTypeMap.Add(ST_VerticalJc.top, XWPFTableCell.XWPFVertAlign.TOP);
      XWPFTableCell.stVertAlignTypeMap.Add(ST_VerticalJc.center, XWPFTableCell.XWPFVertAlign.CENTER);
      XWPFTableCell.stVertAlignTypeMap.Add(ST_VerticalJc.both, XWPFTableCell.XWPFVertAlign.BOTH);
      XWPFTableCell.stVertAlignTypeMap.Add(ST_VerticalJc.bottom, XWPFTableCell.XWPFVertAlign.BOTTOM);
    }

    public XWPFTableCell(CT_Tc cell, XWPFTableRow tableRow, IBody part)
    {
      this.ctTc = cell;
      this.part = part;
      this.tableRow = tableRow;
      if (cell.GetPList().Count < 1)
        cell.AddNewP();
      this.bodyElements = new List<IBodyElement>();
      this.paragraphs = new List<XWPFParagraph>();
      this.tables = new List<XWPFTable>();
      foreach (object obj in this.ctTc.Items)
      {
        if (obj is CT_P)
        {
          XWPFParagraph xwpfParagraph = new XWPFParagraph((CT_P) obj, (IBody) this);
          this.paragraphs.Add(xwpfParagraph);
          this.bodyElements.Add((IBodyElement) xwpfParagraph);
        }
        if (obj is CT_Tbl)
        {
          XWPFTable xwpfTable = new XWPFTable((CT_Tbl) obj, (IBody) this);
          this.tables.Add(xwpfTable);
          this.bodyElements.Add((IBodyElement) xwpfTable);
        }
      }
    }

    public CT_Tc GetCTTc()
    {
      return this.ctTc;
    }

    public IList<IBodyElement> BodyElements
    {
      get
      {
        return (IList<IBodyElement>) this.bodyElements.AsReadOnly();
      }
    }

    public void SetParagraph(XWPFParagraph p)
    {
      if (this.ctTc.SizeOfPArray() == 0)
        this.ctTc.AddNewP();
      this.ctTc.SetPArray(0, p.GetCTP());
    }

    public IList<XWPFParagraph> Paragraphs
    {
      get
      {
        return (IList<XWPFParagraph>) this.paragraphs;
      }
    }

    public XWPFParagraph AddParagraph()
    {
      XWPFParagraph p = new XWPFParagraph(this.ctTc.AddNewP(), (IBody) this);
      this.AddParagraph(p);
      return p;
    }

    public void AddParagraph(XWPFParagraph p)
    {
      this.paragraphs.Add(p);
    }

    public void RemoveParagraph(int pos)
    {
      this.paragraphs.RemoveAt(pos);
      this.ctTc.RemoveP(pos);
    }

    public XWPFParagraph GetParagraph(CT_P p)
    {
      foreach (XWPFParagraph paragraph in this.paragraphs)
      {
        if (p.Equals((object) paragraph.GetCTP()))
          return paragraph;
      }
      return (XWPFParagraph) null;
    }

    public void SetText(string text)
    {
      new XWPFParagraph(this.ctTc.SizeOfPArray() == 0 ? this.ctTc.AddNewP() : this.ctTc.GetPArray(0), (IBody) this).CreateRun().SetText(text);
    }

    public XWPFTableRow GetTableRow()
    {
      return this.tableRow;
    }

    public void SetColor(string rgbStr)
    {
      CT_TcPr ctTcPr = this.ctTc.IsSetTcPr() ? this.ctTc.tcPr : this.ctTc.AddNewTcPr();
      CT_Shd ctShd = ctTcPr.IsSetShd() ? ctTcPr.shd : ctTcPr.AddNewShd();
      ctShd.color = "auto";
      ctShd.val = ST_Shd.clear;
      ctShd.fill = rgbStr;
    }

    public string GetColor()
    {
      string str = (string) null;
      CT_TcPr tcPr = this.ctTc.tcPr;
      if (tcPr != null)
      {
        CT_Shd shd = tcPr.shd;
        if (shd != null)
          str = shd.fill;
      }
      return str;
    }

    public void SetVerticalAlignment(XWPFTableCell.XWPFVertAlign vAlign)
    {
      (this.ctTc.IsSetTcPr() ? (CT_TcPrBase) this.ctTc.tcPr : (CT_TcPrBase) this.ctTc.AddNewTcPr()).AddNewVAlign().val = XWPFTableCell.alignMap[vAlign];
    }

    public XWPFTableCell.XWPFVertAlign GetVerticalAlignment()
    {
      XWPFTableCell.XWPFVertAlign xwpfVertAlign = XWPFTableCell.XWPFVertAlign.TOP;
      CT_TcPr tcPr = this.ctTc.tcPr;
      if (this.ctTc != null)
      {
        CT_VerticalJc vAlign = tcPr.vAlign;
        xwpfVertAlign = XWPFTableCell.stVertAlignTypeMap[vAlign.val];
      }
      return xwpfVertAlign;
    }

    public XWPFParagraph insertNewParagraph(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    public XWPFTable insertNewTbl(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    private bool IsCursorInTableCell(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    public XWPFParagraph GetParagraphArray(int pos)
    {
      if (pos > 0 && pos < this.paragraphs.Count)
        return this.paragraphs[pos];
      return (XWPFParagraph) null;
    }

    public POIXMLDocumentPart GetPart()
    {
      return this.tableRow.GetTable().GetPart();
    }

    public BodyType GetPartType()
    {
      return BodyType.TABLECELL;
    }

    public XWPFTable GetTable(CT_Tbl ctTable)
    {
      for (int index = 0; index < this.tables.Count; ++index)
      {
        if (this.GetTables()[index].GetCTTbl() == ctTable)
          return this.GetTables()[index];
      }
      return (XWPFTable) null;
    }

    public XWPFTable GetTableArray(int pos)
    {
      if (pos > 0 && pos < this.tables.Count)
        return this.tables[pos];
      return (XWPFTable) null;
    }

    public IList<XWPFTable> GetTables()
    {
      return (IList<XWPFTable>) this.tables.AsReadOnly();
    }

    public void insertTable(int pos, XWPFTable table)
    {
      this.bodyElements.Insert(pos, (IBodyElement) table);
      int num = 0;
      while (num < this.ctTc.GetTblList().Count && this.ctTc.GetTblArray(num) != table.GetCTTbl())
        ++num;
      this.tables.Insert(num, table);
    }

    public string GetText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (XWPFParagraph paragraph in this.paragraphs)
        stringBuilder.Append(paragraph.GetText());
      return stringBuilder.ToString();
    }

    public XWPFTableCell GetTableCell(CT_Tc cell)
    {
      throw new NotImplementedException();
    }

    public XWPFDocument GetXWPFDocument()
    {
      return this.part.GetXWPFDocument();
    }

    public enum XWPFVertAlign
    {
      TOP,
      CENTER,
      BOTH,
      BOTTOM,
    }
  }
}
