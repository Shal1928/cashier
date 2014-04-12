using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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

        //public static T Clone<T>(this T source) where T : class
        //{
        //    var sourceType = source.GetType().UnderlyingSystemType;
        //    var sourceTypeConstructor = sourceType.GetConstructor(new[] { typeof(Int32) });
        //    var newInstance = sourceTypeConstructor.Invoke(new object[] { -2 }) as T;

        //    var nonPublicFields = source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        //    var publicFields = source.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (var field in nonPublicFields)
        //    {
        //        var value = field.GetValue(source);
        //        field.SetValue(newInstance, value);
        //    }
        //    foreach (var field in publicFields)
        //    {
        //        var value = field.GetValue(source);
        //        field.SetValue(newInstance, value);
        //    }
        //    return newInstance;
        //}
    }
}
