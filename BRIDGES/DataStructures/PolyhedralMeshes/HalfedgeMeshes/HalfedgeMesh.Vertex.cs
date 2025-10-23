using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMeshes
{
    public partial class HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents a vertex in a generic halfedge mesh data-structure.
        /// </summary>
        public new class Vertex : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>.Vertex
        {
            #region Fields

            /// <summary>
            /// Mesh containing this vertex.
            /// </summary>
            protected readonly new HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> _mesh;

            /// <summary>
            /// Outgoing <see cref="Halfedge"/> of this vertex
            /// </summary>
            /// <remarks> If the vertex is on a boundary, the outgoing halfedge must be on the boundary. </remarks>
            internal Halfedge? _outgoing;

            #endregion

            #region Properties

            /// <summary>
            /// Gets an outgoing halfedge of this vertex.
            /// </summary>
            /// <remarks> If the vertex is on a boundary, the outgoing halfedge must be on the boundary. </remarks>
            public Halfedge? Outgoing 
            {
                get => _outgoing; 
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Vertex"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the vertex. </param>
            /// <param name="index"> Index for the vertex. </param>
            /// <param name="traits"> Traits for the vertex. </param>
            protected internal Vertex(HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> mesh, int index, TVertexTraits traits)
                : base(mesh, index, traits)
            {
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
                if (Outgoing is null || Outgoing.IsBoundary()) { return true; }
                else { return false; }
            }

            /// <summary>
            /// Evaluates whether this vertex is connected to an edge.
            /// </summary>
            /// <returns> <see langword="true"/> if the vertex is connected to at least one edge, <see langword="false"/> otherwise. </returns>
            public override bool IsConnected()
            {
                return !(Outgoing is null);
            }

            /// <summary>
            /// Calculates the valency of this vertex.
            /// </summary>
            /// <returns> The number of edges connected to this vertex. </returns>
            public override int Valency()
            {
                return OutgoingHalfedges().Count;
            }


            //     -----     About Halfedges      -----     //

            /// <summary>
            /// Retrieves the halfedges leaving this vertex.
            /// </summary>
            /// <returns> The set of outgoing <see cref="Halfedge"/>. An empty set can be returned. </returns>
            public IReadOnlyList<Halfedge> OutgoingHalfedges()
            {
                List<Halfedge> result = new List<Halfedge>();

                // If the vertex is not connected
                if (Outgoing is null) { return result; }


                Halfedge firstOutgoing = Outgoing;
                result.Add(firstOutgoing);

                Halfedge outgoing = firstOutgoing.Previous.Pair;

                while (!firstOutgoing.Equals(outgoing))
                {
                    result.Add(outgoing);
                    outgoing = outgoing.Previous.Pair;
                }

                return result;
            }

            /// <summary>
            /// Retrieves the halfedges coming to this vertex.
            /// </summary>
            /// <returns> The set of incoming <see cref="Halfedge"/>. An empty set can be returned. </returns>
            public IReadOnlyList<Halfedge> IncomingHalfedges()
            {
                List<Halfedge> result = new List<Halfedge>();

                // If the vertex is not connected
                if (Outgoing is null) { return result; }

                Halfedge firstIncoming = Outgoing.Pair;
                result.Add(firstIncoming);

                Halfedge incoming = firstIncoming.Pair.Previous;

                while (!firstIncoming.Equals(incoming))
                {
                    result.Add(incoming);
                    incoming = incoming.Pair.Previous;
                }
                return result;
            }


            //     -----     About Edges     -----     //

            /// <summary>
            /// Retrieves the edges connected to this vertex.
            /// </summary>
            /// <returns> The set of connected <see cref="Edge"/>. An empty set can be returned. </returns>
            public override IReadOnlyList<Edge> ConnectedEdges()
            {
                List<Edge> result = new List<Edge>();

                // If the vertex is not connected
                if (Outgoing is null) { return result; }


                Halfedge firstOutgoing = Outgoing;
                Edge firstEdge = firstOutgoing.GetEdge();
                result.Add(firstEdge);

                Halfedge outgoing = firstOutgoing.Previous.Pair;

                while (!firstOutgoing.Equals(outgoing))
                {
                    Edge edge = outgoing.GetEdge();

                    result.Add(edge);
                    outgoing = outgoing.Previous.Pair;
                }

                return result;
            }


            //     -----     About Faces     -----     //

            /// <summary>
            /// Retrieves the faces adjacent to this vertex.
            /// </summary>
            /// <returns> The set of adjacent <see cref="Face"/>. An empty set can be returned. </returns>
            public override IReadOnlyList<Face> AdjacentFaces()
            {
                IReadOnlyList<Halfedge> outgoings = OutgoingHalfedges();

                int edgeValency = outgoings.Count;
                List<Face> result = new List<Face>(edgeValency);

                for (int i_OHe = 0; i_OHe < edgeValency; i_OHe++)
                {
                    Halfedge outgoing = outgoings[i_OHe];

                    if (!(outgoing.AdjacentFace is null)) { result.Add(outgoing.AdjacentFace); }
                }

                return result;
            }

            #endregion

            #region Other Methods

            /// <summary>
            /// Unsets all the fields of this vertex.
            /// </summary>
            internal override void Unset()
            {
                // Unset field
                _outgoing = null;

                // Unset property
                Index = -1;
            }

            #endregion
        }
    }
}
