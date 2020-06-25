using static ABRV.Abrv;
using UnityEngine;

public class Tcp_Cubic : Tcp {
    public float C { get; set; }
    public float t { get; set; }
    public float Wmax { get; set; }
    public float k { get; set; }
    public float B { get; set; }

    public Tcp_Cubic() {
        C = 0.4f;
        t = 0.0f;
        Wmax = 25.0f;
        B = 0.2f;
    }

    public override float Run(string recebido) {
        nomeVariante = "Cubic";
        //RTT -> Tempo medido desde o envio de um pacote até a chegada 
        //       da confirmação do recebimento do mesmo

        if (estado == SLOWSTART) {
            if (recebido == ACK) {
                cwnd++;
                estado = SLOWSTART;
            } else if (recebido == TOUT) {
                Wmax = cwnd;
                cwnd = 1;
                t = 0;
                ssthreshold = Wmax;
                estado = SLOWSTART;
                Debug.Log("Houve TOUT");
            } else if (recebido == TACK) {
                k = Mathf.Pow(Wmax * (B / C), 1f / 3f);
                t = 0;
                Wmax = cwnd;
                ssthreshold = Wmax;
                cwnd = C * Mathf.Pow(t - k, 3f) + Wmax;
                estado = C_AVOIDENCE;
            }
        } else if (estado == C_AVOIDENCE) {
            if (recebido == ACK) {
                k = Mathf.Pow(Wmax * (B / C), 1f / 3f);
                cwnd = cwnd + C * Mathf.Pow(t - k, 3f) + Wmax;
                Wmax = Wmax * B;
                t++;
            } else if (recebido == TOUT) {
                Wmax = cwnd;
                cwnd = 1;
                t = 0;
                ssthreshold = Wmax;
                estado = SLOWSTART;
            } else if (recebido == TACK) {
                k = Mathf.Pow(Wmax * (B / C), 1f / 3f);
                t = 0;
                Wmax = cwnd;
                ssthreshold = Wmax;
                cwnd = C * Mathf.Pow(t - k, 3f) + Wmax;
                estado = C_AVOIDENCE;
            }
        }
        return cwnd;
    }
}
