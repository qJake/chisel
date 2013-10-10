using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chisel
{
    public static class NullUtilities
    {
        /// <summary>
        /// Coalesces a nullable value type, or returns the default value if the nullable type is null.
        /// </summary>
        /// <typeparam name="T">The type of nullable value object being checked (without "Nullable" or "?").</typeparam>
        /// <param name="Value">The nullable value type to check.</param>
        /// <param name="DefaultValue">The default value if the nullable value is null.</param>
        /// <returns>If <paramref name="Value" /> is not null, returns Value, otherwise returns <paramref name="DefaultValue" />.</returns>
        public static T Coalesce<T>(Nullable<T> Value, T DefaultValue) where T : struct
        {
            return Value.HasValue ? Value.Value : DefaultValue;
        }

        /// <summary>
        /// Coalesces a reference type, or returns the default value if the original value is null.
        /// </summary>
        /// <typeparam name="T">The type of reference object being checked.</typeparam>
        /// <param name="Value">The nullable value type to check.</param>
        /// <param name="DefaultValue">The default value if the nullable value is null.</param>
        /// <returns>If <paramref name="Value" /> is not null, returns Value, otherwise returns <paramref name="DefaultValue" />.</returns>
        public static T Coalesce<T>(T Value, T DefaultValue) where T : class
        {
            return Value != null ? Value : DefaultValue;
        }
    }
}
