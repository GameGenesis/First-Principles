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

        for (int i = 0; i < gridRenderer.gridSize.x; i++)
        {
            xLabels.Add(Instantiate(labelPrefab, transform.TransformPoint(xPos), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>());
            xLabels[i].text = i.ToString();
            xPos.x += 70;
        }

        for (int i = 0; i < gridRenderer.gridSize.y - 1; i++)
        {
            yLabels.Add(Instantiate(labelPrefab, transform.TransformPoint(yPos), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>());
            yLabels[i].text = (i + 1).ToString();
            yPos.y += 70;
        }
    }

    void Update()
    {
        
    }
}
