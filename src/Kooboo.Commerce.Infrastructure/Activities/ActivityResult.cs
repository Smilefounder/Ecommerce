using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    // TODO: Activity都是管理员添加的，一个Activity实现者并不知道后面会有什么Activity，
    //       如果它错误返回了SkipSubsequenceActivities，那后面的Activity全都不执行，这是否会使得线上系统变得不好理解？
    //       例如某天加了一个Activity后，发现本来后面发邮件的Activity都不发邮件了，以为问题出现在后面的Activity上面。
    public enum ActivityResult
    {
        Continue = 0,
        SkipSubsequentActivities = 1,
        AbortTransaction = 2
    }
}
