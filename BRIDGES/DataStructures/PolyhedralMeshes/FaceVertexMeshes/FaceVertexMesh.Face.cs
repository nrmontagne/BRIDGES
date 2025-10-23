using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMeshes
{
    public partial class FaceVertexMesh<TVertexTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents a face in a generic face-vertex mesh data-structure.
        /// </summary>
        public new class Face : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>.Face
        {
            #region Fields

            /// <summary>
            /// Ordered list of face <see cref="Vertex"/>.
            /// </summary>
            protected List<Vertex> _faceVertices;

            /// <summary>
            /// Ordered list of face <see cref="Edge"/>.
            /// </summary>
            /// <remarks> This is not necessary in the face-vertex mesh data structure, but it simplifies and speeds up methods. </remarks>
            protected List<Edge> _faceEdges;

            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Face"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the face. </param>
            /// <param name="index"> Index for the face. </param>
            /// <param name="traits"> Traits for the face. </param>
            /// <param name="faceVertices"> Ordered set for the face <see cref="Vertex"/>. </param>
            /// <param name="faceEdges"> Ordered set for the face <see cref="Edge"/>. </param>
            protected internal Face(Mesh<TVertexTraits, TEdgeTraits, TFaceTraits> mesh, int index, TFaceTraits traits,
                IReadOnlyList<Vertex> faceVertices, IReadOnlyList<Edge> faceEdges)
                : base(mesh, index, traits)
            {
                // Initialise fields
                _faceVertices = new List<Vertex>(faceVertices);
                _faceEdges = new List<Edge>(faceEdges);


                // Add to face edges
                for (int i_E = 0; i_E < _faceEdges.Count; i_E++)
                {
                    _faceEdges[i_E].AddAdjacentFace(this);
                }
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
                return _faceVertices;
            }


            //     -----     About Edges     -----     //

            /// <summary>
            /// Retrieves the edges of this face.
            /// </summary>
            /// <returns> The ordered set of face <see cref="Edge"/>. </returns>
            public override IReadOnlyList<Edge> FaceEdges()
            {
                return _faceEdges;
            }


            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this face.
            /// </summary>
            /// <returns> The ordered set of adjacent <see cref="Face"/>. An empty set can be returned. </returns>
            public override IReadOnlyList<Face> AdjacentFaces()
            {
                int edgeCount = _faceEdges.Count;

                List<Face> result = new List<Face>(edgeCount);

                for (int i_FE = 0; i_FE < edgeCount; i_FE++)
                {
                    Edge edge = _faceEdges[i_FE];

                    IReadOnlyList<Face> edgeFaces = edge.AdjacentFaces();
                    for (int i_EF = 0; i_EF < edgeFaces.Count; i_EF++)
                    {
                        Face edgeFace = edgeFaces[i_EF];

                        if (edgeFace.Equals(this)) { continue; }
                        else if (!result.Contains(edgeFace)) { result.Add(edgeFace); }
                    }
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
                // Unset fields
                _faceVertices = new List<Vertex>(0);
                _faceEdges = new List<Edge>(0);

                // Unset properties
                Index = -1;
            }

            #endregion
        }
    }
}