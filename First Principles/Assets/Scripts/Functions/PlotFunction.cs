using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Designed by John Seong in Ontario
public class PlotFunction
{
    // Local 'points' list — should not affect the global 'points' list in the LindRendererUI class
    private List<Vector2> points = new List<Vector2>();

    // Get the values from LineRendererUI class
    public List<Vector2> computeGraph(string functionType, float xValue, float yValue, float transA, float transK, float transC, float transD, int power, int baseN)
    {
        int state = 1;

        switch (state)
        {
            case 1:
                for (float i = -1000; i <= 1000; i++)
                {
                    // Brute-force values from -1000 to 1000 into X
                    xValue = i;

                    if (xValue != 0 && transA != 0 && transK != 0 && transC != 0 && transD != 0)
                    {
                        // Power of N Function
                        if (functionType == "Power")
                        {
                            yValue = transA * (float)(Mathf.Pow(transK * (xValue - transD), power) + transC);
                        }

                        // Absolute Value Function
                        else if (functionType == "Absolute")
                        {
                            yValue = transA * (float)(Mathf.Abs(transK * (xValue - transD)) + transC);
                        }

                        // Exponential Function
                        else if (functionType == "Exponential")
                        {
                            yValue = transA * (float)(Mathf.Pow(transK * baseN, (xValue - transD)) + transC);
                        }

                        // Square Root Function
                        else if (functionType == "Square Root")
                        {
                            yValue = transA * (float)(Mathf.Sqrt(transK * (xValue - transD)) + transC);
                        }
                    }
                    // Add the coordinates to the array
                    points.Add(new Vector2(xValue, yValue));
                }
                state = 2;
                break;

            case 2:
                // Add additional components here...
                break;
        }
        return points;
    }
}