using System;
using System.Collections.Generic;

using Xunit;

using He = BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMeshes;


namespace BRIDGES.Tests.DataStructures.PolyhedralMeshes.HalfedgeMeshes
{
    // Alias
    using HeMesh = He.HalfedgeMesh<EmptyTraits, EmptyTraits, EmptyTraits, EmptyTraits>;


    /// <summary>
    /// Tests the members of the <see cref="HeMesh"/> data structure.
    /// </summary>
    public class HalfedgeMesh
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="HeMesh.VertexCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of vertices. </param>
        [Theory(DisplayName = "Prop. VertexCount")]
        [ClassData(typeof(ClassDatas.VertexCount))]
        public void Property_VertexCount(HeMesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.VertexCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="HeMesh.HalfedgeCount"/>.
        /// </summary>
        [Theory(DisplayName = "Prop. HalfedgeCount")]
        [ClassData(typeof(ClassDatas.HalfedgeCount))]
        public void Property_HalfedgeCount(HeMesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.HalfedgeCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="HeMesh.EdgeCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of edges. </param>
        [Theory(DisplayName = "Prop. EdgeCount")]
        [ClassData(typeof(ClassDatas.EdgeCount))]
        public void Property_EdgeCount(HeMesh mesh, int expected)
        {
            // Arrange

            //Act
            int actual = mesh.EdgeCount;
            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="HeMesh.FaceCount"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="expected"> Expected number of faces. </param>
        [Theory(DisplayName = "Prop. FaceCount")]
        [ClassData(typeof(ClassDatas.FaceCount))]
        public void Property_FaceCount(HeMesh mesh, int expected)
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
        /// Tests the initialisation of the <see cref="HeMesh.FaceCount"/>.
        /// </summary>
        [Fact(DisplayName = "HeMesh.HeMesh()")]
        public void Constructor()
        {
            // Arrange
            HeMesh fvMesh = new HeMesh();

            //Act
            int vertexCount = fvMesh.VertexCount;
            int halfedgeCount = fvMesh.HalfedgeCount;
            int edgeCount = fvMesh.EdgeCount;
            int faceCount = fvMesh.FaceCount;

            // Assert
            Assert.Equal(0, vertexCount);
            Assert.Equal(0, halfedgeCount);
            Assert.Equal(0, edgeCount);
            Assert.Equal(0, faceCount);
        }

        #endregion

        #region Tests : Public Methods

        //     -----     About Vertices     -----     //

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddVertex(EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddVertex(VertexTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddVertex_VertexTraits(HeMesh mesh)
        {
            EmptyTraits traits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(1, mesh.VertexCount);
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.GetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Vertex"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="HeMesh.Vertex"/> index. </param>
        [Theory(DisplayName = "GetVertex(int) - Existence")]
        [ClassData(typeof(ClassDatas.VertexExistence))]
        public void GetVertex_Int__Existence(HeMesh mesh, int existingIndex, int nonExistentIndex)
        {
            bool throwsException = false;

            // Act
            HeMesh.Vertex existingVertex = mesh.GetVertex(existingIndex);

            HeMesh.Vertex? nonExistentVertex = null;
            try { nonExistentVertex = mesh.GetVertex(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingVertex!.Index);

            Assert.True(throwsException);
            Assert.True(nonExistentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.GetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "GetVertex(int) - Created")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void GetVertex_Int__Created(HeMesh mesh)
        {
            // Arrange
            EmptyTraits traits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(vertex, mesh.GetVertex(0));
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.TryGetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Vertex"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="HeMesh.Vertex"/> index. </param>
        [Theory(DisplayName = "TryGetVertex(int) - Existence")]
        [ClassData(typeof(ClassDatas.VertexExistence))]
        public void TryGetVertex_Int__Existence(HeMesh mesh, int existingIndex, int nonExistentIndex)
        {
            // Arrange
            bool throwsException = false;

            // Act
            HeMesh.Vertex? existingVertex = mesh.TryGetVertex(existingIndex);

            HeMesh.Vertex? nonExistentVertex = null;
            try { nonExistentVertex = mesh.TryGetVertex(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingVertex!.Index);

            Assert.False(throwsException);
            Assert.True(nonExistentVertex is null);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.TryGetVertex(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "TryGetVertex(int) - Created")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void TryGetVertex_Int__Created(HeMesh mesh)
        {
            // Arrange
            EmptyTraits traits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertex = mesh.AddVertex(traits);

            // Assert
            Assert.Equal(vertex, mesh.TryGetVertex(0));
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.GetVertices()"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "GetVertices()")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void GetVertices(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTrait = new EmptyTraits();

            HeMesh.Vertex[] vertices =
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
            foreach (HeMesh.Vertex vertex in mesh.GetVertices())
            {
                Assert.Equal(vertices[vertex.Index], vertex);
            }
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.RemoveVertex(int))"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="vertexIndex"> Index of the <see cref="HeMesh.Vertex"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the vertex is removed.</param>
        /// <param name="edgeCount"> Number of edges after the vertex is removed.</param>
        /// <param name="faceCount"> Number of faces after the vertex is removed.</param>
        [Theory(DisplayName = "RemoveVertex(int)")]
        [ClassData(typeof(ClassDatas.VertexRemoval))]
        public void RemoveVertex_Int(HeMesh mesh, int vertexIndex, int vertexCount, int edgeCount, int faceCount)
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
        /// Tests the method <see cref="HeMesh.RemoveVertex(int))"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="vertexIndex"> Index of the <see cref="HeMesh.Vertex"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the vertex is removed.</param>
        /// <param name="edgeCount"> Number of edges after the vertex is removed.</param>
        /// <param name="faceCount"> Number of faces after the vertex is removed.</param>
        [Theory(DisplayName = "RemoveVertex(Vertex)")]
        [ClassData(typeof(ClassDatas.VertexRemoval))]
        public void RemoveVertex_Vertex(HeMesh mesh, int vertexIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange
            HeMesh.Vertex vertex = mesh.GetVertex(vertexIndex);

            // Act
            mesh.RemoveVertex(vertex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }


        //     -----     About Halfedges     -----     //


        /// <summary>
        /// Tests the method <see cref="HeMesh.GetHalfedge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Halfedge"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="HeMesh.Halfedge"/> index. </param>
        /// <param name="halfedgesEndsIndices"> Indices of the halfedge end vertices. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the halfedge index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the start (0) and end (1) vertex. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "GetHalfedge(int)")]
        [ClassData(typeof(ClassDatas.HalfedgeIndices))]
        public void GetHalfedge_Int(HeMesh mesh, int existingIndex, int nonExistentIndex, int[,] halfedgesEndsIndices)
        {
            // Arrange
            HeMesh.Vertex existingStart = mesh.GetVertex(halfedgesEndsIndices[existingIndex, 0]);
            HeMesh.Vertex existingEnd = mesh.GetVertex(halfedgesEndsIndices[existingIndex, 1]);

            bool throwsException = false;

            // Act
            HeMesh.Halfedge existingHe = mesh.GetHalfedge(existingIndex);

            HeMesh.Halfedge? nonExistentHe = null;
            try { nonExistentHe = mesh.GetHalfedge(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingHe.Index);
            Assert.Equal(existingStart, existingHe.Start);
            Assert.Equal(existingEnd, existingHe.End);

            Assert.True(throwsException);
            Assert.True(nonExistentHe is null);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.GetHalfedge(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Halfedge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="HeMesh.Halfedge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="HeMesh.Halfedge"/> end vertex index. </param>
        [Theory(DisplayName = "GetHalfedge(int,int)")]
        [ClassData(typeof(ClassDatas.HalfedgeEndIndices))]
        public void GetHalfedge_Int_Int(HeMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            HeMesh.Vertex existingStart = mesh.GetVertex(existingEnds[0]);
            HeMesh.Vertex existingEnd = mesh.GetVertex(existingEnds[1]);

            bool throwsException = false;

            // Act
            HeMesh.Halfedge existingHe = mesh.GetHalfedge(existingEnds[0], existingEnds[1]);

            HeMesh.Halfedge? absentHe = null;
            try { absentHe = mesh.GetHalfedge(nonExistentEnds[0], nonExistentEnds[1]); }
            catch (ArgumentException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingHe.Index);
            Assert.Equal(existingStart, existingHe.Start);
            Assert.Equal(existingEnd, existingHe.End);

            Assert.True(throwsException);
            Assert.True(absentHe is null);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.GetHalfedge(HeMesh.Vertex, HeMesh.Vertex)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Halfedge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="HeMesh.Halfedge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="HeMesh.Halfedge"/> end vertex index. </param>
        [Theory(DisplayName = "GetHalfedge(Vertex,Vertex)")]
        [ClassData(typeof(ClassDatas.HalfedgeEndIndices))]
        public void GetHalfedge_Vertex_Vertex(HeMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            HeMesh.Vertex existingStart = mesh.GetVertex(existingEnds[0]);
            HeMesh.Vertex existingEnd = mesh.GetVertex(existingEnds[1]);

            bool throwsException = false;
            HeMesh.Vertex nonExistentStart = mesh.GetVertex(nonExistentEnds[0]);
            HeMesh.Vertex nonExistentEnd = mesh.GetVertex(nonExistentEnds[1]);
            // Act
            HeMesh.Halfedge existingHe = mesh.GetHalfedge(existingStart, existingEnd);

            HeMesh.Halfedge? absentHe = null;
            try { absentHe = mesh.GetHalfedge(nonExistentStart, nonExistentEnd); }
            catch (ArgumentException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingHe.Index);
            Assert.Equal(existingStart, existingHe.Start);
            Assert.Equal(existingEnd, existingHe.End);

            Assert.True(throwsException);
            Assert.True(absentHe is null);
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.TryGetHalfedge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Halfedge"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="HeMesh.Halfedge"/> index. </param>
        /// <param name="halfedgesEndsIndices"> Indices of the halfedge end vertices. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the halfedge index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the start (0) and end (1) vertex. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "TryGetHalfedge(int)")]
        [ClassData(typeof(ClassDatas.HalfedgeIndices))]
        public void TryGetHalfedge_Int(HeMesh mesh, int existingIndex, int nonExistentIndex, int[,] halfedgesEndsIndices)
        {
            // Arrange
            HeMesh.Vertex existingStart = mesh.GetVertex(halfedgesEndsIndices[existingIndex, 0]);
            HeMesh.Vertex existingEnd = mesh.GetVertex(halfedgesEndsIndices[existingIndex, 1]);

            bool throwsException = false;

            // Act
            HeMesh.Halfedge? existingHe = mesh.TryGetHalfedge(existingIndex);

            HeMesh.Halfedge? nonExistentHe = null;
            try { nonExistentHe = mesh.TryGetHalfedge(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingHe!.Index);
            Assert.Equal(existingStart, existingHe.Start);
            Assert.Equal(existingEnd, existingHe.End);

            Assert.False(throwsException);
            Assert.True(nonExistentHe is null);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.TryGetHalfedge(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Halfedge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="HeMesh.Halfedge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="HeMesh.Halfedge"/> end vertex index. </param>
        [Theory(DisplayName = "TryGetHalfedge(int,int)")]
        [ClassData(typeof(ClassDatas.HalfedgeEndIndices))]
        public void TryGetHalfedge_Int_Int(HeMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            HeMesh.Vertex existingStart = mesh.GetVertex(existingEnds[0]);
            HeMesh.Vertex existingEnd = mesh.GetVertex(existingEnds[1]);

            bool throwsException = false;

            // Act
            HeMesh.Halfedge? existingHe = mesh.TryGetHalfedge(existingEnds[0], existingEnds[1]);

            HeMesh.Halfedge? absentHe = null;
            try { absentHe = mesh.TryGetHalfedge(nonExistentEnds[0], nonExistentEnds[1]); }
            catch (ArgumentException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingHe!.Index);
            Assert.Equal(existingStart, existingHe.Start);
            Assert.Equal(existingEnd, existingHe.End);

            Assert.False(throwsException);
            Assert.True(absentHe is null);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.TryGetHalfedge(HeMesh.Vertex, HeMesh.Vertex)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Halfedge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="HeMesh.Halfedge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="HeMesh.Halfedge"/> end vertex index. </param>
        [Theory(DisplayName = "TryGetHalfedge(Vertex,Vertex)")]
        [ClassData(typeof(ClassDatas.HalfedgeEndIndices))]
        public void TryGetHalfedge_Vertex_Vertex(HeMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            HeMesh.Vertex existingStart = mesh.GetVertex(existingEnds[0]);
            HeMesh.Vertex existingEnd = mesh.GetVertex(existingEnds[1]);

            bool throwsException = false;
            HeMesh.Vertex nonExistentStart = mesh.GetVertex(nonExistentEnds[0]);
            HeMesh.Vertex nonExistentEnd = mesh.GetVertex(nonExistentEnds[1]);

            // Act
            HeMesh.Halfedge? existingHe = mesh.TryGetHalfedge(existingStart, existingEnd);

            HeMesh.Halfedge? nonExistentHe = null;
            try { nonExistentHe = mesh.TryGetHalfedge(nonExistentStart, nonExistentEnd); }
            catch (ArgumentException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingHe!.Index);
            Assert.Equal(existingStart, existingHe.Start);
            Assert.Equal(existingEnd, existingHe.End);

            Assert.False(throwsException);
            Assert.True(nonExistentHe is null);
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.GetHalfedges()"/>.
        /// </summary>        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="halfedgesEndsIndices"> Indices of the halfedge end vertices. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the halfedge index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the start (0) and end (1) vertex. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "GetHalfedges()")]
        [ClassData(typeof(ClassDatas.HalfedgesEndIndices))]
        public void GetHalfedges(HeMesh mesh, int[,] halfedgesEndsIndices)
        {
            // Arrange

            // Act & Assert
            foreach (HeMesh.Halfedge halfedge in mesh.GetHalfedges())
            {
                Assert.Equal(mesh.GetVertex(halfedgesEndsIndices[halfedge.Index, 0]), halfedge.Start);
                Assert.Equal(mesh.GetVertex(halfedgesEndsIndices[halfedge.Index, 1]), halfedge.End);
            }
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.GetHalfedges(HalfedgeEnumeration)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="halfedgesEndsIndices"> Indices of the halfedge end vertices. 
        /// <list type="bullet">
        ///     <item>
        ///         <description> The first (left) index correspond to the halfedge index. </description>
        ///     </item>
        ///     <item>
        ///         <description> The second (right) index corresponds to the start (0) and end (1) vertex. </description>
        ///     </item>
        /// </list>
        /// </param>
        [Theory(DisplayName = "GetHalfedges(HalfedgeEnumeration)")]
        [ClassData(typeof(ClassDatas.HalfedgesEndIndices))]
        public void GetHalfedges_HalfedgeEnumeration(HeMesh mesh, int[,] halfedgesEndsIndices)
        {
            // Arrange
            List<int> expectedAllHalfedge = new List<int>();
            List<int> expectedPairHalfedge = new List<int>();
            List<int> expectedOddHalfedge = new List<int>();
            for (int i = 0; i < mesh.HalfedgeCount / 2; i++)
            {
                int pairIndex = 2 * i, oddIndex = pairIndex + 1;

                expectedAllHalfedge.Add(pairIndex); expectedAllHalfedge.Add(oddIndex);
                expectedPairHalfedge.Add(pairIndex);
                expectedOddHalfedge.Add(oddIndex);
            }

            // Act & Assert

            foreach (HeMesh.Halfedge halfedge in mesh.GetHalfedges(He.HalfedgeEnumeration.All))
            {
                Assert.Equal(mesh.GetVertex(halfedgesEndsIndices[halfedge.Index, 0]), halfedge.Start);
                Assert.Equal(mesh.GetVertex(halfedgesEndsIndices[halfedge.Index, 1]), halfedge.End);

                expectedAllHalfedge.Remove(halfedge.Index);
            }
            Assert.Empty(expectedAllHalfedge);


            foreach (HeMesh.Halfedge halfedge in mesh.GetHalfedges(He.HalfedgeEnumeration.Pair))
            {
                int edgeIndex = halfedge.Index / 2;
                int parity = halfedge.Index - 2 * edgeIndex;

                Assert.Equal(0, parity); 
                Assert.Equal(mesh.GetVertex(halfedgesEndsIndices[halfedge.Index, 0]), halfedge.Start);
                Assert.Equal(mesh.GetVertex(halfedgesEndsIndices[halfedge.Index, 1]), halfedge.End);

                expectedPairHalfedge.Remove(halfedge.Index);
            }
            Assert.Empty(expectedPairHalfedge);

            foreach (HeMesh.Halfedge halfedge in mesh.GetHalfedges(He.HalfedgeEnumeration.Odd))
            {
                int edgeIndex = halfedge.Index / 2;
                int parity = halfedge.Index - 2 * edgeIndex;

                Assert.Equal(1, parity);
                Assert.Equal(mesh.GetVertex(halfedgesEndsIndices[halfedge.Index, 0]), halfedge.Start);
                Assert.Equal(mesh.GetVertex(halfedgesEndsIndices[halfedge.Index, 1]), halfedge.End);

                expectedOddHalfedge.Remove(halfedge.Index);
            }
            Assert.Empty(expectedOddHalfedge);
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.RemoveHalfedge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="halfedgeIndex"> Index of the <see cref="HeMesh.Halfedge"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the halfedge is removed.</param>
        /// <param name="halfedgeCount"> Number of halfedges after the halfedge is removed.</param>
        /// <param name="edgeCount"> Number of edges after the halfedge is removed.</param>
        /// <param name="faceCount"> Number of faces after the halfedge is removed.</param>
        [Theory(DisplayName = "RemoveHalfedge(int)")]
        [ClassData(typeof(ClassDatas.HalfedgeRemoval))]
        public void RemoveHalfedge_Int(HeMesh mesh, int halfedgeIndex, int vertexCount, int halfedgeCount, int edgeCount, int faceCount)
        {
            // Arrange

            // Act
            mesh.RemoveHalfedge(halfedgeIndex);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(halfedgeCount, mesh.HalfedgeCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.RemoveHalfedge(HeMesh.Halfedge)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="halfedgeIndex"> Index of the <see cref="HeMesh.Halfedge"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the halfedge is removed.</param>
        /// <param name="halfedgeCount"> Number of halfedges after the halfedge is removed.</param>
        /// <param name="edgeCount"> Number of edges after the halfedge is removed.</param>
        /// <param name="faceCount"> Number of faces after the halfedge is removed.</param>
        [Theory(DisplayName = "RemoveHalfedge(Halfedge)")]
        [ClassData(typeof(ClassDatas.HalfedgeRemoval))]
        public void RemoveHalfedge_Halfedge(HeMesh mesh, int halfedgeIndex, int vertexCount, int halfedgeCount, int edgeCount, int faceCount)
        {
            // Arrange
            HeMesh.Halfedge halfedge = mesh.GetHalfedge(halfedgeIndex);

            // Act
            mesh.RemoveHalfedge(halfedge);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(halfedgeCount, mesh.HalfedgeCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }


        //     -----     About Edges     -----     //

        /// <summary>
        /// Tests the method <see cref="HeMesh.GetEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Edge"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="HeMesh.Edge"/> index. </param>
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
        public void GetEdge_Int(HeMesh mesh, int existingIndex, int nonExistentIndex, int[,] edgesEndsIndices)
        {
            // Arrange
            bool throwsException = false;

            // Act
            HeMesh.Edge existingEdge = mesh.GetEdge(existingIndex);

            HeMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="HeMesh.GetEdge(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="HeMesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="HeMesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "GetEdge(int,int)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void GetEdge_Int_Int(HeMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            int[] directEdgeEnds = new int[2] { existingEnds[0], existingEnds[1] };
            int[] inverseEdgeEnds = new int[2] { existingEnds[1], existingEnds[0] };
            int[] nonExistentEdgeEnds = new int[2] { nonExistentEnds[0], nonExistentEnds[1] };

            bool throwsException = false;

            // Act
            HeMesh.Edge directEdge = mesh.GetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            HeMesh.Edge inverseEdge = mesh.GetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            HeMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="HeMesh.GetEdge(HeMesh.Vertex, HeMesh.Vertex)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="HeMesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="HeMesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "GetEdge(Vertex,Vertex)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void GetEdge_Vertex_Vertex(HeMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            HeMesh.Vertex[] directEdgeEnds = [mesh.GetVertex(existingEnds[0]), mesh.GetVertex(existingEnds[1])];
            HeMesh.Vertex[] inverseEdgeEnds = [mesh.GetVertex(existingEnds[1]), mesh.GetVertex(existingEnds[0])];
            HeMesh.Vertex[] nonExistentEdgeEnds = [mesh.GetVertex(nonExistentEnds[0]), mesh.GetVertex(nonExistentEnds[1])];

            bool throwsException = false;

            // Act

            HeMesh.Edge directEdge = mesh.GetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            HeMesh.Edge inverseEdge = mesh.GetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            HeMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="HeMesh.TryGetEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Edge"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="HeMesh.Edge"/> index. </param>
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
        public void TryGetEdge_Int(HeMesh mesh, int existingIndex, int nonExistentIndex, int[,] edgesEndsIndices)
        {
            // Arrange
            bool throwsException = false;

            // Act
            HeMesh.Edge? existingEdge = mesh.TryGetEdge(existingIndex);

            HeMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="HeMesh.TryGetEdge(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="HeMesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="HeMesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "TryGetEdge(int,int)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void TryGetEdge_Int_Int(HeMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            int[] directEdgeEnds = new int[2] { existingEnds[0], existingEnds[1] };
            int[] inverseEdgeEnds = new int[2] { existingEnds[1], existingEnds[0] };
            int[] nonExistentEdgeEnds = new int[2] { nonExistentEnds[0], nonExistentEnds[1] };

            bool throwsException = false;

            // Act
            HeMesh.Edge? directEdge = mesh.TryGetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            HeMesh.Edge? inverseEdge = mesh.TryGetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            HeMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="HeMesh.TryGetEdge(HeMesh.Vertex, HeMesh.Vertex)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Edge"/> index.</param>
        /// <param name="existingEnds"> Existing <see cref="HeMesh.Edge"/> end vertex index. </param>
        /// <param name="nonExistentEnds"> Non-existent <see cref="HeMesh.Edge"/> end vertex index. </param>
        [Theory(DisplayName = "TryGetEdge(Vertex,Vertex)")]
        [ClassData(typeof(ClassDatas.EdgeEndIndices))]
        public void TryGetEdge_Vertex_Vertex(HeMesh mesh, int existingIndex, int[] existingEnds, int[] nonExistentEnds)
        {
            // Arrange
            HeMesh.Vertex[] directEdgeEnds = [mesh.GetVertex(existingEnds[0]), mesh.GetVertex(existingEnds[1])];
            HeMesh.Vertex[] inverseEdgeEnds = [mesh.GetVertex(existingEnds[1]), mesh.GetVertex(existingEnds[0])];
            HeMesh.Vertex[] nonExistentEdgeEnds = [mesh.GetVertex(nonExistentEnds[0]), mesh.GetVertex(nonExistentEnds[1])];

            bool throwsException = false;

            // Act
            HeMesh.Edge? directEdge = mesh.TryGetEdge(directEdgeEnds[0], directEdgeEnds[1]);

            HeMesh.Edge? inverseEdge = mesh.TryGetEdge(inverseEdgeEnds[0], inverseEdgeEnds[1]);

            HeMesh.Edge? nonExistentEdge = null;
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
        /// Tests the method <see cref="HeMesh.GetEdges()"/>.
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
        public void GetEdges(HeMesh mesh, int[,] edgesEndsIndices)
        {
            // Arrange

            // Act & Assert
            foreach (HeMesh.Edge edge in mesh.GetEdges())
            {
                Assert.Equal(mesh.GetVertex(edgesEndsIndices[edge.Index, 0]), edge.Start);
                Assert.Equal(mesh.GetVertex(edgesEndsIndices[edge.Index, 1]), edge.End);
            }
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.RemoveEdge(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="edgeIndex"> Index of the <see cref="HeMesh.Edge"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the edge is removed.</param>
        /// <param name="edgeCount"> Number of edges after the edge is removed.</param>
        /// <param name="faceCount"> Number of faces after the edge is removed.</param>
        [Theory(DisplayName = "RemoveEdge(int)")]
        [ClassData(typeof(ClassDatas.EdgeRemoval))]
        public void RemoveEdge_Int(HeMesh mesh, int edgeIndex, int vertexCount, int edgeCount, int faceCount)
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
        /// Tests the method <see cref="HeMesh.RemoveEdge(HeMesh.Edge)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="edgeIndex"> Index of the <see cref="HeMesh.Edge"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the edge is removed.</param>
        /// <param name="edgeCount"> Number of edges after the edge is removed.</param>
        /// <param name="faceCount"> Number of faces after the edge is removed.</param>
        [Theory(DisplayName = "RemoveEdge(Vertex)")]
        [ClassData(typeof(ClassDatas.EdgeRemoval))]
        public void RemoveEdge_Edge(HeMesh mesh, int edgeIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange
            HeMesh.Edge edge = mesh.GetEdge(edgeIndex);

            // Act
            mesh.RemoveEdge(edge);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }


        //     -----     About Faces     -----     //

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddFace(int, int, int, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(int,int,int,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Int_Int_Int_FaceTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<HeMesh.Vertex> vertices = new List<HeMesh.Vertex>();
            for (int i = 0; i < 3; i++)
            {
                HeMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            HeMesh.Face face = mesh.AddFace(0, 1, 2, faceTraits);

            // Assert
            IReadOnlyList<HeMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<HeMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(3, mesh.VertexCount);
            Assert.Equal(3, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 3; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 3; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddFace(HeMesh.Vertex, HeMesh.Vertex, HeMesh.Vertex, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(Vertex,Vertex,Vertex,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Vertex_Vertex_Vertex_FaceTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<HeMesh.Vertex> vertices = new List<HeMesh.Vertex>();
            for (int i = 0; i < 3; i++)
            {
                HeMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            HeMesh.Face face = mesh.AddFace(vertices[0], vertices[1], vertices[2], faceTraits);

            // Assert
            IReadOnlyList<HeMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<HeMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(3, mesh.VertexCount);
            Assert.Equal(3, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 3; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 3; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddFace(int, int, int, int, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(int,int,int,int,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Int_Int_Int_Int_FaceTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<HeMesh.Vertex> vertices = new List<HeMesh.Vertex>();
            for (int i = 0; i < 4; i++)
            {
                HeMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            HeMesh.Face face = mesh.AddFace(0, 1, 2, 3, faceTraits);

            // Assert
            IReadOnlyList<HeMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<HeMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(4, mesh.VertexCount);
            Assert.Equal(4, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 4; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddFace(HeMesh.Vertex, HeMesh.Vertex, HeMesh.Vertex, HeMesh.Vertex, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(Vertex,Vertex,Vertex,Vertex,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_Vertex_Vertex_Vertex_Vertex_FaceTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<HeMesh.Vertex> vertices = new List<HeMesh.Vertex>();
            for (int i = 0; i < 4; i++)
            {
                HeMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
            }

            HeMesh.Face face = mesh.AddFace(vertices[0], vertices[1], vertices[2], vertices[3], faceTraits);

            // Assert
            IReadOnlyList<HeMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<HeMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(4, mesh.VertexCount);
            Assert.Equal(4, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < 4; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddFace(IReadOnlyList{int}, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(IReadOnlyList<int>,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_IReadOnlyListOfInt_FaceTraits(HeMesh mesh)
        {
            // Arrange
            int faceVertexCount = 5;
            int faceEdgeCount = faceVertexCount;

            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<int> verticesIndex = new List<int>(faceVertexCount);
            List<HeMesh.Vertex> vertices = new List<HeMesh.Vertex>(faceVertexCount);
            for (int i = 0; i < faceVertexCount; i++)
            {
                HeMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
                verticesIndex.Add(i);
            }

            HeMesh.Face face = mesh.AddFace(verticesIndex, faceTraits);

            // Assert
            IReadOnlyList<HeMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<HeMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(faceVertexCount, mesh.VertexCount);
            Assert.Equal(faceEdgeCount, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < faceVertexCount; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < faceEdgeCount; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddFace(IReadOnlyList{HeMesh.Vertex}, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddFace(IReadOnlyList<Vertex>,FaceTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_IReadOnlyListOfVertex_FaceTraits(HeMesh mesh)
        {
            // Arrange
            int faceVertexCount = 5;
            int faceEdgeCount = faceVertexCount;

            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            List<int> verticesIndex = new List<int>(faceVertexCount);
            List<HeMesh.Vertex> vertices = new List<HeMesh.Vertex>(faceVertexCount);
            for (int i = 0; i < faceVertexCount; i++)
            {
                HeMesh.Vertex vertex = mesh.AddVertex(vertexTraits);
                vertices.Add(vertex);
                verticesIndex.Add(i);
            }

            HeMesh.Face face = mesh.AddFace(vertices, faceTraits);

            // Assert
            IReadOnlyList<HeMesh.Vertex> faceVertices = face.FaceVertices();
            IReadOnlyList<HeMesh.Edge> faceEdges = face.FaceEdges();

            Assert.Equal(faceVertexCount, mesh.VertexCount);
            Assert.Equal(faceEdgeCount, mesh.EdgeCount);
            Assert.Equal(1, mesh.FaceCount);

            Assert.Equal(0, face.Index);
            for (int i_V = 0; i_V < faceVertexCount; i_V++) { Assert.Equal(vertices[i_V], faceVertices[i_V]); }
            for (int i_E = 0; i_E < faceEdgeCount; i_E++) { Assert.Equal(mesh.GetEdge(i_E), faceEdges[i_E]); }
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddFace(IReadOnlyList{HeMesh.Vertex}, EmptyTraits)"/>.
        /// </summary>
        [Theory(DisplayName = "AddFace(List<Vertex>,FaceTraits) - Non Manifold A")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddFace_IReadOnlyListOfVertex_FaceTraits__NonManifold_A(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits faceTraits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertex0 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex1 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex2 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex3 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex4 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex5 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex6 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex7 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex8 = mesh.AddVertex(vertexTraits);

            List<HeMesh.Vertex> faceVertices_A = new List<HeMesh.Vertex>() { vertex0, vertex1, vertex2 };
            List<HeMesh.Vertex> faceVertices_B = new List<HeMesh.Vertex>() { vertex0, vertex3, vertex4 };
            List<HeMesh.Vertex> faceVertices_C = new List<HeMesh.Vertex>() { vertex0, vertex7, vertex8 };
            List<HeMesh.Vertex> faceVertices_D = new List<HeMesh.Vertex>() { vertex0, vertex5, vertex6 };

            List<HeMesh.Vertex> faceVertices_E = new List<HeMesh.Vertex>() { vertex0, vertex6, vertex7 };
            List<HeMesh.Vertex> faceVertices_F = new List<HeMesh.Vertex>() { vertex0, vertex8, vertex1 };
            List<HeMesh.Vertex> faceVertices_G = new List<HeMesh.Vertex>() { vertex0, vertex4, vertex5 };
            List<HeMesh.Vertex> faceVertices_H = new List<HeMesh.Vertex>() { vertex0, vertex2, vertex3 };

            HeMesh.Face face_A = mesh.AddFace(faceVertices_A, faceTraits);
            HeMesh.Face face_B = mesh.AddFace(faceVertices_B, faceTraits);
            HeMesh.Face face_C = mesh.AddFace(faceVertices_C, faceTraits);
            HeMesh.Face face_D = mesh.AddFace(faceVertices_D, faceTraits);

            HeMesh.Face face_E = mesh.AddFace(faceVertices_E, faceTraits);
            HeMesh.Face face_F = mesh.AddFace(faceVertices_F, faceTraits);
            HeMesh.Face face_G = mesh.AddFace(faceVertices_G, faceTraits);
            HeMesh.Face face_H = mesh.AddFace(faceVertices_H, faceTraits);

            // Assert
            Assert.Equal(9, mesh.VertexCount);
            Assert.Equal(32, mesh.HalfedgeCount);
            Assert.Equal(16, mesh.EdgeCount);
            Assert.Equal(8, mesh.FaceCount);
        }



        /// <summary>
        /// Tests the method <see cref="HeMesh.GetFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Face"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="HeMesh.Face"/> index. </param>
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
        public void GetFace_Int(HeMesh mesh, int existingIndex, int nonExistentIndex, int[][] facesEdgesIndices)
        {
            // Arrange
            int[] faceEdgesIndices = facesEdgesIndices[existingIndex];
            HeMesh.Edge[] edges = new HeMesh.Edge[faceEdgesIndices.Length];
            for (int i = 0; i < faceEdgesIndices.Length; i++)
            {
                int index = Math.Abs(faceEdgesIndices[i]);
                edges[i] = mesh.GetEdge(index);
            }

            bool throwsException = false;

            // Act
            HeMesh.Face existingFace = mesh.GetFace(existingIndex);
            IReadOnlyList<HeMesh.Edge> faceEdges = existingFace.FaceEdges();

            HeMesh.Face? nonExistentFace = null;
            try { nonExistentFace = mesh.GetFace(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(edges[i_E], faceEdges[i_E]); }

            Assert.True(throwsException);
            Assert.True(nonExistentFace is null);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.TryGetFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to evaluate. </param>
        /// <param name="existingIndex"> Existing <see cref="HeMesh.Face"/> index. </param>
        /// <param name="nonExistentIndex"> Non-existent <see cref="HeMesh.Face"/> index. </param>
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
        public void TryGetFace_Int(HeMesh mesh, int existingIndex, int nonExistentIndex, int[][] facesEdgesIndices)
        {
            // Arrange
            int[] faceEdgesIndices = facesEdgesIndices[existingIndex];
            HeMesh.Edge[] edges = new HeMesh.Edge[faceEdgesIndices.Length];
            for (int i = 0; i < faceEdgesIndices.Length; i++)
            {
                int index = Math.Abs(faceEdgesIndices[i]);
                edges[i] = mesh.GetEdge(index);
            }

            bool throwsException = false;

            // Act
            HeMesh.Face? existingFace = mesh.TryGetFace(existingIndex);
            IReadOnlyList<HeMesh.Edge> faceEdges = existingFace!.FaceEdges();

            HeMesh.Face? nonExistentFace = null;
            try { nonExistentFace = mesh.TryGetFace(nonExistentIndex); }
            catch (KeyNotFoundException) { throwsException = true; }

            // Assert
            Assert.Equal(existingIndex, existingFace.Index);
            for (int i_E = 0; i_E < 4; i_E++) { Assert.Equal(edges[i_E], faceEdges[i_E]); }

            Assert.False(throwsException);
            Assert.True(nonExistentFace is null);
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.GetFaces()"/>.
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
        public void GetFaces(HeMesh mesh, int[][] facesEdgesIndices)
        {
            // Arrange

            // Act & Assert
            foreach (HeMesh.Face face in mesh.GetFaces())
            {
                int[] faceEdgesIndices = facesEdgesIndices[face.Index];

                IReadOnlyList<HeMesh.Edge> faceEdges = face.FaceEdges();
                for (int i = 0; i < faceEdges.Count; i++)
                {
                    Assert.Equal(Math.Abs(faceEdgesIndices[i]), faceEdges[i].Index);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="HeMesh.RemoveFace(int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="faceIndex"> Index of the <see cref="HeMesh.Face"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the face is removed.</param>
        /// <param name="edgeCount"> Number of edges after the face is removed.</param>
        /// <param name="faceCount"> Number of faces after the face is removed.</param>
        [Theory(DisplayName = "RemoveFace(int)")]
        [ClassData(typeof(ClassDatas.FaceRemoval))]
        public void RemoveFace_Int(HeMesh mesh, int faceIndex, int vertexCount, int edgeCount, int faceCount)
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
        /// Tests the method <see cref="HeMesh.RemoveFace(HeMesh.Face)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        /// <param name="faceIndex"> Index of the <see cref="HeMesh.Face"/> to remove. </param>
        /// <param name="vertexCount"> Number of vertices after the face is removed.</param>
        /// <param name="edgeCount"> Number of edges after the face is removed.</param>
        /// <param name="faceCount"> Number of faces after the face is removed.</param>
        [Theory(DisplayName = "RemoveFace(int)")]
        [ClassData(typeof(ClassDatas.FaceRemoval))]
        public void RemoveFace_Face(HeMesh mesh, int faceIndex, int vertexCount, int edgeCount, int faceCount)
        {
            // Arrange
            HeMesh.Face face = mesh.GetFace(faceIndex);

            // Act
            mesh.RemoveFace(face);

            // Assert
            Assert.Equal(vertexCount, mesh.VertexCount);
            Assert.Equal(edgeCount, mesh.EdgeCount);
            Assert.Equal(faceCount, mesh.FaceCount);
        }

        #endregion

        #region Tests : Internal Methods

        //     -----     About Halfedges     -----     //

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddHalfedgePair(int, int)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddHalfedgePair(int, int)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddHalfedgePair_Int_Int(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertexA = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertexB = mesh.AddVertex(vertexTraits);

            HeMesh.Halfedge[] pairOfhalfedges = mesh.AddHalfedgePair(0, 1);

            // Assert

            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(2, mesh.HalfedgeCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, pairOfhalfedges[0].Index);
            Assert.Equal(vertexA, pairOfhalfedges[0].Start);
            Assert.Equal(vertexB, pairOfhalfedges[0].End);
            Assert.Equal(pairOfhalfedges[1], pairOfhalfedges[0].Pair);
            Assert.Null(pairOfhalfedges[0]._correspondingEdge);

            Assert.Equal(1, pairOfhalfedges[1].Index);
            Assert.Equal(vertexB, pairOfhalfedges[1].Start);
            Assert.Equal(vertexA, pairOfhalfedges[1].End);
            Assert.Equal(pairOfhalfedges[0], pairOfhalfedges[1].Pair);
            Assert.Null(pairOfhalfedges[1]._correspondingEdge);

            // Also tests the live creation of the corresping edge

            HeMesh.Edge edge = mesh.GetEdge(0);
            Assert.Equal(vertexA, edge.Start);
            Assert.Equal(vertexB, edge.End);
            Assert.Equal(pairOfhalfedges[0], edge.CorrespondingHalfedge);

            Assert.Equal(edge, pairOfhalfedges[0]._correspondingEdge);
            Assert.Equal(edge, pairOfhalfedges[1]._correspondingEdge);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddHalfedgePair(HeMesh.Vertex, HeMesh.Vertex)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddHalfedgePair(Vertex, Vertex)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddHalfedgePair_Vertex_Vertex(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertexA = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertexB = mesh.AddVertex(vertexTraits);

            HeMesh.Halfedge[] pairOfhalfedges = mesh.AddHalfedgePair(vertexA, vertexB);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(2, mesh.HalfedgeCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, pairOfhalfedges[0].Index);
            Assert.Equal(vertexA, pairOfhalfedges[0].Start);
            Assert.Equal(vertexB, pairOfhalfedges[0].End);
            Assert.Equal(pairOfhalfedges[1], pairOfhalfedges[0].Pair);
            Assert.Null(pairOfhalfedges[0]._correspondingEdge);

            Assert.Equal(1, pairOfhalfedges[1].Index);
            Assert.Equal(vertexB, pairOfhalfedges[1].Start);
            Assert.Equal(vertexA, pairOfhalfedges[1].End);
            Assert.Equal(pairOfhalfedges[0], pairOfhalfedges[1].Pair);
            Assert.Null(pairOfhalfedges[1]._correspondingEdge);

            // Also tests the live creation of the corresping edge

            HeMesh.Edge edge = mesh.GetEdge(0);
            Assert.Equal(vertexA, edge.Start);
            Assert.Equal(vertexB, edge.End);
            Assert.Equal(pairOfhalfedges[0], edge.CorrespondingHalfedge);

            Assert.Equal(edge, pairOfhalfedges[0]._correspondingEdge);
            Assert.Equal(edge, pairOfhalfedges[1]._correspondingEdge);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddHalfedgePair(int, int, EmptyTraits[])"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddHalfedgePair(int, int, HalfedgeTraits[])")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddHalfedgePair_Int_Int_ArrayOfHalfedgeTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits[] halfedgesTraits = new EmptyTraits[2] { default, default };

            // Act
            HeMesh.Vertex vertexA = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertexB = mesh.AddVertex(vertexTraits);

            HeMesh.Halfedge[] pairOfhalfedges = mesh.AddHalfedgePair(0, 1, halfedgesTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(2, mesh.HalfedgeCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, pairOfhalfedges[0].Index);
            Assert.Equal(vertexA, pairOfhalfedges[0].Start);
            Assert.Equal(vertexB, pairOfhalfedges[0].End);
            Assert.Equal(pairOfhalfedges[1], pairOfhalfedges[0].Pair);
            Assert.Null(pairOfhalfedges[0]._correspondingEdge);

            Assert.Equal(1, pairOfhalfedges[1].Index);
            Assert.Equal(vertexB, pairOfhalfedges[1].Start);
            Assert.Equal(vertexA, pairOfhalfedges[1].End);
            Assert.Equal(pairOfhalfedges[0], pairOfhalfedges[1].Pair);
            Assert.Null(pairOfhalfedges[1]._correspondingEdge);

            // Also tests the live creation of the corresping edge

            HeMesh.Edge edge = mesh.GetEdge(0);
            Assert.Equal(vertexA, edge.Start);
            Assert.Equal(vertexB, edge.End);
            Assert.Equal(pairOfhalfedges[0], edge.CorrespondingHalfedge);

            Assert.Equal(edge, pairOfhalfedges[0]._correspondingEdge);
            Assert.Equal(edge, pairOfhalfedges[1]._correspondingEdge);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddHalfedgePair(HeMesh.Vertex, HeMesh.Vertex, EmptyTraits[])"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddHalfedgePair(Vertex, Vertex, HalfedgeTraits[])")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddHalfedgePair_Vertex_Vertex_ArrayOfHalfedgeTraits(HeMesh mesh)
        {

            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits[] halfedgesTraits = new EmptyTraits[2] { default, default };

            // Act
            HeMesh.Vertex vertexA = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertexB = mesh.AddVertex(vertexTraits);

            HeMesh.Halfedge[] pairOfhalfedges = mesh.AddHalfedgePair(vertexA, vertexB, halfedgesTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(2, mesh.HalfedgeCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, pairOfhalfedges[0].Index);
            Assert.Equal(vertexA, pairOfhalfedges[0].Start);
            Assert.Equal(vertexB, pairOfhalfedges[0].End);
            Assert.Equal(pairOfhalfedges[1], pairOfhalfedges[0].Pair);
            Assert.Null(pairOfhalfedges[0]._correspondingEdge);

            Assert.Equal(1, pairOfhalfedges[1].Index);
            Assert.Equal(vertexB, pairOfhalfedges[1].Start);
            Assert.Equal(vertexA, pairOfhalfedges[1].End);
            Assert.Equal(pairOfhalfedges[0], pairOfhalfedges[1].Pair);
            Assert.Null(pairOfhalfedges[1]._correspondingEdge);

            // Also tests the live creation of the corresping edge

            HeMesh.Edge edge = mesh.GetEdge(0);
            Assert.Equal(vertexA, edge.Start);
            Assert.Equal(vertexB, edge.End);
            Assert.Equal(pairOfhalfedges[0], edge.CorrespondingHalfedge);

            Assert.Equal(edge, pairOfhalfedges[0]._correspondingEdge);
            Assert.Equal(edge, pairOfhalfedges[1]._correspondingEdge);

        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddHalfedgePair(int, int, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddHalfedgePair(int, int, EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddHalfedgePair_Int_Int_EdgeTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits edgeTraits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertexA = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertexB = mesh.AddVertex(vertexTraits);

            HeMesh.Halfedge[] pairOfhalfedges = mesh.AddHalfedgePair(0, 1, edgeTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(2, mesh.HalfedgeCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, pairOfhalfedges[0].Index);
            Assert.Equal(vertexA, pairOfhalfedges[0].Start);
            Assert.Equal(vertexB, pairOfhalfedges[0].End);
            Assert.Equal(pairOfhalfedges[1], pairOfhalfedges[0].Pair);
            Assert.NotNull(pairOfhalfedges[0]._correspondingEdge);

            Assert.Equal(1, pairOfhalfedges[1].Index);
            Assert.Equal(vertexB, pairOfhalfedges[1].Start);
            Assert.Equal(vertexA, pairOfhalfedges[1].End);
            Assert.Equal(pairOfhalfedges[0], pairOfhalfedges[1].Pair);
            Assert.NotNull(pairOfhalfedges[1]._correspondingEdge);

            HeMesh.Edge edge = mesh.GetEdge(0);
            Assert.Equal(vertexA, edge.Start);
            Assert.Equal(vertexB, edge.End);
            Assert.Equal(pairOfhalfedges[0], edge.CorrespondingHalfedge);

            Assert.Equal(edge, pairOfhalfedges[0]._correspondingEdge);
            Assert.Equal(edge, pairOfhalfedges[1]._correspondingEdge);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddHalfedgePair(HeMesh.Vertex, HeMesh.Vertex, EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddHalfedgePair(Vertex, Vertex, EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddHalfedgePair_Vertex_Vertex_EdgeTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits edgeTraits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertexA = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertexB = mesh.AddVertex(vertexTraits);

            HeMesh.Halfedge[] pairOfhalfedges = mesh.AddHalfedgePair(vertexA, vertexB, edgeTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(2, mesh.HalfedgeCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, pairOfhalfedges[0].Index);
            Assert.Equal(vertexA, pairOfhalfedges[0].Start);
            Assert.Equal(vertexB, pairOfhalfedges[0].End);
            Assert.Equal(pairOfhalfedges[1], pairOfhalfedges[0].Pair);
            Assert.NotNull(pairOfhalfedges[0]._correspondingEdge);

            Assert.Equal(1, pairOfhalfedges[1].Index);
            Assert.Equal(vertexB, pairOfhalfedges[1].Start);
            Assert.Equal(vertexA, pairOfhalfedges[1].End);
            Assert.Equal(pairOfhalfedges[0], pairOfhalfedges[1].Pair);
            Assert.NotNull(pairOfhalfedges[1]._correspondingEdge);

            HeMesh.Edge edge = mesh.GetEdge(0);
            Assert.Equal(vertexA, edge.Start);
            Assert.Equal(vertexB, edge.End);
            Assert.Equal(pairOfhalfedges[0], edge.CorrespondingHalfedge);

            Assert.Equal(edge, pairOfhalfedges[0]._correspondingEdge);
            Assert.Equal(edge, pairOfhalfedges[1]._correspondingEdge);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddHalfedgePair(int, int, EmptyTraits[], EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddHalfedgePair(int, int, HalfedgeTraits[], EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddHalfedgePair_Int_Int_ArrayOfHalfedgeTraits_EdgeTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits[] halfedgesTraits = new EmptyTraits[2] { default, default };
            EmptyTraits edgeTraits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertexA = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertexB = mesh.AddVertex(vertexTraits);

            HeMesh.Halfedge[] pairOfhalfedges = mesh.AddHalfedgePair(0, 1, halfedgesTraits, edgeTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(2, mesh.HalfedgeCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, pairOfhalfedges[0].Index);
            Assert.Equal(vertexA, pairOfhalfedges[0].Start);
            Assert.Equal(vertexB, pairOfhalfedges[0].End);
            Assert.Equal(pairOfhalfedges[1], pairOfhalfedges[0].Pair);
            Assert.NotNull(pairOfhalfedges[0]._correspondingEdge);

            Assert.Equal(1, pairOfhalfedges[1].Index);
            Assert.Equal(vertexB, pairOfhalfedges[1].Start);
            Assert.Equal(vertexA, pairOfhalfedges[1].End);
            Assert.Equal(pairOfhalfedges[0], pairOfhalfedges[1].Pair);
            Assert.NotNull(pairOfhalfedges[1]._correspondingEdge);

            HeMesh.Edge edge = mesh.GetEdge(0);
            Assert.Equal(vertexA, edge.Start);
            Assert.Equal(vertexB, edge.End);
            Assert.Equal(pairOfhalfedges[0], edge.CorrespondingHalfedge);

            Assert.Equal(edge, pairOfhalfedges[0]._correspondingEdge);
            Assert.Equal(edge, pairOfhalfedges[1]._correspondingEdge);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddHalfedgePair(HeMesh.Vertex, HeMesh.Vertex, EmptyTraits[], EmptyTraits)"/>.
        /// </summary>
        /// <param name="mesh"> Mesh to operate on. </param>
        [Theory(DisplayName = "AddHalfedgePair(Vertex, Vertex, HalfedgeTraits[], EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddHalfedgePair_Vertex_Vertex_ArrayOfHalfedgeTraits_EdgeTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            EmptyTraits[] halfedgesTraits = new EmptyTraits[2] { default, default };
            EmptyTraits edgeTraits = new EmptyTraits();

            // Act
            HeMesh.Vertex vertexA = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertexB = mesh.AddVertex(vertexTraits);

            HeMesh.Halfedge[] pairOfhalfedges = mesh.AddHalfedgePair(vertexA, vertexB, halfedgesTraits, edgeTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(2, mesh.HalfedgeCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, pairOfhalfedges[0].Index);
            Assert.Equal(vertexA, pairOfhalfedges[0].Start);
            Assert.Equal(vertexB, pairOfhalfedges[0].End);
            Assert.Equal(pairOfhalfedges[1], pairOfhalfedges[0].Pair);
            Assert.NotNull(pairOfhalfedges[0]._correspondingEdge);

            Assert.Equal(1, pairOfhalfedges[1].Index);
            Assert.Equal(vertexB, pairOfhalfedges[1].Start);
            Assert.Equal(vertexA, pairOfhalfedges[1].End);
            Assert.Equal(pairOfhalfedges[0], pairOfhalfedges[1].Pair);
            Assert.NotNull(pairOfhalfedges[1]._correspondingEdge);

            HeMesh.Edge edge = mesh.GetEdge(0);
            Assert.Equal(vertexA, edge.Start);
            Assert.Equal(vertexB, edge.End);
            Assert.Equal(pairOfhalfedges[0], edge.CorrespondingHalfedge);

            Assert.Equal(edge, pairOfhalfedges[0]._correspondingEdge);
            Assert.Equal(edge, pairOfhalfedges[1]._correspondingEdge);
        }


        //     -----     About Edges     -----     //

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddEdge(int, int, EmptyTraits)"/>.
        /// </summary>
        [Theory(DisplayName = "AddEdge(int, int, EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddEdge_Int_Int_EdgeTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            HeMesh.Vertex vertex0 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex1 = mesh.AddVertex(vertexTraits);

            // Act
            EmptyTraits edgeTraits = new EmptyTraits();
            HeMesh.Edge edge = mesh.AddEdge(0, 1, edgeTraits);

            // Assert
            Assert.Equal(2, mesh.VertexCount);
            Assert.Equal(1, mesh.EdgeCount);
            Assert.Equal(0, mesh.FaceCount);

            Assert.Equal(0, edge.Index);
            Assert.Equal(vertex0, edge.Start);
            Assert.Equal(vertex1, edge.End);
        }

        /// <summary>
        /// Tests the method <see cref="HeMesh.AddEdge(HeMesh.Vertex, HeMesh.Vertex, EmptyTraits)"/>.
        /// </summary>
        [Theory(DisplayName = "AddEdge(Vertex, Vertex, EdgeTraits)")]
        [ClassData(typeof(ClassDatas.EmptyMesh))]
        public void AddEdge_Vertex_Vertex_EdgeTraits(HeMesh mesh)
        {
            // Arrange
            EmptyTraits vertexTraits = new EmptyTraits();
            HeMesh.Vertex vertex0 = mesh.AddVertex(vertexTraits);
            HeMesh.Vertex vertex1 = mesh.AddVertex(vertexTraits);

            // Act
            EmptyTraits edgeTraits = new EmptyTraits();
            HeMesh.Edge edge = mesh.AddEdge(vertex0, vertex1, edgeTraits);

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
            /// Computes and stores general data related to general <see cref="HeMesh"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class Empty
            {
                #region Static Fields

                /// <summary>
                /// Halfedge mesh data-structure with the topology of a cube.
                /// </summary>
                private static readonly HeMesh _empty = EmptyMesh();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a mesh that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Staticly stored <see cref="HeMesh"/>. </description>
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
                ///         <description> Newly computed <see cref="HeMesh"/>. </description>
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
                /// Creates an empty halfedge mesh data structures.
                /// </summary>
                /// <returns> The <see cref="HeMesh"/> representing a topological cube. </returns>
                private static HeMesh EmptyMesh()
                {
                    return new HeMesh();
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores data related to a topological cube <see cref="HeMesh"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class Cube
            {
                #region Static Fields

                /// <summary>
                /// Halfedge mesh data-structure with the topology of a cube.
                /// </summary>
                private static readonly HeMesh _cube = TopologicalCube();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a mesh that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Staticly stored <see cref="HeMesh"/>. </description>
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
                ///         <description> Newly computed <see cref="HeMesh"/>. </description>
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
                /// /// <summary>
                /// Provides the expected number of halfedges in the mesh.
                /// </summary>
                /// <returns> An single element array containing the halfedge count. </returns>
                public static object[] HalfedgeCount() => [24];

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
                public static object[] HalfedgesEndIndices()
                {
                    return [new int[,] { { 0, 1 }, { 1, 0 }, { 1, 2 }, { 2, 1 }, { 2, 3 }, { 3, 2 }, { 3, 0 }, { 0, 3 }, { 0, 4 }, { 4, 0 }, { 4, 5 }, { 5, 4 }, { 5, 1 }, { 1, 5 }, { 5, 6 }, { 6, 5 }, { 6, 2 }, { 2, 6 }, { 6, 7 }, { 7, 6 }, { 7, 3 }, { 3, 7 }, { 7, 4 }, { 4, 7 } }];
                }

                /// <summary>
                /// Provides halfedge indices.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Existing <see cref="HeMesh.Halfedge"/> index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Non-existent <see cref="HeMesh.Halfedge"/> index. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] HalfedgeIndices() => [8, 32];

                /// <summary>
                /// Provides halfedge end vertex indices.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Existing <see cref="HeMesh.Halfedge"/> index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Existing <see cref="HeMesh.Halfedge"/> end <see cref="HeMesh.Vertex"/> indices. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Non-existent <see cref="HeMesh.Halfedge"/> end <see cref="HeMesh.Vertex"/> indices. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] HalfedgeEndIndices() => [16, new int[2] { 6, 2 }, new int[2] { 6, 3 }];

                /// <summary>
                /// Provides informations for a removed halfedge.
                /// </summary>
                /// <returns> A <see cref="List{T}"/> of <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the halfedge to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The number of vertices after removal. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The number of halfedges after removal. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> The number of edge after removal. </description>
                ///     </item>
                ///     <item>
                ///         <term> 4 </term>
                ///         <description> The number of face after removal. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] HalfedgeRemoval() => [17, 8, 22, 11, 4];


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
                ///         <description> Existing <see cref="HeMesh.Edge"/> index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Non-existent <see cref="HeMesh.Edge"/> index. </description>
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
                ///         <description> Existing <see cref="HeMesh.Edge"/> index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Existing <see cref="HeMesh.Edge"/> end <see cref="HeMesh.Vertex"/> indices. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Non-existent <see cref="HeMesh.Edge"/> end <see cref="HeMesh.Vertex"/> indices. </description>
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
                ///         <description> Existing <see cref="HeMesh.Face"/> index. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Non-existent <see cref="HeMesh.Face"/> index. </description>
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
                /// <returns> The <see cref="HeMesh"/> representing a topological cube. </returns>
                private static HeMesh TopologicalCube()
                {
                    HeMesh cube = new HeMesh();

                    //     -----     Add vertices

                    EmptyTraits vertexTrait = new EmptyTraits();

                    HeMesh.Vertex v0 = cube.AddVertex(vertexTrait);
                    HeMesh.Vertex v1 = cube.AddVertex(vertexTrait);
                    HeMesh.Vertex v2 = cube.AddVertex(vertexTrait);
                    HeMesh.Vertex v3 = cube.AddVertex(vertexTrait);
                    HeMesh.Vertex v4 = cube.AddVertex(vertexTrait);
                    HeMesh.Vertex v5 = cube.AddVertex(vertexTrait);
                    HeMesh.Vertex v6 = cube.AddVertex(vertexTrait);
                    HeMesh.Vertex v7 = cube.AddVertex(vertexTrait);


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
                        { DataStorages.Empty.WritableMesh},
                    };
            }

            //     -----     Properties

            /// <summary>
            /// Class data for <see cref="HalfedgeMesh.Property_VertexCount(HeMesh, int)"/>.
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
            /// Class data for <see cref="HalfedgeMesh.Property_HalfedgeCount(HeMesh, int)"/>.
            /// </summary>
            internal class HalfedgeCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.Empty.ReadableMesh, DataStorages.Empty.HalfedgeCount },
                        { DataStorages.Cube.ReadableMesh, DataStorages.Cube.HalfedgeCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="HalfedgeMesh.Property_EdgeCount(HeMesh, int)"/>.
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
            /// Class data for <see cref="HalfedgeMesh.Property_FaceCount(HeMesh, int)"/>.
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
            /// <see cref="HalfedgeMesh.GetVertex_Int__Existence(HeMesh, int, int)"/> and
            /// <see cref="HalfedgeMesh.TryGetVertex_Int__Existence(HeMesh, int, int)"/>.
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
            /// Class data for <see cref="HalfedgeMesh.RemoveVertex_Int(HeMesh, int, int, int, int)"/> and
            /// <see cref="HalfedgeMesh.RemoveVertex_Vertex(HeMesh, int, int, int, int)"/>.
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
            /// <see cref="HalfedgeMesh.GetHalfedge_Int(HeMesh, int, int, int[,])"/> and
            /// <see cref="HalfedgeMesh.TryGetHalfedge_Int(HeMesh, int, int, int[,])"/>.
            /// </summary>
            internal class HalfedgeIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.Cube.WritableMesh, DataStorages.Cube.HalfedgeIndices, DataStorages.Cube.HalfedgesEndIndices },
                    };
            }

            /// <summary>
            /// Class data for 
            /// <see cref="HalfedgeMesh.GetHalfedge_Int_Int(HeMesh, int, int[], int[])"/> and
            /// <see cref="HalfedgeMesh.GetHalfedge_Vertex_Vertex(HeMesh, int, int[], int[])"/> and
            /// <see cref="HalfedgeMesh.TryGetHalfedge_Int_Int(HeMesh, int, int[], int[])"/> and
            /// <see cref="HalfedgeMesh.TryGetHalfedge_Vertex_Vertex(HeMesh, int, int[], int[])"/>.
            /// </summary>
            internal class HalfedgeEndIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.Cube.ReadableMesh, DataStorages.Cube.HalfedgeEndIndices },
                    };
            }

            /// <summary>
            /// Class data for <see cref="HalfedgeMesh.GetHalfedges(HeMesh, int[,])"/> and
            /// <see cref="HalfedgeMesh.GetHalfedges_HalfedgeEnumeration(HeMesh, int[,])"/>.
            /// </summary>
            internal class HalfedgesEndIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.Cube.ReadableMesh, DataStorages.Cube.HalfedgesEndIndices },
                    };
            }


            /// <summary>
            /// Class data for <see cref="HalfedgeMesh.RemoveHalfedge_Int(HeMesh, int, int, int, int)"/> and
            /// <see cref="HalfedgeMesh.RemoveHalfedge_Halfedge(HeMesh, int, int, int, int)"/>.
            /// </summary>
            internal class HalfedgeRemoval : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.Cube.WritableMesh, DataStorages.Cube.HalfedgeRemoval },
                    };
            }


            /// <summary>
            /// Class data for 
            /// <see cref="HalfedgeMesh.GetEdge_Int(HeMesh, int, int, int[,])"/> and
            /// <see cref="HalfedgeMesh.TryGetEdge_Int(HeMesh, int, int, int[,])"/>.
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
            /// <see cref="HalfedgeMesh.GetEdge_Int_Int(HeMesh, int, int[], int[])"/> and
            /// <see cref="HalfedgeMesh.GetEdge_Vertex_Vertex(HeMesh, int, int[], int[])"/> and
            /// <see cref="HalfedgeMesh.TryGetEdge_Int_Int(HeMesh, int, int[], int[])"/> and
            /// <see cref="HalfedgeMesh.TryGetEdge_Vertex_Vertex(HeMesh, int, int[], int[])"/>.
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
            /// Class data for <see cref="HalfedgeMesh.GetEdges(HeMesh, int[,])"/>.
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
            /// Class data for <see cref="HalfedgeMesh.RemoveEdge_Int(HeMesh, int, int, int, int)"/> and
            /// <see cref="HalfedgeMesh.RemoveEdge_Edge(HeMesh, int, int, int, int)"/>.
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
            /// <see cref="HalfedgeMesh.GetFace_Int(HeMesh, int, int, int[][])"/> and
            /// <see cref="HalfedgeMesh.TryGetFace_Int(HeMesh, int, int, int[][])"/>.
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
            /// Class data for <see cref="HalfedgeMesh.GetFaces(HeMesh, int[][])"/>.
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
            /// Class data for <see cref="HalfedgeMesh.RemoveFace_Int(HeMesh, int, int, int, int)"/> and
            /// <see cref="HalfedgeMesh.RemoveFace_Face(HeMesh, int, int, int, int)"/>.
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
