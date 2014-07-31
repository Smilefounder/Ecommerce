using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class TranslationPair<T>
        where T : class
    {
        public T Compared { get; set; }

        public T Translated { get; set; }

        public TranslationPair() { }

        public TranslationPair(T compared, T translated)
        {
            Compared = compared;
            Translated = translated;
        }
    }
}