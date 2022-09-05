

using System.Xml.Linq;

namespace Chapter00.Treaps
{
    public class Node
    {
        public Node(object key, double priority, Node? left = default, Node? right = default)
        {
            Key = key;
            Priority = priority;
            Left = left;
            Right = right;
        }

        public object Key { get; private set; }

        public double Priority { get; set; }

        public Node? Left { get; private set; }

        public Node? Right { get; private set; }

        public Node? Parent { get; set; }

        public void SetLeftNode(Node? node)
        {
            Left = node;
            if (node is not null)
            {
                node.Parent = this;
            }
        }

        public void SetRightNode(Node? node)
        {
            Right = node;
            if (node is not null)
            {
                node.Parent = this;
            }
        }
    }

    public class Treap
    {
        public Treap()
        {
            Root = default;
        }

        public Node? Root { get; set; }
    }
}
