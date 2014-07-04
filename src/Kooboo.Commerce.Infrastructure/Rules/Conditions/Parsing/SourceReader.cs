using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Parsing
{
    public class SourceReader
    {
        private string _source;
        private int _position;

        public int Position
        {
            get
            {
                return _position;
            }
        }

        public SourceReader(string source)
        {
            Require.NotNull(source, "source");
            _source = source;
        }

        public bool IsEndOfFile
        {
            get
            {
                return Position == _source.Length;
            }
        }

        public char Peek()
        {
            if (IsEndOfFile)
            {
                return '\0';
            }

            return _source[_position];
        }

        public char Read()
        {
            if (IsEndOfFile)
            {
                return '\0';
            }

            return _source[_position++];
        }

        public void MoveNext()
        {
            if (!IsEndOfFile)
            {
                _position++;
            }
        }

        /// <summary>
        /// Try reading the expected string.
        /// If the expected string is sucessfully read, return true and move the cursor.
        /// Otherwise return false and move back the cursor.
        /// </summary>
        public bool Read(string expected)
        {
            Require.NotNullOrEmpty(expected, "expected");

            if (IsEndOfFile)
            {
                return false;
            }

            using (var lookahead = BeginLookahead())
            {
                for (var i = 0; i < expected.Length; i++)
                {
                    if (IsEndOfFile)
                    {
                        return false;
                    }

                    if (Read() != expected[i])
                    {
                        return false;
                    }
                }

                lookahead.Accept();
            }

            return true;
        }

        public void SkipWhile(Predicate<char> predicate)
        {
            Require.NotNull(predicate, "predicate");

            while (!IsEndOfFile && predicate(Peek()))
            {
                Read();
            }
        }

        /// <summary>
        /// After the call to BeginLookahead, 
        /// future reads will be cancelled if the lookahead is not accepted before it's disposed.
        /// </summary>
        public Lookahead BeginLookahead()
        {
            var currentPosition = _position;
            return new Lookahead(() =>
            {
                _position = currentPosition;
            });
        }
    }
}
