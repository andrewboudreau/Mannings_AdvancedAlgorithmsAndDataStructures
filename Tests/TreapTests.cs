/// <remarks>reference https://raw.githubusercontent.com/mlarocca/AlgorithmsAndDataStructuresInAction/refs/heads/master/Java/tests/org/mlarocca/containers/treap/TreapTest.java </remarks>
using Chapter00;
using Chapter00.Treaps;

namespace Tests;

[TestClass]
public class TreapTest
{
    private static readonly Random Rnd = new Random();

    [TestMethod]
    public void Top_EmptyTreap_ReturnsEmptyOptional()
    {
        var treap = new Treap<string, double>();
        var result = treap.Top();

        result.Should().BeNull("top() should return null when the treap is empty");
    }

    [TestMethod]
    public void Top_SingleElement_ReturnsElement()
    {
        var treap = new Treap<string, double>();
        treap.Insert(new TreapEntry<string, double>("primo", 1.0));
        var result = treap.Top();

        result.Should().NotBeNull("top() should return a value when the treap is not empty");
        Assert.AreEqual("primo", result!.Value, "top() should return the only element in the treap");
    }

    [TestMethod]
    public void Top_MultipleElements_ReturnsHighestPriority()
    {
        var treap = new Treap<string, double>();
        treap.Insert(new TreapEntry<string, double>("primo", 1.0));
        treap.Insert(new TreapEntry<string, double>("primo", 1.0));
        treap.Insert(new TreapEntry<string, double>("secondo", -1.0));
        treap.Insert(new TreapEntry<string, double>("a", 11.0));
        treap.Insert(new TreapEntry<string, double>("b", 0.0));
        treap.Insert(new TreapEntry<string, double>("c", -0.99));

        var result = treap.Top();
        Assert.AreEqual("secondo", result.Value, "top() should return the highest priority element in the treap");

        // Test with random elements
        for (int i = 0; i < 10; i++)
        {
            treap.Insert(new TreapEntry<string, double>(Rnd.Next().ToString(), Rnd.NextDouble()));
            Assert.IsTrue(treap.CheckTreapInvariants(treap.Root));
        }

        while (!treap.IsEmpty())
        {
            Assert.IsTrue(treap.CheckTreapInvariants(treap.Root));
            treap.Top();
        }
    }

    //[TestMethod]
    //public void Peek_EmptyTreap_ReturnsNull()
    //{
    //    var treap = new Treap<string, double>();
    //    var result = treap.Peek();

    //    Assert.IsFalse(result.HasValue, "peek() should return null when the treap is empty");
    //}

    //[TestMethod]
    //public void Peek_WithElements_ReturnsHighestPriority()
    //{
    //    var treap = new Treap<string, double>();
    //    treap.Add(new TreapEntry<string, double>("primo", 1e14));
    //    var result = treap.Peek();

    //    Assert.IsTrue(result.HasValue, "peek() should return a value when the treap is not empty");
    //    Assert.AreEqual("primo", result.Value.Key, "peek() should return the only element in the treap");

    //    treap.Add(new TreapEntry<string, double>("b", 0.0));
    //    treap.Add(new TreapEntry<string, double>("c", -0.99));
    //    treap.Add(new TreapEntry<string, double>("secondo", -1.0));
    //    treap.Add(new TreapEntry<string, double>("a", 11.0));

    //    result = treap.Peek();
    //    Assert.AreEqual("secondo", result.Value.Key, "peek() should return the highest priority element in the treap");
    //}

    //[TestMethod]
    //public void Add_MultipleElements_SuccessfullyAdds()
    //{
    //    var treap = new Treap<string, double>();
    //    Assert.AreEqual(0, treap.Size);

    //    Assert.IsTrue(treap.Add(new TreapEntry<string, double>("d", 1.0)));
    //    Assert.AreEqual(1, treap.Size);
    //    Assert.IsTrue(treap.Contains(new TreapEntry<string, double>("d", 1.0)));

    //    Assert.IsTrue(treap.Add(new TreapEntry<string, double>("c", 2.0)));
    //    Assert.AreEqual(2, treap.Size);
    //    Assert.IsTrue(treap.Contains(new TreapEntry<string, double>("c", 2.0)));

