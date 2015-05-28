using System;
using System.Collections.Generic;

namespace CGG.Core
{
    public class FunctionTriangleProvider : ITriangleProvider
    {
        private readonly float x1;
        private readonly float x2;
        private readonly float y1;
        private readonly float y2;
        private readonly float tick;
        private readonly Func<float, float, float> func;

        public FunctionTriangleProvider(float x1, float x2, float y1, float y2, float tick, Func<float, float, float> func)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            this.tick = tick;
            this.func = func;
        }

        public IEnumerable<Triangle> Produce()
        {
            throw new NotImplementedException();
        }
    }
}