// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFParagraph
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPOI.XWPF.UserModel
{
  public class XWPFParagraph : IBodyElement
  {
    private StringBuilder footnoteText = new StringBuilder();
    private CT_P paragraph;
    protected IBody part;
    protected XWPFDocument document;
    protected List<XWPFRun> Runs;

    public XWPFParagraph(CT_P prgrph, IBody part)
    {
      this.paragraph = prgrph;
      this.part = part;
      this.document = part.GetXWPFDocument();
      if (this.document == null)
        throw new NullReferenceException();
      this.Runs = new List<XWPFRun>();
      this.BuildRunsInOrderFromXml(this.paragraph.Items);
      foreach (XWPFRun run in this.Runs)
      {
        CT_R ctr = run.GetCTR();
        if (this.document != null)
        {
          foreach (object obj in ctr.Items)
          {
            if (obj is CT_FtnEdnRef)
            {
              CT_FtnEdnRef ctFtnEdnRef = (CT_FtnEdnRef) obj;
              this.footnoteText.Append("[").Append(ctFtnEdnRef.id).Append(": ");
              XWPFFootnote xwpfFootnote = this.document.GetFootnoteByID(int.Parse(ctFtnEdnRef.id)) ?? this.document.GetEndnoteByID(int.Parse(ctFtnEdnRef.id));
              bool flag = true;
              foreach (XWPFParagraph paragraph in (IEnumerable<XWPFParagraph>) xwpfFootnote.Paragraphs)
              {
                if (!flag)
                {
                  this.footnoteText.Append("\n");
                  flag = false;
                }
                this.footnoteText.Append(paragraph.GetText());
              }
              this.footnoteText.Append("]");
            }
          }
        }
      }
    }

    private void BuildRunsInOrderFromXml(object[] items)
    {
      foreach (object obj in items)
      {
        if (obj is CT_R)
          this.Runs.Add(new XWPFRun((CT_R) obj, this));
        if (obj is CT_Hyperlink1)
        {
          CT_Hyperlink1 hyperlink = (CT_Hyperlink1) obj;
          foreach (CT_R Run in hyperlink.GetRList())
            this.Runs.Add((XWPFRun) new XWPFHyperlinkRun(hyperlink, Run, this));
        }
        if (obj is CT_SdtRun)
        {
          foreach (CT_R r in ((CT_SdtRun) obj).sdtContent.GetRList())
            this.Runs.Add(new XWPFRun(r, this));
        }
        if (obj is CT_RunTrackChange)
        {
          foreach (CT_R r in ((CT_RunTrackChange) obj).GetRList())
            this.Runs.Add(new XWPFRun(r, this));
        }
        if (obj is CT_SimpleField)
        {
          foreach (CT_R r in ((CT_SimpleField) obj).GetRList())
            this.Runs.Add(new XWPFRun(r, this));
        }
        if (obj is CT_SmartTagRun)
          this.BuildRunsInOrderFromXml((obj as CT_SmartTagRun).Items);
      }
    }

    public CT_P GetCTP()
    {
      return this.paragraph;
    }

    public IList<XWPFRun> GetRuns()
    {
      return (IList<XWPFRun>) this.Runs.AsReadOnly();
    }

    public bool IsEmpty()
    {
      return this.paragraph.Items.Length == 0;
    }

    public XWPFDocument GetDocument()
    {
      return this.document;
    }

    public string GetText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (XWPFRun run in this.Runs)
        stringBuilder.Append(run.ToString());
      stringBuilder.Append((object) this.footnoteText);
      return stringBuilder.ToString();
    }

    public string GetStyleID()
    {
      throw new NotImplementedException();
    }

    public string GetNumID()
    {
      if (this.paragraph.pPr != null && this.paragraph.pPr.numPr != null && this.paragraph.pPr.numPr.numId != null)
        return this.paragraph.pPr.numPr.numId.val;
      return (string) null;
    }

    public void SetNumID(string numId)
    {
      if (this.paragraph.pPr == null)
        this.paragraph.AddNewPPr();
      if (this.paragraph.pPr.numPr == null)
        this.paragraph.pPr.AddNewNumPr();
      if (this.paragraph.pPr.numPr.numId == null)
        this.paragraph.pPr.numPr.AddNewNumId();
      this.paragraph.pPr.numPr.ilvl.val = "0";
      this.paragraph.pPr.numPr.numId.val = numId;
    }

    public void SetNumID(string numId, string ilvl)
    {
      if (this.paragraph.pPr == null)
        this.paragraph.AddNewPPr();
      if (this.paragraph.pPr.numPr == null)
        this.paragraph.pPr.AddNewNumPr();
      if (this.paragraph.pPr.numPr.numId == null)
        this.paragraph.pPr.numPr.AddNewNumId();
      this.paragraph.pPr.numPr.ilvl.val = ilvl;
      this.paragraph.pPr.numPr.numId.val = numId;
    }

    public string GetParagraphText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (XWPFRun run in this.Runs)
        stringBuilder.Append(run.ToString());
      return stringBuilder.ToString();
    }

    public string GetPictureText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (XWPFRun run in this.Runs)
        stringBuilder.Append(run.GetPictureText());
      return stringBuilder.ToString();
    }

    public string GetFootnoteText()
    {
      return this.footnoteText.ToString();
    }

    public XWPFRun CreateRun()
    {
      XWPFRun xwpfRun = new XWPFRun(this.paragraph.AddNewR(), this);
      this.Runs.Add(xwpfRun);
      return xwpfRun;
    }

    public ParagraphAlignment GetAlignment()
    {
      CT_PPr ctpPr = this.GetCTPPr();
      if (ctpPr != null && ctpPr.IsSetJc())
        return EnumConverter.ValueOf<ParagraphAlignment, ST_Jc>(ctpPr.jc.val);
      return ParagraphAlignment.LEFT;
    }

    public void SetAlignment(ParagraphAlignment align)
    {
      CT_PPr ctpPr = this.GetCTPPr();
      (ctpPr.IsSetJc() ? ctpPr.jc : ctpPr.AddNewJc()).val = EnumConverter.ValueOf<ST_Jc, ParagraphAlignment>(align);
    }

    public TextAlignment GetVerticalAlignment()
    {
      CT_PPr ctpPr = this.GetCTPPr();
      if (ctpPr != null && ctpPr.IsSetTextAlignment())
        return EnumConverter.ValueOf<TextAlignment, ST_TextAlignment>(ctpPr.textAlignment.val);
      return TextAlignment.AUTO;
    }

    public void SetVerticalAlignment(TextAlignment valign)
    {
      CT_PPr ctpPr = this.GetCTPPr();
      (ctpPr.IsSetTextAlignment() ? ctpPr.textAlignment : ctpPr.AddNewTextAlignment()).val = EnumConverter.ValueOf<ST_TextAlignment, TextAlignment>(valign);
    }

    public void SetBorderTop(Borders border)
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(true);
      CT_Border ctBorder = ctpBrd == null || !ctpBrd.IsSetTop() ? ctpBrd.AddNewTop() : ctpBrd.top;
      if (border == Borders.NONE)
        ctpBrd.UnsetTop();
      else
        ctBorder.val = EnumConverter.ValueOf<ST_Border, Borders>(border);
    }

    public Borders GetBorderTop()
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(false);
      CT_Border ctBorder = (CT_Border) null;
      if (ctpBrd != null)
        ctBorder = ctpBrd.top;
      return EnumConverter.ValueOf<Borders, ST_Border>(ctBorder != null ? ctBorder.val : ST_Border.none);
    }

    public void SetBorderBottom(Borders border)
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(true);
      CT_Border ctBorder = ctpBrd.IsSetBottom() ? ctpBrd.bottom : ctpBrd.AddNewBottom();
      if (border == Borders.NONE)
        ctpBrd.UnsetBottom();
      else
        ctBorder.val = EnumConverter.ValueOf<ST_Border, Borders>(border);
    }

    public Borders GetBorderBottom()
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(false);
      CT_Border ctBorder = (CT_Border) null;
      if (ctpBrd != null)
        ctBorder = ctpBrd.bottom;
      return EnumConverter.ValueOf<Borders, ST_Border>(ctBorder != null ? ctBorder.val : ST_Border.none);
    }

    public void SetBorderLeft(Borders border)
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(true);
      CT_Border ctBorder = ctpBrd.IsSetLeft() ? ctpBrd.left : ctpBrd.AddNewLeft();
      if (border == Borders.NONE)
        ctpBrd.UnsetLeft();
      else
        ctBorder.val = EnumConverter.ValueOf<ST_Border, Borders>(border);
    }

    public Borders GetBorderLeft()
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(false);
      CT_Border ctBorder = (CT_Border) null;
      if (ctpBrd != null)
        ctBorder = ctpBrd.left;
      return EnumConverter.ValueOf<Borders, ST_Border>(ctBorder != null ? ctBorder.val : ST_Border.none);
    }

    public void SetBorderRight(Borders border)
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(true);
      CT_Border ctBorder = ctpBrd.IsSetRight() ? ctpBrd.right : ctpBrd.AddNewRight();
      if (border == Borders.NONE)
        ctpBrd.UnsetRight();
      else
        ctBorder.val = EnumConverter.ValueOf<ST_Border, Borders>(border);
    }

    public Borders GetBorderRight()
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(false);
      CT_Border ctBorder = (CT_Border) null;
      if (ctpBrd != null)
        ctBorder = ctpBrd.right;
      return EnumConverter.ValueOf<Borders, ST_Border>(ctBorder != null ? ctBorder.val : ST_Border.none);
    }

    public void SetBorderBetween(Borders border)
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(true);
      CT_Border ctBorder = ctpBrd.IsSetBetween() ? ctpBrd.between : ctpBrd.AddNewBetween();
      if (border == Borders.NONE)
        ctpBrd.UnsetBetween();
      else
        ctBorder.val = EnumConverter.ValueOf<ST_Border, Borders>(border);
    }

    public Borders GetBorderBetween()
    {
      CT_PBdr ctpBrd = this.GetCTPBrd(false);
      CT_Border ctBorder = (CT_Border) null;
      if (ctpBrd != null)
        ctBorder = ctpBrd.between;
      return EnumConverter.ValueOf<Borders, ST_Border>(ctBorder != null ? ctBorder.val : ST_Border.none);
    }

    public void SetPageBreak(bool pageBreak)
    {
      CT_PPr ctpPr = this.GetCTPPr();
      CT_OnOff ctOnOff = ctpPr.IsSetPageBreakBefore() ? ctpPr.pageBreakBefore : ctpPr.AddNewPageBreakBefore();
      if (pageBreak)
        ctOnOff.val = ST_OnOff.True;
      else
        ctOnOff.val = ST_OnOff.False;
    }

    public bool IsPageBreak()
    {
      CT_PPr ctpPr = this.GetCTPPr();
      CT_OnOff ctOnOff = ctpPr.IsSetPageBreakBefore() ? ctpPr.pageBreakBefore : (CT_OnOff) null;
      return ctOnOff != null && ctOnOff.val == ST_OnOff.True;
    }

    public void SetSpacingAfter(int spaces)
    {
      CT_Spacing ctSpacing = this.GetCTSpacing(true);
      if (ctSpacing == null)
        return;
      ctSpacing.after = (ulong) spaces;
    }

    public int GetSpacingAfter()
    {
      CT_Spacing ctSpacing = this.GetCTSpacing(false);
      if (ctSpacing == null || !ctSpacing.IsSetAfter())
        return -1;
      return (int) ctSpacing.after;
    }

    public void SetSpacingAfterLines(int spaces)
    {
      this.GetCTSpacing(true).afterLines = spaces.ToString();
    }

    public int GetSpacingAfterLines()
    {
      CT_Spacing ctSpacing = this.GetCTSpacing(false);
      if (ctSpacing == null || !ctSpacing.IsSetAfterLines())
        return -1;
      return int.Parse(ctSpacing.afterLines);
    }

    public void SetSpacingBefore(int spaces)
    {
      this.GetCTSpacing(true).before = (ulong) spaces;
    }

    public int GetSpacingBefore()
    {
      CT_Spacing ctSpacing = this.GetCTSpacing(false);
      if (ctSpacing == null || !ctSpacing.IsSetBefore())
        return -1;
      return (int) ctSpacing.before;
    }

    public void SetSpacingBeforeLines(int spaces)
    {
      this.GetCTSpacing(true).beforeLines = spaces.ToString();
    }

    public int GetSpacingBeforeLines()
    {
      CT_Spacing ctSpacing = this.GetCTSpacing(false);
      if (ctSpacing == null || !ctSpacing.IsSetBeforeLines())
        return -1;
      return int.Parse(ctSpacing.beforeLines);
    }

    public void SetSpacingLineRule(LineSpacingRule rule)
    {
      this.GetCTSpacing(true).lineRule = EnumConverter.ValueOf<ST_LineSpacingRule, LineSpacingRule>(rule);
    }

    public LineSpacingRule GetSpacingLineRule()
    {
      CT_Spacing ctSpacing = this.GetCTSpacing(false);
      if (ctSpacing == null || !ctSpacing.IsSetLineRule())
        return LineSpacingRule.AUTO;
      return EnumConverter.ValueOf<LineSpacingRule, ST_LineSpacingRule>(ctSpacing.lineRule);
    }

    public void SetIndentationLeft(int indentation)
    {
      this.GetCTInd(true).left = indentation.ToString();
    }

    public int GetIndentationLeft()
    {
      CT_Ind ctInd = this.GetCTInd(false);
      if (ctInd == null || !ctInd.IsSetLeft())
        return -1;
      return int.Parse(ctInd.left);
    }

    public void SetIndentationRight(int indentation)
    {
      this.GetCTInd(true).right = indentation.ToString();
    }

    public int GetIndentationRight()
    {
      CT_Ind ctInd = this.GetCTInd(false);
      if (ctInd == null || !ctInd.IsSetRight())
        return -1;
      return int.Parse(ctInd.right);
    }

    public void SetIndentationHanging(int indentation)
    {
      this.GetCTInd(true).hanging = (ulong) indentation;
    }

    public int GetIndentationHanging()
    {
      CT_Ind ctInd = this.GetCTInd(false);
      if (ctInd == null || !ctInd.IsSetHanging())
        return -1;
      return (int) ctInd.hanging;
    }

    public void SetIndentationFirstLine(int indentation)
    {
      this.GetCTInd(true).firstLine = (ulong) indentation;
    }

    public int GetIndentationFirstLine()
    {
      CT_Ind ctInd = this.GetCTInd(false);
      if (ctInd == null || !ctInd.IsSetFirstLine())
        return -1;
      return (int) ctInd.firstLine;
    }

    public void SetWordWrap(bool wrap)
    {
      CT_OnOff ctOnOff = this.GetCTPPr().IsSetWordWrap() ? this.GetCTPPr().wordWrap : this.GetCTPPr().AddNewWordWrap();
      if (wrap)
        ctOnOff.val = ST_OnOff.True;
      else
        ctOnOff.UnSetVal();
    }

    public bool IsWordWrap()
    {
      CT_OnOff ctOnOff = this.GetCTPPr().IsSetWordWrap() ? this.GetCTPPr().wordWrap : (CT_OnOff) null;
      return ctOnOff != null && (ctOnOff.val == ST_OnOff.on || ctOnOff.val == ST_OnOff.True || ctOnOff.val == ST_OnOff.Value1);
    }

    public void SetStyle(string newStyle)
    {
      CT_PPr ctpPr = this.GetCTPPr();
      (ctpPr.pStyle != null ? ctpPr.pStyle : ctpPr.AddNewPStyle()).val = newStyle;
    }

    public string GetStyle()
    {
      CT_PPr ctpPr = this.GetCTPPr();
      return (ctpPr.IsSetPStyle() ? ctpPr.pStyle : (CT_String) null)?.val;
    }

    private CT_PBdr GetCTPBrd(bool create)
    {
      CT_PPr ctpPr = this.GetCTPPr();
      CT_PBdr ctPbdr = ctpPr.IsSetPBdr() ? ctpPr.pBdr : (CT_PBdr) null;
      if (create && ctPbdr == null)
        ctPbdr = ctpPr.AddNewPBdr();
      return ctPbdr;
    }

    private CT_Spacing GetCTSpacing(bool create)
    {
      CT_PPr ctpPr = this.GetCTPPr();
      CT_Spacing ctSpacing = ctpPr.spacing == null ? (CT_Spacing) null : ctpPr.spacing;
      if (create && ctSpacing == null)
        ctSpacing = ctpPr.AddNewSpacing();
      return ctSpacing;
    }

    private CT_Ind GetCTInd(bool create)
    {
      CT_PPr ctpPr = this.GetCTPPr();
      CT_Ind ctInd = ctpPr.ind == null ? (CT_Ind) null : ctpPr.ind;
      if (create && ctInd == null)
        ctInd = ctpPr.AddNewInd();
      return ctInd;
    }

    private CT_PPr GetCTPPr()
    {
      return this.paragraph.pPr == null ? this.paragraph.AddNewPPr() : this.paragraph.pPr;
    }

    protected void AddRun(CT_R Run)
    {
      throw new NotImplementedException();
    }

    public TextSegement searchText(string searched, PositionInParagraph startPos)
    {
      throw new NotImplementedException();
    }

    public XWPFRun InsertNewRun(int pos)
    {
      if (pos < 0 || pos > this.paragraph.SizeOfRArray())
        return (XWPFRun) null;
      XWPFRun xwpfRun = new XWPFRun(this.paragraph.InsertNewR(pos), this);
      this.Runs.Insert(pos, xwpfRun);
      return xwpfRun;
    }

    public string GetText(TextSegement segment)
    {
      throw new NotImplementedException();
    }

    public bool RemoveRun(int pos)
    {
      if (pos < 0 || pos >= this.paragraph.SizeOfRArray())
        return false;
      this.GetCTP().RemoveR(pos);
      this.Runs.RemoveAt(pos);
      return true;
    }

    public BodyElementType ElementType
    {
      get
      {
        return BodyElementType.PARAGRAPH;
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

    public void AddRun(XWPFRun r)
    {
      if (this.Runs.Contains(r))
        return;
      this.Runs.Add(r);
    }

    public XWPFRun GetRun(CT_R r)
    {
      for (int index = 0; index < this.GetRuns().Count; ++index)
      {
        if (this.GetRuns()[index].GetCTR() == r)
          return this.GetRuns()[index];
      }
      return (XWPFRun) null;
    }
  }
}
