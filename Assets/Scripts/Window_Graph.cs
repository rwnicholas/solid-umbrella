using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Window_Graph : MonoBehaviour {
    [SerializeField] private Sprite circleSpriteTahoe;
    [SerializeField] private Sprite circleSpriteReno;
    [SerializeField]private RectTransform graphContainer; //onde inicia o grafico
    [SerializeField]private List<int> valueList = new List<int>();
    [SerializeField] private RectTransform background;
    [SerializeField] private GameObject line;
    [SerializeField] private RectTransform lineReference;
    [SerializeField] private GameObject numberLine;

    public static Window_Graph instance;

    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        background = transform.Find("background").GetComponent<RectTransform>();
        instance = this;
        CreateLinesGraph();
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, string variant) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);

        if (variant.ToLower().Equals("tahoe"))
        {
            gameObject.GetComponent<Image>().sprite = circleSpriteTahoe;
        } else if (variant.ToLower().Equals("reno"))
        {
            gameObject.GetComponent<Image>().sprite = circleSpriteReno;
        }

        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    public void ShowGraph(List<int> valueList, string variant) {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 5f;
        
        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i *xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), variant);
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB -dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        rectTransform.sizeDelta = new Vector2(distance, 1f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0,0, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
    }

    private void CreateLinesGraph()
    {
        GameObject linha = Instantiate(line) as GameObject;
        linha.transform.SetParent(background, false);
        float tax = 0.6f;
        linha.transform.position = new Vector3(lineReference.position.x, lineReference.position.y, linha.transform.position.z);

        int n = 0;
        GameObject numb = Instantiate(numberLine) as GameObject;
        numb.transform.SetParent(linha.transform, false);
        numb.transform.position = new Vector3(linha.transform.position.x - 0.3f, linha.transform.position.y+0.2f, linha.transform.position.z);
        TextMeshProUGUI number = numb.GetComponent<TextMeshProUGUI>();
        number.text = n.ToString();

        
        for (int i = 1; i < 11; i++)
        {
            n += 20;
            float pos = lineReference.position.y + tax;
            linha = Instantiate(line) as GameObject;
            linha.transform.SetParent(background, false);

            linha.transform.position = new Vector3(lineReference.position.x, pos, linha.transform.position.z);

            numb = Instantiate(numberLine) as GameObject;
            numb.transform.SetParent(linha.transform, false);
            numb.transform.position = new Vector3(linha.transform.position.x - 0.3f, linha.transform.position.y+0.2f, linha.transform.position.z);
            number = numb.GetComponent<TextMeshProUGUI>();
            number.text = n.ToString();

            tax += 0.6f;
        }

    }

    public void ClearDotsAndConection()
    {
        RectTransform[] dotsAndConections = graphContainer.GetComponentsInChildren<RectTransform>();

        foreach (RectTransform rt in dotsAndConections)
        {
            if (!rt.gameObject.name.Equals("graphContainer"))
            {
                Destroy(rt.gameObject);
            }
            
        }
    }
}
