using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public LineRendererUI path;

    public void CreatePath()
    {
        path = new LineRendererUI(transform.position);
    }
}