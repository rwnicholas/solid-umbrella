using static ABRV.Abrv;
using UnityEngine;

public class Tcp_Reno : Tcp
{
    private int cont = 0;
    public Tcp_Reno() {
        nomeVariante = "Reno";
    }

    public override Tcp Init() {
        return new Tcp_Reno();
    }

    public override float Run(string recebido)
    {
        cont++;
        Debug.Log("CONT:::"+cont);
        if (estado == SLOWSTART)
        {
            if (recebido == ACK && cwnd < ssthreshold)
            {
                cwnd++;
                estado = SLOWSTART;
            }
            else if (recebido == TOUT)
            {
                ssthreshold = Mathf.Round(cwnd / 2);                
                cwnd = 1;
                estado = SLOWSTART;
            }
            else if (recebido == TACK)
            {
                ssthreshold = Mathf.Round(cwnd / 2);
                cwnd = Mathf.Round((cwnd / 2) + 3);
                estado = C_AVOIDENCE;
            }
            else if (recebido == ACK && cwnd >= ssthreshold)
            {
                cwnd += 3;
                estado = C_AVOIDENCE;
            }
        }
        else if (estado == C_AVOIDENCE)
        {
            if (recebido == ACK)
            {
                cwnd += 3;
                estado = C_AVOIDENCE;
            }
            else if (recebido == TOUT)
            {
                ssthreshold = Mathf.Round(cwnd / 2);
                cwnd = 1;
                estado = SLOWSTART;
            }
            else if (recebido == TACK)
            {
                ssthreshold = Mathf.Round(cwnd / 2);
                cwnd = Mathf.Round((cwnd / 2) + 3);
                estado = C_AVOIDENCE;
            }
        }
        return cwnd;
    }
}