using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tcp_Cubic : Tcp {
	public float C {get; set;}
	public float t {get; set;}
	public float Wmax {get; set;}
	public float k {get; set;}
	public float B {get; set;}

	public float W_last_max {get; set;}

	public Tcp_Cubic(){
		C = 0.4f;
		t = 0.0f;
		Wmax = 64.0f;
		B = 0.2f;
	}

	public override void Calcular(string Recebido){
		//algoritimo tcp cubic
		
		if (estado == "Slow Start" && cwnd * 2 <= ssthreshold && Recebido == "ACK") {
			cwnd = cwnd * 2;
		} 

		else if (Recebido == "ACK" && estado != "Slow Start" && (cwnd + C * Mathf.Pow ((t - k), 3) + Wmax) <= 100) {
			k = Mathf.Pow((Wmax*((B)/C)),(1.0f/3.0f));
			cwnd = cwnd + C * Mathf.Pow ((t - k), 3) + Wmax;
			Wmax = Wmax * B;
			if (cwnd >= ssthreshold - 0.01f) {
				t = t + 0.5f;
			}
			estado = "Congestion Avoidence";
		} 

		else if (Recebido == "TOUT") {
			t = 0.0f;
			Wmax = cwnd;

			if (Wmax < W_last_max) {
				W_last_max = Wmax;
				Wmax = Wmax * (2 - B) / 2;
			} 

			else {
				W_last_max = Wmax;
			}

			ssthreshold = Wmax;
			if(cwnd < Wmax){
				Wmax = Wmax * 0.9f;
			}

			cwnd = 1.0f;
			estado = "Slow Start";
		}

		else {
			t = 0.0f;
			Wmax = cwnd;

			if (Wmax < W_last_max) {
				W_last_max = Wmax;
				Wmax = Wmax * (2 - B) / 2;
			} 

			else {
				W_last_max = Wmax;
			}

			ssthreshold = Wmax;
			if(cwnd < Wmax){
				Wmax = Wmax * 0.9f;
			}

			k = Mathf.Pow ((Wmax * ((B) / C)), (1.0f / 3.0f));
			cwnd = C * Mathf.Pow ((t - k), 3) + Wmax;
			Wmax = Wmax * B;
			estado = "Fast Retransmit/Recovery";
		}
	}
	public override void setNomeVariante(){
		nomeVariante = "Cubic";
	} 
}