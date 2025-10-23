using System;

using CSp_Stor = CSparse.Storage;

using Mat = BRIDGES.Numerics.LinearAlgebra.Matrices;
using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Double
{
    /// <summary>
    /// Class containing extension methods for <see cref="SparseStorages.CompressedColumn{T}"/> with <see cref="double"/>-precision values.
    /// </summary>
    public static class SparseMatrixExtensions
    {
        #region Solve Ax=y

        /// <summary>
        /// Solves the system <c>A·x=y</c> using Cholesky decomposition.
        /// </summary>
        /// <param name="matrix"> The sparse matrix A. </param>
        /// <param name="vector"> The vector y. </param>
        /// <returns> The vector x. </returns>
        public static Vect.DenseVector<double> SolveCholesky(this SparseMatrix<double> matrix, Vect.DenseVector<double> vector)
        {
            Mat.SparseStorages.SparseStorage<double> storage = matrix.GetSparseStorage();

            Mat.SparseStorages.CompressedColumn<double> ccs;
            if (storage.StorageType == SparseStorages.SparseStorageType.CompressedColumn) { ccs = (Mat.SparseStorages.CompressedColumn<double>)storage; }
            else { ccs = storage.ToCompressedColumn(); }

            CSp_Stor.CompressedColumnStorage<double> sparseStorage = new CSparse.Double.SparseMatrix(ccs.RowCount, ccs.ColumnCount, ccs.Values.ToArray(), ccs.RowIndices.ToArray(), ccs.ColumnPointers);

            var cholesky = CSparse.Double.Factorization.SparseCholesky.Create(sparseStorage, CSparse.ColumnOrdering.MinimumDegreeAtPlusA);

            double[] x = new double[matrix.ColumnCount];
            cholesky.Solve(vector.ToArray(), x);

            return new Vect.DenseVector<double>(x);
        }

        /// <summary>
        /// Solves the system <c>A·x=y</c> using Cholesky decomposition.
        /// </summary>
        /// <param name="matrix"> The sparse matrix A. </param>
        /// <param name="vector"> The vector y. </param>
        /// <returns> The vector x. </returns>
        public static Vect.SparseVector<double> SolveCholesky(this SparseMatrix<double> matrix, Vect.SparseVector<double> vector)
        {
            Mat.SparseStorages.SparseStorage<double> storage = matrix.GetSparseStorage();

            Mat.SparseStorages.CompressedColumn<double> ccs;
            if (storage.StorageType == SparseStorages.SparseStorageType.CompressedColumn) { ccs = (Mat.SparseStorages.CompressedColumn<double>)storage; }
            else { ccs = storage.ToCompressedColumn(); }

            CSp_Stor.CompressedColumnStorage<double> sparseStorage = new CSparse.Double.SparseMatrix(ccs.RowCount, ccs.ColumnCount, ccs.Values.ToArray(), ccs.RowIndices.ToArray(), ccs.ColumnPointers);

            var cholesky = CSparse.Double.Factorization.SparseCholesky.Create(sparseStorage, CSparse.ColumnOrdering.MinimumDegreeAtPlusA);

            double[] x = new double[matrix.ColumnCount];
            cholesky.Solve(vector.ToArray(), x);

            Vect.SparseVector<double> sparseX = new Vect.SparseVector<double>(x.Length);
            for (int i = 0; i < x.Length; i++)
            {
                double value = x[i];
                if (value != 0.0) { sparseX.Add(i, value); }
            }

            return sparseX;
        }

        #endregion
    }
}
