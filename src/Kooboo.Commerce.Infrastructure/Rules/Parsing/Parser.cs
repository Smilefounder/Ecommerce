using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parsing
{
    /// <summary>
    /// The parser used to parse the source into expression tree (abstract syntax tree).
    /// </summary>
    /// <remarks>
    /// Grammer:
    /// 
    ///      expression : term [ OR term ]...
    ///            term : factor [ AND factor ]
    ///          factor : falt_condition | ( expression )
    ///  flat_condition : identifier comparison_op param_value
    ///   comparison_op : identifier in the available comparison operator list
    ///     param_value : [ datasource_id:: ] string_literal | number
    ///     
    /// Notes:
    /// - falt_condition means the condition expression without nesting expressions;
    /// - term and factor are temp non-terminals used to handle logical opoerator (AND, OR) precedence;
    /// </remarks>
    public class Parser
    {
        private Tokenizer _tokenzier;
        private List<Error> _errors;

        public Expression Parse(string source)
        {
            Require.NotNull(source, "source");

            _errors = new List<Error>();
            _tokenzier = new Tokenizer(source);

            return Expression();
        }

        // expression : term [ OR term ]...
        private Expression Expression()
        {
            var exp = Term();

            if (exp != null)
            {
                while (!_tokenzier.IsEndOfFile)
                {
                    using (var lookahead = _tokenzier.BeginLookahead())
                    {
                        var op = _tokenzier.NextToken();
                        if (op != null && op.Kind == TokenKind.Or)
                        {
                            var sourceLocation = _tokenzier.CurrentLocation;

                            var right = Term();
                            if (right != null)
                            {
                                lookahead.Accept();
                                exp = new LogicalBindaryExpression(exp, right, LogicalOperator.Or);
                            }
                            else
                            {
                                _errors.Add(new Error("Missing right operand for operator " + op.Kind + ".", sourceLocation));
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return exp;
        }

        // term : factor [ AND factor ]
        private Expression Term()
        {
            var exp = Factor();

            if (exp != null)
            {
                while (!_tokenzier.IsEndOfFile)
                {
                    using (var lookahead = _tokenzier.BeginLookahead())
                    {
                        var op = _tokenzier.NextToken();
                        if (op != null && op.Kind == TokenKind.And)
                        {
                            var sourceLocation = _tokenzier.CurrentLocation;

                            var right = Factor();
                            if (right != null)
                            {
                                lookahead.Accept();
                                exp = new LogicalBindaryExpression(exp, right, LogicalOperator.And);
                            }
                            else
                            {
                                _errors.Add(new Error("Missing right operand for operator " + op.Kind + ".", sourceLocation));
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return exp;
        }

        // factor : falt_condition | ( expression )
        private Expression Factor()
        {
            using (var lookahead = _tokenzier.BeginLookahead())
            {
                var token = _tokenzier.NextToken();
                if (token != null && token.Kind == TokenKind.Parenthesis && token.Value == "(")
                {
                    var sourceLocation = _tokenzier.CurrentLocation;
                    var exp = Expression();
                    if (exp == null)
                    {
                        _errors.Add(new Error("Missing expression after open parenthesis char (.", sourceLocation));
                        return null;
                    }

                    sourceLocation = _tokenzier.CurrentLocation;

                    var nextToken = _tokenzier.NextToken();
                    if (nextToken == null || nextToken.Kind != TokenKind.Parenthesis)
                    {
                        _errors.Add(new Error("Missing closing parenthesis char ).", sourceLocation));
                        return null;
                    }

                    if (nextToken.Value == "(")
                    {
                        _errors.Add(new Error("Expected closing parenthesis char ).", sourceLocation));
                    }

                    lookahead.Accept();

                    return exp;
                }
            }

            return FlatCondition();
        }

        // flat_condition : identifier comparison_op param_value
        private Expression FlatCondition()
        {
            using (var lookahead = _tokenzier.BeginLookahead())
            {
                var sourceLocation = _tokenzier.CurrentLocation;

                var paramName = _tokenzier.NextToken();
                if (paramName == null)
                {
                    return null;
                }

                if (paramName.Kind != TokenKind.Identifier)
                {
                    _errors.Add(new Error("Expected identifier.", sourceLocation));
                    return null;
                }

                // TODO: Check if it's valid param name

                sourceLocation = _tokenzier.CurrentLocation;

                var op = _tokenzier.NextToken();
                if (op == null || op.Kind != TokenKind.Identifier)
                {
                    _errors.Add(new Error("Missing comparison operator after the parameter name.", sourceLocation));
                    return null;
                }

                // TODO: Check if it's valid comparison operator

                sourceLocation = _tokenzier.CurrentLocation;

                var value = ParamValue();

                if (value == null)
                {
                    _errors.Add(new Error("Missing parameter value.", sourceLocation));
                    return null;
                }

                lookahead.Accept();

                var param = new ConditionParamExpression(paramName.Value);

                return new ConditionExpression(param, value, op.Value);
            }
        }

        // param_value : [ datasource_id:: ] string_literal | number
        private ConditionValueExpression ParamValue()
        {
            using (var lookahead = _tokenzier.BeginLookahead())
            {
                var token = _tokenzier.NextToken();
                if (token == null)
                {
                    return null;
                }

                var sourceLocation = _tokenzier.CurrentLocation;
                string dataSourceId = null;

                if (token.Kind == TokenKind.Identifier)
                {
                    dataSourceId = token.Value;

                    var doubleColon = _tokenzier.NextToken();
                    if (doubleColon == null || doubleColon.Kind != TokenKind.DoubleColon)
                    {
                        _errors.Add(new Error("Missing '::' after data source id.", sourceLocation));
                        return null;
                    }
                }

                sourceLocation = _tokenzier.CurrentLocation;

                var value = _tokenzier.NextToken();
                if (value == null)
                {
                    _errors.Add(new Error("Missing parameter value.", sourceLocation));
                    return null;
                }
                if (value.Kind != TokenKind.StringLiteral && value.Kind != TokenKind.Number)
                {
                    _errors.Add(new Error("Incorrect parameter value. Expected string or number.", sourceLocation));
                    return null;
                }

                return new ConditionValueExpression(value.Value, dataSourceId);
            }
        }
    }
}
