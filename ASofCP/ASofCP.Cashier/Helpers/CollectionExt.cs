using System.Collections;

namespace ASofCP.Cashier.Helpers
{
    public static class CollectionExt
    {
        public static bool NotEmpty(this IEnumerable enumerable)
        {
            return enumerable != null && enumerable.GetEnumerator().MoveNext();
        }

        public static bool IsEmpty(this IEnumerable enumerable)
        {
            return !enumerable.GetEnumerator().MoveNext();
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            return enumerable == null || !enumerable.GetEnumerator().MoveNext();
        }
    }
}
