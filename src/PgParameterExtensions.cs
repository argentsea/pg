// © John Hicks. All rights reserved. Licensed under the MIT license.
// See the LICENSE file in the repository root for more information.

using System;
using System.Data.Common;
using System.Collections.Generic;

namespace ArgentSea.Pg
{
    public static class PgParameterExtensions
    {
        /// <summary>
        /// Gets an array from the output parameter, or null if the parameter value is DbNull.
        /// </summary>
        /// <param name="prm">The output parameter, populated with a value (after Execute).</param>
        /// <returns>The parameter value as a typed array.</returns>
        public static T[] GetArray<T>(this DbParameter prm)
        {
            if (prm.Value == System.DBNull.Value)
            {
                return null;
            }
            return (T[])prm.Value;
        }
        /// <summary>
        /// Gets an array from the output parameter, or null if the parameter value is DbNull.
        /// </summary>
        /// <param name="prm">The output parameter, populated with a value (after Execute).</param>
        /// <returns>The parameter value as a typed array.</returns>
        public static IDictionary<string, string> GetHStore(this DbParameter prm)
        {
            if (prm.Value == System.DBNull.Value)
            {
                return null;
            }
            return (Dictionary<string, string>)prm.Value;
        }
    }
}
