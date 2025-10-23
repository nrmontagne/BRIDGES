using System;
using System.Numerics;
using System.Collections.Generic;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors
{
    /// <summary>
    /// Represents a generic sparse vector.
    /// </summary>
    /// <typeparam name="T"> Type of the vector values. </typeparam>
    public class SparseVector<T> : Vector<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Storage for the non-zero components of this <see cref="SparseVector{T}"/>.
        /// </summary>
        /// <remarks> 
        /// <list type="bullet"> 
        ///     <item>
        ///         <term>Key</term>
        ///         <description> Row index of the non-zero component. </description>
        ///     </item>
        ///     <item>
        ///         <term>Value</term>
        ///         <description> Value of the non-zero component at the given row index. </description>
        ///     </item>
        /// </list> 
        /// </remarks>
        private readonly Dictionary<int, T> _components;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of non-zero components in this sparse vector. <br/>
        /// It obtained as the number of entries in this sparse storage.
        /// </summary>
        /// <remarks> The total number of components is given by <see cref="Vector{T}.Size"/>. </remarks>
        public int NonZeroCount => _components.Count;

        /// <summary>
        /// Gets or sets the component at a given index in this sparse vector. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>.
        /// </remarks>
        /// <param name="index"> Index of the component to get or set. </param>
        /// <returns> The <typeparamref name="T"/> number at the given index. </returns>
        public T this[int index]
        {
            get { return _components[index]; }
            set { _components[index] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from its size.
        /// </summary>
        /// <remarks> The sparse storage is initialised empty. </remarks>
        /// <param name="size"> Number of components of the <see cref="SparseVector{T}"/>. </param>
        public SparseVector(int size)
            : base(size)
        {
            _components = new Dictionary<int, T>(Size / 4);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from its size.
        /// </summary>
        /// <remarks> The sparse storage is initialised empty. </remarks>
        /// <param name="size"> Number of components for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="capacity"> Number of non-zero components that can be stored in the <see cref="SparseVector{T}"/>. </param>
        public SparseVector(int size, int capacity)
            : base(size)
        {
            _components = new Dictionary<int, T>(capacity);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from its size and sets of indices and values.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, no checks are performed on the non-zero condition of the <paramref name="values"/>.
        /// </remarks>
        /// <param name="size"> Number of components for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="indices"> Indices of the non-zero values for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="values"> Non-zero values for the <see cref="SparseVector{T}"/>. </param>
        /// <exception cref="ArgumentException"> The numbers of elements in <paramref name="indices"/> and <paramref name="values"/> must be the same. </exception>
        public SparseVector(int size, IList<int> indices, IList<T> values)
            : base(size)
        {
            //     -----     Verifications

            if (indices.Count != values.Count)
            {
                throw new ArgumentException($"The numbers of elements in {nameof(indices)} and {nameof(values)} must be the same.");
            }


            _components = new Dictionary<int, T>(Size);
            for (int i = 0; i < values.Count; i++) { _components.Add(indices[i], values[i]); }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from its size and sets of indices and values.
        /// </summary>
        /// <param name="size"> Number of components for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="indices"> Indices of the non-zero values for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="values"> Non-zero values for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="nonZeroCheck"> Evaluates whether the <paramref name="values"/> should be checked for the non-zero condition. </param>
        /// <exception cref="ArgumentException"> The numbers of elements in <paramref name="indices"/> and <paramref name="values"/> must be the same. </exception>
        public SparseVector(int size, IList<int> indices, IList<T> values, bool nonZeroCheck)
            : base(size)
        {
            //     -----     Verifications

            if (indices.Count != values.Count)
            {
                throw new ArgumentException($"The numbers of elements in {nameof(indices)} and {nameof(values)} must be the same.");
            }

            _components = new Dictionary<int, T>(Size);

            if (nonZeroCheck)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i] == T.Zero) { continue; }

                    _components.Add(indices[i], values[i]);
                }
            }
            else
            {
                for (int i = 0; i < values.Count; i++) { _components.Add(indices[i], values[i]); }
            }

                
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from another <see cref="SparseVector{T}"/>.
        /// </summary>
        /// <param name="other"> <see cref="SparseVector{T}"/> to deep copy. </param>
        public SparseVector(SparseVector<T> other)
            : base(other.Size)
        {
            _components = new Dictionary<int, T>(other._components);
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Returns the zero sparse vector.
        /// </summary>
        /// <param name="size"> Number of component of the <see cref="SparseVector{T}"/>. </param>
        /// <returns> The <see cref="SparseVector{T}"/> of the given size, with zero on each components. </returns>
        public static SparseVector<T> Zero(int size) => new SparseVector<T>(size);

        /// <summary>
        /// Returns the i-th unit sparse vector.
        /// </summary>
        /// <remarks> Corresponds to the vector with <paramref name="size"/> components, with one at the given row <paramref name="index"/> and zeros elsewhere. </remarks>
        /// <param name="size"> Number of components for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="index"> Index of the standard vector, which corresponds to the coordinate of the component equal to one. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> representing the standard vector. </returns>
        public static SparseVector<T> StandardVector(int size, int index) => new SparseVector<T>(size) { [index] = T.One };

        #endregion

        #region Public Static Methods

        //     -----     -----     Additive Abelian Group : SparseVector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(SparseVector{T}, SparseVector{T})"/>
        public static SparseVector<T> Add(SparseVector<T> left, SparseVector<T> right) => left + right;

        /// <inheritdoc cref="operator -(SparseVector{T}, SparseVector{T})"/>
        public static SparseVector<T> Subtract(SparseVector<T> left, SparseVector<T> right) => left - right;


        /// <inheritdoc cref="operator -(SparseVector{T})"/>
        public static SparseVector<T> Opposite(SparseVector<T> operand) => -operand;


        //     -----     Other Operations : SparseVector<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse vector with another sparse vector : <c>V<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> to multiply. </param>
        /// <returns> The <typeparamref name="T"/> number resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The two sparse vectors must have the same number of components. </exception>
        public static T TransposeMultiply(SparseVector<T> left, SparseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The two sparse vectors must have the same number of components."); }


            T result = T.Zero;
            foreach ((int index, T leftValue) in left.NonZeros())
            {
                if (right.TryGet(index, out T? rightValue)) { result += leftValue * rightValue!; }
            }

            return result;
        }
        

        //     -----     -----     Right Embedding : DenseVector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(SparseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> Add(SparseVector<T> left, DenseVector<T> right) => left + right;

        /// <inheritdoc cref="operator -(SparseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> Subtract(SparseVector<T> left, DenseVector<T> right) => left - right;


        //     -----     Other Right Operations : SparseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse vector with a dense vector : <c>V<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> to multiply. </param>
        /// <returns> The <typeparamref name="T"/> number resulting from the operation. </returns>
        /// <exception cref="ArgumentException"> The sparse vector and the dense vector must have the same number of components. </exception>
        public static T TransposeMultiply(SparseVector<T> left, DenseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The vector and the dense vector must have the same number of components."); }


            T result = T.Zero;
            foreach ((int index, T leftValue) in left.NonZeros())
            {
                result += leftValue * right[index];
            }

            return result;
        }


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(SparseVector{T}, T)"/>
        public static SparseVector<T> Multiply(SparseVector<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, SparseVector{T})"/>
        public static SparseVector<T> Multiply(T factor, SparseVector<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(SparseVector{T}, T)"/>
        public static SparseVector<T> Divide(SparseVector<T> operand, T divisor) => operand / divisor;


        #endregion

        #region Operators

        //     -----     -----     Additive Abelian Group : SparseVector<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of two sparse vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> for the addition. </param>
        /// <returns> The <see cref="SparseVector{T}"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The two sparse vectors must have the same number of components. </exception>
        public static SparseVector<T> operator +(SparseVector<T> left, SparseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The two sparse vectors must have the same number of components."); }


            int capacity = left.NonZeroCount + right.NonZeroCount < left.Size ? left.NonZeroCount + right.NonZeroCount : left.Size;
            SparseVector <T> result = new SparseVector<T>(left.Size, capacity);

            foreach ((int index, T leftValue) in left.NonZeros()) { result.Add(index, leftValue); }

            foreach ((int index, T rightValue) in right.NonZeros())
            {
                if (result.TryGet(index, out T? leftValue))
                {
                    if (leftValue == -rightValue) { result.Remove(index); }
                    else { result[index] = leftValue! + rightValue; }
                }
                else { result.Add(index, rightValue); }
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of two sparse vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> to subtract with. </param>
        /// <returns> The <see cref="SparseVector{T}"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The two sparse vectors must have the same number of components. </exception>
        public static SparseVector<T> operator -(SparseVector<T> left, SparseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The two sparse vectors must have the same number of components."); }


            int capacity = left.NonZeroCount + right.NonZeroCount < left.Size ? left.NonZeroCount + right.NonZeroCount : left.Size;
            SparseVector<T> result = new SparseVector<T>(left.Size, capacity);

            foreach ((int index, T leftValue) in left.NonZeros()) { result.Add(index, leftValue); }

            foreach ((int index, T rightValue) in right.NonZeros())
            {
                if (result.TryGet(index, out T? leftValue))
                {
                    if (leftValue == rightValue) { result.Remove(index); }
                    else { result[index] = leftValue! - rightValue; }
                }
                else { result.Add(index, -rightValue); }
            }

            return result;
        }


        /// <summary>
        /// Computes the unary negation of a sparse vector.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector{T}"/> to operate from. </param>
        /// <returns> The <see cref="SparseVector{T}"/> resulting from the unary negation. </returns>
        public static SparseVector<T> operator -(SparseVector<T> operand)
        {
            SparseVector<T> result = new SparseVector<T>(operand.Size, operand.NonZeroCount);
            foreach ((int index, T value) in operand.NonZeros())
            {
                result.Add(index, -value);
            }

            return result;
        }


        //     -----     -----     Right Embedding : DenseVector<T>     -----     -----     //

        /// <summary>
        /// Computes the addition of a sparse vector with a dense vector.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The sparse vector and the dense vector must have the same number of components. </exception>
        public static DenseVector<T> operator +(SparseVector<T> left, DenseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The sparse vector and the dense vector must have the same number of components."); }


            DenseVector<T> result = new DenseVector<T>(right);
            foreach ((int index, T leftValue) in left.NonZeros()) 
            {
                result[index] = leftValue + right[index];
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a sparse vector with a dense vector.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The sparse vector and the dense vector must have the same number of components. </exception>
        public static DenseVector<T> operator -(SparseVector<T> left, DenseVector<T> right)
        {
            //     -----     Verifications

            if (left.Size != right.Size) { throw new ArgumentException("The sparse vector and the dense vector must have the same number of components."); }


            DenseVector<T> result = -right;
            foreach ((int index, T leftValue) in left.NonZeros())
            {
                result[index] = leftValue - right[index];
            }

            return result;
        }


        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Computes the right scalar multiplication of a sparse vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> resulting from the right scalar multiplication. </returns>
        public static SparseVector<T> operator *(SparseVector<T> operand, T factor)
        {
            SparseVector<T> result = new SparseVector<T>(operand.Size, operand.NonZeroCount);
            if (factor != T.Zero)
            {
                foreach ((int index, T value) in operand.NonZeros())
                {
                    result.Add(index, value * factor);
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the left scalar multiplication of a sparse vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="SparseVector{T}"/> to multiply on the left. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> resulting from the left scalar multiplication. </returns>
        public static SparseVector<T> operator *(T factor, SparseVector<T> operand)
        {
            SparseVector<T> result = new SparseVector<T>(operand.Size, operand.NonZeroCount);
            if (factor != T.Zero)
            {
                foreach ((int index, T value) in operand.NonZeros())
                {
                    result.Add(index, factor * value);
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the scalar division of a sparse vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> resulting from the scalar division. </returns>
        /// <exception cref="DivideByZeroException"> The <paramref name="divisor"/> can't be equal to zero. </exception>
        public static SparseVector<T> operator /(SparseVector<T> operand, T divisor)
        {
            //     -----     Verifications

            if (divisor == T.Zero) { throw new DivideByZeroException($"The {nameof(divisor)} can't be equal to zero."); }


            SparseVector<T> result = new SparseVector<T>(operand.Size, operand.NonZeroCount);
            foreach ((int index, T value) in operand.NonZeros())
            {
                result.Add(index, value / divisor);
            }

            return result;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a non-zero component to this sparse vector. <br/>
        /// The sparse storage must not have an existing entry for the component.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>.
        /// </remarks>
        /// <param name="index"> Index of the component to add. </param>
        /// <param name="value"> Value of the component to add. </param>
        /// <returns> <see langword="true"/> if the component was added, <see langword="false"/> otherwise. </returns>
        public bool Add(int index, T value)
        {
            _components.Add(index, value);

            return true;
        }

        /// <summary>
        /// Adds a non-zero component to this sparse vector. <br/>
        /// The sparse storage must not have an existing entry for the component.
        /// </summary>
        /// <param name="index"> Index for the component to add. </param>
        /// <param name="value"> Value for the component to add. </param>
        /// <param name="nonZeroCheck"> Evaluates whether the <paramref name="value"/> should be checked for the non-zero condition. </param>
        /// <returns> <see langword="true"/> if the component was added, <see langword="false"/> otherwise. </returns>
        public bool Add(int index, T value, bool nonZeroCheck)
        {
            if(nonZeroCheck && value == T.Zero) { return false; }

            return Add(index, value);
        }


        /// <summary>
        /// Attemps to get the components at the given index.
        /// </summary>
        /// <param name="index"> Index of the component to get. </param>
        /// <param name="value"> Value of the component at the given index if it was found, <see langword="default"/> otherwise. </param>
        /// <returns> <see langword="true"/> if the component was found, <see langword="false"/> otherwise. </returns>
        public bool TryGet(int index, out T? value)
        {
            return _components.TryGetValue(index, out value);
        }


        /// <summary>
        /// Replaces the value of a non-zero component in this sparse vector. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>.
        /// </remarks>
        /// <param name="index"> Index of the component to replace. </param>
        /// <param name="value"> Value for the component to replace. </param>
        /// <returns> <see langword="true"/> if the component was replaced, <see langword="false"/> otherwise. </returns>
        public bool Replace(int index, T value)
        {
            _components[index] = value;

            return true;
        }

        /// <summary>
        /// Replaces the value of a non-zero component in this sparse vector. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <param name="index"> Index of the component to replace. </param>
        /// <param name="value"> Value for the component to replace. </param>
        /// <param name="nonZeroCheck"> Evaluates whether the <paramref name="value"/> should be checked for the non-zero condition. </param>
        /// <returns> <see langword="true"/> if the component was replaced, <see langword="false"/> otherwise. </returns>
        public bool Replace(int index, T value, bool nonZeroCheck)
        {
            if (nonZeroCheck && value == T.Zero) { return false; }

            return Replace(index, value);
        }


        /// <summary>
        /// Removes a non-zero component in this sparse vector. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <param name="index"> Index of the component to remmove. </param>
        /// <returns> <see langword="true"/> if the component was removed, <see langword="false"/> otherwise. </returns>
        public bool Remove(int index)
        {
            return _components.Remove(index);
        }


        /// <summary>
        /// Evaluates whether the sparse storage contains an entry for the component at the given index.
        /// </summary>
        /// <param name="index"> Index of the component to retrieve. </param>
        /// <returns> <see langword="true"/> if the sparse storage contains an entry at the given index, <see langword="false"/> otherwise.  </returns>
        public bool Contains(int index)
        {
            return _components.ContainsKey(index);
        }


        /// <inheritdoc/>
        /// <remarks> If the sparse storage does not have an existing entry for the component, zero is returned. </remarks>
        public override T GetComponent(int index)
        {
            return _components.TryGetValue(index, out T? value) ? value : T.Zero;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// If the sparse storage does not have an existing entry for the component, it is added when the value is not zero. <br/>
        /// If the sparse storage has an existing entry for the component, it either removed or replaced depending on whether the value is zero or not.
        /// </remarks>
        public override void SetComponent(int index, T value)
        {
            if (_components.ContainsKey(index))
            {
                if (value != T.Zero) { Replace(index, value, false); }
                else { Remove(index); }
            }
            else
            {
                if (value != T.Zero) { Add(index, value); }
            }
        }


        /// <inheritdoc cref="Vector{T}.ToArray()"/>
        public override T[] ToArray()
        {
            T[] result = new T[Size];
            foreach ((int index, T value) in NonZeros()) { result[index] = value; }

            return result;
        }


        /// <summary>
        /// Creates an enumerator reading through the non-zero components of this <see cref="SparseVector{T}"/>. <br/>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> is composed of the index and the component value.
        /// </summary>
        /// <returns> The enumerator of the <see cref="SparseVector{T}"/>. </returns>
        public IEnumerable<(int Index, T Component)> NonZeros()
        {
            var kvp_Enumerator = _components.GetEnumerator();
            try
            {
                while (kvp_Enumerator.MoveNext())
                {
                    var kvp = kvp_Enumerator.Current;
                    yield return (kvp.Key, kvp.Value);
                }
            }
            finally { kvp_Enumerator.Dispose(); }
        }

        #endregion


        #region Protected Inheritence : Vector<T>

        //     -----     -----     Additive Abelian Group : Vector<T>     -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The addition of this sparse vector with <paramref name="right"/> as a <see cref="Vector{T}"/> is not implemented. 
        /// </exception>
        protected override Vector<T> VectorAdditionOperator(Vector<T> right)
        {
            if (right is SparseVector<T> sparseRight) { return this + sparseRight; }
            else if(right is DenseVector<T> denseRight) { return this + denseRight; }
            else
            {
                throw new NotImplementedException($"The addition of this sparse vector with a {right.GetType()} as a {typeof(Vector<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of this sparse vector with <paramref name="right"/> as a <see cref="Vector{T}"/> is not implemented. 
        /// </exception>
        protected override Vector<T> VectorSubtractionOperator(Vector<T> right)
        {
            if (right is SparseVector<T> sparseRight) { return this - sparseRight; }
            else if(right is DenseVector<T> denseRight) { return this - denseRight; }
            else
            {
                throw new NotImplementedException($"The subtraction of this sparse vector with a {right.GetType()} as a {typeof(Vector<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        protected override Vector<T> VectorUnaryNegationOperator() => -this;


        //     -----     Other Operations : Vector<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">
        /// The multiplication of this transposed sparse vector with <paramref name="right"/> as a <see cref="Vector{T}"/> is not implemented.
        /// </exception>
        protected override T VectorTransposeMultiply(Vector<T> right)
        {
            if (right is SparseVector<T> sparseRight) { return SparseVector<T>.TransposeMultiply(this, sparseRight); }
            else if (right is DenseVector<T> denseRight) { return SparseVector<T>.TransposeMultiply(this, denseRight); }
            else
            {
                throw new NotImplementedException($"The multiplication of this transposed sparse vector with a {right.GetType()} as a {typeof(Vector<T>)} is not implemented.");
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
