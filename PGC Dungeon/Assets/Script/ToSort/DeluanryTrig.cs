using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeluanryTrig : MonoBehaviour
{
    [SerializeField]
    public List<Triangle> triangulation = new List<Triangle>();

    private void Start()
    {

        List<Vector2> pointList = new List<Vector2>();



        for (int i = 0; i < 50; i++)
        {

            Vector2 point = new Vector2(Random.Range(1, 100), Random.Range(1, 100));


            if (pointList.Contains(point))
            {
            }
            else { pointList.Add(point); }

        }



        triangulation = new List<Triangle>();

        Vector2 superTriangleA = new Vector2(10000, 10000);
        Vector2 superTriangleB = new Vector2(10000, 0);
        Vector2 superTriangleC = new Vector2(0, 10000);

        triangulation.Add(new Triangle(superTriangleA, superTriangleB, superTriangleC));


        foreach (Vector2 point in pointList)
        {

            List<Triangle> badTriangles = new List<Triangle>();

            foreach (Triangle triangle in triangulation)
            {
                if (IspointInCircumcircle(triangle.a, triangle.b, triangle.c, point))
                {
                    badTriangles.Add(triangle);
                }
            }





            List<Edge> polygon = new List<Edge>();

            foreach (Triangle triangle in badTriangles)
            {
                foreach (Edge triangleEdge in triangle.edges)
                {
                    bool isShared = false;

                    foreach (Triangle otherTri in badTriangles)
                    {
                        if (otherTri == triangle) { continue; }

                        foreach (Edge otherEdge in otherTri.edges)
                        {
                            if (LineIsEqual(triangleEdge, otherEdge))
                            {
                                isShared = true;
                            }
                        }



                    }

                    if (isShared == false)
                    {
                        polygon.Add(triangleEdge);
                    }

                }
            }


            foreach (Triangle badTriangle in badTriangles)
            {
                triangulation.Remove(badTriangle);   // i think this is the issue here
            }

            foreach (Edge edge in polygon)
            {
                Triangle newTriangle = new Triangle(edge.edge[0], edge.edge[1], point);
                triangulation.Add(newTriangle);
            }

        }



        for (int i = triangulation.Count - 1; i >= 0; i--)
        {
            if (triangulation[i].HasVertex(superTriangleA) || triangulation[i].HasVertex(superTriangleB) || triangulation[i].HasVertex(superTriangleC))
            {
                triangulation.Remove(triangulation[i]);
            }

        }


    }


    public class Triangle
    {
        public Vector2 a;
        public Vector2 b;
        public Vector2 c;

        public Edge[] edges = new Edge[3];

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;


            this.edges[0] = new Edge(a, b);
            this.edges[1] = new Edge(b, c);
            this.edges[2] = new Edge(c, a);
        }


        public bool HasVertex(Vector2 point)
        {
            if (a == point || b == point || c == point) { return true; }
            else { return false; }
        }

    }

    public class Edge
    {
        public Vector2[] edge = new Vector2[2];

        public Edge(Vector2 a, Vector2 b)
        {
            edge[0] = a;
            edge[1] = b;
        }

    }


    /// <summary>
    /// returns true if the point is in the circle
    /// </summary>
    public bool IspointInCircumcircle(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {


        float ax_ = A[0] - D[0];
        float ay_ = A[1] - D[1];
        float bx_ = B[0] - D[0];
        float by_ = B[1] - D[1];
        float cx_ = C[0] - D[0];
        float cy_ = C[1] - D[1];



        if ((
            (ax_ * ax_ + ay_ * ay_) * (bx_ * cy_ - cx_ * by_) -
            (bx_ * bx_ + by_ * by_) * (ax_ * cy_ - cx_ * ay_) +
            (cx_ * cx_ + cy_ * cy_) * (ax_ * by_ - bx_ * ay_)
        ) < 0)
        {
            return true;
        }

        else { return false; }

    }

    public bool LineIsEqual(Edge A, Edge B)
    {
        if ((A.edge[0] == B.edge[0] && A.edge[1] == B.edge[1]) || (A.edge[0] == B.edge[1] && A.edge[1] == B.edge[0])) { return true; }
        else { return false; }
    }
}
