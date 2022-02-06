using System.Collections.Generic;
using UnityEngine;

public class FunctionPlotter : MonoBehaviour
{
    //The starting & ending x coordinates
    public float xStart = 0f;
    public float xEnd = 10f;

    //Increase in xValue for each for loop iteration
    public float step = 0.5f;

    public float transA = 0, transK = 0, transC = 0, transD = 0;

    // Power variable only applicable to 'x^n' functions — represented as 'n' here
    // Base N variable only applicable to 'n^x' functions — represented as 'n' here

    public int power = 2;

    private float hValue = (float)(Mathf.Pow(10, -4));

    //The type of function to be plotted
    public FunctionType functionType;

    public int baseN = 2;

    public bool differentiate = false;

    // Local points list
    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> dPoints = new List<Vector2>();

    private LineRendererUI lineRenderer;
    private DerivRendererUI derivRenderer;

    public GameObject lineSegment;
    public GameObject derivativeLine;

    private void Reset()
    {
        InitPlotFunction();
    }

    private void OnValidate()
    {
        InitPlotFunction();
    }

    private void Update()
    {
        InitPlotFunction();
    }

    public void InitPlotFunction()
    {
        FunctionType functionType;
        functionType = this.functionType;

        PlotFunction(functionType);
    }

    public void GraphRefresh()
    {
        lineSegment.SetActive(false);
        lineSegment.SetActive(true);
    }

    private void PlotFunction(FunctionType type)
    {
        lineRenderer = FindObjectOfType<LineRendererUI>();
        derivRenderer = FindObjectOfType<DerivRendererUI>();

        if (lineRenderer != null)
        {
            points.Clear();

            GraphRefresh();

            ComputeGraph(type, transA, transK, transC, transD, power, baseN);

            lineRenderer.points = points;
        }

        if (differentiate == true && lineRenderer != null)
        {
            derivativeLine.SetActive(false);
            derivativeLine.SetActive(true);

            if (derivRenderer != null)
            {

                dPoints.Clear();

                points.Clear();

                GraphRefresh();

                ComputeGraph(type, transA, transK, transC, transD, power, baseN);

                derivRenderer.points = dPoints;
            }
        }

        else if (differentiate == false)
        {
            dPoints.Clear();

            derivativeLine.SetActive(true);
            derivativeLine.SetActive(false);
        }
    }

    public void ComputeGraph(FunctionType functionType, float transA, float transK, float transC, float transD, int power, int baseN)
    {
        Vector2Int gridOrigin = lineRenderer.gridSize / 2;

        for (float i = xStart; i <= xEnd; i += step)
        {
            float xValue = i;
            float yValue = 0;

            float dyValue = 0;

            // Power of N Function
            if (functionType == FunctionType.Power)
            {
                yValue = transA * (float)(Mathf.Pow(transK * (xValue - transD), power) + transC);

                // Differentiate numerically using the centred three-point method
                dyValue = ((transA * (float)(Mathf.Pow(transK * ((xValue + hValue) - transD), power) + transC)) - (transA * (float)(Mathf.Pow(transK * ((xValue - hValue) - transD), power) + transC))) / (hValue * 2);
            }
            // Absolute Value Function
            else if (functionType == FunctionType.Absolute)
            {
                yValue = transA * (float)(Mathf.Abs(transK * (xValue - transD)) + transC);

                // Differentiate
                dyValue = ((transA * (float)(Mathf.Abs(transK * ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Abs(transK * ((xValue - hValue) - transD)) + transC))) / (hValue * 2);
            }
            // Exponential Function
            else if (functionType == FunctionType.Exponential)
            {
                yValue = transA * (float)(Mathf.Pow(transK * baseN, (xValue - transD)) + transC);

                // Differentiate
                dyValue = ((transA * (float)(Mathf.Pow(transK * baseN, ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Pow(transK * baseN, ((xValue - hValue) - transD)) + transC))) / (hValue * 2);
            }
            // Square Root Function
            else if (functionType == FunctionType.SquareRoot)
            {
                yValue = transA * (float)(Mathf.Sqrt(transK * (xValue - transD)) + transC);

                // Differentiate
                dyValue = ((transA * (float)(Mathf.Sqrt(transK * ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Sqrt(transK * ((xValue - hValue) - transD)) + transC))) / (hValue * 2);
            }

            else if (functionType == FunctionType.Sine)
            {
                yValue = transA * (float)(Mathf.Sin(transK * (xValue - transD)) + transC);

                dyValue = transA * (float)(Mathf.Cos(transK * (xValue - transD)) + transC);
            }
            else if (functionType == FunctionType.Cosine)
            {
                yValue = transA * (float)(Mathf.Cos(transK * (xValue - transD)) + transC);

                dyValue = transA * (float)(Mathf.Sin(transK * (xValue - transD)) + transC);
            }
            // Add the coordinates to the array
            this.points.Add(new Vector2(xValue + gridOrigin.x, yValue + gridOrigin.y));

            // Get the differentiated coordinates to another array
            this.dPoints.Add(new Vector2(xValue + gridOrigin.x, dyValue + gridOrigin.y));
        }
    }
}

public enum FunctionType
{
    Power, Absolute, Exponential, SquareRoot, Sine, Cosine
}