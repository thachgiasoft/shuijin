// Decompiled with JetBrains decompiler
// Type: NPOI.SS.UserModel.WorkbookFactory
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.HSSF.UserModel;
using NPOI.OpenXml4Net.OPC;
using NPOI.POIFS.FileSystem;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace NPOI.SS.UserModel
{
  public class WorkbookFactory
  {
    public static IWorkbook Create(POIFSFileSystem fs)
    {
      return (IWorkbook) new HSSFWorkbook(fs);
    }

    public static IWorkbook Create(NPOIFSFileSystem fs)
    {
      return (IWorkbook) new HSSFWorkbook(fs.Root, true);
    }

    public static IWorkbook Create(OPCPackage pkg)
    {
      return (IWorkbook) new XSSFWorkbook(pkg);
    }

    public static IWorkbook Create(Stream inputStream)
    {
      inputStream = (Stream) new PushbackStream(inputStream);
      if (POIFSFileSystem.HasPOIFSHeader(inputStream))
        return (IWorkbook) new HSSFWorkbook(inputStream);
      inputStream.Position = 0L;
      if (POIXMLDocument.HasOOXMLHeader(inputStream))
        return (IWorkbook) new XSSFWorkbook(OPCPackage.Open(inputStream));
      throw new ArgumentException("Your InputStream was neither an OLE2 stream, nor an OOXML stream.");
    }

    public static IWorkbook Create(string file)
    {
      if (!File.Exists(file))
        throw new FileNotFoundException(file);
      FileStream channel = (FileStream) null;
      try
      {
        channel = new FileStream(file, FileMode.Open, FileAccess.Read);
        return (IWorkbook) new HSSFWorkbook(new NPOIFSFileSystem(channel).Root, true);
      }
      catch (OfficeXmlFileException ex)
      {
        return (IWorkbook) new XSSFWorkbook(OPCPackage.Open(file));
      }
      finally
      {
        channel?.Close();
      }
    }

    public static IWorkbook Create(Stream inputStream, ImportOption importOption)
    {
      WorkbookFactory.SetImportOption(importOption);
      return WorkbookFactory.Create(inputStream);
    }

    public static IFormulaEvaluator CreateFormulaEvaluator(IWorkbook workbook)
    {
      if (typeof (HSSFWorkbook) == workbook.GetType())
        return (IFormulaEvaluator) new HSSFFormulaEvaluator((IWorkbook) (workbook as HSSFWorkbook));
      return (IFormulaEvaluator) new XSSFFormulaEvaluator(workbook as XSSFWorkbook);
    }

    public static void SetImportOption(ImportOption importOption)
    {
      if (ImportOption.SheetContentOnly == importOption)
      {
        XSSFRelation.AddRelation(XSSFRelation.WORKSHEET);
        XSSFRelation.AddRelation(XSSFRelation.SHARED_STRINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.MACROS_WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.TEMPLATE_WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.MACRO_TEMPLATE_WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.MACRO_ADDIN_WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.CHARTSHEET);
        XSSFRelation.RemoveRelation(XSSFRelation.STYLES);
        XSSFRelation.RemoveRelation(XSSFRelation.DRAWINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.CHART);
        XSSFRelation.RemoveRelation(XSSFRelation.VML_DRAWINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.CUSTOM_XML_MAPPINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.TABLE);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGES);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_EMF);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_WMF);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_PICT);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_JPEG);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_PNG);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_DIB);
        XSSFRelation.RemoveRelation(XSSFRelation.SHEET_COMMENTS);
        XSSFRelation.RemoveRelation(XSSFRelation.SHEET_HYPERLINKS);
        XSSFRelation.RemoveRelation(XSSFRelation.OLEEMBEDDINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.PACKEMBEDDINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.VBA_MACROS);
        XSSFRelation.RemoveRelation(XSSFRelation.ACTIVEX_CONTROLS);
        XSSFRelation.RemoveRelation(XSSFRelation.ACTIVEX_BINS);
        XSSFRelation.RemoveRelation(XSSFRelation.THEME);
        XSSFRelation.RemoveRelation(XSSFRelation.CALC_CHAIN);
        XSSFRelation.RemoveRelation(XSSFRelation.PRINTER_SETTINGS);
      }
      else if (ImportOption.TextOnly == importOption)
      {
        XSSFRelation.AddRelation(XSSFRelation.WORKSHEET);
        XSSFRelation.AddRelation(XSSFRelation.SHARED_STRINGS);
        XSSFRelation.AddRelation(XSSFRelation.SHEET_COMMENTS);
        XSSFRelation.RemoveRelation(XSSFRelation.WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.MACROS_WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.TEMPLATE_WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.MACRO_TEMPLATE_WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.MACRO_ADDIN_WORKBOOK);
        XSSFRelation.RemoveRelation(XSSFRelation.CHARTSHEET);
        XSSFRelation.RemoveRelation(XSSFRelation.STYLES);
        XSSFRelation.RemoveRelation(XSSFRelation.DRAWINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.CHART);
        XSSFRelation.RemoveRelation(XSSFRelation.VML_DRAWINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.CUSTOM_XML_MAPPINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.TABLE);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGES);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_EMF);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_WMF);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_PICT);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_JPEG);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_PNG);
        XSSFRelation.RemoveRelation(XSSFRelation.IMAGE_DIB);
        XSSFRelation.RemoveRelation(XSSFRelation.SHEET_HYPERLINKS);
        XSSFRelation.RemoveRelation(XSSFRelation.OLEEMBEDDINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.PACKEMBEDDINGS);
        XSSFRelation.RemoveRelation(XSSFRelation.VBA_MACROS);
        XSSFRelation.RemoveRelation(XSSFRelation.ACTIVEX_CONTROLS);
        XSSFRelation.RemoveRelation(XSSFRelation.ACTIVEX_BINS);
        XSSFRelation.RemoveRelation(XSSFRelation.THEME);
        XSSFRelation.RemoveRelation(XSSFRelation.CALC_CHAIN);
        XSSFRelation.RemoveRelation(XSSFRelation.PRINTER_SETTINGS);
      }
      else
      {
        XSSFRelation.AddRelation(XSSFRelation.WORKBOOK);
        XSSFRelation.AddRelation(XSSFRelation.MACROS_WORKBOOK);
        XSSFRelation.AddRelation(XSSFRelation.TEMPLATE_WORKBOOK);
        XSSFRelation.AddRelation(XSSFRelation.MACRO_TEMPLATE_WORKBOOK);
        XSSFRelation.AddRelation(XSSFRelation.MACRO_ADDIN_WORKBOOK);
        XSSFRelation.AddRelation(XSSFRelation.WORKSHEET);
        XSSFRelation.AddRelation(XSSFRelation.CHARTSHEET);
        XSSFRelation.AddRelation(XSSFRelation.SHARED_STRINGS);
        XSSFRelation.AddRelation(XSSFRelation.STYLES);
        XSSFRelation.AddRelation(XSSFRelation.DRAWINGS);
        XSSFRelation.AddRelation(XSSFRelation.CHART);
        XSSFRelation.AddRelation(XSSFRelation.VML_DRAWINGS);
        XSSFRelation.AddRelation(XSSFRelation.CUSTOM_XML_MAPPINGS);
        XSSFRelation.AddRelation(XSSFRelation.TABLE);
        XSSFRelation.AddRelation(XSSFRelation.IMAGES);
        XSSFRelation.AddRelation(XSSFRelation.IMAGE_EMF);
        XSSFRelation.AddRelation(XSSFRelation.IMAGE_WMF);
        XSSFRelation.AddRelation(XSSFRelation.IMAGE_PICT);
        XSSFRelation.AddRelation(XSSFRelation.IMAGE_JPEG);
        XSSFRelation.AddRelation(XSSFRelation.IMAGE_PNG);
        XSSFRelation.AddRelation(XSSFRelation.IMAGE_DIB);
        XSSFRelation.AddRelation(XSSFRelation.SHEET_COMMENTS);
        XSSFRelation.AddRelation(XSSFRelation.SHEET_HYPERLINKS);
        XSSFRelation.AddRelation(XSSFRelation.OLEEMBEDDINGS);
        XSSFRelation.AddRelation(XSSFRelation.PACKEMBEDDINGS);
        XSSFRelation.AddRelation(XSSFRelation.VBA_MACROS);
        XSSFRelation.AddRelation(XSSFRelation.ACTIVEX_CONTROLS);
        XSSFRelation.AddRelation(XSSFRelation.ACTIVEX_BINS);
        XSSFRelation.AddRelation(XSSFRelation.THEME);
        XSSFRelation.AddRelation(XSSFRelation.CALC_CHAIN);
        XSSFRelation.AddRelation(XSSFRelation.PRINTER_SETTINGS);
      }
    }
  }
}
