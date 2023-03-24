using System.Collections.Generic;

namespace BFS.Utils
{
    public static class BfsUtils
    {
        private class Node
        {
            public Node Parent;
            public (int X, int Y) Position;
        }

        public static int FindPath((int, int) des, (int, int) src, bool[,] matrix, ref (int, int)[] path)
        {
            if (des == src)
            {
                path[0] = des;
                return 1;
            }

            var frontier = new Queue<Node>();
            var height = matrix.GetLength(1);
            var length = -1;
            var visited = new HashSet<(int, int)>();
            var width = matrix.GetLength(0);
            frontier.Enqueue(new Node
            {
                Position = src
            });
            visited.Add(src);
            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                bool Check((int X, int Y) position, ref (int, int)[] path)
                {
                    if (position.X < 0 ||
                        position.Y < 0 ||
                        position.X >= width ||
                        position.Y >= height) return false;
                    if (matrix[position.X, position.Y]) return false;
                    if (visited.Contains(position)) return false;
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

                        return true;
                    }

                    visited.Add(position);
                    frontier.Enqueue(new Node
                    {
                        Parent = current,
                        Position = position
                    });
                    return false;
                }

                if (Check((current.Position.X - 1, current.Position.Y), ref path) ||
                    Check((current.Position.X + 1, current.Position.Y), ref path) ||
                    Check((current.Position.X, current.Position.Y - 1), ref path) ||
                    Check((current.Position.X, current.Position.Y + 1), ref path)) return length;
            }

            return length;
        }
    }
}