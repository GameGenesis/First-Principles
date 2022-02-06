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

    private int hValue = (int)(Mathf.Pow(10, -8));

    //The type of function to be plotted
    public FunctionType functionType;

    public int baseN = 2;

    public bool differentiate = false;

    // Local points list
    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> dPoints = new List<Vector2>();

    private LineRendererUI lineRenderer;
    private DerivRendererUI derivRenderer;

    public GameObject derivativeLine;

    private void Reset()
    {
        PlotFunction(functionType);
    }

    private void OnValidate()
    {
        PlotFunction(functionType);
    }

    private void Update()
    {
        PlotFunction(functionType);
    }

    public void PlotFunction(FunctionType type)
    {
        lineRenderer = FindObjectOfType<LineRendererUI>();
        derivRenderer = FindObjectOfType<DerivRendererUI>();

        if (lineRenderer != null)
        {
            points.Clear();

            ComputeGraph(type, transA, transK, transC, transD, power, baseN);
            ComputeGraph(type, transA, transK, transC, transD, power, baseN);

            lineRenderer.points = points;
        }

        if (differentiate == true)
        {
            derivativeLine.SetActive(true);

            if (derivRenderer != null)
            {
                dPoints.Clear();
                derivRenderer.points = dPoints;
            }
        }

        else if (differentiate == false)
        {
            dPoints.Clear();
            derivativeLine.SetActive(false);
        }
    }

    public void ComputeGraph(FunctionType functionType, float transA, float transK, float transC, float transD, int power, int baseN)
    {
        Vector2Int gridOrigin = lineRenderer.gridSize / 2;

        for (float i = -50; i <= 50; i += step)
        {
            float xValue = i;
            float yValue = 0;

            float dyValue = 0;

            // Power of N Function
            if (functionType == FunctionType.Power)
            {
                yValue = transA * (float)(Mathf.Pow(transK * (xValue - transD), power) + transC);

                // Differentiate
                dyValue = ((transA * (float)(Mathf.Pow(transK * ((xValue + hValue) - transD), power) + transC)) - (transA * (float)(Mathf.Pow(transK * (xValue - transD), power) + transC))) / hValue;
            }
            // Absolute Value Function
            else if (functionType == FunctionType.Absolute)
            {
                yValue = transA * (float)(Mathf.Abs(transK * (xValue - transD)) + transC);

                // Differentiate
                dyValue = ((transA * (float)(Mathf.Abs(transK * ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Abs(transK * (xValue - transD)) + transC))) / hValue;
            }
            // Exponential Function
            else if (functionType == FunctionType.Exponential)
            {
                yValue = transA * (float)(Mathf.Pow(transK * baseN, (xValue - transD)) + transC);

                // Differentiate
                dyValue = ((transA * (float)(Mathf.Pow(transK * baseN, ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Pow(transK * baseN, (xValue - transD)) + transC))) / hValue;
            }
            // Square Root Function
            else if (functionType == FunctionType.SquareRoot)
            {
                yValue = transA * (float)(Mathf.Sqrt(transK * (xValue - transD)) + transC);

                // Differentiate
                dyValue = ((transA * (float)(Mathf.Sqrt(transK * ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Sqrt(transK * (xValue - transD)) + transC))) / hValue;
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
            points.Add(new Vector2(xValue + gridOrigin.x, yValue + gridOrigin.y));

            // Get the differentiated coordinates to another array
            dPoints.Add(new Vector2(xValue + gridOrigin.x, dyValue + gridOrigin.y));
        }
    }
}

public enum FunctionType
{
    Power, Absolute, Exponential, SquareRoot, Sine, Cosine
}