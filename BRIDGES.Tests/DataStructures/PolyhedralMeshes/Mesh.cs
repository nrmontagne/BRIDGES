using System;
using System.Collections.Generic;

using Xunit;

using PM = BRIDGES.DataStructures.PolyhedralMeshes;

using Tests_Fv = BRIDGES.Tests.DataStructures.PolyhedralMeshes.FaceVertexMeshes;
using Tests_He = BRIDGES.Tests.DataStructures.PolyhedralMeshes.HalfedgeMeshes;


namespace BRIDGES.Tests.DataStructures.PolyhedralMeshes
{
    // Alias
    using Ph_Mesh = PM.Mesh<EmptyTraits, EmptyTraits, EmptyTraits>;

    /// <summary>
    /// Tests the members of the <see cref="Ph_Mesh"/> data structure.
    /// </summary>
    public class Mesh
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Ph_Mesh.VertexCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of vertices. </param>
        [Theory(DisplayName = "Prop. VertexCount")]
        [ClassData(typeof(ClassDatas.VertexCount))]
        public void Property_VertexCount(Ph_Mesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.VertexCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="Ph_Mesh.EdgeCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of edges. </param>
        [Theory(DisplayName = "Prop. EdgeCount")]
        [ClassData(typeof(ClassDatas.EdgeCount))]
        public void Property_EdgeCount(Ph_Mesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.EdgeCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="Ph_Mesh.FaceCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of faces. </param>
        [Theory(DisplayName = "Prop. FaceCount")]
        [ClassData(typeof(ClassDatas.FaceCount))]
        public void Property_FaceCount(Ph_Mesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.FaceCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Tests : Public Methods

        //     -----     About Vertices     -----     //

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.AddVertex(EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddVertex(VertexTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddVertex_VertexTraits(Ph_Mesh mesh)
        {
            EmptyTraits traits = new EmptyTraits();

            // Act
            Ph_Mesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(1, mesh.VertexCount);
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.GetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="Ph_Mesh.Vertex"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="Ph_Mesh.Vertex"/> index. </param>
        [Theory(DisplayName = "GetVertex(int) - Existence")]
        [ClassData(typeof(ClassDatas.VertexExistence))]
        public void GetVertex_Int__Existence(Ph_Mesh mesh, int existingIndex, int nonExistentIndex)
        {
            bool throwsException = false;

            // Act
            Ph_Mesh.Vertex existingVertex = mesh.GetVertex(existingIndex);

            Ph_Mesh.Vertex? nonExistentVertex = null;
            try { nonExistentVertex = mesh.GetVertex(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingVertex!.Index);

            Assert.True(throwsException);
            Assert.True(nonExistentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.GetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "GetVertex(int) - Created")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void GetVertex_Int__Created(Ph_Mesh mesh)
        {
            // Arrange
            EmptyTraits traits = new EmptyTraits();

            // Act
            Ph_Mesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(vertex, mesh.GetVertex(0));
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.TryGetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="Ph_Mesh.Vertex"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="Ph_Mesh.Vertex"/> index. </param>
        [Theory(DisplayName = "TryGetVertex(int) - Existence")]
        [ClassData(typeof(ClassDatas.VertexExistence))]
        public void TryGetVertex_Int__Existence(Ph_Mesh mesh, int existingIndex, int nonExistentIndex)
        {
            // Arrange
            bool throwsException = false;

            // Act
            Ph_Mesh.Vertex? existingVertex = mesh.TryGetVertex(existingIndex);

            Ph_Mesh.Vertex? nonExistentVertex = null;
            try { nonExistentVertex = mesh.TryGetVertex(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingVertex!.Index);

            Assert.False(throwsException);
            Assert.True(nonExistentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.TryGetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "TryGetVertex(int) - Created")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void TryGetVertex_Int__Created(Ph_Mesh mesh)
        {
            // Arrange
            EmptyTraits traits = new EmptyTraits();

            // Act
            Ph_Mesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(vertex, mesh.TryGetVertex(0));
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.GetVertices()"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "GetVertices()")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void GetVertices(Ph_Mesh mesh)
        {
            // Arrange
            EmptyTraits vertexTrait = new EmptyTraits();

            Ph_Mesh.Vertex[] vertices =
            [
                mesh.AddVertex(vertexTrait),
                mesh.AddVertex(vertexTrait),
                mesh.AddVertex(vertexTrait),
                mesh.AddVertex(vertexTrait),
                mesh.AddVertex(vertexTrait),
                mesh.AddVertex(vertexTrait),
                mesh.AddVertex(vertexTrait),
                mesh.AddVertex(vertexTrait),
            ];

            // Act and Assert
            foreach (Ph_Mesh.Vertex vertex in mesh.GetVertices())
            {
                Assert.Equal(vertices[vertex.Index], vertex);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.RemoveVertex(int))"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="vertexIndex"> Index of the <see cref="Ph_Mesh.Vertex"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the vertex is removed.</param>
        /// <param name="edgeCount"> Number of edges after the vertex is removed.</param>
        /// <param name="faceCount"> Number of faces after the vertex is removed.</param>
        [Theory(DisplayName = "RemoveVertex(int)")]
        [ClassData(typeof(ClassDatas.VertexRemoval))]
        public void RemoveVertex_Int(Ph_Mesh mesh, int vertexIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange

            // Act
            mesh.RemoveVertex(vertexIndex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }


        //     -----     About Edges     -----     //

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.GetEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="Ph_Mesh.Edge"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="Ph_Mesh.Edge"/> index. </param>
        /// <param name="edgesEndsIndices"> Indices of the edge end vertices. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the edge index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the start (0) and end (1) vertex. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "GetEdge(int)")]
        [ClassData(typeof(ClassDatas.EdgeIndices))]
        public void GetEdge_Int(Ph_Mesh mesh, int existingIndex, int nonExistentIndex, int[,] edgesEndsIndices)
        {
            // Arrange
            bool throwsException = false;

            // Act
            Ph_Mesh.Edge existingEdge = mesh.GetEdge(existingIndex);

            Ph_Mesh.Edge? nonExistentEdge = null;
            try { nonExistentEdge = mesh.GetEdge(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingEdge.Index);
            Assert.Equal(mesh.GetVertex(edgesEndsIndices[existingIndex, 0]), existingEdge.Start);
            Assert.Equal(mesh.GetVertex(edgesEndsIndices[existingIndex, 1]), existingEdge.End);

            Assert.True(throwsException);
            Assert.True(nonExistentEdge is null);
        }

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.GetEdge(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="Ph_Mesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="Ph_Mesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="Ph_Mesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "GetEdge(int,int)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void GetEdge_Int_Int(Ph_Mesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            int[] directEdgeEnds = new int[2] { existingEnds[0], existingEnds[1] };
            int[] inverseEdgeEnds = new int[2] { existingEnds[1], existingEnds[0] };
            int[] nonExistentEdgeEnds = new int[2] { nonExistentEnds[0], nonExistentEnds[1] };

            bool throwsException = false;

            // Act
            Ph_Mesh.Edge directEdge = mesh.GetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            Ph_Mesh.Edge inverseEdge = mesh.GetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            Ph_Mesh.Edge? nonExistentEdge = null;
            try { nonExistentEdge = mesh.GetEdge(nonExistentEdgeEnds[0], nonExistentEdgeEnds[1]); ; }
            catch (ArgumentException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, directEdge!.Index);
            Assert.Equal(mesh.GetVertex(directEdgeEnds[0]), directEdge.Start);
            Assert.Equal(mesh.GetVertex(directEdgeEnds[1]), directEdge.End);

            Assert.Equal(existingIndex, inverseEdge!.Index);
            Assert.Equal(mesh.GetVertex(inverseEdgeEnds[1]), inverseEdge.Start);
            Assert.Equal(mesh.GetVertex(inverseEdgeEnds[0]), inverseEdge.End);

            Assert.True(throwsException);
            Assert.True(nonExistentEdge is null);
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.TryGetEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="Ph_Mesh.Edge"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="Ph_Mesh.Edge"/> index. </param>
        /// <param name="edgesEndsIndices"> Indices of the edge end vertices. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the edge index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the start (0) and end (1) vertex. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "TryGetEdge(int)")]
        [ClassData(typeof(ClassDatas.EdgeIndices))]
        public void TryGetEdge_Int(Ph_Mesh mesh, int existingIndex, int nonExistentIndex, int[,] edgesEndsIndices)
        {
            // Arrange
            bool throwsException = false;

            // Act
            Ph_Mesh.Edge? existingEdge = mesh.TryGetEdge(existingIndex);

            Ph_Mesh.Edge? nonExistentEdge = null;
            try { nonExistentEdge = mesh.TryGetEdge(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingEdge!.Index);
            Assert.Equal(mesh.GetVertex(edgesEndsIndices[existingIndex, 0]), existingEdge.Start);
            Assert.Equal(mesh.GetVertex(edgesEndsIndices[existingIndex, 1]), existingEdge.End);

            Assert.False(throwsException);
            Assert.True(nonExistentEdge is null);
        }

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.TryGetEdge(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="Ph_Mesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="Ph_Mesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="Ph_Mesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "TryGetEdge(int,int)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void TryGetEdge_Int_Int(Ph_Mesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            int[] directEdgeEnds = new int[2] { existingEnds[0], existingEnds[1] };
            int[] inverseEdgeEnds = new int[2] { existingEnds[1], existingEnds[0] };
            int[] nonExistentEdgeEnds = new int[2] { nonExistentEnds[0], nonExistentEnds[1] };

            bool throwsException = false;

            // Act
            Ph_Mesh.Edge? directEdge = mesh.TryGetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            Ph_Mesh.Edge? inverseEdge = mesh.TryGetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            Ph_Mesh.Edge? nonExistentEdge = null;
            try { nonExistentEdge = mesh.TryGetEdge(nonExistentEdgeEnds[0], nonExistentEdgeEnds[1]); ; }
            catch (ArgumentException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, directEdge!.Index);
            Assert.Equal(mesh.GetVertex(directEdgeEnds[0]), directEdge.Start);
            Assert.Equal(mesh.GetVertex(directEdgeEnds[1]), directEdge.End);

            Assert.Equal(existingIndex, inverseEdge!.Index);
            Assert.Equal(mesh.GetVertex(inverseEdgeEnds[1]), inverseEdge.Start);
            Assert.Equal(mesh.GetVertex(inverseEdgeEnds[0]), inverseEdge.End);

            Assert.False(throwsException);
            Assert.True(nonExistentEdge is null);
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.GetEdges()"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="edgesEndsIndices"> Indices of the edge end vertices. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the edge index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the start (0) and end (1) vertex. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "GetEdges()")]
        [ClassData(typeof(ClassDatas.EdgesEndIndices))]
        public void GetEdges(Ph_Mesh mesh, int[,] edgesEndsIndices)
        {
            // Arrange

            // Act & Assert
            foreach (Ph_Mesh.Edge edge in mesh.GetEdges())
            {
                Assert.Equal(mesh.GetVertex(edgesEndsIndices[edge.Index, 0]), edge.Start);
                Assert.Equal(mesh.GetVertex(edgesEndsIndices[edge.Index, 1]), edge.End);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.RemoveEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="edgeIndex"> Index of the <see cref="Ph_Mesh.Edge"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the vertex is removed.</param>
        /// <param name="edgeCount"> Number of edges after the vertex is removed.</param>
        /// <param name="faceCount"> Number of faces after the vertex is removed.</param>
        [Theory(DisplayName = "RemoveEdge(int)")]
        [ClassData(typeof(ClassDatas.EdgeRemoval))]
        public void RemoveEdge_Int(Ph_Mesh mesh, int edgeIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange

            // Act
            mesh.RemoveEdge(edgeIndex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }


        //     -----     About Faces     -----     //

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.AddFace(int, int, int, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(int,int,int,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Int_Int_Int_FaceTraits(Ph_Mesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<Ph_Mesh.Vertex> vertices = new List<Ph_Mesh.Vertex>();
            for (int i = 0; i < 3; i++)
            {
                Ph_Mesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            Ph_Mesh.Face face = mesh.AddFace(0, 1, 2, faceTraits);

            // Assert
            IReadOnlyList<Ph_Mesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<Ph_Mesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(3, mesh.VertexCount);
            Assert.Equal(3, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 3; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 3; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.AddFace(int, int, int, int, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(int,int,int,int,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Int_Int_Int_Int_FaceTraits(Ph_Mesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<Ph_Mesh.Vertex> vertices = new List<Ph_Mesh.Vertex>();
            for (int i = 0; i < 4; i++)
            {
                Ph_Mesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            Ph_Mesh.Face face = mesh.AddFace(0, 1, 2, 3, faceTraits);

            // Assert
            IReadOnlyList<Ph_Mesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<Ph_Mesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(4, mesh.VertexCount);
            Assert.Equal(4, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 4; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.AddFace(IReadOnlyList{int}, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(IReadOnlyList<int>,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_IReadOnlyListOfInt_FaceTraits(Ph_Mesh mesh)
        {
            // Arrange
            int faceVertexCount = 5;
            int faceEdgeCount = faceVertexCount;

            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<int> verticesIndex = new List<int>(faceVertexCount);
            List<Ph_Mesh.Vertex> vertices = new List<Ph_Mesh.Vertex>(faceVertexCount);
            for (int i = 0; i < faceVertexCount; i++)
            {
                Ph_Mesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
                verticesIndex.Add(i);
            }

            Ph_Mesh.Face face = mesh.AddFace(verticesIndex, faceTraits);

            // Assert
            IReadOnlyList<Ph_Mesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<Ph_Mesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(faceVertexCount, mesh.VertexCount);
            Assert.Equal(faceEdgeCount, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < faceVertexCount; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < faceEdgeCount; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.GetFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="Ph_Mesh.Face"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="Ph_Mesh.Face"/> index. </param>
        /// <param name="facesEdgesIndices"> Indices of the face edges. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the face index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the index of the edge in the face edge set. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "GetFace(int)")]
        [ClassData(typeof(ClassDatas.FaceIndices))]
        public void GetFace_Int(Ph_Mesh mesh, int existingIndex, int nonExistentIndex, int[][] facesEdgesIndices)
        {
            // Arrange
            int[] faceEdgesIndices = facesEdgesIndices[existingIndex];
            Ph_Mesh.Edge[] edges = new Ph_Mesh.Edge[faceEdgesIndices.Length];
            for (int i = 0; i < faceEdgesIndices.Length; i++)
            {
                int index = Math.Abs(faceEdgesIndices[i]);
                edges[i] = mesh.GetEdge(index);
            }

            bool throwsException = false;

            // Act
            Ph_Mesh.Face existingFace = mesh.GetFace(existingIndex);
            IReadOnlyList<Ph_Mesh.Edge> faceEdges = existingFace.FaceEdges();

            Ph_Mesh.Face? nonExistentFace = null;
            try { nonExistentFace = mesh.GetFace(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(edges[i_E], faceEdges[i_E]); }

            Assert.True(throwsException);
            Assert.True(nonExistentFace is null);
        }

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.TryGetFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="Ph_Mesh.Face"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="Ph_Mesh.Face"/> index. </param>
        /// <param name="facesEdgesIndices"> Indices of the face edges. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the face index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the index of the edge in the face edge set. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "TryGetFace(int)")]
        [ClassData(typeof(ClassDatas.FaceIndices))]
        public void TryGetFace_Int(Ph_Mesh mesh, int existingIndex, int nonExistentIndex, int[][] facesEdgesIndices)
        {
            // Arrange
            int[] faceEdgesIndices = facesEdgesIndices[existingIndex];
            Ph_Mesh.Edge[] edges = new Ph_Mesh.Edge[faceEdgesIndices.Length];
            for (int i = 0; i < faceEdgesIndices.Length; i++)
            {
                int index = Math.Abs(faceEdgesIndices[i]);
                edges[i] = mesh.GetEdge(index);
            }

            bool throwsException = false;

            // Act
            Ph_Mesh.Face? existingFace = mesh.TryGetFace(existingIndex);
            IReadOnlyList<Ph_Mesh.Edge> faceEdges = existingFace!.FaceEdges();

            Ph_Mesh.Face? nonExistentFace = null;
            try { nonExistentFace = mesh.TryGetFace(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(edges[i_E], faceEdges[i_E]); }

            Assert.False(throwsException);
            Assert.True(nonExistentFace is null);
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.GetFaces()"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="facesEdgesIndices"> Indices of the face edges. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the face index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the index of the edge in the face edge set. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "GetFaces()")]
        [ClassData(typeof(ClassDatas.GetFaces))]
        public void GetFaces(Ph_Mesh mesh, int[][] facesEdgesIndices)
        {
            // Arrange

            // Act & Assert
            foreach (Ph_Mesh.Face face in mesh.GetFaces())
            {
                int[] faceEdgesIndices = facesEdgesIndices[face.Index];

                IReadOnlyList<Ph_Mesh.Edge> faceEdges = face.FaceEdges();
                for (int i = 0; i < faceEdges.Count; i++)
                {
                    Assert.Equal(Math.Abs(faceEdgesIndices[i]), faceEdges[i].Index);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.RemoveFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="faceIndex"> Index of the <see cref="Ph_Mesh.Face"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the face is removed.</param>
        /// <param name="edgeCount"> Number of edges after the face is removed.</param>
        /// <param name="faceCount"> Number of faces after the face is removed.</param>
        [Theory(DisplayName = "RemoveFace(int)")]
        [ClassData(typeof(ClassDatas.FaceRemoval))]
        public void RemoveFace_Int(Ph_Mesh mesh, int faceIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange

            // Act
            mesh.RemoveFace(faceIndex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }

        #endregion

        #region Tests : Internal Methods

        //     -----     About Edges     -----     //

        /// <summary>
        /// Tests the method <see cref="Ph_Mesh.AddEdge(int, int, EmptyTraits)"/>.
        /// </summary>
        [Theory(DisplayName = "AddEdge(int, int, EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddEdge_Int_Int_EdgeTraits(Ph_Mesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            Ph_Mesh.Vertex vertex0 = mesh.AddVertex(vertexTraits);
            Ph_Mesh.Vertex vertex1 = mesh.AddVertex(vertexTraits);

            // Act
            EmptyTraits edgeTraits = new EmptyTraits();
            Ph_Mesh.Edge edge = mesh.AddEdge(0, 1, edgeTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, edge.Index);
            Assert.Equal(vertex0, edge.Start);
            Assert.Equal(vertex1, edge.End);
        }

        #endregion


        #region Data Classes for Parametrised Tests

        internal static class ClassDatas
        {
            /// <summary>
            /// Class data providing an empty mesh.
            /// </summary>
            internal class EmptyMesh : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Empty.WritableMesh },
                    { Tests_He.HalfedgeMesh.DataStorages.Empty.WritableMesh },
                    };
            }

            //     -----     Properties

            /// <summary>
            /// Class data for <see cref="Mesh.Property_VertexCount(Ph_Mesh, int)"/>.
            /// </summary>
            internal class VertexCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.ReadableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.VertexCount },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.ReadableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.VertexCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Mesh.Property_EdgeCount(Ph_Mesh, int)"/>.
            /// </summary>
            internal class EdgeCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.ReadableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.EdgeCount },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.ReadableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.EdgeCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Mesh.Property_FaceCount(Ph_Mesh, int)"/>.
            /// </summary>
            internal class FaceCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.ReadableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.FaceCount },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.ReadableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.FaceCount },
                    };
            }


            //     -----     Methods

            /// <summary>
            /// Class data for
            /// <see cref="Mesh.GetVertex_Int__Existence(Ph_Mesh, int, int)"/> and
            /// <see cref="Mesh.TryGetVertex_Int__Existence(Ph_Mesh, int, int)"/>.
            /// </summary>
            internal class VertexExistence : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.ReadableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.VertexExistence },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.ReadableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.VertexExistence },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Mesh.RemoveVertex_Int(Ph_Mesh, int, int, int, int)"/>.
            /// </summary>
            internal class VertexRemoval : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.WritableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.VertexRemoval },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.WritableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.VertexRemoval },
                    };
            }


            /// <summary>
            /// Class data for 
            /// <see cref="Mesh.GetEdge_Int(Ph_Mesh, int, int, int[,])"/> and
            /// <see cref="Mesh.TryGetEdge_Int(Ph_Mesh, int, int, int[,])"/>.
            /// </summary>
            internal class EdgeIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.WritableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.EdgeIndices, Tests_Fv.FaceVertexMesh.DataStorages.Cube.EdgesEndIndices },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.WritableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.EdgeIndices, Tests_He.HalfedgeMesh.DataStorages.Cube.EdgesEndIndices },
                    };
            }

            /// <summary>
            /// Class data for 
            /// <see cref="Mesh.GetEdge_Int_Int(Ph_Mesh, int, int[], int[])"/> and
            /// <see cref="Mesh.TryGetEdge_Int_Int(Ph_Mesh, int, int[], int[])"/>.
            /// </summary>
            internal class EdgeEndIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.ReadableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.EdgeEndIndices },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.ReadableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.EdgeEndIndices },
                    };
            }


            /// <summary>
            /// Class data for <see cref="Mesh.GetEdges(Ph_Mesh, int[,])"/>.
            /// </summary>
            internal class EdgesEndIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.ReadableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.EdgesEndIndices },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.ReadableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.EdgesEndIndices },
                    };
            }


            /// <summary>
            /// Class data for <see cref="Mesh.RemoveEdge_Int(Ph_Mesh, int, int, int, int)"/>.
            /// </summary>
            internal class EdgeRemoval : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.WritableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.EdgeRemoval },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.WritableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.EdgeRemoval },
                    };
            }


            /// <summary>
            /// Class data for 
            /// <see cref="Mesh.GetFace_Int(Ph_Mesh, int, int, int[][])"/> and
            /// <see cref="Mesh.TryGetFace_Int(Ph_Mesh, int, int, int[][])"/>.
            /// </summary>
            internal class FaceIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.ReadableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.FaceIndices, Tests_Fv.FaceVertexMesh.DataStorages.Cube.FacesEdgesIndices },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.ReadableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.FaceIndices, Tests_He.HalfedgeMesh.DataStorages.Cube.FacesEdgesIndices },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Mesh.GetFaces(Ph_Mesh, int[][])"/>.
            /// </summary>
            internal class GetFaces : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.ReadableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.FacesEdgesIndices },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.ReadableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.FacesEdgesIndices },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Mesh.RemoveFace_Int(Ph_Mesh, int, int, int, int)"/>.
            /// </summary>
            internal class FaceRemoval : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                    { Tests_Fv.FaceVertexMesh.DataStorages.Cube.WritableMesh, Tests_Fv.FaceVertexMesh.DataStorages.Cube.RemoveFace },
                    { Tests_He.HalfedgeMesh.DataStorages.Cube.WritableMesh, Tests_He.HalfedgeMesh.DataStorages.Cube.RemoveFace },
                    };
            }
        }

        #endregion
    }
}