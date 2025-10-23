using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMeshes
{
    public partial class HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents an edge in a generic halfedge mesh data-structure.
        /// </summary>
        public new class Edge : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>.Edge
        {
            #region Fields

            /// <summary>
            /// The <see cref="Halfedge"/> whose index is twice the index of this edge.
            /// </summary>
            private Halfedge? _correspondingHalfedge;


            /// <summary>
            /// Start <see cref="Vertex"/> of this edge.
            /// </summary>
            private Vertex? _start;

            /// <summary>
            /// End <see cref="Vertex"/> of this edge.
            /// </summary>
            private Vertex? _end;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the corresponding halfedge of this edge.
            /// </summary>
            /// <remarks> The halfedge whose index is twice the index of this edge. </remarks>
            public Halfedge CorrespondingHalfedge
            {
                get => _correspondingHalfedge is not null ? _correspondingHalfedge : throw new NullReferenceException("This edge has been unset for removal.");
            }


            /// <summary>
            /// Gets the start vertex of this edge.
            /// </summary>
            public override Vertex Start
            {
                get => _start is not null ? _start : throw new NullReferenceException("This edge has been unset for removal.");
            }

            /// <summary>
            /// Gets the end vertex of this edge.
            /// </summary>
            public override Vertex End
            {
                get => _end is not null ? _end : throw new NullReferenceException("This edge has been unset for removal.");
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Edge"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the edge. </param>
            /// <param name="halfedge"> Corresponding <see cref="Halfedge"/> for the edge. It is the halfedge whose index is twice the index of the edge to create. </param>
            /// <param name="traits"> Traits for the edge. </param>
            protected internal Edge(HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> mesh, Halfedge halfedge, TEdgeTraits? traits)
                : base(mesh, halfedge.Index / 2, halfedge.Start, halfedge.End, traits)
            {
                // Initialise field
                _correspondingHalfedge = halfedge;

                // Initialise properties
                _start = halfedge.Start;
                _end = halfedge.End;
            }

            #endregion

            #region Public Methods

            //     -----     About Edges     -----     //

            /// <summary>
            /// Evaluates whether this edge is on a boundary.
            /// </summary>
            /// <returns> <see langword="true"/> if the edge is on a boundary, <see langword="false"/> otherwise. </returns>
            public override bool IsBoundary()
            {
                return CorrespondingHalfedge.IsBoundary() || CorrespondingHalfedge.Pair.IsBoundary();
            }
            

            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this edge.
            /// </summary>
            /// <returns> The set of adjacent <see cref="Face"/>. </returns>
            public override IReadOnlyList<Face> AdjacentFaces()
            {
                List<Face> adjacentFaces = new List<Face>(2);
                if (CorrespondingHalfedge.AdjacentFace is not null) { adjacentFaces.Add(CorrespondingHalfedge.AdjacentFace); }
                if (CorrespondingHalfedge.Pair.AdjacentFace is not null) { adjacentFaces.Add(CorrespondingHalfedge.Pair.AdjacentFace); }

                return adjacentFaces;
            }

            #endregion

            #region Other Methods

            /// <summary>
            /// Unsets all the fields of this edge.
            /// </summary>
            internal override void Unset()
            {
                // Unset fields
                _correspondingHalfedge = null;
                _start = null;
                _end = null;

                // Unset property
                Index = -1;
            }

            #endregion
        }
    }
}
