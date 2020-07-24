using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using AbreviacoesTCP;
using TCP_Interface;


public class Main : MonoBehaviour {
    private List<float> valueListTcp1 = new List<float>();
    private List<float> valueListTcp2 = new List<float>();
    private List<float> valueListTcp3 = new List<float>();
    TCP tcp1, tcp2, tcp3;
    TCP tcp1Default, tcp2Default, tcp3Default;
    public string recebido;
    private bool started = false; //flag pra dizer que ja iniciou

    //UIs
    [SerializeField] private Sprite pauseImg, playImg;
    [SerializeField] private Button startBtn;
    [SerializeField] private GameObject tcp1Obj, tcp2Obj, tcp3Obj;

    //variants flags
    [SerializeField] private bool tcp1On = false; 
    [SerializeField] private bool tcp2On = false;
    [SerializeField] private bool tcp3On = false;

    //public bool TahoeOn { get => tcp1On; }
    //public bool RenoOn { get => tcp2On;}
    public const float graphicLimit_y = 100;
    public bool limitReached_y = false;
    private string recebidoTcp1Limit;
    private string recebidoTcp2Limit;
    private string recebidoTcp3Limit;

    public void Start() {
        print(Application.dataPath);
        string[] Assemblys = Directory.GetFiles(Application.dataPath + "\\TCPs\\", "TCP_Variant_*.dll");
        int AssemblyNumber = Directory.GetFiles(Application.dataPath + "\\TCPs\\", "TCP_Variant_*.dll").Length;
        Assembly[] DLL = new Assembly[AssemblyNumber];
        for (int i = 0; i < AssemblyNumber && i < 3; i++) {
            DLL[i] = Assembly.LoadFile(Assemblys[i]);
        }


        for (int i = 0; i < AssemblyNumber; i++) {
            foreach(Type type in DLL[i].GetExportedTypes()) {
                if (i == 0) {
                    var c = Activator.CreateInstance(type);
                    type.InvokeMember("Init", BindingFlags.InvokeMethod, null, c, null);
                    tcp1 = (TCP)c;
                    
                }else if (i == 1) {
                    var c = Activator.CreateInstance(type);
                    type.InvokeMember("Init", BindingFlags.InvokeMethod, null, c, null);
                    tcp2 = (TCP)c;

                }else if (i == 2) {
                    var c = Activator.CreateInstance(type);
                    type.InvokeMember("Init", BindingFlags.InvokeMethod, null, c, null);
                    tcp3 = (TCP)c;

                }
            }
        }

        foreach (var child in GameObject.FindGameObjectsWithTag("tcpName1")) {
            child.GetComponent<TMPro.TextMeshProUGUI>().text = tcp1.nomeVariante;
        }
        foreach (var child in GameObject.FindGameObjectsWithTag("tcpName2")) {
            child.GetComponent<TMPro.TextMeshProUGUI>().text = tcp2.nomeVariante;
        }
        foreach (var child in GameObject.FindGameObjectsWithTag("tcpName3")) {
            child.GetComponent<TMPro.TextMeshProUGUI>().text = tcp3.nomeVariante;
        }
    }

    public void RunStart() {
        if (!started)
        {
            float updateInterval = 0.5f;
            recebido = Abrv.ACK;
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
        float valorTcp1 = 0; ;
        float valorTcp2 = 0;
        float valorTcp3 = 0;

        if (tcp1On)
        {
            if(limitReached_y) {
                valorTcp1 = tcp1.Run(recebidoTcp1Limit);
            } else {
                valorTcp1 = tcp1.Run(recebido);
            }
        }
        if (tcp2On)
        {
            if (limitReached_y) {
                valorTcp2 = tcp2.Run(recebidoTcp2Limit);
            } else {
                valorTcp2 = tcp2.Run(recebido);
            }
        }
        if (tcp3On) {
            if (limitReached_y) {
                valorTcp3 = tcp3.Run(recebidoTcp3Limit);
            } else {
                valorTcp3 = tcp3.Run(recebido);
            }
        }
        // o recebido eh resetado para que o grafico continue andando, ah nao ser que seja disparado um 
        // tout /tack novamente
        recebido = Abrv.ACK;
        limitReached_y = false;
        recebidoTcp1Limit = recebidoTcp2Limit = recebidoTcp3Limit = Abrv.ACK;

        if (valorTcp1 >= graphicLimit_y) {
            limitReached_y = true;
            recebidoTcp1Limit = Abrv.TACK;
        }
        
        if (valorTcp2 >= graphicLimit_y) {
            limitReached_y = true;
            recebidoTcp2Limit = Abrv.TACK;
        }

        if (valorTcp3 >= graphicLimit_y) {
            limitReached_y = true;
            recebidoTcp3Limit = Abrv.TACK;
        }

        if (tcp1On)
        {
            Debug.Log("CWND:::::" + tcp1.Cwnd);
            valueListTcp1.Add(valorTcp1);
            tcp1Obj.GetComponent<Variant>().ChangeCWNDTax(tcp1.Cwnd.ToString());
            tcp1Obj.GetComponent<Variant>().ChangeCurrentState(tcp1.Estado);
            Window_Graph.instance.ShowGraph(valueListTcp1, "tcp1");
        }

        if (tcp2On)
        {
            valueListTcp2.Add(valorTcp2);
            tcp2Obj.GetComponent<Variant>().ChangeCWNDTax(tcp2.Cwnd.ToString());
            tcp2Obj.GetComponent<Variant>().ChangeCurrentState(tcp2.Estado);
            Window_Graph.instance.ShowGraph(valueListTcp2, "tcp2");
        }

        if (tcp3On) {
            valueListTcp3.Add(valorTcp3);
            tcp3Obj.GetComponent<Variant>().ChangeCWNDTax(tcp3.Cwnd.ToString());
            tcp3Obj.GetComponent<Variant>().ChangeCurrentState(tcp3.Estado);
            Window_Graph.instance.ShowGraph(valueListTcp3, "tcp3");
        }
    }

    public void timeout() {
        recebido = Abrv.TOUT;
    }

    public void tack() {
        recebido = Abrv.TACK;
    }

    public void ResetTcp1()
    {
        valueListTcp1.Clear();
        tcp1 = tcp1.Init();
        //tcp1 = new TahoeVariantTCP(); //resetando o tcp (janela e sttresh)
    }

    public void ResetTcp2()
    {
        valueListTcp2.Clear();
        tcp2 = tcp2.Init();
        //tcp2 = new RenoVariantTCP();
    }

    public void ResetTcp3() {
        valueListTcp3.Clear();
        tcp3 = tcp3.Init();
        //tcp3 = new CubicVariantTCP();
    }

    public void Reset()
    {
        if (started)
        {
            started = false;
            changeButtonStart();
        }
        CancelInvoke("UpdateInterval");

        ResetTcp1();
        ResetTcp2();
        ResetTcp3();
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

    public void ChangeTcp1(bool value)
    {
        this.tcp1On = value;

        if (!tcp1On)
        {
            ResetTcp1();
        }
    }

    public void ChangeTcp2(bool value)
    {
        tcp2On = value;

        if (tcp2On==false)
        {
            ResetTcp2();
        }
    }

    public void ChangeTcp3(bool value) {
        tcp3On = value;

        if (tcp3On == false) {
            ResetTcp3();
        }
    }
}
