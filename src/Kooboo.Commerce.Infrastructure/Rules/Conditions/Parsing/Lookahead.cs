using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Parsing
{
    public class Lookahead : IDisposable
    {
        private bool _isAccepted;
        private Action _cancelAction;

        public Lookahead(Action cancelAction)
        {
            Require.NotNull(cancelAction, "cancelAction");
            _cancelAction = cancelAction;
        }

        public void Accept()
        {
            _isAccepted = true;
        }

        public void Dispose()
        {
            if (!_isAccepted)
            {
                _cancelAction();
            }
        }
    }
}
