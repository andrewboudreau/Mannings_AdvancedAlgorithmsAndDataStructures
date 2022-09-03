// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation", 
    Justification = "Exception handling message.", Scope = "member", Target = "~M:Chapter00.Heap.DHeap`1.#ctor(System.Collections.Generic.IEnumerable{System.ValueTuple{`0,System.Int32}},System.Int32)")]
