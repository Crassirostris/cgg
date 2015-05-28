using System;
using System.Collections.Generic;
using System.Linq;

namespace CGG.Core
{
    public class CombinedTriangleProvider : ITriangleProvider
    {
        private readonly ITriangleProvider[] providers;

        public CombinedTriangleProvider(params ITriangleProvider[] providers)
        {
            this.providers = providers;
        }

        public IEnumerable<Triangle> Produce()
        {
            return providers.SelectMany(provider => provider.Produce());
        }
    }

    public class FunctionTriangleProvider : ITriangleProvider
    {

        public FunctionTriangleProvider(float x1, float x2, float y1, float y2, float tick, Func<float, float, float> func)
        {
        }

        public IEnumerable<Triangle> Produce()
        {
            throw new System.NotImplementedException();
        }
    }
}