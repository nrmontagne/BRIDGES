using System;
using System.Numerics;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages
{
    /// <summary>
    /// Represents a generic sparse storage for sparse matrices.
    /// </summary>
    /// <typeparam name="T"> Type of the values. </typeparam>
    public abstract class SparseStorage<T>
        where T : INumberBase<T>
    {
        #region Properties

        /// <summary>
        /// Gets the number of rows of this sparse storage.
        /// </summary>
        public int RowCount { get; init; }

        /// <summary>
        /// Gets the number of columns of this sparse storage.
        /// </summary>
        public int ColumnCount { get; init; }


        /// <summary>
        /// Gets the number of non-zero components in this sparse storage.
        /// </summary>
        /// <remarks> It is obtained as the number of entries in this sparse storage. </remarks>
        public abstract int Count { get; }


        /// <summary>
        /// Gets the component value of this sparse storage at the given row and column. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <param name="row"> Row index of the component to get or set. </param>
        /// <param name="column"> Column index of the component to get or set. </param>
        /// <returns> The <typeparamref name="T"/> number at the given row and column. </returns>
        public abstract T this[int row, int column] { get; }


        /// <summary>
        /// Gets the storage type of this sparse storage
        /// </summary>
        public abstract SparseStorageType StorageType { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseStorage{T}"/> class from its size.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the <see cref="SparseStorage{T}"/>. </param>
        /// <param name="columnCount"> Number of columns for the <see cref="SparseStorage{T}"/>. </param>
        /// <exception cref="ArgumentOutOfRangeException"> The number of rows and columns must be strictly larger than zero. </exception>
        internal SparseStorage(int rowCount, int columnCount)
        {
            //     -----     Verifications

            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), "The number of rows must be strictly larger than zero.");
            }
            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "The number of columns must be strictly larger than zero.");
            }

            // Initialise properties
            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        #endregion

        #region Internal Static Methods

        //     -----     -----     Algebraic Near Ring : SparseStorage<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of two sparse storages.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> for the addition. </param>
        /// <returns> The <see cref="SparseStorage{T}"/> resulting from the addition. </returns>
        internal static SparseStorage<T> AdditionOperator(SparseStorage<T> left, SparseStorage<T> right) => left.AdditionOperator(right);

        /// <summary>
        /// Computes the subtraction of two sparse storages.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to subtract with. </param>
        /// <returns> The <see cref="SparseStorage{T}"/> resulting from the subtraction. </returns>
        internal static SparseStorage<T> SubtractionOperator(SparseStorage<T> left, SparseStorage<T> right) => left.SubtractionOperator(right);


        /// <summary>
        /// Computes the unary negation of a sparse storage.
        /// </summary>
        /// <param name="operand"> <see cref="SparseStorage{T}"/> to operate from. </param>
        /// <returns> The <see cref="SparseStorage{T}"/> resulting from the unary negation. </returns>
        internal static SparseStorage<T> UnaryNegationOperator(SparseStorage<T> operand) => operand.UnaryNegationOperator();


        /// <summary>
        /// Computes the multiplication of two sparse storages.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="SparseStorage{T}"/> resulting from the multiplication. </returns>
        internal static SparseStorage<T> MultiplyOperator(SparseStorage<T> left, SparseStorage<T> right) => left.MultiplyOperator(right);


        //     -----     Other Operations : SparseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse storage with another sparse storage : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to multiply. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the operation. </returns>
        internal static SparseStorage<T> TransposeMultiply(SparseStorage<T> left, SparseStorage<T> right) => left.TransposeMultiply(right);

        /// <summary>
        /// Computes the multiplication of a sparse storage with another transposed sparse storage : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the operation. </returns>
        internal static SparseStorage<T> MultiplyTranspose(SparseStorage<T> left, SparseStorage<T> right) => left.MultiplyTranspose(right);


        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of a sparse storage with a dense matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the addition. </returns>
        internal static DenseMatrix<T> RightAdditionOperator(SparseStorage<T> left, DenseMatrix<T> right) => left.RightAdditionOperator(right);

        /// <summary>
        /// Computes the subtraction of a sparse storage with a dense matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        internal static DenseMatrix<T> RightSubtractionOperator(SparseStorage<T> left, DenseMatrix<T> right) => left.RightSubtractionOperator(right);


        /// <summary>
        /// Computes the right multiplication of a sparse storage with a dense matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        internal static DenseMatrix<T> RightMultiplyOperator(SparseStorage<T> left, DenseMatrix<T> right) => left.RightMultiplyOperator(right);


        //     -----     Other Right Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse storage with a dense matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> RightTransposeMultiply(SparseStorage<T> left, DenseMatrix<T> right) => left.RightTransposeMultiply(right);

        /// <summary>
        /// Computes the multiplication of a sparse storage with a transposed dense matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseStorage{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> RightMultiplyTranspose(SparseStorage<T> left, DenseMatrix<T> right) => left.RightMultiplyTranspose(right);


        //     -----     -----     Left Embedding : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of a dense matrix with a sparse storage.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the addition. </returns>
        internal static DenseMatrix<T> LeftAdditionOperator(DenseMatrix<T> left, SparseStorage<T> right) => right.LeftAdditionOperator(left);

        /// <summary>
        /// Computes the subtraction of a dense matrix with a sparse storage.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        internal static DenseMatrix<T> LeftSubtractionOperator(DenseMatrix<T> left, SparseStorage<T> right) => right.LeftSubtractionOperator(left);


        /// <summary>
        /// Computes the multiplication of a dense matrix with a sparse storage.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        internal static DenseMatrix<T> LeftMultiplyOperator(DenseMatrix<T> left, SparseStorage<T> right) => right.LeftMultiplyOperator(left);


        //     -----     Other Left Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with a sparse storage : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> LeftTransposeMultiply(DenseMatrix<T> left, SparseStorage<T> right) => right.LeftTransposeMultiply(left);

        /// <summary>
        /// Computes the multiplication of a dense matrix with a transposed sparse storage : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> LeftMultiplyTranspose(DenseMatrix<T> left, SparseStorage<T> right) => right.LeftMultiplyTranspose(left);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of a sparse storage with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseStorage{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the right scalar multiplication. </returns>
        internal static SparseStorage<T> RightMultiplyOperator(SparseStorage<T> operand, T factor) => operand.RightMultiplyOperator(factor);

        /// <summary>
        /// Computes the left scalar multiplication of a sparse storage with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseStorage{T}"/> to multiply on the left. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the left scalar multiplication. </returns>
        internal static SparseStorage<T> LeftMultiplyOperator(T factor, SparseStorage<T> operand) => operand.LeftMultiplyOperator(factor);


        /// <summary>
        /// Computes the scalar division of a sparse storage with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseStorage{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the scalar division. </returns>
        internal static SparseStorage<T> DivisionOperator(SparseStorage<T> operand, T divisor) => operand.DivisionOperator(divisor);


        //     -----      -----     Vectors     -----     -----     //

        /// <summary>
        /// Computes the right multiplication of a sparse storage with a sparse vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseStorage{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.SparseVector<T> MultiplyOperator(SparseStorage<T> matrix, Vect.SparseVector<T> vector) => matrix.MultiplyOperator(vector);

        /// <summary>
        /// Computes the right multiplication of a sparse storage with a dense vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseStorage{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.DenseVector<T> MultiplyOperator(SparseStorage<T> matrix, Vect.DenseVector<T> vector) => matrix.MultiplyOperator(vector);


        //     -----      Other Operations : Vectors     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse storage with a sparse vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseStorage{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.SparseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.SparseVector<T> TransposeMultiply(SparseStorage<T> matrix, Vect.SparseVector<T> vector) => matrix.TransposeMultiply(vector);

        /// <summary>
        /// Computes the multiplication of a transposed sparse storage with a dense vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseStorage{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.DenseVector<T> TransposeMultiply(SparseStorage<T> matrix, Vect.DenseVector<T> vector) => matrix.TransposeMultiply(vector);

        #endregion

        #region Public Methods

        /// <summary>
        /// Attemps to get the value of a component in this sparse storage at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to get. </param>
        /// <param name="column"> Column index of the component to get. </param>
        /// <param name="value"> Value of the component at the given row and column if it was found, <see langword="default"/> otherwise. </param>
        /// <returns> <see langword="true"/> if the component was found, <see langword="false"/> otherwise. </returns>
        public abstract bool TryGet(int row, int column, out T? value);


        /// <summary>
        /// Adds a non-zero component to this sparse storage.
        /// </summary>
        /// <remarks> For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>. </remarks>
        /// <param name="row"> Row index of the component to insert. </param>
        /// <param name="column"> Column index of the component to insert. </param>
        /// <param name="value"> Value for the component to insert. </param>
        /// <returns> <see langword="true"/> if the component was inserted, <see langword="false"/> otherwise. </returns>
        public abstract bool Add(int row, int column, T value);

        /// <summary>
        /// Adds a non-zero component to this sparse storage.
        /// </summary>
        /// <param name="row"> Row index of the component to insert. </param>
        /// <param name="column"> Column index of the component to insert. </param>
        /// <param name="value"> Value for the component to insert. </param>
        /// <param name="nonZeroCheck"> Evaluates whether the <paramref name="value"/> should be checked for the non-zero condition. </param>
        /// <returns> <see langword="true"/> if the component was inserted, <see langword="false"/> otherwise. </returns>
        public abstract bool Add(int row, int column, T value, bool nonZeroCheck);


        /// <summary>
        /// Replaces the value of a non-zero component in this sparse storage.
        /// </summary>
        /// <remarks> For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>. </remarks>
        /// <param name="row"> Row index of the component to replace. </param>
        /// <param name="column"> Column index of the component to replace. </param>
        /// <param name="value"> Value for the component to replace. </param>
        /// <returns> <see langword="true"/> if the component was retrieved and replaced, <see langword="false"/> if the component was not found in the sparse storage. </returns>
        public abstract bool Replace(int row, int column, T value);

        /// <summary>
        /// Replaces the value of a non-zero component in this sparse storage.
        /// </summary>
        /// <param name="row"> Row index of the component to replace. </param>
        /// <param name="column"> Column index of the component to replace. </param>
        /// <param name="value"> Value for the component to replace. </param>
        /// <param name="nonZeroCheck"> Evaluates whether the <paramref name="value"/> should be checked for the non-zero condition. </param>
        /// <returns> <see langword="true"/> if the component was retrieved and replaced, <see langword="false"/> if the component was not found in the sparse storage. </returns>
        public abstract bool Replace(int row, int column, T value, bool nonZeroCheck);


        /// <summary>
        /// Removes the value of a non-zero component in this sparse storage. 
        /// </summary>
        /// <param name="row"> Row index of the component to remove. </param>
        /// <param name="column"> Column index of the component to remove. </param>
        /// <returns> <see langword="true"/> if the component was retrieved and removed, <see langword="false"/> if the component was not found in the sparse storage. </returns>
        public abstract bool Remove(int row, int column);


        /// <summary>
        /// Evaluates whether the sparse storage contains an entry for the component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to retrieve. </param>
        /// <param name="column"> Column index of the component to retrieve. </param>
        /// <returns> <see langword="true"/> if the sparse storage has an entry, <see langword="false"/> otherwise.  </returns>
        public abstract bool Contains(int row, int column);



        /// <summary>
        /// Provides a compressed column storage representation of this sparse storage.
        /// </summary>
        /// <returns> The <see cref="CompressedColumn{T}"/> </returns>
        public abstract CompressedColumn<T> ToCompressedColumn();

        #endregion

        #region Internal Methods

        /// <summary>
        /// Provides an array representation of this sparse storage.
        /// </summary>
        /// <returns> The two-dimensional array. </returns>
        internal abstract T[,] ToArray();

        /// <summary>
        /// Provides an array representation of this sparse storage, with the values arranged row by row.
        /// </summary>
        /// <returns> The one-dimensional array, resulting from the concatenation of the rows. </returns>
        internal abstract T[] ToRowMajorArray();

        /// <summary>
        /// Provides an array representation of this sparse storage, with the values arranged column by column.
        /// </summary>
        /// <returns> The one-dimensional array, resulting from the concatenation of the columns. </returns>
        internal abstract T[] ToColumnMajorArray();

        /// <summary>
        /// Arranges each sparse storage row into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.SparseVector{T}"/>.</returns>
        internal abstract Vect.SparseVector<T>[] RowVectors();

        /// <summary>
        /// Arranges each sparse storage column into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.SparseVector{T}"/>.</returns>
        internal abstract Vect.SparseVector<T>[] ColumnVectors();


        #endregion

        #region Protected Methods

        //     -----     -----     Algebraic Near Ring : ISparseStorage<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of this sparse storage with another sparse storage.
        /// </summary>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> for the addition. </param>
        /// <returns> The <see cref="SparseStorage{T}"/> resulting from the addition. </returns>
        protected abstract SparseStorage<T> AdditionOperator(SparseStorage<T> right);

        /// <summary>
        /// Computes the subtraction of this sparse storage with another sparse storage.
        /// </summary>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to subtract with. </param>
        /// <returns> The <see cref="SparseStorage{T}"/> resulting from the subtraction. </returns>
        protected abstract SparseStorage<T> SubtractionOperator(SparseStorage<T> right);


        /// <summary>
        /// Computes the unary negation of this sparse storage.
        /// </summary>
        /// <returns> The <see cref="SparseStorage{T}"/> resulting from the unary negation. </returns>
        protected abstract SparseStorage<T> UnaryNegationOperator();


        /// <summary>
        /// Computes the multiplication of this sparse storage with another sparse storage.
        /// </summary>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="SparseStorage{T}"/> resulting from the multiplication. </returns>
        protected abstract SparseStorage<T> MultiplyOperator(SparseStorage<T> right);


        //     -----     Other Operations : ISparseStorage<T>     -----     //

        /// <summary>
        /// Computes the multiplication of this transposed sparse storage with another sparse storage : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to multiply. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the operation. </returns>
        protected abstract SparseStorage<T> TransposeMultiply(SparseStorage<T> right);

        /// <summary>
        /// Computes the multiplication of this sparse storage with another transposed sparse storage : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="right"> Right <see cref="SparseStorage{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the operation. </returns>
        protected abstract SparseStorage<T> MultiplyTranspose(SparseStorage<T> right);


        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of this sparse storage with a dense matrix.
        /// </summary>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the addition. </returns>
        protected abstract DenseMatrix<T> RightAdditionOperator(DenseMatrix<T> right);

        /// <summary>
        /// Computes the subtraction of this sparse storage with a dense matrix.
        /// </summary>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        protected abstract DenseMatrix<T> RightSubtractionOperator(DenseMatrix<T> right);


        /// <summary>
        /// Computes the right multiplication of this sparse storage with a dense matrix.
        /// </summary>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        protected abstract DenseMatrix<T> RightMultiplyOperator(DenseMatrix<T> right);


        //     -----     Other Right Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of this transposed sparse storage with a dense matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        protected abstract DenseMatrix<T> RightTransposeMultiply(DenseMatrix<T> right);

        /// <summary>
        /// Computes the multiplication of this sparse storage with a transposed dense matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        protected abstract DenseMatrix<T> RightMultiplyTranspose(DenseMatrix<T> right);


        //     -----     -----     Left Embedding : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of a dense matrix with this sparse storage.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the addition. </returns>
        protected abstract DenseMatrix<T> LeftAdditionOperator(DenseMatrix<T> left);

        /// <summary>
        /// Computes the subtraction of a dense matrix with this sparse storage.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to subtract. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        protected abstract DenseMatrix<T> LeftSubtractionOperator(DenseMatrix<T> left);


        /// <summary>
        /// Computes the multiplication of a dense matrix with this sparse storage.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        protected abstract DenseMatrix<T> LeftMultiplyOperator(DenseMatrix<T> left);


        //     -----     Other Left Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with this sparse storage : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        protected abstract DenseMatrix<T> LeftTransposeMultiply(DenseMatrix<T> left);

        /// <summary>
        /// Computes the multiplication of a dense matrix with this transposed sparse storage : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        protected abstract DenseMatrix<T> LeftMultiplyTranspose(DenseMatrix<T> left);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of this sparse storage with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the right scalar multiplication. </returns>
        protected abstract SparseStorage<T> RightMultiplyOperator(T factor);

        /// <summary>
        /// Computes the left scalar multiplication of this sparse storage with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the left scalar multiplication. </returns>
        protected abstract SparseStorage<T> LeftMultiplyOperator(T factor);


        /// <summary>
        /// Computes the scalar division of this sparse storage with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="SparseStorage{T}"/> resulting from the scalar division. </returns>
        protected abstract SparseStorage<T> DivisionOperator(T divisor);


        //     -----      -----     Vectors     -----     -----     //

        /// <summary>
        /// Computes the right multiplication of this sparse storage with a sparse vector : <c>M·V</c>.
        /// </summary>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector{T}"/> resulting from the multiplication. </returns>
        protected abstract Vect.SparseVector<T> MultiplyOperator(Vect.SparseVector<T> vector);

        /// <summary>
        /// Computes the right multiplication of this sparse storage with a dense vector : <c>M·V</c>.
        /// </summary>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        protected abstract Vect.DenseVector<T> MultiplyOperator(Vect.DenseVector<T> vector);


        //     -----      Other Operations : Vectors     -----     //

        /// <summary>
        /// Computes the multiplication of this transposed sparse storage with a sparse vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.SparseVector{T}"/> resulting from the multiplication. </returns>
        protected abstract Vect.SparseVector<T> TransposeMultiply(Vect.SparseVector<T> vector);

        /// <summary>
        /// Computes the multiplication of this transposed sparse storage with a dense vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        protected abstract Vect.DenseVector<T> TransposeMultiply(Vect.DenseVector<T> vector);

        #endregion
    }
}
