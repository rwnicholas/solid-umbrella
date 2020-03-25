using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tcp_Reno : Tcp {
	public override void Calcular(string Recebido){
		//algoritimo tcp reno
		if (Recebido == "TOUT") {
			ssthreshold  = Mathf.Round(cwnd/2);
			cwnd = 1.0f;
			estado = "Slow Start";
		} 

		else if (Recebido == "TACK") {
			ssthreshold = Mathf.Round(cwnd/2);
			if(ssthreshold < 1){
				ssthreshold = 1.0f;
			}
			cwnd = ssthreshold + 3;
			estado = "Fast Retransmit/Recovery";

		}

		else if (cwnd < ssthreshold && estado == "Slow Start") {
			if (cwnd * 2 > ssthreshold) {
				cwnd = ssthreshold;
			} 

			else {
				cwnd = cwnd * 2;
			}
		}

		else{
			if (cwnd + 1 > 100) {
				ssthreshold = Mathf.Round(cwnd/2);
				cwnd = ssthreshold + 3;
				estado = "Fast Retransmit/Recovery";
			} 

			else {
				cwnd = cwnd + 1;
				ssthreshold = ssthreshold + 1;
				estado = "Congestion Avoidence";
			}
		}
	}

	public override void setNomeVariante(){
		nomeVariante = "Reno";
	} 
}