using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Kooboo.Extensions;
using Newtonsoft.Json;

namespace Kooboo.Commerce.Activities
{
    public class AttachedActivityInfo
    {
        public virtual int Id { get; set; }

        [Required, StringLength(100)]
        public virtual string Description { get; set; }

        [Required, StringLength(100)]
        public virtual string ActivityName { get; set; }

        // TODO: Make private
        public virtual string ParametersJson { get; protected set; }

        private Dictionary<string, string> _parameters;

        public virtual string GetParameterValue(string paramName)
        {
            EnsureParametersLoaded();

            string value = null;

            if (_parameters.TryGetValue(paramName, out value))
            {
                return value;
            }

            return null;
        }

        public virtual T GetParameterValue<T>(string paramName, T defaultValue = default(T))
        {
            var strValue = GetParameterValue(paramName);
            if (strValue == null)
            {
                return defaultValue;
            }

            var resultType = typeof(T);

            if (resultType == typeof(String))
            {
                return (T)(object)strValue;
            }

            if (resultType.IsValueType)
            {
                return (T)(object)Convert.ChangeType(strValue, resultType);
            }

            return JsonConvert.DeserializeObject<T>(strValue);
        }

        public virtual void SetParmeterValue(string paramName, object value)
        {
            EnsureParametersLoaded();

            _parameters.Remove(paramName);

            if (value == null)
            {
                _parameters.Add(paramName, null);
            }
            else
            {
                var valueType = value.GetType();
                string strValue = null;

                if (valueType == typeof(String))
                {
                    strValue = (string)value;
                }
                else if (valueType.IsValueType)
                {
                    strValue = value.ToString();
                }
                else
                {
                    strValue = JsonConvert.SerializeObject(value);
                }

                _parameters.Add(paramName, strValue);
            }

            ParametersJson = JsonConvert.SerializeObject(_parameters);
        }

        public virtual IDictionary<string, string> GetParameters(IDictionary<string, string> defaultValues = null)
        {
            EnsureParametersLoaded();

            var result = new Dictionary<string, string>(_parameters, StringComparer.OrdinalIgnoreCase);

            if (defaultValues != null)
            {
                foreach (var each in defaultValues)
                {
                    if (!result.ContainsKey(each.Key))
                    {
                        result.Add(each.Key, each.Value);
                    }
                }
            }

            return result;
        }

        public virtual void SetParameters(IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                ParametersJson = null;
            }
            else
            {
                ParametersJson = JsonConvert.SerializeObject(parameters);
            }

            _parameters = null;
        }

        private void EnsureParametersLoaded()
        {
            if (_parameters == null)
            {
                if (String.IsNullOrWhiteSpace(ParametersJson))
                {
                    _parameters = new Dictionary<string, string>();
                }
                else
                {
                    _parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(ParametersJson);
                }
            }
        }

        public virtual bool IsEnabled { get; set; }

        public virtual int Priority { get; set; }

        /// <summary>
        /// Get or sets if this activity need to execute asynchrously.
        /// </summary>
        public virtual bool IsAsyncExeuctionEnabled { get; set; }

        /// <summary>
        /// Get or sets the delay (in seconds) from the time the event occurs.
        /// </summary>
        public virtual int AsyncExecutionDelay { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }

        public virtual ActivityRule Rule { get; set; }

        public virtual RuleBranch RuleBranch { get; set; }

        protected AttachedActivityInfo()
        {
        }

        public AttachedActivityInfo(ActivityRule rule, RuleBranch branch, string description, string activityName)
        {
            Require.NotNullOrEmpty(description, "description");
            Require.NotNullOrEmpty(activityName, "activityName");

            Rule = rule;
            RuleBranch = branch;
            Description = description;
            ActivityName = activityName;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public DateTime CalculateExecutionTime(DateTime eventTimeUtc)
        {
            if (!IsAsyncExeuctionEnabled)
            {
                return eventTimeUtc;
            }

            return eventTimeUtc.AddSeconds(AsyncExecutionDelay);
        }

        public void EnableAsyncExecution(int delay)
        {
            IsAsyncExeuctionEnabled = true;
            AsyncExecutionDelay = delay;
        }

        public void UpdateAsyncExecutionDelay(int newDelay)
        {
            AsyncExecutionDelay = newDelay;
        }

        public void DisableAsyncExecution()
        {
            IsAsyncExeuctionEnabled = false;
        }
    }
}
