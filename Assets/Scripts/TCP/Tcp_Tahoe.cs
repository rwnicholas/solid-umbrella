using static ABRV.Abrv;
using UnityEngine;
public class Tcp_Tahoe : Tcp {

	public Tcp_Tahoe (){
		Debug.Log(estado);
	}
	public override int Run (string recebido){
		nomeVariante = "Tahoe";
		//RTT -> Tempo medido desde o envio de um pacote até a chegada 
		//       da confirmação do recebimento do mesmo

		if (estado == SLOWSTART) {
			if (recebido == ACK && cwnd < ssthreshold) {
				cwnd++;
			} else if (recebido == TOUT) {
				ssthreshold = cwnd/2;
				cwnd = 1;
			} else if (recebido == TACK) {
				ssthreshold = cwnd/2;
				cwnd = 1;
			} else if (recebido == ACK && cwnd >= ssthreshold) {
				cwnd+=3;
				estado = C_AVOIDENCE;
			}
		} else if (estado == C_AVOIDENCE) {
			if (recebido == ACK) {
				cwnd+=3;
			} else if (recebido == TOUT) {
				ssthreshold = cwnd/2;
				cwnd = 1;
				estado = SLOWSTART;
			} else if (recebido == TACK) {
				ssthreshold = cwnd/2;
				cwnd = 1;
				estado = SLOWSTART;
			}
		}
		return cwnd;
	}
}