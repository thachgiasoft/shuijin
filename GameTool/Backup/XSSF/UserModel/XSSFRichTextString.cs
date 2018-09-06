// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFRichTextString
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NPOI.XSSF.UserModel
{
  public class XSSFRichTextString : IRichTextString
  {
    private static Regex utfPtrn = new Regex("_x([0-9A-F]{4})_");
    private CT_Rst st;
    private StylesTable styles;

    public XSSFRichTextString(string str)
    {
      this.st = new CT_Rst();
      this.st.t = str;
      XSSFRichTextString.PreserveSpaces(this.st.t);
    }

    public void SetStylesTableReference(StylesTable stylestable)
    {
      this.styles = stylestable;
      if (this.st.sizeOfRArray() <= 0)
        return;
      foreach (CT_RElt ctRelt in this.st.r)
      {
        CT_RPrElt rPr = ctRelt.rPr;
        if (rPr != null && rPr.sizeOfRFontArray() > 0)
        {
          string val = rPr.GetRFontArray(0).val;
          if (val.StartsWith("#"))
          {
            XSSFFont fontAt = this.styles.GetFontAt(int.Parse(val.Substring(1)));
            rPr.rFont = (CT_FontName) null;
            this.SetRunAttributes(fontAt.GetCTFont(), rPr);
          }
        }
      }
    }

    public XSSFRichTextString()
    {
      this.st = new CT_Rst();
    }

    public XSSFRichTextString(CT_Rst st)
    {
      this.st = st;
    }

    public void ApplyFont(int startIndex, int endIndex, short fontIndex)
    {
      XSSFFont xssfFont;
      if (this.styles == null)
      {
        xssfFont = new XSSFFont();
        xssfFont.FontName = "#" + (object) fontIndex;
      }
      else
        xssfFont = this.styles.GetFontAt((int) fontIndex);
      this.ApplyFont(startIndex, endIndex, (IFont) xssfFont);
    }

    internal void ApplyFont(SortedDictionary<int, CT_RPrElt> formats, int startIndex, int endIndex, CT_RPrElt fmt)
    {
      List<int> intList = new List<int>();
      SortedDictionary<int, CT_RPrElt>.KeyCollection.Enumerator enumerator = formats.Keys.GetEnumerator();
      while (enumerator.MoveNext())
      {
        int current = enumerator.Current;
        if (current >= startIndex && current < endIndex)
          intList.Add(current);
      }
      foreach (int key in intList)
        formats.Remove(key);
      if (startIndex > 0 && !formats.ContainsKey(startIndex))
      {
        foreach (KeyValuePair<int, CT_RPrElt> format in formats)
        {
          if (format.Key > startIndex)
          {
            formats[startIndex] = format.Value;
            break;
          }
        }
      }
      formats[endIndex] = fmt;
    }

    public void ApplyFont(int startIndex, int endIndex, IFont font)
    {
      if (startIndex > endIndex)
        throw new ArgumentException("Start index must be less than end index.");
      if (startIndex < 0 || endIndex > this.Length)
        throw new ArgumentException("Start and end index not in range.");
      if (startIndex == endIndex)
        return;
      if (this.st.sizeOfRArray() == 0 && this.st.IsSetT())
      {
        this.st.AddNewR().t = this.st.t;
        this.st.unsetT();
      }
      string text = this.String;
      XSSFFont xssfFont = (XSSFFont) font;
      SortedDictionary<int, CT_RPrElt> formatMap = this.GetFormatMap(this.st);
      CT_RPrElt ctRprElt = new CT_RPrElt();
      this.SetRunAttributes(xssfFont.GetCTFont(), ctRprElt);
      this.ApplyFont(formatMap, startIndex, endIndex, ctRprElt);
      this.st.Set(this.buildCTRst(text, formatMap));
    }

    internal SortedDictionary<int, CT_RPrElt> GetFormatMap(CT_Rst entry)
    {
      int index = 0;
      SortedDictionary<int, CT_RPrElt> sortedDictionary = new SortedDictionary<int, CT_RPrElt>();
      foreach (CT_RElt ctRelt in entry.r)
      {
        string t = ctRelt.t;
        CT_RPrElt rPr = ctRelt.rPr;
        index += t.Length;
        sortedDictionary[index] = rPr;
      }
      return sortedDictionary;
    }

    public void ApplyFont(IFont font)
    {
      this.ApplyFont(0, this.String.Length, font);
    }

    public void ApplyFont(short fontIndex)
    {
      XSSFFont xssfFont;
      if (this.styles == null)
      {
        xssfFont = new XSSFFont();
        xssfFont.FontName = "#" + (object) fontIndex;
      }
      else
        xssfFont = this.styles.GetFontAt((int) fontIndex);
      this.ApplyFont(0, this.String.Length, (IFont) xssfFont);
    }

    public void Append(string text, XSSFFont font)
    {
      if (this.st.sizeOfRArray() == 0 && this.st.IsSetT())
      {
        CT_RElt ctRelt = this.st.AddNewR();
        ctRelt.t = this.st.t;
        XSSFRichTextString.PreserveSpaces(ctRelt.t);
        this.st.unsetT();
      }
      CT_RElt ctRelt1 = this.st.AddNewR();
      ctRelt1.t = text;
      XSSFRichTextString.PreserveSpaces(ctRelt1.t);
      CT_RPrElt pr = ctRelt1.AddNewRPr();
      if (font == null)
        return;
      this.SetRunAttributes(font.GetCTFont(), pr);
    }

    public void Append(string text)
    {
      this.Append(text, (XSSFFont) null);
    }

    private void SetRunAttributes(CT_Font ctFont, CT_RPrElt pr)
    {
      if (ctFont.sizeOfBArray() > 0)
        pr.AddNewB().val = ctFont.GetBArray(0).val;
      if (ctFont.sizeOfUArray() > 0)
        pr.AddNewU().val = ctFont.GetUArray(0).val;
      if (ctFont.sizeOfIArray() > 0)
        pr.AddNewI().val = ctFont.GetIArray(0).val;
      if (ctFont.sizeOfColorArray() > 0)
      {
        CT_Color colorArray = ctFont.GetColorArray(0);
        CT_Color ctColor = pr.AddNewColor();
        if (colorArray.IsSetAuto())
        {
          ctColor.auto = colorArray.auto;
          ctColor.autoSpecified = true;
        }
        if (colorArray.IsSetIndexed())
        {
          ctColor.indexed = colorArray.indexed;
          ctColor.indexedSpecified = true;
        }
        if (colorArray.IsSetRgb())
        {
          ctColor.SetRgb(colorArray.rgb);
          ctColor.rgbSpecified = true;
        }
        if (colorArray.IsSetTheme())
        {
          ctColor.theme = colorArray.theme;
          ctColor.themeSpecified = true;
        }
        if (colorArray.IsSetTint())
        {
          ctColor.tint = colorArray.tint;
          ctColor.tintSpecified = true;
        }
      }
      if (ctFont.sizeOfSzArray() > 0)
        pr.AddNewSz().val = ctFont.GetSzArray(0).val;
      if (ctFont.sizeOfNameArray() > 0)
        pr.AddNewRFont().val = ctFont.GetNameArray(0).val;
      if (ctFont.sizeOfFamilyArray() > 0)
        pr.AddNewFamily().val = ctFont.GetFamilyArray(0).val;
      if (ctFont.sizeOfSchemeArray() > 0)
        pr.AddNewScheme().val = ctFont.GetSchemeArray(0).val;
      if (ctFont.sizeOfCharsetArray() > 0)
        pr.AddNewCharset().val = ctFont.GetCharsetArray(0).val;
      if (ctFont.sizeOfCondenseArray() > 0)
        pr.AddNewCondense().val = ctFont.GetCondenseArray(0).val;
      if (ctFont.sizeOfExtendArray() > 0)
        pr.AddNewExtend().val = ctFont.GetExtendArray(0).val;
      if (ctFont.sizeOfVertAlignArray() > 0)
        pr.AddNewVertAlign().val = ctFont.GetVertAlignArray(0).val;
      if (ctFont.sizeOfOutlineArray() > 0)
        pr.AddNewOutline().val = ctFont.GetOutlineArray(0).val;
      if (ctFont.sizeOfShadowArray() > 0)
        pr.AddNewShadow().val = ctFont.GetShadowArray(0).val;
      if (ctFont.sizeOfStrikeArray() <= 0)
        return;
      pr.AddNewStrike().val = ctFont.GetStrikeArray(0).val;
    }

    public void ClearFormatting()
    {
      string str = this.String;
      this.st.r = (List<CT_RElt>) null;
      this.st.t = str;
    }

    public int GetIndexOfFormattingRun(int index)
    {
      if (this.st.sizeOfRArray() == 0)
        return 0;
      int num = 0;
      for (int index1 = 0; index1 < this.st.sizeOfRArray(); ++index1)
      {
        CT_RElt rarray = this.st.GetRArray(index1);
        if (index1 == index)
          return num;
        num += rarray.t.Length;
      }
      return -1;
    }

    public int GetLengthOfFormattingRun(int index)
    {
      if (this.st.sizeOfRArray() == 0)
        return this.Length;
      for (int index1 = 0; index1 < this.st.sizeOfRArray(); ++index1)
      {
        CT_RElt rarray = this.st.GetRArray(index1);
        if (index1 == index)
          return rarray.t.Length;
      }
      return -1;
    }

    public string String
    {
      get
      {
        if (this.st.sizeOfRArray() == 0)
          return XSSFRichTextString.UtfDecode(this.st.t);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (CT_RElt ctRelt in this.st.r)
          stringBuilder.Append(ctRelt.t);
        return XSSFRichTextString.UtfDecode(stringBuilder.ToString());
      }
      set
      {
        this.ClearFormatting();
        this.st.t = value;
        XSSFRichTextString.PreserveSpaces(this.st.t);
      }
    }

    public override string ToString()
    {
      return this.String;
    }

    public int Length
    {
      get
      {
        return this.String.Length;
      }
    }

    public int NumFormattingRuns
    {
      get
      {
        return this.st.sizeOfRArray();
      }
    }

    public IFont GetFontOfFormattingRun(int index)
    {
      if (this.st.sizeOfRArray() == 0)
        return (IFont) null;
      for (int index1 = 0; index1 < this.st.sizeOfRArray(); ++index1)
      {
        CT_RElt rarray = this.st.GetRArray(index1);
        if (index1 == index)
        {
          XSSFFont xssfFont = new XSSFFont(XSSFRichTextString.ToCTFont(rarray.rPr));
          xssfFont.SetThemesTable(this.GetThemesTable());
          return (IFont) xssfFont;
        }
      }
      return (IFont) null;
    }

    public short GetFontAtIndex(int index)
    {
      if (this.st.sizeOfRArray() == 0)
        return -1;
      int num = 0;
      for (int index1 = 0; index1 < this.st.sizeOfRArray(); ++index1)
      {
        CT_RElt rarray = this.st.GetRArray(index1);
        if (index >= num && index < num + rarray.t.Length)
        {
          XSSFFont xssfFont = new XSSFFont(XSSFRichTextString.ToCTFont(rarray.rPr));
          xssfFont.SetThemesTable(this.GetThemesTable());
          return xssfFont.Index;
        }
        num += rarray.t.Length;
      }
      return -1;
    }

    public CT_Rst GetCTRst()
    {
      return this.st;
    }

    protected static CT_Font ToCTFont(CT_RPrElt pr)
    {
      CT_Font ctFont = new CT_Font();
      if (pr.sizeOfBArray() > 0)
        ctFont.AddNewB().val = pr.GetBArray(0).val;
      if (pr.sizeOfUArray() > 0)
        ctFont.AddNewU().val = pr.GetUArray(0).val;
      if (pr.sizeOfIArray() > 0)
        ctFont.AddNewI().val = pr.GetIArray(0).val;
      if (pr.sizeOfColorArray() > 0)
      {
        CT_Color colorArray = pr.GetColorArray(0);
        CT_Color ctColor = ctFont.AddNewColor();
        if (colorArray.IsSetAuto())
        {
          ctColor.auto = colorArray.auto;
          ctColor.autoSpecified = true;
        }
        if (colorArray.IsSetIndexed())
        {
          ctColor.indexed = colorArray.indexed;
          ctColor.indexedSpecified = true;
        }
        if (colorArray.IsSetRgb())
        {
          ctColor.SetRgb(colorArray.GetRgb());
          ctColor.rgbSpecified = true;
        }
        if (colorArray.IsSetTheme())
        {
          ctColor.theme = colorArray.theme;
          ctColor.themeSpecified = true;
        }
        if (colorArray.IsSetTint())
        {
          ctColor.tint = colorArray.tint;
          ctColor.tintSpecified = true;
        }
      }
      if (pr.sizeOfSzArray() > 0)
        ctFont.AddNewSz().val = pr.GetSzArray(0).val;
      if (pr.sizeOfRFontArray() > 0)
        ctFont.AddNewName().val = pr.GetRFontArray(0).val;
      if (pr.sizeOfFamilyArray() > 0)
        ctFont.AddNewFamily().val = pr.GetFamilyArray(0).val;
      if (pr.sizeOfSchemeArray() > 0)
        ctFont.AddNewScheme().val = pr.GetSchemeArray(0).val;
      if (pr.sizeOfCharsetArray() > 0)
        ctFont.AddNewCharset().val = pr.GetCharsetArray(0).val;
      if (pr.sizeOfCondenseArray() > 0)
        ctFont.AddNewCondense().val = pr.GetCondenseArray(0).val;
      if (pr.sizeOfExtendArray() > 0)
        ctFont.AddNewExtend().val = pr.GetExtendArray(0).val;
      if (pr.sizeOfVertAlignArray() > 0)
        ctFont.AddNewVertAlign().val = pr.GetVertAlignArray(0).val;
      if (pr.sizeOfOutlineArray() > 0)
        ctFont.AddNewOutline().val = pr.GetOutlineArray(0).val;
      if (pr.sizeOfShadowArray() > 0)
        ctFont.AddNewShadow().val = pr.GetShadowArray(0).val;
      if (pr.sizeOfStrikeArray() > 0)
        ctFont.AddNewStrike().val = pr.GetStrikeArray(0).val;
      return ctFont;
    }

    protected static void PreserveSpaces(string xs)
    {
      string str = xs;
      if (str == null || str.Length <= 0)
        return;
      char c1 = str[0];
      char c2 = str[str.Length - 1];
      if (char.IsWhiteSpace(c1))
        return;
      char.IsWhiteSpace(c2);
    }

    private static string UtfDecode(string value)
    {
      if (value == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      MatchCollection matchCollection = XSSFRichTextString.utfPtrn.Matches(value);
      int startIndex = 0;
      for (int index1 = 0; index1 < matchCollection.Count; ++index1)
      {
        int index2 = matchCollection[index1].Index;
        if (index2 > startIndex)
          stringBuilder.Append(value.Substring(startIndex, index2 - startIndex));
        int num = int.Parse(matchCollection[index1].Groups[1].Value, NumberStyles.AllowHexSpecifier);
        stringBuilder.Append((char) num);
        startIndex = matchCollection[index1].Index + matchCollection[index1].Length;
      }
      stringBuilder.Append(value.Substring(startIndex));
      return stringBuilder.ToString();
    }

    public int GetLastKey(SortedDictionary<int, CT_RPrElt>.KeyCollection keys)
    {
      int num = 0;
      foreach (int key in keys)
      {
        if (num == keys.Count - 1)
          return key;
        ++num;
      }
      throw new ArgumentOutOfRangeException("GetLastKey failed");
    }

    private CT_Rst buildCTRst(string text, SortedDictionary<int, CT_RPrElt> formats)
    {
      if (text.Length != this.GetLastKey(formats.Keys))
        throw new ArgumentException("Text length was " + (object) text.Length + " but the last format index was " + (object) this.GetLastKey(formats.Keys));
      CT_Rst ctRst = new CT_Rst();
      int startIndex = 0;
      SortedDictionary<int, CT_RPrElt>.KeyCollection.Enumerator enumerator = formats.Keys.GetEnumerator();
      while (enumerator.MoveNext())
      {
        int current = enumerator.Current;
        CT_RElt ctRelt = ctRst.AddNewR();
        string str = text.Substring(startIndex, current - startIndex);
        ctRelt.t = str;
        XSSFRichTextString.PreserveSpaces(ctRelt.t);
        CT_RPrElt format = formats[current];
        if (format != null)
          ctRelt.rPr = format;
        startIndex = current;
      }
      return ctRst;
    }

    private ThemesTable GetThemesTable()
    {
      if (this.styles == null)
        return (ThemesTable) null;
      return this.styles.GetTheme();
    }
  }
}
