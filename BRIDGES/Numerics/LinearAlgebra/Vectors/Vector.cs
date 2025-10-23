using System;
using System.Numerics;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors
{
    /// <summary>
    /// Represents a generic vector.
    /// </summary>
    /// <typeparam name="T"> Type of the vector components. </typeparam>
    public abstract class Vector<T>
        where T : INumberBase<T>
    {
        #region Properties

        /// <summary>
        /// Number of components of this vector.
        /// </summary>
        public int Size { get; init; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Vector{T}"/> class.
        /// </summary>
        /// <param name="size"> Total number of component. </param>
        internal Vector(int size)
        {
            Size = size;
        }

        #endregion

        #region Public Static Methods

        //     -----     -----     Additive Abelian Group : Vector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(Vector{T}, Vector{T})"/>
        public static Vector<T> Add(Vector<T> left, Vector<T> right) => left + right;

        /// <inheritdoc cref="operator -(Vector{T}, Vector{T})"/>
        public static Vector<T> Subtract(Vector<T> left, Vector<T> right) => left - right;


        /// <inheritdoc cref="operator -(Vector{T})"/>
        public static Vector<T> Opposite(Vector<T> operand) => -operand;


        //     -----     Other Operations : Vector<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed vector with another vector : <c>V<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Vector{T}"/> to multiply. </param>
        /// <returns> The <typeparamref name="T"/> number resulting from the operation. </returns>
        public static T TransposeMultiply(Vector<T> left, Vector<T> right) => left.VectorTransposeMultiply(right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(Vector{T}, T)"/>
        public static Vector<T> Multiply(Vector<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, Vector{T})"/>
        public static Vector<T> Multiply(T factor, Vector<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(Vector{T}, T)"/>
        public static Vector<T> Divide(Vector<T> operand, T divisor) => operand / divisor;

        #endregion

        #region Operators

        //     -----     -----     Additive Abelian Group : Vector<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of two vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vector{T}"/> for the addition. </param>
        /// <returns> The <see cref="Vector{T}"/> resulting from the addition. </returns>
        public static Vector<T> operator +(Vector<T> left, Vector<T> right) => left.VectorAdditionOperator(right);

        /// <summary>
        /// Computes the subtraction of two vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Vector{T}"/> to subtract with. </param>
        /// <returns> The <see cref="Vector{T}"/> resulting from the subtraction. </returns>
        public static Vector<T> operator -(Vector<T> left, Vector<T> right) => left.VectorSubtractionOperator(right);


        /// <summary>
        /// Computes the unary negation of a vector.
        /// </summary>
        /// <param name="operand"> <see cref="Vector{T}"/> to operate from. </param>
        /// <returns> The <see cref="Vector{T}"/> resulting from the unary negation. </returns>
        public static Vector<T> operator -(Vector<T> operand) => operand.VectorUnaryNegationOperator();


        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of a vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the right scalar multiplication. </returns>
        public static Vector<T> operator *(Vector<T> operand, T factor) => operand.VectorRightMultiplyOperator(factor);

        /// <summary>
        /// Computes the left scalar multiplication of a vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector{T}"/> to multiply on the left. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the left scalar multiplication. </returns>
        public static Vector<T> operator *(T factor, Vector<T> operand) => operand.VectorLeftMultiplyOperator(factor);


        /// <summary>
        /// Computes the scalar division of a vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the scalar division. </returns>
        public static Vector<T> operator /(Vector<T> operand, T divisor) => operand.VectorDivisionOperator(divisor);

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the value of the component at the given index.
        /// </summary>
        /// <param name="index"> Index of the component. </param>
        /// <returns> The value of the component. </returns>
        public abstract T GetComponent(int index);

        /// <summary>
        /// Assigns a value of the component at the given index
        /// </summary>
        /// <param name="index"> Index of the component to set. </param>
        /// <param name="value"> Value for the component. </param>
        public abstract void SetComponent(int index, T value);


        /// <summary>
        /// Translates this vector into its array representation.
        /// </summary>
        /// <returns> The array representing the vector. </returns>
        public abstract T[] ToArray();

        #endregion

        #region Protected Methods

        //     -----     -----     Algebraic Near Ring : Vector<T>    -----     -----     //

        /// <summary>
        /// Computes the addition of this vector with another vector.
        /// </summary>
        /// <param name="right"> Right <see cref="Vector{T}"/> for the addition. </param>
        /// <returns> The <see cref="Vector{T}"/> resulting from the addition. </returns>
        protected abstract Vector<T> VectorAdditionOperator(Vector<T> right);

        /// <summary>
        /// Computes the subtraction of this vector with another vector.
        /// </summary>
        /// <param name="right"> Right <see cref="Vector{T}"/> to subtract with. </param>
        /// <returns> The <see cref="Vector{T}"/> resulting from the subtraction. </returns>
        protected abstract Vector<T> VectorSubtractionOperator(Vector<T> right);


        /// <summary>
        /// Computes the unary negation of this vector.
        /// </summary>
        /// <returns> The <see cref="Vector{T}"/> resulting from the unary negation. </returns>
        protected abstract Vector<T> VectorUnaryNegationOperator();


        //     -----     -----     Other Operations : Vector<T>     -----     -----     //

        /// <summary>
        /// Computes the multiplication of this transposed vector with another vector : <c>V<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="right"> Right <see cref="Vector{T}"/> to multiply. </param>
        /// <returns> The <typeparamref name="T"/> number resulting from the operation. </returns>
        protected abstract T VectorTransposeMultiply(Vector<T> right);


        //     -----     -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of this vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the right scalar multiplication. </returns>
        protected abstract Vector<T> VectorRightMultiplyOperator(T factor);

        /// <summary>
        /// Computes the left scalar multiplication of this vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the left scalar multiplication. </returns>
        protected abstract Vector<T> VectorLeftMultiplyOperator(T factor);


        /// <summary>
        /// Computes the scalar division of this vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the scalar division. </returns>
        protected abstract Vector<T> VectorDivisionOperator(T divisor);

        #endregion
    }
}
