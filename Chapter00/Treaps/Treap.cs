using System.Net.Http.Headers;

namespace Chapter00.Treaps;

public class Treap<TValue, TPriority>
    where TValue : IComparable<TValue>
    where TPriority : IComparable<TPriority>
{
    public Treap()
    {
        Root = default;
    }

    public TreapEntry<TValue, TPriority>? Root { get; set; }

    public void RotateRight(TreapEntry<TValue, TPriority> node)
    {
        ArgumentNullException.ThrowIfNull(node);
        if (node == Root || node.Parent is null)
        {
            throw new InvalidOperationException("Cannot rotate the root node to the right.");
        }

        var parent = node.Parent;
        if (parent.Left != node)
        {
            throw new InvalidOperationException("Cannot rotate a node to the right if it is not the left child of its parent.");
        }

        var grandParent = parent.Parent;
        if (grandParent is not null)
        {
            if (grandParent.Left == parent)
            {
                grandParent.SetLeft(node);
            }
            else
            {
                grandParent.SetRight(node);
            }
        }
        else
        {
            Root = node;
        }

        parent.SetLeft(node.Right);
        node.SetRight(parent);
    }

    void RotateLeft(TreapEntry<TValue, TPriority> node)
    {
        ArgumentNullException.ThrowIfNull(node);
        if (node == Root || node.Parent is null)
        {
            throw new InvalidOperationException("Cannot rotate the root node to the left.");
        }

        var parent = node.Parent;
        if (parent.Right != node)
        {
            throw new InvalidOperationException("Cannot rotate a node to the left if it is not the right child of its parent.");
        }

        var grandParent = parent.Parent;
        if (grandParent is not null)
        {
            if (grandParent.Left == parent)
            {
                grandParent.SetLeft(node);
            }
            else
            {
                grandParent.SetRight(node);
            }
        }
        else
        {
            Root = node;
        }
        parent.SetRight(node.Left);
        node.SetLeft(parent);
    }

    /// <summary>
    /// Inserts a new node with the given key and priority into the treap.
    /// </summary>
    /// <param name="key">The node's key.</param>
    /// <param name="priority">The nodes priority.</param>
    /// <remarks>Duplicates are allowed and are added to a node's left subtree.</remarks>
    public void Insert(TreapEntry<TValue, TPriority> entry)
    {
        TreapEntry<TValue, TPriority>? node = Root;
        TreapEntry<TValue, TPriority>? parent = null;
        var newNode = entry;

        // traverse the tree until we get to a null node 
        while (node is not null)
        {
            // update the parent node, since current node is not null
            parent = node;

            // check how the new key compares to the current node's key
            if (newNode.Value.CompareTo(node.Value) <= 0)
            {
                node = node.Left;
            }
            else
            {
                node = node.Right;
            }
        }

        // if the parent is null, then the tree is empty
        if (parent == null)
        {
            Root = newNode;
            return;
        }
        // check if we should add the new node to the left or right of the parent
        else if (newNode.Value.CompareTo(parent.Value) <= 0)
        {
            parent.SetLeft(newNode);
        }
        else
        {
            parent.SetRight(newNode);
        }

        // Either way, the new node is a leaf node, so we set its parent
        newNode.SetParent(parent);

        // We need to check heap's invariants. Until they are reinstated or we get to the root, we need to bubble up current node
        while (newNode.Parent != null && newNode.Priority.CompareTo(newNode.Parent.Priority) < 0)
        {
            // If the new node is the left child of its parent, we rotate it to the right
            if (newNode.Parent.Left == newNode)
            {
                RotateRight(newNode);
            }
            else
            {
                RotateLeft(newNode);
            }
        }

        // At the end of the cycle the newNode bubbled up to the root, we need to update the root property.
        if (newNode.Parent is null)
        {
            Root = newNode;
        }
    }

    /// <summary>
    /// Deletes the node with the given key from the treap.
    /// </summary>
    /// <param name="key">The key to be removed</param>
    /// <returns>True if the node was found and removed, false otherwise.</returns>
    public bool Remove(TValue key)
    {
        TreapEntry<TValue, TPriority>? node = Search(Root, key);
        if (node is null)
        {
            return false;
        }

        if (IsRoot(node) && IsLeaf(node))
        {
            Root = null;
            return true;
        }

        // push the node down the tree until it becomes a leaf
        while (!IsLeaf(node))
        {
            // check which of node's two children should replace it. Choose the one with the highest priority (lowest value; min-treap)
            if (node.Left is not null && (node.Right == null || node.Left.Priority.CompareTo(node.Right.Priority) > 0))
            {
                RotateRight(node.Left);
            }
            else if (node.Right is not null)
            {
                RotateLeft(node.Right);
            }

            // In case node was the root. we need to update treap's property. This check can only be true only on the first iteration of the cycle. 
            if (node.Parent is not null && IsRoot(node.Parent))
            {
                Root = node.Parent;
            }
        }

        // After exiting the while loop, node is now a leaf, and certainly not the root anymore.
        // We can just disconnect it by null the pointer from its parent.
        if (node.Parent!.Left == node)
        {
            node.Parent.SetLeft(null);
        }
        else
        {
            node.Parent.SetRight(null);
        }

        return true;
    }

    /// <summary>
    /// Returns and removes the key of the node with the highest priority in the treap.
    /// </summary>
    /// <returns></returns>
    public TreapEntry<TValue, TPriority>? Top()
    {
        if (Root is null)
        {
            return default;
        }

        var root = Root;
        Remove(Root.Value);
        return root;
    }

    public TValue? Peek()
    {
        if (Root is null)
        {
            return default;
        }

        return Root.Value;
    }

    public TValue? Min()
    {
        if (Root is null) return default;
        var node = Root;
        while (node.Left is not null)
        {
            node = node.Left;
        }

        return node.Value;
    }


    public TreapEntry<TValue, TPriority>? Search(TreapEntry<TValue, TPriority>? node, TValue targetKey)
    {
        if (node is null)
        {
            return null;
        }

        if (node.Value.CompareTo(targetKey) == 0)
        {
            return node;
        }
        else if (targetKey.CompareTo(node.Value) < 0)
        {
            return Search(node.Left, targetKey);
        }
        else
        {
            return Search(node.Right, targetKey);
        }
    }

    private static bool IsLeaf(TreapEntry<TValue, TPriority> node) => node.Left is null && node.Right is null;
    private static bool IsRoot(TreapEntry<TValue, TPriority> node) => node.Parent is null;
    
    public bool CheckTreapInvariants(TreapEntry<TValue, TPriority>? node)
    {
        if (node is null)
        {
            return true;
        }

        bool leftInvariant = node.Left is null ||
                             (node.Left.Priority.CompareTo(node.Priority) >= 0 && node.Left.Value.CompareTo(node.Value) <= 0);
        
        bool rightInvariant = node.Right is null ||
                              (node.Right.Priority.CompareTo(node.Priority) >= 0 && node.Right.Value.CompareTo(node.Value) > 0);

        return leftInvariant && rightInvariant && CheckTreapInvariants(node.Left) && CheckTreapInvariants(node.Right);
    }

    public bool IsEmpty()
    {
        return Root is null;
    }
}
