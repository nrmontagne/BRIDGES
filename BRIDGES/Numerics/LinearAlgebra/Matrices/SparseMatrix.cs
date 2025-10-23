using System;
using System.Numerics;

using BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices
{
    /// <summary>
    /// Represents a generic sparse matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix component values. </typeparam>
    public class SparseMatrix<T> : Matrix<T>
        where T : INumberBase<T>
    {
        #region Fields

        private readonly SparseStorage<T> _storage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of non-zero values in this matrix. 
        /// </summary>
        /// <remarks> It is obtained as the number of entries in the sparse storage. </remarks>
        public int NonZeroCount => _storage.Count;

        /// <summary>
        /// Gets or sets the component value of this sparse matrix at the given row and column. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>.
        /// </remarks>
        /// <param name="row"> Row index of the component to get or set. </param>
        /// <param name="column"> Column index of the component to get or set. </param>
        /// <returns> The <typeparamref name="T"/> number at the given row and column. </returns>
        public T this[int row, int column]
        {
            get { return _storage[row, column]; }
            set { _storage.Replace(row, column, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseMatrix{T}"/> class from its sparse storage.
        /// </summary>
        /// <param name="storage"> Storage containing the values for the matrix, arranged in an efficient way. </param>
        public SparseMatrix(SparseStorage<T> storage)
            : base(storage.RowCount, storage.ColumnCount)
        {
            _storage = storage;
        }

        #endregion

        #region Public Static Methods

        //     -----     -----     Algebraic Near Ring : SparseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> Add(SparseMatrix<T> left, SparseMatrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> Subtract(SparseMatrix<T> left, SparseMatrix<T> right) => left - right;


        /// <inheritdoc cref="operator -(SparseMatrix{T})"/>
        public static SparseMatrix<T> Opposite(SparseMatrix<T> operand) => -operand;


        /// <inheritdoc cref="operator *(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> Multiply(SparseMatrix<T> left, SparseMatrix<T> right) => left * right;


        //     -----     Other Operations : SparseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with another sparse matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the operation. </returns>
        public static SparseMatrix<T> TransposeMultiply(SparseMatrix<T> left, SparseMatrix<T> right)
        {
            SparseStorage<T> result = SparseStorage<T>.TransposeMultiply(left._storage, right._storage);
            return new SparseMatrix<T>(result);
        }

        /// <summary>
        /// Computes the multiplication of a sparse matrix with another transposed sparse matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the operation. </returns>
        public static SparseMatrix<T> MultiplyTranspose(SparseMatrix<T> left, SparseMatrix<T> right)
        {
            SparseStorage<T> result = SparseStorage<T>.MultiplyTranspose(left._storage, right._storage);
            return new SparseMatrix<T>(result);
        }


        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Add(SparseMatrix<T> left, DenseMatrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Subtract(SparseMatrix<T> left, DenseMatrix<T> right) => left - right;


        /// <inheritdoc cref="operator *(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Multiply(SparseMatrix<T> left, DenseMatrix<T> right) => left * right;


        //     -----     Other Right Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a dense matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        public static DenseMatrix<T> TransposeMultiply(SparseMatrix<T> left, DenseMatrix<T> right)
        {
            return SparseStorage<T>.RightTransposeMultiply(left._storage, right);
        }

        /// <summary>
        /// Computes the multiplication of a sparse matrix with a transposed dense matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        public static DenseMatrix<T> MultiplyTranspose(SparseMatrix<T> left, DenseMatrix<T> right)
        {
            return SparseStorage<T>.RightMultiplyTranspose(left._storage, right);
        }


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(SparseMatrix{T}, T)"/>
        public static SparseMatrix<T> Multiply(SparseMatrix<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, SparseMatrix{T})"/>
        public static SparseMatrix<T> Multiply(T factor, SparseMatrix<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(SparseMatrix{T}, T)"/>
        public static SparseMatrix<T> Divide(SparseMatrix<T> operand, T divisor) => operand / divisor;


        //     -----     -----     Vectors     -----     -----     //

        /// <inheritdoc cref="operator *(SparseMatrix{T}, Vect.DenseVector{T})"/>
        public static Vect.DenseVector<T> Multiply(SparseMatrix<T> matrix, Vect.DenseVector<T> vector) => matrix * vector;

        /// <inheritdoc cref="operator *(SparseMatrix{T}, Vect.SparseVector{T})"/>
        public static Vect.SparseVector<T> Multiply(SparseMatrix<T> matrix, Vect.SparseVector<T> vector) => matrix * vector;


        //     -----      Other Operations : Vectors     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a sparse vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.SparseVector{T}"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector<T> TransposeMultiply(SparseMatrix<T> matrix, Vect.SparseVector<T> vector)
        {
            return SparseStorage<T>.TransposeMultiply(matrix._storage, vector);
        }

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a dense vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector<T> TransposeMultiply(SparseMatrix<T> matrix, Vect.DenseVector<T> vector)
        {
            return SparseStorage<T>.TransposeMultiply(matrix._storage, vector);
        }

        #endregion

        #region Internal Static Methods

        //     -----      Left Embedding : DenseMatrix<T>     -----     //

        /// <inheritdoc cref="DenseMatrix{T}.operator+(DenseMatrix{T}, SparseMatrix{T})"/>
        internal static DenseMatrix<T> AdditionOperator(DenseMatrix<T> left, SparseMatrix<T> right)
        {
            return SparseStorage<T>.LeftAdditionOperator(left, right._storage);
        }

        /// <inheritdoc cref="DenseMatrix{T}.operator-(DenseMatrix{T}, SparseMatrix{T})"/>
        internal static DenseMatrix<T> SubtractionOperator(DenseMatrix<T> left, SparseMatrix<T> right)
        {
            return SparseStorage<T>.LeftSubtractionOperator(left, right._storage);
        }

        /// <inheritdoc cref="DenseMatrix{T}.operator*(DenseMatrix{T}, SparseMatrix{T})"/>
        internal static DenseMatrix<T> MultiplyOperator(DenseMatrix<T> left, SparseMatrix<T> right)
        {
            return SparseStorage<T>.LeftMultiplyOperator(left, right._storage);
        }


        //     -----      Other Left Operations : DenseMatrix<T>     -----     //

        /// <inheritdoc cref="DenseMatrix{T}.TransposeMultiply(DenseMatrix{T}, SparseMatrix{T})"/>
        internal static DenseMatrix<T> TransposeMultiply(DenseMatrix<T> left, SparseMatrix<T> right)
        {
            return SparseStorage<T>.LeftTransposeMultiply(left, right._storage);
        }

        /// <inheritdoc cref="DenseMatrix{T}.MultiplyTranspose(DenseMatrix{T}, SparseMatrix{T})"/>
        internal static DenseMatrix<T> MultiplyTranspose(DenseMatrix<T> left, SparseMatrix<T> right)
        {
            return SparseStorage<T>.LeftMultiplyTranspose(left, right._storage);
        }

        #endregion

        #region Operators

        //     -----     -----     Algebraic Near Ring : SparseMatrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of two sparse matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> for the addition. </param>
        /// <returns> The <see cref="SparseMatrix{T}"/> resulting from the addition. </returns>
        public static SparseMatrix<T> operator +(SparseMatrix<T> left, SparseMatrix<T> right)
        {
            SparseStorage<T> result = SparseStorage<T>.AdditionOperator(left._storage, right._storage);
            return new SparseMatrix<T>(result);
        }

        /// <summary>
        /// Computes the subtraction of two sparse matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to subtract with. </param>
        /// <returns> The <see cref="SparseMatrix{T}"/> resulting from the subtraction. </returns>
        public static SparseMatrix<T> operator -(SparseMatrix<T> left, SparseMatrix<T> right)
        {
            SparseStorage<T> result = SparseStorage<T>.SubtractionOperator(left._storage, right._storage);
            return new SparseMatrix<T>(result);
        }

        /// <summary>
        /// Computes the unary negation of a sparse matrix.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="SparseMatrix{T}"/> resulting from the unary negation. </returns>
        public static SparseMatrix<T> operator -(SparseMatrix<T> operand)
        {
            SparseStorage<T> result = SparseStorage<T>.UnaryNegationOperator(operand._storage);
            return new SparseMatrix<T>(result);
        }


        /// <summary>
        /// Computes the multiplication of two sparse matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="SparseMatrix{T}"/> resulting from the multiplication. </returns>
        public static SparseMatrix<T> operator *(SparseMatrix<T> left, SparseMatrix<T> right)
        {
            SparseStorage<T> result = SparseStorage<T>.MultiplyOperator(left._storage, right._storage);
            return new SparseMatrix<T>(result);
        }


        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of a sparse matrix with a dense matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the addition. </returns>
        public static DenseMatrix<T> operator +(SparseMatrix<T> left, DenseMatrix<T> right)
        {
            return SparseStorage<T>.RightAdditionOperator(left._storage, right);
        }

        /// <summary>
        /// Computes the subtraction of a sparse matrix with a dense matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        public static DenseMatrix<T> operator -(SparseMatrix<T> left, DenseMatrix<T> right)
        {
            return SparseStorage<T>.RightSubtractionOperator(left._storage, right);
        }


        /// <summary>
        /// Computes the right multiplication of a sparse matrix with a dense matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        public static DenseMatrix<T> operator *(SparseMatrix<T> left, DenseMatrix<T> right)
        {
            return SparseStorage<T>.RightMultiplyOperator(left._storage, right);
        }



        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of a sparse matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the right scalar multiplication. </returns>
        public static SparseMatrix<T> operator *(SparseMatrix<T> operand, T factor)
        {
            SparseStorage<T> result = SparseStorage<T>.RightMultiplyOperator(operand._storage, factor);
            return new SparseMatrix<T>(result);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a sparse matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to multiply on the left. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the left scalar multiplication. </returns>
        public static SparseMatrix<T> operator *(T factor, SparseMatrix<T> operand)
        {
            SparseStorage<T> result = SparseStorage<T>.LeftMultiplyOperator(factor, operand._storage);
            return new SparseMatrix<T>(result);
        }

        /// <summary>
        /// Computes the scalar division of a sparse matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the scalar division. </returns>
        public static SparseMatrix<T> operator /(SparseMatrix<T> operand, T divisor)
        {
            SparseStorage<T> result = SparseStorage<T>.DivisionOperator(operand._storage, divisor);
            return new SparseMatrix<T>(result);
        }


        //     -----      -----     Vectors     -----     -----     //

        /// <summary>
        /// Computes the right multiplication of a sparse matrix with a sparse vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector{T}"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector<T> operator *(SparseMatrix<T> matrix, Vect.SparseVector<T> vector)
        {
            return SparseStorage<T>.MultiplyOperator(matrix._storage, vector);
        }

        /// <summary>
        /// Computes the right multiplication of a sparse matrix with a dense vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector<T> operator *(SparseMatrix<T> matrix, Vect.DenseVector<T> vector)
        {
            return SparseStorage<T>.MultiplyOperator(matrix._storage, vector);
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override T GetComponent(int row, int column)
        {
            return _storage.TryGet(row, column, out T? value) ? value! : T.Zero;
        }

        /// <inheritdoc/>
        public override void SetComponent(int row, int column, T value)
        {
            if (_storage.Contains(row, column))
            {
                if (value != T.Zero) { _storage.Replace(row, column, value, false); }
                else { _storage.Remove(row, column); }
            }
            else
            {
                if (value != T.Zero) { _storage.Add(row, column, value); }
            }
        }


        /// <summary>
        /// Evaluates whether the sparse matrix contains an entry for the component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to retrieve. </param>
        /// <param name="column"> Column index of the component to retrieve. </param>
        /// <returns> <see langword="true"/> if the sparse matrix contains an entry, <see langword="false"/> otherwise.  </returns>
        public bool Contains(int row, int column)
        {
            return _storage.Contains(row, column);
        }


        /// <inheritdoc/>
        public override T[,] ToArray() => _storage.ToArray();

        /// <inheritdoc/>
        public override T[] ToRowMajorArray() => _storage.ToRowMajorArray();

        /// <inheritdoc/>
        public override T[] ToColumnMajorArray() => _storage.ToColumnMajorArray();

        /// <summary>
        /// Arranges each sparse matrix row into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.SparseVector{T}"/>.</returns>
        public override Vect.SparseVector<T>[] RowVectors() => _storage.RowVectors();

        /// <summary>
        /// Arranges each sparse matrix column into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.SparseVector{T}"/>.</returns>
        public override Vect.SparseVector<T>[] ColumnVectors() => _storage.ColumnVectors();

        #endregion

        #region Other Methods

        /// <summary>
        /// Gets the sparse storage of this sparse matrix.
        /// </summary>
        internal SparseStorage<T> GetSparseStorage() => _storage;

        #endregion


        #region Protected Inheritence : Matrix<T>

        //     -----     Algebraic Near Ring : Matrix<T>    -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The addition of this sparse matrix with <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented. 
        /// </exception>
        protected override Matrix<T> MatrixAdditionOperator(Matrix<T> right)
        {
            if (right is SparseMatrix<T> sparseRight) { return this + sparseRight; } 
            else if (right is DenseMatrix<T> denseRight) { return this + denseRight; }
            else
            {
                throw new NotImplementedException($"The addition of this sparse matrix with a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of this sparse matrix with <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented. 
        /// </exception>
        protected override Matrix<T> MatrixSubtractionOperator(Matrix<T> right)
        {
            if (right is SparseMatrix<T> sparseRight) { return this - sparseRight; }
            else if (right is DenseMatrix<T> denseRight) { return this - denseRight; }
            else
            {
                throw new NotImplementedException($"The subtraction of this sparse matrix with a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }


        /// <inheritdoc/>
        protected override Matrix<T> MatrixUnaryNegationOperator() => -this;


        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The multiplication of this sparse matrix with <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented.
        /// </exception>
        protected override Matrix<T> MatrixMultiplyOperator(Matrix<T> right)
        {
            if (right is SparseMatrix<T> sparseRight) { return this * sparseRight; }
            else if (right is DenseMatrix<T> denseRight) { return this * denseRight; }
            else
            {
                throw new NotImplementedException($"The multiplication of this sparse matrix with a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }


        //     -----     Other Operations : Matrix<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The multiplication of this transposed sparse matrix with <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented.
        /// </exception>
        protected override Matrix<T> MatrixTransposeMultiply(Matrix<T> right)
        {
            if (right is SparseMatrix<T> sparseRight) { return SparseMatrix<T>.TransposeMultiply(this, sparseRight); }
            else if (right is DenseMatrix<T> denseRight) { return SparseMatrix<T>.TransposeMultiply(this, denseRight); }
            else
            {
                throw new NotImplementedException($"The multiplication of this transposed sparse matrix with a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The multiplication of this sparse matrix with transposed <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented.
        /// </exception>
        protected override Matrix<T> MatrixMultiplyTranspose(Matrix<T> right)
        {
            if (right is SparseMatrix<T> sparseRight) { return SparseMatrix<T>.MultiplyTranspose(this, sparseRight); }
            else if (right is DenseMatrix<T> denseRight) { return SparseMatrix<T>.MultiplyTranspose(this, denseRight); }
            else
            {
                throw new NotImplementedException($"The multiplication of this sparse matrix with transposed a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }


        //     -----     Group Action : T     -----     //

        /// <inheritdoc/>
        protected override Matrix<T> MatrixRightMultiplyOperator(T factor) => this * factor;

        /// <inheritdoc/>
        protected override Matrix<T> MatrixLeftMultiplyOperator(T factor) => factor * this;

        /// <inheritdoc/>
        protected override Matrix<T> MatrixDivisionOperator(T divisor) => this / divisor;


        //     -----     -----     Vectors     -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The right multiplication of this sparse matrix with <paramref name="vector"/> as a <see cref="Vect.Vector{T}"/> is not implemented.
        /// </exception>
        protected override Vect.Vector<T> MatrixMultiplyOperator(Vect.Vector<T> vector)
        {
            if (vector is Vect.SparseVector<T> sparseVector) { return this * sparseVector; }
            else if (vector is Vect.DenseVector<T> denseVector) { return this * denseVector; }
            else
            {
                throw new NotImplementedException($"The multiplication of this sparse matrix with a {vector.GetType()} as a {typeof(Vect.Vector<T>)} is not implemented.");
            }
        }


        //     -----     Other Operations : Vector<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The right multiplication of this transposed sparse matrix with <paramref name="vector"/> as a <see cref="Vect.Vector{T}"/> is not implemented.
        /// </exception>
        protected override Vect.Vector<T> MatrixTransposeMultiply(Vect.Vector<T> vector)
        {
            if (vector is Vect.SparseVector<T> sparseVector) { return SparseMatrix<T>.TransposeMultiply(this, sparseVector); }
            else if (vector is Vect.DenseVector<T> denseVector) { return SparseMatrix<T>.TransposeMultiply(this, denseVector); }
            else
            {
                throw new NotImplementedException($"The multiplication of this transposed sparse matrix with a {vector.GetType()} as a {typeof(Vect.Vector<T>)} is not implemented.");
            }
        }

        #endregion
    }
}
