//namespace Chapter02
//{
//    public static class HuffmanCoding
//    {
//        public static object Huffman(string text)
//        {
//            var charFrequenciesMap = ComputeFrequencies(text);
//            var priorityQueue = new DHeap_v1();
//            foreach (var (chr, frequency) in charFrequenciesMap)
//            {
//                priorityQueue.Insert(frequency, new string(new[] { chr }));
//            }

//            while (priorityQueue.Size > 1)
//            {
//                var left = priorityQueue.Top();
//                var right = priorityQueue.Top();
//                var parent = new Pair(left.Element + right.Element, left.Priority + right.Priority);

//                parent.Left = left;
//                parent.Right = right;
//                priorityQueue.Insert(par);
//            }

//            return text;
//        }

//        public static Dictionary<char, int> ComputeFrequencies(string text)
//        {
//            var frequencies = new Dictionary<char, int>();
//            for (var i = 0; i < text.Length; i++)
//            {
//                if (!frequencies.ContainsKey(text[i]))
//                {
//                    frequencies.Add(text[i], 1);
//                }
//                else
//                {
//                    frequencies[text[i]] += 1;
//                }
//            }

//            return frequencies;
//        }
//    }
//}
