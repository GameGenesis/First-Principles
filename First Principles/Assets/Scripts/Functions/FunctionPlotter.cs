using System.Collections.Generic;
using UnityEngine;

public class FunctionPlotter : MonoBehaviour
{
    //The starting x coordinate
    public float xStart = 0f;

    //The ending x coordinate
    public float xEnd = 10f;

    //Increase in xValue for each for loop iteration
    public float step = 0.5f;

    public float transA = 0, transK = 0, transC = 0, transD = 0;

    // Power variable only applicable to 'x^n' functions — represented as 'n' here
    // Base N variable only applicable to 'n^x' functions — represented as 'n' here

    public int power = 2;

    //The type of function to be plotted
    public FunctionType functionType;

    [HideInInspector]
    public int baseN = 2;

    // Local points list
    private List<Vector2> points = new List<Vector2>();

    private LineRendererUI lineRenderer;

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

        if (lineRenderer != null)
        {
            points.Clear();
            ComputeGraph(type, transA, transK, transC, transD, power, baseN);

            //lineRenderer.dPoints = dPoints;
            lineRenderer.points = points;
        }
    }

    public void ComputeGraph(FunctionType functionType, float transA, float transK, float transC, float transD, int power, int baseN)
    {
        for (float i = xStart; i <= xEnd; i += step)
        {
            float xValue = i;
            float yValue = 0;

            // Power of N Function
            if (functionType == FunctionType.Power)
            {
                yValue = transA * (float)(Mathf.Pow(transK * (xValue - transD), power) + transC);
            }
            // Absolute Value Function
            else if (functionType == FunctionType.Absolute)
            {
                yValue = transA * (float)(Mathf.Abs(transK * (xValue - transD)) + transC);
            }
            // Exponential Function
            else if (functionType == FunctionType.Exponential)
            {
                yValue = transA * (float)(Mathf.Pow(transK * baseN, (xValue - transD)) + transC);
            }
            // Square Root Function
            else if (functionType == FunctionType.SquareRoot)
            {
                yValue = transA * (float)(Mathf.Sqrt(transK * (xValue - transD)) + transC);
            }
            // Add the coordinates to the array
            points.Add(new Vector2(xValue, yValue));

            // Get the derivative and add to an array
            //dPoints.Add(GetDerivative());
        }
    }

    // Get the derivative of the graph
    private Vector2 GetDerivative()
    {
        return (new Vector2(0, 0));
    }
}

public enum FunctionType
{
    Power, Absolute, Exponential, SquareRoot
}