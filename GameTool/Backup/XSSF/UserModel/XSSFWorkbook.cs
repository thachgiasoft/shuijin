// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFWorkbook
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Udf;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.Util;
using NPOI.XSSF.Model;
using NPOI.XSSF.UserModel.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace NPOI.XSSF.UserModel
{
  public class XSSFWorkbook : POIXMLDocument, IWorkbook, IList<XSSFSheet>, ICollection<XSSFSheet>, IEnumerable<XSSFSheet>, IEnumerable
  {
    private static Regex COMMA_PATTERN = new Regex(",");
    public static float DEFAULT_CHARACTER_WIDTH = 7.0017f;
    private static int MAX_SENSITIVE_SHEET_NAME_LEN = 31;
    public static int PICTURE_TYPE_EMF = 2;
    public static int PICTURE_TYPE_WMF = 3;
    public static int PICTURE_TYPE_PICT = 4;
    public static int PICTURE_TYPE_JPEG = 5;
    public static int PICTURE_TYPE_PNG = 6;
    public static int PICTURE_TYPE_DIB = 7;
    public static int PICTURE_TYPE_GIF = 8;
    public static int PICTURE_TYPE_TIFF = 9;
    public static int PICTURE_TYPE_EPS = 10;
    public static int PICTURE_TYPE_BMP = 11;
    public static int PICTURE_TYPE_WPG = 12;
    private static POILogger logger = POILogFactory.GetLogger(typeof (XSSFWorkbook));
    private IndexedUDFFinder _udfFinder = new IndexedUDFFinder(new UDFFinder[1]{ UDFFinder.DEFAULT });
    private MissingCellPolicy _missingCellPolicy = MissingCellPolicy.RETURN_NULL_AND_BLANK;
    private CT_Workbook workbook;
    private List<XSSFSheet> sheets;
    private List<XSSFName> namedRanges;
    private SharedStringsTable sharedStringSource;
    private StylesTable stylesSource;
    private ThemesTable theme;
    private CalculationChain calcChain;
    private MapInfo mapInfo;
    private XSSFDataFormat formatter;
    private List<XSSFPictureData> pictures;
    private XSSFCreationHelper _creationHelper;

    public XSSFWorkbook()
      : base(XSSFWorkbook.newPackage())
    {
      this.OnWorkbookCreate();
    }

    public XSSFWorkbook(OPCPackage pkg)
      : base(pkg)
    {
      this.Load((POIXMLFactory) XSSFFactory.GetInstance());
    }

    public XSSFWorkbook(Stream is1)
      : base(PackageHelper.Open(is1))
    {
      this.Load((POIXMLFactory) XSSFFactory.GetInstance());
    }

    [Obsolete]
    public XSSFWorkbook(string path)
      : this(POIXMLDocument.OpenPackage(path))
    {
    }

    internal override void OnDocumentRead()
    {
      try
      {
        this.workbook = WorkbookDocument.Parse(this.GetPackagePart().GetInputStream()).Workbook;
        Dictionary<string, XSSFSheet> dictionary = new Dictionary<string, XSSFSheet>();
        foreach (POIXMLDocumentPart relation in this.GetRelations())
        {
          if (relation is SharedStringsTable)
            this.sharedStringSource = (SharedStringsTable) relation;
          else if (relation is StylesTable)
            this.stylesSource = (StylesTable) relation;
          else if (relation is ThemesTable)
            this.theme = (ThemesTable) relation;
          else if (relation is CalculationChain)
            this.calcChain = (CalculationChain) relation;
          else if (relation is MapInfo)
            this.mapInfo = (MapInfo) relation;
          else if (relation is XSSFSheet)
            dictionary.Add(relation.GetPackageRelationship().Id, (XSSFSheet) relation);
        }
        if (this.stylesSource != null)
          this.stylesSource.SetTheme(this.theme);
        if (this.sharedStringSource == null)
          this.sharedStringSource = (SharedStringsTable) this.CreateRelationship((POIXMLRelation) XSSFRelation.SHARED_STRINGS, (POIXMLFactory) XSSFFactory.GetInstance());
        this.sheets = new List<XSSFSheet>(dictionary.Count);
        foreach (CT_Sheet ctSheet in this.workbook.sheets.sheet)
        {
          XSSFSheet xssfSheet = dictionary[ctSheet.id];
          if (xssfSheet == null)
          {
            XSSFWorkbook.logger.Log(5, (object) ("Sheet with name " + ctSheet.name + " and r:id " + ctSheet.id + " was defined, but didn't exist in package, skipping"));
          }
          else
          {
            xssfSheet.sheet = ctSheet;
            xssfSheet.OnDocumentRead();
            this.sheets.Add(xssfSheet);
          }
        }
        this.namedRanges = new List<XSSFName>();
        if (!this.workbook.IsSetDefinedNames())
          return;
        foreach (CT_DefinedName name in this.workbook.definedNames.definedName)
          this.namedRanges.Add(new XSSFName(name, this));
      }
      catch (XmlException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
    }

    private void OnWorkbookCreate()
    {
      this.workbook = new CT_Workbook();
      this.workbook.AddNewWorkbookPr().date1904 = false;
      this.workbook.AddNewBookViews().AddNewWorkbookView().activeTab = 0U;
      this.workbook.AddNewSheets();
      CT_ExtendedProperties underlyingProperties = this.GetProperties().GetExtendedProperties().GetUnderlyingProperties();
      underlyingProperties.Application = POIXMLDocument.DOCUMENT_CREATOR;
      underlyingProperties.DocSecurity = 0;
      underlyingProperties.DocSecuritySpecified = true;
      underlyingProperties.ScaleCrop = false;
      underlyingProperties.ScaleCropSpecified = true;
      underlyingProperties.LinksUpToDate = false;
      underlyingProperties.LinksUpToDateSpecified = true;
      underlyingProperties.HyperlinksChanged = false;
      underlyingProperties.HyperlinksChangedSpecified = true;
      underlyingProperties.SharedDoc = false;
      underlyingProperties.SharedDocSpecified = true;
      this.sharedStringSource = (SharedStringsTable) this.CreateRelationship((POIXMLRelation) XSSFRelation.SHARED_STRINGS, (POIXMLFactory) XSSFFactory.GetInstance());
      this.stylesSource = (StylesTable) this.CreateRelationship((POIXMLRelation) XSSFRelation.STYLES, (POIXMLFactory) XSSFFactory.GetInstance());
      this.namedRanges = new List<XSSFName>();
      this.sheets = new List<XSSFSheet>();
    }

    protected static OPCPackage newPackage()
    {
      try
      {
        OPCPackage opcPackage = OPCPackage.Create((Stream) new MemoryStream());
        PackagePartName partName = PackagingUriHelper.CreatePartName(XSSFRelation.WORKBOOK.DefaultFileName);
        opcPackage.AddRelationship(partName, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
        opcPackage.CreatePart(partName, XSSFRelation.WORKBOOK.ContentType);
        opcPackage.GetPackageProperties().SetCreatorProperty(POIXMLDocument.DOCUMENT_CREATOR);
        return opcPackage;
      }
      catch (Exception ex)
      {
        throw new POIXMLException(ex);
      }
    }

    public CT_Workbook GetCTWorkbook()
    {
      return this.workbook;
    }

    public int AddPicture(byte[] pictureData, int format)
    {
      return this.AddPicture(pictureData, (PictureType) format);
    }

    public int AddPicture(Stream picStream, int format)
    {
      int idx = this.GetAllPictures().Count + 1;
      XSSFPictureData relationship = (XSSFPictureData) this.CreateRelationship(XSSFPictureData.RELATIONS[format], (POIXMLFactory) XSSFFactory.GetInstance(), idx, true);
      Stream outputStream = relationship.GetPackagePart().GetOutputStream();
      IOUtils.Copy(picStream, outputStream);
      outputStream.Close();
      this.pictures.Add(relationship);
      return idx - 1;
    }

    public ISheet CloneSheet(int sheetNum)
    {
      this.ValidateSheetIndex(sheetNum);
      XSSFSheet sheet1 = this.sheets[sheetNum];
      XSSFSheet sheet2 = (XSSFSheet) this.CreateSheet(this.GetUniqueSheetName(sheet1.SheetName));
      try
      {
        MemoryStream memoryStream = new MemoryStream();
        sheet1.Write((Stream) memoryStream);
        sheet2.Read((Stream) new MemoryStream(memoryStream.ToArray()));
      }
      catch (IOException ex)
      {
        throw new POIXMLException("Failed to clone sheet", (Exception) ex);
      }
      CT_Worksheet ctWorksheet = sheet2.GetCTWorksheet();
      if (ctWorksheet.IsSetLegacyDrawing())
      {
        XSSFWorkbook.logger.Log(5, (object) "Cloning sheets with comments is not yet supported.");
        ctWorksheet.UnsetLegacyDrawing();
      }
      if (ctWorksheet.IsSetPageSetup())
      {
        XSSFWorkbook.logger.Log(5, (object) "Cloning sheets with page setup is not yet supported.");
        ctWorksheet.UnsetPageSetup();
      }
      sheet2.IsSelected = false;
      List<POIXMLDocumentPart> relations = sheet1.GetRelations();
      XSSFDrawing xssfDrawing = (XSSFDrawing) null;
      foreach (POIXMLDocumentPart part in relations)
      {
        if (part is XSSFDrawing)
        {
          xssfDrawing = (XSSFDrawing) part;
        }
        else
        {
          PackageRelationship packageRelationship = part.GetPackageRelationship();
          sheet2.GetPackagePart().AddRelationship(packageRelationship.TargetUri, packageRelationship.TargetMode.Value, packageRelationship.RelationshipType);
          sheet2.AddRelation(packageRelationship.Id, part);
        }
      }
      if (xssfDrawing != null)
      {
        if (ctWorksheet.IsSetDrawing())
          ctWorksheet.UnsetDrawing();
        (sheet2.CreateDrawingPatriarch() as XSSFDrawing).GetCTDrawing().Set(xssfDrawing.GetCTDrawing());
        foreach (POIXMLDocumentPart relation in (sheet1.CreateDrawingPatriarch() as XSSFDrawing).GetRelations())
        {
          PackageRelationship packageRelationship = relation.GetPackageRelationship();
          (sheet2.CreateDrawingPatriarch() as XSSFDrawing).GetPackagePart().AddRelationship(packageRelationship.TargetUri, packageRelationship.TargetMode.Value, packageRelationship.RelationshipType, packageRelationship.Id);
        }
      }
      return (ISheet) sheet2;
    }

    private string GetUniqueSheetName(string srcName)
    {
      int num = 2;
      string str1 = srcName;
      int length = srcName.LastIndexOf('(');
      if (length > 0 && srcName.EndsWith(")"))
      {
        string str2 = srcName.Substring(length + 1, srcName.Length - length - 2);
        try
        {
          num = int.Parse(str2.Trim());
          ++num;
          str1 = srcName.Substring(0, length).Trim();
        }
        catch (FormatException ex)
        {
        }
      }
      string name;
      do
      {
        string str2 = num++.ToString();
        name = str1.Length + str2.Length + 2 >= 31 ? str1.Substring(0, 31 - str2.Length - 2) + "(" + str2 + ")" : str1 + " (" + str2 + ")";
      }
      while (this.GetSheetIndex(name) != -1);
      return name;
    }

    public ICellStyle CreateCellStyle()
    {
      return (ICellStyle) this.stylesSource.CreateCellStyle();
    }

    public IDataFormat CreateDataFormat()
    {
      if (this.formatter == null)
        this.formatter = new XSSFDataFormat(this.stylesSource);
      return (IDataFormat) this.formatter;
    }

    public IFont CreateFont()
    {
      XSSFFont xssfFont = new XSSFFont();
      xssfFont.RegisterTo(this.stylesSource);
      return (IFont) xssfFont;
    }

    public IName CreateName()
    {
      XSSFName xssfName = new XSSFName(new CT_DefinedName() { name = "" }, this);
      this.namedRanges.Add(xssfName);
      return (IName) xssfName;
    }

    public ISheet CreateSheet()
    {
      string str = "Sheet" + (object) this.sheets.Count;
      int num = 0;
      while (this.GetSheet(str) != null)
      {
        str = "Sheet" + (object) num;
        ++num;
      }
      return this.CreateSheet(str);
    }

    public ISheet CreateSheet(string sheetname)
    {
      if (sheetname == null)
        throw new ArgumentException("sheetName must not be null");
      if (this.ContainsSheet(sheetname, this.sheets.Count))
        throw new ArgumentException("The workbook already contains a sheet of this name");
      if (sheetname.Length > 31)
        sheetname = sheetname.Substring(0, 31);
      WorkbookUtil.ValidateSheetName(sheetname);
      CT_Sheet ctSheet = this.AddSheet(sheetname);
      int idx = 1;
      foreach (XSSFSheet sheet in this.sheets)
        idx = (int) Math.Max((long) (sheet.sheet.sheetId + 1U), (long) idx);
      XSSFSheet relationship = (XSSFSheet) this.CreateRelationship((POIXMLRelation) XSSFRelation.WORKSHEET, (POIXMLFactory) XSSFFactory.GetInstance(), idx);
      relationship.sheet = ctSheet;
      ctSheet.id = relationship.GetPackageRelationship().Id;
      ctSheet.sheetId = (uint) idx;
      if (this.sheets.Count == 0)
        relationship.IsSelected = true;
      this.sheets.Add(relationship);
      return (ISheet) relationship;
    }

    protected XSSFDialogsheet CreateDialogsheet(string sheetname, CT_Dialogsheet dialogsheet)
    {
      return new XSSFDialogsheet((XSSFSheet) this.CreateSheet(sheetname));
    }

    private CT_Sheet AddSheet(string sheetname)
    {
      CT_Sheet ctSheet = this.workbook.sheets.AddNewSheet();
      ctSheet.name = sheetname;
      return ctSheet;
    }

    public IFont FindFont(short boldWeight, short color, short fontHeight, string name, bool italic, bool strikeout, short typeOffset, byte underline)
    {
      return (IFont) this.stylesSource.FindFont(boldWeight, color, fontHeight, name, italic, strikeout, typeOffset, underline);
    }

    public int ActiveSheetIndex
    {
      get
      {
        return (int) this.workbook.bookViews.GetWorkbookViewArray(0).activeTab;
      }
    }

    public IList GetAllPictures()
    {
      if (this.pictures == null)
      {
        List<PackagePart> partsByName = this.Package.GetPartsByName(new Regex("/xl/media/.*?"));
        this.pictures = new List<XSSFPictureData>(partsByName.Count);
        foreach (PackagePart part in partsByName)
          this.pictures.Add(new XSSFPictureData(part, (PackageRelationship) null));
      }
      return (IList) this.pictures;
    }

    public ICellStyle GetCellStyleAt(short idx)
    {
      return (ICellStyle) this.stylesSource.GetStyleAt((int) idx);
    }

    public IFont GetFontAt(short idx)
    {
      return (IFont) this.stylesSource.GetFontAt((int) idx);
    }

    public IName GetName(string name)
    {
      int nameIndex = this.GetNameIndex(name);
      if (nameIndex < 0)
        return (IName) null;
      return (IName) this.namedRanges[nameIndex];
    }

    public IName GetNameAt(int nameIndex)
    {
      int count = this.namedRanges.Count;
      if (count < 1)
        throw new InvalidOperationException("There are no defined names in this workbook");
      if (nameIndex < 0 || nameIndex > count)
        throw new ArgumentException("Specified name index " + (object) nameIndex + " is outside the allowable range (0.." + (object) (count - 1) + ").");
      return (IName) this.namedRanges[nameIndex];
    }

    public int GetNameIndex(string name)
    {
      int num = 0;
      foreach (XSSFName namedRange in this.namedRanges)
      {
        if (namedRange.NameName.Equals(name))
          return num;
        ++num;
      }
      return -1;
    }

    public short NumCellStyles
    {
      get
      {
        return (short) this.stylesSource.GetNumCellStyles();
      }
    }

    public short NumberOfFonts
    {
      get
      {
        return (short) this.stylesSource.GetFonts().Count;
      }
    }

    public int NumberOfNames
    {
      get
      {
        return this.namedRanges.Count;
      }
    }

    public int NumberOfSheets
    {
      get
      {
        return this.sheets.Count;
      }
    }

    public string GetPrintArea(int sheetIndex)
    {
      return this.GetBuiltInName(XSSFName.BUILTIN_PRINT_AREA, sheetIndex)?.RefersToFormula;
    }

    public ISheet GetSheet(string name)
    {
      foreach (XSSFSheet sheet in this.sheets)
      {
        if (name.Equals(sheet.SheetName, StringComparison.InvariantCultureIgnoreCase))
          return (ISheet) sheet;
      }
      return (ISheet) null;
    }

    public ISheet GetSheetAt(int index)
    {
      this.ValidateSheetIndex(index);
      return (ISheet) this.sheets[index];
    }

    public int GetSheetIndex(string name)
    {
      for (int index = 0; index < this.sheets.Count; ++index)
      {
        XSSFSheet sheet = this.sheets[index];
        if (name.Equals(sheet.SheetName, StringComparison.InvariantCultureIgnoreCase))
          return index;
      }
      return -1;
    }

    public int GetSheetIndex(ISheet sheet)
    {
      int num = 0;
      foreach (XSSFSheet sheet1 in this.sheets)
      {
        if (sheet1 == sheet)
          return num;
        ++num;
      }
      return -1;
    }

    public string GetSheetName(int sheetIx)
    {
      this.ValidateSheetIndex(sheetIx);
      return this.sheets[sheetIx].SheetName;
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) this.sheets.GetEnumerator();
    }

    public bool IsMacroEnabled()
    {
      return this.GetPackagePart().ContentType.Equals(XSSFRelation.MACROS_WORKBOOK.ContentType);
    }

    public void RemoveName(int nameIndex)
    {
      this.namedRanges.RemoveAt(nameIndex);
    }

    public void RemoveName(string name)
    {
      for (int nameIndex = 0; nameIndex < this.namedRanges.Count; ++nameIndex)
      {
        if (this.namedRanges[nameIndex].NameName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        {
          this.RemoveName(nameIndex);
          return;
        }
      }
      throw new ArgumentException("Named range was not found: " + name);
    }

    public void RemoveName(XSSFName name)
    {
      if (!this.namedRanges.Remove(name))
        throw new ArgumentException("Name was not found: " + (object) name);
    }

    public void RemovePrintArea(int sheetIndex)
    {
      int index = 0;
      foreach (XSSFName namedRange in this.namedRanges)
      {
        if (namedRange.NameName.Equals(XSSFName.BUILTIN_PRINT_AREA) && namedRange.SheetIndex == sheetIndex)
        {
          this.namedRanges.RemoveAt(index);
          break;
        }
        ++index;
      }
    }

    public void RemoveSheetAt(int index)
    {
      this.ValidateSheetIndex(index);
      this.OnSheetDelete(index);
      this.RemoveRelation((POIXMLDocumentPart) this.GetSheetAt(index));
      this.sheets.RemoveAt(index);
    }

    private void OnSheetDelete(int index)
    {
      this.workbook.sheets.RemoveSheet(index);
      if (this.calcChain != null)
      {
        this.RemoveRelation((POIXMLDocumentPart) this.calcChain);
        this.calcChain = (CalculationChain) null;
      }
      List<XSSFName> xssfNameList = new List<XSSFName>();
      foreach (XSSFName namedRange in this.namedRanges)
      {
        CT_DefinedName ctName = namedRange.GetCTName();
        if (ctName.IsSetLocalSheetId())
        {
          if ((long) ctName.localSheetId == (long) index)
            xssfNameList.Add(namedRange);
          else if ((long) ctName.localSheetId > (long) index)
          {
            --ctName.localSheetId;
            ctName.localSheetIdSpecified = true;
          }
        }
      }
      foreach (XSSFName xssfName in xssfNameList)
        this.namedRanges.Remove(xssfName);
    }

    public MissingCellPolicy MissingCellPolicy
    {
      get
      {
        return this._missingCellPolicy;
      }
      set
      {
        this._missingCellPolicy = value;
      }
    }

    private void ValidateSheetIndex(int index)
    {
      int num = this.sheets.Count - 1;
      if (index < 0 || index > num)
        throw new ArgumentException("Sheet index (" + (object) index + ") is out of range (0.." + (object) num + ")");
    }

    public int FirstVisibleTab
    {
      get
      {
        return (int) this.workbook.bookViews.GetWorkbookViewArray(0).activeTab;
      }
      set
      {
        this.workbook.bookViews.GetWorkbookViewArray(0).activeTab = (uint) value;
      }
    }

    public void SetPrintArea(int sheetIndex, string reference)
    {
      XSSFName xssfName = this.GetBuiltInName(XSSFName.BUILTIN_PRINT_AREA, sheetIndex) ?? this.CreateBuiltInName(XSSFName.BUILTIN_PRINT_AREA, sheetIndex);
      string[] strArray = XSSFWorkbook.COMMA_PATTERN.Split(reference);
      StringBuilder out1 = new StringBuilder(32);
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (index > 0)
          out1.Append(",");
        SheetNameFormatter.AppendFormat(out1, this.GetSheetName(sheetIndex));
        out1.Append("!");
        out1.Append(strArray[index]);
      }
      xssfName.RefersToFormula = out1.ToString();
    }

    public void SetPrintArea(int sheetIndex, int startColumn, int endColumn, int startRow, int endRow)
    {
      string referencePrintArea = XSSFWorkbook.GetReferencePrintArea(this.GetSheetName(sheetIndex), startColumn, endColumn, startRow, endRow);
      this.SetPrintArea(sheetIndex, referencePrintArea);
    }

    [Obsolete("use XSSFSheet#setRepeatingRows(CellRangeAddress) or XSSFSheet#setRepeatingColumns(CellRangeAddress)")]
    public void SetRepeatingRowsAndColumns(int sheetIndex, int startColumn, int endColumn, int startRow, int endRow)
    {
      XSSFSheet sheetAt = (XSSFSheet) this.GetSheetAt(sheetIndex);
      CellRangeAddress cellRangeAddress1 = (CellRangeAddress) null;
      CellRangeAddress cellRangeAddress2 = (CellRangeAddress) null;
      if (startRow != -1)
        cellRangeAddress1 = new CellRangeAddress(startRow, endRow, -1, -1);
      if (startColumn != -1)
        cellRangeAddress2 = new CellRangeAddress(-1, -1, startColumn, endColumn);
      sheetAt.RepeatingRows = cellRangeAddress1;
      sheetAt.RepeatingColumns = cellRangeAddress2;
    }

    private static string GetReferenceBuiltInRecord(string sheetName, int startC, int endC, int startR, int endR)
    {
      CellReference cellReference1 = new CellReference(sheetName, 0, startC, true, true);
      CellReference cellReference2 = new CellReference(sheetName, 0, endC, true, true);
      string str1 = SheetNameFormatter.Format(sheetName);
      string str2;
      if (startC == -1 && endC == -1)
        str2 = "";
      else
        str2 = str1 + "!$" + cellReference1.CellRefParts[2] + ":$" + cellReference2.CellRefParts[2];
      CellReference cellReference3 = new CellReference(sheetName, startR, 0, true, true);
      CellReference cellReference4 = new CellReference(sheetName, endR, 0, true, true);
      string str3 = "";
      if (startR == -1 && endR == -1)
        str3 = "";
      else if (!cellReference3.CellRefParts[1].Equals("0") && !cellReference4.CellRefParts[1].Equals("0"))
        str3 = str1 + "!$" + cellReference3.CellRefParts[1] + ":$" + cellReference4.CellRefParts[1];
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(str2);
      if (stringBuilder.Length > 0 && str3.Length > 0)
        stringBuilder.Append(',');
      stringBuilder.Append(str3);
      return stringBuilder.ToString();
    }

    private static string GetReferencePrintArea(string sheetName, int startC, int endC, int startR, int endR)
    {
      CellReference cellReference1 = new CellReference(sheetName, startR, startC, true, true);
      CellReference cellReference2 = new CellReference(sheetName, endR, endC, true, true);
      return "$" + cellReference1.CellRefParts[2] + "$" + cellReference1.CellRefParts[1] + ":$" + cellReference2.CellRefParts[2] + "$" + cellReference2.CellRefParts[1];
    }

    public XSSFName GetBuiltInName(string builtInCode, int sheetNumber)
    {
      foreach (XSSFName namedRange in this.namedRanges)
      {
        if (namedRange.NameName.Equals(builtInCode, StringComparison.InvariantCultureIgnoreCase) && namedRange.SheetIndex == sheetNumber)
          return namedRange;
      }
      return (XSSFName) null;
    }

    internal XSSFName CreateBuiltInName(string builtInName, int sheetNumber)
    {
      this.ValidateSheetIndex(sheetNumber);
      CT_DefinedName name = (this.workbook.definedNames == null ? this.workbook.AddNewDefinedNames() : this.workbook.definedNames).AddNewDefinedName();
      name.name = builtInName;
      name.localSheetId = (uint) sheetNumber;
      name.localSheetIdSpecified = true;
      XSSFName xssfName = new XSSFName(name, this);
      foreach (object namedRange in this.namedRanges)
      {
        if (namedRange.Equals((object) xssfName))
          throw new POIXMLException("Builtin (" + builtInName + ") already exists for sheet (" + (object) sheetNumber + ")");
      }
      this.namedRanges.Add(xssfName);
      return xssfName;
    }

    public void SetSelectedTab(int index)
    {
      for (int index1 = 0; index1 < this.sheets.Count; ++index1)
        this.sheets[index1].IsSelected = index1 == index;
    }

    public void SetSheetName(int sheetIndex, string sheetname)
    {
      this.ValidateSheetIndex(sheetIndex);
      if (sheetname != null && sheetname.Length > 31)
        sheetname = sheetname.Substring(0, 31);
      WorkbookUtil.ValidateSheetName(sheetname);
      if (this.ContainsSheet(sheetname, sheetIndex))
        throw new ArgumentException("The workbook already contains a sheet of this name");
      new XSSFFormulaUtils(this).UpdateSheetName(sheetIndex, sheetname);
      this.workbook.sheets.GetSheetArray(sheetIndex).name = sheetname;
    }

    public void SetSheetOrder(string sheetname, int pos)
    {
      int sheetIndex = this.GetSheetIndex(sheetname);
      XSSFSheet sheet1 = this.sheets[sheetIndex];
      this.sheets.RemoveAt(sheetIndex);
      this.sheets.Insert(pos, sheet1);
      CT_Sheets sheets = this.workbook.sheets;
      CT_Sheet sheet2 = sheets.GetSheetArray(sheetIndex).Copy();
      this.workbook.sheets.RemoveSheet(sheetIndex);
      sheets.InsertNewSheet(pos).Set(sheet2);
      for (int index = 0; index < this.sheets.Count; ++index)
        this.sheets[index].sheet = sheets.GetSheetArray(index);
    }

    private void SaveNamedRanges()
    {
      if (this.namedRanges.Count > 0)
      {
        CT_DefinedNames definedNames = new CT_DefinedNames();
        List<CT_DefinedName> array = new List<CT_DefinedName>(this.namedRanges.Count);
        foreach (XSSFName namedRange in this.namedRanges)
          array.Add(namedRange.GetCTName());
        definedNames.SetDefinedNameArray(array);
        this.workbook.SetDefinedNames(definedNames);
      }
      else
      {
        if (!this.workbook.IsSetDefinedNames())
          return;
        this.workbook.unsetDefinedNames();
      }
    }

    private void SaveCalculationChain()
    {
      if (this.calcChain == null || this.calcChain.GetCTCalcChain().SizeOfCArray() != 0)
        return;
      this.RemoveRelation((POIXMLDocumentPart) this.calcChain);
      this.calcChain = (CalculationChain) null;
    }

    protected override void Commit()
    {
      this.SaveNamedRanges();
      this.SaveCalculationChain();
      using (Stream outputStream = this.GetPackagePart().GetOutputStream())
        this.workbook.Save(outputStream);
    }

    public SharedStringsTable GetSharedStringSource()
    {
      return this.sharedStringSource;
    }

    public StylesTable GetStylesSource()
    {
      return this.stylesSource;
    }

    public ThemesTable GetTheme()
    {
      return this.theme;
    }

    public ICreationHelper GetCreationHelper()
    {
      if (this._creationHelper == null)
        this._creationHelper = new XSSFCreationHelper(this);
      return (ICreationHelper) this._creationHelper;
    }

    private bool ContainsSheet(string name, int excludeSheetIdx)
    {
      List<CT_Sheet> sheet = this.workbook.sheets.sheet;
      if (name.Length > XSSFWorkbook.MAX_SENSITIVE_SHEET_NAME_LEN)
        name = name.Substring(0, XSSFWorkbook.MAX_SENSITIVE_SHEET_NAME_LEN);
      for (int index = 0; index < sheet.Count; ++index)
      {
        string str = sheet[index].name;
        if (str.Length > XSSFWorkbook.MAX_SENSITIVE_SHEET_NAME_LEN)
          str = str.Substring(0, XSSFWorkbook.MAX_SENSITIVE_SHEET_NAME_LEN);
        if (excludeSheetIdx != index && name.Equals(str, StringComparison.InvariantCultureIgnoreCase))
          return true;
      }
      return false;
    }

    internal bool IsDate1904()
    {
      CT_WorkbookPr workbookPr = this.workbook.workbookPr;
      if (workbookPr.date1904Specified)
        return workbookPr.date1904;
      return false;
    }

    public override List<PackagePart> GetAllEmbedds()
    {
      List<PackagePart> packagePartList = new List<PackagePart>();
      foreach (XSSFSheet sheet in this.sheets)
      {
        foreach (PackageRelationship rel in sheet.GetPackagePart().GetRelationshipsByType(XSSFRelation.OLEEMBEDDINGS.Relation))
          packagePartList.Add(sheet.GetPackagePart().GetRelatedPart(rel));
        foreach (PackageRelationship rel in sheet.GetPackagePart().GetRelationshipsByType(XSSFRelation.PACKEMBEDDINGS.Relation))
          packagePartList.Add(sheet.GetPackagePart().GetRelatedPart(rel));
      }
      return packagePartList;
    }

    public bool IsHidden
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

    public bool IsSheetHidden(int sheetIx)
    {
      this.ValidateSheetIndex(sheetIx);
      return this.sheets[sheetIx].sheet.state == ST_SheetState.hidden;
    }

    public bool IsSheetVeryHidden(int sheetIx)
    {
      this.ValidateSheetIndex(sheetIx);
      return this.sheets[sheetIx].sheet.state == ST_SheetState.veryHidden;
    }

    public void SetSheetHidden(int sheetIx, bool hidden)
    {
      this.SetSheetHidden(sheetIx, hidden ? SheetState.HIDDEN : SheetState.VISIBLE);
    }

    public void SetSheetHidden(int sheetIx, SheetState state)
    {
      this.ValidateSheetIndex(sheetIx);
      WorkbookUtil.ValidateSheetState(state);
      this.sheets[sheetIx].sheet.state = (ST_SheetState) state;
    }

    public void SetSheetHidden(int sheetIx, int hidden)
    {
      this.ValidateSheetIndex(sheetIx);
      this.SetSheetHidden(sheetIx, (SheetState) hidden);
    }

    internal void OnDeleteFormula(XSSFCell cell)
    {
      if (this.calcChain == null)
        return;
      this.calcChain.RemoveItem((int) ((XSSFSheet) cell.Sheet).sheet.sheetId, cell.GetReference());
    }

    public CalculationChain GetCalculationChain()
    {
      return this.calcChain;
    }

    public List<XSSFMap> GetCustomXMLMappings()
    {
      if (this.mapInfo != null)
        return this.mapInfo.GetAllXSSFMaps();
      return new List<XSSFMap>();
    }

    public MapInfo GetMapInfo()
    {
      return this.mapInfo;
    }

    public bool IsStructureLocked()
    {
      if (this.WorkbookProtectionPresent())
        return this.workbook.workbookProtection.lockStructure;
      return false;
    }

    public bool IsWindowsLocked()
    {
      if (this.WorkbookProtectionPresent())
        return this.workbook.workbookProtection.lockWindows;
      return false;
    }

    public bool IsRevisionLocked()
    {
      if (this.WorkbookProtectionPresent())
        return this.workbook.workbookProtection.lockRevision;
      return false;
    }

    public void LockStructure()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.workbook.workbookProtection.lockStructure = true;
    }

    public void UnlockStructure()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.workbook.workbookProtection.lockStructure = false;
    }

    public void LockWindows()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.workbook.workbookProtection.lockWindows = true;
    }

    public void UnlockWindows()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.workbook.workbookProtection.lockWindows = false;
    }

    public void LockRevision()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.workbook.workbookProtection.lockRevision = true;
    }

    public void UnlockRevision()
    {
      this.CreateProtectionFieldIfNotPresent();
      this.workbook.workbookProtection.lockRevision = false;
    }

    private bool WorkbookProtectionPresent()
    {
      return this.workbook.workbookProtection != null;
    }

    private void CreateProtectionFieldIfNotPresent()
    {
      if (this.workbook.workbookProtection != null)
        return;
      this.workbook.workbookProtection = new CT_WorkbookProtection();
    }

    internal UDFFinder GetUDFFinder()
    {
      return (UDFFinder) this._udfFinder;
    }

    public void AddToolPack(UDFFinder toopack)
    {
      this._udfFinder.Add(toopack);
    }

    public void SetForceFormulaRecalculation(bool value)
    {
      CT_Workbook ctWorkbook = this.GetCTWorkbook();
      CT_CalcPr ctCalcPr = ctWorkbook.IsSetCalcPr() ? ctWorkbook.calcPr : ctWorkbook.AddNewCalcPr();
      ctCalcPr.calcId = 0U;
      if (!value || ctCalcPr.calcMode != ST_CalcMode.manual)
        return;
      ctCalcPr.calcMode = ST_CalcMode.auto;
    }

    public bool GetForceFormulaRecalculation()
    {
      CT_CalcPr calcPr = this.GetCTWorkbook().calcPr;
      if (calcPr != null)
        return calcPr.calcId != 0U;
      return false;
    }

    public void SetActiveSheet(int sheetIndex)
    {
      this.ValidateSheetIndex(sheetIndex);
      foreach (CT_BookView ctBookView in this.workbook.bookViews.workbookView)
        ctBookView.activeTab = (uint) sheetIndex;
    }

    public int AddPicture(byte[] pictureData, PictureType format)
    {
      int idx = this.GetAllPictures().Count + 1;
      XSSFPictureData relationship = (XSSFPictureData) this.CreateRelationship(XSSFPictureData.RELATIONS[(int) format], (POIXMLFactory) XSSFFactory.GetInstance(), idx, true);
      try
      {
        Stream outputStream = relationship.GetPackagePart().GetOutputStream();
        outputStream.Write(pictureData, 0, pictureData.Length);
        outputStream.Close();
      }
      catch (IOException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
      this.pictures.Add(relationship);
      return idx - 1;
    }

    public int IndexOf(XSSFSheet item)
    {
      throw new NotImplementedException();
    }

    public void Insert(int index, XSSFSheet item)
    {
      throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
      throw new NotImplementedException();
    }

    public XSSFSheet this[int index]
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

    public void Add(XSSFSheet item)
    {
      throw new NotImplementedException();
    }

    public void Clear()
    {
      throw new NotImplementedException();
    }

    public bool Contains(XSSFSheet item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(XSSFSheet[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public int Count
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool IsReadOnly
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool Remove(XSSFSheet item)
    {
      throw new NotImplementedException();
    }

    IEnumerator<XSSFSheet> IEnumerable<XSSFSheet>.GetEnumerator()
    {
      throw new NotImplementedException();
    }
  }
}
