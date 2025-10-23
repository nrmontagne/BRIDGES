using System;
using System.Numerics;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices
{
    /// <summary>
    /// Represents a generic dense matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix component values. </typeparam>
    public class DenseMatrix<T> : Matrix<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Storage of this <see cref="DenseMatrix{T}"/>.
        /// </summary>
        private readonly T[,] _storage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the component value of this matrix at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to get or set. </param>
        /// <param name="column"> Column index of the component to get or set. </param>
        /// <returns> The <typeparamref name="T"/> number at the given row and column. </returns>
        public T  this[int row, int column]
        {
            get { return _storage[row, column]; }
            set { _storage[row, column] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix{T}"/> class from its size.
        /// </summary>
        /// <remarks> The matrix is initialised with <see langword="default"/> values. </remarks>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        public DenseMatrix(int rowCount, int columnCount) 
            : base (rowCount, columnCount)
        {
            // Instantiate fields
            _storage = new T[RowCount, ColumnCount];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix{T}"/> class from its size and values.
        /// </summary>
        /// <remarks> The values must be given rows by rows. </remarks>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="values"> Values to fill the matrix with. </param>
        /// <exception cref="ArgumentOutOfRangeException"> The number of values provided doesn't match the given number of rows and columns.</exception>
        public DenseMatrix(int rowCount, int columnCount, T[] values)
            : base(rowCount, columnCount)
        {
            //     -----     Verifications

            if (values.Length != RowCount * ColumnCount)
            {
                throw new ArgumentOutOfRangeException (nameof(values), "The number of values provided doesn't match the given number of rows and columns.");
            }

            _storage = new T[RowCount, ColumnCount];
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    _storage[r, c] = values[(r * ColumnCount) + c];
                }
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix{T}"/> class by deep copying another <see cref="DenseMatrix{T}"/>.
        /// </summary>
        /// <param name="other"> <see cref="DenseMatrix{T}"/> to deep copy. </param>
        public DenseMatrix(DenseMatrix<T> other)
            : base(other.RowCount, other.ColumnCount)
        {
            // Initialises properties
            _storage = new T[RowCount, ColumnCount];
            Array.Copy(other._storage, _storage, RowCount * ColumnCount);
        }

        #endregion

        #region Public Static Methods

        //     -----     -----     Algebraic Near Ring : DenseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Add(DenseMatrix<T> left, DenseMatrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Subtract(DenseMatrix<T> left, DenseMatrix<T> right) => left - right;


        /// <inheritdoc cref="operator -(DenseMatrix{T})"/>
        public static DenseMatrix<T> Opposite(DenseMatrix<T> operand) => -operand;


        /// <inheritdoc cref="operator *(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Multiply(DenseMatrix<T> left, DenseMatrix<T> right) => left * right;


        //     -----     Other Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with another dense matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The number of rows of <paramref name="left"/> and <paramref name="right"/> are different. </exception>
        public static DenseMatrix<T> TransposeMultiply(DenseMatrix<T> left, DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (left.RowCount != right.RowCount) { throw new ArgumentException($"The number of rows of {nameof(left)} and {nameof(right)} are different."); }


            DenseMatrix<T> result = new DenseMatrix<T>(left.ColumnCount, right.ColumnCount);
            for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
            {
                for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                {
                    T component = default!;
                    for (int i_InitialRow = 0; i_InitialRow < right.RowCount; i_InitialRow++)
                    {
                        component += left[i_InitialRow, i_ResultRow] * right[i_InitialRow, i_ResultColumn];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a dense matrix with another transposed dense matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The number of columns of <paramref name="left"/> and <paramref name="right"/> are different. </exception>
        public static DenseMatrix<T> MultiplyTranspose(DenseMatrix<T> left, DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (left.ColumnCount != right.ColumnCount) { throw new ArgumentException($"The number of columns of {nameof(left)} and {nameof(right)} are different."); }


            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, right.RowCount);
            for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
            {
                for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                {
                    T component = default!;
                    for (int i_InitialColumn = 0; i_InitialColumn < left.ColumnCount; i_InitialColumn++)
                    {
                        component += left[i_ResultRow, i_InitialColumn] * right[i_ResultColumn, i_InitialColumn];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }
        
        
        //     -----     -----     Right Embedding : SparseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> Add(DenseMatrix<T> left, SparseMatrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> Subtract(DenseMatrix<T> left, SparseMatrix<T> right) => left - right;


        /// <inheritdoc cref="operator *(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> Multiply(DenseMatrix<T> left, SparseMatrix<T> right) => left * right;


        //     -----     Other Right Operations : SparseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with a sparse matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        public static DenseMatrix<T> TransposeMultiply(DenseMatrix<T> left, SparseMatrix<T> right)
        {
            return SparseMatrix<T>.TransposeMultiply(left, right);
        }

        /// <summary>
        /// Computes the multiplication of a dense matrix with a transposed sparse matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        public static DenseMatrix<T> MultiplyTranspose(DenseMatrix<T> left, SparseMatrix<T> right)
        {
            return SparseMatrix<T>.MultiplyTranspose(left, right);
        }


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(DenseMatrix{T}, T)"/>
        public static DenseMatrix<T> Multiply(DenseMatrix<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, DenseMatrix{T})"/>
        public static DenseMatrix<T> Multiply(T factor, DenseMatrix<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(DenseMatrix{T}, T)"/>
        public static DenseMatrix<T> Divide(DenseMatrix<T> operand, T divisor) => operand / divisor;


        //     -----     -----     Vectors     -----     -----     //

        /// <inheritdoc cref="operator *(DenseMatrix{T}, Vect.DenseVector{T})"/>
        public static Vect.DenseVector<T> Multiply(DenseMatrix<T> matrix, Vect.DenseVector<T> vector) => matrix * vector;

        /// <inheritdoc cref="operator *(DenseMatrix{T}, Vect.SparseVector{T})"/>
        public static Vect.DenseVector<T> Multiply(DenseMatrix<T> matrix, Vect.SparseVector<T> vector) => matrix * vector;


        //     -----      Other Operations : Vectors     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with a dense vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of rows of <paramref name="matrix"/> and the size of <paramref name="vector"/> must be equal. </exception>
        public static Vect.DenseVector<T> TransposeMultiply(DenseMatrix<T> matrix, Vect.DenseVector<T> vector)
        {
            //     -----     Verifications

            if (matrix.RowCount != vector.Size)
            {
                throw new ArgumentException($"The number of rows of {nameof(matrix)} and the size of {nameof(vector)} must be equal.");
            }

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(matrix.ColumnCount);
            for (int i_Result = 0; i_Result < result.Size; i_Result++)
            {
                T component = default!;
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    component += matrix[i, i_Result] * vector[i];
                }
                result[i_Result] = component;
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with a sparse vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of rows of <paramref name="matrix"/> and the size of <paramref name="vector"/> must be equal. </exception>
        public static Vect.DenseVector<T> TransposeMultiply(DenseMatrix<T> matrix, Vect.SparseVector<T> vector)
        {
            //     -----     Verifications

            if (matrix.RowCount != vector.Size)
            {
                throw new ArgumentException($"The number of rows of {nameof(matrix)} and the size of {nameof(vector)} must be equal.");
            }

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(matrix.ColumnCount);
            for (int i_Result = 0; i_Result < result.Size; i_Result++)
            {
                T component = default!;
                foreach ((int index, T vectorComponent) in vector.NonZeros())
                {
                    component += matrix[index, i_Result] * vectorComponent;
                }
                result[i_Result] = component;
            }

            return result;
        }


        //     -----     -----     Other Operations     -----     -----     //

        /// <summary>
        /// Computes the transposition of a dense matrix.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to operate from. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the transposition. </returns>
        public static DenseMatrix<T> Transpose(DenseMatrix<T> matrix)
        {
            DenseMatrix<T> result = new DenseMatrix<T>(matrix.ColumnCount, matrix.RowCount);
            for (int i_InitialRow = 0; i_InitialRow < matrix.RowCount; i_InitialRow++)
            {
                for (int i_InitialColumn = 0; i_InitialColumn < matrix.ColumnCount; i_InitialColumn++)
                {
                    result[i_InitialColumn, i_InitialRow] = matrix[i_InitialRow, i_InitialColumn];
                }
            }

            return result;
        }


        //     -----     -----     Other Static Methods     -----     -----     //

        /// <summary>
        /// Creates the identity matrix of a given size.
        /// </summary>
        /// <remarks> The identity matrix has ones in the diagonal and zeros elsewhere. </remarks>
        /// <param name="size"> Number of rows and columns for the matrix. </param>
        /// <returns> The identity <see cref="DenseMatrix{T}"/>. </returns>
        public static DenseMatrix<T> Identity(int size)
        {
            DenseMatrix<T> result = new DenseMatrix<T>(size, size);
            for (int i_Diagonal = 0; i_Diagonal < size; i_Diagonal++) { result[i_Diagonal, i_Diagonal] = T.One; }

            return result;
        }

        #endregion

        #region Operators

        //     -----     -----     Algebraic Near Ring : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of two dense matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the addition. </param>
        /// <returns> The <see cref="DenseMatrix{T}"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The two matrices must have the same number of rows and columns. </exception>
        public static DenseMatrix<T> operator +(DenseMatrix<T> left, DenseMatrix<T> right)
        {
            //     -----     Verifications

            if(left.RowCount != right.RowCount | left.ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The two matrices must have the same number of rows and columns.");
            }

            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, left.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = left[i_Row, i_Column] + right[i_Row, i_Column];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of two dense matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to subtract with. </param>
        /// <returns> The <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The two matrices must have the same number of rows and columns. </exception>
        public static DenseMatrix<T> operator -(DenseMatrix<T> left, DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (left.RowCount != right.RowCount | left.ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The two matrices must have the same number of rows and columns.");
            }

            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, left.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = left[i_Row, i_Column] - right[i_Row, i_Column];
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the unary negation of a dense matrix.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="DenseMatrix{T}"/> resulting from the unary negation. </returns>
        public static DenseMatrix<T> operator -(DenseMatrix<T> operand)
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.RowCount, operand.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = -operand[i_Row, i_Column];
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the multiplication of two dense matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException">
        /// The number of column of <paramref name="left"/> matrix and the number of rows of <paramref name="right"/> matrix must be equal.
        /// </exception>
        public static DenseMatrix<T> operator *(DenseMatrix<T> left, DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException($"The number of columns of {nameof(left)} matrix and the number of rows of {nameof(right)} matrix must be equal.");
            }

            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, right.ColumnCount);
            for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
            {
                for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                {
                    T component = default!;
                    for (int i_InitialColumn = 0; i_InitialColumn < left.ColumnCount; i_InitialColumn++)
                    {
                        component += left[i_ResultRow, i_InitialColumn] * right[i_InitialColumn, i_ResultColumn];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }


        //     -----     -----     Right Embedding : SparseMatrix<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of a dense matrix with a sparse matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the addition. </returns>
        public static DenseMatrix<T> operator +(DenseMatrix<T> left, SparseMatrix<T> right) =>  SparseMatrix<T>.AdditionOperator(left, right);

        /// <summary>
        /// Computes the subtraction of a dense matrix with a sparse matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        public static DenseMatrix<T> operator -(DenseMatrix<T> left, SparseMatrix<T> right) => SparseMatrix<T>.SubtractionOperator(left, right);


        /// <summary>
        /// Computes the multiplication of a dense matrix with a sparse matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        public static DenseMatrix<T> operator *(DenseMatrix<T> left, SparseMatrix<T> right) => SparseMatrix<T>.MultiplyOperator(left, right);


        //     -----      -----      Group Action : T      -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of a dense matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the right scalar multiplication. </returns>
        public static DenseMatrix<T> operator *(DenseMatrix<T> operand, T factor)
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.RowCount, operand.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = operand[i_Row, i_Column] * factor;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the left scalar multiplication of a dense matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to multiply on the left. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the left scalar multiplication. </returns>
        public static DenseMatrix<T> operator *(T factor, DenseMatrix<T> operand)
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.RowCount, operand.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = factor * operand[i_Row, i_Column];
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the scalar division of a dense matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the scalar division. </returns>
        public static DenseMatrix<T> operator /(DenseMatrix<T> operand, T divisor)
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.RowCount, operand.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = operand[i_Row, i_Column] / divisor;
                }
            }

            return result;
        }


        //     -----      -----      Vectors      -----     -----     //

        /// <summary>
        /// Computes the right multiplication of a dense matrix with a dense vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of <paramref name="matrix"/> and the size of <paramref name="vector"/> must be equal. </exception>
        public static Vect.DenseVector<T> operator *(DenseMatrix<T> matrix, Vect.DenseVector<T> vector)
        {
            //     -----     Verifications

            if (matrix.ColumnCount != vector.Size)
            {
                throw new ArgumentException($"The number of columns of {nameof(matrix)} and the size of {nameof(vector)} must be equal.");
            }

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(matrix.RowCount);
            for (int i_Result = 0; i_Result < result.Size; i_Result++)
            {
                T component = default!;
                for (int i = 0; i < matrix.ColumnCount; i++)
                {
                    component += matrix[i_Result, i] * vector[i];
                }
                result[i_Result] = component;
            }

            return result;
        }

        /// <summary>
        /// Computes the right multiplication of a dense matrix with a sparse vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of <paramref name="matrix"/> and the size of <paramref name="vector"/> must be equal. </exception>
        public static Vect.DenseVector<T> operator *(DenseMatrix<T> matrix, Vect.SparseVector<T> vector)
        {
            //     -----     Verifications

            if (matrix.ColumnCount != vector.Size)
            {
                throw new ArgumentException($"The number of columns of {nameof(matrix)} and the size of {nameof(vector)} must be equal.");
            }

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(matrix.RowCount);
            for (int i_Result = 0; i_Result < result.Size; i_Result++)
            {
                T component = default!;
                foreach((int index, T vectorComponent) in vector.NonZeros())
                {
                    component += matrix[i_Result, index] * vectorComponent;
                }
                result[i_Result] = component;
            }

            return result;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override T GetComponent(int row, int column)
        {
            return _storage[row, column];
        }

        /// <inheritdoc/>
        public override void SetComponent(int row, int column, T value)
        {
            _storage[row, column] = value;
        }

        
        /// <inheritdoc/>
        public override T[,] ToArray()
        {
            T[,] array = new T[RowCount,ColumnCount];
            Array.Copy(_storage, array, RowCount * ColumnCount);
            
            return array;
        }

        /// <inheritdoc/>
        public override T[] ToRowMajorArray()
        {
            T[] values = new T[RowCount * ColumnCount];
            for (int i_Row = 0; i_Row < RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
                {
                    values[(i_Row * ColumnCount) + i_Column] = _storage[i_Row, i_Column];
                }
            }
            return values;
        }

        /// <inheritdoc/>
        public override T[] ToColumnMajorArray()
        {
            T[] values = new T[RowCount * ColumnCount];
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i_Row = 0; i_Row < RowCount; i_Row++)
                {
                    values[i_Row + (i_Column * RowCount)] = _storage[i_Row, i_Column];
                }
            }
            return values;
        }

        /// <summary>
        /// Arranges each dense matrix row into a dense vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.DenseVector{T}"/>. </returns>
        public override Vect.DenseVector<T>[] RowVectors()
        {
            Vect.DenseVector<T>[] rows = new Vect.DenseVector<T>[RowCount];
            for (int i_Row = 0; i_Row < RowCount; i_Row++)
            {
                rows[i_Row] = new Vect.DenseVector<T>(ColumnCount);
                for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
                {
                    rows[i_Row][i_Column] = _storage[i_Row, i_Column];
                }
            }
            return rows;

        }

        /// <summary>
        /// Arranges each dense matrix column into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.DenseVector{T}"/>. </returns>
        public override Vect.DenseVector<T>[] ColumnVectors()
        {
            Vect.DenseVector<T>[] columns = new Vect.DenseVector<T>[ColumnCount];
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                columns[i_Column] = new Vect.DenseVector<T>(RowCount);
                for (int i_Row = 0; i_Row < RowCount; i_Row++)
                {
                    columns[i_Column][i_Row] = _storage[i_Row, i_Column];
                }
            }
            return columns;

        }

        #endregion


        #region Protected Inheritence : Matrix<T>

        //     -----     -----     Algebraic Near Ring : Matrix<T>    -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The addition of this dense matrix with <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented. 
        /// </exception>
        protected override Matrix<T> MatrixAdditionOperator(Matrix<T> right)
        {
            if (right is DenseMatrix<T> denseRight) { return this + denseRight; }
            else if (right is SparseMatrix<T> sparseRight) { return this + sparseRight; }
            else
            {
                throw new NotImplementedException($"The addition of this dense matrix with a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of this dense matrix with <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented. 
        /// </exception>
        protected override Matrix<T> MatrixSubtractionOperator(Matrix<T> right)
        {
            if (right is DenseMatrix<T> denseRight) { return this - denseRight; }
            else if (right is SparseMatrix<T> sparseRight) { return this - sparseRight; }
            else
            {
                throw new NotImplementedException($"The subtraction of this dense matrix with a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }


        /// <inheritdoc/>
        protected override Matrix<T> MatrixUnaryNegationOperator() => -this;


        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The multiplication of this dense matrix with <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented.
        /// </exception>
        protected override Matrix<T> MatrixMultiplyOperator(Matrix<T> right)
        {
            if (right is DenseMatrix<T> denseRight) { return this * denseRight; }
            else if (right is SparseMatrix<T> sparseRight) { return this * sparseRight; }
            else
            {
                throw new NotImplementedException($"The multiplication of this matrix with a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }


        //     -----     Other Operations : Matrix<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The multiplication of this transposed dense matrix with <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented.
        /// </exception>
        protected override Matrix<T> MatrixTransposeMultiply(Matrix<T> right)
        {
            if (right is DenseMatrix<T> denseRight) { return DenseMatrix<T>.TransposeMultiply(this, denseRight); }
            else if (right is SparseMatrix<T> sparseRight) { return DenseMatrix<T>.TransposeMultiply(this, sparseRight); }
            else
            {
                throw new NotImplementedException($"The multiplication of this transposed dense matrix with a {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The multiplication of this dense matrix with transposed <paramref name="right"/> as a <see cref="Matrix{T}"/> is not implemented.
        /// </exception>
        protected override Matrix<T> MatrixMultiplyTranspose(Matrix<T> right)
        {
            if (right is DenseMatrix<T> denseRight) { return DenseMatrix<T>.MultiplyTranspose(this, denseRight); }
            else if (right is SparseMatrix<T> sparseRight) { return DenseMatrix<T>.MultiplyTranspose(this, sparseRight); }
            else
            {
                throw new NotImplementedException($"The multiplication of this dense matrix with a transposed {right.GetType()} as a {typeof(Matrix<T>)} is not implemented.");
            }
        }


        //     -----     -----     Group Action : T     -----     -----     //

        /// <inheritdoc/>
        protected override Matrix<T> MatrixRightMultiplyOperator(T factor) => this * factor;

        /// <inheritdoc/>
        protected override Matrix<T> MatrixLeftMultiplyOperator(T factor) => factor * this;

        /// <inheritdoc/>
        protected override Matrix<T> MatrixDivisionOperator(T divisor) => this / divisor;


        //     -----     -----     Vectors     -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The right multiplication of this dense matrix with <paramref name="vector"/> as a <see cref="Vect.Vector{T}"/> is not implemented.
        /// </exception>
        protected override Vect.Vector<T> MatrixMultiplyOperator(Vect.Vector<T> vector)
        {
            if (vector is Vect.DenseVector<T> denseVector) { return this * denseVector; }
            else if (vector is Vect.SparseVector<T> sparseVector) { return this * sparseVector; }
            else
            {
                throw new NotImplementedException($"The multiplication of this dense matrix with a {vector.GetType()} as a {typeof(Vect.Vector<T>)} is not implemented.");
            }
        }

        //     -----     Other Operations : Vector<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The right multiplication of this transposed dense matrix with <paramref name="vector"/> as a <see cref="Vect.Vector{T}"/> is not implemented.
        /// </exception>
        protected override Vect.Vector<T> MatrixTransposeMultiply(Vect.Vector<T> vector)
        {
            if (vector is Vect.DenseVector<T> denseVector) { return DenseMatrix<T>.TransposeMultiply(this, denseVector); }
            else if (vector is Vect.SparseVector<T> sparseVector) { return DenseMatrix<T>.TransposeMultiply(this, sparseVector); }
            else
            {
                throw new NotImplementedException($"The multiplication of this transposed dense matrix with a {vector.GetType()} as a {typeof(Vect.Vector<T>)} is not implemented.");
            }
        }

        #endregion
    }
}
