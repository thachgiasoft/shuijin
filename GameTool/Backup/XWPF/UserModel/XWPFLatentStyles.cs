// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFLatentStyles
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Wordprocessing;
using System.Collections.Generic;

namespace NPOI.XWPF.UserModel
{
  public class XWPFLatentStyles
  {
    private CT_LatentStyles latentStyles;
    protected XWPFStyles styles;

    protected XWPFLatentStyles()
    {
    }

    public XWPFLatentStyles(CT_LatentStyles latentStyles)
      : this(latentStyles, (XWPFStyles) null)
    {
    }

    public XWPFLatentStyles(CT_LatentStyles latentStyles, XWPFStyles styles)
    {
      this.latentStyles = latentStyles;
      this.styles = styles;
    }

    public bool IsLatentStyle(string latentStyleID)
    {
      using (List<CT_LsdException>.Enumerator enumerator = this.latentStyles.lsdException.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          enumerator.Current.name.Equals(latentStyleID);
          return true;
        }
      }
      return false;
    }
  }
}