    //    // Test duplicates
    //    Assert.IsTrue(treap.Add(new TreapEntry<string, double>("c", 2.0)));
    //    Assert.AreEqual(3, treap.Size);
    //    Assert.IsTrue(treap.Contains(new TreapEntry<string, double>("c", 2.0)));
    //}

    //[TestMethod]
    //public void UpdatePriority_ValidUpdate_Succeeds()
    //{
    //    var keys = new[] { "a", "b", "c", "d", "e", "f", "g" };
    //    var priorities = Enumerable.Range(0, 7).ToList();
    //    var treap = InitTreap(keys, priorities);

    //    Assert.IsFalse(treap.UpdatePriority(
    //        new TreapEntry<string, int>("d", 2),
    //        new TreapEntry<string, int>("d", 1)),
    //        "Should return false for keys not in the treap");

    //    Assert.IsTrue(treap.UpdatePriority(
    //        new TreapEntry<string, int>("b", 1),
    //        new TreapEntry<string, int>("b", 7)),
    //        "Should return true for legitimate update of an existing key's priority");

    //    Assert.IsTrue(treap.CheckTreapInvariants(), "Update Priority shouldn't mess treap up");
    //    Assert.IsFalse(treap.Contains(new TreapEntry<string, int>("b", 1)),
    //        "After updatePriority the old element should not be in the heap");
    //    Assert.IsTrue(treap.Contains(new TreapEntry<string, int>("b", 7)),
    //        "After updatePriority the new element should be in the heap");
    //}

    //[TestMethod]
    //[ExpectedException(typeof(ArgumentException))]
    //public void UpdatePriority_DifferentKeys_ThrowsException()
    //{
    //    var treap = new Treap<string, int>();
    //    treap.Add(new TreapEntry<string, int>("a", 0));
    //    // Should throw if the keys don't match
    //    treap.UpdatePriority(
    //        new TreapEntry<string, int>("a", 2),
    //        new TreapEntry<string, int>("c", 2));
    //}

    //[TestMethod]
    //public void Remove_ExistingElements_SuccessfullyRemoves()
    //{
    //    var keys = Enumerable.Range(0, 9).ToList();
    //    var priorities = keys.Select(_ => Rnd.NextDouble()).ToList();
    //    var treap = InitTreap(keys, priorities);

    //    Assert.AreEqual(keys.Count, treap.Size);

    //    // Shuffle keys for random removal order
    //    var shuffledKeys = keys.OrderBy(_ => Rnd.Next()).ToList();
    //    foreach (var key in shuffledKeys)
    //    {
    //        int size = treap.Size;
    //        Assert.IsTrue(treap.Remove(new TreapEntry<int, double>(key, priorities[key])),
    //            "Remove should succeed");
    //        Assert.AreEqual(size - 1, treap.Size, "Treap's size should decrease by 1");
    //        Assert.IsFalse(treap.Contains(new TreapEntry<int, double>(key, priorities[key])),
    //            "Element should have been removed");
    //    }
    //}

    //[TestMethod]
    //public void Clear_RemovesAllElements()
    //{
    //    var treap = new Treap<int, double>();
    //    int numElements = 5 + Rnd.Next(10);

    //    for (int i = 0; i < numElements; i++)
    //    {
    //        Assert.IsTrue(treap.Add(new TreapEntry<int, double>(i, Rnd.NextDouble())));
    //    }

    //    Assert.AreEqual(numElements, treap.Size);
    //    treap.Clear();
    //    Assert.AreEqual(0, treap.Size);
    //    Assert.IsTrue(treap.IsEmpty);

    //    treap.Add(new TreapEntry<int, double>(1, 0.0));
    //    Assert.AreEqual(1, treap.Size);
    //    Assert.IsFalse(treap.IsEmpty);
    //}

    //private Treap<K, P> InitTreap<K, P>(IEnumerable<K> keys, IEnumerable<P> priorities)
    //    where K : IComparable<K>
    //    where P : IComparable<P>
    //{
    //    var keysList = keys.ToList();
    //    var prioritiesList = priorities.ToList();

    //    if (keysList.Count != prioritiesList.Count)
    //    {
    //        throw new ArgumentException("Both collections must have the same length");
    //    }

    //    var treap = new Treap<K, P>();
    //    for (int i = 0; i < keysList.Count; i++)
    //    {
    //        treap.Add(new TreapEntry<K, P>(keysList[i], prioritiesList[i]));
    //    }
    //    return treap;
    //}
}
