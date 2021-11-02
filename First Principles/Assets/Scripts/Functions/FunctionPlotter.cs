using System.Collections.Generic;
using UnityEngine;

public class FunctionPlotter : MonoBehaviour
{
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

    // Local 'points' list — should not affect the global 'points' list in the LindRendererUI class
    private List<Vector2> points = new List<Vector2>();

    private LineRendererUI lineRenderer;

    private void Awake()
    {
        
    }

    public void PlotFunction(FunctionType type)
    {
        lineRenderer = FindObjectOfType<LineRendererUI>();

        if (lineRenderer != null)
        {
            ComputeGraph(type, transA, transK, transC, transD, power, baseN);
            lineRenderer.points = points;
        }
    }

    // Get the values from LineRendererUI class
    public void ComputeGraph(FunctionType functionType, float transA, float transK, float transC, float transD, int power, int baseN)
    {
        for (float i = 0; i <= 50; i += step)
        {
            // Brute-force values from -1000 to 1000 into X
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
        }
    }
}

public enum FunctionType
{
    Power, Absolute, Exponential, SquareRoot
}