using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TCP_Interface;
using AbreviacoesTCP;

namespace TCP_Variant_Reno
{
    public class RenoVariantTCP : TCP {
        private int cont = 0;

        public RenoVariantTCP() {
            nomeVariante = "Reno";
        }

        public override float Run(string recebido) {
            cont++;
            Debug.Log("CONT:::" + cont);
            if (estado == Abrv.SLOWSTART) {
                if (recebido == Abrv.ACK && cwnd < ssthreshold) {
                    cwnd++;
                    estado = Abrv.SLOWSTART;
                } else if (recebido == Abrv.TOUT) {
                    ssthreshold = Mathf.Round(cwnd / 2);
                    cwnd = 1;
                    estado = Abrv.SLOWSTART;
                } else if (recebido == Abrv.TACK) {
                    ssthreshold = Mathf.Round(cwnd / 2);
                    cwnd = Mathf.Round((cwnd / 2) + 3);
                    estado = Abrv.C_AVOIDENCE;
                } else if (recebido == Abrv.ACK && cwnd >= ssthreshold) {
                    cwnd += 3;
                    estado = Abrv.C_AVOIDENCE;
                }
            } else if (estado == Abrv.C_AVOIDENCE) {
                if (recebido == Abrv.ACK) {
                    cwnd += 3;
                    estado = Abrv.C_AVOIDENCE;
                } else if (recebido == Abrv.TOUT) {
                    ssthreshold = Mathf.Round(cwnd / 2);
                    cwnd = 1;
                    estado = Abrv.SLOWSTART;
                } else if (recebido == Abrv.TACK) {
                    ssthreshold = Mathf.Round(cwnd / 2);
                    cwnd = Mathf.Round((cwnd / 2) + 3);
                    estado = Abrv.C_AVOIDENCE;
                }
            }
            return cwnd;
        }

        public override TCP Init() {
            return new RenoVariantTCP();
        }
    }
}
