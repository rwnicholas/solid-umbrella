using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tcp : Caracteristicas_Graficas {
	
	protected string estado {get; set;}
	protected string recebido {get; set;}
	protected float cwnd {get; set;}
	protected float ssthreshold {get; set;}

	protected Tcp(){
		cwnd = 1.0f;
		ssthreshold = 64.0f;
		estado = "Slow Start";
	}

	public float getCwnd(){
		return cwnd;
	}

	public string getEstado(){
		return estado;
	}

	public virtual void Calcular(string Recebido){

	}
}