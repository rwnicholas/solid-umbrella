using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ABRV.Abrv;

public class Main : MonoBehaviour {
    private List<int> valueList = new List<int>();
    Tcp tcp1 = new Tcp_Tahoe();
    public string recebido;

    public void RunStart() {
        float updateInterval = 0.5f;
        recebido = ACK;
        InvokeRepeating("UpdateInterval", updateInterval, updateInterval);
    }

    private void UpdateInterval() {
        var valor = tcp1.Run(recebido);
        recebido = ACK;
        valueList.Add(valor);
        Window_Graph.instance.ShowGraph(valueList);
    }

    public void timeout() {
        recebido = TOUT;
    }

    public void tack() {
        recebido = TACK;
    }
}
