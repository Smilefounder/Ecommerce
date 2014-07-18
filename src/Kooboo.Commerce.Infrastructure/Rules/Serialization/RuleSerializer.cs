using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Rules.Conditions;
using Kooboo.Commerce.Rules.Parameters;
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
        private Dictionary<Type, Func<Rule, XElement>> _serializers;
        private Dictionary<string, Func<EventEntry, XElement, Rule>> _deserializers;

        private ActivityEventManager _eventManager = ActivityEventManager.Instance;
        public ActivityEventManager EventManager
        {
            get
            {
                return _eventManager;
            }
            set
            {
                Require.NotNull(value, "value");
                _eventManager = value;
            }
        }

        private RuleParameterProviderCollection _parameterProviders;

        public RuleParameterProviderCollection ParameterProviders
        {
            get
            {
                if (_parameterProviders == null)
                {
                    _parameterProviders = Kooboo.Commerce.Rules.Parameters.RuleParameterProviders.Providers;
                }
                return _parameterProviders;
            }
            set
            {
                Require.NotNull(value, "value");
                _parameterProviders = value;
            }
        }

        public RuleSerializer()
        {
            _serializers = new Dictionary<Type, Func<Rule, XElement>>
            {
                { typeof(IfElseRule), SerializeIfElse },
                { typeof(SwitchCaseRule), SerializeSwitchCase },
                { typeof(AlwaysRule), SerializeAlways }
            };
            _deserializers = new Dictionary<string, Func<EventEntry, XElement, Rule>>
            {
                { "if", DeserializeIfElse },
                { "switch", DeserializeSwitchCase },
                { "always", DeserializeAlways }
            };
        }

        public XElement SerializeSlot(EventSlot slot)
        {
            var @event = EventManager.FindEvent(slot.EventName);
            return SerializeSlot(@event, slot.Rules);
        }

        public XElement SerializeSlot(EventEntry @event, IEnumerable<Rule> rules)
        {
            var element = new XElement("event", new XAttribute("name", @event.EventName));
            foreach (var rule in rules)
            {
                var ruleElement = SerializeRule(rule);
                element.Add(ruleElement);
            }

            return element;
        }

        public XElement SerializeRule(Rule rule)
        {
            var serializer = _serializers[rule.GetType()];
            return serializer(rule);
        }

        public EventSlot DeserializeSlot(string xml)
        {
            return DeserializeSlot(XElement.Parse(xml));
        }

        public EventSlot DeserializeSlot(XElement element)
        {
            var eventName = element.Attribute("name").Value;

            var @event = EventManager.FindEvent(eventName);
            if (@event == null)
            {
                return null;
            }

            var slot = new EventSlot(@event.EventName);

            foreach (var ruleElement in element.Elements())
            {
                slot.Rules.Add(DeserializeRule(@event, ruleElement));
            }

            return slot;
        }

        public Rule DeserializeRule(EventEntry @event, XElement element)
        {
            var deserializer = _deserializers[element.Name.LocalName];
            return deserializer(@event, element);
        }

        #region If-Else Rule

        private XElement SerializeIfElse(Rule rule)
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
                    thenElement.Add(SerializeRule(thenRule));
                }
            }

            // Else
            if (ifElseRule.Else.Count > 0)
            {
                var elseElement = new XElement("else");
                ifElement.Add(elseElement);

                foreach (var elseRule in ifElseRule.Else)
                {
                    elseElement.Add(SerializeRule(elseRule));
                }
            }

            return ifElement;
        }

        private Rule DeserializeIfElse(EventEntry @event, XElement element)
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
                    rule.Then.Add(DeserializeRule(@event, ruleElement));
                }
            }

            var elseElement = element.Element("else");
            if (elseElement != null)
            {
                foreach (var ruleElement in elseElement.Elements())
                {
                    rule.Else.Add(DeserializeRule(@event, ruleElement));
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

        #region Switch-Case-Default Rule

        private XElement SerializeSwitchCase(Rule rule)
        {
            var switchCaseRule = rule as SwitchCaseRule;
            var switchElement = new XElement("switch", new XAttribute("parameter", switchCaseRule.Parameter.Name));

            // Cases
            if (switchCaseRule.Cases.Count > 0)
            {
                foreach (var caze in switchCaseRule.Cases)
                {
                    var caseElement = new XElement("case", new XAttribute("value", caze.Key));
                    foreach (var childRule in caze.Value)
                    {
                        caseElement.Add(SerializeRule(childRule));
                    }

                    switchElement.Add(caseElement);
                }
            }

            // Default
            if (switchCaseRule.Default.Count > 0)
            {
                var defaultElement = new XElement("default");

                foreach (var childRule in switchCaseRule.Default)
                {
                    defaultElement.Add(SerializeRule(childRule));
                }

                switchElement.Add(defaultElement);
            }

            return switchElement;
        }

        private Rule DeserializeSwitchCase(EventEntry @event, XElement element)
        {
            var paramName = element.Attribute("parameter").Value;
            var param = ParameterProviders.GetParameter(@event.EventType, paramName);
            if (param == null)
            {
                return null;
            }

            var rule = new SwitchCaseRule(param);

            // Cases
            foreach (var caseElement in element.Elements("case"))
            {
                var value = Convert.ChangeType(caseElement.Attribute("value").Value, param.ValueType);
                var caseRules = new List<Rule>();

                foreach (var caseRuleElement in caseElement.Elements())
                {
                    var caseRule = DeserializeRule(@event, caseRuleElement);
                    if (caseRule != null)
                    {
                        caseRules.Add(caseRule);
                    }
                }

                rule.Cases.Add(value, caseRules);
            }

            // Default
            var defaultElement = element.Element("default");
            if (defaultElement != null)
            {
                foreach (var defaultRuleElement in defaultElement.Elements())
                {
                    var defaultRule = DeserializeRule(@event, defaultRuleElement);
                    if (defaultRule != null)
                    {
                        rule.Default.Add(defaultRule);
                    }
                }
            }

            return rule;
        }

        #endregion

        #region Always Rule

        private XElement SerializeAlways(Rule rule)
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

        private Rule DeserializeAlways(EventEntry @event, XElement element)
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
            var name = element.Attribute("name").Value;
            var desc = element.Attribute("description").Value;

            var activity = new ConfiguredActivity(name, desc);

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
