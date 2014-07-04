using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Activities
{
    /// <summary>
    /// Indicates an activity wants to use a custom activity setting editor.
    /// </summary>
    public interface IHasCustomActivityConfigEditor
    {
        string GetEditorVirtualPath();
    }
}
