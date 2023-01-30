using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RCR.Settings.Collections
{
    public class AdjacencyMatrix<T>
    {
    private int[,] _AdjacencyRepresentation;
    private T[,] Nodes;
    private List<MatrixEdge<T>> Edges;
    private int Width;
    private int Height;

    public AdjacencyMatrix(int width, int height)
    {
        _AdjacencyRepresentation = new int[width, height];
        Nodes = new T[width, height];
        Edges = new List<MatrixEdge<T>>();
        Width = width;
        Height = height;
        SortOutNullPaths();
    }

    private void SortOutNullPaths() //Flags all path's that do not make sense e.g path Node 0 TO Node 0.
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == y)
                    _AdjacencyRepresentation[x, y] = -1;
            }
        }
    }

    private bool ValidateIndex(int x, int y)
    {
        if ((x < 0 || y < 0 || x > Width - 1 || y > Height - 1))
        {
            Debug.LogWarning($"Index is out of bounds");
            return false;
        }

        return true;
    }

    private int ValidNodesCount()
    {
        int c = 0;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Nodes[x, y] != null)
                    c++;
            }
        }

        return c;
    }

    private bool ValidatePath(int x1, int y1, int x2, int y2)
    {
        if (x1 == y1 || x1 == x2 || x1 == y2 ||
            y1 == x2 || y1 == y2 ||
            x2 == y2)
        {
            Debug.LogWarning($"Can't Create a path to the same index Caution: could loop");
            return false;
        }

        return true;
    }

    private bool CheckIfValueExits(int x, int y)
    {
        if (!ValidateIndex(x, y))
            return false;
        if (Nodes[x, y] == null)
            return false;

        return true;
    }

    public virtual int GetLength(int dimension)
    {
        if (dimension > 1)
            return 0;

        return dimension == 0 ? Width : Height;

    }

    public virtual int GetLength() => Height * Width;

    public T CreateNode(T value, int x, int y)
    {
        if (!ValidateIndex(x, y))
            return default;

        Nodes[x, y] = value;
        return value;
    }

    public void DeleteNode(int x, int y)
    {
        if (!ValidateIndex(x, y))
            return;

        foreach (var edge in GetEdges(x, y))
            Edges.Remove(edge);

        foreach (var edge in Edges.Where(e =>
                     e.EdgeEndID.x == x && e.EdgeEndID.y == y))
        {
            Edges.Remove(edge);
        }

    }

    public void CreatePath(int x1, int y1, int x2, int y2)
    {
        if (!CheckIfValueExits(x1, y1) || !CheckIfValueExits(x2, y2) || !ValidatePath(x1, y1, x2, y2))
            return;

        Edges.Add(new MatrixEdge<T>(
            x1, y1, x2, y2, Nodes[x1, y1], Nodes[x2, y2]
        ));
    }
    
    /// <summary>
    /// Create a path from Index (x1,y1) to (x2,y2) if both Nodes succeed the predicate
    /// </summary>
    public void CreatePath(int x1, int y1, int x2, int y2, Func<T,T,bool> predicate)
    {
        if (!CheckIfValueExits(x1, y1) || !CheckIfValueExits(x2, y2) || !ValidatePath(x1, y1, x2, y2))
            return;
        if(!predicate(Nodes[x1,y1], Nodes[x2,y2]))
            return;

        Edges.Add(new MatrixEdge<T>(
            x1, y1, x2, y2, Nodes[x1, y1], Nodes[x2, y2]
        ));
    }

    public void DeletePath(int x1, int y1, int x2, int y2)
    {
        if (!CheckIfValueExits(x1, y1) || !CheckIfValueExits(x2, y2) || !ValidatePath(x1, y1, x2, y2))
            return;

        Edges.Remove(Edges.Find(e => e.EdgeStartID.x == x1
                                     && e.EdgeStartID.y == y1 && e.EdgeEndID.x == x2 && e.EdgeEndID.y == y2));
    }

    /// <summary>
    /// Get Collection of Edges where X,Y is the starting Point
    /// </summary>
    /// <returns>Collection Edges T</returns>
    public MatrixEdge<T>[] GetEdges(int x, int y)
    {
        return Edges.Where(e => e.EdgeStartID.x == x && e.EdgeStartID.y == y)
            .ToArray();
    }

    public T[] GetNodes()
    {
        List<T> Nodes = new List<T>();
        foreach (var node in this.Nodes)
        {
            Nodes.Add(node);
        }

        Nodes.RemoveAll(n => n == null);
        return Nodes.ToArray();
    }

    public T GetNode(int x, int y)
    {
        if (!ValidateIndex(x, y) || !CheckIfValueExits(x, y))
            return default;
        return Nodes[x, y];
    }
    
    /// <summary>
    ///  Check Node at X,Y Against all other based on a predicate
    /// </summary>
    /// <returns>True if all Nodes meet the condition, False if one Node Fails to meet condition</returns>
    public bool CheckOtherNodes(int x, int y, Func<T, T, bool> predicate) 
    {
        T main = GetNode(x, y);
        IEnumerable<T> ToCheck = GetNodes().Where(N => !N.Equals(main));

        foreach (var node in ToCheck)
        {
            if (!predicate(main, node))
                return false;
        }

        return true;
    }

    }
}