// Decompiled with JetBrains decompiler
// Type: NPOI.POIXMLProperties
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.Exceptions;
using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXml4Net.OPC.Internal;
using NPOI.OpenXmlFormats;
using System;
using System.IO;

namespace NPOI
{
  public class POIXMLProperties
  {
    private static ExtendedPropertiesDocument NEW_EXT_INSTANCE = new ExtendedPropertiesDocument();
    private OPCPackage pkg;
    private POIXMLProperties.CoreProperties core;
    private POIXMLProperties.ExtendedProperties ext;
    private POIXMLProperties.CustomProperties cust;
    private PackagePart extPart;
    private PackagePart custPart;
    private static CustomPropertiesDocument NEW_CUST_INSTANCE;

    static POIXMLProperties()
    {
      POIXMLProperties.NEW_EXT_INSTANCE.AddNewProperties();
      POIXMLProperties.NEW_CUST_INSTANCE = new CustomPropertiesDocument();
      POIXMLProperties.NEW_CUST_INSTANCE.AddNewProperties();
    }

    public POIXMLProperties(OPCPackage docPackage)
    {
      this.pkg = docPackage;
      this.core = new POIXMLProperties.CoreProperties((PackagePropertiesPart) this.pkg.GetPackageProperties());
      PackageRelationshipCollection relationshipsByType1 = this.pkg.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
      if (relationshipsByType1.Size == 1)
      {
        this.extPart = this.pkg.GetPart(relationshipsByType1.GetRelationship(0));
        this.ext = new POIXMLProperties.ExtendedProperties(ExtendedPropertiesDocument.Parse(this.extPart.GetInputStream()));
      }
      else
      {
        this.extPart = (PackagePart) null;
        this.ext = new POIXMLProperties.ExtendedProperties(POIXMLProperties.NEW_EXT_INSTANCE.Copy());
      }
      PackageRelationshipCollection relationshipsByType2 = this.pkg.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties");
      if (relationshipsByType2.Size == 1)
      {
        this.custPart = this.pkg.GetPart(relationshipsByType2.GetRelationship(0));
        this.cust = new POIXMLProperties.CustomProperties(CustomPropertiesDocument.Parse(this.custPart.GetInputStream()));
      }
      else
      {
        this.custPart = (PackagePart) null;
        this.cust = new POIXMLProperties.CustomProperties(POIXMLProperties.NEW_CUST_INSTANCE.Copy());
      }
    }

    public POIXMLProperties.CoreProperties GetCoreProperties()
    {
      return this.core;
    }

    public POIXMLProperties.ExtendedProperties GetExtendedProperties()
    {
      return this.ext;
    }

    public POIXMLProperties.CustomProperties GetCustomProperties()
    {
      return this.cust;
    }

