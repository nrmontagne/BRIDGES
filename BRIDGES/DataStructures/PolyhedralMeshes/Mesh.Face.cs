using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    public abstract partial class Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents a face in a generic mesh data-structure.
        /// </summary>
        public abstract class Face : IEquatable<Face>
        {
            #region Fields

            /// <summary>
            /// Mesh containing this face.
            /// </summary>
            private readonly Mesh<TVertexTraits, TEdgeTraits, TFaceTraits> _mesh;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the (zero-based) index of this face. 
            /// </summary>
            public int Index { get; protected set; }


            /// <summary>
            /// Gets or sets the traits of this face.
            /// </summary>
            public TFaceTraits Traits { get; set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Face"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the face. </param>
            /// <param name="index"> Index for the face. </param>
            /// <param name="traits"> Traits for the face </param>
            protected internal Face(Mesh<TVertexTraits, TEdgeTraits, TFaceTraits> mesh, int index, TFaceTraits traits)
            {
                // Initialise fields
                _mesh = mesh;

                // Initialise properties
                Index = index;
                Traits = traits;
            }

            #endregion

            #region Public Abstract Methods

            //     -----     About Vertices     -----     //

            /// <summary>
            /// Retrieves the vertices of this face.
            /// </summary>
            /// <returns> The ordered set of face <see cref="Vertex"/>. </returns>
            public abstract IReadOnlyList<Vertex> FaceVertices();


            //     -----     About Edges     -----     //

            /// <summary>
            /// Retrieves the edges of this face.
            /// </summary>
            /// <returns> The ordered set of face <see cref="Edge"/>. </returns>
            public abstract IReadOnlyList<Edge> FaceEdges();


            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this face.
            /// </summary>
            /// <returns> The ordered set of adjacent <see cref="Face"/>. An empty set can be returned. </returns>
            public abstract IReadOnlyList<Face> AdjacentFaces();

            #endregion

            #region Other Abstract Methods

            /// <summary>
            /// Unsets all the fields of this face.
            /// </summary>
            internal abstract void Unset();

            #endregion


            #region Implement : IEquatable<Face>

            /// <summary>
            /// Indicates whether this face is equal to another face.
            /// </summary>
            /// <param name="other"> A <see cref="Face"/> to compare with. </param>
            /// <returns> <see langword="true"/> if this face is equal to the <paramref name="other"/> face, <see langword="false"/> otherwise. </returns>
            public virtual bool Equals(Face? other)
            {
                if (other is null || other.Index == -1) { return false; }
                else { return other._mesh.Equals(_mesh) & other.Index == Index; }
            }

            #endregion


            #region Override : Object

            /// <inheritdoc cref="object.Equals(object)"/>
            public override bool Equals(object? obj)
            {
                return obj is Face face && Equals(face);
            }

            /// <inheritdoc cref="object.GetHashCode()"/>
            public override int GetHashCode()
            {
                return HashCode.Combine(_mesh, Index);
            }

            /// <inheritdoc cref="object.ToString()"/>
            public override string ToString()
            {
                return $"Face {Index}";
            }

            #endregion
        }
    }
}
