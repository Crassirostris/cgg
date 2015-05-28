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
}