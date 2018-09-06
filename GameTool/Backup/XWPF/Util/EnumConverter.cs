// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.Util.EnumConverter
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;

namespace NPOI.XWPF.Util
{
  public static class EnumConverter
  {
    public static ST_Jc ValueOf(ParagraphAlignment val)
    {
      switch (val)
      {
        case ParagraphAlignment.CENTER:
          return ST_Jc.center;
        case ParagraphAlignment.RIGHT:
          return ST_Jc.right;
        case ParagraphAlignment.BOTH:
          return ST_Jc.both;
        case ParagraphAlignment.MEDIUM_KASHIDA:
          return ST_Jc.mediumKashida;
        case ParagraphAlignment.DISTRIBUTE:
          return ST_Jc.distribute;
        case ParagraphAlignment.NUM_TAB:
          return ST_Jc.numTab;
        case ParagraphAlignment.HIGH_KASHIDA:
          return ST_Jc.highKashida;
        case ParagraphAlignment.LOW_KASHIDA:
          return ST_Jc.lowKashida;
        case ParagraphAlignment.THAI_DISTRIBUTE:
          return ST_Jc.thaiDistribute;
        default:
          return ST_Jc.left;
      }
    }

    public static ParagraphAlignment ValueOf(ST_Jc val)
    {
      switch (val)
      {
        case ST_Jc.center:
          return ParagraphAlignment.CENTER;
        case ST_Jc.both:
          return ParagraphAlignment.BOTH;
        case ST_Jc.distribute:
          return ParagraphAlignment.DISTRIBUTE;
        default:
          return ParagraphAlignment.LEFT;
      }
    }

    public static T ValueOf<T, F>(F val)
    {
      return (T) Enum.Parse(typeof (T), Enum.GetName(val.GetType(), (object) val), true);
    }
  }
}
