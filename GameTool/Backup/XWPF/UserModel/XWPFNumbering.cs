// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFNumbering
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XWPF.UserModel
{
  public class XWPFNumbering : POIXMLDocumentPart
  {
    protected List<XWPFAbstractNum> abstractNums = new List<XWPFAbstractNum>();
    protected List<XWPFNum> nums = new List<XWPFNum>();
    private CT_Numbering ctNumbering;
    private bool isNew;

    public XWPFNumbering(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.isNew = true;
    }

    public XWPFNumbering()
    {
      this.abstractNums = new List<XWPFAbstractNum>();
      this.nums = new List<XWPFNum>();
      this.isNew = true;
    }

    internal override void OnDocumentRead()
    {
      Stream inputStream = this.GetPackagePart().GetInputStream();
      try
      {
        this.ctNumbering = NumberingDocument.Parse(inputStream).Numbering;
        foreach (CT_Num num in (IEnumerable<CT_Num>) this.ctNumbering.GetNumList())
          this.nums.Add(new XWPFNum(num, this));
        foreach (CT_AbstractNum abstractNum in (IEnumerable<CT_AbstractNum>) this.ctNumbering.GetAbstractNumList())
          this.abstractNums.Add(new XWPFAbstractNum(abstractNum, this));
        this.isNew = false;
      }
      catch (Exception ex)
      {
        throw new POIXMLException();
      }
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      NumberingDocument numberingDocument = new NumberingDocument(this.ctNumbering);
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[8]{ new XmlQualifiedName("ve", "http://schemas.openxmlformats.org/markup-compatibility/2006"), new XmlQualifiedName("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"), new XmlQualifiedName("m", "http://schemas.openxmlformats.org/officeDocument/2006/math"), new XmlQualifiedName("v", "urn:schemas-microsoft-com:vml"), new XmlQualifiedName("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"), new XmlQualifiedName("w10", "urn:schemas-microsoft-com:office:word"), new XmlQualifiedName("wne", "http://schemas.microsoft.com/office/word/2006/wordml"), new XmlQualifiedName("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") });
      numberingDocument.Save(outputStream, namespaces);
      outputStream.Close();
    }

    public void SetNumbering(CT_Numbering numbering)
    {
      this.ctNumbering = numbering;
    }

    public bool NumExist(string numID)
    {
      foreach (XWPFNum num in this.nums)
      {
        if (num.GetCTNum().numId.Equals(numID))
          return true;
      }
      return false;
    }

    public string AddNum(XWPFNum num)
    {
      this.ctNumbering.AddNewNum();
      this.ctNumbering.SetNumArray(this.ctNumbering.GetNumList().Count - 1, num.GetCTNum());
      this.nums.Add(num);
      return num.GetCTNum().numId;
    }

    public string AddNum(string abstractNumID)
    {
      CT_Num ctNum = this.ctNumbering.AddNewNum();
      ctNum.AddNewAbstractNumId();
      ctNum.abstractNumId.val = abstractNumID;
      ctNum.numId = (this.nums.Count + 1).ToString();
      this.nums.Add(new XWPFNum(ctNum, this));
      return ctNum.numId;
    }

    public void AddNum(string abstractNumID, string numID)
    {
      CT_Num ctNum = this.ctNumbering.AddNewNum();
      ctNum.AddNewAbstractNumId();
      ctNum.abstractNumId.val = abstractNumID;
      ctNum.numId = numID;
      this.nums.Add(new XWPFNum(ctNum, this));
    }

    public XWPFNum GetNum(string numID)
    {
      foreach (XWPFNum num in this.nums)
      {
        if (num.GetCTNum().numId.Equals(numID))
          return num;
      }
      return (XWPFNum) null;
    }

    public XWPFAbstractNum GetAbstractNum(string abstractNumID)
    {
      foreach (XWPFAbstractNum abstractNum in this.abstractNums)
      {
        if (abstractNum.GetAbstractNum().abstractNumId.Equals(abstractNumID))
          return abstractNum;
      }
      return (XWPFAbstractNum) null;
    }

    public string GetIdOfAbstractNum(XWPFAbstractNum abstractNum)
    {
      XWPFAbstractNum xwpfAbstractNum = new XWPFAbstractNum(abstractNum.GetCTAbstractNum().Copy(), this);
      for (int index = 0; index < this.abstractNums.Count; ++index)
      {
        xwpfAbstractNum.GetCTAbstractNum().abstractNumId = index.ToString();
        xwpfAbstractNum.SetNumbering(this);
        if (xwpfAbstractNum.GetCTAbstractNum().ValueEquals(this.abstractNums[index].GetCTAbstractNum()))
          return xwpfAbstractNum.GetCTAbstractNum().abstractNumId;
      }
      return (string) null;
    }

    public string AddAbstractNum(XWPFAbstractNum abstractNum)
    {
      int count = this.abstractNums.Count;
      if (abstractNum.GetAbstractNum() != null)
      {
        this.ctNumbering.AddNewAbstractNum().Set(abstractNum.GetAbstractNum());
      }
      else
      {
        this.ctNumbering.AddNewAbstractNum();
        abstractNum.GetAbstractNum().abstractNumId = count.ToString();
        this.ctNumbering.SetAbstractNumArray(count, abstractNum.GetAbstractNum());
      }
      this.abstractNums.Add(abstractNum);
      return abstractNum.GetAbstractNum().abstractNumId;
    }

    public string AddAbstractNum()
    {
      XWPFAbstractNum xwpfAbstractNum = new XWPFAbstractNum(this.ctNumbering.AddNewAbstractNum(), this);
      xwpfAbstractNum.AbstractNumId = this.abstractNums.Count.ToString();
      xwpfAbstractNum.MultiLevelType = MultiLevelType.HybridMultilevel;
      xwpfAbstractNum.InitLvl();
      this.abstractNums.Add(xwpfAbstractNum);
      return xwpfAbstractNum.GetAbstractNum().abstractNumId;
    }

    public bool RemoveAbstractNum(string abstractNumID)
    {
      if (int.Parse(abstractNumID) >= this.abstractNums.Count)
        return false;
      this.ctNumbering.RemoveAbstractNum(int.Parse(abstractNumID));
      this.abstractNums.RemoveAt(int.Parse(abstractNumID));
      return true;
    }

    public string GetAbstractNumID(string numID)
    {
      XWPFNum num = this.GetNum(numID);
      if (num == null)
        return (string) null;
      if (num.GetCTNum() == null)
        return (string) null;
      if (num.GetCTNum().abstractNumId == null)
        return (string) null;
      return num.GetCTNum().abstractNumId.val;
    }
  }
}
