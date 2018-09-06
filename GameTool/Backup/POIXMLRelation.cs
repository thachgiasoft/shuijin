// Decompiled with JetBrains decompiler
// Type: NPOI.POIXMLRelation
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using System;

namespace NPOI
{
  public abstract class POIXMLRelation
  {
    protected string _type;
    protected string _relation;
    protected string _defaultName;
    private Type _cls;

    public POIXMLRelation(string type, string rel, string defaultName, Type cls)
    {
      this._type = type;
      this._relation = rel;
      this._defaultName = defaultName;
      this._cls = cls;
    }

    public POIXMLRelation(string type, string rel, string defaultName)
      : this(type, rel, defaultName, (Type) null)
    {
    }

    public string ContentType
    {
      get
      {
        return this._type;
      }
    }

    public string Relation
    {
      get
      {
        return this._relation;
      }
    }

    public string DefaultFileName
    {
      get
      {
        return this._defaultName;
      }
    }

    public string GetFileName(int index)
    {
      if (this._defaultName.IndexOf("#") == -1)
        return this.DefaultFileName;
      return this._defaultName.Replace("#", index.ToString());
    }

    public Type RelationClass
    {
      get
      {
        return this._cls;
      }
    }
  }
}
