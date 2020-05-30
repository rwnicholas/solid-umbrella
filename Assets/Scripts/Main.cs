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

    //UIs
    [SerializeField] private Sprite pauseImg, playImg;
    [SerializeField] private Button startBtn;
    [SerializeField] private GameObject tahoeObj, renoObj;

    //variants flags
    [SerializeField] private bool tahoeOn = false;
    [SerializeField]private bool renoOn = false;

    //public bool TahoeOn { get => tahoeOn; }
    //public bool RenoOn { get => renoOn;}
    
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
        var valorTahoe = 0; ;
        var valorReno = 0;

        if (tahoeOn)
        {
            valorTahoe = tcp1.Run(recebido);
        }
        if (renoOn)
        {
            valorReno = tcp2.Run(recebido);
        }
        // o recebido eh resetado para que o grafico continue andando, ah nao ser que seja disparado um 
        // tout /tack novamente
        recebido = ACK;

        if (tahoeOn)
        {
            Debug.Log("CWND:::::" + tcp1.Cwnd);
            valueListTahoe.Add(valorTahoe);
            tahoeObj.GetComponent<Variant>().ChangeCWNDTax(tcp1.Cwnd.ToString());
            tahoeObj.GetComponent<Variant>().ChangeCurrentState(tcp1.Estado);
            Window_Graph.instance.ShowGraph(valueListTahoe, tcp1.nomeVariante);
        }

        if (renoOn)
        {
            valueListReno.Add(valorReno);
            renoObj.GetComponent<Variant>().ChangeCWNDTax(tcp2.Cwnd.ToString());
            renoObj.GetComponent<Variant>().ChangeCurrentState(tcp2.Estado);
            Window_Graph.instance.ShowGraph(valueListReno, tcp2.nomeVariante);
        }
    }
    

    public void timeout() {
        recebido = TOUT;
    }

    public void tack() {
        recebido = TACK;
    }

    public void ResetTahoe()
    {
        valueListTahoe.Clear();
        tcp1 = new Tcp_Tahoe(); //resetando o tcp (janela e sttresh)
    }

    public void ResetReno()
    {
        valueListReno.Clear();
        tcp2 = new Tcp_Reno();
    }

    public void Reset()
    {
        if (started)
        {
            started = false;
            changeButtonStart();
        }
        CancelInvoke("UpdateInterval");

        ResetTahoe();
        ResetReno();
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

    public void ChangeTahoe(bool value)
    {
        this.tahoeOn = value;

        if (!tahoeOn)
        {
            ResetTahoe();
        }
    }

    public void ChangeReno(bool value)
    {
        renoOn = value;

        if (renoOn==false)
        {
            ResetReno();
        }
    }
}
