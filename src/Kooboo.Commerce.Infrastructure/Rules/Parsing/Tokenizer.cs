using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parsing
{
    // Identifier  : [a-z]\w* -> Custom operator should be in the form of identifier
    // Number      : [0-9]+(.[0-9]+)?
    // String      : "[^"]*" -> escape double quote char by using two double quotes
    // Parenthsis  : ( | )
    // Double colon: ::
    public class Tokenizer
    {
        private SourceReader _source;
        private StringBuilder _buffer = new StringBuilder();
        private List<Error> _errors = new List<Error>();

        public IEnumerable<Error> Errors
        {
            get
            {
                return _errors;
            }
        }

        public bool IsEndOfFile
        {
            get
            {
                return _source.IsEndOfFile;
            }
        }

        public SourceLocation CurrentLocation
        {
            get
            {
                return new SourceLocation(_source.Position);
            }
        }

        public Tokenizer(string source)
            : this(new SourceReader(source))
        {
        }

        public Tokenizer(SourceReader source)
        {
            Require.NotNull(source, "source");
            _source = source;
        }

        public Lookahead BeginLookahead()
        {
            return _source.BeginLookahead();
        }

        public Token NextToken()
        {
            return Parse(Identifier, StringLiteral, Parenthsis, DoubleColon, Number);
        }

        private Token Parse(params Func<Token>[] parsers)
        {
            foreach (var parser in parsers)
            {
                var token = parser();
                if (token != null)
                {
                    return token;
                }
            }

            return null;
        }

        private Token Identifier()
        {
            _source.SkipWhile(char.IsWhiteSpace);

            if (_source.IsEndOfFile)
            {
                return null;
            }

            _buffer.Clear();

            Token token = null;

            using (var lookahead = _source.BeginLookahead())
            {
                var startPosition = _source.Position;
                var ch = _source.Read();
                // identifier must start with a letter
                if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch == '_')
                {
                    _buffer.Append(ch);

                    while (!_source.IsEndOfFile)
                    {
                        ch = _source.Peek();

                        if (char.IsDigit(ch) || char.IsLetter(ch) || ch == '_')
                        {
                            _buffer.Append(ch);
                            _source.MoveNext();
                        }
                        else
                        {
                            lookahead.Accept();

                            var tokenValue = _buffer.ToString();
                            var tokenKind = TokenKind.Identifier;

                            if (tokenValue.Equals("AND", StringComparison.OrdinalIgnoreCase))
                            {
                                tokenKind = TokenKind.And;
                            }
                            else if (tokenValue.Equals("OR", StringComparison.OrdinalIgnoreCase))
                            {
                                tokenKind = TokenKind.Or;
                            }

                            token = new Token(tokenValue, tokenKind, new SourceLocation(startPosition));

                            break;
                        }
                    }
                }
            }

            return token;
        }

        enum NumberParseState
        {
            InPrimaryPart,
            BeginFractionalPart,
            InFractionalPart
        }

        private Token Number()
        {
            SkipWhitespaces();

            if (_source.IsEndOfFile)
            {
                return null;
            }

            Token token = null;
                var state = NumberParseState.InPrimaryPart;

            using (var lookahead = _source.BeginLookahead())
            {
                _buffer.Clear();

                var startPosition = _source.Position;

                while (!_source.IsEndOfFile)
                {
                    var ch = _source.Peek();

                    if (char.IsDigit(ch))
                    {
                        if (state == NumberParseState.BeginFractionalPart)
                        {
                            state = NumberParseState.InFractionalPart;
                        }

                        _buffer.Append(ch);
                        _source.MoveNext();

                        if (_source.IsEndOfFile && ( state == NumberParseState.InPrimaryPart || state == NumberParseState.InFractionalPart))
                        {
                            lookahead.Accept();
                            token = new Token(_buffer.ToString(), TokenKind.Number, new SourceLocation(startPosition));
                            break;
                        }
                    }
                    else if (ch == '.')
                    {
                        state = NumberParseState.BeginFractionalPart;
                        _buffer.Append(ch);
                        _source.MoveNext();
                    }
                    else if (char.IsWhiteSpace(ch))
                    {
                        if (state == NumberParseState.BeginFractionalPart)
                        {
                            _errors.Add(new Error("Missing fractional part.", new SourceLocation(_source.Position)));
                            break;
                        }
                        else
                        {
                            lookahead.Accept();
                            token = new Token(_buffer.ToString(), TokenKind.Number, new SourceLocation(startPosition));
                            break;
                        }
                    }
                    else
                    {
                        _errors.Add(new Error("Expect space.", new SourceLocation(_source.Position)));
                        break;
                    }
                }
            }

            return token;
        }

        private Token StringLiteral()
        {
            SkipWhitespaces();

            if (_source.IsEndOfFile)
            {
                return null;
            }

            using (var lookahead = _source.BeginLookahead())
            {
                var startPosition = _source.Position;

                var ch = _source.Peek();
                if (ch == '"')
                {
                    _buffer.Clear();
                    _source.MoveNext();

                    while (!_source.IsEndOfFile)
                    {
                        ch = _source.Read();

                        if (ch != '"')
                        {
                            _buffer.Append(ch);
                        }
                        else
                        {
                            var next = _source.Peek();
                            // Two double quote is escape
                            if (next == '"')
                            {
                                _source.Read();
                                _buffer.Append('"');
                            }
                            else
                            {
                                // Read string complete
                                lookahead.Accept();
                                return new Token(_buffer.ToString(), TokenKind.StringLiteral, new SourceLocation(startPosition));
                            }
                        }
                    }

                    _errors.Add(new Error("Missing closing double quote for string.", new SourceLocation(_source.Position)));
                }
            }

            return null;
        }

        private Token Parenthsis()
        {
            SkipWhitespaces();

            var location = CurrentLocation;

            if (_source.Read("("))
            {
                return new Token("(", TokenKind.Parenthesis, location);
            }
            if (_source.Read(")"))
            {
                return new Token(")", TokenKind.Parenthesis, location);
            }

            return null;
        }

        private Token DoubleColon()
        {
            SkipWhitespaces();

            if (!_source.IsEndOfFile)
            {
                var location = CurrentLocation;
                if (_source.Read("::"))
                {
                    return new Token("::", TokenKind.DoubleColon, location);
                }
            }

            return null;
        }

        private void SkipWhitespaces()
        {
            _source.SkipWhile(char.IsWhiteSpace);
        }
    }
}
