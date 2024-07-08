// See https://aka.ms/new-console-template for more information

using System;
using System.Linq;
using Fujiberg.Identifiers;
using Test;

Console.WriteLine("Hello, World!");
Console.WriteLine(string.Join(",", typeof(Something).CustomAttributes.Select(a => a.AttributeType.Name)));

namespace Test
{
    [TypedId]
    internal partial record struct Something
    {
    }
}
