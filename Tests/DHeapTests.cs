using Chapter00.Heap;

using System.Globalization;
using System.Reflection;

namespace Tests
{
    [TestClass]
    /// <remarks>reference https://github.com/mlarocca/AlgorithmsAndDataStructuresInAction/blob/7a5b7a7a2b84257c99c28f6b92e47141f844afc9/Python/mlarocca/tests/dway_heap_tests.py </remarks>
    public class DHeapTests
    {
        [TestMethod]
        public void Insert_Figure2_9()
        {
            var heap = new DHeap<string>(3,
                ("Unencrypted password on DB", 100),
                ("UI breaks on browser X", 90),
                ("Memory leak", 90),
                ("CSS style causes misalignment", 80),
                ("Page loads take 2+ seconds", 70),
                ("CSS style causes 1px offset", 50),
                ("Refactor CSS using SASS", 30),
                ("Optional form field blocked", 80));

            heap.Insert("Add exception for superbowl", 95);

            heap.Top().Priority.Should().Be(100);
            heap.Top().Priority.Should().Be(95);
            heap.Top().Priority.Should().Be(90);
            heap.Top().Priority.Should().Be(90);
            heap.Top().Priority.Should().Be(80);
            heap.Top().Priority.Should().Be(80);
            heap.Top().Priority.Should().Be(70);
            heap.Top().Priority.Should().Be(50);
            heap.Top().Priority.Should().Be(30);
        }

        [DataTestMethod]
        [BranchingFactorDataSource]
        public void Peek(int branchingFactor)
        {
            var heap = new DHeap<string>(branchingFactor);
            heap.Insert("First", int.MinValue);
            heap.Peek().Element.Should().Be("First", because: "only one item exists");
            heap.Top().Element.Should().Be("First", because: "only one item exists");

            heap.Insert("b", 0);
            heap.Insert("second", 2);
            heap.Insert("c", 1);
            heap.Insert("a", -11);

            heap.Peek().Element.Should().Be("second", because: "second has the highest priority");
            heap.Peek().Element.Should().Be("second", because: "second has the highest priority");
        }

        [DataTestMethod]
        [BranchingFactorDataSource]
        public void Insert(int branchingFactor)
        {
            var heap = new DHeap<string>(branchingFactor);
            heap.Insert("First", int.MinValue);
            heap.Top().Element.Should().Be("First", because: "only one item exists");
            heap.Size.Should().Be(0, because: "heap is empty");
            heap.Validate();

            heap.Insert("c", 1);
            heap.Validate();
            
            heap.Insert("a", -11);
            heap.Validate();
            
            heap.Insert("second", 225);
            heap.Validate();
            
            heap.Insert("b", 23);
            heap.Validate();
            
            heap.Insert("d", 72);
            heap.Validate();
            
            heap.Insert("e", 145); 
            heap.Validate();

            heap.Top().Element.Should().Be("second", because: "second has the highest priority");
            heap.Size.Should().Be(5, because: "6 total and 1 are removed");
            heap.Validate();

            heap.Top().Element.Should().Be("e", because: "has the next highest priority");
            heap.Validate(); 
            
            heap.Top().Element.Should().Be("d", because: "has the next highest priority");
            heap.Validate(); 
            
            heap.Top().Element.Should().Be("b", because: "has the next highest priority");
            heap.Validate(); 
            
            heap.Top().Element.Should().Be("c", because: "has the next highest priority");
            heap.Validate(); 
            
            heap.Top().Element.Should().Be("a", because: "has the next highest priority");
            heap.Validate();
        }

