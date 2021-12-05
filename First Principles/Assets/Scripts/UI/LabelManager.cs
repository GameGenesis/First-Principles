using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelManager : MonoBehaviour
{
    [SerializeField] private GameObject labelPrefab;

    [SerializeField] private Vector2 xStartPos = new Vector2(0, 0);
    [SerializeField] private Vector2 yStartPos = new Vector2(0, 0);

    private List<TextMeshProUGUI> xLabels = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> yLabels = new List<TextMeshProUGUI>();

    private GridRendererUI gridRenderer;

    private void Awake()
    {
        gridRenderer = FindObjectOfType<GridRendererUI>();
    }

    void Start()
    {
        Vector2 xPos = xStartPos;
        Vector2 yPos = yStartPos;

        int xSize = gridRenderer.gridSize.x;
        int ySize = gridRenderer.gridSize.y - 1;

        int xMidpoint = gridRenderer.gridSize.x / 2;
        int yMidpoint = (gridRenderer.gridSize.y / 2) - 1;

        for (int i = 0; i < xSize; i++)
        {
            xLabels.Add(Instantiate(labelPrefab, transform.TransformPoint(xPos), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>());
            xLabels[i].text = (i - xMidpoint).ToString();
            xPos.x += 70;
        }

        for (int i = 0; i < ySize; i++)
        {
            yLabels.Add(Instantiate(labelPrefab, transform.TransformPoint(yPos), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>());
            yLabels[i].text = (i - yMidpoint).ToString();
            yPos.y += 70;
        }
    }

    void Update()
    {
        
    }
}
