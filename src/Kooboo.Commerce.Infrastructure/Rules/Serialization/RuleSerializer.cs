using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules.Activities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Kooboo.Commerce.Rules.Serialization
{
    public class RuleSerializer
    {
        private Dictionary<Type, Func<RuleBase, XElement>> _serializers;
        private Dictionary<string, Func<XElement, RuleBase>> _deserializers;

        public RuleSerializer()
        {
            _serializers = new Dictionary<Type, Func<RuleBase, XElement>>
            {
                { typeof(IfElseRule), SerializeIfElse },
                { typeof(AlwaysRule), SerializeAlways }
            };
        }

        public XElement Serialize(string eventName, IEnumerable<RuleBase> rules)
        {
            var element = new XElement("event", new XAttribute("name", eventName));

            foreach (var rule in rules)
            {
                var ruleElement = Serialize(rule);
                element.Add(ruleElement);
            }

            return element;
        }

        public XElement Serialize(RuleBase rule)
        {
            var serializer = _serializers[rule.GetType()];
            return serializer(rule);
        }

        public RuleBase Deserialize(XElement element)
        {
            var deserializer = _deserializers[element.Name.LocalName];
            return deserializer(element);
        }

        #region If-Else Rule

        private XElement SerializeIfElse(RuleBase rule)
        {
            var ifElseRule = rule as IfElseRule;
            var ifElement = new XElement("if");
            
            var conditionsElement = new XElement("conditions");
            ifElement.Add(conditionsElement);

            foreach (var condition in ifElseRule.Conditions)
            {
                conditionsElement.Add(SerializeCondition(condition));
            }

            // Then
            if (ifElseRule.Then.Count > 0)
            {
                var thenElement = new XElement("then");
                ifElement.Add(thenElement);

                foreach (var thenRule in ifElseRule.Then)
                {
                    thenElement.Add(Serialize(thenRule));
                }
            }

            // Else
            if (ifElseRule.Else.Count > 0)
            {
                var elseElement = new XElement("else");
                ifElement.Add(elseElement);

                foreach (var elseRule in ifElseRule.Else)
                {
                    elseElement.Add(Serialize(elseRule));
                }
            }

            return ifElement;
        }

        private RuleBase DeserializeIfElse(XElement element)
        {
            var rule = (IfElseRule)TypeActivator.CreateInstance(typeof(IfElseRule));
            var conditionsElement = element.Element("conditions");
            if (conditionsElement != null)
            {
                foreach (var conditionElement in conditionsElement.Elements())
                {
                    rule.Conditions.Add(DeserializeCondition(conditionElement));
                }
            }

            var thenElement = element.Element("then");
            if (thenElement != null)
            {
                foreach (var ruleElement in thenElement.Elements())
                {
                    rule.Then.Add(Deserialize(ruleElement));
                }
            }

            var elseElement = element.Element("else");
            if (elseElement != null)
            {
                foreach (var ruleElement in elseElement.Elements())
                {
                    rule.Else.Add(Deserialize(ruleElement));
                }
            }

            return rule;
        }

        private XElement SerializeCondition(Condition condition)
        {
            var elementName = condition.Type == ConditionType.Include ? "include" : "exclude";
            return new XElement(elementName, condition.Expression);
        }

        private Condition DeserializeCondition(XElement element)
        {
            var type = element.Name == "include" ? ConditionType.Include : ConditionType.Exclude;
            return new Condition(element.Value, type);
        }

        #endregion

        #region Always Rule

        private XElement SerializeAlways(RuleBase rule)
        {
            var alwaysRule = rule as AlwaysRule;
            var element = new XElement("always");

            foreach (var activity in alwaysRule.Activities)
            {
                element.Add(SerializeActivity(activity));
            }

            return element;
        }

        private XElement SerializeActivity(ConfiguredActivity activity)
        {
            var element = new XElement("activity",
                new XAttribute("name", activity.ActivityName),
                new XAttribute("description", activity.Description),
                new XAttribute("priority", activity.Priority),
                new XAttribute("async", activity.Async)
            );

            if (activity.Async)
            {
                element.Add("async-delay", activity.AsyncDelay);
            }

            if (activity.Config != null)
            {
                var configElement = new XElement("config");
                configElement.Add(new XCData(activity.Config));
                element.Add(configElement);
            }

            return element;
        }

        private RuleBase DeserializeAlways(XElement element)
        {
            var rule = (AlwaysRule)TypeActivator.CreateInstance(typeof(AlwaysRule));

            foreach (var activityElement in element.Elements("activity"))
            {
                rule.Activities.Add(DeserializeActivity(activityElement));
            }

            return rule;
        }

        private ConfiguredActivity DeserializeActivity(XElement element)
        {
            var activity = new ConfiguredActivity
            {
                ActivityName = element.Attribute("name").Value,
                Description = element.Attribute("description").Value
            };

            var priorityAttr = element.Attribute("priority");
            if (priorityAttr != null)
            {
                activity.Priority = Convert.ToInt32(priorityAttr.Value);
            }

            var asyncAttr = element.Attribute("async");
            if (asyncAttr != null)
            {
                activity.Async = Convert.ToBoolean(asyncAttr.Value);
            }

            if (activity.Async)
            {
                var delayAttr = element.Attribute("async-delay");
                if (delayAttr != null)
                {
                    activity.AsyncDelay = Convert.ToInt32(delayAttr.Value);
                }
            }

            var configElement = element.Element("config");
            if (configElement != null)
            {
                activity.Config = configElement.Value;
            }

            return activity;
        }

        #endregion

    }
}