    public virtual void Commit()
    {
      if (this.extPart == null)
      {
        if (!POIXMLProperties.NEW_EXT_INSTANCE.ToString().Equals(this.ext.props.ToString()))
        {
          try
          {
            PackagePartName partName = PackagingUriHelper.CreatePartName("/docProps/app.xml");
            this.pkg.AddRelationship(partName, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
            this.extPart = this.pkg.CreatePart(partName, "application/vnd.openxmlformats-officedocument.extended-properties+xml");
          }
          catch (InvalidFormatException ex)
          {
            throw new POIXMLException((Exception) ex);
          }
        }
      }
      if (this.custPart == null)
      {
        if (!POIXMLProperties.NEW_CUST_INSTANCE.ToString().Equals(this.cust.props.ToString()))
        {
          try
          {
            PackagePartName partName = PackagingUriHelper.CreatePartName("/docProps/custom.xml");
            this.pkg.AddRelationship(partName, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties");
            this.custPart = this.pkg.CreatePart(partName, "application/vnd.openxmlformats-officedocument.custom-properties+xml");
          }
          catch (InvalidFormatException ex)
          {
            throw new POIXMLException((Exception) ex);
          }
        }
      }
      if (this.extPart != null)
      {
        Stream outputStream = this.extPart.GetOutputStream();
        this.ext.props.Save(outputStream);
        outputStream.Close();
      }
      if (this.custPart == null)
        return;
      Stream outputStream1 = this.custPart.GetOutputStream();
      this.cust.props.Save(outputStream1);
      outputStream1.Close();
    }

    public class CoreProperties
    {
      private PackagePropertiesPart part;

      internal CoreProperties(PackagePropertiesPart part)
      {
        this.part = part;
      }

      public string GetCategory()
      {
        return this.part.GetCategoryProperty();
      }

      public void SetCategory(string category)
      {
        this.part.SetCategoryProperty(category);
      }

      public string GetContentStatus()
      {
        return this.part.GetContentStatusProperty();
      }

      public void SetContentStatus(string contentStatus)
      {
        this.part.SetContentStatusProperty(contentStatus);
      }

      public string GetContentType()
      {
        return this.part.GetContentTypeProperty();
      }

      public void SetContentType(string contentType)
      {
        this.part.SetContentTypeProperty(contentType);
      }

      public DateTime? GetCreated()
      {
        return this.part.GetCreatedProperty();
      }

      public void SetCreated(DateTime? date)
      {
        this.part.SetCreatedProperty(date);
      }

      public void SetCreated(string date)
      {
        this.part.SetCreatedProperty(date);
      }

      public string GetCreator()
      {
        return this.part.GetCreatorProperty();
      }

      public void SetCreator(string creator)
      {
        this.part.SetCreatorProperty(creator);
      }

      public string GetDescription()
      {
        return this.part.GetDescriptionProperty();
      }

      public void SetDescription(string description)
      {
        this.part.SetDescriptionProperty(description);
      }

      public string GetIdentifier()
      {
        return this.part.GetIdentifierProperty();
      }

      public void SetIdentifier(string identifier)
      {
        this.part.SetIdentifierProperty(identifier);
      }

      public string GetKeywords()
      {
        return this.part.GetKeywordsProperty();
      }

      public void SetKeywords(string keywords)
      {
        this.part.SetKeywordsProperty(keywords);
      }

      public DateTime? GetLastPrinted()
      {
        return this.part.GetLastPrintedProperty();
      }

      public void SetLastPrinted(DateTime? date)
      {
        this.part.SetLastPrintedProperty(date);
      }

      public void SetLastPrinted(string date)
      {
        this.part.SetLastPrintedProperty(date);
      }

      public DateTime? GetModified()
      {
        return this.part.GetModifiedProperty();
      }

      public void SetModified(DateTime? date)
      {
        this.part.SetModifiedProperty(date);
      }

      public void SetModified(string date)
      {
        this.part.SetModifiedProperty(date);
      }

      public string GetSubject()
      {
        return this.part.GetSubjectProperty();
      }

      public void SetSubjectProperty(string subject)
      {
        this.part.SetSubjectProperty(subject);
      }

      public void SetTitle(string title)
      {
        this.part.SetTitleProperty(title);
      }

      public string GetTitle()
      {
        return this.part.GetTitleProperty();
      }

      public string GetRevision()
      {
        return this.part.GetRevisionProperty();
      }

      public void SetRevision(string revision)
      {
        try
        {
          long.Parse(revision);
          this.part.SetRevisionProperty(revision);
        }
        catch (FormatException ex)
        {
        }
      }

      public PackagePropertiesPart GetUnderlyingProperties()
      {
        return this.part;
      }
    }

    public class ExtendedProperties
    {
      public ExtendedPropertiesDocument props;

      internal ExtendedProperties(ExtendedPropertiesDocument props)
      {
        this.props = props;
      }

      public CT_ExtendedProperties GetUnderlyingProperties()
      {
        return this.props.GetProperties();
      }
    }

    public class CustomProperties
    {
      public static string FORMAT_ID = "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}";
      public CustomPropertiesDocument props;

      internal CustomProperties(CustomPropertiesDocument props)
      {
        this.props = props;
      }

      public CT_CustomProperties GetUnderlyingProperties()
      {
        return this.props.GetProperties();
      }

      private CT_Property Add(string name)
      {
        if (this.Contains(name))
          throw new ArgumentException("A property with this name already exists in the custom properties");
        CT_Property ctProperty = this.props.GetProperties().AddNewProperty();
        int num = this.NextPid();
        ctProperty.pid = num;
        ctProperty.fmtid = POIXMLProperties.CustomProperties.FORMAT_ID;
        ctProperty.name = name;
        return ctProperty;
      }

      public void AddProperty(string name, string value)
      {
        CT_Property ctProperty = this.Add(name);
        ctProperty.ItemElementName = ItemChoiceType.lpwstr;
        ctProperty.Item = (object) value;
      }

      public void AddProperty(string name, double value)
      {
        CT_Property ctProperty = this.Add(name);
        ctProperty.ItemElementName = ItemChoiceType.r8;
        ctProperty.Item = (object) value;
      }

      public void AddProperty(string name, int value)
      {
        CT_Property ctProperty = this.Add(name);
        ctProperty.ItemElementName = ItemChoiceType.i4;
        ctProperty.Item = (object) value;
      }

      public void AddProperty(string name, bool value)
      {
        CT_Property ctProperty = this.Add(name);
        ctProperty.ItemElementName = ItemChoiceType.@bool;
        ctProperty.Item = (object) value;
      }

      protected int NextPid()
      {
        int num = 1;
        foreach (CT_Property property in this.props.GetProperties().GetPropertyList())
        {
          if (property.pid > num)
            num = property.pid;
        }
        return num + 1;
      }

      public bool Contains(string name)
      {
        foreach (CT_Property property in this.props.GetProperties().GetPropertyList())
        {
          if (property.name.Equals(name))
            return true;
        }
        return false;
      }
    }
  }
}
