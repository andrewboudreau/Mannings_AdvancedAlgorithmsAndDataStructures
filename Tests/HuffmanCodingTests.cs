using Chapter02;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

namespace Tests
{
    [TestClass]
    /// <remarks> reference https://github.com/mlarocca/AlgorithmsAndDataStructuresInAction/blob/7a5b7a7a2b84257c99c28f6b92e47141f844afc9/Python/mlarocca/tests/dway_heap_tests.py </remarks>
    public class HuffmanCodingTests
    {
        public const string HuffmanTestString = "fffeeeeeddddddcccccccbbbbbbbbbbbbbbbbbbbbbbaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        [TestMethod]
        public void TestHuffmanCoding()
        {
            var encoding = HuffmanCoding.Huffman(HuffmanTestString);

            encoding['a'].Should().Be("0");
            encoding['b'].Should().Be("10");
            encoding['c'].Should().Be("1100");
            encoding['d'].Should().Be("1101");
            encoding['e'].Should().Be("1110");
            encoding['f'].Should().Be("1111");
        }

        [TestMethod]
        public void ComputeFrequencies()
        {
            var freq = HuffmanCoding.ComputeFrequencies(HuffmanTestString);

            freq['a'].Should().Be(57);
            freq['b'].Should().Be(22);
            freq['c'].Should().Be(7);
            freq['d'].Should().Be(6);
            freq['e'].Should().Be(5);
            freq['f'].Should().Be(3);

            foreach (var (key, value) in freq)
            {
                Console.WriteLine($"key:{key} freq:{value}");
            }
        }

        [TestMethod]
        public void FrequencyTableToHeap()
        {
            var frequencies = HuffmanCoding.ComputeFrequencies(HuffmanTestString);
            var heap = HuffmanCoding.FrequencyToHeap(frequencies);

            heap.Validate().Should().BeTrue();
            heap.Print();

            var node = heap.Top();
            node.Element.Value.Should().Be("f");
            node.Element.Frequency.Should().Be(-3);

            node = heap.Top();
            node.Element.Value.Should().Be("e");
            node.Element.Frequency.Should().Be(-5);

            node = heap.Top();
            node.Element.Value.Should().Be("d");
            node.Element.Frequency.Should().Be(-6);

            node = heap.Top();
            node.Element.Value.Should().Be("c");
            node.Element.Frequency.Should().Be(-7);

            node = heap.Top();
            node.Element.Value.Should().Be("b");
            node.Element.Frequency.Should().Be(-22);

            node = heap.Top();
            node.Element.Value.Should().Be("a");
            node.Element.Frequency.Should().Be(-57);
        }

        [TestMethod]
        public void HeapToTree()
        {
            var freq = HuffmanCoding.ComputeFrequencies(HuffmanTestString);
            var heap = HuffmanCoding.FrequencyToHeap(freq);
            var tree = HuffmanCoding.HeapToTree(heap);

            tree.Should().NotBeNull();
            tree.Validate().Should().BeTrue();

            tree.Value.Should().Be("abcdef");
            tree.Frequency.Should().Be(-100);

            tree.Left?.Should().NotBeNull();
            tree.Left?.Value.Should().Be("a");
            tree.Left?.Frequency.Should().Be(-57);

            tree.Right?.Should().NotBeNull();
            tree.Right?.Value.Should().Be("bcdef");
            tree.Right?.Frequency.Should().Be(tree.Frequency - tree.Left?.Frequency);

            tree.Left?.Left?.Should().NotBeNull();
            tree.Left?.Left?.Value.Should().Be("b");
            tree.Left?.Left?.Frequency.Should().Be(tree.Frequency - tree.Right?.Frequency);

            tree.Right?.Right?.Should().NotBeNull();
            tree.Right?.Right?.Value.Should().Be("cdef");
            tree.Right?.Right?.Frequency.Should().Be(tree.Frequency - tree.Left?.Frequency - tree.Right?.Left?.Frequency);
        }
    }
}