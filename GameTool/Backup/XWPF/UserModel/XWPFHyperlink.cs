﻿// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFHyperlink
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

namespace NPOI.XWPF.UserModel
{
  public class XWPFHyperlink
  {
    private string id;
    private string url;

    public XWPFHyperlink(string id, string url)
    {
      this.id = id;
      this.url = url;
    }

    public string Id
    {
      get
      {
        return this.id;
      }
    }

    public string URL
    {
      get
      {
        return this.url;
      }
    }
  }
}