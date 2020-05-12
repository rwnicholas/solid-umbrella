using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ABRV.Abrv;

public class Main : MonoBehaviour {
    private List<int> valueListTahoe = new List<int>();
    private List<int> valueListReno = new List<int>();
    Tcp tcp1 = new Tcp_Tahoe();
    Tcp tcp2 = new Tcp_Reno();
    public string recebido;

    public void RunStart() {
        float updateInterval = 0.5f;
        recebido = ACK;
        InvokeRepeating("UpdateInterval", updateInterval, updateInterval); //invoca o metodo com o nome selecionado nos tempo definido e fica repetindo a invocacao a cada tempo definido para isso 
     
    }

    private void UpdateInterval() {
        
        var valor = tcp1.Run(recebido);
        var valorReno = tcp2.Run(recebido);

        // o recebido eh resetado para que o grafico continue andando, ah nao ser que seja disparado um 
        // tout /tack novamente
        recebido = ACK;

        valueListTahoe.Add(valor);
        valueListReno.Add(valorReno);

        Window_Graph.instance.ShowGraph(valueListTahoe, "Tahoe");
        Window_Graph.instance.ShowGraph(valueListReno, "Reno");

    }
    

    public void timeout() {
        recebido = TOUT;
    }

    public void tack() {
        recebido = TACK;
    }
}
