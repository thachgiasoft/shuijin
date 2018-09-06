// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFPrintSetup
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFPrintSetup : IPrintSetup
  {
    private CT_Worksheet ctWorksheet;
    private CT_PageSetup pageSetup;
    private CT_PageMargins pageMargins;

    public XSSFPrintSetup(CT_Worksheet worksheet)
    {
      this.ctWorksheet = worksheet;
      this.pageSetup = !this.ctWorksheet.IsSetPageSetup() ? this.ctWorksheet.AddNewPageSetup() : this.ctWorksheet.pageSetup;
      if (this.ctWorksheet.IsSetPageMargins())
        this.pageMargins = this.ctWorksheet.pageMargins;
      else
        this.pageMargins = this.ctWorksheet.AddNewPageMargins();
    }

    public void SetPaperSize(NPOI.SS.UserModel.PaperSize size)
    {
      this.PaperSize = (short) (size + (short) 1);
    }

    public PrintOrientation Orientation
    {
      get
      {
        ST_Orientation? nullable = new ST_Orientation?(this.pageSetup.orientation);
        if (nullable.HasValue)
          return PrintOrientation.ValueOf((int) nullable.Value);
        return PrintOrientation.DEFAULT;
      }
      set
      {
        this.pageSetup.orientation = (ST_Orientation) value.Value;
      }
    }

    public PrintCellComments GetCellComment()
    {
      ST_CellComments? nullable = new ST_CellComments?(this.pageSetup.cellComments);
      if (nullable.HasValue)
        return PrintCellComments.ValueOf((int) nullable.Value);
      return PrintCellComments.NONE;
    }

    public PageOrder PageOrder
    {
      get
      {
        return PageOrder.ValueOf((int) this.pageSetup.pageOrder);
      }
      set
      {
        this.pageSetup.pageOrder = (ST_PageOrder) value.Value;
      }
    }

    public short PaperSize
    {
      get
      {
        return (short) this.pageSetup.paperSize;
      }
      set
      {
        this.pageSetup.paperSize = (uint) value;
      }
    }

    public NPOI.SS.UserModel.PaperSize GetPaperSizeEnum()
    {
      return (NPOI.SS.UserModel.PaperSize) ((int) this.PaperSize - 1);
    }

    public short Scale
    {
      get
      {
        return (short) this.pageSetup.scale;
      }
      set
      {
        if (value < (short) 10 || value > (short) 400)
          throw new POIXMLException("Scale value not accepted: you must choose a value between 10 and 400.");
        this.pageSetup.scale = (uint) value;
      }
    }

    public short PageStart
    {
      get
      {
        return (short) this.pageSetup.firstPageNumber;
      }
      set
      {
        this.pageSetup.firstPageNumber = (uint) value;
      }
    }

    public short FitWidth
    {
      get
      {
        return (short) this.pageSetup.fitToWidth;
      }
      set
      {
        this.pageSetup.fitToWidth = (uint) value;
      }
    }

    public short FitHeight
    {
      get
      {
        return (short) this.pageSetup.fitToHeight;
      }
      set
      {
        this.pageSetup.fitToHeight = (uint) value;
      }
    }

    public bool LeftToRight
    {
      get
      {
        return this.PageOrder == PageOrder.OVER_THEN_DOWN;
      }
      set
      {
        if (!value)
          return;
        this.PageOrder = PageOrder.OVER_THEN_DOWN;
      }
    }

    public bool Landscape
    {
      get
      {
        return this.Orientation == PrintOrientation.LANDSCAPE;
      }
      set
      {
        if (!value)
          return;
        this.Orientation = PrintOrientation.LANDSCAPE;
      }
    }

    public bool ValidSettings
    {
      get
      {
        return this.pageSetup.usePrinterDefaults;
      }
      set
      {
        this.pageSetup.usePrinterDefaults = value;
      }
    }

    public bool NoColor
    {
      get
      {
        return this.pageSetup.blackAndWhite;
      }
      set
      {
        this.pageSetup.blackAndWhite = value;
      }
    }

    public bool Draft
    {
      get
      {
        return this.pageSetup.draft;
      }
      set
      {
        this.pageSetup.draft = value;
      }
    }

    public bool Notes
    {
      get
      {
        return this.GetCellComment() == PrintCellComments.AS_DISPLAYED;
      }
      set
      {
        if (!value)
          return;
        this.pageSetup.cellComments = ST_CellComments.asDisplayed;
      }
    }

    public bool NoOrientation
    {
      get
      {
        return this.Orientation == PrintOrientation.DEFAULT;
      }
      set
      {
        if (!value)
          return;
        this.Orientation = PrintOrientation.DEFAULT;
      }
    }

    public bool UsePage
    {
      get
      {
        return this.pageSetup.useFirstPageNumber;
      }
      set
      {
        this.pageSetup.useFirstPageNumber = value;
      }
    }

    public short HResolution
    {
      get
      {
        return (short) this.pageSetup.horizontalDpi;
      }
      set
      {
        this.pageSetup.horizontalDpi = (uint) value;
      }
    }

    public short VResolution
    {
      get
      {
        return (short) this.pageSetup.verticalDpi;
      }
      set
      {
        this.pageSetup.verticalDpi = (uint) value;
      }
    }

    public double HeaderMargin
    {
      get
      {
        return this.pageMargins.header;
      }
      set
      {
        this.pageMargins.header = value;
      }
    }

    public double FooterMargin
    {
      get
      {
        return this.pageMargins.footer;
      }
      set
      {
        this.pageMargins.footer = value;
      }
    }

    public short Copies
    {
      get
      {
        return (short) this.pageSetup.copies;
      }
      set
      {
        this.pageSetup.copies = (uint) value;
      }
    }

    public DisplayCellErrorType CellError
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

    public bool EndNote
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
  }
}
