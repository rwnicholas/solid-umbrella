using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AbreviacoesTCP;
using TCP_Interface;

namespace TCP_Variant_Tahoe
{
    public class TahoeVariantTCP : TCP {
        public TahoeVariantTCP () {
            nomeVariante = "Tahoe";
        }

        public override float Run(string recebido) {
            //RTT -> Tempo medido desde o envio de um pacote até a chegada 
            //       da confirmação do recebimento do mesmo

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
                    cwnd = 1;
                    estado = Abrv.SLOWSTART;
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
                    cwnd = 1;
                    estado = Abrv.SLOWSTART;
                }
            }
            return cwnd;
        }

        public override TCP Init() {

            return new TahoeVariantTCP();
        }
    }
}

