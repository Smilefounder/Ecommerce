using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Rules
{
    public class RuleEngineFacts
    {
        [Fact]
        public void work_with_full_comparison_operator_name()
        {
            Assert.True(CheckCondition("Age greater than 5", new Person
            {
                Age = 8
            }));
            Assert.False(CheckCondition("Age greater than 5", new Person
            {
                Age = 3
            }));
        }

        [Fact]
        public void work_with_comparison_operator_alias()
        {
            Assert.True(CheckCondition("Age > 5", new Person
            {
                Age = 6
            }));
            Assert.False(CheckCondition("Age > 5", new Person
            {
                Age = 4
            }));
        }

        [Fact]
        public void simple_comparison()
        {
            Assert.True(CheckCondition("Name == \"Kooboo\"", new Person
            {
                Name = "Kooboo"
            }));
            Assert.False(CheckCondition("Name == \"Koo\"", new Person
            {
                Name = "Kooboo"
            }));

            Assert.True(CheckCondition("Age > 5", new Person
            {
                Age = 6
            }));
            Assert.False(CheckCondition("Age > 5", new Person
            {
                Age = 5
            }));
            Assert.False(CheckCondition("Age > 5", new Person
            {
                Age = 4
            }));
        }

        [Fact]
        public void simple_and()
        {
            var condition = "Name == \"Kooboo\" AND Age > 5";

            Assert.True(CheckCondition(condition, new Person
            {
                Name = "Kooboo",
                Age = 7
            }));
            Assert.False(CheckCondition(condition, new Person
            {
                Name = "Kooboo",
                Age = 5
            }));
            Assert.False(CheckCondition(condition, new Person
            {
                Name = "Hello",
                Age = 7
            }));
            Assert.False(CheckCondition(condition, new Person
            {
                Name = "Koo",
                Age = 4
            }));
        }

        [Fact]
        public void simple_or()
        {
            var condition = "Name == \"Kooboo\" or Age > 5";

            Assert.True(CheckCondition(condition, new Person
            {
                Name = "Kooboo",
                Age = 4
            }));
            Assert.True(CheckCondition(condition, new Person
            {
                Name = "Mouhong",
                Age = 6
            }));
            Assert.True(CheckCondition(condition, new Person
            {
                Name = "Kooboo",
                Age = 8
            }));
            Assert.False(CheckCondition(condition, new Person
            {
                Name = "Mouhong",
                Age = 4
            }));
        }

        [Fact]
        public void or_combination()
        {
            var condition = "DevYears > 5 OR Age < 18 OR TotalProjects >= 10";
            Assert.True(CheckCondition(condition, new Person
            {
                DevYears = 4,
                Age = 20,
                TotalProjects = 15
            }));
            Assert.True(CheckCondition(condition, new Person
            {
                DevYears = 4,
                Age = 16,
                TotalProjects = 5
            }));
            Assert.True(CheckCondition(condition, new Person
            {
                DevYears = 6,
                Age = 20,
                TotalProjects = 5
            }));
            Assert.False(CheckCondition(condition, new Person
            {
                DevYears = 4,
                Age = 20,
                TotalProjects = 5
            }));
        }

        [Fact]
        public void and_has_higher_precedence()
        {
            var condition = "DevYears > 5 OR Age < 18 AND TotalProjects > 1";

            Assert.True(CheckCondition(condition, new Person
            {
                DevYears = 6,
                Age = 20,
                TotalProjects = 0
            }));
        }

        [Fact]
        public void parentheses_has_higher_precedence()
        {
            var condition = "(DevYears > 5 OR Age < 18) AND TotalProjects > 1";
            Assert.False(CheckCondition(condition, new Person
            {
                DevYears = 6,
                Age = 20,
                TotalProjects = 0
            }));
        }

        [Fact]
        public void should_awlays_evaluate_as_false_for_null_param_value()
        {
            Assert.False(CheckCondition("Name == \"Kooboo\"", new Person
            {
                Name = null
            }));
        }

        [Fact]
        public void should_throw_for_unrecognized_params()
        {
            var ruleEngine = new RuleEngine();

            Assert.Throws<UnrecognizedParameterException>(() =>
            {
                var condition = "Age > 10 OR NotExistParam == 5";
                ruleEngine.CheckCondition(condition, new Person
                {
                    Age = 10
                });
            });
        }
        
        private bool CheckCondition(string condition, object contextModel)
        {
            var ruleEngine = new RuleEngine();
            return ruleEngine.CheckCondition(condition, contextModel);
        }

        public class IncludeExclude
        {
            [Fact]
            public void can_handle_include()
            {
                var condition = new Condition("Age > 10", ConditionType.Include);
                Assert.True(new RuleEngine().CheckCondition(condition, new Person
                {
                    Age = 12
                }));

                Assert.False(new RuleEngine().CheckCondition(condition, new Person
                {
                    Age = 8
                }));
            }

            [Fact]
            public void exclude_should_gave_inverse_result()
            {
                var condition = new Condition("Age > 10", ConditionType.Exclude);
                Assert.True(new RuleEngine().CheckCondition(condition, new Person
                {
                    Age = 10
                }));
                Assert.True(new RuleEngine().CheckCondition(condition, new Person
                {
                    Age = 8
                }));
                Assert.False(new RuleEngine().CheckCondition(condition, new Person
                {
                    Age = 11
                }));
            }

            [Fact]
            public void multiple_conditions_checking_should_succeed_only_when_all_conditions_succeeded()
            {
                var conditions = new List<Condition>
                {
                    new Condition("Age > 10", ConditionType.Include),
                    new Condition("DevYears > 5", ConditionType.Include),
                    new Condition("TotalProjects > 2", ConditionType.Include)
                };

                Assert.True(new RuleEngine().CheckConditions(conditions, new Person
                {
                    Age = 30,
                    DevYears = 10,
                    TotalProjects = 5
                }));
                Assert.False(new RuleEngine().CheckConditions(conditions, new Person
                {
                    Age = 30,
                    DevYears = 5,
                    TotalProjects = 5
                }));
                Assert.False(new RuleEngine().CheckConditions(conditions, new Person
                {
                    Age = 8,
                    DevYears = 8,
                    TotalProjects = 10
                }));
            }
        }

        public class Person
        {
            [Param]
            public string Name { get; set; }

            [Param]
            public int Age { get; set; }

            [Param]
            public int DevYears { get; set; }

            [Param]
            public int TotalProjects { get; set; }
        }
    }
}
