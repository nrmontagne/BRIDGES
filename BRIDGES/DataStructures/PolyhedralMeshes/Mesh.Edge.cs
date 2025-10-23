using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    public abstract partial class Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents an edge in a generic mesh data-structure.
        /// </summary>
        public abstract class Edge : IEquatable<Edge>
        {
            #region Fields

            /// <summary>
            /// Mesh containing this edge.
            /// </summary>
            protected readonly Mesh<TVertexTraits, TEdgeTraits, TFaceTraits> _mesh;

            /// <summary>
            /// Traits of this edge.
            /// </summary>
            protected TEdgeTraits? _traits;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the (zero-based) index of this edge. 
            /// </summary>
            public int Index { get; protected set; }


            /// <summary>
            /// Gets the start vertex of this edge.
            /// </summary>
            public virtual Vertex Start { get; }

            /// <summary>
            /// Gets the end vertex of this edge.
            /// </summary>
            public virtual Vertex End { get; }


            /// <summary>
            /// Gets or sets the traits of this edge.
            /// </summary>
            public TEdgeTraits Traits
            {
                get => _traits is not null ? _traits : throw new NullReferenceException("This edge traits has either not been instantiated or has been unset for the edge's removal");
                set => _traits = value;
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Edge"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the edge. </param>
            /// <param name="index"> Index for the edge. </param>
            /// <param name="start"> Start <see cref="Vertex"/> for the edge. </param>
            /// <param name="end"> End <see cref="Vertex"/> for the edge. </param>
            /// <param name="traits"> Traits for the edge </param>
            /// 
            protected internal Edge(Mesh<TVertexTraits, TEdgeTraits, TFaceTraits> mesh, int index,
                Vertex start, Vertex end, TEdgeTraits? traits)
            {
                // Initialise field
                _mesh = mesh;

                // Initialise properties
                Index = index;

                Start = start;
                End = end;

                _traits = traits;
            }

            #endregion

            #region Public Abstract Methods

            //     -----     About Edges     -----     //

            /// <summary>
            /// Evaluates whether this edge is on a boundary.
            /// </summary>
            /// <returns> <see langword="true"/> if the edge is on a boundary, <see langword="false"/> otherwise. </returns>
            public abstract bool IsBoundary();


            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this edge.
            /// </summary>
            /// <returns> The set of adjacent <see cref="Face"/>. </returns>
            public abstract IReadOnlyList<Face> AdjacentFaces();

            #endregion

            #region Other Abstract Methods

            /// <summary>
            /// Unsets all the fields of this edge.
            /// </summary>
            internal abstract void Unset();

            #endregion


            #region Implement : IEquatable<Edge>

            /// <summary>
            /// Indicates whether this edge is equal to another edge.
            /// </summary>
            /// <param name="other"> An <see cref="Edge"/> to compare with. </param>
            /// <returns> <see langword="true"/> if this edge is equal to the <paramref name="other"/> edge, <see langword="false"/> otherwise. </returns>
            public virtual bool Equals(Edge? other)
            {
                if (other is null || other.Index == -1) { return false; }
                else { return other._mesh.Equals(_mesh) & other.Index == Index; }
            }

            #endregion


            #region Override : Object

            /// <inheritdoc cref="object.Equals(object)"/>
            public override bool Equals(object? obj)
            {
                return obj is Edge edge && Equals(edge);
            }

            /// <inheritdoc cref="object.GetHashCode()"/>
            public override int GetHashCode()
            {
                return HashCode.Combine(_mesh, Index);
            }

            /// <inheritdoc cref="object.ToString()"/>
            public override string ToString()
            {
                return $"Edge {Index}";
            }

            #endregion
        }
    }
}
