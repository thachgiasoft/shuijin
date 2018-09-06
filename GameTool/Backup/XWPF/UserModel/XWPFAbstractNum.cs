// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFAbstractNum
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.Util;
using System.Collections.Generic;

namespace NPOI.XWPF.UserModel
{
  public class XWPFAbstractNum
  {
    private char[] lvlText = new char[3]{ 'n', 'l', 'u' };
    private CT_AbstractNum ctAbstractNum;
    protected XWPFNumbering numbering;

    protected XWPFAbstractNum()
    {
      this.ctAbstractNum = (CT_AbstractNum) null;
      this.numbering = (XWPFNumbering) null;
    }

    public XWPFAbstractNum(CT_AbstractNum abstractNum)
    {
      this.ctAbstractNum = abstractNum;
    }

    public XWPFAbstractNum(CT_AbstractNum ctAbstractNum, XWPFNumbering numbering)
    {
      this.ctAbstractNum = ctAbstractNum;
      this.numbering = numbering;
    }

    public CT_AbstractNum GetAbstractNum()
    {
      return this.ctAbstractNum;
    }

    public XWPFNumbering GetNumbering()
    {
      return this.numbering;
    }

    public CT_AbstractNum GetCTAbstractNum()
    {
      return this.ctAbstractNum;
    }

    public void SetNumbering(XWPFNumbering numbering)
    {
      this.numbering = numbering;
    }

    public MultiLevelType MultiLevelType
    {
      get
      {
        return EnumConverter.ValueOf<MultiLevelType, ST_MultiLevelType>(this.ctAbstractNum.multiLevelType.val);
      }
      set
      {
        this.ctAbstractNum.multiLevelType.val = EnumConverter.ValueOf<ST_MultiLevelType, MultiLevelType>(value);
      }
    }

    public string AbstractNumId
    {
      get
      {
        return this.ctAbstractNum.abstractNumId;
      }
      set
      {
        this.ctAbstractNum.abstractNumId = value;
      }
    }

    internal void InitLvl()
    {
      List<CT_Lvl> ctLvlList = new List<CT_Lvl>();
      for (int index = 0; index < 9; ++index)
      {
        CT_Lvl ctLvl = new CT_Lvl();
        ctLvl.start.val = "1";
        ctLvl.tentative = index == 0 ? ST_OnOff.Value0 : ST_OnOff.Value1;
        ctLvl.ilvl = index.ToString();
        ctLvl.lvlJc.val = ST_Jc.left;
        ctLvl.numFmt.val = ST_NumberFormat.bullet;
        ctLvl.lvlText.val = this.lvlText[index % 3].ToString();
        CT_Ind ctInd = ctLvl.pPr.AddNewInd();
        ctInd.left = (420 * (index + 1)).ToString();
        ctInd.hanging = 420UL;
        CT_Fonts ctFonts = ctLvl.rPr.AddNewRFonts();
        ctFonts.ascii = "Wingdings";
        ctFonts.hAnsi = "Wingdings";
        ctFonts.hint = ST_Hint.@default;
        ctLvlList.Add(ctLvl);
      }
      this.ctAbstractNum.lvl = ctLvlList.ToArray();
    }

    internal void SetLevelTentative(int lvl, bool tentative)
    {
      if (tentative)
        this.ctAbstractNum.lvl[lvl].tentative = ST_OnOff.Value1;
      else
        this.ctAbstractNum.lvl[lvl].tentative = ST_OnOff.Value0;
    }
  }
}
