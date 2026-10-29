using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Riskified.SDK.Utils
{
    /// <summary>
    /// Backs a bridge where a key the SDK sent as a single object is documented as an array: the
    /// public single property and the public array property share one list.
    /// </summary>
    internal static class SingleOrArray
    {
        /// <summary>
        /// The single property's getter: the first element, or null.
        /// </summary>
        public static T First<T>(List<T> items) where T : class
        {
            return items.Count > 0 ? items[0] : null;
        }

        /// <summary>
        /// The single property's setter: replaces the first element, adds it to an empty list, or
        /// removes it when value is null (so one address set to null sends nothing, as before).
        /// </summary>
        public static void SetFirst<T>(List<T> items, T value) where T : class
        {
            if (value == null)
            {
                if (items.Count > 0)
                {
                    items.RemoveAt(0);
                }
            }
            else if (items.Count == 0)
            {
                items.Add(value);
            }
            else
            {
                items[0] = value;
            }
        }

        /// <summary>
        /// The array property's getter: a new array, or null when empty.
        /// </summary>
        public static T[] ToArray<T>(List<T> items)
        {
            return items.Count > 0 ? items.ToArray() : null;
        }

        /// <summary>
        /// The array property's setter: copies the values, so the list never shares the caller's array.
        /// Null clears the list.
        /// </summary>
        public static void ReplaceAll<T>(List<T> items, T[] values)
        {
            items.Clear();
            if (values != null)
            {
                items.AddRange(values);
            }
        }

        /// <summary>
        /// The wire value: null when empty (not sent), the first element when single, otherwise an array.
        /// </summary>
        public static object ToWire<T>(List<T> items, bool single) where T : class
        {
            if (items.Count == 0)
            {
                return null;
            }

            return single ? (object)items[0] : items.ToArray();
        }

        /// <summary>
        /// Reads a deserialized wire value back, accepting either shape. Deserializing an object-typed
        /// member yields a JArray or a JObject.
        /// </summary>
        public static void FromWire<T>(List<T> items, object value)
        {
            items.Clear();
            if (value is JArray array)
            {
                items.AddRange(array.ToObject<T[]>().Where(item => item != null));
            }
            else if (value is JObject single)
            {
                items.Add(single.ToObject<T>());
            }
        }
    }
}
