using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Actions
{
    public interface IEntityAction
    {
        string Name { get; }

        Type EntityType { get;}

        string ButtonText { get; }

        string IconClass { get; }

        string ConfirmMessage { get; }

        int Order { get; }

        void Execute(object entity, CommerceInstance instance);
    }
}
