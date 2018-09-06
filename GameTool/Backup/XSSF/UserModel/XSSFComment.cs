// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.UserModel.XSSFComment
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.OpenXmlFormats.Vml;
using NPOI.OpenXmlFormats.Vml.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.Model;
using System;

namespace NPOI.XSSF.UserModel
{
  public class XSSFComment : IComment
  {
    private CT_Comment _comment;
    private CommentsTable _comments;
    private CT_Shape _vmlShape;
    private XSSFRichTextString _str;

    public XSSFComment(CommentsTable comments, CT_Comment comment, CT_Shape vmlShape)
    {
      this._comment = comment;
      this._comments = comments;
      this._vmlShape = vmlShape;
    }

    public string Author
    {
      get
      {
        return this._comments.GetAuthor((long) (int) this._comment.authorId);
      }
      set
      {
        this._comment.authorId = (uint) this._comments.FindAuthor(value);
      }
    }

    public int Column
    {
      get
      {
        return (int) new CellReference(this._comment.@ref).Col;
      }
      set
      {
        string oldReference = this._comment.@ref;
        this._comment.@ref = new CellReference(this.Row, value).FormatAsString();
        this._comments.ReferenceUpdated(oldReference, this._comment);
        if (this._vmlShape == null)
          return;
        this._vmlShape.GetClientDataArray(0).SetColumnArray(0, value);
      }
    }

    public int Row
    {
      get
      {
        return new CellReference(this._comment.@ref).Row;
      }
      set
      {
        string oldReference = this._comment.@ref;
        this._comment.@ref = new CellReference(value, this.Column).FormatAsString();
        this._comments.ReferenceUpdated(oldReference, this._comment);
        if (this._vmlShape == null)
          return;
        this._vmlShape.GetClientDataArray(0).SetRowArray(0, value);
      }
    }

    public bool Visible
    {
      get
      {
        bool flag = false;
        if (this._vmlShape != null)
        {
          string style = this._vmlShape.style;
          if (style != null)
          {
            flag = style.IndexOf("visibility:visible") != -1;
          }
          else
          {
            if (this._vmlShape.GetClientDataArray(0) == null)
              return false;
            flag = this._vmlShape.GetClientDataArray(0).visibleSpecified;
          }
        }
        return flag;
      }
      set
      {
        if (this._vmlShape == null)
          return;
        string str;
        if (value)
        {
          str = "position:absolute;visibility:visible";
          this._vmlShape.GetClientDataArray(0).visible = ST_TrueFalseBlank.@true;
          this._vmlShape.GetClientDataArray(0).visibleSpecified = true;
        }
        else
        {
          str = "position:absolute;visibility:hidden";
          this._vmlShape.GetClientDataArray(0).visible = ST_TrueFalseBlank.@false;
          this._vmlShape.GetClientDataArray(0).visibleSpecified = false;
        }
        this._vmlShape.style = str;
      }
    }

    public IRichTextString String
    {
      get
      {
        if (this._str == null && this._comment.text != null)
          this._str = new XSSFRichTextString(this._comment.text);
        return (IRichTextString) this._str;
      }
      set
      {
        if (!(value is XSSFRichTextString))
          throw new ArgumentException("Only XSSFRichTextString argument is supported");
        this._str = (XSSFRichTextString) value;
        this._comment.text = this._str.GetCTRst();
      }
    }

    public void SetString(string str)
    {
      this.String = (IRichTextString) new XSSFRichTextString(str);
    }

    internal CT_Comment GetCTComment()
    {
      return this._comment;
    }

    internal CT_Shape GetCTShape()
    {
      return this._vmlShape;
    }
  }
}
