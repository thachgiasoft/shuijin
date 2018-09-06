// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Model.IndexedUDFFinder
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.Udf;
using System.Collections.Generic;

namespace NPOI.XSSF.Model
{
  public class IndexedUDFFinder : AggregatingUDFFinder
  {
    private Dictionary<int, string> _funcMap;

    public IndexedUDFFinder(params UDFFinder[] usedToolPacks)
      : base(usedToolPacks)
    {
      this._funcMap = new Dictionary<int, string>();
    }

    public override FreeRefFunction FindFunction(string name)
    {
      FreeRefFunction function = base.FindFunction(name);
      if (function != null)
        this._funcMap[this.GetFunctionIndex(name)] = name;
      return function;
    }

    public string GetFunctionName(int idx)
    {
      return this._funcMap[idx];
    }

    public int GetFunctionIndex(string name)
    {
      return name.GetHashCode();
    }
  }
}
