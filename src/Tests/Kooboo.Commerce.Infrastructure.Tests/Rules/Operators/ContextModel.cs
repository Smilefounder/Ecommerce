using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Infrastructure.Tests.Rules.Operators
{
    public class ContextModel
    {
        [Param]
        public int Int32Param { get; set; }

        [Param]
        public int? NullableInt32Param { get; set; }

        [Param]
        public decimal DecimalParam { get; set; }

        [Param]
        public decimal? NullableDecimalParam { get; set; }

        [Param]
        public double DoubleParam { get; set; }

        [Param]
        public float FloatParam { get; set; }

        [Param]
        public string StringParam { get; set; }

        [Param]
        public SampleEnum EnumParam { get; set; }

        [Param]
        public SampleEnum? NullableEnumParam { get; set; }
    }

    public enum SampleEnum
    {
        Value1 = 0,
        Value2 = 1,
        Value3 = 2
    }
}
