// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFNum
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;

namespace NPOI.XWPF.UserModel
{
  public class XWPFNum
  {
    private CT_Num ctNum;
    protected XWPFNumbering numbering;

    public XWPFNum()
    {
      this.ctNum = (CT_Num) null;
      this.numbering = (XWPFNumbering) null;
    }

    public XWPFNum(CT_Num ctNum)
    {
      this.ctNum = ctNum;
      this.numbering = (XWPFNumbering) null;
    }

    public XWPFNum(XWPFNumbering numbering)
    {
      this.ctNum = (CT_Num) null;
      this.numbering = numbering;
    }

    public XWPFNum(CT_Num ctNum, XWPFNumbering numbering)
    {
      this.ctNum = ctNum;
      this.numbering = numbering;
    }

    public XWPFNumbering GetNumbering()
    {
      return this.numbering;
    }

    public CT_Num GetCTNum()
    {
      return this.ctNum;
    }

    public void SetNumbering(XWPFNumbering numbering)
    {
      this.numbering = numbering;
    }

    public void SetCTNum(CT_Num ctNum)
    {
      this.ctNum = ctNum;
    }
  }
}
