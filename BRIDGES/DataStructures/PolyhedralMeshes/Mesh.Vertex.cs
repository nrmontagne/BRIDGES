using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    public abstract partial class Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents a vertex in a generic mesh data-structure.
        /// </summary>
        public abstract class Vertex : IEquatable<Vertex>
        {
            #region Fields

            /// <summary>
            /// Mesh containing this vertex.
            /// </summary>
            protected readonly Mesh<TVertexTraits, TEdgeTraits, TFaceTraits> _mesh;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the (zero-based) index of this vertex. 
            /// </summary>
            public int Index { get; protected set; }

            /// <summary>
            /// Gets or sets the traits of this vertex.
            /// </summary>
            public TVertexTraits Traits { get; set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Vertex"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the vertex. </param>
            /// <param name="index"> Index for the vertex. </param>
            /// <param name="traits"> Traits for the vertex </param>
            protected internal Vertex(Mesh<TVertexTraits, TEdgeTraits, TFaceTraits> mesh, int index, TVertexTraits traits)
            {
                // Initialise field
                _mesh = mesh;

                // Initialise properties
                Index = index;
                Traits = traits;
            }

            #endregion

            #region Abstract Methods

            //     -----     About Vertices     -----     //

            /// <summary>
            /// Evaluates whether this vertex is on a boundary.
            /// </summary>
            /// <remarks> An isolated vertex will be considered as being on a boundary. </remarks>
            /// <returns> <see langword="true"/> if the vertex is on a boundary, <see langword="false"/> otherwise. </returns>
            public abstract bool IsBoundary();

            /// <summary>
            /// Evaluates whether this vertex is connected to an edge.
            /// </summary>
            /// <returns> <see langword="true"/> if the vertex is connected to at least one edge, <see langword="false"/> otherwise. </returns>
            public abstract bool IsConnected();

            /// <summary>
            /// Calculates the valency of this vertex.
            /// </summary>
            /// <returns> The number of edges connected to this vertex. </returns>
            public abstract int Valency();


            //     -----     About Edges     -----     //

            /// <summary>
            /// Retrieves the edges connected to this vertex.
            /// </summary>
            /// <returns> The set of connected <see cref="Edge"/>. An empty set can be returned. </returns>
            public abstract IReadOnlyList<Edge> ConnectedEdges();


            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this vertex.
            /// </summary>
            /// <returns> The set of adjacent <see cref="Face"/>. An empty set can be returned. </returns>
            public abstract IReadOnlyList<Face> AdjacentFaces();

            #endregion

            #region Other Abstract Methods

            /// <summary>
            /// Unsets all the fields of this vertex.
            /// </summary>
            internal abstract void Unset();

            #endregion


            #region Implement : IEquatable<Vertex>

            /// <summary>
            /// Indicates whether this vertex is equal to another vertex.
            /// </summary>
            /// <param name="other"> A <see cref="Vertex"/> to compare with. </param>
            /// <returns> <see langword="true"/> if this vertex is equal to the <paramref name="other"/> vertex, <see langword="false"/> otherwise. </returns>
            public virtual bool Equals(Vertex? other)
            {
                if (other is null || other.Index == -1) { return false; }
                else { return other._mesh.Equals(_mesh) & other.Index == Index; }
            }

            #endregion


            #region Override : Object

            /// <inheritdoc cref="object.Equals(object)"/>
            public override bool Equals(object? obj)
            {
                return obj is Vertex vertex && Equals(vertex);
            }


            /// <inheritdoc cref="object.GetHashCode()"/>
            public override int GetHashCode()
            {
                return HashCode.Combine(_mesh, Index);
            }

            /// <inheritdoc cref="object.ToString()"/>
            public override string ToString()
            {
                return $"Vertex {Index}";
            }

            #endregion
        }
    }
}
