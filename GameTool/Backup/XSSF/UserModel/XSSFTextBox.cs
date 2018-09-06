// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFTextBox
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Dml.Spreadsheet;
using NPOI.SS.UserModel;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFTextBox : XSSFSimpleShape, ITextbox, IShape
  {
    internal XSSFTextBox(XSSFDrawing drawing, CT_Shape ctShape)
      : base(drawing, ctShape)
    {
    }

    public short HorizontalAlignment
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int MarginBottom
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int MarginLeft
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int MarginRight
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int MarginTop
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public IRichTextString String
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public short VerticalAlignment
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public new int CountOfAllChildren
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public new int FillColor
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public new LineStyle LineStyle
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public new int LineStyleColor
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int LineWidth
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public new void SetLineStyleColor(int lineStyleColor)
    {
      throw new NotImplementedException();
    }
  }
}
