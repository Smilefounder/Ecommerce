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
    ///       expression : term [ OR term ]...
    ///             term : factor [ AND factor ]
    ///           factor : leaf_condition | ( expression )
    ///       comparison : identifier comparison_op comparison_value
    ///    comparison_op : identifier in the available comparison operator list
    /// comparison_value : string_literal | number
    ///     
    /// Notes:
    /// - leaf_condition means the condition expression without nesting expressions;
    /// - term and factor are temp non-terminals used to handle operator (AND, OR) precedence.
    ///   
    ///   Expressions could be simply expressed as: condition OR/AND condition, 
    ///   but with this grammer, we are not able to handle operator precendence.
    ///   We have to split each operator:
    ///   - Lower precedence operator first, and the operands should be the production rule of a higher precedence operator.
    ///   - Because precedence(OR) is smaller than precendence(AND), so OR first and we write:
    ///      expression: term [ OR term]
    ///   - Then write the 'term' production rule with the higher precedence operator, that is, 'AND', so then we write:
    ///      term: factor [ AND factor]
    /// </remarks>
    public class Parser
    {
        private Tokenizer _tokenzier;
        private ParsingContext _context;

        public Expression Parse(string source, IEnumerable<string> registeredComparisonOperators)
        {
            Require.NotNullOrEmpty(source, "source");

            _context = new ParsingContext(registeredComparisonOperators);
            _tokenzier = new Tokenizer(source, _context);

            var exp = Expression();

            if (_context.Errors.Count > 0)
                throw new ParserException("Failed parsing condition expression.", _context.Errors);

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
                        if (op != null && op.Kind == TokenKind.OR)
                        {
                            var sourceLocation = _tokenzier.CurrentLocation;

                            var right = Term();
                            if (right != null)
                            {
                                lookahead.Accept();
                                exp = new LogicalBindaryExpression(exp, right, LogicalOperator.OR);
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
                        if (op != null && op.Kind == TokenKind.AND)
                        {
                            var sourceLocation = _tokenzier.CurrentLocation;

                            var right = Factor();
                            if (right != null)
                            {
                                lookahead.Accept();
                                exp = new LogicalBindaryExpression(exp, right, LogicalOperator.AND);
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
                        return null;
                    }

                    lookahead.Accept();

                    return exp;
                }
            }

            return Comparison();
        }

        // comparison : identifier comparison_op param_value
        private Expression Comparison()
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

                sourceLocation = _tokenzier.CurrentLocation;

                var op = _tokenzier.NextToken();
                if (op == null || (op.Kind != TokenKind.ComparisonOperator && op.Kind != TokenKind.Identifier) || !_context.RegisteredComparisonOperators.Contains(op.Value))
                {
                    _context.AddError("Missing comparison operator after the parameter name.", sourceLocation);
                    return null;
                }

                sourceLocation = _tokenzier.CurrentLocation;

                var value = ComparisonValue();

                if (value == null)
                {
                    _context.AddError("Missing parameter value.", sourceLocation);
                    return null;
                }

                lookahead.Accept();

                var param = new ComparisonParamExpression(paramName.Value);

                return new ComparisonExpression(param, value, op.Value);
            }
        }

        // comparison_value : string_literal | number
        private ComparisonValueExpression ComparisonValue()
        {
            using (var lookahead = _tokenzier.BeginLookahead())
            {
                var sourceLocation = _tokenzier.CurrentLocation;

                var valueToken = _tokenzier.NextToken();
                if (valueToken == null)
                {
                    _context.AddError("Missing value after condtion comparison operator.", sourceLocation);
                    return null;
                }

                sourceLocation = _tokenzier.CurrentLocation;

                if (valueToken.Kind != TokenKind.StringLiteral && valueToken.Kind != TokenKind.Number)
                {
                    _context.AddError("Incorrect parameter value. Expected string or number.", sourceLocation);
                    return null;
                }

                lookahead.Accept();

                Type valueType = null;

                if (valueToken.Kind == TokenKind.StringLiteral)
                {
                    valueType = typeof(String);
                }
                else if (valueToken.Kind == TokenKind.Number)
                {
                    valueType = typeof(double);
                }

                return new ComparisonValueExpression(valueToken.Value, valueType);
            }
        }
    }
}
