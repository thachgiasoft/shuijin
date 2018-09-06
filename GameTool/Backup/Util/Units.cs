// Decompiled with JetBrains decompiler
// Type: NPOI.Util.Units
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using System;

namespace NPOI.Util
{
  public class Units
  {
    public static int EMU_PER_PIXEL = 9525;
    public static int EMU_PER_POINT = 12700;

    public static int ToEMU(double value)
    {
      return (int) Math.Round((double) Units.EMU_PER_POINT * value);
    }

    public static double ToPoints(long emu)
    {
      return (double) emu / (double) Units.EMU_PER_POINT;
    }
  }
}
