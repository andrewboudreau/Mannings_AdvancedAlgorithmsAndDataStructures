using Chapter00.Heap;

using System.Globalization;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class DHeapTests
    {
        //https://github.com/mlarocca/AlgorithmsAndDataStructuresInAction/blob/7a5b7a7a2b84257c99c28f6b92e47141f844afc9/Python/mlarocca/tests/dway_heap_tests.py

        public DHeapTests()
        {
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

            heap.Insert("c", 1);
            heap.Insert("a", -11);
            heap.Insert("second", 225);
            heap.Insert("b", 23);
            heap.Insert("d", 72);
            heap.Insert("e", 145);

            heap.Top().Element.Should().Be("second", because: "second has the highest priority");
            heap.Size.Should().Be(5, because: "6 total and 1 are removed");

            heap.Top().Element.Should().Be("e", because: "has the next highest priority");
            heap.Top().Element.Should().Be("d", because: "has the next highest priority");
            heap.Top().Element.Should().Be("b", because: "has the next highest priority");
            heap.Top().Element.Should().Be("c", because: "has the next highest priority");
            heap.Top().Element.Should().Be("a", because: "has the next highest priority");

            //foreach (int i in Enumerable.Range(0, 10))
            //{
            //    var p = Random.Shared.Next(0, 100);
            //    Console.WriteLine($"Inserting '{i}', {p}");
            //    heap.Insert(i.ToString(), p);
            //}

            //var previous = heap.Top().Priority;
            //while (heap.Size > 0)
            //{
            //    var next = heap.Top().Priority;
            //    next.Should().BeLessThanOrEqualTo(previous, because: "priority should be going down");
            //    previous = next;
            //}
        }

        [DataTestMethod]
        [BranchingFactorDataSource]
        public void RandomizedInsert(int branchingFactor)
        {
            var heap = new DHeap<string>(branchingFactor);

            foreach (int i in Enumerable.Range(0, 10))
            {
                var e = ((char)('a' + i)).ToString();
                var p = Random.Shared.Next(0, 100);
                Console.WriteLine($"Inserting '{e}', {p}");
                heap.Insert(e, p);
            }

            var previous = heap.Top().Priority;
            while (heap.Size > 0)
            {
                var next = heap.Top().Priority;
                next.Should().BeLessThanOrEqualTo(previous, because: "priority should be going down");
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