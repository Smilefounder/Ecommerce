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

        /// <summary>
        /// Difference between the compared (original) value and the previous compared value.
        /// </summary>
        public T Diff { get; set; }

        public T Translated { get; set; }

        public TranslationTuple() { }

        public TranslationTuple(T compared, T diff, T translated)
        {
            Compared = compared;
            Diff = diff;
            Translated = translated;
        }
    }
}