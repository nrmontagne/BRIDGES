using BRIDGES.Tests.DataStructures.PolyhedralMeshes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BRIDGES.Tests
{

    /// <summary>
    /// Base class for test data.
    /// </summary>
    internal abstract class BaseDataClass : IEnumerable<object[]>
    {
        /// <summary>
        /// Functions providing the data for the parametrised test.
        /// </summary>
        /// <remarks>
        /// Fixing first dimenension of the array provides a set of datas for the test. <br/>
        /// Each datas (i.e. <see cref="T:object[]"/>), traversed in the second dimension, are concatenated to get the actual parameters for the test.
        /// </remarks> 
        internal abstract Func<object[]>[,] DataProviders { get; }


        /// <inheritdoc/>
        public IEnumerator<object[]> GetEnumerator()
        {
            int? count = null;

            for (int i_Set = 0; i_Set < DataProviders.GetLength(0); i_Set++)
            {
                List<object> parameters = count is null ? new List<object>() : new List<object>(count.Value);

                for (int i_Data = 0; i_Data < DataProviders.GetLength(1); i_Data++)
                {
                    Func<object[]> dataProvider = DataProviders[i_Set, i_Data];
                    parameters.AddRange(dataProvider.Invoke());
                }

                count ??= parameters.Count;

                yield return parameters.ToArray();
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
