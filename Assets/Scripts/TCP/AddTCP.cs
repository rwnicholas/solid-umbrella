using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTCP {
    public void Init() {
        FillRequirements(new Tcp_Tahoe());
        FillRequirements(new Tcp_Reno());
        FillRequirements(new Tcp_Cubic());
    }

    private void FillRequirements(Tcp tcp) {
        Main.tcps.Add(tcp);
        Main.tcpsToggleStates.Add(false);
        Main.listRecebidosTcpLimit.Add("");
        Main.valuesList.Add(new List<float>());
        Window_Graph.lastCircleGameObject.Add(null);
    }
}
