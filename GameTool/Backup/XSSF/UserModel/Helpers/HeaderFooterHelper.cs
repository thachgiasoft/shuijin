// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.Helpers.HeaderFooterHelper
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using System.Text;

namespace NPOI.XSSF.UserModel.Helpers
{
  public class HeaderFooterHelper
  {
    private static string HeaderFooterEntity_L = "&L";
    private static string HeaderFooterEntity_C = "&C";
    private static string HeaderFooterEntity_R = "&R";
    public static string HeaderFooterEntity_File = "&F";
    public static string HeaderFooterEntity_Date = "&D";
    public static string HeaderFooterEntity_Time = "&T";

    public string GetLeftSection(string str)
    {
      return this.GetParts(str)[0];
    }

    public string GetCenterSection(string str)
    {
      return this.GetParts(str)[1];
    }

    public string GetRightSection(string str)
    {
      return this.GetParts(str)[2];
    }

    public string SetLeftSection(string str, string newLeft)
    {
      string[] parts = this.GetParts(str);
      parts[0] = newLeft;
      return this.JoinParts(parts);
    }

    public string SetCenterSection(string str, string newCenter)
    {
      string[] parts = this.GetParts(str);
      parts[1] = newCenter;
      return this.JoinParts(parts);
    }

    public string SetRightSection(string str, string newRight)
    {
      string[] parts = this.GetParts(str);
      parts[2] = newRight;
      return this.JoinParts(parts);
    }

    private string[] GetParts(string str)
    {
      string[] strArray = new string[3]{ "", "", "" };
      if (str == null)
        return strArray;
      int length1;
      int length2;
      int length3;
      while ((length3 = str.IndexOf(HeaderFooterHelper.HeaderFooterEntity_L)) > -2 && (length2 = str.IndexOf(HeaderFooterHelper.HeaderFooterEntity_C)) > -2 && (length1 = str.IndexOf(HeaderFooterHelper.HeaderFooterEntity_R)) > -2 && (length3 > -1 || length2 > -1 || length1 > -1))
      {
        if (length1 > length2 && length1 > length3)
        {
          strArray[2] = str.Substring(length1 + HeaderFooterHelper.HeaderFooterEntity_R.Length);
          str = str.Substring(0, length1);
        }
        else if (length2 > length1 && length2 > length3)
        {
          strArray[1] = str.Substring(length2 + HeaderFooterHelper.HeaderFooterEntity_C.Length);
          str = str.Substring(0, length2);
        }
        else
        {
          strArray[0] = str.Substring(length3 + HeaderFooterHelper.HeaderFooterEntity_L.Length);
          str = str.Substring(0, length3);
        }
      }
      return strArray;
    }

    private string JoinParts(string[] parts)
    {
      return this.JoinParts(parts[0], parts[1], parts[2]);
    }

    private string JoinParts(string l, string c, string r)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (c.Length > 0)
      {
        stringBuilder.Append(HeaderFooterHelper.HeaderFooterEntity_C);
        stringBuilder.Append(c);
      }
      if (l.Length > 0)
      {
        stringBuilder.Append(HeaderFooterHelper.HeaderFooterEntity_L);
        stringBuilder.Append(l);
      }
      if (r.Length > 0)
      {
        stringBuilder.Append(HeaderFooterHelper.HeaderFooterEntity_R);
        stringBuilder.Append(r);
      }
      return stringBuilder.ToString();
    }
  }
}
