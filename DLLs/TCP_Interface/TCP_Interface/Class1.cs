using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AbreviacoesTCP;

namespace TCP_Interface
{
    public abstract class TCP {
        protected string estado { get; set; }
        protected float cwnd { get; set; } //Janela de congestionamento
        protected float ssthreshold { get; set; } //Limiar de partida lenta
        protected string recebido { get; set; }
        public string nomeVariante { get; set; } //nome da variante do tcp (tahoe, cubic, etc)


        public abstract float Run(string recebido);
        public abstract TCP Init();

        protected TCP() {
            cwnd = 1;
            ssthreshold = 25.0f;
            estado = Abrv.SLOWSTART;
        }

        public string Estado {
            get {
                return estado;
            }
        }

        public float Cwnd {
            get {
                return cwnd;
            }
        }

        public TCP ShallowCopy() {
            return (TCP)this.MemberwiseClone();
        }

    }
}
