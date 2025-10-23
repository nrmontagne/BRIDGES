using System;
using System.Numerics;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices
{
    /// <summary>
    /// Represents a generic matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix component values. </typeparam>
    public abstract class Matrix<T>
        where T : INumberBase<T>
    {
        #region Properties

        /// <summary>
        /// Gets the number of rows of this matrix.
        /// </summary>
        public int RowCount { get; init; }

        /// <summary>
        /// Gets the number of columns of this matrix.
        /// </summary>
        public int ColumnCount { get; init; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Matrix{T}"/> class from its size.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the <see cref="Matrix{T}"/>. </param>
        /// <param name="columnCount"> Number of columns for the <see cref="Matrix{T}"/>. </param>
        /// <remarks> The constructor is set to internal to prevent any external derived class. </remarks>
        internal Matrix(int rowCount, int columnCount)
        {
            //     -----     Verifications

            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), "The number of row must be strictly larger than 0.");
            }
            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "The number of columns must be strictly larger than 0.");
            }

            // Initialise properties
            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        #endregion

        #region Public Static Methods

        //     -----     -----     Algebraic Near Ring : Matrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> Add(Matrix<T> left, Matrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> Subtract(Matrix<T> left, Matrix<T> right) => left - right;


        /// <inheritdoc cref="operator -(Matrix{T})"/>
        public static Matrix<T> Opposite(Matrix<T> operand) => -operand;


        /// <inheritdoc cref="operator *(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> Multiply(Matrix<T> left, Matrix<T> right) => left * right;


        //     -----     Other Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed matrix with another matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the operation. </returns>
        public static Matrix<T> TransposeMultiply(Matrix<T> left, Matrix<T> right) => left.MatrixTransposeMultiply(right);

        /// <summary>
        /// Computes the multiplication of a matrix with another transposed matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the operation. </returns>
         public static Matrix<T> MultiplyTranspose(Matrix<T> left, Matrix<T> right) => left.MatrixMultiplyTranspose(right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(Matrix{T}, T)"/>
        public static Matrix<T> Multiply(Matrix<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, Matrix{T})"/>
        public static Matrix<T> Multiply(T factor, Matrix<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(Matrix{T}, T)"/>
        public static Matrix<T> Divide(Matrix<T> operand, T divisor) => operand / divisor;


        //     -----     -----     Vectors     -----     -----     //

        /// <inheritdoc cref="operator *(Matrix{T}, Vect.Vector{T})"/>
        public static Vect.Vector<T> Multiply(Matrix<T> matrix, Vect.Vector<T> vector) => matrix * vector;

        /// <summary>
        /// Computes the multiplication of a transposed matrix with a vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.Vector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.Vector{T}"/> resulting from the multiplication. </returns>
        public static Vect.Vector<T> TransposeMultiply(Matrix<T> matrix, Vect.Vector<T> vector) => matrix.MatrixTransposeMultiply(vector);

        #endregion

        #region Operators

        //     -----     -----     Algebraic Near Ring : Matrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of two matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> for the addition. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the addition. </returns>
        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right) => left.MatrixAdditionOperator(right);

        /// <summary>
        /// Computes the subtraction of two matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> to subtract with. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the subtraction. </returns>
        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right) => left.MatrixSubtractionOperator(right);


        /// <summary>
        /// Computes the unary negation of a matrix.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the unary negation. </returns>
        public static Matrix<T> operator -(Matrix<T> operand) => operand.MatrixUnaryNegationOperator();


        /// <summary>
        /// Computes the multiplication of two matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the multiplication. </returns>
        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right) => left.MatrixMultiplyOperator(right);


        //     -----     Group Action : T     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of a matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the right scalar multiplication. </returns>
        public static Matrix<T> operator *(Matrix<T> operand, T factor) => operand.MatrixRightMultiplyOperator(factor);

        /// <summary>
        /// Computes the left scalar multiplication of a matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to multiply on the left. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the left scalar multiplication. </returns>
        public static Matrix<T> operator *(T factor, Matrix<T> operand) => operand.MatrixLeftMultiplyOperator(factor);


        /// <summary>
        /// Computes the scalar division of a matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the scalar division. </returns>
        public static Matrix<T> operator /(Matrix<T> operand, T divisor) => operand.MatrixDivisionOperator(divisor);


        //     -----     Vectors     -----     //

        /// <summary>
        /// Computes the right multiplication of a matrix with a vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector{T}"/> resulting from the multiplication. </returns>
        public static Vect.Vector<T> operator *(Matrix<T> matrix, Vect.Vector<T> vector) => matrix.MatrixMultiplyOperator(vector);

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the value of the component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to get. </param>
        /// <param name="column"> Column index of the component to get. </param>
        /// <returns> The value of the component. </returns>
        public abstract T GetComponent(int row, int column);

        /// <summary>
        /// Assigns a value of the component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to set. </param>
        /// <param name="column"> Column index of the component to set. </param>
        /// <param name="value"> Value for the component. </param>
        public abstract void SetComponent(int row, int column, T value);


        /// <summary>
        /// Provides an array representation of this matrix.
        /// </summary>
        /// <returns> The two-dimensional array. </returns>
        public abstract T[,] ToArray();

        /// <summary>
        /// Provides an array representation of this matrix, with the values arranged row by row.
        /// </summary>
        /// <returns> The one-dimensional array, resulting from the concatenation of the rows. </returns>
        public abstract T[] ToRowMajorArray();

        /// <summary>
        /// Provides an array representation of this matrix, with the values arranged column by column.
        /// </summary>
        /// <returns> The one-dimensional array, resulting from the concatenation of the columns. </returns>
        public abstract T[] ToColumnMajorArray();

        /// <summary>
        /// Arranges each matrix row into a vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.Vector{T}"/>. </returns>
        public abstract Vect.Vector<T>[] RowVectors();

        /// <summary>
        /// Arranges each matrix column into a vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.Vector{T}"/> array.</returns>
        public abstract Vect.Vector<T>[] ColumnVectors();

        #endregion

        #region Protected Methods

        //     -----     -----     Algebraic Near Ring : Matrix<T>    -----     -----     //

        /// <summary>
        /// Computes the addition of this matrix with another matrix.
        /// </summary>
        /// <param name="right"> Right <see cref="Matrix{T}"/> for the addition. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the addition. </returns>
        protected abstract Matrix<T> MatrixAdditionOperator(Matrix<T> right);

        /// <summary>
        /// Computes the subtraction of this matrix with another matrix.
        /// </summary>
        /// <param name="right"> Right <see cref="Matrix{T}"/> to subtract with. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the subtraction. </returns>
        protected abstract Matrix<T> MatrixSubtractionOperator(Matrix<T> right);


        /// <summary>
        /// Computes the unary negation of this matrix.
        /// </summary>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the unary negation. </returns>
        protected abstract Matrix<T> MatrixUnaryNegationOperator();


        /// <summary>
        /// Computes the multiplication of this matrix with another matrix.
        /// </summary>
        /// <param name="right"> Right <see cref="Matrix{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the multiplication. </returns>
        protected abstract Matrix<T> MatrixMultiplyOperator(Matrix<T> right);


        //     -----     Other Operations : Matrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of this transposed matrix with another matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="right"> Right <see cref="Matrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the operation. </returns>
        protected abstract Matrix<T> MatrixTransposeMultiply(Matrix<T> right);

        /// <summary>
        /// Computes the multiplication of this matrix with another transposed matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        protected abstract Matrix<T> MatrixMultiplyTranspose(Matrix<T> right);


        //     -----     -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of this matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the right scalar multiplication. </returns>
        protected abstract Matrix<T> MatrixRightMultiplyOperator(T factor);

        /// <summary>
        /// Computes the left scalar multiplication of this matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the left scalar multiplication. </returns>
        protected abstract Matrix<T> MatrixLeftMultiplyOperator(T factor);


        /// <summary>
        /// Computes the scalar division of this matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the scalar division. </returns>
        protected abstract Matrix<T> MatrixDivisionOperator(T divisor);


        //     -----     -----     Vectors     -----     -----     //

        /// <summary>
        /// Computes the right multiplication of this matrix with a vector : <c>M·V</c>.
        /// </summary>
        /// <param name="vector"> <see cref="Vect.Vector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector{T}"/> resulting from the multiplication. </returns>
        protected abstract Vect.Vector<T> MatrixMultiplyOperator(Vect.Vector<T> vector);

        /// <summary>
        /// Computes the multiplication of this transposed matrix with a vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="vector"> <see cref="Vect.Vector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.Vector{T}"/> resulting from the multiplication. </returns>
        protected abstract Vect.Vector<T> MatrixTransposeMultiply(Vect.Vector<T> vector);

        #endregion


        #region Override : Object

        /// <inheritdoc/>
        public override string? ToString()
        {
            return $"Matrix [{RowCount};{ColumnCount}]";
        }

        #endregion
    }
}