// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.TextSegement
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

namespace NPOI.XWPF.UserModel
{
  public class TextSegement
  {
    private PositionInParagraph beginPos;
    private PositionInParagraph endPos;

    public TextSegement()
    {
      this.beginPos = new PositionInParagraph();
      this.endPos = new PositionInParagraph();
    }

    public TextSegement(int beginRun, int endRun, int beginText, int endText, int beginChar, int endChar)
    {
      PositionInParagraph positionInParagraph1 = new PositionInParagraph(beginRun, beginText, beginChar);
      PositionInParagraph positionInParagraph2 = new PositionInParagraph(endRun, endText, endChar);
      this.beginPos = positionInParagraph1;
      this.endPos = positionInParagraph2;
    }

    public TextSegement(PositionInParagraph beginPos, PositionInParagraph endPos)
    {
      this.beginPos = beginPos;
      this.endPos = endPos;
    }

    public PositionInParagraph GetBeginPos()
    {
      return this.beginPos;
    }

    public PositionInParagraph GetEndPos()
    {
      return this.endPos;
    }

    public int GetBeginRun()
    {
      return this.beginPos.Run;
    }

    public void SetBeginRun(int beginRun)
    {
      this.beginPos.Run = beginRun;
    }

    public int GetBeginText()
    {
      return this.beginPos.Text;
    }

    public void SetBeginText(int beginText)
    {
      this.beginPos.Text = beginText;
    }

    public int GetBeginChar()
    {
      return this.beginPos.Char;
    }

    public void SetBeginChar(int beginChar)
    {
      this.beginPos.Char = beginChar;
    }

    public int GetEndRun()
    {
      return this.endPos.Run;
    }

    public void SetEndRun(int endRun)
    {
      this.endPos.Run = endRun;
    }

    public int GetEndText()
    {
      return this.endPos.Text;
    }

    public void SetEndText(int endText)
    {
      this.endPos.Text = endText;
    }

    public int GetEndChar()
    {
      return this.endPos.Char;
    }

    public void SetEndChar(int endChar)
    {
      this.endPos.Char = endChar;
    }
  }
}
