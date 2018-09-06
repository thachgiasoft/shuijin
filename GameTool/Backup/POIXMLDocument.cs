// Decompiled with JetBrains decompiler
// Type: NPOI.POIXMLDocument
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net;
using NPOI.OpenXml4Net.Exceptions;
using NPOI.OpenXml4Net.OPC;
using NPOI.POIFS.Common;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace NPOI
{
  public abstract class POIXMLDocument : POIXMLDocumentPart
  {
    public static string DOCUMENT_CREATOR = "NPOI";
    public static string OLE_OBJECT_REL_TYPE = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/oleObject";
    public static string PACK_OBJECT_REL_TYPE = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/package";
    private OPCPackage pkg;
    private POIXMLProperties properties;

    protected POIXMLDocument(OPCPackage pkg)
      : base(pkg)
    {
      this.pkg = pkg;
    }

    public static OPCPackage OpenPackage(string path)
    {
      try
      {
        return OPCPackage.Open(path);
      }
      catch (InvalidFormatException ex)
      {
        throw new IOException(ex.ToString());
      }
    }

    public OPCPackage Package
    {
      get
      {
        return this.pkg;
      }
    }

    protected PackagePart CorePart
    {
      get
      {
        return this.GetPackagePart();
      }
    }

    protected PackagePart[] GetRelatedByType(string contentType)
    {
      PackageRelationshipCollection relationshipsByType = this.GetPackagePart().GetRelationshipsByType(contentType);
      PackagePart[] packagePartArray = new PackagePart[relationshipsByType.Size];
      int index = 0;
      foreach (PackageRelationship rel in relationshipsByType)
      {
        packagePartArray[index] = this.GetPackagePart().GetRelatedPart(rel);
        ++index;
      }
      return packagePartArray;
    }

    public static bool HasOOXMLHeader(Stream inp)
    {
      byte[] b = new byte[4];
      IOUtils.ReadFully(inp, b);
      if (inp is PushbackStream)
        ((PushbackStream) inp).Position -= 4L;
      else
        inp.Position = 0L;
      if ((int) b[0] == (int) POIFSConstants.OOXML_FILE_HEADER[0] && (int) b[1] == (int) POIFSConstants.OOXML_FILE_HEADER[1] && (int) b[2] == (int) POIFSConstants.OOXML_FILE_HEADER[2])
        return (int) b[3] == (int) POIFSConstants.OOXML_FILE_HEADER[3];
      return false;
    }

    public POIXMLProperties GetProperties()
    {
      if (this.properties == null)
      {
        try
        {
          this.properties = new POIXMLProperties(this.pkg);
        }
        catch (Exception ex)
        {
          throw new POIXMLException(ex);
        }
      }
      return this.properties;
    }

    public abstract List<PackagePart> GetAllEmbedds();

    protected void Load(POIXMLFactory factory)
    {
      Dictionary<PackagePart, POIXMLDocumentPart> context = new Dictionary<PackagePart, POIXMLDocumentPart>();
      try
      {
        this.Read(factory, context);
      }
      catch (OpenXml4NetException ex)
      {
        throw new POIXMLException((Exception) ex);
      }
      this.OnDocumentRead();
      context.Clear();
    }

    public void Write(Stream stream)
    {
      List<PackagePart> alreadySaved = new List<PackagePart>();
      this.OnSave(alreadySaved);
      alreadySaved.Clear();
      this.GetProperties().Commit();
      this.Package.Save(stream);
    }
  }
}
