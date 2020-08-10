using static ABRV.Abrv;
using UnityEngine;
public class Tcp_Tahoe : Tcp {

	public Tcp_Tahoe (){
        nomeVariante = "Tahoe";
    }

    public override Tcp Init() {
        return new Tcp_Tahoe();
    }

    public override float Run (string recebido){
		//RTT -> Tempo medido desde o envio de um pacote até a chegada 
		//       da confirmação do recebimento do mesmo

		// if (estado == SLOWSTART) {
		// 	if (recebido == ACK && cwnd < ssthreshold) {
		// 		cwnd++;
        //         estado = SLOWSTART;
		// 	} else if (recebido == TOUT) {
		// 		ssthreshold = Mathf.Round(cwnd / 2);
		// 		cwnd = 1;
        //         estado = SLOWSTART;
		// 	} else if (recebido == TACK) {
		// 		ssthreshold = Mathf.Round(cwnd / 2);
		// 		cwnd = 1;
        //         estado = SLOWSTART;
		// 	} else if (recebido == ACK && cwnd >= ssthreshold) {
		// 		cwnd+=3;
		// 		estado = C_AVOIDENCE;
		// 	}
		// } else if (estado == C_AVOIDENCE) {
		// 	if (recebido == ACK) {
		// 		cwnd+=3;
        //         estado = C_AVOIDENCE;
		// 	} else if (recebido == TOUT) {
		// 		ssthreshold = Mathf.Round(cwnd / 2);
		// 		cwnd = 1;
		// 		estado = SLOWSTART;
		// 	} else if (recebido == TACK) {
		// 		ssthreshold = Mathf.Round(cwnd / 2);
		// 		cwnd = 1;
		// 		estado = SLOWSTART;
		// 	}
		// }
		
		if (recebido == TOUT || recebido == TACK) {
			cwnd = 1.0f;
			ssthreshold = Mathf.Round(ssthreshold / 2);
			if (recebido == TACK) {
				estado = FAST_RET;
			} 
			else {
				estado = SLOWSTART;
			}
		} 

		else if (cwnd < ssthreshold) {
			if (cwnd * 2 > ssthreshold) {
				cwnd = ssthreshold;
			} 

			else {
				cwnd = cwnd * 2;
			}
			estado = SLOWSTART;
		}

		else{
			if (cwnd + 1 > 100) {
				cwnd = 1.0f;
				ssthreshold = Mathf.Round(ssthreshold / 2);
				estado = FAST_RET;
			} 

			else {
				cwnd = cwnd + 1;
				ssthreshold = ssthreshold + 1;
				estado = C_AVOIDENCE;
			}
		}

		return cwnd;
	}


}