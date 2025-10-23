using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMeshes
{
    public partial class FaceVertexMesh<TVertexTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents an edge in a generic face-vertex mesh data-structure.
        /// </summary>
        public new class Edge : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>.Edge
        {
            #region Fields

            /// <summary>
            /// Start <see cref="Vertex"/> of this edge.
            /// </summary>
            private Vertex? _start;

            /// <summary>
            /// End <see cref="Vertex"/> of this edge.
            /// </summary>
            private Vertex? _end;


            /// <summary>
            /// List of <see cref="Face"/> adjacent to this edge.
            /// </summary>
            /// <remarks> This is not necessary in the face-vertex mesh data structure, but it simplifies and speeds up methods. </remarks>
            protected List<Face> _adjacentFaces;

            #endregion

            #region Properties

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
            /// <param name="index"> Index for the edge. </param>
            /// <param name="start"> Start <see cref="Vertex"/> for the edge. </param>
            /// <param name="end"> End <see cref="Vertex"/> for the edge. </param>
            /// <param name="traits"> Traits for the edge. </param>
            protected internal Edge(FaceVertexMesh<TVertexTraits, TEdgeTraits, TFaceTraits> mesh, int index,
                Vertex start, Vertex end, TEdgeTraits? traits)
                : base(mesh, index, start, end, traits)
            {
                // Instantiate field
                _adjacentFaces = new List<Face>(2);

                // Initialise properties
                _start = start;
                _end = end;


                // Add to end vertices
                start.AddConnectedEdge(this);
                end.AddConnectedEdge(this);
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
                return _adjacentFaces.Count < 2;
            }


            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this edge.
            /// </summary>
            /// <returns> The set of adjacent <see cref="Face"/>. </returns>
            public override IReadOnlyList<Face> AdjacentFaces()
            {
                return _adjacentFaces;
            }

            #endregion

            #region Other Methods

            /// <summary>
            /// Adds the reference of the face to this edge list of adjacent faces.
            /// </summary>
            /// <param name="face"> <see cref="Face"/> to add. </param>
            /// <exception cref="InvalidOperationException"> The edge can not have more than two adjacent faces. </exception>
            internal void AddAdjacentFace(Face face)
            {
                if(_adjacentFaces.Count == 2) { throw new InvalidOperationException("The edge can not have more than two adjacent faces."); } 
                _adjacentFaces.Add(face);
            }

            /// <summary>
            /// Removes the reference of the face from this edge list of adjacent faces.
            /// </summary>
            /// <param name="face"> <see cref="Face"/> to detach. </param>
            internal void DetachAdjacentFace(Face face)
            {
                _adjacentFaces.Remove(face);
            }

            /// <summary>
            /// Unsets all the fields of this edge.
            /// </summary>
            internal override void Unset()
            {
                // Unset fields
                _adjacentFaces = new List<Face>(0);
                _start = null;
                _end = null;
                _traits = default;

                // Unset properties
                Index = -1;
            }

            #endregion
        }
    }
}