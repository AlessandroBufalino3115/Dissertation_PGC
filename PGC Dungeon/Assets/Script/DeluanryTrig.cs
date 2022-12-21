using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class DeluanryTrig : MonoBehaviour
{


    // CAN BE DELETED
    [SerializeField]
    public List<Triangle> triangulation = new List<Triangle>();

    public bool showPrim = false;

    [SerializeField]
    List<Vector3> pointList = new List<Vector3>();

    List<TriEdge> primEdges = new List<TriEdge>();

    private void Start()
    {

        pointList = new List<Vector3>();

        for (int i = 0; i < 50; i++)
        {

            Vector3 point = new Vector3(Random.Range(1, 100), Random.Range(1, 100),0);


            if (pointList.Contains(point))
            {
            }
            else { pointList.Add(point); }

        }

        /*

        triangulation = new List<Triangle>();

        Vector3 superTriangleA = new Vector3(10000, 10000,10000);
        Vector3 superTriangleB = new Vector3(10000, 0,10000);
        Vector3 superTriangleC = new Vector3(0, 10000,10000);

        triangulation.Add(new Triangle(superTriangleA, superTriangleB, superTriangleC));


        foreach (Vector3 point in pointList)
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
        PrimsAlgo();
        */

        triangulation = AlgosUtils.DelunayTriangulation2D(pointList);
        primEdges = AlgosUtils.PrimAlgo(pointList, triangulation);

        Debug.Log(primEdges.Count);
        Debug.Log(triangulation.Count);
    }



    /*
    public void PrimsAlgo() 
    {
        HashSet<Vector3> visitedVertices = new HashSet<Vector3>();

        var ran = Random.Range(0, pointList.Count);
        var vertex = pointList[ran];

        visitedVertices.Add(vertex);

        int iter = 0;

        while (visitedVertices.Count != pointList.Count) 
        {

            HashSet<Edge> edgesWithPoint = new HashSet<Edge>();

            foreach (var trig in triangulation)    // we get all the edges
            {
                foreach (var edge in trig.edges)
                {
                    foreach (var point in visitedVertices)
                    {
                        if (visitedVertices.Contains(edge.edge[0]) && visitedVertices.Contains(edge.edge[0])) 
                        {
                            // do nothing
                        }
                        else if (visitedVertices.Contains(edge.edge[0])) 
                        {
                            edgesWithPoint.Add(edge);
                        }
                        else if (visitedVertices.Contains(edge.edge[1])) 
                        {
                            edgesWithPoint.Add(edge);
                        }
                    }
                }
            }

            var edgesWithPointSort = edgesWithPoint.OrderBy(c => c.length).ToArray();   // we sort all the edges by the smallest to biggest

            visitedVertices.Add(edgesWithPointSort[0].edge[0]);
            visitedVertices.Add(edgesWithPointSort[0].edge[1]);
            primEdges.Add(edgesWithPointSort[0]);
        }
    }
    */

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var vertex in pointList)
        {
            Gizmos.DrawSphere(vertex, 0.3f);
        }

        if (!showPrim)
        {
            foreach (var triangle in triangulation)
            {
                foreach (var edge in triangle.edges)
                {
                    // Debug.Log($"{edge.edge[0][0]} and {edge.edge[0][1]}");
                    Debug.DrawLine(new Vector3(edge.edge[0].x, edge.edge[0].y, edge.edge[0].z), new Vector3(edge.edge[1].x, edge.edge[1].y, edge.edge[1].z), Color.green);
                }
            }
        }
        else
        {
            foreach (var edge in primEdges)
            {
                Debug.DrawLine(new Vector3(edge.edge[0].x, edge.edge[0].y, edge.edge[0].z), new Vector3(edge.edge[1].x, edge.edge[1].y, edge.edge[1].z), Color.green);
            }
        }
    }

    /*
    public class Triangle
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Edge[] edges = new Edge[3];

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;


            this.edges[0] = new Edge(a, b);
            this.edges[1] = new Edge(b, c);
            this.edges[2] = new Edge(c, a);
        }


        public bool HasVertex(Vector3 point)
        {
            if (a == point || b == point || c == point) { return true; }
            else { return false; }
        }

    }

    public class Edge
    {
        public Vector3[] edge = new Vector3[2];
        public float length;
        public Edge(Vector3 a, Vector3 b)
        {
            edge[0] = a;
            edge[1] = b;

            length = Mathf.Abs(Vector3.Distance(a, b));
        }

    }


    /// <summary>
    /// returns true if the point is in the circle
    /// </summary>
    public bool IspointInCircumcircle(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
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



    */
}
