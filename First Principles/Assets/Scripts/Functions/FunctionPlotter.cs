/*
 * 
 * FunctionPlotter.cs Written by John Seong
 * An Open-Source Project
 * Main Features:
 * 1. Plot Functions
 * 2. Plot Their Corresponding First Derivatives
 * Enjoy :)
 * 
 */


/*

:: What is Differentation? ::

In mathematics, the derivative of a function of a real variable measures the sensitivity
to change of the function value with respect to a change in its argument.
Derivatives are a fundamental tool of calculus.

*/

using System.Collections.Generic;
using UnityEngine;

public class FunctionPlotter : MonoBehaviour
{
    // The starting & ending x coordinates
    public float xStart = 0f;
    public float xEnd = 10f;

    // Increase in xValue for each for loop iteration
    public float step = 0.5f;

    public float transA = 0, transK = 0, transC = 0, transD = 0;

    // Power variable only applicable to 'x^n' functions — represented as 'n' here
    // Base N variable only applicable to 'n^x' functions — represented as 'n' here

    public int power = 2;

    // A infinitesimally small number chosen in order to perform numerical differentiation
    private float hValue = (float)(Mathf.Pow(10, -4));

    // The type of function to be plotted
    public FunctionType functionType;

    public int baseN = 2;

    public bool differentiate = false;

    // Local points list
    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> dPoints = new List<Vector2>();

    private LineRendererUI lineRenderer;
    private DerivRendererUI derivRenderer;

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

    // Refresh ONLY the original function graph, not the derivative one
    public void RefreshLine()
    {
        lineRenderer.enabled = false;
        lineRenderer.enabled = true;
    }

    public void RefreshDeriv()
    {
        derivRenderer.enabled = false;
        derivRenderer.enabled = true;
    }

    private void PlotFunction(FunctionType type)
    {
        lineRenderer = FindObjectOfType<LineRendererUI>();
        derivRenderer = FindObjectOfType<DerivRendererUI>();

        if (lineRenderer != null)
        {
            points.Clear();
            RefreshLine();

            ComputeGraph(type, transA, transK, transC, transD, power, baseN);
            lineRenderer.points = points;
        }

        if (differentiate == true && lineRenderer != null)
        {
            // Refresh ONLY the derivative graph & show on the UI
            RefreshDeriv();

            if (derivRenderer != null)
            {
                dPoints.Clear();
                points.Clear();
                RefreshLine();

                ComputeGraph(type, transA, transK, transC, transD, power, baseN);
                derivRenderer.points = dPoints;
            }
        }

        else if (differentiate == false)
        {
            dPoints.Clear();

            // Refresh ONLY the derivative graph & hide on the UI
            RefreshDeriv();
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

                // Differentiate numerically using the centred three-point method
                dyValue = ((transA * (float)(Mathf.Abs(transK * ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Abs(transK * ((xValue - hValue) - transD)) + transC))) / (hValue * 2);
            }
            // Exponential Function
            else if (functionType == FunctionType.Exponential)
            {
                yValue = transA * (float)(Mathf.Pow(transK * baseN, (xValue - transD)) + transC);

                // Differentiate numerically using the centred three-point method
                dyValue = ((transA * (float)(Mathf.Pow(transK * baseN, ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Pow(transK * baseN, ((xValue - hValue) - transD)) + transC))) / (hValue * 2);
            }
            // Square Root Function
            else if (functionType == FunctionType.SquareRoot)
            {
                yValue = transA * (float)(Mathf.Sqrt(transK * (xValue - transD)) + transC);

                // Differentiate numerically using the centred three-point method
                dyValue = ((transA * (float)(Mathf.Sqrt(transK * ((xValue + hValue) - transD)) + transC)) - (transA * (float)(Mathf.Sqrt(transK * ((xValue - hValue) - transD)) + transC))) / (hValue * 2);
            }

            else if (functionType == FunctionType.Sine)
            {
                yValue = transA * (float)(Mathf.Sin(transK * (xValue - transD)) + transC);

                // Differentiate numerically using the centred three-point method
                dyValue = transA * (float)(Mathf.Cos(transK * (xValue - transD)) + transC);
            }
            else if (functionType == FunctionType.Cosine)
            {
                yValue = transA * (float)(Mathf.Cos(transK * (xValue - transD)) + transC);

                // Differentiate numerically using the centred three-point method
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

/* 

:: Numerical Differentiation Explanation ::

You can't calculate the exact derivative of a function using a computer program (unless you're doing symbolic math... but that's another, way more complicated, topic).

There are several approaches to computing a numerical derivative of a function. The simplest is the centered three-point method:

Take a small number h
Evaluate  [f(x+h) - f(x-h)] / 2h 
Voilà, an approximation of f'(x), with only two function evaluations
Another approach is the centered five-point method:

Take a small number h
Evaluate [f(x-2h) - 8f(x-h) + 8f(x+h) - f(x+2h)] / 12h
Voilà, a better approximation of f'(x), but it requires more function evaluations

In this program, we use the relatively simpler approach 'the three-point method.'

*/