using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Collaborative
{
    public static class SimilarityMatrixes
    {
        // Key: instance name, Value key: matrix name
        static readonly Dictionary<string, Dictionary<string, ISimilarityMatrix>> _matrixesByInstances = new Dictionary<string, Dictionary<string, ISimilarityMatrix>>();

        public static ISimilarityMatrix GetMatrix(string instance, string name)
        {
            return _matrixesByInstances[instance][name];
        }

        public static IEnumerable<ISimilarityMatrix> GetMatrixes(string instance)
        {
            return _matrixesByInstances[instance].Values;
        }

        public static void SetMatrix(string instance, string name, ISimilarityMatrix matrix)
        {
            if (!_matrixesByInstances.ContainsKey(instance))
            {
                _matrixesByInstances.Add(instance, new Dictionary<string, ISimilarityMatrix>());
            }

            var matrixesByNames = _matrixesByInstances[instance];
            if (matrixesByNames.ContainsKey(name))
            {
                matrixesByNames[name] = matrix;
            }
            else
            {
                matrixesByNames.Add(name, matrix);
            }
        }

        public static void RemoveMatrix(string instance)
        {
            _matrixesByInstances.Remove(instance);
        }
    }
}