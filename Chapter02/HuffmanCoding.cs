﻿using Chapter00.Heap;

namespace Chapter02
{
    public class TreeNode
    {
        public TreeNode(string characters, int frequency)
        {
            Characters = characters;
            Frequency = frequency;
        }

        public TreeNode? Left;
        public TreeNode? Right;
        public string Characters;
        public int Frequency;

        public override string ToString()
        {
            return $"freq:{Frequency}, {Characters} - Left: {Left}, Right: {Right}";
        }
    }

    /// <summary>
    /// Huffman's Algorithm for data compression.
    /// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/341
    /// </summary>
    /// <remarks>2.8.3 Data compression: Huffman codes</remarks>
    public static class HuffmanCoding
    {
        public const string HuffmanTestString = "ffeeeeeddddddcccccccbbbbbbbbbbbbbbbbbbbbaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        public static object Huffman(string text)
        {
            var priorityQueue = new DHeap<TreeNode>();
            var charFrequenciesMap = ComputeFrequencies(text);
            foreach (var (chars, frequency) in charFrequenciesMap)
            {
                priorityQueue.Insert(new TreeNode(chars.ToString(), frequency), frequency);
            }

            while (priorityQueue.Size > 1)
            {
                var left = priorityQueue.Top();
                var right = priorityQueue.Top();

                var parent = new TreeNode(
                    left.Element.Characters + right.Element.Characters,
                    left.Priority + right.Priority)
                {
                    Left = left.Element,
                    Right = right.Element
                };

                priorityQueue.Insert(parent, parent.Frequency);
            }

            priorityQueue.Print();
            return 1;
            //return BuildTable(priorityQueue.Top());
        }

        /// <summary>
        /// The Huffman coding algorithm (building a table from the tree)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// <code>
        /// function buildTable(node, sequence, charactersToSequenceMap)
        ///   if node.characters.size == 1 then
        ///     charactersToSequenceMap[node.characters[0]] ← sequence 
        ///   else
        ///     if node.left <> null then
        ///       buildTable(node.left, 0 + sequence, charactersToSequenceMap) 
        ///     if node.right <> null then
        ///       buildTable(node.right, 1 + sequence, charactersToSequenceMap) 
        ///   return charactersToSequenceMap
        /// </code>
        /// </remarks>
        private static object BuildTable(TreeNode node, List<char> sequence, Dictionary<string, string> charactersToSequenceMap)
        {
            if (node.Characters.Length == 1)
            {
                //charactersToSequenceMap[node.Characters[0]] = sequence;
            }
            return true;
        }

        public static Dictionary<char, int> ComputeFrequencies(string text)
        {
            var frequencies = new Dictionary<char, int>();
            for (var i = 0; i < text.Length; i++)
            {
                if (!frequencies.ContainsKey(text[i]))
                {
                    frequencies.Add(text[i], 1);
                }
                else
                {
                    frequencies[text[i]] += 1;
                }
            }

            return frequencies;
        }
    }
}
