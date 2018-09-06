// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Model.CalculationChain
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Collections.Generic;
using System.IO;

namespace NPOI.XSSF.Model
{
  public class CalculationChain : POIXMLDocumentPart
  {
    private CT_CalcChain chain;

    public CalculationChain()
    {
      this.chain = new CT_CalcChain();
    }

    internal CalculationChain(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.ReadFrom(part.GetInputStream());
    }

    public void ReadFrom(Stream is1)
    {
      this.chain = CalcChainDocument.Parse(is1).GetCalcChain();
    }

    public void WriteTo(Stream out1)
    {
      CalcChainDocument calcChainDocument = new CalcChainDocument();
      calcChainDocument.SetCalcChain(this.chain);
      calcChainDocument.Save(out1);
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.WriteTo(outputStream);
      outputStream.Close();
    }

    public CT_CalcChain GetCTCalcChain()
    {
      return this.chain;
    }

    public void RemoveItem(int sheetId, string ref1)
    {
      int num = -1;
      List<CT_CalcCell> c = this.chain.c;
      for (int index = 0; index < c.Count; ++index)
      {
        if (c[index].iSpecified)
          num = c[index].i;
        if (num == sheetId && c[index].r.Equals(ref1))
        {
          if (c[index].iSpecified && index < c.Count - 1 && !c[index + 1].iSpecified)
            c[index + 1].i = num;
          this.chain.RemoveC(index);
          break;
        }
      }
    }
  }
}
