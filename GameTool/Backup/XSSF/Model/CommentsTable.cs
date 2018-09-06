// Decompiled with JetBrains decompiler
// Type: NPOI.XSSF.Model.CommentsTable
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using NPOI.OpenXml4Net.OPC;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.OpenXmlFormats.Vml;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NPOI.XSSF.Model
{
  public class CommentsTable : POIXMLDocumentPart
  {
    private CT_Comments comments;
    private Dictionary<string, CT_Comment> commentRefs;

    public CommentsTable()
    {
      this.comments = new CT_Comments();
      this.comments.AddNewCommentList();
      this.comments.AddNewAuthors().AddAuthor("");
    }

    internal CommentsTable(PackagePart part, PackageRelationship rel)
      : base(part, rel)
    {
      this.ReadFrom(part.GetInputStream());
    }

    public void ReadFrom(Stream is1)
    {
      try
      {
        this.comments = CommentsDocument.Parse(is1).GetComments();
      }
      catch (XmlException ex)
      {
        throw new IOException(ex.Message);
      }
    }

    public void WriteTo(Stream out1)
    {
      CommentsDocument commentsDocument = new CommentsDocument();
      commentsDocument.SetComments(this.comments);
      commentsDocument.Save(out1);
    }

    protected override void Commit()
    {
      Stream outputStream = this.GetPackagePart().GetOutputStream();
      this.WriteTo(outputStream);
      outputStream.Close();
    }

    public void ReferenceUpdated(string oldReference, CT_Comment comment)
    {
      if (this.commentRefs == null)
        return;
      this.commentRefs.Remove(oldReference);
      this.commentRefs[comment.@ref] = comment;
    }

    public int GetNumberOfComments()
    {
      return this.comments.commentList.SizeOfCommentArray();
    }

    public int GetNumberOfAuthors()
    {
      return this.comments.authors.SizeOfAuthorArray();
    }

    public string GetAuthor(long authorId)
    {
      return this.comments.authors.GetAuthorArray((int) authorId);
    }

    public int FindAuthor(string author)
    {
      for (int index = 0; index < this.comments.authors.SizeOfAuthorArray(); ++index)
      {
        if (this.comments.authors.GetAuthorArray(index).Equals(author))
          return index;
      }
      return this.AddNewAuthor(author);
    }

    public XSSFComment FindCellComment(string cellRef)
    {
      CT_Comment ctComment = this.GetCTComment(cellRef);
      if (ctComment != null)
        return new XSSFComment(this, ctComment, (CT_Shape) null);
      return (XSSFComment) null;
    }

    public CT_Comment GetCTComment(string cellRef)
    {
      if (this.commentRefs == null)
      {
        this.commentRefs = new Dictionary<string, CT_Comment>();
        foreach (CT_Comment ctComment in this.comments.commentList.comment)
          this.commentRefs[ctComment.@ref] = ctComment;
      }
      if (!this.commentRefs.ContainsKey(cellRef))
        return (CT_Comment) null;
      return this.commentRefs[cellRef];
    }

    public CT_Comment CreateComment()
    {
      CT_Comment ctComment = this.comments.commentList.AddNewComment();
      ctComment.@ref = "A1";
      ctComment.authorId = 0U;
      if (this.commentRefs != null)
        this.commentRefs[ctComment.@ref] = ctComment;
      return ctComment;
    }

    public bool RemoveComment(string cellRef)
    {
      CT_CommentList commentList = this.comments.commentList;
      if (commentList != null)
      {
        for (int index = 0; index < commentList.SizeOfCommentArray(); ++index)
        {
          CT_Comment commentArray = commentList.GetCommentArray(index);
          if (cellRef.Equals(commentArray.@ref))
          {
            commentList.RemoveComment(index);
            if (this.commentRefs != null)
              this.commentRefs.Remove(cellRef);
            return true;
          }
        }
      }
      return false;
    }

    private int AddNewAuthor(string author)
    {
      int index = this.comments.authors.SizeOfAuthorArray();
      this.comments.authors.Insert(index, author);
      return index;
    }

    public CT_Comments GetCTComments()
    {
      return this.comments;
    }
  }
}
