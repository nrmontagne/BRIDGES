using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMeshes
{
    public partial class HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        /// <summary>
        /// Represents a halfedge in a generic halfedge mesh data-structure.
        /// </summary>
        public class Halfedge : IEquatable<Halfedge>
        {
            #region Fields

            /// <summary>
            /// Mesh containing this halfedge.
            /// </summary>
            protected readonly Mesh<TVertexTraits, TEdgeTraits, TFaceTraits> _mesh;


            /// <summary>
            /// <see cref="Edge"/> whose index is half the current halfedge's index (rounded towards zero).
            /// </summary>
            internal Edge? _correspondingEdge;


            /// <summary>
            /// Start <see cref="Vertex"/> of this edge.
            /// </summary>
            private Vertex? _start;

            /// <summary>
            /// End <see cref="Vertex"/> of this edge.
            /// </summary>
            private Vertex? _end;


            /// <summary>
            /// Previous <see cref="Halfedge"/> of this halfedge.
            /// </summary>
            internal Halfedge? _previous;

            /// <summary>
            /// Next <see cref="Halfedge"/> of this halfedge.
            /// </summary>
            internal Halfedge? _next;

            /// <summary>
            /// Pair <see cref="Halfedge"/> of this halfedge.
            /// </summary>
            internal Halfedge? _pair;

            /// <summary>
            /// Adjacent <see cref="Face"/> of this halfedge.
            /// </summary>
            internal Face? _adjacent;


            /// <summary>
            /// Traits of this halfedge.
            /// </summary>
            protected THalfedgeTraits? _traits;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the index of this halfedge. 
            /// </summary>
            public int Index { get; protected set; }


            /// <summary>
            /// Gets the start vertex of this edge.
            /// </summary>
            public Vertex Start
            {
                get => _start is not null ? _start : throw new NullReferenceException("This halfedge has been unset for removal.");
            }

            /// <summary>
            /// Gets the end vertex of this edge.
            /// </summary>
            public Vertex End
            {
                get => _end is not null ? _end : throw new NullReferenceException("This halfedge has been unset for removal.");
            }


            /// <summary>
            /// Gets the previous halfedge of this halfedge.
            /// </summary>
            public Halfedge Previous
            {
                get => _previous is not null ? _previous : throw new NullReferenceException("This halfedge has been unset for removal.");
            }

            /// <summary>
            /// Gets the next halfedge of this halfedge.
            /// </summary>
            public Halfedge Next
            {
                get => _next is not null ? _next : throw new NullReferenceException("This halfedge has been unset for removal.");
            }

            /// <summary>
            /// Gets the pair halfedge of this halfedge.
            /// </summary>
            public Halfedge Pair
            {
                get => _pair is not null ? _pair : throw new NullReferenceException("This halfedge has been unset for removal.");
            }


            /// <summary>
            /// Gets the adjacent face of this halfedge.
            /// </summary>
            public Face? AdjacentFace 
            { 
                get => _adjacent; 
            }


            /// <summary>
            /// Gets or sets the traits of this halfedge.
            /// </summary>
            public THalfedgeTraits Traits
            {
                get => _traits is not null ? _traits : throw new NullReferenceException("This halfedge traits has either not been instantiated or has been unset for the halfedge's removal");
                set => _traits = value;
            }


            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="Halfedge"/> class.
            /// </summary>
            /// <param name="mesh"> Parent <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> for the halfedge. </param>
            /// <param name="index"> Index for the halfedge. </param>
            /// <param name="traits"> Traits for the halfedge. </param>
            /// <param name="start"> Start <see cref="Vertex"/> for the edge. </param>
            /// <param name="end"> End <see cref="Vertex"/> for the edge. </param>
            protected internal Halfedge(HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> mesh, int index,
                Vertex start, Vertex end, THalfedgeTraits? traits)
            {
                // Nullify fields
                _previous = null;
                _next = null;
                _pair = null;

                _adjacent = null;

                // Initialise field
                _mesh = mesh;

                // Initialise properties
                Index = index;

                _start = start;
                _end = end;

                _traits = traits;
            }

            #endregion

            #region Public Methods

            //     -----     About Halfedges     -----     //

            /// <summary>
            /// Evaluates whether this halfedge is on a boundary.
            /// </summary>
            /// <returns> <see langword="true"/> if the edge is on a boundary, <see langword="false"/> otherwise. </returns>
            public bool IsBoundary()
            {
                return AdjacentFace is null;
            }

            //     -----     About Edges     -----     //

            /// <summary>
            /// Retrieves the edge corresponding to this halfedge and its pair.
            /// </summary>
            /// <remarks> It is the edge whose index is half this halfedge's index (rounded towards zero). </remarks>
            /// <returns> The <see cref="Edge"/> representing this halfedge. </returns>
            /// <exception cref="InvalidOperationException"> The edge corresponding to this halfedge has either not been instantiated or has been unset for the halfedge's removal. </exception>
            public Edge GetEdge()
            {
                // Creates the halfedge if it was not pre-existing.
                if (_correspondingEdge is null) 
                {
                    int edgeIndex = Index / 2;
                    int parity = Index - (2 * edgeIndex);
                    if (parity == 0)
                    {
                        _correspondingEdge = new Edge((HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits>) _mesh, this, default);
                        Pair._correspondingEdge = _correspondingEdge;
                    }
                    else
                    {
                        Pair._correspondingEdge = new Edge((HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits>)_mesh, Pair, default);
                        _correspondingEdge = Pair._correspondingEdge;
                    }
                }

                return _correspondingEdge;
            }

            #endregion

            #region Other Methods

            /// <summary>
            /// Unsets all the fields of this halfedge.
            /// </summary>
            internal void Unset()
            {
                // Unset fields
                if (_correspondingEdge is not null)
                {
                    _correspondingEdge.Unset();
                    _correspondingEdge = null;
                }

                _start = null;
                _end = null;

                _previous = null;
                _next = null;
                _pair = null;

                _adjacent = null;

                // Unset property
                Index = -1;
            }

            #endregion


            #region Implement : IEquatable<Halfedge>

            /// <summary>
            /// Indicates whether this halfedge is equal to another halfedge.
            /// </summary>
            /// <param name="other"> A <see cref="Halfedge"/> to compare with. </param>
            /// <returns> <see langword="true"/> if this halfedge is equal to the <paramref name="other"/> halfedge, <see langword="false"/> otherwise. </returns>
            public bool Equals(Halfedge? other)
            {
                if (other is null || other.Index == -1) { return false; }
                else { return other._mesh.Equals(_mesh) & other.Index == Index; }
            }

            #endregion
        }
    }
}
