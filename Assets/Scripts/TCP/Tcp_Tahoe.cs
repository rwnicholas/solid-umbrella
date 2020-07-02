using static ABRV.Abrv;
using UnityEngine;
public class Tcp_Tahoe : Tcp {

	public Tcp_Tahoe (){
		Debug.Log(estado);
	}
	public override float Run (string recebido){
		nomeVariante = "Tahoe";
		//RTT -> Tempo medido desde o envio de um pacote até a chegada 
		//       da confirmação do recebimento do mesmo

		if (estado == SLOWSTART) {
			if (recebido == ACK && cwnd < ssthreshold) {
				cwnd++;
                estado = SLOWSTART;
			} else if (recebido == TOUT) {
				ssthreshold = Mathf.Round(cwnd / 2);
				cwnd = 1;
                estado = SLOWSTART;
			} else if (recebido == TACK) {
				ssthreshold = Mathf.Round(cwnd / 2);
				cwnd = 1;
                estado = SLOWSTART;
			} else if (recebido == ACK && cwnd >= ssthreshold) {
				cwnd+=3;
				estado = C_AVOIDENCE;
			}
		} else if (estado == C_AVOIDENCE) {
			if (recebido == ACK) {
				cwnd+=3;
                estado = C_AVOIDENCE;
			} else if (recebido == TOUT) {
				ssthreshold = Mathf.Round(cwnd / 2);
				cwnd = 1;
				estado = SLOWSTART;
			} else if (recebido == TACK) {
				ssthreshold = Mathf.Round(cwnd / 2);
				cwnd = 1;
				estado = SLOWSTART;
			}
		}
		return cwnd;
	}
}