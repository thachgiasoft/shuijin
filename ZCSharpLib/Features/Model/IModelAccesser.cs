using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Features.Model
{
    public interface IModelAccesser
    {
        void Add<T>(T t) where T : class, new();

        void Remove<T>(T t) where T : class, new();

        void Modify<T>(T t) where T : class, new();
    }
}
