using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ABRV.Abrv;

public class Main : MonoBehaviour {
    private List<int> valueListTahoe = new List<int>();
    private List<int> valueListReno = new List<int>();
    Tcp tcp1 = new Tcp_Tahoe();
    Tcp tcp2 = new Tcp_Reno();
    public string recebido;
    private bool started = false; //flag pra dizer que ja iniciou

    [SerializeField] private Sprite pauseImg, playImg;
    [SerializeField] private Button startBtn;

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
        
        var valor = tcp1.Run(recebido);
        var valorReno = tcp2.Run(recebido);

        // o recebido eh resetado para que o grafico continue andando, ah nao ser que seja disparado um 
        // tout /tack novamente
        recebido = ACK;

        valueListTahoe.Add(valor);
        valueListReno.Add(valorReno);
        Debug.Log("CWND:::::"+tcp1.Cwnd);
        Window_Graph.instance.ShowGraph(valueListTahoe, tcp1.nomeVariante);
        Window_Graph.instance.ShowGraph(valueListReno, tcp2.nomeVariante);

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
        valueListReno.Clear();
        valueListTahoe.Clear();
        tcp1 = new Tcp_Tahoe(); //resetando o tcp (janela e sttresh)
        tcp2 = new Tcp_Reno();

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
}
