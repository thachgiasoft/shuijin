//---------------------------------------------------------------------
//  节点定义: 
//      节点->[Entity|执行条件|控制状态->[Parallel or Selector or Sequence ]]
//  详细说明:
//      根据节点的控制状态可以把节点分为如下3类: 
//      并行节点(Parallel): 该节点下满足条件的子节点可以同时运行
//      选择节点(Selector): 该节点下满足条件的子节点选择一个运行, 遇true返回.
//      顺序节点(Sequence): 该节点下满足条件的子节点可以依次运行, 遇false返回.
//---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Features.BevTree
{
    /// <summary>
    /// 
    /// </summary>
    public class BevNode
    {
    }
}
