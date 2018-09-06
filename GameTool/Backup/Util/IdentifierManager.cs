// Decompiled with JetBrains decompiler
// Type: NPOI.Util.IdentifierManager
// Assembly: NPOI.OOXML, Version=2.0.1.0, Culture=neutral, PublicKeyToken=0df73ec7942b34e1
// MVID: 6A760DA3-350E-482C-AD6F-D5C7807E164F
// Assembly location: F:\work\GameTool\Lib\NPOI.OOXML.dll

using System;
using System.Collections.Generic;

namespace NPOI.Util
{
  public class IdentifierManager
  {
    public static long MAX_ID = 9223372036854775806;
    public static long MIN_ID = 0;
    private long upperbound;
    private long lowerbound;
    private List<IdentifierManager.Segment> segments;

    public IdentifierManager(long lowerbound, long upperbound)
    {
      if (lowerbound > upperbound)
        throw new ArgumentException("lowerbound must not be greater than upperbound");
      if (lowerbound < IdentifierManager.MIN_ID)
        throw new ArgumentException("lowerbound must be greater than or equal to " + (object) IdentifierManager.MIN_ID);
      if (upperbound > IdentifierManager.MAX_ID)
        throw new ArgumentException("upperbound must be less thean or equal " + (object) IdentifierManager.MAX_ID);
      this.lowerbound = lowerbound;
      this.upperbound = upperbound;
      this.segments = new List<IdentifierManager.Segment>();
      this.segments.Add(new IdentifierManager.Segment(lowerbound, upperbound));
    }

    public long Reserve(long id)
    {
      if (id < this.lowerbound || id > this.upperbound)
        throw new ArgumentException("Value for parameter 'id' was out of bounds");
      this.VerifyIdentifiersLeft();
      if (id == this.upperbound)
      {
        int index = this.segments.Count - 1;
        IdentifierManager.Segment segment = this.segments[index];
        if (segment.end != this.upperbound)
          return this.ReserveNew();
        segment.end = this.upperbound - 1L;
        if (segment.start > segment.end)
          this.segments.RemoveAt(index);
        return id;
      }
      if (id == this.lowerbound)
      {
        IdentifierManager.Segment segment = this.segments[0];
        if (segment.start != this.lowerbound)
          return this.ReserveNew();
        segment.start = this.lowerbound + 1L;
        if (segment.end < segment.start)
          this.segments.RemoveAt(0);
        return id;
      }
      for (int index = 0; index < this.segments.Count; ++index)
      {
        IdentifierManager.Segment segment = this.segments[index];
        if (segment.end >= id)
        {
          if (segment.start <= id)
          {
            if (segment.start == id)
            {
              segment.start = id + 1L;
              if (segment.end < segment.start)
                this.segments.Remove(segment);
              return id;
            }
            if (segment.end == id)
            {
              segment.end = id - 1L;
              if (segment.start > segment.end)
                this.segments.Remove(segment);
              return id;
            }
            this.segments.Add(new IdentifierManager.Segment(id + 1L, segment.end));
            segment.end = id - 1L;
            return id;
          }
          break;
        }
      }
      return this.ReserveNew();
    }

    public long ReserveNew()
    {
      this.VerifyIdentifiersLeft();
      IdentifierManager.Segment segment = this.segments[0];
      long start = segment.start;
      ++segment.start;
      if (segment.start > segment.end)
        this.segments.RemoveAt(0);
      return start;
    }

    public bool Release(long id)
    {
      if (id < this.lowerbound || id > this.upperbound)
        throw new ArgumentException("Value for parameter 'id' was out of bounds");
      if (id == this.upperbound)
      {
        IdentifierManager.Segment segment = this.segments[this.segments.Count - 1];
        if (segment.end == this.upperbound - 1L)
        {
          segment.end = this.upperbound;
          return true;
        }
        if (segment.end == this.upperbound)
          return false;
        this.segments.Add(new IdentifierManager.Segment(this.upperbound, this.upperbound));
        return true;
      }
      if (id == this.lowerbound)
      {
        IdentifierManager.Segment segment = this.segments[0];
        if (segment.start == this.lowerbound + 1L)
        {
          segment.start = this.lowerbound;
          return true;
        }
        if (segment.start == this.lowerbound)
          return false;
        this.segments.Insert(0, new IdentifierManager.Segment(this.lowerbound, this.lowerbound));
        return true;
      }
      long num1 = id + 1L;
      long num2 = id - 1L;
      for (int index = 0; index < this.segments.Count; ++index)
      {
        IdentifierManager.Segment segment1 = this.segments[0];
        if (segment1.end >= num2)
        {
          if (segment1.start > num1)
          {
            this.segments.Insert(index, new IdentifierManager.Segment(id, id));
            return true;
          }
          if (segment1.start == num1)
          {
            segment1.start = id;
            return true;
          }
          if (segment1.end == num2)
          {
            segment1.end = id;
            if (index + 1 < this.segments.Count)
            {
              IdentifierManager.Segment segment2 = this.segments[index + 1];
              if (segment2.start == segment1.end + 1L)
              {
                segment1.end = segment2.end;
                this.segments.Remove(segment2);
              }
            }
            return true;
          }
          break;
        }
      }
      return false;
    }

    public long GetRemainingIdentifiers()
    {
      long num = 0;
      foreach (IdentifierManager.Segment segment in this.segments)
      {
        num -= segment.start;
        num = num + segment.end + 1L;
      }
      return num;
    }

    private void VerifyIdentifiersLeft()
    {
      if (this.segments.Count == 0)
        throw new InvalidOperationException("No identifiers left");
    }

    internal class Segment
    {
      public long start;
      public long end;

      public Segment(long start, long end)
      {
        this.start = start;
        this.end = end;
      }

      public override string ToString()
      {
        return "[" + (object) this.start + "; " + (object) this.end + "]";
      }
    }
  }
}
