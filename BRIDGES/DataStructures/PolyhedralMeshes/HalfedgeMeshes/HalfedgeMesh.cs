using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMeshes
{
    /// <summary>
    /// Represents a generic halfedge mesh data-structure.
    /// </summary>
    /// <typeparam name="TVertexTraits"> Type defining the traits of the mesh vertices. </typeparam>
    /// <typeparam name="THalfedgeTraits"> Type defining the traits of the mesh halfedges. </typeparam>
    /// <typeparam name="TEdgeTraits"> Type defining the traits of the mesh edges. </typeparam>
    /// <typeparam name="TFaceTraits"> Type defining the traits of the mesh faces. </typeparam>
    public partial class HalfedgeMesh<TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        #region Fields

        /// <summary>
        /// Dictionary containing the <see cref="Vertex"/> of this mesh.
        /// <list type="table">
        ///     <item>
        ///         <term> Key </term>
        ///         <description> Index of the <see cref="Vertex"/>. </description>
        ///     </item>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Corresponding <see cref="Vertex"/>. </description>
        ///     </item>
        /// </list>
        /// </summary>
        internal Dictionary<int, Vertex> _vertices;

        /// <summary>
        /// Index for a newly created vertex.
        /// </summary>
        /// <remarks> This may not match with <see cref="VertexCount"/> if vertices were removed from the mesh. </remarks>
        protected int _newVertexIndex;


        /// <summary>
        /// Dictionary containing the <see cref="Halfedge"/> of this mesh.
        /// <list type="table">
        ///     <listheader>
        ///         <term> Key </term>
        ///         <description> Index of the <see cref="Halfedge"/>. </description>
        ///     </listheader>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Corresponding <see cref="Halfedge"/>. </description>
        ///     </item>
        /// </list>
        /// </summary>
        internal Dictionary<int, Halfedge> _halfedges;

        /// <summary>
        /// Index for a newly created halfedge.
        /// </summary>
        /// <remarks> This may not match with <see cref="HalfedgeCount"/> if halfedges were removed from the mesh. </remarks>
        protected int _newHalfedgeIndex;


        /// <summary>
        /// Dictionary containing the <see cref="Face"/> of this mesh.
        /// <list type="table">
        ///     <listheader>
        ///         <term> Key </term>
        ///         <description> Index of the <see cref="Face"/>. </description>
        ///     </listheader>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Corresponding <see cref="Face"/>. </description>
        ///     </item>
        /// </list>
        /// </summary>
        internal Dictionary<int, Face> _faces;

        /// <summary>
        /// Index for a newly created face.
        /// </summary>
        /// <remarks> This may not match with <see cref="FaceCount"/> if faces were removed from the mesh. </remarks>
        protected int _newFaceIndex;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of vertices of this mesh.
        /// </summary>
        public override int VertexCount => _vertices.Count;

        /// <summary>
        /// Gets the number of halfedges of this mesh.
        /// </summary>
        public int HalfedgeCount => _halfedges.Count;

        /// <summary>
        /// Gets the number of edges of this mesh.
        /// </summary>
        public override int EdgeCount => _halfedges.Count / 2;

        /// <summary>
        /// Gets the number of faces of this mesh.
        /// </summary>
        public override int FaceCount => _faces.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="HalfedgeMesh{TVertexTraits, THalfedgeTraits, TEdgeTraits, TFaceTraits}"/> class.
        /// </summary>
        public HalfedgeMesh()
        {
            // Instantiate fields
            _vertices = new Dictionary<int, Vertex>();
            _halfedges = new Dictionary<int, Halfedge>();
            _faces = new Dictionary<int, Face>();

            // Initialise fields
            _newVertexIndex = 0;
            _newHalfedgeIndex = 0;
            _newFaceIndex = 0;
        }

        #endregion

        #region Public Methods

        //     -----     About Vertices     -----     //

        /// <summary>
        /// Adds a vertex to this mesh.
        /// </summary>
        /// <param name="traits"> Traits of the vertex. </param>
        /// <returns> The new <see cref="Vertex"/>. </returns>
        public override Vertex AddVertex(TVertexTraits traits)
        {
            // Creates new instance of vertex.
            Vertex vertex = new Vertex(this, _newVertexIndex, traits);

            _vertices.Add(_newVertexIndex, vertex);

            _newVertexIndex += 1;

            return vertex;
        }


        /// <summary>
        /// Retrieves a vertex in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the vertex. </param>
        /// <returns> The <see cref="Vertex"/> at the given index. </returns>
        public override Vertex GetVertex(int index)
        {
            return _vertices[index];
        }

        /// <summary>
        /// Retrieves a vertex in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the vertex. </param>
        /// <returns> The <see cref="Vertex"/> at the given index, if it exists, <see langword="null"/> otherwise. </returns>
        public override Vertex? TryGetVertex(int index)
        {
            _vertices.TryGetValue(index, out Vertex? vertex);

            return vertex;
        }

        /// <summary>
        /// Exposes an enumerator to iterate through the vertices of this mesh.
        /// </summary>
        /// <returns> An enumerable set of <see cref="Vertex"/>. </returns>
        public override IEnumerable<Vertex> GetVertices()
        {
            return _vertices.Values;
        }


        /// <summary>
        /// Removes a vertex at a given index in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="index"> Index of the vertex. </param>
        public override void RemoveVertex(int index)
        {
            Vertex vertex = GetVertex(index);
            RemoveVertex(vertex);
        }

        /// <summary>
        /// Removes a vertex in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="vertex"> <see cref="Vertex"/> to remove. </param>
        public void RemoveVertex(Vertex vertex)
        {
            IReadOnlyList<Face> adjacentFaces = new List<Face>(vertex.AdjacentFaces());

            for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
            {
                RemoveFace(adjacentFaces[i_AF]);
            }
        }


        //     -----     About Halfedges     -----     //

        /// <summary>
        /// Retrieves a halfedge in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the halfedge. </param>
        /// <returns> The <see cref="Halfedge"/> at the given index. </returns>
        public Halfedge GetHalfedge(int index)
        {
            return _halfedges[index];
        }

        /// <summary>
        /// Retrieves a halfedge in this mesh from its end vertices.
        /// </summary>
        /// <param name="startIndex"> Index of the start vertex. </param>
        /// <param name="endIndex"> Index of the end vertex. </param>
        /// <returns> The <see cref="Halfedge"/> between the given vertices. </returns>
        public Halfedge GetHalfedge(int startIndex, int endIndex)
        {
            Vertex start = GetVertex(startIndex);
            Vertex end = GetVertex(endIndex);
            return GetHalfedge(start, end);
        }

        /// <summary>
        /// Retrieves a halfedge in this mesh from its end vertices.
        /// </summary>
        /// <param name="start"> Start <see cref="Vertex"/>. </param>
        /// <param name="end"> End <see cref="Vertex"/>. </param>
        /// <returns> The <see cref="Halfedge"/> between the given vertices. </returns>
        /// <exception cref="ArgumentException"> There is no halfedge between the given vertices. </exception>
        public Halfedge GetHalfedge(Vertex start, Vertex end)
        {
            IReadOnlyList<Halfedge> outgoings = start.OutgoingHalfedges();
            for (int i_OHe = 0; i_OHe < outgoings.Count; i_OHe++)
            {
                Halfedge outgoing = outgoings[i_OHe];

                if (outgoing.End.Equals(end)) { return outgoing; }
            }

            throw new ArgumentException("There is no halfedge between the given vertices.");
        }


        /// <summary>
        /// Retrieves a halfedge in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the halfedge. </param>
        /// <returns> The <see cref="Halfedge"/> at the given index, if it exists, <see langword="null"/> otherwise. </returns>
        public Halfedge? TryGetHalfedge(int index)
        {
            _halfedges.TryGetValue(index, out Halfedge? halfedge);

            return halfedge;
        }

        /// <summary>
        /// Retrieves a halfedge in this mesh from the index of its end vertices.
        /// </summary>
        /// <param name="startIndex"> Index of the start vertex. </param>
        /// <param name="endIndex"> Index of the end vertex. </param>
        /// <returns> The <see cref="Halfedge"/> between the given vertices, if it exists, <see langword="null"/> otherwise. </returns>
        public Halfedge? TryGetHalfedge(int startIndex, int endIndex)
        {
            Vertex start = GetVertex(startIndex);
            Vertex end = GetVertex(endIndex);
            return TryGetHalfedge(start, end);
        }

        /// <summary>
        /// Retrieves a halfedge in this mesh from its end vertices.
        /// </summary>
        /// <param name="start"> Start <see cref="Vertex"/>. </param>
        /// <param name="end"> End <see cref="Vertex"/>. </param>
        /// <returns> The <see cref="Halfedge"/> between the given vertices, if it exists, <see langword="null"/> otherwise. </returns>
        public Halfedge? TryGetHalfedge(Vertex start, Vertex end)
        {
            IReadOnlyList<Halfedge> outgoings = start.OutgoingHalfedges();
            for (int i_OHe = 0; i_OHe < outgoings.Count; i_OHe++)
            {
                Halfedge outgoing = outgoings[i_OHe];

                if (outgoing.End.Equals(end)) { return outgoing; }
            }

            return null;
        }


        /// <summary>
        /// Exposes an enumerator to iterate through the halfedges of this mesh.
        /// </summary>
        /// <returns> An enumerable set of <see cref="Halfedge"/>. </returns>
        public IEnumerable<Halfedge> GetHalfedges()
        {
            return _halfedges.Values;
        }

        /// <summary>
        /// Exposes an enumerator to iterate through the halfedges of this mesh.
        /// </summary>
        /// <param name="strategy"> Halfedge enumeration strategy. </param>
        /// <returns> An enumerable set of <see cref="Halfedge"/>. </returns>
        public IEnumerable<Halfedge> GetHalfedges(HalfedgeEnumeration strategy)
        {
            Dictionary<int, Halfedge>.ValueCollection.Enumerator halfedgeEnumerator = _halfedges.Values.GetEnumerator();

            if (strategy == HalfedgeEnumeration.All)
            {
                try
                {
                    while (halfedgeEnumerator.MoveNext())
                    {
                        yield return halfedgeEnumerator.Current;
                    }
                }
                finally { halfedgeEnumerator.Dispose(); }
            }
            else if (strategy == HalfedgeEnumeration.Odd)
            {
                try
                {
                    while (halfedgeEnumerator.MoveNext())
                    {
                        halfedgeEnumerator.MoveNext();
                        yield return halfedgeEnumerator.Current;
                    }
                }
                finally { halfedgeEnumerator.Dispose(); }
            }
            else if (strategy == HalfedgeEnumeration.Pair)
            {
                try
                {
                    while (halfedgeEnumerator.MoveNext())
                    {
                        yield return halfedgeEnumerator.Current;
                        halfedgeEnumerator.MoveNext();
                    }
                }
                finally { halfedgeEnumerator.Dispose(); }
            }
            else
            {
                throw new ArgumentException($"The enumeration strategy must be in {nameof(HalfedgeEnumeration)} enum.");
            }
        }

        
        /// <summary>
        /// Removes a halfedge at a given index in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="index"> Index of the halfedge. </param>
        public void RemoveHalfedge(int index)
        {
            Halfedge halfedge = GetHalfedge(index);
            RemoveHalfedge(halfedge);
        }

        /// <summary>
        /// Removes a halfedge in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="halfedge"> <see cref="Halfedge"/> to remove. </param>
        public void RemoveHalfedge(Halfedge halfedge)
        {
            Halfedge pairHalfedge = halfedge.Pair;

            // If the halfedge and its pair halfedge are naked.
            if (halfedge.IsBoundary() && pairHalfedge.IsBoundary())
            {
                Vertex startVertex = halfedge.Start;
                Vertex endVertex = halfedge.End;

                EraseHalfedge(halfedge);

                if (!startVertex.IsConnected()) { EraseVertex(startVertex); }

                if (!endVertex.IsConnected()) { EraseVertex(endVertex); }
            }
            // If the pair halfedge is naked (but not the halfedge).
            else if (!halfedge.IsBoundary() && pairHalfedge.IsBoundary()) { RemoveFace(halfedge.AdjacentFace!); }

            // If the halfedge is naked (but not the pair halfedge).
            else if (!pairHalfedge.IsBoundary() && halfedge.IsBoundary()) { RemoveFace(pairHalfedge.AdjacentFace!); }

            // If neither the halfedge and the pair halfedge are naked.
            else
            {
                RemoveFace(halfedge.AdjacentFace!);
                RemoveFace(pairHalfedge.AdjacentFace!);
            }
        }


        //     -----     About Edges     -----     //

        /// <summary>
        /// Retrieves an edge in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the edge. </param>
        /// <returns> The <see cref="Edge"/> at the given index. </returns>
        public override Edge GetEdge(int index)
        {
            Halfedge halfedge = GetHalfedge(2 * index);
            return halfedge.GetEdge();
        }

        /// <summary>
        /// Retrieves an edge in this mesh from the index of its end vertices.
        /// </summary>
        /// <param name="indexA"> Index of the first end vertex. </param>
        /// <param name="indexB"> Index of the second end vertex. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices. </returns>
        public override Edge GetEdge(int indexA, int indexB)
        {
            Halfedge halfedge = GetHalfedge(indexA, indexB);
            return halfedge.GetEdge();
        }

        /// <summary>
        /// Retrieves an edge in this mesh from its end vertices.
        /// </summary>
        /// <param name="vertexA"> First end <see cref="Vertex"/>. </param>
        /// <param name="vertexB"> Second end <see cref="Vertex"/>. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices. </returns>
        /// <exception cref="ArgumentException"> There is no edge between the given vertices. </exception>
        public Edge GetEdge(Vertex vertexA, Vertex vertexB)
        {
            Halfedge halfedge = GetHalfedge(vertexA, vertexB);
            return halfedge.GetEdge();
        }


        /// <summary>
        /// Retrieves an edge in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the edge. </param>
        /// <returns> The <see cref="Edge"/> at the given index, if it exists, <see langword="null"/> otherwise. </returns>
        public override Edge? TryGetEdge(int index)
        {
            Halfedge? halfedge = TryGetHalfedge(2 * index);
            return halfedge?.GetEdge();
        }

        /// <summary>
        /// Retrieves an edge in this mesh from the index of its end vertices.
        /// </summary>
        /// <param name="indexA"> Index of the first end vertex. </param>
        /// <param name="indexB"> Index of the second end vertex. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices, if it exists, <see langword="null"/> otherwise. </returns>
        public override Edge? TryGetEdge(int indexA, int indexB)
        {
            Halfedge? halfedge = TryGetHalfedge(indexA, indexB);
            return halfedge?.GetEdge();
        }

        /// <summary>
        /// Retrieves an edge in this mesh from its end vertices.
        /// </summary>
        /// <param name="vertexA"> First end <see cref="Vertex"/>. </param>
        /// <param name="vertexB"> Second end <see cref="Vertex"/>. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices, if it exists, <see langword="null"/> otherwise. </returns>
        public Edge? TryGetEdge(Vertex vertexA, Vertex vertexB)
        {
            Halfedge? halfedge = TryGetHalfedge(vertexA, vertexB);
            return halfedge?.GetEdge();
        }


        /// <summary>
        /// Exposes an enumerator to iterate through the edges of this mesh.
        /// </summary>
        /// <returns> An enumerable set of <see cref="Edge"/>. </returns>
        public override IEnumerable<Edge> GetEdges()
        {
            foreach(Halfedge halfedge in GetHalfedges(HalfedgeEnumeration.Pair))
            {
                yield return halfedge.GetEdge();
            }
        }


        /// <summary>
        /// Removes an edge at a given index in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="index"> Index of the edge. </param>
        public override void RemoveEdge(int index)
        {
            RemoveHalfedge(2 * index);
        }

        /// <summary>
        /// Removes an edge in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="edge"> <see cref="Edge"/> to remove. </param>
        public void RemoveEdge(Edge edge)
        {
            RemoveHalfedge(edge.CorrespondingHalfedge);
        }


        //     -----     About Faces     -----     //

        /// <summary>
        /// Adds a triangular face to this mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first face vertex. </param>
        /// <param name="indexB"> Index of the second face vertex. </param>
        /// <param name="indexC"> Index of the third face vertex. </param>
        /// <param name="traits"> Traits of the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        public override Face AddFace(int indexA, int indexB, int indexC, TFaceTraits traits)
        {
            Vertex vertexA = GetVertex(indexA); Vertex vertexB = GetVertex(indexB); Vertex vertexC = GetVertex(indexC);
            Vertex[] vertices = new Vertex[3] { vertexA, vertexB, vertexC };

            return AddFace(vertices, traits);
        }

        /// <summary>
        /// Adds a triangular face to this mesh.
        /// </summary>
        /// <param name="vertexA"> First <see cref="Vertex"/> of the face. </param>
        /// <param name="vertexB"> Second <see cref="Vertex"/> of the face. </param>
        /// <param name="vertexC"> Third <see cref="Vertex"/> of the face. </param>
        /// <param name="traits"> Traits of the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        public Face AddFace(Vertex vertexA, Vertex vertexB, Vertex vertexC, TFaceTraits traits)
        {
            Vertex[] vertices = new Vertex[3] { vertexA, vertexB, vertexC };

            return AddFace(vertices, traits);
        }


        /// <summary>
        /// Adds a quadrilateral face to this mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first face vertex. </param>
        /// <param name="indexB"> Index of the second face vertex. </param>
        /// <param name="indexC"> Index of the third face vertex. </param>
        /// <param name="indexD"> Index of the fourth face vertex.. </param>
        /// <param name="traits"> Traits of the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        public override Face AddFace(int indexA, int indexB, int indexC, int indexD, TFaceTraits traits)
        {
            Vertex vertexA = GetVertex(indexA); Vertex vertexB = GetVertex(indexB); Vertex vertexC = GetVertex(indexC); Vertex vertexD = GetVertex(indexD);
            Vertex[] vertices = new Vertex[4] { vertexA, vertexB, vertexC, vertexD };

            return AddFace(vertices, traits);
        }

        /// <summary>
        /// Adds a quadrilateral face to this mesh.
        /// </summary>
        /// <param name="vertexA"> First <see cref="Vertex"/> of the face. </param>
        /// <param name="vertexB"> Second <see cref="Vertex"/> of the face. </param>
        /// <param name="vertexC"> Third <see cref="Vertex"/> of the face. </param>
        /// <param name="vertexD"> Fourth <see cref="Vertex"/> of the face. </param>
        /// <param name="traits"> Traits of the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        public Face AddFace(Vertex vertexA, Vertex vertexB, Vertex vertexC, Vertex vertexD, TFaceTraits traits)
        {
            Vertex[] vertices = new Vertex[4] { vertexA, vertexB, vertexC, vertexD };

            return AddFace(vertices, traits);
        }


        /// <summary>
        /// Adds a face to this mesh.
        /// </summary>
        /// <param name="vertexIndices"> Ordered set of vertex indices of the face. </param>
        /// <param name="traits"> Traits of the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        public override Face AddFace(IReadOnlyList<int> vertexIndices, TFaceTraits traits)
        {
            Vertex[] vertices = new Vertex[vertexIndices.Count];
            for (int i = 0; i < vertexIndices.Count; i++) { vertices[i] = GetVertex(vertexIndices[i]); }

            return AddFace(vertices, traits);
        }

        /// <summary>
        /// Adds a face to this mesh.
        /// </summary>
        /// <param name="vertices"> Ordered set of vertices of the face. </param>
        /// <param name="traits"> Traits of the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> A face must have at least three vertices. </exception>
        /// <exception cref="ArgumentException"> The list of vertices must not contain duplicate elements. </exception>
        /// <exception cref="ArgumentException"> One of the input vertex does not belong to this mesh. </exception>
        public Face AddFace(IReadOnlyList<Vertex> vertices, TFaceTraits traits)
        {
            #region Initialisations

            // Create the instance of the face being created
            Face newFace = new Face(this, _newFaceIndex, traits);

            int faceHeCount = vertices.Count;

            #endregion

            #region Verifications

            // On Vertices
            foreach (Vertex vertex in vertices)
            {
                // Check that all vertex indices exist in this mesh
                if (!(this.GetVertex(vertex.Index).Equals(vertex)))
                {
                    throw new NullReferenceException("One of the specified vertex was not found in the mesh.");
                }
                // Check that all vertices are on a boundary
                if (!vertex.IsBoundary())
                {
                    throw new ArgumentException("One of the specified vertex was not on the boundary of the mesh.");
                }
            }

            #endregion

            #region Existence of Halfedges

            /* For each consecutive vertices, look for an existing halfedge :
             * If it exists, verify that it doesn't already have a face.
             * If it doesn't exist, mark for creation of a new HeEdge pair.
             */

            Halfedge[] faceHalfedges = new Halfedge[faceHeCount];
            bool[] isHeNew = new bool[faceHeCount];

            for (int i_He = 0; i_He < faceHeCount; i_He++)
            {
                Vertex startVertex = vertices[i_He];
                Vertex endVertex = vertices[(i_He + 1) % faceHeCount];

                // Look for the HeEdge
                Halfedge? faceHalfedge = TryGetHalfedge(startVertex, endVertex);

                if (faceHalfedge == null) { isHeNew[i_He] = true; }           // If the halfedge doesn't exist.
                else if (!faceHalfedge.IsBoundary())                          // If the halfedge exists, but already has an adjacent face (non-manifold).
                {
                    throw new InvalidOperationException("One of the requested halfedge already belongs to a face.");
                }
                else { faceHalfedges[i_He] = faceHalfedge; }                  // If the halfedge exists, and is on the boundary.


                // To prevent non-manifold vertices, uncomment the line below :
                // if (isHeNew[i_Halfedge] && isHeNew[(i_Halfedge + faceHeCount - 1) % faceHeCount] && startVertex.IsConnected()) { return null; }
            }

            #endregion

            #region Creation of Missing Edges

            /* Create the missing halfedge pair and manage connection of the halfedge to the new face
             * (This could be done in the previous loop but it avoids having to tidy up
             * any recently added halfedges should a non-manifold edge be found.)
             */

            for (int i_He = 0; i_He < faceHeCount; i_He++)
            {
                if (isHeNew[i_He])                                        // Create a new halfedge pair
                {
                    Vertex startVertex = vertices[i_He];
                    Vertex endVertex = vertices[(i_He + 1) % faceHeCount];

                    Halfedge[] pairOfHalfedges = AddHalfedgePair(startVertex, endVertex);
                    faceHalfedges[i_He] = pairOfHalfedges[0];
                }

                faceHalfedges[i_He]._adjacent = newFace;        // Connect the current halfedge with the face
            }

            #endregion

            #region Connecting Edges

            /***** Manages the connection of halfedges around the vertices of the new face. *****/

            for (int i_Vertex = 0; i_Vertex < vertices.Count; i_Vertex++)
            {
                Vertex vertex = vertices[i_Vertex];

                int i_IncFaceHe = (i_Vertex + faceHeCount - 1) % faceHeCount;   // Index of the incoming face halfedge at the vertex.
                int i_OutFaceHe = i_Vertex;                                       // Index of the outgoing face halfedge at the vertex.

                /******************** Evaluate Situation ********************/

                int situation = 0;
                if (isHeNew[i_IncFaceHe]) { situation += 1; }       // If the incoming face halfedge is new .
                if (isHeNew[i_OutFaceHe]) { situation += 2; }       // If the outgoing face halfedge is new.

                /* Check for non-manifold vertex case (i.e. both current halfedges are new but the vertex between them is already part of another face.
                 * This vertex will have at least two outgoing boundary halfedge (Not strictly allowed, but it could happen if faces are added in an "bad" order).
                 * 
                 * TODO: If a mesh has non-manifold vertices perhaps it should be considered invalid. 
                 * Any operations performed on such a mesh cannot be relied upon to perform correctly as the adjacency information may not be correct.
                 */
                if (situation == 3 && vertex.IsConnected()) { situation++; }

                /******************** Manage Connection ********************/

                if (situation > 0) // At least one of the above considered face halfedge pair is new
                {
                    // Bondary edges at the vertex 
                    Halfedge? outBoundary = null;      // Incoming boundary halfedge at the vertex
                    Halfedge? incBoundary = null;      // Outgoing boundary halfedge at the vertex

                    switch (situation)
                    {
                        // iterate through halfedges clockwise around vertex v2 until boundary

                        case 1: // If the incoming face halfedge is new, but the outgoing face halfedge is old.
                            incBoundary = faceHalfedges[i_OutFaceHe].Previous;
                            outBoundary = faceHalfedges[i_IncFaceHe].Pair;
                            break;
                        case 2: // If the incoming face halfedge is old, but the outgoing face halfedge is new.
                            incBoundary = faceHalfedges[i_OutFaceHe].Pair;
                            outBoundary = faceHalfedges[i_IncFaceHe].Next;
                            break;
                        case 3: // If both the incoming and the outgoing face halfedge are new.
                            incBoundary = faceHalfedges[i_OutFaceHe].Pair;
                            outBoundary = faceHalfedges[i_IncFaceHe].Pair;
                            break;
                        case 4: // If both the incoming and the outgoing face halfedge are new (and the vertex is non-manifold).
                            // Two boundary have to be managed here:
                            // For the first boundary
                            incBoundary = vertex.Outgoing!.Previous;
                            outBoundary = faceHalfedges[i_IncFaceHe].Pair;
                            incBoundary._next = outBoundary;
                            outBoundary._previous = incBoundary;
                            // For the second boundary
                            incBoundary = faceHalfedges[i_OutFaceHe].Pair;
                            outBoundary = vertex.Outgoing;
                            break;
                        default:
                            throw new InvalidOperationException($"The value of {nameof(situation)} can't be strictly negative or over 4.");
                    }

                    // Connect vertex's boundary halfedge
                    incBoundary._next = outBoundary;
                    outBoundary._previous = incBoundary;

                    // Connect face halfedges
                    faceHalfedges[i_IncFaceHe]._next = faceHalfedges[i_OutFaceHe];
                    faceHalfedges[i_OutFaceHe]._previous = faceHalfedges[i_IncFaceHe];

                    // Ensures that if a vertex lies on a boundary, then its outgoingHalfedge is a boundary halfedge.
                    vertex._outgoing = outBoundary;
                }

                else // None of the above considered face halfedge pair are new (includes trickery for non-manifold vertex)
                {
                    IReadOnlyList<Halfedge> outgoingHalfedges = vertex.OutgoingHalfedges();

                    // Gets the number of boundary outgoing halfedges at this vertex.
                    int outgoingBoundaryHeCount = outgoingHalfedges.Count;

                    if (outgoingBoundaryHeCount == 0) // In the case where the vertex is manifold.
                    {
                        // Do nothing !
                    }
                    else if (outgoingBoundaryHeCount == 1) // In the case where the vertex is non-manifold but only with only due to one face ("simple case")
                    {
                        // Verifications
                        if (!faceHalfedges[i_IncFaceHe].Next.Equals(faceHalfedges[i_OutFaceHe]))
                        {
                            throw new InvalidOperationException("Two of the requested halfedges for the creation of a face are not consecutive.");
                        }
                        // Assign the only boundary outgoing edge as the outgoing edge of vertex.
                        foreach (Halfedge outgoingHe in outgoingHalfedges)
                        {
                            if (outgoingHe.IsBoundary()) { vertex._outgoing = outgoingHe; break; }
                        }
                    }
                    else // In the case where the vertex is non-manifold ("difficult case")
                    {
                        if (faceHalfedges[i_IncFaceHe].Next.Equals(faceHalfedges[i_OutFaceHe])) // If "luckily" the arrangement of the faces around the vertex is fine.
                        {
                            foreach (Halfedge outgoingHe in outgoingHalfedges)
                            {
                                if (outgoingHe.IsBoundary()) { vertex._outgoing = outgoingHe; break; }
                            }
                        }
                        else // If the arrangement of the faces around the vertex is not compatible with the face to add.
                        {
                            Halfedge next_IncFaceHe = faceHalfedges[i_IncFaceHe].Next;
                            Halfedge prev_OutFaceHe = faceHalfedges[i_OutFaceHe].Previous;

                            // Assign new outgoing edge to the vertex
                            foreach (Halfedge outgoingHe in outgoingHalfedges)
                            {
                                if (outgoingHe.Equals(next_IncFaceHe)) { continue; } // Beware : The outgoing edge can not be any boundary edge.
                                if (outgoingHe.IsBoundary()) { vertex._outgoing = outgoingHe; break; }
                            }

                            // Connect face edges to each other
                            faceHalfedges[i_IncFaceHe]._next = faceHalfedges[i_OutFaceHe];
                            faceHalfedges[i_OutFaceHe]._previous = faceHalfedges[i_IncFaceHe];

                            // Connect the surroundings edges
                            prev_OutFaceHe._next = vertex.Outgoing;
                            next_IncFaceHe._previous = vertex.Outgoing?.Previous;

                            vertex.Outgoing!.Previous._next = next_IncFaceHe;
                            vertex.Outgoing._previous = prev_OutFaceHe;
                        }
                    }
                }
            }

            #endregion

            // Finally set the first halfedge and returns the face.
            newFace._firstHalfedge = faceHalfedges[0];

            this._faces.Add(_newFaceIndex, newFace);

            _newFaceIndex += 1;

            return newFace;
        }


        /// <summary>
        /// Retrieves a face in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the face. </param>
        /// <returns> The <see cref="Face"/> at the given index. </returns>
        public override Face GetFace(int index)
        {
            return _faces[index];
        }

        /// <summary>
        /// Retrieves a face in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the face. </param>
        /// <returns> The <see cref="Face"/> at the given index, if it exists, <see langword="null"/> otherwise. </returns>
        public override Face? TryGetFace(int index)
        {
            _faces.TryGetValue(index, out Face? face);

            return face;
        }

        /// <summary>
        /// Exposes an enumerator to iterate through the faces of this mesh.
        /// </summary>
        /// <returns> An enumerable set of <see cref="Face"/>. </returns>
        public override IEnumerable<Face> GetFaces()
        {
            return _faces.Values;
        }


        /// <summary>
        /// Removes a face at a given index in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="index"> Index of the face. </param>
        public override void RemoveFace(int index)
        {
            Face face = GetFace(index);
            RemoveFace(face);
        }

        /// <summary>
        /// Removes a face in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="face"> <see cref="Face"/> to remove. </param>
        public void RemoveFace(Face face)
        {
            // Save the face vertices for future treatment
            IReadOnlyList<Vertex> faceVertices = new List<Vertex>(face.FaceVertices());

            // Manage connection with edges
            foreach (Halfedge halfedge in face.FaceHalfedges())
            {
                if (halfedge.Pair.IsBoundary())
                {
                    halfedge._adjacent = null;
                    halfedge.Pair._adjacent = null;
                    EraseHalfedge(halfedge);
                }
                else
                {
                    halfedge._adjacent = null;
                    if (!halfedge.Start.Outgoing!.IsBoundary())
                    {
                        halfedge.Start._outgoing = halfedge;
                    }
                }
            }

            // Manage isolated vertices
            foreach (Vertex vertex in faceVertices)
            {
                if (!vertex.IsConnected()) { EraseVertex(vertex); }
            }

            // Remove the face from the mesh
            _faces.Remove(face.Index);

            // Unset the face
            face.Unset();
        }

        #endregion

        #region Other Methods

        //     -----     About Vertices     -----     //

        /// <summary>
        /// Erases a vertex at a given index in the mesh.
        /// </summary>
        /// <remarks> Every reference to this vertex in the mesh should be deleted before it is erased. </remarks>
        /// <param name="index"> Index of the vertex. </param>
        protected override void EraseVertex(int index)
        {
            Vertex vertex = GetVertex(index);
            EraseVertex(vertex);
        }

        /// <summary>
        /// Erases a vertex in the mesh.
        /// </summary>
        /// <remarks> Every reference to this vertex in the mesh should be deleted before it is erased. </remarks>
        /// <param name="vertex"> <see cref="Vertex"/> to erase. </param>
        /// <exception cref="InvalidOperationException"> The vertex cannot be erased. It is still connected. </exception>
        protected void EraseVertex(Vertex vertex)
        {
            // If the vertex is still connected.
            if (!(vertex.Outgoing is null))
            {
                throw new InvalidOperationException("The vertex cannot be erased. It is still connected.");
            }

            // Remove the vertex from the mesh.
            _vertices.Remove(vertex.Index);

            // Unset the current vertex.
            vertex.Unset();
        }


        //     -----     About Halfedges     -----     //

        /// <summary>
        /// Adds a pair of halfedges to this mesh with <see langword="default"/> traits, without creating the corresponding edge.
        /// </summary>
        /// <param name="indexA"> Index of the first vertex, which will be the start of the first halfedge and the end of the second. </param>
        /// <param name="indexB"> Index of the second vertex, which will be the end of the first halfedge and the end of the second. </param>
        /// <returns> The new pair of <see cref="Halfedge"/>. </returns>
        internal Halfedge[] AddHalfedgePair(int indexA, int indexB)
        {
            Vertex vertexA = GetVertex(indexA);
            Vertex vertexB = GetVertex(indexB);
            return AddHalfedgePair(vertexA, vertexB);
        }

        /// <summary>
        /// Adds a pair of halfedges to this mesh with <see langword="default"/> traits, without creating the corresponding edge.
        /// </summary>
        /// <param name="vertexA"> First <see cref="Vertex"/>, which will be the start of the first halfedge and the end of the second. </param>
        /// <param name="vertexB"> Second <see cref="Vertex"/>, which will be the end of the first halfedge and the end of the second. </param>
        /// <returns> The new pair of <see cref="Halfedge"/>. </returns>
        /// <exception cref="ArgumentException"> End vertices must be different. </exception>
        /// <exception cref="ArgumentException"> A pair of halfedge with the same end vertices already exist. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> Two halfedge traits must be provided. </exception>
        internal Halfedge[] AddHalfedgePair(Vertex vertexA, Vertex vertexB)
        {
            THalfedgeTraits?[] halfedgesTraits = new THalfedgeTraits?[] { default, default };

            return AddHalfedgePair(vertexA, vertexB, halfedgesTraits);
        }

        /// <summary>
        /// Adds a pair of halfedges to this mesh, without creating the corresponding edge.
        /// </summary>
        /// <param name="indexA"> Index of the first vertex, which will be the start of the first halfedge and the end of the second. </param>
        /// <param name="indexB"> Index of the second vertex, which will be the end of the first halfedge and the end of the second. </param>
        /// <param name="halfedgesTraits"> Traits for the pair of halfedge :
        /// <list type="table">
        ///     <item>
        ///         <term> 0 </term>
        ///         <description> Traits for the first halfedge. </description>
        ///     </item>
        ///     <item>
        ///         <term> 1 </term>
        ///         <description> Traits for the second halfedge. </description>
        ///     </item>
        /// </list>
        /// </param>
        /// <returns> The new pair of <see cref="Halfedge"/>. </returns>
        internal Halfedge[] AddHalfedgePair(int indexA, int indexB, THalfedgeTraits?[] halfedgesTraits)
        {
            Vertex vertexA = GetVertex(indexA);
            Vertex vertexB = GetVertex(indexB);
            return AddHalfedgePair(vertexA, vertexB, halfedgesTraits);
        }

        /// <summary>
        /// Adds a pair of halfedges to this mesh, without creating the corresponding edge.
        /// </summary>
        /// <param name="vertexA"> First <see cref="Vertex"/>, which will be the start of the first halfedge and the end of the second. </param>
        /// <param name="vertexB"> Second <see cref="Vertex"/>, which will be the end of the first halfedge and the end of the second. </param>
        /// <param name="halfedgesTraits"> Traits for the pair of halfedge :
        /// <list type="number">
        ///     <item>
        ///         <term> 0 </term>
        ///         <description> Traits for the first halfedge. </description>
        ///     </item>
        ///     <item>
        ///         <term> 1 </term>
        ///         <description> Traits for the second halfedge. </description>
        ///     </item>
        /// </list>
        /// </param>
        /// <returns> The new pair of <see cref="Halfedge"/>. </returns>
        /// <exception cref="ArgumentException"> End vertices must be different. </exception>
        /// <exception cref="ArgumentException"> A pair of halfedge with the same end vertices already exist. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> Two halfedge traits must be provided. </exception>
        internal Halfedge[] AddHalfedgePair(Vertex vertexA, Vertex vertexB, THalfedgeTraits?[] halfedgesTraits)
        {
            // Verification : Avoid looping halfedges
            if (vertexA.Equals(vertexB)) { throw new ArgumentException("End vertices must be different."); }

            // Verification : Avoid multiple halfedges
            Halfedge? exitingHalfedge = TryGetHalfedge(vertexA, vertexB);
            if (exitingHalfedge is not null) { throw new ArgumentException("A pair of halfedge with the same end vertices already exist."); }

            // Verification : Number of traits
            if (halfedgesTraits.Length != 2) { throw new ArgumentOutOfRangeException("Two halfedge traits must be provided."); }


            Halfedge firstHalfedge = new Halfedge(this, _newHalfedgeIndex, vertexA, vertexB, halfedgesTraits[0]);
            Halfedge pairHalfedge = new Halfedge(this, _newHalfedgeIndex + 1, vertexB, vertexA, halfedgesTraits[1]);

            firstHalfedge._pair = pairHalfedge;
            pairHalfedge._pair = firstHalfedge;

            _halfedges.Add(_newHalfedgeIndex, firstHalfedge);
            _halfedges.Add(_newHalfedgeIndex + 1, pairHalfedge);

            _newHalfedgeIndex += 2;

            return new Halfedge[2] { firstHalfedge, pairHalfedge };
        }


        /// <summary>
        /// Adds a pair of halfedges to this mesh with <see langword="default"/> traits, and creates the corresponding edge.
        /// </summary>
        /// <param name="indexA"> Index of the first vertex, which will be the start of the first halfedge and the end of the second. </param>
        /// <param name="indexB"> Index of the second vertex, which will be the end of the first halfedge and the end of the second. </param>
        /// <param name="edgeTraits"> Traits for the corresponding edge. </param>
        /// <returns> The new pair of <see cref="Halfedge"/>. </returns>
        internal Halfedge[] AddHalfedgePair(int indexA, int indexB, TEdgeTraits? edgeTraits)
        {
            Vertex vertexA = GetVertex(indexA);
            Vertex vertexB = GetVertex(indexB);

            return AddHalfedgePair(vertexA, vertexB, edgeTraits);
        }

        /// <summary>
        /// Adds a pair of halfedges to this mesh with <see langword="default"/> traits, and creates the corresponding edge.
        /// </summary>
        /// <param name="vertexA"> First <see cref="Vertex"/>, which will be the start of the first halfedge and the end of the second. </param>
        /// <param name="vertexB"> Second <see cref="Vertex"/>, which will be the end of the first halfedge and the end of the second. </param>
        /// <param name="edgeTraits"> Traits for the corresponding edge. </param>
        /// <returns> The new pair of <see cref="Halfedge"/>. </returns>
        internal Halfedge[] AddHalfedgePair(Vertex vertexA, Vertex vertexB, TEdgeTraits? edgeTraits)
        {
            THalfedgeTraits?[] halfedgesTraits = new THalfedgeTraits?[] { default, default };

            return AddHalfedgePair(vertexA, vertexB, halfedgesTraits, edgeTraits);
        }

        /// <summary>
        /// Adds a pair of halfedges to this mesh, and creates the corresponding edge.
        /// </summary>
        /// <param name="indexA"> Index of the first vertex, which will be the start of the first halfedge and the end of the second. </param>
        /// <param name="indexB"> Index of the second vertex, which will be the end of the first halfedge and the end of the second. </param>
        /// <param name="halfedgesTraits"> Traits for the pair of halfedge :
        /// <list type="number">
        ///     <item>
        ///         <term> 0 </term>
        ///         <description> Traits for the first halfedge. </description>
        ///     </item>
        ///     <item>
        ///         <term> 1 </term>
        ///         <description> Traits for the second halfedge. </description>
        ///     </item>
        /// </list>
        /// </param>
        /// <param name="edgeTraits"> Traits for the corresponding edge. </param>
        /// <returns> The new pair of <see cref="Halfedge"/>. </returns>
        internal Halfedge[] AddHalfedgePair(int indexA, int indexB, THalfedgeTraits?[] halfedgesTraits, TEdgeTraits? edgeTraits)
        {
            Vertex vertexA = GetVertex(indexA);
            Vertex vertexB = GetVertex(indexB);
            return AddHalfedgePair(vertexA, vertexB, halfedgesTraits, edgeTraits);
        }

        /// <summary>
        /// Adds a pair of halfedges to this mesh, and creates the corresponding edge.
        /// </summary>
        /// <param name="vertexA"> First <see cref="Vertex"/>, which will be the start of the first halfedge and the end of the second. </param>
        /// <param name="vertexB"> Second <see cref="Vertex"/>, which will be the end of the first halfedge and the end of the second. </param>
        /// <param name="halfedgesTraits"> Traits for the pair of halfedge :
        /// <list type="number">
        ///     <item>
        ///         <term> 0 </term>
        ///         <description> Traits for the first halfedge. </description>
        ///     </item>
        ///     <item>
        ///         <term> 1 </term>
        ///         <description> Traits for the second halfedge. </description>
        ///     </item>
        /// </list>
        /// </param>
        /// <param name="edgeTraits"> Traits for the corresponding edge. </param>
        /// <returns> The new pair of <see cref="Halfedge"/>. </returns>
        /// <exception cref="ArgumentException"> End vertices must be different. </exception>
        /// <exception cref="ArgumentException"> A pair of halfedge with the same end vertices already exist. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> Two halfedge traits must be provided. </exception>
        internal Halfedge[] AddHalfedgePair(Vertex vertexA, Vertex vertexB, THalfedgeTraits?[] halfedgesTraits, TEdgeTraits? edgeTraits)
        {
            // Verification : Avoid looping halfedges
            if (vertexA.Equals(vertexB)) { throw new ArgumentException("End vertices must be different."); }

            // Verification : Avoid multiple halfedges
            Halfedge? exitingHalfedge = TryGetHalfedge(vertexA, vertexB);
            if (exitingHalfedge is not null) { throw new ArgumentException("A pair of halfedge with the same end vertices already exist."); }

            // Verification : Number of traits
            if (halfedgesTraits.Length != 2) { throw new ArgumentOutOfRangeException("Two halfedge traits must be provided."); }


            Halfedge firstHalfedge = new Halfedge(this, _newHalfedgeIndex, vertexA, vertexB, halfedgesTraits[0]);
            Halfedge pairHalfedge = new Halfedge(this, _newHalfedgeIndex + 1, vertexB, vertexA, halfedgesTraits[1]);

            firstHalfedge._pair = pairHalfedge;
            pairHalfedge._pair = firstHalfedge;

            _halfedges.Add(_newHalfedgeIndex, firstHalfedge);
            _halfedges.Add(_newHalfedgeIndex + 1, pairHalfedge);


            Edge correspondingEdge = new Edge(this, firstHalfedge, edgeTraits);

            firstHalfedge._correspondingEdge = correspondingEdge;
            pairHalfedge._correspondingEdge = correspondingEdge;


            _newHalfedgeIndex += 2;

            return new Halfedge[2] { firstHalfedge, pairHalfedge };
        }


        /// <summary>
        /// Erases a halfedge at a given index in the mesh.
        /// </summary>
        /// <remarks> Every reference to this halfedge in the mesh should be deleted before it is erased. </remarks>
        /// <param name="index"> Index of the halfedge. </param>
        protected void EraseHalfedge(int index)
        {
            Halfedge halfedge = GetHalfedge(index);
            EraseHalfedge(halfedge);
        }

        /// <summary>
        /// Erases a halfedge in the mesh.
        /// </summary>
        /// <remarks> Every reference to this halfedge in the mesh should be deleted before it is erased. </remarks>
        /// <param name="halfedge"> <see cref="Halfedge"/> to erase. </param>
        protected void EraseHalfedge(Halfedge halfedge)
        {
            Halfedge pairHalfedge = halfedge.Pair;

            /***** Manage connection of the neighbour edges *****/

            halfedge.Previous._next = pairHalfedge.Next;
            halfedge.Next._previous = pairHalfedge.Previous;
            pairHalfedge.Previous._next = halfedge.Next;
            pairHalfedge.Next._previous = halfedge.Previous;

            /***** Manage connection with start and end vertices *****/

            // If the halfedge is stored in its start vertex.
            if (halfedge.Start.Outgoing!.Equals(halfedge))
            {
                // If the start vertex is only connected to the erased halfedge pair.
                if (halfedge.Previous.Equals(pairHalfedge)) { halfedge.Start._outgoing = null; }
                else { halfedge.Start._outgoing = pairHalfedge.Next; }
            }

            // If the pair halfedge is stored in its start vertex.
            if (pairHalfedge.Start.Outgoing!.Equals(pairHalfedge))
            {
                // If the start vertex is only connected to the erased halfedge pair.
                if (pairHalfedge.Previous.Equals(halfedge)) { pairHalfedge.Start._outgoing= null; }
                else { pairHalfedge.Start._outgoing = halfedge.Next; }
            }

            /***** Manage connection with adjacent face *****/

            // If neither the halfedge and its pair halfedge are on a boundary.
            if ((halfedge.AdjacentFace is not null) && (pairHalfedge.AdjacentFace is not null))
            {
                EraseFace(halfedge.AdjacentFace, halfedge.Next);

                // We cannot call erase the adjacent face of the pair halfedge.
                // Most of the work has already been done in the above "EraseFace".
                _faces.Remove(pairHalfedge.AdjacentFace.Index);
                pairHalfedge.AdjacentFace.Unset();
            }
            // If the pair halfedge is on a boundary but not the halfedge.
            else if (halfedge.AdjacentFace is not null)
            {
                EraseFace(halfedge.AdjacentFace, halfedge.Next);
            }
            // If the halfedge is on a boundary but not the pair halfedge.
            else if (pairHalfedge.AdjacentFace is not null)
            {
                EraseFace(pairHalfedge.AdjacentFace, pairHalfedge.Next);
            }

            // Remove the pair of edges from the mesh
            _halfedges.Remove(halfedge.Index);
            _halfedges.Remove(pairHalfedge.Index);

            // Unset the pair of edges

            halfedge.Unset();
            pairHalfedge.Unset();
        }


        //     -----     About Edges     -----     //

        /// <summary>
        /// Adds an edge to this mesh, and creates the underlying pair of halfedges with <see langword="default"/> traits.
        /// </summary>
        /// <remarks> This method calls <see cref="AddHalfedgePair(int, int, THalfedgeTraits[], TEdgeTraits)"/> with default halfedge traits. </remarks>
        /// <param name="startIndex"> Index of the start vertex. </param>
        /// <param name="endIndex"> Index of the end vertex. </param>
        /// <param name="traits"> Traits of the edge. </param>
        /// <returns> The new <see cref="Edge"/>. </returns>
        internal override Edge AddEdge(int startIndex, int endIndex, TEdgeTraits? traits = default)
        {
            Vertex start = GetVertex(startIndex);
            Vertex end = GetVertex(endIndex);
            return AddEdge(start, end, traits);
        }

        /// <summary>
        /// Adds an edge to this mesh, and creates the underlying pair of halfedges with <see langword="default"/> traits.
        /// </summary>
        /// <param name="start"> Start <see cref="Vertex"/>. </param>
        /// <param name="end"> End <see cref="Vertex"/>. </param>
        /// <param name="traits"> Traits of the edge. </param>
        /// <returns> The new <see cref="Edge"/>. </returns>
        internal Edge AddEdge(Vertex start, Vertex end, TEdgeTraits? traits = default)
        {
            Halfedge[] halfedgePair = AddHalfedgePair(start, end, traits);

            return halfedgePair[0].GetEdge();
        }


        /// <summary>
        /// Erases an edge at a given index in the mesh.
        /// </summary>
        /// <remarks> Every reference to this edge in the mesh should be deleted before it is erased. </remarks>
        /// <param name="index"> Index of the edge. </param>
        protected override void EraseEdge(int index)
        {
            EraseHalfedge(2 * index);
        }

        /// <summary>
        /// Erases an edge in the mesh.
        /// </summary>
        /// <remarks> Every reference to this edge in the mesh should be deleted before it is erased. </remarks>
        /// <param name="edge"> <see cref="Edge"/> to erase. </param>
        protected void EraseEdge(Edge edge)
        {
            EraseHalfedge(edge.CorrespondingHalfedge);
        }


        //     -----     About Faces     -----     //

        /// <summary>
        /// Erases a face at a given index in the mesh.
        /// </summary>
        /// <remarks> Every reference to this face in the mesh should be deleted before it is erased. </remarks>
        /// <param name="index"> Index of the face. </param>
        protected override void EraseFace(int index)
        {
            Face face = GetFace(index);
            EraseFace(face);
        }

        /// <summary>
        /// Erases a face in the mesh.
        /// </summary>
        /// <remarks> Every reference to this face in the mesh should be deleted before it is erased. </remarks>
        /// <param name="face"> <see cref="Face"/> to erase. </param>
        protected void EraseFace(Face face)
        {
            // Manage connection with edges
            foreach (Halfedge halfedge in face.FaceHalfedges())
            {
                halfedge._adjacent = null;
            }

            // Remove the face from the mesh
            _faces.Remove(face.Index);

            // Unset the face
            face.Unset();
        }


        /// <summary>
        /// Erases a face after the connection between halfedges has been adapted for a halfedge pair erasure.
        /// </summary>
        /// <remarks> Deletes the reference of adjacent face in a loop of former face halfedges and in the disconnected halfedge pair. </remarks>
        /// <param name="face"> <see cref="Face"/> to erase. </param>
        /// <param name="faceHalfedge"> <see cref="Halfedge"/> to start the computation of the loop. </param>
        private void EraseFace(Face face, Halfedge faceHalfedge)
        {
            // Manage connection with the face in a halfedge loop.
            Halfedge halfedge = faceHalfedge;
            do
            {
                halfedge._adjacent = null;
                halfedge = halfedge.Next;
            }
            while (!halfedge.Equals(faceHalfedge));

            // Remove the face from the mesh
            _faces.Remove(face.Index);

            // Unset the face
            face.Unset();
        }

        #endregion
    }


    /// <summary>
    /// Defines the type of enumeration for the mesh halfedges.
    /// </summary>
    public enum HalfedgeEnumeration : short
    {
        /// <summary>
        /// Eumerates all the halfedges.
        /// </summary>
        All = 0,
        /// <summary>
        /// Emumerates halfedges with odd indices.
        /// </summary>
        Odd = 1,
        /// <summary>
        /// Enumerates halfedge with pair indices.
        /// </summary>
        Pair = 2
    }

}