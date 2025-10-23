using System;
using System.Collections.Generic;

using Xunit;

using Fv = BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMeshes;


namespace BRIDGES.Tests.DataStructures.PolyhedralMeshes.FaceVertexMeshes
{
    // Alias
    using FvMesh = Fv.FaceVertexMesh<EmptyTraits, EmptyTraits, EmptyTraits>;


    /// <summary>
    /// Tests the members of the <see cref="FvMesh"/> data structure.
    /// </summary>
    public class FaceVertexMesh
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="FvMesh.VertexCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of vertices. </param>
        [Theory(DisplayName = "Prop. VertexCount")]
        [ClassData(typeof(ClassDatas.VertexCount))]
        public void Property_VertexCount(FvMesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.VertexCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="FvMesh.EdgeCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of edges. </param>
        [Theory(DisplayName = "Prop. EdgeCount")]
        [ClassData(typeof(ClassDatas.EdgeCount))]
        public void Property_EdgeCount(FvMesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.EdgeCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="FvMesh.FaceCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of faces. </param>
        [Theory(DisplayName = "Prop. FaceCount")]
        [ClassData(typeof(ClassDatas.FaceCount))]
        public void Property_FaceCount(FvMesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.FaceCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Tests : Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="FvMesh"/>.
        /// </summary>
        [Fact(DisplayName = "FvMesh.FvMesh()")]
        public void Constructor()
        {
            // Arrange
            FvMesh mesh = new FvMesh();

            //Act
            int vertexCount = mesh.VertexCount;
            int edgeCount = mesh.EdgeCount;
            int faceCount = mesh.FaceCount;

            // Assert
            Assert.Equal(0, vertexCount);
            Assert.Equal(0, edgeCount);
            Assert.Equal(0, faceCount);
        }

        #endregion

        #region Tests : Public Methods

        //     -----     About Vertices     -----     //

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddVertex(EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddVertex(VertexTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddVertex_VertexTraits(FvMesh mesh)
        {
            EmptyTraits traits = new EmptyTraits();

            // Act
            FvMesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(1, mesh.VertexCount);
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.GetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Vertex"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="FvMesh.Vertex"/> index. </param>
        [Theory(DisplayName = "GetVertex(int) - Existence")]
        [ClassData(typeof(ClassDatas.VertexExistence))]
        public void GetVertex_Int__Existence(FvMesh mesh, int existingIndex, int nonExistentIndex)
        {
            bool throwsException = false;

            // Act
            FvMesh.Vertex existingVertex = mesh.GetVertex(existingIndex);

            FvMesh.Vertex? nonExistentVertex = null;
            try { nonExistentVertex = mesh.GetVertex(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingVertex!.Index);

            Assert.True(throwsException);
            Assert.True(nonExistentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.GetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "GetVertex(int) - Created")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void GetVertex_Int__Created(FvMesh mesh)
        {
            // Arrange
            EmptyTraits traits = new EmptyTraits();

            // Act
            FvMesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(vertex, mesh.GetVertex(0));
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.TryGetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Vertex"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="FvMesh.Vertex"/> index. </param>
        [Theory(DisplayName = "TryGetVertex(int) - Existence")]
        [ClassData(typeof(ClassDatas.VertexExistence))]
        public void TryGetVertex_Int__Existence(FvMesh mesh, int existingIndex, int nonExistentIndex)
        {
            // Arrange
            bool throwsException = false;

            // Act
            FvMesh.Vertex? existingVertex = mesh.TryGetVertex(existingIndex);

            FvMesh.Vertex? nonExistentVertex = null;
            try { nonExistentVertex = mesh.TryGetVertex(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingVertex!.Index);

            Assert.False(throwsException);
            Assert.True(nonExistentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.TryGetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "TryGetVertex(int) - Created")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void TryGetVertex_Int__Created(FvMesh mesh)
        {
            // Arrange
            EmptyTraits traits = new EmptyTraits();

            // Act
            FvMesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(vertex, mesh.TryGetVertex(0));
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.GetVertices()"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "GetVertices()")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void GetVertices(FvMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTrait = new EmptyTraits();

            FvMesh.Vertex[] vertices =
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
            foreach (FvMesh.Vertex vertex in mesh.GetVertices())
            {
                Assert.Equal(vertices[vertex.Index], vertex);
            }
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.RemoveVertex(int))"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="vertexIndex"> Index of the <see cref="FvMesh.Vertex"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the vertex is removed.</param>
        /// <param name="edgeCount"> Number of edges after the vertex is removed.</param>
        /// <param name="faceCount"> Number of faces after the vertex is removed.</param>
        [Theory(DisplayName = "RemoveVertex(int)")]
        [ClassData(typeof(ClassDatas.VertexRemoval))]
        public void RemoveVertex_Int(FvMesh mesh, int vertexIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange

            // Act
            mesh.RemoveVertex(vertexIndex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.RemoveVertex(int))"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="vertexIndex"> Index of the <see cref="FvMesh.Vertex"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the vertex is removed.</param>
        /// <param name="edgeCount"> Number of edges after the vertex is removed.</param>
        /// <param name="faceCount"> Number of faces after the vertex is removed.</param>
        [Theory(DisplayName = "RemoveVertex(Vertex)")]
        [ClassData(typeof(ClassDatas.VertexRemoval))]
        public void RemoveVertex_Vertex(FvMesh mesh, int vertexIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange
            FvMesh.Vertex vertex = mesh.GetVertex(vertexIndex);

            // Act
            mesh.RemoveVertex(vertex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }


        //     -----     About Edges     -----     //

        /// <summary>
        /// Tests the method <see cref="FvMesh.GetEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Edge"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="FvMesh.Edge"/> index. </param>
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
        public void GetEdge_Int(FvMesh mesh, int existingIndex, int nonExistentIndex, int[,] edgesEndsIndices)
        {
            // Arrange
            bool throwsException = false;

            // Act
            FvMesh.Edge existingEdge = mesh.GetEdge(existingIndex);

            FvMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="FvMesh.GetEdge(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="FvMesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="FvMesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "GetEdge(int,int)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void GetEdge_Int_Int(FvMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            int[] directEdgeEnds = new int[2] { existingEnds[0], existingEnds[1] };
            int[] inverseEdgeEnds = new int[2] { existingEnds[1], existingEnds[0] };
            int[] nonExistentEdgeEnds = new int[2] { nonExistentEnds[0], nonExistentEnds[1] };

            bool throwsException = false;

            // Act
            FvMesh.Edge directEdge = mesh.GetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            FvMesh.Edge inverseEdge = mesh.GetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            FvMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="FvMesh.GetEdge(FvMesh.Vertex, FvMesh.Vertex)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="FvMesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="FvMesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "GetEdge(Vertex,Vertex)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void GetEdge_Vertex_Vertex(FvMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            FvMesh.Vertex[] directEdgeEnds = [mesh.GetVertex(existingEnds[0]), mesh.GetVertex(existingEnds[1])];
            FvMesh.Vertex[] inverseEdgeEnds = [mesh.GetVertex(existingEnds[1]), mesh.GetVertex(existingEnds[0])];
            FvMesh.Vertex[] nonExistentEdgeEnds = [mesh.GetVertex(nonExistentEnds[0]), mesh.GetVertex(nonExistentEnds[1])];

            bool throwsException = false;

            // Act

            FvMesh.Edge directEdge = mesh.GetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            FvMesh.Edge inverseEdge = mesh.GetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            FvMesh.Edge? nonExistentEdge = null;
            try { nonExistentEdge = mesh.GetEdge(nonExistentEdgeEnds[0], nonExistentEdgeEnds[1]); ; }
            catch (ArgumentException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, directEdge!.Index);
            Assert.Equal(directEdgeEnds[0], directEdge.Start);
            Assert.Equal(directEdgeEnds[1], directEdge.End);

            Assert.Equal(existingIndex, inverseEdge!.Index);
            Assert.Equal(inverseEdgeEnds[1], inverseEdge.Start);
            Assert.Equal(inverseEdgeEnds[0], inverseEdge.End);

            Assert.True(throwsException);
            Assert.True(nonExistentEdge is null);
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.TryGetEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Edge"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="FvMesh.Edge"/> index. </param>
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
        public void TryGetEdge_Int(FvMesh mesh, int existingIndex, int nonExistentIndex, int[,] edgesEndsIndices)
        {
            // Arrange
            bool throwsException = false;

            // Act
            FvMesh.Edge? existingEdge = mesh.TryGetEdge(existingIndex);

            FvMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="FvMesh.TryGetEdge(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="FvMesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="FvMesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "TryGetEdge(int,int)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void TryGetEdge_Int_Int(FvMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            int[] directEdgeEnds = new int[2] { existingEnds[0], existingEnds[1] };
            int[] inverseEdgeEnds = new int[2] { existingEnds[1], existingEnds[0] };
            int[] nonExistentEdgeEnds = new int[2] { nonExistentEnds[0], nonExistentEnds[1] };

            bool throwsException = false;

            // Act
            FvMesh.Edge? directEdge = mesh.TryGetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            FvMesh.Edge? inverseEdge = mesh.TryGetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            FvMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="FvMesh.TryGetEdge(FvMesh.Vertex, FvMesh.Vertex)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="FvMesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="FvMesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "TryGetEdge(Vertex,Vertex)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void TryGetEdge_Vertex_Vertex(FvMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            FvMesh.Vertex[] directEdgeEnds = [mesh.GetVertex(existingEnds[0]), mesh.GetVertex(existingEnds[1])];
            FvMesh.Vertex[] inverseEdgeEnds = [mesh.GetVertex(existingEnds[1]), mesh.GetVertex(existingEnds[0])];
            FvMesh.Vertex[] nonExistentEdgeEnds = [mesh.GetVertex(nonExistentEnds[0]), mesh.GetVertex(nonExistentEnds[1])];

            bool throwsException = false;

            // Act
            FvMesh.Edge? directEdge = mesh.TryGetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            FvMesh.Edge? inverseEdge = mesh.TryGetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            FvMesh.Edge? nonExistentEdge = null;
            try { nonExistentEdge = mesh.TryGetEdge(nonExistentEdgeEnds[0], nonExistentEdgeEnds[1]); ; }
            catch (ArgumentException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, directEdge!.Index);
            Assert.Equal(directEdgeEnds[0], directEdge.Start);
            Assert.Equal(directEdgeEnds[1], directEdge.End);

            Assert.Equal(existingIndex, inverseEdge!.Index);
            Assert.Equal(inverseEdgeEnds[1], inverseEdge.Start);
            Assert.Equal(inverseEdgeEnds[0], inverseEdge.End);

            Assert.False(throwsException);
            Assert.True(nonExistentEdge is null);
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.GetEdges()"/>.
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
        public void GetEdges(FvMesh mesh, int[,] edgesEndsIndices)
        {
            // Arrange

            // Act & Assert
            foreach (FvMesh.Edge edge in mesh.GetEdges())
            {
                Assert.Equal(mesh.GetVertex(edgesEndsIndices[edge.Index, 0]), edge.Start);
                Assert.Equal(mesh.GetVertex(edgesEndsIndices[edge.Index, 1]), edge.End);
            }
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.RemoveEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="edgeIndex"> Index of the <see cref="FvMesh.Edge"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the vertex is removed.</param>
        /// <param name="edgeCount"> Number of edges after the vertex is removed.</param>
        /// <param name="faceCount"> Number of faces after the vertex is removed.</param>
        [Theory(DisplayName = "RemoveEdge(int)")]
        [ClassData(typeof(ClassDatas.EdgeRemoval))]
        public void RemoveEdge_Int(FvMesh mesh, int edgeIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange

            // Act
            mesh.RemoveEdge(edgeIndex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.RemoveEdge(FvMesh.Edge)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="edgeIndex"> Index of the <see cref="FvMesh.Edge"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the vertex is removed.</param>
        /// <param name="edgeCount"> Number of edges after the vertex is removed.</param>
        /// <param name="faceCount"> Number of faces after the vertex is removed.</param>
        [Theory(DisplayName = "RemoveEdge(Vertex)")]
        [ClassData(typeof(ClassDatas.EdgeRemoval))]
        public void RemoveEdge_Edge(FvMesh mesh, int edgeIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange
            FvMesh.Edge edge = mesh.GetEdge(edgeIndex);

            // Act
            mesh.RemoveEdge(edge);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }


        //     -----     About Faces     -----     //

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddFace(int, int, int, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(int,int,int,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Int_Int_Int_FaceTraits(FvMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<FvMesh.Vertex> vertices = new List<FvMesh.Vertex>();
            for (int i = 0; i < 3; i++)
            {
                FvMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            FvMesh.Face face = mesh.AddFace(0, 1, 2, faceTraits);

            // Assert
            IReadOnlyList<FvMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<FvMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(3, mesh.VertexCount);
            Assert.Equal(3, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 3; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 3; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddFace(FvMesh.Vertex, FvMesh.Vertex, FvMesh.Vertex, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(Vertex,Vertex,Vertex,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Vertex_Vertex_Vertex_FaceTraits(FvMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<FvMesh.Vertex> vertices = new List<FvMesh.Vertex>();
            for (int i = 0; i < 3; i++)
            {
                FvMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            FvMesh.Face face = mesh.AddFace(vertices[0], vertices[1], vertices[2], faceTraits);

            // Assert
            IReadOnlyList<FvMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<FvMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(3, mesh.VertexCount);
            Assert.Equal(3, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 3; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 3; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddFace(int, int, int, int, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(int,int,int,int,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Int_Int_Int_Int_FaceTraits(FvMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<FvMesh.Vertex> vertices = new List<FvMesh.Vertex>();
            for (int i = 0; i < 4; i++)
            {
                FvMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            FvMesh.Face face = mesh.AddFace(0, 1, 2, 3, faceTraits);

            // Assert
            IReadOnlyList<FvMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<FvMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(4, mesh.VertexCount);
            Assert.Equal(4, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 4; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddFace(FvMesh.Vertex, FvMesh.Vertex, FvMesh.Vertex, FvMesh.Vertex, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(Vertex,Vertex,Vertex,Vertex,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Vertex_Vertex_Vertex_Vertex_FaceTraits(FvMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<FvMesh.Vertex> vertices = new List<FvMesh.Vertex>();
            for (int i = 0; i < 4; i++)
            {
                FvMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            FvMesh.Face face = mesh.AddFace(vertices[0], vertices[1], vertices[2], vertices[3], faceTraits);

            // Assert
            IReadOnlyList<FvMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<FvMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(4, mesh.VertexCount);
            Assert.Equal(4, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 4; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddFace(IReadOnlyList{int}, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(IReadOnlyList<int>,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_IReadOnlyListOfInt_FaceTraits(FvMesh mesh)
        {
            // Arrange
            int faceVertexCount = 5;
            int faceEdgeCount = faceVertexCount;

            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<int> verticesIndex = new List<int>(faceVertexCount);
            List<FvMesh.Vertex> vertices = new List<FvMesh.Vertex>(faceVertexCount);
            for (int i = 0; i < faceVertexCount; i++)
            {
                FvMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
                verticesIndex.Add(i);
            }

            FvMesh.Face face = mesh.AddFace(verticesIndex, faceTraits);

            // Assert
            IReadOnlyList<FvMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<FvMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(faceVertexCount, mesh.VertexCount);
            Assert.Equal(faceEdgeCount, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < faceVertexCount; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < faceEdgeCount; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddFace(IReadOnlyList{FvMesh.Vertex}, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(IReadOnlyList<Vertex>,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_IReadOnlyListOfVertex_FaceTraits(FvMesh mesh)
        {
            // Arrange
            int faceVertexCount = 5;
            int faceEdgeCount = faceVertexCount;

            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<int> verticesIndex = new List<int>(faceVertexCount);
            List<FvMesh.Vertex> vertices = new List<FvMesh.Vertex>(faceVertexCount);
            for (int i = 0; i < faceVertexCount; i++)
            {
                FvMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
                verticesIndex.Add(i);
            }

            FvMesh.Face face = mesh.AddFace(vertices, faceTraits);

            // Assert
            IReadOnlyList<FvMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<FvMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(faceVertexCount, mesh.VertexCount);
            Assert.Equal(faceEdgeCount, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < faceVertexCount; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < faceEdgeCount; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.GetFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Face"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="FvMesh.Face"/> index. </param>
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
        public void GetFace_Int(FvMesh mesh, int existingIndex, int nonExistentIndex, int[][] facesEdgesIndices)
        {
            // Arrange
            int[] faceEdgesIndices = facesEdgesIndices[existingIndex];
            FvMesh.Edge[] edges = new FvMesh.Edge[faceEdgesIndices.Length];
            for (int i = 0; i < faceEdgesIndices.Length; i++)
            {
                int index = Math.Abs(faceEdgesIndices[i]);
                edges[i] = mesh.GetEdge(index);
            }

            bool throwsException = false;

            // Act
            FvMesh.Face existingFace = mesh.GetFace(existingIndex);
            IReadOnlyList<FvMesh.Edge> faceEdges = existingFace.FaceEdges();

            FvMesh.Face? nonExistentFace = null;
            try { nonExistentFace = mesh.GetFace(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(edges[i_E], faceEdges[i_E]); }

            Assert.True(throwsException);
            Assert.True(nonExistentFace is null);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.TryGetFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="FvMesh.Face"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="FvMesh.Face"/> index. </param>
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
        public void TryGetFace_Int(FvMesh mesh, int existingIndex, int nonExistentIndex, int[][] facesEdgesIndices)
        {
            // Arrange
            int[] faceEdgesIndices = facesEdgesIndices[existingIndex];
            FvMesh.Edge[] edges = new FvMesh.Edge[faceEdgesIndices.Length];
            for (int i = 0; i < faceEdgesIndices.Length; i++)
            {
                int index = Math.Abs(faceEdgesIndices[i]);
                edges[i] = mesh.GetEdge(index);
            }

            bool throwsException = false;

            // Act
            FvMesh.Face? existingFace = mesh.TryGetFace(existingIndex);
            IReadOnlyList<FvMesh.Edge> faceEdges = existingFace!.FaceEdges();

            FvMesh.Face? nonExistentFace = null;
            try { nonExistentFace = mesh.TryGetFace(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(edges[i_E], faceEdges[i_E]); }

            Assert.False(throwsException);
            Assert.True(nonExistentFace is null);
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.GetFaces()"/>.
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
        public void GetFaces(FvMesh mesh, int[][] facesEdgesIndices)
        {
            // Arrange

            // Act & Assert
            foreach (FvMesh.Face face in mesh.GetFaces())
            {
                int[] faceEdgesIndices = facesEdgesIndices[face.Index];

                IReadOnlyList<FvMesh.Edge> faceEdges = face.FaceEdges();
                for (int i = 0; i < faceEdges.Count; i++)
                {
                    Assert.Equal(Math.Abs(faceEdgesIndices[i]), faceEdges[i].Index);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="FvMesh.RemoveFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="faceIndex"> Index of the <see cref="FvMesh.Face"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the face is removed.</param>
        /// <param name="edgeCount"> Number of edges after the face is removed.</param>
        /// <param name="faceCount"> Number of faces after the face is removed.</param>
        [Theory(DisplayName = "RemoveFace(int)")]
        [ClassData(typeof(ClassDatas.FaceRemoval))]
        public void RemoveFace_Int(FvMesh mesh, int faceIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange

            // Act
            mesh.RemoveFace(faceIndex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.RemoveFace(FvMesh.Face)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="faceIndex"> Index of the <see cref="FvMesh.Face"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the face is removed.</param>
        /// <param name="edgeCount"> Number of edges after the face is removed.</param>
        /// <param name="faceCount"> Number of faces after the face is removed.</param>
        [Theory(DisplayName = "RemoveFace(int)")]
        [ClassData(typeof(ClassDatas.FaceRemoval))]
        public void RemoveFace_Face(FvMesh mesh, int faceIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange
            FvMesh.Face face = mesh.GetFace(faceIndex);

            // Act
            mesh.RemoveFace(face);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }



        #endregion

        #region Tests : Internal Methods

        //     -----     About Edges     -----     //

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddEdge(int, int, EmptyTraits)"/>.
        /// </summary>
        [Theory(DisplayName = "AddEdge(int, int, EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddEdge_Int_Int_EdgeTraits(FvMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            FvMesh.Vertex vertex0 = mesh.AddVertex(vertexTraits);
            FvMesh.Vertex vertex1 = mesh.AddVertex(vertexTraits);

            // Act
            EmptyTraits edgeTraits = new EmptyTraits();
            FvMesh.Edge edge = mesh.AddEdge(0, 1, edgeTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, edge.Index);
            Assert.Equal(vertex0, edge.Start);
            Assert.Equal(vertex1, edge.End);
        }

        /// <summary>
        /// Tests the method <see cref="FvMesh.AddEdge(FvMesh.Vertex, FvMesh.Vertex, EmptyTraits)"/>.
        /// </summary>
        [Theory(DisplayName = "AddEdge(Vertex, Vertex, EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddEdge_Vertex_Vertex_EdgeTraits(FvMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            FvMesh.Vertex vertex0 = mesh.AddVertex(vertexTraits);
            FvMesh.Vertex vertex1 = mesh.AddVertex(vertexTraits);

            // Act
            EmptyTraits edgeTraits = new EmptyTraits();
            FvMesh.Edge edge = mesh.AddEdge(vertex0, vertex1, edgeTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, edge.Index);
            Assert.Equal(vertex0, edge.Start);
            Assert.Equal(vertex1, edge.End);
        }

        #endregion


        #region Storage Classes for Data Classes

        internal static class DataStorages
        {
            /// <summary>
            /// Computes and stores general data related to general <see cref="FvMesh"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class Empty
            {
                #region Static Fields

                /// <summary>
                /// Halfedge mesh data-structure with the topology of a cube.
                /// </summary>
                private static readonly FvMesh _empty = EmptyMesh();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a mesh that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Staticly stored <see cref="FvMesh"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ReadableMesh()
                {
                    return [_empty];
                }

                /// <summary>
                /// Provides a new mesh that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Newly computed <see cref="FvMesh"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] WritableMesh()
                {
                    return [EmptyMesh()];
                }


                //     -----     Properties

                /// <summary>
                /// /// <summary>
                /// Provides the expected number of vertices in the mesh.
                /// </summary>
                /// <returns> An single element array containing the vertex count. </returns>
                public static object[] VertexCount() => [0];

                /// <summary>
                /// /// <summary>
                /// Provides the expected number of halfedges in the mesh.
                /// </summary>
                /// <returns> An single element array containing the halfedge count. </returns>
                public static object[] HalfedgeCount() => [0];

                /// <summary>
                /// Provides the expected number of edges in the mesh.
                /// </summary>
                /// <returns> An single element array containing the edge count. </returns>
                public static object[] EdgeCount() => [0];

                /// <summary>
                /// Provides the expected number of faces in the mesh.
                /// </summary>
                /// <returns> An single element array containing the face count. </returns>
                public static object[] FaceCount() => [0];

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates an empty face-vertex mesh data structures.
                /// </summary>
                /// <returns> The <see cref="FvMesh"/> representing a topological cube. </returns>
                private static FvMesh EmptyMesh()
                {
                    return new FvMesh();
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores data related to a topological cube <see cref="FvMesh"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class Cube
            {
                #region Static Fields

                /// <summary>
                /// Face-vertex mesh data-structure with the topology of a cube.
                /// </summary>
                private static readonly FvMesh _cube = TopologicalCube();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a mesh that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Staticly stored <see cref="FvMesh"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ReadableMesh()
                {
                    return [_cube];
                }

                /// <summary>
                /// Provides a new mesh that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Newly computed <see cref="FvMesh"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] WritableMesh()
                {
                    return [TopologicalCube()];
                }


                //     -----     Properties

                /// <summary>
                /// /// <summary>
                /// Provides the expected number of vertices in the mesh.
                /// </summary>
                /// <returns> An single element array containing the vertex count. </returns>
                public static object[] VertexCount() => [8];

                /// <summary>
                /// Provides the expected number of edges in the mesh.
                /// </summary>
                /// <returns> An single element array containing the edge count. </returns>
                public static object[] EdgeCount() => [12];

                /// <summary>
                /// Provides the expected number of faces in the mesh.
                /// </summary>
                /// <returns> An single element array containing the face count. </returns>
                public static object[] FaceCount() => [6];


                //     -----     Methods

                /// <summary>
                /// Provides vertex indices.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Existing vertex index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Non-existent vertex index. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] VertexExistence() => [6, 12];

                /// <summary>
                /// Provides informations for a removed vertex.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the vertex to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The number of vertices. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The number of edge. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> The number of face. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] VertexRemoval() => [4, 7, 9, 3];


                /// <summary>
                /// Provides edge's ends vertex indices.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Indices of the end <see cref="Mesh.Vertex"/> for each <see cref="Mesh.Edge"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] EdgesEndIndices()
                {
                    return [new int[,] { { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 }, { 0, 4 }, { 4, 5 }, { 5, 1 }, { 5, 6 }, { 6, 2 }, { 6, 7 }, { 7, 3 }, { 7, 4 } }];
                }

                /// <summary>
                /// Provides edge indices.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Existing <see cref="FvMesh.Edge"/> index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Non-existent <see cref="FvMesh.Edge"/> index. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] EdgeIndices() => [5, 15];

                /// <summary>
                /// Provides edge end vertex indices.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Existing <see cref="FvMesh.Edge"/> index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Existing <see cref="FvMesh.Edge"/> end <see cref="FvMesh.Vertex"/> indices. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Non-existent <see cref="FvMesh.Edge"/> end <see cref="FvMesh.Vertex"/> indices. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] EdgeEndIndices() => [10, new int[] { 7, 3 }, new int[] { 0, 6 }];

                /// <summary>
                /// Provides informations for a removed edge.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the edge to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The number of vertices. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The number of edge. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> The number of face. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] EdgeRemoval() => [5, 8, 11, 4];


                /// <summary>
                /// Provides face's edge indices.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Indices of the <see cref="Mesh.Edge"/> for each <see cref="Mesh.Face"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] FacesEdgesIndices()
                {
                    return [new int[6][] { [0, 1, 2, 3], [-0, 4, 5, 6], [-1, -6, 7, 8], [-2, -8, 9, 10], [-3, -10, 11, -4], [-9, -7, -5, -11] }];
                }

                /// <summary>
                /// Provides face indices.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Existing <see cref="FvMesh.Face"/> index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Non-existent <see cref="FvMesh.Face"/> index. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] FaceIndices() => [4, 6];

                /// <summary>
                /// Provides informations for a removed face.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the face to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The number of vertices. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The number of edge. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> The number of face. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RemoveFace() => [0, 8, 12, 5];

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a topological cube.
                /// </summary>
                /// <returns> The <see cref="FvMesh"/> representing a topological cube. </returns>
                private static FvMesh TopologicalCube()
                {
                    FvMesh cube = new FvMesh();

                    //     -----     Add vertices

                    EmptyTraits vertexTrait = new EmptyTraits();

                    FvMesh.Vertex v0 = cube.AddVertex(vertexTrait);
                    FvMesh.Vertex v1 = cube.AddVertex(vertexTrait);
                    FvMesh.Vertex v2 = cube.AddVertex(vertexTrait);
                    FvMesh.Vertex v3 = cube.AddVertex(vertexTrait);
                    FvMesh.Vertex v4 = cube.AddVertex(vertexTrait);
                    FvMesh.Vertex v5 = cube.AddVertex(vertexTrait);
                    FvMesh.Vertex v6 = cube.AddVertex(vertexTrait);
                    FvMesh.Vertex v7 = cube.AddVertex(vertexTrait);

                    //     -----     Add faces

                    EmptyTraits faceTrait = new EmptyTraits();

                    cube.AddFace(v0, v1, v2, v3, faceTrait);     // Bottom

                    cube.AddFace(v1, v0, v4, v5, faceTrait);     // Front
                    cube.AddFace(v2, v1, v5, v6, faceTrait);     // Right
                    cube.AddFace(v3, v2, v6, v7, faceTrait);     // Back
                    cube.AddFace(v0, v3, v7, v4, faceTrait);     // Right

                    cube.AddFace(v7, v6, v5, v4, faceTrait);     // Top

                    return cube;
                }

                #endregion
            }
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
                { DataStorages.Empty.WritableMesh },
                    };
            }

            //     -----     Properties

            /// <summary>
            /// Class data for <see cref="FaceVertexMesh.Property_VertexCount(FvMesh, int)"/>.
            /// </summary>
            internal class VertexCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Empty.ReadableMesh, DataStorages.Empty.VertexCount },
                { DataStorages.Cube.ReadableMesh, DataStorages.Cube.VertexCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="FaceVertexMesh.Property_EdgeCount(FvMesh, int)"/>.
            /// </summary>
            internal class EdgeCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Empty.ReadableMesh, DataStorages.Empty.EdgeCount },
                { DataStorages.Cube.ReadableMesh, DataStorages.Cube.EdgeCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="FaceVertexMesh.Property_FaceCount(FvMesh, int)"/>.
            /// </summary>
            internal class FaceCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Empty.ReadableMesh, DataStorages.Empty.FaceCount },
                { DataStorages.Cube.ReadableMesh, DataStorages.Cube.FaceCount },
                    };
            }


            //     -----     Methods

            /// <summary>
            /// Class data for
            /// <see cref="FaceVertexMesh.GetVertex_Int__Existence(FvMesh, int, int)"/> and
            /// <see cref="FaceVertexMesh.TryGetVertex_Int__Existence(FvMesh, int, int)"/>.
            /// </summary>
            internal class VertexExistence : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.ReadableMesh, DataStorages.Cube.VertexExistence },
                    };
            }

            /// <summary>
            /// Class data for <see cref="FaceVertexMesh.RemoveVertex_Int(FvMesh, int, int, int, int)"/> and
            /// <see cref="FaceVertexMesh.RemoveVertex_Vertex(FvMesh, int, int, int, int)"/>.
            /// </summary>
            internal class VertexRemoval : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.WritableMesh, DataStorages.Cube.VertexRemoval },
                    };
            }


            /// <summary>
            /// Class data for 
            /// <see cref="FaceVertexMesh.GetEdge_Int(FvMesh, int, int, int[,])"/> and
            /// <see cref="FaceVertexMesh.TryGetEdge_Int(FvMesh, int, int, int[,])"/>.
            /// </summary>
            internal class EdgeIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.WritableMesh, DataStorages.Cube.EdgeIndices, DataStorages.Cube.EdgesEndIndices },
                    };
            }

            /// <summary>
            /// Class data for 
            /// <see cref="FaceVertexMesh.GetEdge_Int_Int(FvMesh, int, int[], int[])"/> and
            /// <see cref="FaceVertexMesh.GetEdge_Vertex_Vertex(FvMesh, int, int[], int[])"/> and
            /// <see cref="FaceVertexMesh.TryGetEdge_Int_Int(FvMesh, int, int[], int[])"/> and
            /// <see cref="FaceVertexMesh.TryGetEdge_Vertex_Vertex(FvMesh, int, int[], int[])"/>.
            /// </summary>
            internal class EdgeEndIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.ReadableMesh, DataStorages.Cube.EdgeEndIndices },
                    };
            }


            /// <summary>
            /// Class data for <see cref="FaceVertexMesh.GetEdges(FvMesh, int[,])"/>.
            /// </summary>
            internal class EdgesEndIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.ReadableMesh, DataStorages.Cube.EdgesEndIndices },
                    };
            }


            /// <summary>
            /// Class data for <see cref="FaceVertexMesh.RemoveEdge_Int(FvMesh, int, int, int, int)"/> and
            /// <see cref="FaceVertexMesh.RemoveEdge_Edge(FvMesh, int, int, int, int)"/>.
            /// </summary>
            internal class EdgeRemoval : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.WritableMesh, DataStorages.Cube.EdgeRemoval },
                    };
            }


            /// <summary>
            /// Class data for 
            /// <see cref="FaceVertexMesh.GetFace_Int(FvMesh, int, int, int[][])"/> and
            /// <see cref="FaceVertexMesh.TryGetFace_Int(FvMesh, int, int, int[][])"/>.
            /// </summary>
            internal class FaceIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.ReadableMesh, DataStorages.Cube.FaceIndices, DataStorages.Cube.FacesEdgesIndices },
                    };
            }

            /// <summary>
            /// Class data for <see cref="FaceVertexMesh.GetFaces(FvMesh, int[][])"/>.
            /// </summary>
            internal class GetFaces : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.ReadableMesh, DataStorages.Cube.FacesEdgesIndices },
                    };
            }

            /// <summary>
            /// Class data for <see cref="FaceVertexMesh.RemoveFace_Int(FvMesh, int, int, int, int)"/> and
            /// <see cref="FaceVertexMesh.RemoveFace_Face(FvMesh, int, int, int, int)"/>.
            /// </summary>
            internal class FaceRemoval : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                { DataStorages.Cube.WritableMesh, DataStorages.Cube.RemoveFace },
                    };
            }

        }

        #endregion
    }
}
