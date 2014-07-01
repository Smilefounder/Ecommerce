using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parsing
{
    [Serializable]
    public class ParserException : Exception
    {
        public ReadOnlyCollection<Error> Errors { get; private set; }

        public override string Message
        {
            get
            {
                var message = new StringBuilder(base.Message);
                message.AppendLine();

                foreach (var error in Errors)
                {
                    message.AppendLine(("Char " + (error.Location.CharIndex + 1) + ": " + error.Message));
                }

                return message.ToString();
            }
        }

        public ParserException(string message, IEnumerable<Error> errors)
            : base(message)
        {
            Errors = errors.ToList().AsReadOnly();
        }

        protected ParserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
