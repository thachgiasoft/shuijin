// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.TOC
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using System.Text;

namespace NPOI.XWPF.UserModel
{
  public class TOC
  {
    private CT_SdtBlock block;

    public TOC()
      : this(new CT_SdtBlock())
    {
    }

    public TOC(CT_SdtBlock block)
    {
      this.block = block;
      CT_SdtPr ctSdtPr = block.AddNewSdtPr();
      ctSdtPr.AddNewId().val = "4844945";
      ctSdtPr.AddNewDocPartObj().AddNewDocPartGallery().val = "Table of contents";
      CT_RPr ctRpr = block.AddNewSdtEndPr().AddNewRPr();
      CT_Fonts ctFonts = ctRpr.AddNewRFonts();
      ctFonts.asciiTheme = ST_Theme.minorHAnsi;
      ctFonts.eastAsiaTheme = ST_Theme.minorHAnsi;
      ctFonts.hAnsiTheme = ST_Theme.minorHAnsi;
      ctFonts.cstheme = ST_Theme.minorBidi;
      ctRpr.AddNewB().val = ST_OnOff.off;
      ctRpr.AddNewBCs().val = ST_OnOff.off;
      ctRpr.AddNewColor().val = "auto";
      ctRpr.AddNewSz().val = 24UL;
      ctRpr.AddNewSzCs().val = 24UL;
      CT_P ctP = block.AddNewSdtContent().AddNewP();
      byte[] bytes = Encoding.Unicode.GetBytes("00EF7E24");
      ctP.rsidR = bytes;
      ctP.rsidRDefault = bytes;
      ctP.AddNewPPr().AddNewPStyle().val = "TOCHeading";
      ctP.AddNewR().AddNewT().Value = "Table of Contents";
    }

    public CT_SdtBlock GetBlock()
    {
      return this.block;
    }

    public void AddRow(int level, string title, int page, string bookmarkRef)
    {
      CT_P ctP = this.block.sdtContent.AddNewP();
      byte[] bytes = Encoding.Unicode.GetBytes("00EF7E24");
      ctP.rsidR = bytes;
      ctP.rsidRDefault = bytes;
      CT_PPr ctPpr = ctP.AddNewPPr();
      ctPpr.AddNewPStyle().val = nameof (TOC) + (object) level;
      CT_TabStop ctTabStop = ctPpr.AddNewTabs().AddNewTab();
      ctTabStop.val = ST_TabJc.right;
      ctTabStop.leader = ST_TabTlc.dot;
      ctTabStop.pos = "8290";
      ctPpr.AddNewRPr().AddNewNoProof();
      CT_R ctR1 = ctP.AddNewR();
      ctR1.AddNewRPr().AddNewNoProof();
      ctR1.AddNewT().Value = title;
      CT_R ctR2 = ctP.AddNewR();
      ctR2.AddNewRPr().AddNewNoProof();
      ctR2.AddNewTab();
      CT_R ctR3 = ctP.AddNewR();
      ctR3.AddNewRPr().AddNewNoProof();
      ctR3.AddNewFldChar().fldCharType = ST_FldCharType.begin;
      CT_R ctR4 = ctP.AddNewR();
      ctR4.AddNewRPr().AddNewNoProof();
      CT_Text ctText = ctR4.AddNewInstrText();
      ctText.space = "preserve";
      ctText.Value = " PAGEREF _Toc" + bookmarkRef + " \\h ";
      ctP.AddNewR().AddNewRPr().AddNewNoProof();
      CT_R ctR5 = ctP.AddNewR();
      ctR5.AddNewRPr().AddNewNoProof();
      ctR5.AddNewFldChar().fldCharType = ST_FldCharType.separate;
      CT_R ctR6 = ctP.AddNewR();
      ctR6.AddNewRPr().AddNewNoProof();
      ctR6.AddNewT().Value = page.ToString();
      CT_R ctR7 = ctP.AddNewR();
      ctR7.AddNewRPr().AddNewNoProof();
      ctR7.AddNewFldChar().fldCharType = ST_FldCharType.end;
    }
  }
}
