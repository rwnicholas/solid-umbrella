using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class Window_Graph : MonoBehaviour {
    [SerializeField] private Sprite circleSpriteTcp;
    [SerializeField]private RectTransform graphContainer; //onde inicia o grafico
    [SerializeField]private List<int> valueList = new List<int>();
    [SerializeField] private RectTransform background;
    [SerializeField] private GameObject line;
    [SerializeField] private RectTransform lineReference;
    [SerializeField] private GameObject numberLine;
    
    [SerializeField] private Dictionary<string, ReactiveCollection<float>> tcpsValuesList = new Dictionary<string, ReactiveCollection<float>>(); //lista que vai cuidar do valor de cada tcp

    [SerializeField] private int counterLastUsedCircle = 0;  //vai contar do ultimo circulo utilizado pra n ter q reconstruir a lista
    private int iterations = 0; //vai contar as iteracoes pra que o contador de circulos so aumente de dois em 2

    public static Window_Graph instance;


    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        background = transform.Find("background").GetComponent<RectTransform>();
        instance = this;
        CreateLinesGraph();
    }

    public void CreateTcpsLists(List<Tcp> tcps, List<Color> colors)
    {
        int contColor = 0;

        foreach(Tcp tcpKey in tcps)
        {
            ReactiveCollection<float> collection = new ReactiveCollection<float>();
            collection.ObserveAdd().Subscribe(
                x => {
                    print("INDex=>" + x.Index);
                    this.ShowGraph(x.Value, x.Index, tcpKey, colors[tcps.IndexOf(tcpKey)], tcps.IndexOf(tcpKey));
                }
            );
            tcpsValuesList.Add(tcpKey.nomeVariante, collection);
            contColor++;
        }
    }
    public static List<GameObject> lastCircleGameObject = new List<GameObject>(); //mudar isso pro inicio depois

    public void AddTcpValue(string tcpKey, float newValue)
    {
        if (tcpsValuesList.ContainsKey(tcpKey))
        {
            tcpsValuesList[tcpKey].Add(newValue);
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, Tcp tcp, Color color) { //tinha o tcp e a cor
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.AddComponent<CircleCollider2D>().radius = 2.53f;

        gameObject.AddComponent<ChartCircle>();
        gameObject.GetComponent<ChartCircle>().AddAtributesValues(tcp.Cwnd, tcp.Estado);

        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSpriteTcp;
        gameObject.GetComponent<Image>().color = color;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    public void ShowGraph(float value, int index, Tcp tcp, Color color, int idTCP) {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 5f;

        float xPosition = xSize + index * xSize;
        float yPosition = (value / yMaximum) * graphHeight;
        GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), tcp, color);
        print(idTCP);
        if (lastCircleGameObject[idTCP] != null)
        {
            CreateDotConnection(lastCircleGameObject[idTCP].GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
        }
        lastCircleGameObject[idTCP] = circleGameObject;
        

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
        float tax = 0.75f;
        linha.transform.position = new Vector3(lineReference.position.x, lineReference.position.y, linha.transform.position.z);

        int n = 0;
        GameObject numb = Instantiate(numberLine) as GameObject;
        numb.transform.SetParent(linha.transform, false);
        numb.transform.position = new Vector3(linha.transform.position.x - 0.3f, linha.transform.position.y+0.2f, linha.transform.position.z);
        TextMeshProUGUI number = numb.GetComponent<TextMeshProUGUI>();
        number.text = n.ToString();

        
        for (int i = 1; i < 11; i++)
        {
            n += 10;
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
