using BRIDGES.Numerics.LinearAlgebra.Matrices;
using System;
using System.Collections.Generic;
using System.Numerics;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors
{
    /// <summary>
    /// Represents a generic dense vector.
    /// </summary>
    /// <typeparam name="T"> Type of the vector components. </typeparam>
    public class DenseVector<T> : Vector<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Components of this <see cref="DenseVector{T}"/>.
        /// </summary>
        private readonly T[] _components;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the component at a given index.
        /// </summary>
        /// <param name="index"> Index of the component to get or set. </param>
        /// <returns> The value of the component. </returns>
        public T this[int index]
        {
            get { return _components[index]; }
            set { _components[index] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector{T}"/> class from its size.
        /// </summary>
        /// <remarks> The dense vector is initialised with <see langword="default"/> values. </remarks>
        /// <param name="size"> Number of components of the <see cref="DenseVector{T}"/>. </param>
        public DenseVector(int size)
            : base(size)
        {
            _components = new T[size];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector{T}"/> class from its components.
        /// </summary>
        /// <param name="components"> Components of the <see cref="DenseVector{T}"/>. </param>
        public DenseVector(IList<T> components)
            : base(components.Count)
        {
            _components = new T[Size];
            for (int i = 0; i < Size; i++)
            {
                _components[i] = components[i];
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector{T}"/> class from another <see cref="DenseVector{T}"/>.
        /// </summary>
        /// <param name="vector"> <see cref="DenseVector{T}"/> to deep copy. </param>
        public DenseVector(DenseVector<T> vector)
            : base(vector.Size)
        {
            _components = new T[Size];
            for (int i = 0; i < Size; i++)
            {
                _components[i] = vector._components[i];
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector{T}"/> class from a <see cref="SparseVector{T}"/>.
        /// </summary>
        /// <param name="vector"> <see cref="SparseVector{T}"/> to copy. </param>
        public DenseVector(SparseVector<T> vector)
            : base(vector.Size)
        {
            _components = new T[Size];
            foreach (var (index, value) in vector.NonZeros())
            {
                _components[index] = value;
            }
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the zero dense vector.
        /// </summary>
        /// <param name="size"> Number of component of the <see cref="DenseVector{T}"/>. </param>
        /// <returns> The <see cref="DenseVector{T}"/> of the given size, with zero on each components. </returns>
        public static DenseVector<T> Zero(int size) => new DenseVector<T>(size);

        /// <summary>
        /// Returns the i-th unit dense vector.
        /// </summary>
        /// <remarks> It corresponds to the vector with <paramref name="size"/> components, with one at the given row <paramref name="index"/> and zeros elsewhere. </remarks>
        /// <param name="size"> Number of components for the <see cref="DenseVector{T}"/>. </param>
        /// <param name="index"> Index of the standard vector, which corresponds to the coordinate of the component equal to one. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> representing the standard vector. </returns>
        public static DenseVector<T> StandardVector(int size, int index) => new DenseVector<T>(size) { [index] = T.One };

        #endregion

        #region Public Static Methods

        //     -----     -----     Additive Abelian Group : DenseVector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(DenseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> Add(DenseVector<T> left, DenseVector<T> right) => left + right;

        /// <inheritdoc cref="operator -(DenseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> Subtract(DenseVector<T> left, DenseVector<T> right) => left - right;


        /// <inheritdoc cref="operator -(DenseVector{T})"/>
        public static DenseVector<T> Opposite(DenseVector<T> operand) => -operand;


        //     -----     Other Operations : DenseVector<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed dense vector with another dense vector : <c>V<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> to multiply. </param>
        /// <returns> The <typeparamref name="T"/> number resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The two dense vectors must have the same number of components. </exception>
        public static T TransposeMultiply(DenseVector<T> left, DenseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The two dense vectors must have the same number of components."); }


            int size = left.Size;

            T result = T.Zero;
            for (int i = 0; i < size; i++)
            {
                result += left[i] * right[i];
            }

            return result;
        }


        //     -----     -----     Right Embedding : SparseVector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(DenseVector{T}, SparseVector{T})"/>
        public static DenseVector<T> Add(DenseVector<T> left, SparseVector<T> right) => left + right;

        /// <inheritdoc cref="operator -(DenseVector{T}, SparseVector{T})"/>
        public static DenseVector<T> Subtract(DenseVector<T> left, SparseVector<T> right) => left - right;


        //     -----     Other Right Operations : SparseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed dense vector with a sparse vector : <c>V<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> to multiply. </param>
        /// <returns> The <typeparamref name="T"/> number resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The two vectors must have the same number of components. </exception>
        public static T TransposeMultiply(DenseVector<T> left, SparseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The two vectors must have the same number of components."); }


            T result = T.Zero;
            foreach ((int index, T value) in right.NonZeros())
            {
                result += left[index] * value;
            }

            return result;
        }


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(DenseVector{T}, T)"/>
        public static DenseVector<T> Multiply(DenseVector<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, DenseVector{T})"/>
        public static DenseVector<T> Multiply(T factor, DenseVector<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(DenseVector{T}, T)"/>
        public static DenseVector<T> Divide(DenseVector<T> operand, T divisor) => operand / divisor;

        #endregion

        #region Operators

        //     -----     -----     Additive Abelian Group : DenseVector<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of two dense vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> for the addition. </param>
        /// <returns> The <see cref="DenseVector{T}"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The two dense vectors must have the same number of components. </exception>
        public static DenseVector<T> operator +(DenseVector<T> left, DenseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The two dense vectors must have the same number of components."); }


            DenseVector<T> result = new DenseVector<T>(left.Size);
            for (int i = 0; i < result.Size; i++)
            {
                result[i] = left[i] + right[i];
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of two dense vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> to subtract with. </param>
        /// <returns> The <see cref="DenseVector{T}"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The two vectors must have the same number of components. </exception>
        public static DenseVector<T> operator -(DenseVector<T> left, DenseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The two dense vectors must have the same number of components."); }


            DenseVector<T> result = new DenseVector<T>(left.Size);
            for (int i = 0; i < result.Size; i++)
            {
                result[i] = left[i] - right[i];
            }

            return result;
        }


        /// <summary>
        /// Computes the unary negation of a dense vector.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector{T}"/> to operate from. </param>
        /// <returns> The <see cref="DenseVector{T}"/> resulting from the unary negation. </returns>
        public static DenseVector<T> operator -(DenseVector<T> operand)
        {
            DenseVector<T> result = new DenseVector<T>(operand.Size);
            for (int i = 0; i < result.Size; i++)
            {
                result[i] = - operand[i];
            }

            return result;
        }


        //     -----     -----     Right Embedding : SparseVector<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of a dense vector with a sparse vector.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The dense vector and the sparse vector must have the same number of components. </exception>
        public static DenseVector<T> operator +(DenseVector<T> left, SparseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The dense vector and the sparse vector must have the same number of components."); }


            DenseVector<T> result = new DenseVector<T>(left);
            foreach ((int index, T value) in right.NonZeros())
            {
                result[index] += value;
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a dense vector with a sparse vector.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The dense vector and the sparse vector must have the same number of components. </exception>
        public static DenseVector<T> operator -(DenseVector<T> left, SparseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The dense vector and the sparse vector must have the same number of components."); }


            DenseVector<T> result = new DenseVector<T>(left);
            foreach ((int index, T value) in right.NonZeros())
            {
                result[index] -= value;
            }

            return result;
        }


        //     -----      -----      Group Action : T      -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of a dense vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the right scalar multiplication. </returns>
        public static DenseVector<T> operator *(DenseVector<T> operand, T factor)
        {
            DenseVector<T> result = new DenseVector<T>(operand.Size);
            for (int i = 0; i < result.Size; i++)
            {
                result[i] = operand[i] * factor;
            }

            return result;
        }

        /// <summary>
        /// Computes the left scalar multiplication of a dense vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="DenseVector{T}"/> to multiply on the left. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the left scalar multiplication. </returns>
        public static DenseVector<T> operator *(T factor, DenseVector<T> operand)
        {
            DenseVector<T> result = new DenseVector<T>(operand.Size);
            for (int i = 0; i < result.Size; i++)
            {
                result[i] = factor * operand[i];
            }

            return result;
        }


        /// <summary>
        /// Computes the scalar division of a dense vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the scalar division. </returns>
        public static DenseVector<T> operator /(DenseVector<T> operand, T divisor)
        {
            DenseVector<T> result = new DenseVector<T>(operand.Size);
            for (int i = 0; i < result.Size; i++)
            {
                result[i] = operand[i] / divisor;
            }

            return result;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override T GetComponent(int index)
        {
            return _components[index];
        }

        /// <inheritdoc/>
        public override void SetComponent(int index, T value)
        {
            _components[index] = value;
        }


        /// <inheritdoc cref="Vector{T}.ToArray()"/>
        public override T[] ToArray() => (T[])_components.Clone();

        #endregion


        #region Protected Inheritence : Vector<T>

        //     -----     -----     Additive Abelian Group : Vector<T>     -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The addition of this dense vector with <paramref name="right"/> as a <see cref="Vector{T}"/> is not implemented. 
        /// </exception>
        protected override Vector<T> VectorAdditionOperator(Vector<T> right)
        {
            if (right is DenseVector<T> denseRight) { return this + denseRight; }
            else if (right is SparseVector<T> sparseRight) { return this + sparseRight; }
            else
            {
                throw new NotImplementedException($"The addition of this dense vector with a {right.GetType()} as a {typeof(Vector<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of this dense vector with <paramref name="right"/> as a <see cref="Vector{T}"/> is not implemented. 
        /// </exception>
        protected override Vector<T> VectorSubtractionOperator(Vector<T> right)
        {
            if (right is DenseVector<T> denseRight) { return this - denseRight; }
            else if (right is SparseVector<T> sparseRight) { return this - sparseRight; }
            else
            {
                throw new NotImplementedException($"The subtraction of this dense vector with a {right.GetType()} as a {typeof(Vector<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        protected override Vector<T> VectorUnaryNegationOperator() => -this;


        //     -----     Other Operations : Vector<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The multiplication of this transposed dense vector with <paramref name="right"/> as a <see cref="Vector{T}"/> is not implemented.
        /// </exception>
        protected override T VectorTransposeMultiply(Vector<T> right)
        {
            if (right is DenseVector<T> denseRight) { return DenseVector<T>.TransposeMultiply(this, denseRight); }
            else if (right is SparseVector<T> sparseRight) { return DenseVector<T>.TransposeMultiply(this, sparseRight); }
            else
            {
                throw new NotImplementedException($"The multiplication of this transposed dense vector with a {right.GetType()} as a {typeof(Vector<T>)} is not implemented.");
            }
        }


        //     -----     -----     Group Action : T     -----     -----     //

        /// <inheritdoc/>
        protected override Vector<T> VectorRightMultiplyOperator(T factor) => this * factor;

        /// <inheritdoc/>
        protected override Vector<T> VectorLeftMultiplyOperator(T factor) => factor * this;

        /// <inheritdoc/>
        protected override Vector<T> VectorDivisionOperator(T divisor) => this / divisor;

        #endregion
    }
}
