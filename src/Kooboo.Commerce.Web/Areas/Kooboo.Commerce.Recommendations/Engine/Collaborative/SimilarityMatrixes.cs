using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    public static class SimilarityMatrixes
    {
        // Key: instance name, Value key: behavior type
        static readonly Dictionary<string, Dictionary<string, ISimilarityMatrix>> _matrixesByInstances = new Dictionary<string, Dictionary<string, ISimilarityMatrix>>();

        public static ISimilarityMatrix Get(string instance, string behaviorType)
        {
            return _matrixesByInstances[instance][behaviorType];
        }

        public static IEnumerable<ISimilarityMatrix> All(string instance)
        {
            return _matrixesByInstances[instance].Values;
        }

        public static void Register(string instance, string behaviorType, ISimilarityMatrix matrix)
        {
            if (!_matrixesByInstances.ContainsKey(instance))
            {
                _matrixesByInstances.Add(instance, new Dictionary<string, ISimilarityMatrix>());
            }

            var matrixesByNames = _matrixesByInstances[instance];
            if (matrixesByNames.ContainsKey(behaviorType))
            {
                matrixesByNames[behaviorType] = matrix;
            }
            else
            {
                matrixesByNames.Add(behaviorType, matrix);
            }
        }

        public static void Remove(string instance)
        {
            _matrixesByInstances.Remove(instance);
        }
    }
}