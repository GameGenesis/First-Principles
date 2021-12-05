using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelManager : MonoBehaviour
{
    [SerializeField] private GameObject labelPrefab;

    //Label placement every how many grid increments
    [SerializeField] private int horizontalIncrement = 1;
    [SerializeField] private int verticalIncrement = 1;

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
        int ySize = gridRenderer.gridSize.y;

        int xPositive = xSize / (2 * horizontalIncrement) + 1;
        int xNegative = (xPositive * 2) - 1;

        for (int i = 0; i < xPositive; i++)
        {
            xLabels.Add(Instantiate(labelPrefab, transform.TransformPoint(xPos), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>());
            string labelTxt = (i * horizontalIncrement).ToString();
            xLabels[i].text = labelTxt;
            xPos.x += 70 * horizontalIncrement;
        }

        xPos = xStartPos;
        xPos.x -= 70;

        for (int i = xPositive ; i < xNegative; i++)
        {
            xLabels.Add(Instantiate(labelPrefab, transform.TransformPoint(xPos), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>());
            string labelTxt = (-(i - (xSize / 2)) * horizontalIncrement).ToString();
            xLabels[i].text = labelTxt;
            xPos.x -= 70 * horizontalIncrement;
        }

        for (int i = 0; i < ySize / (2 * verticalIncrement) + 1; i++)
        {
            yLabels.Add(Instantiate(labelPrefab, transform.TransformPoint(yPos), Quaternion.identity, transform).GetComponent<TextMeshProUGUI>());
            yLabels[i].text = (i * verticalIncrement).ToString();
            yPos.y += 70 * verticalIncrement;
        }
    }

    void Update()
    {
        
    }
}
