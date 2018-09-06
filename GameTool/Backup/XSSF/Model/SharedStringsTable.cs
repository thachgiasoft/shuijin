// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Model.SharedStringsTable
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NPOI.XSSF.Model
{
  public class SharedStringsTable : POIXMLDocumentPart
  {
    private List<CT_Rst> strings = new List<CT_Rst>();
    private Dictionary<string, int> stmap = new Dictionary<string, int>();
    private int count;
    private int uniqueCount;
    private SstDocument _sstDoc;

    public SharedStringsTable()
    {
      this._sstDoc = new SstDocument();
      this._sstDoc.AddNewSst();
    }

    internal SharedStringsTable(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.ReadFrom(part.GetInputStream());
    }

    public void ReadFrom(Stream is1)
    {
      try
      {
        int num = 0;
        this._sstDoc = SstDocument.Parse(is1);
        CT_Sst sst = this._sstDoc.GetSst();
        this.count = sst.count;
        this.uniqueCount = sst.uniqueCount;
        foreach (CT_Rst st in sst.si)
        {
          string key = this.GetKey(st);
          if (key != null && !this.stmap.ContainsKey(key))
            this.stmap.Add(key, num);
          this.strings.Add(st);
          ++num;
        }
      }
      catch (XmlException ex)
      {
        throw new IOException(ex.Message);
      }
    }

    private string GetKey(CT_Rst st)
    {
      if (st.t != null)
        return st.t;
      return string.Empty;
    }

    public CT_Rst GetEntryAt(int idx)
    {
      return this.strings[idx];
    }

    public int GetCount()
    {
      return this.count;
    }

    public int GetUniqueCount()
    {
      return this.uniqueCount;
    }

    public int AddEntry(CT_Rst st)
    {
      string key = this.GetKey(st);
      ++this.count;
      if (this.stmap.ContainsKey(key))
        return this.stmap[key];
      ++this.uniqueCount;
      CT_Rst ctRst = new CT_Rst();
      this._sstDoc.GetSst().si.Add(ctRst);
      ctRst.Set(st);
      int count = this.strings.Count;
      this.stmap[key] = count;
      this.strings.Add(ctRst);
      return count;
    }

    public List<CT_Rst> GetItems()
    {
      return this.strings;
    }

    public void WriteTo(Stream out1)
    {
      CT_Sst sst = this._sstDoc.GetSst();
      sst.count = this.count;
      sst.countSpecified = true;
      sst.uniqueCount = this.uniqueCount;
      sst.uniqueCountSpecified = true;
      this._sstDoc.Save(out1);
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.WriteTo(outputStream);
      outputStream.Close();
    }
  }
}
