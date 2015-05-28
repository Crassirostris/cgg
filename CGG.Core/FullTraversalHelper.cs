using System.Collections.Generic;
using System.Linq;

namespace CGG.Core
{
    public static class FullTraversalHelper
    {
        public static IEnumerable<List<int>> FullTraversal(int setCount, int subsetCount)
        {
            return FullTraversal(setCount, subsetCount, new List<int>(), Enumerable.Repeat(false, setCount).ToList());
        }

        private static IEnumerable<List<int>> FullTraversal(int setCount, int subsetCount, List<int> currentSet, List<bool> used)
        {
            if (currentSet.Count == subsetCount)
            {
                yield return currentSet.ToList();
                yield break;
            }
            for (int i = 0; i < setCount; i++)
            {
                if (used[i])
                    continue;
                currentSet.Add(i);
                used[i] = true;
                foreach (var subset in FullTraversal(setCount, subsetCount, currentSet, used))
                    yield return subset;
                used[i] = false;
                currentSet.RemoveAt(currentSet.Count - 1);
            }
        }
    }
}