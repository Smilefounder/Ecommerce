using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class TranslationTuple<T>
        where T : class
    {
        public T Compared { get; set; }

        public T Translated { get; set; }

        public TranslationTuple() { }

        public TranslationTuple(T compared, T translated)
        {
            Compared = compared;
            Translated = translated;
        }
    }
}