        [DataTestMethod]
        [BranchingFactorDataSource]
        public void InsertTen_Top_ShouldReturnPriorityOrder(int branchingFactor)
        {
            var heap = new DHeap<string>(branchingFactor);
            heap.Insert("First", int.MinValue);
            heap.Top().Priority.Should().Be(int.MinValue, because: "only one item exists");
            heap.Size.Should().Be(0, because: "heap is empty");

            heap.Insert("a", 48);
            heap.Insert("b", 15);
            heap.Insert("c", 96);
            heap.Insert("d", 18);
            heap.Insert("e", 98);
            heap.Insert("f", 6);
            heap.Insert("g", 25);
            heap.Insert("h", 58);
            heap.Insert("i", 67);
            heap.Insert("j", 85);

            heap.Top().Priority.Should().Be(98, because: "98 is the highest priority");
            heap.Size.Should().Be(9, because: "10 total and 1 are removed");

            heap.Top().Priority.Should().Be(96, because: "is the next highest priority");
            heap.Top().Priority.Should().Be(85, because: "is the next highest priority");
            heap.Top().Priority.Should().Be(67, because: "is the next highest priority");
            heap.Top().Priority.Should().Be(58, because: "is the next highest priority");
            heap.Top().Priority.Should().Be(48, because: "is the next highest priority");
            heap.Top().Priority.Should().Be(25, because: "is the next highest priority");
            heap.Top().Priority.Should().Be(18, because: "is the next highest priority");
            heap.Top().Priority.Should().Be(15, because: "is the next highest priority");
            heap.Top().Priority.Should().Be(6, because: "is the next highest priority");
        }

        [DataTestMethod]
        [BranchingFactorDataSource]
        public void InsertTen_TopK_ShouldReturnPriorityOrder(int branchingFactor)
        {
            var heap = new DHeap<string>(branchingFactor);
            heap.Size.Should().Be(0, because: "heap is empty");

            heap.Insert("a", 48);
            heap.Insert("b", 15);
            heap.Insert("c", 96);
            heap.Insert("d", 18);
            heap.Insert("e", 98);
            heap.Insert("f", 6);
            heap.Insert("g", 25);
            heap.Insert("h", 58);
            heap.Insert("i", 67);
            heap.Insert("j", 85);

            var results = heap.TopK(10);
            results.Top().Element.Should().Be("e", because: "e has the highest priority");
            results.Top().Element.Should().Be("c", because: "has the next highest priority");
            results.Top().Element.Should().Be("j", because: "has the next highest priority");
            results.Top().Element.Should().Be("i", because: "has the next highest priority");
            results.Top().Element.Should().Be("h", because: "has the next highest priority");
            results.Top().Element.Should().Be("a", because: "has the next highest priority");
            results.Top().Element.Should().Be("g", because: "has the next highest priority");
            results.Top().Element.Should().Be("d", because: "has the next highest priority");
            results.Top().Element.Should().Be("b", because: "has the next highest priority");
            results.Top().Element.Should().Be("f", because: "has the next highest priority");
        }

        [DataTestMethod]
        [BranchingFactorDataSource]
        public void Randomized_Insert(int branchingFactor)
        {
            var heap = new DHeap<string>(branchingFactor);

            foreach (int i in Enumerable.Range(0, 1000))
            {
                var e = ('a' + i).ToString();
                var p = Random.Shared.Next(0, 100);
                Console.WriteLine($"Inserting '{e}', {p}");

                heap.Insert(e, p);
            }

            var previous = heap.Top().Priority;
            while (heap.Size > 0)
            {
                var next = heap.Top().Priority;
                next.Should().BeLessThanOrEqualTo(previous, because: "priority should be going down");
                heap.Validate();
                previous = next;
            }
        }

        [DataTestMethod]
        [BranchingFactorDataSource]
        public void Randomized_Heapify(int branchingFactor)
        {
            var tuples = Enumerable.Range(0, 1000)
                .Select(x => (((char)('a' + x)).ToString(), Random.Shared.Next(0, 100)))
                .ToArray();

            var heap = new DHeap<string>(branchingFactor, tuples);
            heap.Validate();

            var previous = heap.Top().Priority;
            while (heap.Size > 0)
            {
                var next = heap.Top().Priority;
                next.Should().BeLessThanOrEqualTo(previous, because: "priority should be going down");
                heap.Validate();

                previous = next;
            }
        }
    }

    public class BranchingFactorDataSourceAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[] { 2 };
            yield return new object[] { 3 };
            yield return new object[] { 4 };
            yield return new object[] { 5 };
            yield return new object[] { 6 };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null)
                return string.Format(CultureInfo.CurrentCulture, "{0} {1}ary-Heap", methodInfo.Name, data[0]);

            return "";
        }
    }
}