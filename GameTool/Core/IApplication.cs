using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool.Core
{
    public interface IApplication
    {
        void Initialize();

        void Uninitialize();
    }
}
