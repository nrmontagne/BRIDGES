using System;
using System.Collections;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMeshes
{
    /// <summary>
    /// Represents a generic face|vertex mesh data-structure.
    /// </summary>
    /// <typeparam name="TVertexTraits"> Type defining the traits of the mesh vertices. </typeparam>
    /// <typeparam name="TEdgeTraits"> Type defining the traits of the mesh edges. </typeparam>
    /// <typeparam name="TFaceTraits"> Type defining the traits of the mesh faces. </typeparam>
    public partial class FaceVertexMesh<TVertexTraits, TEdgeTraits, TFaceTraits> : Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        #region Fields

        /// <summary>
        /// Dictionary containing the <see cref="Vertex"/> of this mesh.
        /// <list type="table">
        ///     <listheader>
        ///         <term> Key </term>
        ///         <description> Index of the <see cref="Vertex"/>. </description>
        ///     </listheader>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Corresponding <see cref="Vertex"/>. </description>
        ///     </item>
        /// </list>
        /// </summary>
        protected Dictionary<int, Vertex> _vertices;

        /// <summary>
        /// Index for a newly created vertex.
        /// </summary>
        /// <remarks> This may not match with <see cref="VertexCount"/> if vertices were removed from the mesh. </remarks>
        protected int _newVertexIndex;


        /// <summary>
        /// Dictionary containing the <see cref="Edge"/> of this mesh.
        /// <list type="table">
        ///     <listheader>
        ///         <term> Key </term>
        ///         <description> Index of the <see cref="Edge"/>. </description>
        ///     </listheader>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Corresponding <see cref="Edge"/>. </description>
        ///     </item>
        /// </list>
        /// </summary>
        protected Dictionary<int, Edge> _edges;

        /// <summary>
        /// Index for a newly created edge.
        /// </summary>
        /// <remarks> This may not match with <see cref="EdgeCount"/> if edges were removed from the mesh. </remarks>
        protected int _newEdgeIndex;


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
        protected Dictionary<int, Face> _faces;

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
        public override int VertexCount  => _vertices.Count;

        /// <summary>
        /// Gets the number of edges of this mesh.
        /// </summary>
        public override int EdgeCount => _edges.Count;

        /// <summary>
        /// Gets the number of faces of this mesh.
        /// </summary>
        public override int FaceCount => _faces.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="FaceVertexMesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> class.
        /// </summary>
        public FaceVertexMesh()
        {
            // Instantiate fields
            _vertices = new Dictionary<int, Vertex>();
            _edges = new Dictionary<int, Edge>();
            _faces = new Dictionary<int, Face>();

            // Initialise fields
            _newVertexIndex = 0;
            _newEdgeIndex = 0;
            _newFaceIndex = 0;
        }

        #endregion

        #region Public Methods

        //     -----     About Vertices     -----     //

        /// <summary>
        /// Adds a vertex to this mesh.
        /// </summary>
        /// <param name="traits"> Traits for the vertex. </param>
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
            IReadOnlyList<Face> adjacentFaces = vertex.AdjacentFaces();

            for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
            {
                RemoveFace(adjacentFaces[i_AF]);
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
            return _edges[index];
        }

        /// <summary>
        /// Retrieves an edge in this mesh from the index of its end vertices.
        /// </summary>
        /// <param name="indexA"> Index of the first end vertex. </param>
        /// <param name="indexB"> Index of the second end vertex. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices. </returns>
        public override Edge GetEdge(int indexA, int indexB)
        {
            Vertex vertexA = GetVertex(indexA);
            Vertex vertexB = GetVertex(indexB);
            return GetEdge(vertexA, vertexB);
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
            IReadOnlyList<Edge> connectedEdges = vertexA.ConnectedEdges();
            for (int i_CE = 0; i_CE < connectedEdges.Count; i_CE++)
            {
                Edge connectedEdge = connectedEdges[i_CE];

                if (connectedEdge.Start.Equals(vertexB)) { return connectedEdge; }
                else if (connectedEdge.End.Equals(vertexB)) { return connectedEdge; }
            }

            throw new ArgumentException("There is no edge between the given vertices.");
        }


        /// <summary>
        /// Retrieves an edge in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the edge. </param>
        /// <returns> The <see cref="Edge"/> at the given index, if it exists, <see langword="null"/> otherwise. </returns>
        public override Edge? TryGetEdge(int index)
        {
            _edges.TryGetValue(index, out Edge? edge);

            return edge;
        }

        /// <summary>
        /// Retrieves an edge in this mesh from the index of its end vertices.
        /// </summary>
        /// <param name="indexA"> Index of the first end vertex. </param>
        /// <param name="indexB"> Index of the second end vertex. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices, if it exists, <see langword="null"/> otherwise. </returns>
        public override Edge? TryGetEdge(int indexA, int indexB)
        {
            Vertex vertexA = GetVertex(indexA);
            Vertex vertexB = GetVertex(indexB);
            return TryGetEdge(vertexA, vertexB);
        }

        /// <summary>
        /// Retrieves an edge in this mesh from its end vertices.
        /// </summary>
        /// <param name="vertexA"> First end <see cref="Vertex"/>. </param>
        /// <param name="vertexB"> Second end <see cref="Vertex"/>. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices, if it exists, <see langword="null"/> otherwise. </returns>
        public Edge? TryGetEdge(Vertex vertexA, Vertex vertexB)
        {
            IReadOnlyList<Edge> connectedEdges = vertexA.ConnectedEdges();
            for (int i_CE = 0; i_CE < connectedEdges.Count; i_CE++)
            {
                Edge connectedEdge = connectedEdges[i_CE];

                if (connectedEdge.Start.Equals(vertexB)) { return connectedEdge; }
                else if (connectedEdge.End.Equals(vertexB)) { return connectedEdge; }
            }

            return null;
        }


        /// <summary>
        /// Exposes an enumerator to iterate through the edges of this mesh.
        /// </summary>
        /// <returns> An enumerable set of <see cref="Edge"/>. </returns>
        public override IEnumerable<Edge> GetEdges()
        {
            return _edges.Values;
        }


        /// <summary>
        /// Removes an edge at a given index in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="index"> Index of the edge. </param>
        public override void RemoveEdge(int index)
        {
            Edge edge = GetEdge(index);
            RemoveEdge(edge);
        }

        /// <summary>
        /// Removes an edge in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="edge"> <see cref="Edge"/> to remove. </param>
        public void RemoveEdge(Edge edge)
        {
            List<Face> adjacentFaces = new List<Face>(edge.AdjacentFaces());

            if (edge.IsBoundary() && adjacentFaces.Count == 0)
            {
                Vertex startVertex = edge.Start;
                Vertex endVertex = edge.End;

                EraseEdge(edge);

                // Manage start and end vertex
                if (!startVertex.IsConnected()) { EraseVertex(startVertex); }
                if (!endVertex.IsConnected()) { EraseVertex(endVertex); }
            }
            else
            {
                // Manage connection with adjacent face
                for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
                {
                    RemoveFace(adjacentFaces[i_AF]);
                }
            }
        }


        //     -----     About Faces     -----     //

        /// <summary>
        /// Adds a triangular face to this mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first face vertex. </param>
        /// <param name="indexB"> Index of the second face vertex. </param>
        /// <param name="indexC"> Index of the third face vertex. </param>
        /// <param name="traits"> Traits for the face. </param>
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
        /// <param name="traits"> Traits for the face. </param>
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
        /// <param name="traits"> Traits for the face. </param>
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
        /// <param name="traits"> Traits for the face. </param>
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
        /// <param name="traits"> Traits for the face. </param>
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
        /// <param name="traits"> Traits for the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> A face must have at least three vertices. </exception>
        /// <exception cref="ArgumentException"> The list of vertices must not contain duplicate elements. </exception>
        /// <exception cref="ArgumentException"> One of the input vertex does not belong to this mesh. </exception>
        public Face AddFace(IReadOnlyList<Vertex> vertices, TFaceTraits traits)
        {
            // Verification : The face has at least three vertices.
            if (vertices.Count < 3) { throw new ArgumentOutOfRangeException(nameof(vertices), "A face must have at least three vertices."); }

            // Verification : The vertices are different 
            HashSet<int> indices = new HashSet<int>(vertices.Count);
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex vertex = vertices[i];
                if (indices.Contains(vertex.Index))
                {
                    throw new ArgumentException("The list of vertices must not contain duplicate elements.");
                }
                else { indices.Add(vertex.Index); }
            }

            // Vertification : The vertices belong to this mesh
            foreach (Vertex vertex in vertices)
            {
                if (!vertex.Equals(GetVertex(vertex.Index)))
                {
                    throw new ArgumentException("One of the input vertex does not belong to this mesh.");
                }
            }

            // Create the list of edges around the face
            List<Edge> edges = new List<Edge>();
            for (int i = 0; i < vertices.Count; i++)
            {
                int j = (i + 1) % (vertices.Count);
                Edge? edge = TryGetEdge(vertices[i], vertices[j]);

                if (edge is null)
                {
                    edge = AddEdge(vertices[i], vertices[j]);
                }
                edges.Add(edge);
            }


            // Should check if the face already exists and other things (vertex belonging to the right mesh, etc...)
            Face face = new Face(this, _newFaceIndex, traits, vertices, edges);

            this._faces.Add(_newFaceIndex, face);
            _newFaceIndex += 1;

            return face;
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
            // Manage connection with edges
            IReadOnlyList<Edge> faceEdges = face.FaceEdges();
            for (int i_FE = 0; i_FE < faceEdges.Count; i_FE++)
            {
                Edge edge = faceEdges[i_FE];
                if (edge.IsBoundary())
                {
                    edge.DetachAdjacentFace(face); // Empty the list
                    EraseEdge(edge);
                }
                else
                {
                    edge.DetachAdjacentFace(face);
                }
            }

            // Manage isolated vertices
            IReadOnlyList<Vertex> faceVertices = face.FaceVertices();
            for (int i_FV = 0; i_FV < faceEdges.Count; i_FV++)
            {
                Vertex vertex = faceVertices[i_FV];

                if (!vertex.IsConnected()) { EraseVertex(vertex); }
            }

            // Erase the face.
            EraseFace(face);
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
            // Verification : The vertex must be disconnected.
            if (vertex.Valency() != 0)
            {
                throw new InvalidOperationException("The vertex cannot be erased. It is still connected.");
            }

            // Remove the vertex from the mesh.
            _vertices.Remove(vertex.Index);

            // Unset the vertex.
            vertex.Unset();
        }


        //     -----     About Edges     -----     //

        /// <summary>
        /// Adds an edge to this mesh.
        /// </summary>
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
        /// Adds an edge to this mesh.
        /// </summary>
        /// <param name="start"> Start <see cref="Vertex"/>. </param>
        /// <param name="end"> End <see cref="Vertex"/>. </param>
        /// <param name="traits"> Traits of the edge. </param>
        /// <returns> The new <see cref="Edge"/>. </returns>
        /// <exception cref="ArgumentException"> End vertices must be different. </exception>
        /// <exception cref="ArgumentException"> An edge with the same end vertices already exist. </exception>
        internal Edge AddEdge(Vertex start, Vertex end, TEdgeTraits? traits = default)
        {
            // Verification : Avoid looping edges
            if (start.Equals(end)) { throw new ArgumentException("End vertices must be different."); }

            // Verification : Avoid multiple edges
            Edge? exitingEdge = TryGetEdge(start, end);
            if (exitingEdge is not null) { throw new ArgumentException("An edge with the same end vertices already exist."); }

            Edge edge = new Edge(this, _newEdgeIndex, start, end, traits);

            this._edges.Add(_newEdgeIndex, edge);
            _newEdgeIndex += 1;

            return edge;
        }


        /// <summary>
        /// Erases an edge at a given index in the mesh.
        /// </summary>
        /// <remarks> Every reference to this edge in the mesh should be deleted before it is erased. </remarks>
        /// <param name="index"> Index of the edge. </param>
        protected override void EraseEdge(int index)
        {
            Edge edge = GetEdge(index);
            EraseEdge(edge);
        }

        /// <summary>
        /// Erases an edge in the mesh.
        /// </summary>
        /// <remarks> Every reference to this edge in the mesh should be deleted before it is erased. </remarks>
        /// <param name="edge"> <see cref="Edge"/> to erase. </param>
        protected void EraseEdge(Edge edge)
        {
            // Manage connection with adjacent face
            IReadOnlyList<Face> adjacentFaces = edge.AdjacentFaces();
            for (int i_AF = 0; i_AF < adjacentFaces.Count; i_AF++)
            {
                EraseFace(adjacentFaces[i_AF]);
            }

            // Manage connection with start and end vertices
            edge.Start.DetachConnectedEdge(edge);
            edge.End.DetachConnectedEdge(edge);

            // Remove the pair of edges from the mesh
            _edges.Remove(edge.Index);

            // Unset the pair of edges
            edge.Unset();
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
            IReadOnlyList<Edge> faceEdges = face.FaceEdges();
            for (int i_FE = 0; i_FE < faceEdges.Count; i_FE++)
            {
                Edge edge = faceEdges[i_FE];

                edge.DetachAdjacentFace(face);
            }

            // Remove the face from the mesh
            _faces.Remove(face.Index);

            // Unset the face
            face.Unset();
        }

        #endregion
    }
}
