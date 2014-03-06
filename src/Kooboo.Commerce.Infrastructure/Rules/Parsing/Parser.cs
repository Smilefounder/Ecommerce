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
        private ParsingContext _context;

        public Expression Parse(string source, ParsingContext context)
        {
            Require.NotNull(source, "source");
            Require.NotNull(context, "context");

            _context = context;
            _tokenzier = new Tokenizer(source, _context);

            var exp =  Expression();

            _context = null;
            _tokenzier = null;

            return exp;
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
                                _context.AddError("Missing right operand for operator " + op.Kind + ".", sourceLocation);
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
                                _context.AddError("Missing right operand for operator " + op.Kind + ".", sourceLocation);
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
                        _context.AddError("Missing expression after open parenthesis char (.", sourceLocation);
                        return null;
                    }

                    sourceLocation = _tokenzier.CurrentLocation;

                    var nextToken = _tokenzier.NextToken();
                    if (nextToken == null || nextToken.Kind != TokenKind.Parenthesis)
                    {
                        _context.AddError("Missing closing parenthesis char ).", sourceLocation);
                        return null;
                    }

                    if (nextToken.Value == "(")
                    {
                        _context.AddError("Expected closing parenthesis char ).", sourceLocation);
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
                    _context.AddError("Expected identifier.", sourceLocation);
                    return null;
                }

                // TODO: Check if it's valid param name

                sourceLocation = _tokenzier.CurrentLocation;

                var op = _tokenzier.NextToken();
                if (op == null || !IsPossibleComparisonOperator(op))
                {
                    _context.AddError("Missing comparison operator after the parameter name.", sourceLocation);
                    return null;
                }

                // TODO: Check if it's valid comparison operator

                sourceLocation = _tokenzier.CurrentLocation;

                var value = ParamValue();

                if (value == null)
                {
                    _context.AddError("Missing parameter value.", sourceLocation);
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

                var valueToken = token;

                if (token.Kind == TokenKind.Identifier)
                {
                    dataSourceId = token.Value;

                    var doubleColon = _tokenzier.NextToken();
                    if (doubleColon == null || doubleColon.Kind != TokenKind.DoubleColon)
                    {
                        _context.AddError("Missing '::' after data source id.", sourceLocation);
                        return null;
                    }

                    valueToken = _tokenzier.NextToken();
                }

                sourceLocation = _tokenzier.CurrentLocation;

                if (valueToken == null)
                {
                    _context.AddError("Missing parameter value.", sourceLocation);
                    return null;
                }
                if (valueToken.Kind != TokenKind.StringLiteral && valueToken.Kind != TokenKind.Number)
                {
                    _context.AddError("Incorrect parameter value. Expected string or number.", sourceLocation);
                    return null;
                }

                lookahead.Accept();

                return new ConditionValueExpression(valueToken.Value, dataSourceId);
            }
        }

        private bool IsPossibleComparisonOperator(Token token)
        {
            return token.Kind == TokenKind.Identifier
                || token.Kind == TokenKind.GreaterThan
                || token.Kind == TokenKind.GreaterThanOrEqual
                || token.Kind == TokenKind.LessThan
                || token.Kind == TokenKind.LessThanOrEqual
                || token.Kind == TokenKind.Equal
                || token.Kind == TokenKind.NotEqual;
        }
    }
}
