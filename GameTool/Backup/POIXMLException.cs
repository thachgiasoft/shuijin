﻿// Decompiled with JetBrains decompiler
// Type: NPOI.POIXMLException
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using System;

namespace NPOI
{
  public class POIXMLException : Exception
  {
    public POIXMLException()
    {
    }

    public POIXMLException(string msg)
      : base(msg)
    {
    }

    public POIXMLException(string msg, Exception ex)
      : base(msg, ex)
    {
    }

    public POIXMLException(Exception ex)
      : base(string.Empty, ex)
    {
    }
  }
}