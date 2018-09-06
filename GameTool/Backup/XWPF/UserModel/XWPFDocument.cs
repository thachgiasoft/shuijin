// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFDocument
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.Util;
using NPOI.XWPF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XWPF.UserModel
{
  public class XWPFDocument : POIXMLDocument, NPOI.XWPF.UserModel.Document, IBody
  {
    private IdentifierManager DrawingIdManager = new IdentifierManager(1L, (long) uint.MaxValue);
    protected List<XWPFFooter> footers = new List<XWPFFooter>();
    protected List<XWPFHeader> headers = new List<XWPFHeader>();
    protected List<XWPFComment> comments = new List<XWPFComment>();
    protected List<XWPFHyperlink> hyperlinks = new List<XWPFHyperlink>();
    protected List<XWPFParagraph> paragraphs = new List<XWPFParagraph>();
    protected List<XWPFTable> tables = new List<XWPFTable>();
    protected List<IBodyElement> bodyElements = new List<IBodyElement>();
    protected List<XWPFPictureData> pictures = new List<XWPFPictureData>();
    protected Dictionary<long, List<XWPFPictureData>> packagePictures = new Dictionary<long, List<XWPFPictureData>>();
    protected Dictionary<int, XWPFFootnote> endnotes = new Dictionary<int, XWPFFootnote>();
    private CT_Document ctDocument;
    private XWPFSettings Settings;
    protected XWPFNumbering numbering;
    protected XWPFStyles styles;
    protected XWPFFootnotes footnotes;
    private XWPFHeaderFooterPolicy headerFooterPolicy;

    public XWPFDocument(OPCPackage pkg)
      : base(pkg)
    {
      this.Load((POIXMLFactory) XWPFFactory.GetInstance());
    }

    public XWPFDocument(Stream is1)
      : base(PackageHelper.Open(is1))
    {
      this.Load((POIXMLFactory) XWPFFactory.GetInstance());
    }

    public XWPFDocument()
      : base(XWPFDocument.newPackage())
    {
      this.OnDocumentCreate();
    }

    internal override void OnDocumentRead()
    {
      try
      {
        DocumentDocument documentDocument = DocumentDocument.Parse(this.GetPackagePart().GetInputStream());
        this.ctDocument = documentDocument.Document;
        this.InitFootnotes();
        foreach (object obj in this.ctDocument.body.Items)
        {
          if (obj is CT_P)
          {
            XWPFParagraph xwpfParagraph = new XWPFParagraph((CT_P) obj, (IBody) this);
            this.bodyElements.Add((IBodyElement) xwpfParagraph);
            this.paragraphs.Add(xwpfParagraph);
          }
          else if (obj is CT_Tbl)
          {
            XWPFTable xwpfTable = new XWPFTable((CT_Tbl) obj, (IBody) this);
            this.bodyElements.Add((IBodyElement) xwpfTable);
            this.tables.Add(xwpfTable);
          }
        }
        if (documentDocument.Document.body.sectPr != null)
          this.headerFooterPolicy = new XWPFHeaderFooterPolicy(this);
        foreach (POIXMLDocumentPart relation1 in this.GetRelations())
        {
          string relationshipType = relation1.GetPackageRelationship().RelationshipType;
          if (relationshipType.Equals(XWPFRelation.STYLES.Relation))
          {
            this.styles = (XWPFStyles) relation1;
            this.styles.OnDocumentRead();
          }
          else if (relationshipType.Equals(XWPFRelation.NUMBERING.Relation))
          {
            this.numbering = (XWPFNumbering) relation1;
            this.numbering.OnDocumentRead();
          }
          else if (relationshipType.Equals(XWPFRelation.FOOTER.Relation))
          {
            XWPFFooter xwpfFooter = (XWPFFooter) relation1;
            this.footers.Add(xwpfFooter);
            xwpfFooter.OnDocumentRead();
          }
          else if (relationshipType.Equals(XWPFRelation.HEADER.Relation))
          {
            XWPFHeader xwpfHeader = (XWPFHeader) relation1;
            this.headers.Add(xwpfHeader);
            xwpfHeader.OnDocumentRead();
          }
          else if (relationshipType.Equals(XWPFRelation.COMMENT.Relation))
          {
            foreach (CT_Comment comment in CommentsDocument.Parse(relation1.GetPackagePart().GetInputStream()).Comments.comment)
              this.comments.Add(new XWPFComment(comment, this));
          }
          else if (relationshipType.Equals(XWPFRelation.SETTINGS.Relation))
          {
            this.Settings = (XWPFSettings) relation1;
            this.Settings.OnDocumentRead();
          }
          else if (relationshipType.Equals(XWPFRelation.IMAGES.Relation))
          {
            XWPFPictureData picData = (XWPFPictureData) relation1;
            picData.OnDocumentRead();
            this.RegisterPackagePictureData(picData);
            this.pictures.Add(picData);
          }
          else if (relationshipType.Equals(XWPFRelation.GLOSSARY_DOCUMENT.Relation))
          {
            foreach (POIXMLDocumentPart relation2 in relation1.GetRelations())
            {
              try
              {
                relation2.OnDocumentRead();
              }
              catch (Exception ex)
              {
                throw new POIXMLException(ex);
              }
            }
          }
        }
        this.InitHyperlinks();
      }
      catch (XmlException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
    }

    private void InitHyperlinks()
    {
      try
      {
        IEnumerator<PackageRelationship> enumerator = this.GetPackagePart().GetRelationshipsByType(XWPFRelation.HYPERLINK.Relation).GetEnumerator();
        while (enumerator.MoveNext())
        {
          PackageRelationship current = enumerator.Current;
          this.hyperlinks.Add(new XWPFHyperlink(current.Id, current.TargetUri.ToString()));
        }
      }
      catch (InvalidDataException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
    }

    private void InitFootnotes()
    {
      foreach (POIXMLDocumentPart relation in this.GetRelations())
      {
        string relationshipType = relation.GetPackageRelationship().RelationshipType;
        if (relationshipType.Equals(XWPFRelation.FOOTNOTE.Relation))
        {
          FootnotesDocument footnotesDocument = FootnotesDocument.Parse(relation.GetPackagePart().GetInputStream());
          this.footnotes = (XWPFFootnotes) relation;
          this.footnotes.OnDocumentRead();
          foreach (CT_FtnEdn note in footnotesDocument.Footnotes.footnote)
            this.footnotes.AddFootnote(note);
        }
        else if (relationshipType.Equals(XWPFRelation.ENDNOTE.Relation))
        {
          foreach (CT_FtnEdn body in EndnotesDocument.Parse(relation.GetPackagePart().GetInputStream()).Endnotes.endnote)
            this.endnotes.Add(int.Parse(body.id), new XWPFFootnote(this, body));
        }
      }
    }

    protected static OPCPackage newPackage()
    {
      try
      {
        OPCPackage opcPackage = OPCPackage.Create((Stream) new MemoryStream());
        PackagePartName partName = PackagingUriHelper.CreatePartName(XWPFRelation.DOCUMENT.DefaultFileName);
        opcPackage.AddRelationship(partName, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
        opcPackage.CreatePart(partName, XWPFRelation.DOCUMENT.ContentType);
        opcPackage.GetPackageProperties().SetCreatorProperty(POIXMLDocument.DOCUMENT_CREATOR);
        return opcPackage;
      }
      catch (Exception ex)
      {
        throw new POIXMLException(ex);
      }
    }

    internal override void OnDocumentCreate()
    {
      this.ctDocument = new CT_Document();
      this.ctDocument.AddNewBody();
      this.Settings = (XWPFSettings) this.CreateRelationship((POIXMLRelation) XWPFRelation.SETTINGS, (POIXMLFactory) XWPFFactory.GetInstance());
      this.CreateStyles();
      this.GetProperties().GetExtendedProperties().GetUnderlyingProperties().Application = POIXMLDocument.DOCUMENT_CREATOR;
    }

    public CT_Document Document
    {
      get
      {
        return this.ctDocument;
      }
    }

    internal IdentifierManager GetDrawingIdManager()
    {
      return this.DrawingIdManager;
    }

    public IList<IBodyElement> BodyElements
    {
      get
      {
        return (IList<IBodyElement>) this.bodyElements.AsReadOnly();
      }
    }

    public IEnumerator<IBodyElement> GetBodyElementsIterator()
    {
      return (IEnumerator<IBodyElement>) this.bodyElements.GetEnumerator();
    }

    public IList<XWPFParagraph> Paragraphs
    {
      get
      {
        return (IList<XWPFParagraph>) this.paragraphs.AsReadOnly();
      }
    }

    public IList<XWPFTable> GetTables()
    {
      return (IList<XWPFTable>) this.tables.AsReadOnly();
    }

    public XWPFTable GetTableArray(int pos)
    {
      if (pos > 0 && pos < this.tables.Count)
        return this.tables[pos];
      return (XWPFTable) null;
    }

    public IList<XWPFFooter> GetFooterList()
    {
      return (IList<XWPFFooter>) this.footers.AsReadOnly();
    }

    public XWPFFooter GetFooterArray(int pos)
    {
      return this.footers[pos];
    }

    public IList<XWPFHeader> HeaderList
    {
      get
      {
        return (IList<XWPFHeader>) this.headers.AsReadOnly();
      }
    }

    public XWPFHeader GetHeaderArray(int pos)
    {
      return this.headers[pos];
    }

    public string GetTblStyle(XWPFTable table)
    {
      return table.GetStyleID();
    }

    public XWPFHyperlink GetHyperlinkByID(string id)
    {
      IEnumerator<XWPFHyperlink> enumerator = (IEnumerator<XWPFHyperlink>) this.hyperlinks.GetEnumerator();
      while (enumerator.MoveNext())
      {
        XWPFHyperlink current = enumerator.Current;
        if (current.Id.Equals(id))
          return current;
      }
      return (XWPFHyperlink) null;
    }

    public XWPFFootnote GetFootnoteByID(int id)
    {
      if (this.footnotes == null)
        return (XWPFFootnote) null;
      return this.footnotes.GetFootnoteById(id);
    }

    public XWPFFootnote GetEndnoteByID(int id)
    {
      if (this.endnotes == null)
        return (XWPFFootnote) null;
      return this.endnotes[id];
    }

    public List<XWPFFootnote> GetFootnotes()
    {
      if (this.footnotes == null)
        return new List<XWPFFootnote>();
      return this.footnotes.GetFootnotesList();
    }

    public XWPFHyperlink[] GetHyperlinks()
    {
      return this.hyperlinks.ToArray();
    }

    public XWPFComment GetCommentByID(string id)
    {
      IEnumerator<XWPFComment> enumerator = (IEnumerator<XWPFComment>) this.comments.GetEnumerator();
      while (enumerator.MoveNext())
      {
        XWPFComment current = enumerator.Current;
        if (current.GetId().Equals(id))
          return current;
      }
      return (XWPFComment) null;
    }

    public XWPFComment[] GetComments()
    {
      return this.comments.ToArray();
    }

    public PackagePart GetPartById(string id)
    {
      try
      {
        PackagePart corePart = this.CorePart;
        return corePart.GetRelatedPart(corePart.GetRelationship(id));
      }
      catch (Exception ex)
      {
        throw new ArgumentException("GetTargetPart exception", ex);
      }
    }

    public XWPFHeaderFooterPolicy GetHeaderFooterPolicy()
    {
      return this.headerFooterPolicy;
    }

    public CT_Styles GetStyle()
    {
      PackagePart[] relatedByType;
      try
      {
        relatedByType = this.GetRelatedByType(XWPFRelation.STYLES.Relation);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("get Style document part exception", ex);
      }
      if (relatedByType.Length != 1)
        throw new InvalidOperationException("Expecting one Styles document part, but found " + (object) relatedByType.Length);
      return StylesDocument.Parse(relatedByType[0].GetInputStream()).Styles;
    }

    public override List<PackagePart> GetAllEmbedds()
    {
      List<PackagePart> packagePartList = new List<PackagePart>();
      PackagePart packagePart = this.GetPackagePart();
      foreach (PackageRelationship rel in this.GetPackagePart().GetRelationshipsByType(POIXMLDocument.OLE_OBJECT_REL_TYPE))
        packagePartList.Add(packagePart.GetRelatedPart(rel));
      foreach (PackageRelationship rel in this.GetPackagePart().GetRelationshipsByType(POIXMLDocument.PACK_OBJECT_REL_TYPE))
        packagePartList.Add(packagePart.GetRelatedPart(rel));
      return packagePartList;
    }

    private int GetBodyElementSpecificPos(int pos, List<IBodyElement> list)
    {
      if (list.Count == 0 || pos < 0 || pos >= this.bodyElements.Count)
        return -1;
      IBodyElement bodyElement = this.bodyElements[pos];
      if (bodyElement.ElementType != list[0].ElementType)
        return -1;
      for (int index = Math.Min(pos, list.Count - 1); index >= 0; --index)
      {
        if (list[index] == bodyElement)
          return index;
      }
      return -1;
    }

    public int GetParagraphPos(int pos)
    {
      List<IBodyElement> list = new List<IBodyElement>();
      foreach (IBodyElement paragraph in this.paragraphs)
        list.Add(paragraph);
      return this.GetBodyElementSpecificPos(pos, list);
    }

    public int GetTablePos(int pos)
    {
      List<IBodyElement> list = new List<IBodyElement>();
      foreach (IBodyElement table in this.tables)
        list.Add(table);
      return this.GetBodyElementSpecificPos(pos, list);
    }

    public XWPFParagraph insertNewParagraph(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    public XWPFTable insertNewTbl(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    private bool IsCursorInBody(XmlDocument cursor)
    {
      throw new NotImplementedException();
    }

    private int GetPosOfBodyElement(IBodyElement needle)
    {
      BodyElementType elementType = needle.ElementType;
      for (int index = 0; index < this.bodyElements.Count; ++index)
      {
        IBodyElement bodyElement = this.bodyElements[index];
        if (bodyElement.ElementType == elementType && bodyElement.Equals((object) needle))
          return index;
      }
      return -1;
    }

    public int GetPosOfParagraph(XWPFParagraph p)
    {
      return this.GetPosOfBodyElement((IBodyElement) p);
    }

    public int GetPosOfTable(XWPFTable t)
    {
      return this.GetPosOfBodyElement((IBodyElement) t);
    }

    protected override void Commit()
    {
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[9]{ new XmlQualifiedName("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main"), new XmlQualifiedName("m", "http://schemas.openxmlformats.org/officeDocument/2006/math"), new XmlQualifiedName("o", "urn:schemas-microsoft-com:office:office"), new XmlQualifiedName("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"), new XmlQualifiedName("v", "urn:schemas-microsoft-com:vml"), new XmlQualifiedName("ve", "http://schemas.openxmlformats.org/markup-compatibility/2006"), new XmlQualifiedName("w10", "urn:schemas-microsoft-com:office:word"), new XmlQualifiedName("wne", "http://schemas.microsoft.com/office/word/2006/wordml"), new XmlQualifiedName("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing") });
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      new DocumentDocument(this.ctDocument).Save(outputStream, namespaces);
      outputStream.Close();
    }

    private int GetRelationIndex(XWPFRelation relation)
    {
      List<POIXMLDocumentPart> relations = this.GetRelations();
      int num = 1;
      IEnumerator<POIXMLDocumentPart> enumerator = (IEnumerator<POIXMLDocumentPart>) relations.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (enumerator.Current.GetPackageRelationship().RelationshipType.Equals(relation.Relation))
          ++num;
      }
      return num;
    }

    public XWPFParagraph CreateParagraph()
    {
      XWPFParagraph xwpfParagraph = new XWPFParagraph(this.ctDocument.body.AddNewP(), (IBody) this);
      this.bodyElements.Add((IBodyElement) xwpfParagraph);
      this.paragraphs.Add(xwpfParagraph);
      return xwpfParagraph;
    }

    public XWPFNumbering CreateNumbering()
    {
      if (this.numbering == null)
      {
        NumberingDocument numberingDocument = new NumberingDocument();
        XWPFRelation numbering = XWPFRelation.NUMBERING;
        int relationIndex = this.GetRelationIndex(numbering);
        XWPFNumbering relationship = (XWPFNumbering) this.CreateRelationship((POIXMLRelation) numbering, (POIXMLFactory) XWPFFactory.GetInstance(), relationIndex);
        relationship.SetNumbering(numberingDocument.Numbering);
        this.numbering = relationship;
      }
      return this.numbering;
    }

    public XWPFStyles CreateStyles()
    {
      if (this.styles == null)
      {
        StylesDocument stylesDocument = new StylesDocument();
        XWPFRelation styles = XWPFRelation.STYLES;
        int relationIndex = this.GetRelationIndex(styles);
        XWPFStyles relationship = (XWPFStyles) this.CreateRelationship((POIXMLRelation) styles, (POIXMLFactory) XWPFFactory.GetInstance(), relationIndex);
        relationship.SetStyles(stylesDocument.Styles);
        this.styles = relationship;
      }
      return this.styles;
    }

    public XWPFFootnotes CreateFootnotes()
    {
      if (this.footnotes == null)
      {
        FootnotesDocument footnotesDocument = new FootnotesDocument();
        XWPFRelation footnote = XWPFRelation.FOOTNOTE;
        int relationIndex = this.GetRelationIndex(footnote);
        XWPFFootnotes relationship = (XWPFFootnotes) this.CreateRelationship((POIXMLRelation) footnote, (POIXMLFactory) XWPFFactory.GetInstance(), relationIndex);
        relationship.SetFootnotes(footnotesDocument.Footnotes);
        this.footnotes = relationship;
      }
      return this.footnotes;
    }

    public XWPFFootnote AddFootnote(CT_FtnEdn note)
    {
      return this.footnotes.AddFootnote(note);
    }

    public XWPFFootnote AddEndnote(CT_FtnEdn note)
    {
      XWPFFootnote xwpfFootnote = new XWPFFootnote(this, note);
      this.endnotes.Add(int.Parse(note.id), xwpfFootnote);
      return xwpfFootnote;
    }

    public bool RemoveBodyElement(int pos)
    {
      if (pos < 0 || pos >= this.bodyElements.Count)
        return false;
      BodyElementType elementType = this.bodyElements[pos].ElementType;
      if (elementType == BodyElementType.TABLE)
      {
        int tablePos = this.GetTablePos(pos);
        this.tables.RemoveAt(tablePos);
        this.ctDocument.body.RemoveTbl(tablePos);
      }
      if (elementType == BodyElementType.PARAGRAPH)
      {
        int paragraphPos = this.GetParagraphPos(pos);
        this.paragraphs.RemoveAt(paragraphPos);
        this.ctDocument.body.RemoveP(paragraphPos);
      }
      this.bodyElements.RemoveAt(pos);
      return true;
    }

    public void SetParagraph(XWPFParagraph paragraph, int pos)
    {
      this.paragraphs[pos] = paragraph;
      this.ctDocument.body.SetPArray(pos, paragraph.GetCTP());
    }

    public XWPFParagraph GetLastParagraph()
    {
      return this.paragraphs[this.paragraphs.ToArray().Length - 1];
    }

    public XWPFTable CreateTable()
    {
      XWPFTable xwpfTable = new XWPFTable(this.ctDocument.body.AddNewTbl(), (IBody) this);
      this.bodyElements.Add((IBodyElement) xwpfTable);
      this.tables.Add(xwpfTable);
      return xwpfTable;
    }

    public XWPFTable CreateTable(int rows, int cols)
    {
      XWPFTable xwpfTable = new XWPFTable(this.ctDocument.body.AddNewTbl(), (IBody) this, rows, cols);
      this.bodyElements.Add((IBodyElement) xwpfTable);
      this.tables.Add(xwpfTable);
      return xwpfTable;
    }

    public void CreateTOC()
    {
      TOC toc = new TOC(this.Document.body.AddNewSdt());
      foreach (XWPFParagraph paragraph in this.paragraphs)
      {
        string style = paragraph.GetStyle();
        if (style != null)
        {
          if (style.Substring(0, 7).Equals("Heading"))
          {
            try
            {
              int level = int.Parse(style.Substring("Heading".Length));
              toc.AddRow(level, paragraph.GetText(), 1, "112723803");
            }
            catch (FormatException ex)
            {
              Console.Write(ex.StackTrace);
            }
          }
        }
      }
    }

    public void SetTable(int pos, XWPFTable table)
    {
      this.tables[pos] = table;
      this.ctDocument.body.SetTblArray(pos, table.GetCTTbl());
    }

    public bool IsEnforcedReadonlyProtection()
    {
      return this.Settings.IsEnforcedWith(ST_DocProtect.readOnly);
    }

    public bool IsEnforcedFillingFormsProtection()
    {
      return this.Settings.IsEnforcedWith(ST_DocProtect.forms);
    }

    public bool IsEnforcedCommentsProtection()
    {
      return this.Settings.IsEnforcedWith(ST_DocProtect.comments);
    }

    public bool IsEnforcedTrackedChangesProtection()
    {
      return this.Settings.IsEnforcedWith(ST_DocProtect.trackedChanges);
    }

    public bool IsEnforcedUpdateFields()
    {
      return this.Settings.IsUpdateFields();
    }

    public void EnforceReadonlyProtection()
    {
      this.Settings.SetEnforcementEditValue(ST_DocProtect.readOnly);
    }

    public void EnforceFillingFormsProtection()
    {
      this.Settings.SetEnforcementEditValue(ST_DocProtect.forms);
    }

    public void EnforceCommentsProtection()
    {
      this.Settings.SetEnforcementEditValue(ST_DocProtect.comments);
    }

    public void EnforceTrackedChangesProtection()
    {
      this.Settings.SetEnforcementEditValue(ST_DocProtect.trackedChanges);
    }

    public void RemoveProtectionEnforcement()
    {
      this.Settings.RemoveEnforcement();
    }

    public void EnforceUpdateFields()
    {
      this.Settings.SetUpdateFields();
    }

    public void insertTable(int pos, XWPFTable table)
    {
      this.bodyElements.Insert(pos, (IBodyElement) table);
      CT_Tbl[] tblArray = this.ctDocument.body.GetTblArray();
      int index = 0;
      while (index < tblArray.Length && tblArray[index] != table.GetCTTbl())
        ++index;
      this.tables.Insert(index, table);
    }

    public IList<XWPFPictureData> AllPictures
    {
      get
      {
        return (IList<XWPFPictureData>) this.pictures.AsReadOnly();
      }
    }

    public IList<XWPFPictureData> AllPackagePictures
    {
      get
      {
        List<XWPFPictureData> xwpfPictureDataList1 = new List<XWPFPictureData>();
        foreach (List<XWPFPictureData> xwpfPictureDataList2 in this.packagePictures.Values)
          xwpfPictureDataList1.AddRange((IEnumerable<XWPFPictureData>) xwpfPictureDataList2);
        return (IList<XWPFPictureData>) xwpfPictureDataList1.AsReadOnly();
      }
    }

    public void RegisterPackagePictureData(XWPFPictureData picData)
    {
      List<XWPFPictureData> xwpfPictureDataList = (List<XWPFPictureData>) null;
      if (this.packagePictures.ContainsKey(picData.Checksum))
        xwpfPictureDataList = this.packagePictures[picData.Checksum];
      if (xwpfPictureDataList == null)
      {
        xwpfPictureDataList = new List<XWPFPictureData>(1);
        this.packagePictures.Add(picData.Checksum, xwpfPictureDataList);
      }
      if (xwpfPictureDataList.Contains(picData))
        return;
      xwpfPictureDataList.Add(picData);
    }

    public XWPFPictureData FindPackagePictureData(byte[] pictureData, int format)
    {
      long checksum = IOUtils.CalculateChecksum(pictureData);
      XWPFPictureData xwpfPictureData = (XWPFPictureData) null;
      List<XWPFPictureData> xwpfPictureDataList = (List<XWPFPictureData>) null;
      if (this.packagePictures.ContainsKey(checksum))
        xwpfPictureDataList = this.packagePictures[checksum];
      if (xwpfPictureDataList != null)
      {
        IEnumerator<XWPFPictureData> enumerator = (IEnumerator<XWPFPictureData>) xwpfPictureDataList.GetEnumerator();
        while (enumerator.MoveNext() && xwpfPictureData == null)
        {
          XWPFPictureData current = enumerator.Current;
          if (Arrays.Equals((object) pictureData, (object) current.GetData()))
            xwpfPictureData = current;
        }
      }
      return xwpfPictureData;
    }

    public string AddPictureData(byte[] pictureData, int format)
    {
      XWPFPictureData packagePictureData = this.FindPackagePictureData(pictureData, format);
      POIXMLRelation descriptor = XWPFPictureData.RELATIONS[format];
      if (packagePictureData == null)
      {
        int nextPicNameNumber = this.GetNextPicNameNumber(format);
        XWPFPictureData relationship = (XWPFPictureData) this.CreateRelationship(descriptor, (POIXMLFactory) XWPFFactory.GetInstance(), nextPicNameNumber);
        PackagePart packagePart = relationship.GetPackagePart();
        Stream stream = (Stream) null;
        try
        {
          stream = packagePart.GetOutputStream();
          stream.Write(pictureData, 0, pictureData.Length);
        }
        catch (IOException ex)
        {
          throw new POIXMLException((Exception) ex);
        }
        finally
        {
          try
          {
            stream.Close();
          }
          catch (IOException ex)
          {
          }
        }
        this.RegisterPackagePictureData(relationship);
        this.pictures.Add(relationship);
        return this.GetRelationId((POIXMLDocumentPart) relationship);
      }
      if (this.GetRelations().Contains((POIXMLDocumentPart) packagePictureData))
        return this.GetRelationId((POIXMLDocumentPart) packagePictureData);
      PackagePart packagePart1 = packagePictureData.GetPackagePart();
      TargetMode targetMode = TargetMode.Internal;
      PackagePartName partName = packagePart1.PartName;
      string relation = descriptor.Relation;
      string id = this.GetPackagePart().AddRelationship(partName, targetMode, relation).Id;
      this.AddRelation(id, (POIXMLDocumentPart) packagePictureData);
      this.pictures.Add(packagePictureData);
      return id;
    }

    public string AddPictureData(Stream is1, int format)
    {
      try
      {
        return this.AddPictureData(IOUtils.ToByteArray(is1), format);
      }
      catch (IOException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
    }

    public int GetNextPicNameNumber(int format)
    {
      int index = this.AllPackagePictures.Count + 1;
      for (PackagePartName partName = PackagingUriHelper.CreatePartName(XWPFPictureData.RELATIONS[format].GetFileName(index)); this.Package.GetPart(partName) != null; partName = PackagingUriHelper.CreatePartName(XWPFPictureData.RELATIONS[format].GetFileName(index)))
        ++index;
      return index;
    }

    public XWPFPictureData GetPictureDataByID(string blipID)
    {
      POIXMLDocumentPart relationById = this.GetRelationById(blipID);
      if (relationById is XWPFPictureData)
        return (XWPFPictureData) relationById;
      return (XWPFPictureData) null;
    }

    public XWPFNumbering GetNumbering()
    {
      return this.numbering;
    }

    public XWPFStyles GetStyles()
    {
      return this.styles;
    }

    public XWPFParagraph GetParagraph(CT_P p)
    {
      for (int index = 0; index < this.Paragraphs.Count; ++index)
      {
        if (this.Paragraphs[index].GetCTP() == p)
          return this.Paragraphs[index];
      }
      return (XWPFParagraph) null;
    }

    public XWPFTable GetTable(CT_Tbl ctTbl)
    {
      for (int index = 0; index < this.tables.Count; ++index)
      {
        if (this.tables[index].GetCTTbl() == ctTbl)
          return this.tables[index];
      }
      return (XWPFTable) null;
    }

    public IEnumerator<XWPFTable> GetTablesEnumerator()
    {
      return (IEnumerator<XWPFTable>) this.tables.GetEnumerator();
    }

    public IEnumerator<XWPFParagraph> GetParagraphsEnumerator()
    {
      return (IEnumerator<XWPFParagraph>) this.paragraphs.GetEnumerator();
    }

    public XWPFParagraph GetParagraphArray(int pos)
    {
      if (pos >= 0 && pos < this.paragraphs.Count)
        return this.paragraphs[pos];
      return (XWPFParagraph) null;
    }

    public POIXMLDocumentPart GetPart()
    {
      return (POIXMLDocumentPart) this;
    }

    public BodyType GetPartType()
    {
      return BodyType.DOCUMENT;
    }

    public XWPFTableCell GetTableCell(CT_Tc cell)
    {
      if (cell == null || cell.Table == null)
        return (XWPFTableCell) null;
      return this.GetTable(cell.Table)?.GetRow(cell.TableRow)?.GetTableCell(cell);
    }

    public XWPFDocument GetXWPFDocument()
    {
      return this;
    }
  }
}
