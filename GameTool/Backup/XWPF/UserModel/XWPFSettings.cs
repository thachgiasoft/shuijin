// Decompiled with JetBrains decompiler
// Type: NPOI.XWPF.UserModel.XWPFSettings
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NPOI.XWPF.UserModel
{
  public class XWPFSettings : POIXMLDocumentPart
  {
    private CT_Settings ctSettings;

    public XWPFSettings(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
    }

    public XWPFSettings()
    {
      this.ctSettings = new CT_Settings();
    }

    internal override void OnDocumentRead()
    {
      base.OnDocumentRead();
      this.ReadFrom(this.GetPackagePart().GetInputStream());
    }

    public long GetZoomPercent()
    {
      CT_Zoom zoom = this.ctSettings.zoom;
      return long.Parse((this.ctSettings.IsSetZoom() ? this.ctSettings.zoom : this.ctSettings.AddNewZoom()).percent);
    }

    public void SetZoomPercent(long zoomPercent)
    {
      if (!this.ctSettings.IsSetZoom())
        this.ctSettings.AddNewZoom();
      this.ctSettings.zoom.percent = zoomPercent.ToString();
    }

    public bool IsEnforcedWith(ST_DocProtect editValue)
    {
      CT_DocProtect documentProtection = this.ctSettings.documentProtection;
      if (documentProtection == null || !documentProtection.enforcement.Equals((object) ST_OnOff.Value1))
        return false;
      return documentProtection.edit.Equals((object) editValue);
    }

    public void SetEnforcementEditValue(ST_DocProtect editValue)
    {
      this.SafeGetDocumentProtection().enforcement = ST_OnOff.Value1;
      this.SafeGetDocumentProtection().edit = editValue;
    }

    public void RemoveEnforcement()
    {
      this.SafeGetDocumentProtection().enforcement = ST_OnOff.Value0;
    }

    public void SetUpdateFields()
    {
      this.ctSettings.updateFields = new CT_OnOff()
      {
        val = ST_OnOff.True
      };
    }

    public bool IsUpdateFields()
    {
      if (this.ctSettings.IsSetUpdateFields())
        return this.ctSettings.updateFields.val == ST_OnOff.True;
      return false;
    }

    protected override void Commit()
    {
      if (this.ctSettings == null)
        throw new InvalidOperationException("Unable to write out settings that were never read in!");
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[1]{ new XmlQualifiedName("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main") });
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      new SettingsDocument(this.ctSettings).Save(outputStream, namespaces);
      outputStream.Close();
    }

    private CT_DocProtect SafeGetDocumentProtection()
    {
      if (this.ctSettings.documentProtection == null)
        this.ctSettings.documentProtection = new CT_DocProtect();
      return this.ctSettings.documentProtection;
    }

    private void ReadFrom(Stream inputStream)
    {
      try
      {
        this.ctSettings = SettingsDocument.Parse(inputStream).Settings;
      }
      catch (Exception ex)
      {
        throw new Exception("SettingsDocument parse failed", ex);
      }
    }
  }
}
