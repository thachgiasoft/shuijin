// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.PositionInParagraph
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

namespace NPOI.XWPF.UserModel
{
  public class PositionInParagraph
  {
    private int posRun;
    private int posText;
    private int posChar;

    public PositionInParagraph()
    {
    }

    public PositionInParagraph(int posRun, int posText, int posChar)
    {
      this.posRun = posRun;
      this.posChar = posChar;
      this.posText = posText;
    }

    public int Run
    {
      get
      {
        return this.posRun;
      }
      set
      {
        this.posRun = value;
      }
    }

    public int Text
    {
      get
      {
        return this.posText;
      }
      set
      {
        this.posText = value;
      }
    }

    public int Char
    {
      get
      {
        return this.posChar;
      }
      set
      {
        this.posChar = value;
      }
    }
  }
}
