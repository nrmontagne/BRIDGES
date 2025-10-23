using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMeshes
{
    public partial class HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents a face in a generic halfedge mesh data-structure.
        /// </summary>
        public new class Face : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>.Face
        {
            #region Fields

            /// <summary>
            /// First <see cref="Halfedge"/> of this face.
            /// </summary>
            internal Halfedge? _firstHalfedge;

            #endregion

            #region Properties

            /// <summary>
            /// Gets a halfedge around this face.
            /// </summary>
            public Halfedge FirstHalfedge
            {
                get => _firstHalfedge is not null ? _firstHalfedge : throw new NullReferenceException("This face has been unset for removal.");
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Face"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the face. </param>
            /// <param name="index"> Index for the face. </param>
            /// <param name="traits"> Traits for the face. </param>
            protected internal Face(HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> mesh, int index, TFaceTraits traits)
                : base(mesh, index, traits)
            {
                // Nothing to do
            }

            /// <summary>
            /// Initialises a new instance of the <see cref="Face"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the face. </param>
            /// <param name="index"> Index for the face. </param>
            /// <param name="halfedge"> Ordered set for the face <see cref="Halfedge"/>. </param>
            /// <param name="traits"> Traits for the face. </param>
            protected internal Face(HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> mesh, int index, Halfedge halfedge, TFaceTraits traits)
                : base(mesh, index, traits)
            {
                _firstHalfedge = halfedge;
            }

            #endregion

            #region Public Methods

            //     -----     About Vertices     -----     //

            /// <summary>
            /// Retrieves the vertices of this face.
            /// </summary>
            /// <returns> The ordered set of face <see cref="Vertex"/>. </returns>
            public override IReadOnlyList<Vertex> FaceVertices()
            {
                List<Vertex> result = new List<Vertex>();

                Halfedge firstHalfedge = FirstHalfedge;
                result.Add(firstHalfedge.Start);

                Halfedge halfedge = firstHalfedge.Next;
                while (!firstHalfedge.Equals(halfedge))
                {
                    result.Add(halfedge.Start);
                    halfedge = halfedge.Next;
                }

                return result;
            }


            //     -----     About Halfedges     -----     //

            /// <summary>
            /// Retrieves the halfedges of this face.
            /// </summary>
            /// <returns> The ordered set of face <see cref="Halfedge"/>. </returns>
            public IReadOnlyList<Halfedge> FaceHalfedges()
            {
                List<Halfedge> result = new List<Halfedge>();

                result.Add(FirstHalfedge);

                Halfedge halfedge = FirstHalfedge.Next;
                while (!FirstHalfedge.Equals(halfedge))
                {
                    result.Add(halfedge);
                    halfedge = halfedge.Next;
                }

                return result;
            }


            //     -----     About Edges     -----     //

            /// <summary>
            /// Retrieves the edges of this face.
            /// </summary>
            /// <returns> The ordered set of face <see cref="Edge"/>. </returns>
            public override IReadOnlyList<Edge> FaceEdges()
            {
                List<Edge> result = new List<Edge>();

                Halfedge firstHalfedge = FirstHalfedge;
                Edge firstEdge = firstHalfedge.GetEdge();
                result.Add(firstEdge);

                Halfedge halfedge = firstHalfedge.Next;

                while (!firstHalfedge.Equals(halfedge))
                {
                    Edge edge = halfedge.GetEdge();

                    result.Add(edge);
                    halfedge = halfedge.Next;
                }

                return result;
            }


            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this face.
            /// </summary>
            /// <returns> The ordered set of adjacent <see cref="Face"/>. An empty set can be returned. </returns>
            public override IReadOnlyList<Face> AdjacentFaces()
            {
                List<Face> result = new List<Face>();

                if (FirstHalfedge.Pair.AdjacentFace is not null) { result.Add(FirstHalfedge.Pair.AdjacentFace); }
                
                Halfedge halfedge = FirstHalfedge.Next;
                while (!FirstHalfedge.Equals(halfedge))
                {
                    if (halfedge.Pair.AdjacentFace is not null) { result.Add(halfedge.Pair.AdjacentFace); }
                    halfedge = halfedge.Next;
                }

                return result;
            }

            #endregion

            #region Other Methods

            /// <summary>
            /// Unsets all the fields of this face.
            /// </summary>
            internal override void Unset()
            {
                // Unset field
                _firstHalfedge = null;

                // Unset property
                Index = -1;
            }

            #endregion
        }
    }
}
