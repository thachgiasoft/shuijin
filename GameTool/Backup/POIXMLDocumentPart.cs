// Decompiled with JetBrains decompiler
// Type: NPOI.POIXMLDocumentPart
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.Exceptions;
using NPOI.OpenXml4Net.OPC;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace NPOI
{
  public class POIXMLDocumentPart
  {
    private static POILogger logger = POILogFactory.GetLogger(typeof (POIXMLDocumentPart));
    private Dictionary<string, POIXMLDocumentPart> relations = new Dictionary<string, POIXMLDocumentPart>();
    private PackagePart packagePart;
    private PackageRelationship packageRel;
    private POIXMLDocumentPart parent;
    private int relationCounter;

    private int IncrementRelationCounter()
    {
      ++this.relationCounter;
      return this.relationCounter;
    }

    private int DecrementRelationCounter()
    {
      --this.relationCounter;
      return this.relationCounter;
    }

    private int GetRelationCounter()
    {
      return this.relationCounter;
    }

    public POIXMLDocumentPart(OPCPackage pkg)
    {
      PackageRelationship relationship = pkg.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument").GetRelationship(0);
      this.packagePart = pkg.GetPart(relationship);
      this.packageRel = relationship;
    }

    public POIXMLDocumentPart()
    {
    }

    public POIXMLDocumentPart(PackagePart part, PackageRelationship rel)
    {
      this.packagePart = part;
      this.packageRel = rel;
    }

    public POIXMLDocumentPart(POIXMLDocumentPart parent, PackagePart part, PackageRelationship rel)
    {
      this.packagePart = part;
      this.packageRel = rel;
      this.parent = parent;
    }

    protected void Rebase(OPCPackage pkg)
    {
      PackageRelationshipCollection relationshipsByType = this.packagePart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
      if (relationshipsByType.Size != 1)
        throw new InvalidOperationException("Tried to rebase using http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument but found " + (object) relationshipsByType.Size + " parts of the right type");
      this.packageRel = relationshipsByType.GetRelationship(0);
      this.packagePart = this.packagePart.GetRelatedPart(this.packageRel);
    }

    public PackagePart GetPackagePart()
    {
      return this.packagePart;
    }

    public PackageRelationship GetPackageRelationship()
    {
      return this.packageRel;
    }

    public List<POIXMLDocumentPart> GetRelations()
    {
      return new List<POIXMLDocumentPart>((IEnumerable<POIXMLDocumentPart>) this.relations.Values);
    }

    public POIXMLDocumentPart GetRelationById(string id)
    {
      return this.relations[id];
    }

    public string GetRelationId(POIXMLDocumentPart part)
    {
      foreach (KeyValuePair<string, POIXMLDocumentPart> relation in this.relations)
      {
        if (relation.Value == part)
          return relation.Key;
      }
      return (string) null;
    }

    public void AddRelation(string id, POIXMLDocumentPart part)
    {
      this.relations[id] = part;
      part.IncrementRelationCounter();
    }

    protected void RemoveRelation(POIXMLDocumentPart part)
    {
      this.RemoveRelation(part, true);
    }

    protected bool RemoveRelation(POIXMLDocumentPart part, bool RemoveUnusedParts)
    {
      string relationId = this.GetRelationId(part);
      if (relationId == null)
        return false;
      part.DecrementRelationCounter();
      this.GetPackagePart().RemoveRelationship(relationId);
      this.relations.Remove(relationId);
      if (RemoveUnusedParts)
      {
        if (part.GetRelationCounter() == 0)
        {
          try
          {
            part.onDocumentRemove();
          }
          catch (IOException ex)
          {
            throw new POIXMLException((Exception) ex);
          }
          this.GetPackagePart().Package.RemovePart(part.GetPackagePart());
        }
      }
      return true;
    }

    public POIXMLDocumentPart GetParent()
    {
      return this.parent;
    }

    public override string ToString()
    {
      if (this.packagePart != null)
        return this.packagePart.ToString();
      return (string) null;
    }

    protected virtual void Commit()
    {
    }

    protected void OnSave(List<PackagePart> alreadySaved)
    {
      this.Commit();
      alreadySaved.Add(this.GetPackagePart());
      foreach (POIXMLDocumentPart poixmlDocumentPart in this.relations.Values)
      {
        if (!alreadySaved.Contains(poixmlDocumentPart.GetPackagePart()))
          poixmlDocumentPart.OnSave(alreadySaved);
      }
    }

    public POIXMLDocumentPart CreateRelationship(POIXMLRelation descriptor, POIXMLFactory factory)
    {
      return this.CreateRelationship(descriptor, factory, -1, false);
    }

    public POIXMLDocumentPart CreateRelationship(POIXMLRelation descriptor, POIXMLFactory factory, int idx)
    {
      return this.CreateRelationship(descriptor, factory, idx, false);
    }

    protected POIXMLDocumentPart CreateRelationship(POIXMLRelation descriptor, POIXMLFactory factory, int idx, bool noRelation)
    {
      try
      {
        PackagePartName partName = PackagingUriHelper.CreatePartName(descriptor.GetFileName(idx));
        PackageRelationship packageRelationship = (PackageRelationship) null;
        PackagePart part = this.packagePart.Package.CreatePart(partName, descriptor.ContentType);
        if (!noRelation)
          packageRelationship = this.packagePart.AddRelationship(partName, TargetMode.Internal, descriptor.Relation);
        POIXMLDocumentPart documentPart = factory.CreateDocumentPart(descriptor);
        documentPart.packageRel = packageRelationship;
        documentPart.packagePart = part;
        documentPart.parent = this;
        if (!noRelation)
          this.AddRelation(packageRelationship.Id, documentPart);
        return documentPart;
      }
      catch (PartAlreadyExistsException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        throw new POIXMLException(ex);
      }
    }

    protected void Read(POIXMLFactory factory, Dictionary<PackagePart, POIXMLDocumentPart> context)
    {
      try
      {
        foreach (PackageRelationship relationship in this.packagePart.Relationships)
        {
          TargetMode? targetMode = relationship.TargetMode;
          if ((targetMode.GetValueOrDefault() != TargetMode.Internal ? 0 : (targetMode.HasValue ? 1 : 0)) != 0)
          {
            Uri targetUri = relationship.TargetUri;
            PackagePart index;
            if (targetUri.OriginalString.IndexOf('#') >= 0)
            {
              index = (PackagePart) null;
            }
            else
            {
              index = this.packagePart.Package.GetPart(PackagingUriHelper.CreatePartName(targetUri));
              if (index == null)
              {
                POIXMLDocumentPart.logger.Log(7, (object) ("Skipped invalid entry " + (object) relationship.TargetUri));
                continue;
              }
            }
            if (index == null || !context.ContainsKey(index))
            {
              POIXMLDocumentPart documentPart = factory.CreateDocumentPart(this, relationship, index);
              documentPart.parent = this;
              this.AddRelation(relationship.Id, documentPart);
              if (index != null)
              {
                context[index] = documentPart;
                if (index.HasRelationships)
                  documentPart.Read(factory, context);
              }
            }
            else
              this.AddRelation(relationship.Id, context[index]);
          }
        }
      }
      catch (Exception ex)
      {
        if (ex.InnerException != null && ex.InnerException.InnerException != null)
          POIXMLDocumentPart.logger.Log(1, ex.InnerException.InnerException);
        throw;
      }
    }

    protected PackagePart GetTargetPart(PackageRelationship rel)
    {
      return this.GetPackagePart().GetRelatedPart(rel);
    }

    internal virtual void OnDocumentCreate()
    {
    }

    internal virtual void OnDocumentRead()
    {
    }

    protected virtual void onDocumentRemove()
    {
    }
  }
}
