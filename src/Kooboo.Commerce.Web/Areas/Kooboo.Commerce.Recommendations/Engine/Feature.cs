using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    /// <summary>
    /// 表示一个特征。
    /// </summary>
    public class Feature
    {
        public string Id { get; set; }

        public double Weight { get; set; }

        public Feature(string id)
            : this(id, 1.0d)
        {
        }

        public Feature(string id, double weight)
        {
            Id = id;
            Weight = weight;
        }

        public override string ToString()
        {
            return String.Format("[{0}:{1}]", Id, Weight);
        }
    }
}