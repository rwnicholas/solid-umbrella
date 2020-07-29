using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static ABRV.Abrv;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour {
    private List<List<float>> valuesList = new List<List<float>>();
    List<Tcp> tcps = new List<Tcp>();
    List<bool> tcpsToggleStates = new List<bool>();
    public string recebido;
    private bool started = false; //flag pra dizer que ja iniciou

    //UIs
    [SerializeField] private Sprite pauseImg, playImg;
    [SerializeField] private Button startBtn;
    [SerializeField] private List<GameObject> tcpsObjects;
    [SerializeField] private RectTransform contentTCPsScrollView;
    [SerializeField] private GameObject tcpInformationPanelPrefab;
    [SerializeField] private List<Color> tcpColors = new List<Color>();
    
    public const float graphicLimit_y = 100;
    public bool limitReached_y = false;
    List<string> listRecebidosTcpLimit = new List<string>();

    public void Start() {
        //print(Application.dataPath);
        //string[] Assemblys = Directory.GetFiles(Application.dataPath + "\\TCPs\\", "TCP_Variant_*.dll");
        //int AssemblyNumber = Directory.GetFiles(Application.dataPath + "\\TCPs\\", "TCP_Variant_*.dll").Length;
        //Assembly[] DLL = new Assembly[AssemblyNumber];
        //for (int i = 0; i < AssemblyNumber; i++) {
        //    DLL[i] = Assembly.LoadFile(Assemblys[i]);
        //}


        //for (int i = 0; i < AssemblyNumber; i++) {
        //    foreach (Type type in DLL[i].GetExportedTypes()) {
        //        var c = Activator.CreateInstance(type);
        //        type.InvokeMember("Init", BindingFlags.InvokeMethod, null, c, null);
        //        TCP tcp = (TCP)c;

        //        //criacao das listas
        //        tcps.Add(tcp);
        //        tcpsToggleStates.Add(false); //adicionando o estado do toggle de cada um q sera identificado pelo indice
        //        listRecebidosTcpLimit.Add("");
        //        valuesList.Add(new List<float>());

        //    }
        //}

        tcps.Add(new Tcp_Tahoe());
        tcpsToggleStates.Add(false);
        listRecebidosTcpLimit.Add("");
        valuesList.Add(new List<float>());

        tcps.Add(new Tcp_Reno());
        tcpsToggleStates.Add(false);
        listRecebidosTcpLimit.Add("");
        valuesList.Add(new List<float>());

        tcps.Add(new Tcp_Cubic());
        tcpsToggleStates.Add(false);
        listRecebidosTcpLimit.Add("");
        valuesList.Add(new List<float>());

        for (int j=0; j < tcps.Count; j++)
        {
            GameObject tcpInforPrefab = Instantiate(tcpInformationPanelPrefab);
            tcpInforPrefab.name = tcps[j].nomeVariante;
            print(tcps[j].nomeVariante);
            tcpInforPrefab.transform.SetParent(contentTCPsScrollView,false);

            //ajustando as cores
            Color randomColor = new Color(
              Random.Range(0f, 1f),
              Random.Range(0f, 1f),
              Random.Range(0f, 1f)
            );

            //cor do toggle
            tcpInforPrefab.GetComponentsInChildren<Image>()[0].color = randomColor;
            //cor do fundo do painel
            tcpInforPrefab.GetComponentsInChildren<Image>()[2].color = randomColor;
            //cor da faixa do painel
            tcpInforPrefab.GetComponentsInChildren<Image>()[3].color = randomColor;
            //texto do toggle
            tcpInforPrefab.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = tcps[j].nomeVariante;
            //texto do titulo do painel
            tcpInforPrefab.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[5].text = tcps[j].nomeVariante;


            tcpsObjects.Add(tcpInforPrefab);

            tcpColors.Add(randomColor);
            
        }
        
    }

    public void RunStart() {
        if (!started)
        {
            float updateInterval = 0.5f;
            recebido = ACK;
            InvokeRepeating("UpdateInterval", updateInterval, updateInterval); //invoca o metodo com o nome selecionado nos tempo definido e fica repetindo a invocacao a cada tempo definido para isso 

            started = true;

            //trocando imagem de botao
            changeButtonStart();
        } else
        {
            started = false;
            CancelInvoke("UpdateInterval");

            //trocando imagem de botao
            changeButtonStart();
        }

    }

    private void UpdateInterval() {
        Dictionary<string, float> tcpValuesDic = new Dictionary<string, float>();        

        for(int i=0; i < tcpsToggleStates.Count; i++)
        {
            if (tcpsToggleStates[i]==true)
            {
                if (limitReached_y)
                {
                    float value = tcps[i].Run(listRecebidosTcpLimit[i]); //executando com a string respectiva do tcp
                    tcpValuesDic[tcps[i].nomeVariante] = value;
                } else
                {
                    float value = tcps[i].Run(recebido);
                    tcpValuesDic[tcps[i].nomeVariante] = value;
                }
            }
        }
        print(tcpValuesDic.Values.ToString());

        // o recebido eh resetado para que o grafico continue andando, ah nao ser que seja disparado um 
        // tout /tack novamente
        recebido = ACK;
        limitReached_y = false;

        for(int i = 0; i < listRecebidosTcpLimit.Count; i++)
        {
            listRecebidosTcpLimit[i]= ACK;
        }

        int contadorLoop = 0;
        foreach(string s in tcpValuesDic.Keys)
        {
            if (tcpValuesDic[s] >= graphicLimit_y)
            {
                limitReached_y = true;
                listRecebidosTcpLimit[contadorLoop] = TACK;
            }
            contadorLoop++;
        }

        for (int i=0; i < tcpsToggleStates.Count; i++)
        {
            if (tcpsToggleStates[i])
            {
                valuesList[i].Add(
                    tcpValuesDic[tcps[i].nomeVariante] //pegando o valor com a chave sendo o nome da variante
                ); //adicionando na matriz dos valores dos tcps o valor
                tcpsObjects[i].GetComponent<Variant>().ChangeCWNDTax(tcps[i].Cwnd.ToString());
                tcpsObjects[i].GetComponent<Variant>().ChangeCurrentState(tcps[i].Estado);
                Window_Graph.instance.ShowGraph(valuesList[i], ("tcp" + (i + 1).ToString()), tcpColors[i]);
            }
        }
    }

    public void timeout() {
        recebido = TOUT;
    }

    public void tack() {
        recebido = TACK;
    }

    public void Reset()
    {
        if (started)
        {
            started = false;
            changeButtonStart();
        }
        CancelInvoke("UpdateInterval");
        
        //pra resetar todos:
        for(int i = 0; i < tcps.Count; i++)
        {
            valuesList[i].Clear();

            tcps[i] = tcps[i].Init();
        }
        
        Window_Graph.instance.ClearDotsAndConection();
    }

    private void changeButtonStart()
    {
        if (startBtn.GetComponent<Image>().sprite.name.Equals("play-btn"))
        {
            startBtn.GetComponent<Image>().sprite = pauseImg;
        } else
        {
            startBtn.GetComponent<Image>().sprite = playImg;
        }
    }

    public void ChangeTcpState(string nameVariant, bool value)
    {
        int tcpIndex = 0;
        foreach(Tcp tcp in tcps)
        {
            if (tcp.nomeVariante.ToLower().Equals(nameVariant.ToLower()))
            {
                tcpsToggleStates[tcpIndex] = value; //mudando o estado do toggle
                if (value)
                {
                    ChangePositionAccordingToSelecteds(tcpIndex);
                }
            }
            tcpIndex++;
        }
    }

    private void ChangePositionAccordingToSelecteds(int index)
    {
        tcpsObjects[index].transform.SetAsFirstSibling();
    }
}
