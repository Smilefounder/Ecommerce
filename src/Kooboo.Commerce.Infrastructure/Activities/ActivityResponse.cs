using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    // TODO: 是否有更好的名称？这个枚举控制的是执行完一个Activity后的下一个动作，继续或取消主流程 
    public enum ActivityResponse
    {
        Continue = 0,
        SkipSubsequentActivities = 1,
        AbortTransaction = 2
    }
}
