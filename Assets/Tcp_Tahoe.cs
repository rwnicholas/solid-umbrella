using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tcp_Tahoe : Tcp {
	public override void Calcular(string Recebido){
		//algoritimo tcp tahoe
		if (Recebido == "TOUT" || Recebido == "TACK") {
			cwnd = 1.0f;
			ssthreshold = Mathf.Round(ssthreshold / 2);
			if (Recebido == "TACK") {
				estado = "Fast Retransmit";
			} 
			else {
				estado = "Slow Start";
			}
		} 

		else if (cwnd < ssthreshold) {
			if (cwnd * 2 > ssthreshold) {
				cwnd = ssthreshold;
			} 

			else {
				cwnd = cwnd * 2;
			}
			estado = "Slow Start";
		}

		else{
			if (cwnd + 1 > 100) {
				cwnd = 1.0f;
				ssthreshold = Mathf.Round(ssthreshold / 2);
				estado = "Fast Retransmit";
			} 

			else {
				cwnd = cwnd + 1;
				ssthreshold = ssthreshold + 1;
				estado = "Congestion Avoidence";
			}
		}
	}

	public override void setNomeVariante(){
		nomeVariante = "Tahoe";
	} 
}