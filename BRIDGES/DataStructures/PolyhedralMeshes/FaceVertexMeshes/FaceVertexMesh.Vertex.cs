using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMeshes
{
    public partial class FaceVertexMesh<TVertexTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents a vertex in a generic face-vertex mesh data-structure.
        /// </summary>
        public new class Vertex : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>.Vertex
        {
            #region Fields

            /// <summary>
            /// Mesh containing this vertex.
            /// </summary>
            protected readonly new FaceVertexMesh<TVertexTraits, TEdgeTraits, TFaceTraits> _mesh;

            /// <summary>
            /// List of <see cref="Edge"/> connected to this vertex.
            /// </summary>
            /// <remarks> This field is not necessary in the face-vertex mesh data-structure, but it simplifies and speeds up methods. </remarks>
            protected List<Edge> _connectedEdges;

            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Vertex"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the vertex. </param>
            /// <param name="index"> Index for the vertex. </param>
            /// <param name="traits"> Traits for the vertex. </param>
            protected internal Vertex(FaceVertexMesh<TVertexTraits, TEdgeTraits, TFaceTraits> mesh, int index, TVertexTraits traits)
                :  base(mesh, index, traits)
            {
                // Instantiate field
                _connectedEdges = new List<Edge>();

                // Initialise field
                _mesh = mesh;
            }

            #endregion

            #region Public Methods

            //     -----     About Vertices     -----     //

            /// <summary>
            /// Evaluates whether this vertex is on a boundary.
            /// </summary>
            /// <remarks> An isolated vertex will be considered as being on a boundary. </remarks>
            /// <returns> <see langword="true"/> if the vertex is on a boundary, <see langword="false"/> otherwise. </returns>
            public override bool IsBoundary()
            {
                int edgeValency = _connectedEdges.Count;

                if (edgeValency == 0) { return true; }

                for (int i_CE = 0; i_CE < edgeValency; i_CE++)
                {
                    if (_connectedEdges[i_CE].IsBoundary()) { return true; }
                }

                return false;
            }

            /// <summary>
            /// Evaluates whether this vertex is connected to an edge.
            /// </summary>
            /// <returns> <see langword="true"/> if the vertex is connected to at least one edge, <see langword="false"/> otherwise. </returns>
            public override bool IsConnected()
            {
                return _connectedEdges.Count != 0;
            }

            /// <summary>
            /// Calculates the valency of this vertex.
            /// </summary>
            /// <returns> The number of edges connected to this vertex. </returns>
            public override int Valency()
            {
                return _connectedEdges.Count;
            }


            //     -----     About Edges     -----     //

            /// <summary>
            /// Retrieves the edges connected to this vertex.
            /// </summary>
            /// <returns> The set of connected <see cref="Edge"/>. An empty set can be returned. </returns>
            public override IReadOnlyList<Edge> ConnectedEdges()
            {
                return _connectedEdges;
            }


            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this vertex.
            /// </summary>
            /// <returns> The set of adjacent <see cref="Face"/>. An empty set can be returned. </returns>
            public override IReadOnlyList<Face> AdjacentFaces()
            {
                int edgeValency = _connectedEdges.Count;

                List<Face> result = new List<Face>(edgeValency);

                for (int i_CE = 0; i_CE < edgeValency; i_CE++)
                {
                    Edge edge = _connectedEdges[i_CE];

                    IReadOnlyList<Face> edgeFaces = edge.AdjacentFaces();
                    for (int i_EF = 0; i_EF < edgeFaces.Count; i_EF++)
                    {
                        Face edgeFace = edgeFaces[i_EF];

                        if (!result.Contains(edgeFace)) { result.Add(edgeFace); }
                    }
                }

                return result;
            }

            #endregion

            #region Other Methods

            /// <summary>
            /// Adds the reference of the edge to this vertex list of connected edges.
            /// </summary>
            /// <param name="edge"> <see cref="Edge"/> to add. </param>
            internal void AddConnectedEdge(Edge edge)
            {
                _connectedEdges.Add(edge);
            }

            /// <summary>
            /// Removes the reference of the edge from this vertex list of connected edges.
            /// </summary>
            /// <param name="edge"> <see cref="Edge"/> to detach. </param>
            internal void DetachConnectedEdge(Edge edge)
            {
                _connectedEdges.Remove(edge);
            }

            /// <summary>
            /// Unsets all the fields of this vertex.
            /// </summary>
            internal override void Unset()
            {
                // Unset fields
                _connectedEdges = new List<Edge>(0); ;

                // Unset properties 
                Index = -1;
            }

            #endregion
        }
    }
} 