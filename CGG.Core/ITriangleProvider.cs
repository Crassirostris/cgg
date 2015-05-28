using System.Collections.Generic;

namespace CGG.Core
{
    public interface ITriangleProvider
    {
        IEnumerable<Triangle> Produce();
    }
}