using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider
{
    public class IncludeCollection : IEnumerable<string>
    {
        private TrieNode _trie = TrieNode.Root;
        private StringComparison _comparison;

        public IncludeCollection()
            : this(StringComparison.Ordinal)
        {
        }

        public IncludeCollection(StringComparison comparison)
        {
            _comparison = comparison;
        }

        public void Add(string path)
        {
            Require.NotNullOrEmpty(path, "path");
            _trie.AddNode(path, 0, _comparison);
        }

        public void AddRange(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                Add(path);
            }
        }

        public bool Includes(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return false;
            }

            return _trie.Contains(path, 0, _comparison);
        }

        public void Clear()
        {
            _trie = TrieNode.Root;
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var node in _trie.Nodes)
            {
                foreach (var path in node.Paths())
                {
                    yield return path;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class TrieNode
        {
            public static readonly TrieNode Root = new TrieNode(null);

            public string Value { get; private set; }

            private List<TrieNode> _nodes;

            public List<TrieNode> Nodes
            {
                get
                {
                    if (_nodes == null)
                    {
                        _nodes = new List<TrieNode>();
                    }
                    return _nodes;
                }
            }

            public bool HasNodes
            {
                get
                {
                    return _nodes != null && _nodes.Count > 0;
                }
            }

            public TrieNode(string value)
            {
                Value = value;
            }

            public void AddNode(string value, int start, StringComparison comparison)
            {
                var indexOfDot = value.IndexOf('.', start);
                if (indexOfDot < 0)
                {
                    var segment = value.Substring(start);
                    AddChildNode(segment, comparison);
                }
                else
                {
                    var segment = value.Substring(start, indexOfDot - start);
                    var node = AddChildNode(segment, comparison);
                    node.AddNode(value, indexOfDot + 1, comparison);
                }
            }

            private TrieNode AddChildNode(string value, StringComparison comparison)
            {
                var node = Nodes.Find(n => n.Value.Equals(value, comparison));
                if (node == null)
                {
                    node = new TrieNode(value);
                    Nodes.Add(node);
                }

                return node;
            }

            public bool Contains(string path, int start, StringComparison comparison)
            {
                if (!HasNodes)
                {
                    return false;
                }

                var indexOfDot = path.IndexOf('.', start);
                if (indexOfDot < 0)
                {
                    var segment = path.Substring(start);
                    return Nodes.Any(n => n.Value.Equals(segment, comparison));
                }
                else
                {
                    var segment = path.Substring(start, indexOfDot - start);

                    foreach (var node in Nodes.Where(n => n.Value.Equals(segment, comparison)))
                    {
                        var nextStart = start + segment.Length + 1;
                        if (nextStart >= path.Length)
                        {
                            return true;
                        }
                        else if (node.Contains(path, nextStart, comparison))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            public IEnumerable<string> Paths()
            {
                if (!HasNodes)
                {
                    if (Value == null)
                    {
                        yield break;
                    }

                    yield return Value;
                }

                foreach (var node in Nodes)
                {
                    foreach (var subpath in node.Paths())
                    {
                        if (Value == null)
                        {
                            yield return subpath;
                        }

                        yield return String.Concat(Value, ".", subpath);
                    }
                }
            }

            public override string ToString()
            {
                return Value;
            }
        }
    }
}
