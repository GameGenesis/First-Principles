using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRendererUI : Graphic
{
    public Vector2Int gridSize;

    public List<Vector2> points = new List<Vector2>();

    //This clears the graph
    public bool bezierPlot = false;

    public float thickness = 10f;

    private float width;
    private float height;
    private float unitWidth;
    private float unitHeight;

    //NOTE: Naming scheme = lowercaseUppercase; can you change the name of this list. Also, the list "points" already exists
    public List<Vector2> mPoints;

    private GridRendererUI grid;

    //Updates the line grid based on the parent graph
    protected override void Reset()
    {
        base.Reset();

        if (grid == null)
            grid = GetComponentInParent<GridRendererUI>();

        if (grid != null && gridSize != grid.gridSize)
        {
            gridSize = grid.gridSize;
            SetVerticesDirty();
        }
    }

    //Updates the line grid based on the parent graph
    protected override void Awake()
    {
        base.Awake();

        if (grid == null)
            grid = GetComponentInParent<GridRendererUI>();
    }

    //Polymorphism; when the class receives the values for the centrePoint...
    public LineRendererUI(Vector2 centrePoint)
    {
        mPoints = new List<Vector2>
        {
            centrePoint + Vector2.left,
            centrePoint + (Vector2.left + Vector2.up) * .5f,
            centrePoint + (Vector2.right + Vector2.down) * .5f,
            centrePoint + Vector2.right
        };
    }

    //When a UI generates a mesh...
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / (float)gridSize.x;
        unitHeight = height / (float)gridSize.y;

        if (points.Count < 2)
            return;

        float angle = 0;

        if (bezierPlot == false)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 point = points[i];
                Vector2 point2 = points[i + 1];

                if (i < points.Count - 1)
                    angle = GetAngle(point, point2) + 90f;

                DrawVerticesForPoint(point, point2, vh, angle);
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                int index = i * 4;
                vh.AddTriangle(index + 0, index + 1, index + 2);
                vh.AddTriangle(index + 1, index + 2, index + 3);
            }
        }
    }

    //Converts the dimensions of the graph into an aspect ratio
    public Vector2 GetAspectRatio(float width, float height)
    {
        Vector2 aspectRatio = new Vector2(width, height);

        while (!floatIsInt(aspectRatio.x) && !floatIsInt(aspectRatio.y))
        {
            aspectRatio *= 10;
        }

        aspectRatio /= GetGreatestCommonDivisor((int)aspectRatio.x, (int)aspectRatio.y);

        return aspectRatio;
    }

    //Checks if a float is an integer
    public bool floatIsInt(float f)
    {
        return Mathf.Approximately(f, Mathf.RoundToInt(f));
    }

    //Returns the greatest common divisor
    public int GetGreatestCommonDivisor(int first, int second)
    {
        while (first != 0 && second != 0)
        {
            if (first > second) first %= second;
            else second %= first;
        }

        return first == 0 ? second : first;
    }

    //Gets the angle from one point to the next
    public float GetAngle(Vector2 current, Vector2 target)
    {
        Vector2 aspectRatio = GetAspectRatio(width, height);
        return (float)(Mathf.Atan2(aspectRatio.y * (target.y - current.y), aspectRatio.x * (target.x - current.x)) * (180 / Mathf.PI));
    }

    //Draws vertices for each point on the graph
    private void DrawVerticesForPoint(Vector2 point, Vector2 point2, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point2.x, unitHeight * point2.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point2.x, unitHeight * point2.y);
        vh.AddVert(vertex);
    }

    // For the bezier curve...
    public Vector2 this [int i]
    {
        get
        {
            return mPoints[i];
        }
    }

    // For the bezier curve...
    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    // For the bezier curve...
    public int NumSegments
    {
        get
        {
            return (mPoints.Count - 4) / 3 + 1;
        }
    }

    // For the bezier curve...
    public void AddSegement(Vector2 anchorPos)
    {
        mPoints.Add(mPoints[mPoints.Count - 1] * 2 - mPoints[mPoints.Count - 2]);
        mPoints.Add((mPoints[mPoints.Count - 1] + anchorPos) * .5f);
        mPoints.Add(anchorPos);
    }

    // For the bezier curve...
    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3] };
    }

    // For the bezier curve...
    public void MovePoint(int i, Vector2 pos)
    {
        points[i] = pos;
    }

    //Updates the line grid based on the parent graph
    private void Update()
    {
        if (grid != null && gridSize != grid.gridSize)
        {
            gridSize = grid.gridSize;
            SetVerticesDirty();
        }
    }
}