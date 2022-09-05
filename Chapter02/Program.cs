using Chapter00.Heap;

using Chapter02;

using System.Numerics;

// Resources
// https://github.com/mlarocca/AlgorithmsAndDataStructuresInAction/blob/7a5b7a7a2b84257c99c28f6b92e47141f844afc9/Python/mlarocca/datastructures/heap/dway_heap.py#L169
// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/184

// Figure 2.6
var heap = new DHeap<string>(
    ("Unencrypted password on DB", 100),
    ("UI breaks on browser X", 90),
    ("Optional form field blocked", 80),
    ("CSS style causes misalignment", 80),
    ("Page loads take 2+ seconds", 70),
    ("CSS style causes 1px offset", 50),
    ("Refactor CSS using SASS", 30),
    ("Memory leak", 90));

Console.WriteLine("BubbleUp");
heap.Print();
heap.BubbleUp(heap.Size - 1);
heap.Print();

//-------------------------------------------------------------------------
// Figure 2.8
Console.WriteLine();
Console.WriteLine("PushDown");
heap = new DHeap<string>(
    ("UI breaks on browser X", 90),
    ("Unencrypted password on DB", 100),
    ("Memory leak", 90),
    ("CSS style causes misalignment", 80),
    ("Page loads take 2+ seconds", 70),
    ("CSS style causes 1px offset", 50),
    ("Refactor CSS using SASS", 30),
    ("Optional form field blocked", 80));

heap.Print();
heap.PushDown();
heap.Print();


//--------------------------------------------------------------
// Figure 2.9
Console.WriteLine();
Console.WriteLine("Insert");
heap = new DHeap<string>(
    ("Unencrypted password on DB", 100),
    ("UI breaks on browser X", 90),
    ("Memory leak", 90),
    ("CSS style causes misalignment", 80),
    ("Page loads take 2+ seconds", 70),
    ("CSS style causes 1px offset", 50),
    ("Refactor CSS using SASS", 30),
    ("Optional form field blocked", 80));

heap.Print();
heap.Insert("Add exception for Super Bowl", 95);
heap.Print();

//--------------------------------------------------------------
// Fig 2.10
var top = heap.Top();
Console.WriteLine($"Top value is = {top}");
heap.Print();

// Update Method No Figure in book.
Console.WriteLine();
Console.WriteLine($"Updating task to priority 97");
heap.Update("CSS style causes 1px offset", 97);
heap.Print();

// Peek
Console.WriteLine();
Console.WriteLine("Peeking top");
var peek = heap.Peek();
Console.WriteLine($"Peeked the value {peek}");
heap.Print();
Console.WriteLine();
Console.WriteLine("TopK=6 are...");
var topK = heap.TopK(6);
topK.Print();


//--------------------------------------------------------------
// Fig 2.12
Console.WriteLine();
Console.WriteLine();
var huffmanString = "fffeeeeeddddddcccccccbbbbbbbbbbbbbbbbbbbbbbaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
Console.WriteLine($"Starting Huffman coding algorithm for '{huffmanString}'");
Console.WriteLine("Huffman Encoding Table:");
var encoding = HuffmanCoding.Huffman(huffmanString);
foreach (var (key, value) in encoding)
{
    Console.WriteLine($"  {key}: {value}");
}

Console.WriteLine();
foreach (var character in huffmanString)
{
    Console.Write(encoding[character]);
}
