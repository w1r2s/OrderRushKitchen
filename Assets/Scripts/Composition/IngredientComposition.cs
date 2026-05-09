using System.Collections.Generic;

namespace Assets.Scripts.Composition
{
    public static class IngredientComposition
    {
        public static Dictionary<KitchenObjectSo, int> BuildCounts(IReadOnlyList<KitchenObjectSo> items, KitchenObjectSo addition = null)
        {
            var counts = new Dictionary<KitchenObjectSo, int>();

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    if (item == null) continue;

                    if (counts.TryGetValue(item, out var value))
                        counts[item] = value + 1;
                    else
                        counts[item] = 1;
                }
            }

            if (addition != null)
            {
                if (!counts.TryAdd(addition, 1))
                {
                    counts[addition]++;
                }
            }

            return counts;
        }

        public static bool AreEqual(Dictionary<KitchenObjectSo, int> left, Dictionary<KitchenObjectSo, int> right)
        {
            if (left == null || right == null)
                return false;

            if (left.Count != right.Count)
                return false;

            foreach (var keyValuePair in left)
            {
                if (!right.TryGetValue(keyValuePair.Key, out var rightCount))
                    return false;

                if (keyValuePair.Value != rightCount)
                    return false;
            }

            return true;
        }

        public static bool IsSubsetOf(Dictionary<KitchenObjectSo, int> subset, Dictionary<KitchenObjectSo, int> superset)
        {
            if (subset == null || superset == null)
                return false;

            foreach (var keyValuePair in subset)
            {
                if (!superset.TryGetValue(keyValuePair.Key, out var supersetValue))
                    return false;

                if (keyValuePair.Value > supersetValue)
                    return false;
            }

            return true;
        }

    }
}
