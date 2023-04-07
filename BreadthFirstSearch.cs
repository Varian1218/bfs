using System;
using System.Collections.Generic;
using System.Linq;
using Numerics;

namespace BreadthFirstSearches
{
    public class BreadthFirstSearch
    {
        private class Node
        {
            public Node Parent;
            public Int3 Position;
        }

        private Queue<Node> _frontier;
        private Queue<Node> _nodes;
        private HashSet<Int3> _visited;

        public int Size
        {
            set
            {
                _frontier = new Queue<Node>(value);
                _nodes = new Queue<Node>(Enumerable.Repeat(true, value).Select(_ => new Node()));
                _visited = new HashSet<Int3>(value);
            }
        }

        public void Clear()
        {
            while (_frontier.Count > 0)
            {
                _nodes.Enqueue(_frontier.Dequeue());
            }
        }

        public bool TrySearch(Int3 des, Func<Int3, bool> isWalkable, out int length, ref Int3[] path, Int3 src)
        {
            if (des == src)
            {
                length = 1;
                path[0] = des;
                return true;
            }

            _frontier.Enqueue(new Node
            {
                Position = src
            });
            _visited.Add(src);
            while (_frontier.Count > 0)
            {
                var current = _frontier.Dequeue();
                foreach (var dir in Int3.FourDirs)
                {
                    var position = current.Position + dir;
                    if (!isWalkable(position)) continue;
                    if (_visited.Contains(position)) continue;
                    if (position == des)
                    {
                        path[0] = des;
                        length = 1;
                        while (current.Parent != null)
                        {
                            path[length] = current.Position;
                            current = current.Parent;
                            length++;
                        }

                        Clear();
                        return true;
                    }

                    _visited.Add(position);
                    var node = _nodes.Dequeue();
                    node.Parent = current;
                    node.Position = position;
                    _frontier.Enqueue(node);
                }
            }

            length = -1;
            Clear();
            return false;
        }
    }
}