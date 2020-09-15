using System;
using System.Collections.Generic;

namespace Algorithms
{
    internal interface IRank<T> where T : IComparable<T>
    {
        int Priority { get; }
    }
}
