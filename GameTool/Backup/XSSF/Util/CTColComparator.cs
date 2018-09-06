// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Util.CTColComparator
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using System.Collections.Generic;

namespace NPOI.XSSF.Util
{
  public class CTColComparator : Comparer<CT_Col>
  {
    public override int Compare(CT_Col o1, CT_Col o2)
    {
      if (o1.min < o2.min)
        return -1;
      if (o1.min > o2.min)
        return 1;
      if (o1.max < o2.max)
        return -1;
      return o1.max > o2.max ? 1 : 0;
    }
  }
}
