using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Tasks
{
    public static class RecomputeSimilarityMatrixTasks
    {
        static readonly Dictionary<string, List<RecomputeSimilarityMatrixTask>> _tasksByInstance = new Dictionary<string,List<RecomputeSimilarityMatrixTask>>();

        public static IEnumerable<RecomputeSimilarityMatrixTask> GetTasks(string instance)
        {
            return _tasksByInstance[instance];
        }

        public static RecomputeSimilarityMatrixTask GetTask(string instance, string taskName)
        {
            return _tasksByInstance[instance].FirstOrDefault(t => t.Name == taskName);
        }

        public static void AddTask(string instance, RecomputeSimilarityMatrixTask task)
        {
            AddTasks(instance, new[] { task });
        }

        public static void AddTasks(string instance, IEnumerable<RecomputeSimilarityMatrixTask> tasks)
        {
            if (!_tasksByInstance.ContainsKey(instance))
            {
                _tasksByInstance.Add(instance, tasks.ToList());
            }
            else
            {
                _tasksByInstance[instance].AddRange(tasks);
            }
        }

        public static void RemoveTasks(string instance)
        {
            _tasksByInstance.Remove(instance);
        }
    }
}