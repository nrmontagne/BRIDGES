using System;
using System.Collections.Generic;


namespace BRIDGES.DataStructures.PolyhedralMeshes
{
    /// <summary>
    /// Represents a generic mesh data-structure.
    /// </summary>
    /// <typeparam name="TVertexTraits"> Type defining the traits of the mesh vertices. </typeparam>
    /// <typeparam name="TEdgeTraits"> Type defining the traits of the mesh edges. </typeparam>
    /// <typeparam name="TFaceTraits"> Type defining the traits of the mesh faces. </typeparam>
    public abstract partial class Mesh<TVertexTraits, TEdgeTraits, TFaceTraits>
    {
        #region Abstract Properties

        /// <summary>
        /// Gets the number of vertices of this mesh.
        /// </summary>
        public abstract int VertexCount { get; }

        /// <summary>
        /// Gets the number of edges of this mesh.
        /// </summary>
        public abstract int EdgeCount { get; }

        /// <summary>
        /// Gets the number of faces of this mesh.
        /// </summary>
        public abstract int FaceCount { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Mesh{TVertexTraits, TEdgeTraits, TFaceTraits}"/> class.
        /// </summary>
        public Mesh()
        {
            // Nothing to do
        }

        #endregion

        #region Public Abstract Methods

        //     -----     About Vertices     -----     //

        /// <summary>
        /// Adds a vertex to this mesh.
        /// </summary>
        /// <param name="traits"> Traits for the vertex. </param>
        /// <returns> The new <see cref="Vertex"/>. </returns>
        public abstract Vertex AddVertex(TVertexTraits traits);


        /// <summary>
        /// Retrieves a vertex in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the vertex. </param>
        /// <returns> The <see cref="Vertex"/> at the given index. </returns>
        public abstract Vertex GetVertex(int index);

        /// <summary>
        /// Retrieves a vertex in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the vertex. </param>
        /// <returns> The <see cref="Vertex"/> at the given index, if it exists, <see langword="null"/> otherwise. </returns>
        public abstract Vertex? TryGetVertex(int index);

        /// <summary>
        /// Exposes an enumerator to iterate through the vertices of this mesh.
        /// </summary>
        /// <returns> An enumerable set of <see cref="Vertex"/>. </returns>
        public abstract IEnumerable<Vertex> GetVertices();


        /// <summary>
        /// Removes a vertex at a given index in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="index"> Index of the vertex. </param>
        public abstract void RemoveVertex(int index);


        //     -----     About Edges     -----     //

        /// <summary>
        /// Retrieves an edge in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the edge. </param>
        /// <returns> The <see cref="Edge"/> at the given index. </returns>
        public abstract Edge GetEdge(int index);

        /// <summary>
        /// Retrieves an edge in this mesh from the index of its end vertices.
        /// </summary>
        /// <param name="indexA"> Index of the first end vertex. </param>
        /// <param name="indexB"> Index of the second end vertex. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices. </returns>
        public abstract Edge GetEdge(int indexA, int indexB);


        /// <summary>
        /// Retrieves an edge in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the edge. </param>
        /// <returns> The <see cref="Edge"/> at the given index, if it exists, <see langword="null"/> otherwise. </returns>
        public abstract Edge? TryGetEdge(int index);

        /// <summary>
        /// Retrieves an edge in this mesh from the index of its end vertices.
        /// </summary>
        /// <param name="indexA"> Index of the first end vertex. </param>
        /// <param name="indexB"> Index of the second end vertex. </param>
        /// <returns> The <see cref="Edge"/> between the given vertices, if it exists, <see langword="null"/> otherwise. </returns>
        public abstract Edge? TryGetEdge(int indexA, int indexB);


        /// <summary>
        /// Exposes an enumerator to iterate through the edges of this mesh.
        /// </summary>
        /// <returns> An enumerable set of <see cref="Edge"/>. </returns>
        public abstract IEnumerable<Edge> GetEdges();


        /// <summary>
        /// Removes an edge at a given index in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="index"> Index of the edge. </param>
        public abstract void RemoveEdge(int index);


        //     -----     About Faces     -----     //

        /// <summary>
        /// Adds a triangular face to this mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first face vertex. </param>
        /// <param name="indexB"> Index of the second face vertex. </param>
        /// <param name="indexC"> Index of the third face vertex. </param>
        /// <param name="traits"> Traits for the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        public abstract Face AddFace(int indexA, int indexB, int indexC, TFaceTraits traits);

        /// <summary>
        /// Adds a quadrilateral face to this mesh.
        /// </summary>
        /// <param name="indexA"> Index of the first face vertex. </param>
        /// <param name="indexB"> Index of the second face vertex. </param>
        /// <param name="indexC"> Index of the third face vertex. </param>
        /// <param name="indexD"> Index of the fourth face vertex.. </param>
        /// <param name="traits"> Traits for the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        public abstract Face AddFace(int indexA, int indexB, int indexC, int indexD, TFaceTraits traits);

        /// <summary>
        /// Adds an face to this mesh.
        /// </summary>
        /// <param name="vertexIndices"> Ordered set of vertices of the face. </param>
        /// <param name="traits"> Traits for the face. </param>
        /// <returns> The new <see cref="Face"/>. </returns>
        public abstract Face AddFace(IReadOnlyList<int> vertexIndices, TFaceTraits traits);


        /// <summary>
        /// Retrieves a face in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the face. </param>
        /// <returns> The <see cref="Face"/> at the given index. </returns>
        public abstract Face GetFace(int index);

        /// <summary>
        /// Retrieves a face in this mesh from its index.
        /// </summary>
        /// <param name="index"> Index of the face. </param>
        /// <returns> The <see cref="Face"/> at the given index, if it exists, <see langword="null"/> otherwise. </returns>
        public abstract Face? TryGetFace(int index);

        /// <summary>
        /// Exposes an enumerator to iterate through the faces of this mesh.
        /// </summary>
        /// <returns> An enumerable set of <see cref="Face"/>. </returns>
        public abstract IEnumerable<Face> GetFaces();


        /// <summary>
        /// Removes a face at a given index in this mesh, by keeping the mesh manifold. 
        /// </summary>
        /// <param name="index"> Index of the face. </param>
        public abstract void RemoveFace(int index);

        #endregion

        #region Other Abstract Methods

        //     -----     About Vertices     -----     //

        /// <summary>
        /// Erases a vertex at a given index in the mesh.
        /// </summary>
        /// <remarks> Every reference to this vertex in the mesh should be deleted before it is erased. </remarks>
        /// <param name="index"> Index of the vertex. </param>
        protected abstract void EraseVertex(int index);


        //     -----     About Edges     -----     //

        /// <summary>
        /// Adds an edge to this mesh.
        /// </summary>
        /// <param name="startIndex"> Index of the start vertex. </param>
        /// <param name="endIndex"> Index of the end vertex. </param>
        /// <param name="traits"> Traits for the edge. </param>
        /// <returns> The new <see cref="Edge"/>. </returns>
        internal abstract Edge AddEdge(int startIndex, int endIndex, TEdgeTraits? traits = default);


        /// <summary>
        /// Erases an edge at a given index in the mesh.
        /// </summary>
        /// <remarks> Every reference to this edge in the mesh should be deleted before it is erased. </remarks>
        /// <param name="index"> Index of the edge. </param>
        protected abstract void EraseEdge(int index);


        //     -----     About Faces     -----     //

        /// <summary>
        /// Erases a face at a given index in the mesh.
        /// </summary>
        /// <remarks> Every reference to this face in the mesh should be deleted before it is erased. </remarks>
        /// <param name="index"> Index of the face. </param>
        protected abstract void EraseFace(int index);

        #endregion
    }
}
