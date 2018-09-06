// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Model.ThemesTable
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NPOI.XSSF.Model
{
  public class ThemesTable : POIXMLDocumentPart
  {
    private ThemeDocument theme;

    internal ThemesTable(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      try
      {
        this.theme = ThemeDocument.Parse(part.GetInputStream());
      }
      catch (XmlException ex)
      {
        throw new IOException(ex.Message);
      }
    }

    internal ThemesTable(ThemeDocument theme)
    {
      this.theme = theme;
    }

    public XSSFColor GetThemeColor(int idx)
    {
      CT_ColorScheme clrScheme = this.theme.GetTheme().themeElements.clrScheme;
      int num = 0;
      foreach (NPOI.OpenXmlFormats.Dml.CT_Color ctColor1 in new List<NPOI.OpenXmlFormats.Dml.CT_Color>() { clrScheme.dk1, clrScheme.lt1, clrScheme.dk2, clrScheme.lt2, clrScheme.accent1, clrScheme.accent2, clrScheme.accent3, clrScheme.accent4, clrScheme.accent5, clrScheme.accent6, clrScheme.hlink, clrScheme.folHlink })
      {
        if (num == idx)
        {
          NPOI.OpenXmlFormats.Dml.CT_Color ctColor2 = ctColor1;
          byte[] rgb = (byte[]) null;
          if (ctColor2.srgbClr != null)
            rgb = ctColor2.srgbClr.val;
          else if (ctColor2.sysClr != null)
            rgb = ctColor2.sysClr.lastClr;
          return new XSSFColor(rgb);
        }
        ++num;
      }
      return (XSSFColor) null;
    }

    public void InheritFromThemeAsRequired(XSSFColor color)
    {
      if (color == null || !color.GetCTColor().themeSpecified)
        return;
      XSSFColor themeColor = this.GetThemeColor(color.GetTheme());
      color.GetCTColor().SetRgb(themeColor.GetCTColor().GetRgb());
    }
  }
}
