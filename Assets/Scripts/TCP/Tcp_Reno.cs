using static ABRV.Abrv;
using UnityEngine;

public class Tcp_Reno : Tcp
{
    private int cont = 0;
    public Tcp_Reno()
    {
        Debug.Log(estado);
    }
    public override float Run(string recebido)
    {
        nomeVariante = "Reno";
        cont++;
        Debug.Log("CONT:::"+cont);
        if (estado == SLOWSTART)
        {
            if (recebido == ACK && cwnd < ssthreshold)
            {
                cwnd++;
            }
            else if (recebido == TOUT)
            {
                cwnd = 1;
                ssthreshold = cwnd / 2;                
            }
            else if (recebido == TACK)
            {
                ssthreshold = cwnd / 2;
                cwnd = (cwnd/2)+3;
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
            }
            else if (recebido == TOUT)
            {
                cwnd = 1;
                ssthreshold = cwnd / 2;
                estado = SLOWSTART;
            }
            else if (recebido == TACK)
            {
                ssthreshold = cwnd / 2;
                cwnd = (cwnd / 2) + 3;
            }
        }
        return cwnd;
    }
}