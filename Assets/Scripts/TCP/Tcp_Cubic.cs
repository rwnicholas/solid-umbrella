using static ABRV.Abrv;
using UnityEngine;

public class Tcp_Cubic : Tcp {
    // public const float C = 0.4f;
    // public const float B = 0.2f;
    // public const float tcp_time_stamp = 1f;
    // public int tcp_friendliness;
    // public int fast_convergence;

    // public float cwnd_cnt;
    // public float Wlast_max;
    // public float epoch_start;
    // public float origin_point;
    // public float dMin;
    // public float Wtcp;
    // public float K;
    // public float ack_cnt;

    // public float t;

    public float C {get; set;}
	public float t {get; set;}
	public float Wmax {get; set;}
	public float k {get; set;}
	public float B {get; set;}

	public float W_last_max {get; set;}

    public Tcp_Cubic() {
        // tcp_friendliness = 1;
        // fast_convergence = 1;
        // cubic_reset();
        C = 0.4f;
		t = 0.0f;
		Wmax = 64.0f;
		B = 0.2f;
        nomeVariante = "Cubic";
    }

    // public void cubic_reset() {
    //     Wlast_max = epoch_start = origin_point = Wtcp = K = ack_cnt = cwnd_cnt = 0;
    //     dMin = 100; // Já que por ser simulado, é constante;
    // }

    // public float cubic_tcp_friendliness() {
    //     float max_cnt = 0;

    //     Wtcp = Wtcp + (((3 * B) / (2 - B)) * (ack_cnt / cwnd));
    //     ack_cnt = 0;
    //     if (Wtcp > cwnd) {
    //         max_cnt = cwnd / (Wtcp - cwnd);
    //     }
    //     return max_cnt;
    // }

    // public float cubic_update() {
    //     float target;
    //     float cnt;

    //     ack_cnt++;
    //     if (epoch_start <= 0) {
    //         epoch_start = tcp_time_stamp;
    //         if (cwnd < Wlast_max) {
    //             K = Mathf.Pow((Wlast_max - cwnd) / C, 1f / 3f);
    //             origin_point = Wlast_max;
    //         } else {
    //             K = 0;
    //             origin_point = cwnd;
    //         }
    //         ack_cnt = 1;
    //         Wtcp = cwnd;
    //     }
    //     t = tcp_time_stamp + dMin - epoch_start;
    //     target = origin_point + (C * (Mathf.Pow(t, 3) - 3 * Mathf.Pow(t, 2) * K + 3 * t * Mathf.Pow(K, 2) - Mathf.Pow(K, 3)));
    //     if (target > cwnd) {
    //         cnt = (cwnd / (target - cwnd));
    //     } else {
    //         cnt = 100 * cwnd;
    //     }
    //     if (tcp_friendliness == 1) {
    //         float max_cnt = cubic_tcp_friendliness();
    //         if (cnt > max_cnt) {
    //             cnt = max_cnt;
    //         }
    //     }

    //     return cnt;
    // }

    public override Tcp Init() {
        return new Tcp_Cubic();
    }

    public override float Run(string recebido) {
        // float cnt;
        
        // if (recebido == ACK) {
        //     if (cwnd <= ssthreshold) {
        //         cwnd++;
        //     } else {
        //         cnt = cubic_update();
        //         if (cwnd_cnt > cnt) {
        //             cwnd++; cwnd_cnt = 0;
        //         } else {
        //             cwnd_cnt++;
        //         }
        //     }
        //     estado = SLOWSTART;
        // } else if (recebido == TACK) {
        //     epoch_start = 0;
        //     if (cwnd < Wlast_max && fast_convergence == 1) {
        //         Wlast_max = cwnd * ((2 - B) / 2);
        //     } else {
        //         Wlast_max = cwnd;
        //     }
        //     ssthreshold = cwnd = cwnd * (1 - B);
        //     if (estado == SLOWSTART) {
        //         estado = C_AVOIDENCE;
        //     } else if (estado == C_AVOIDENCE) {
        //         estado = C_AVOIDENCE;
        //     }
        // } else if (recebido == TOUT) {
        //     cubic_reset();
        //     if (estado == SLOWSTART) {
        //         estado = SLOWSTART;
        //     } else if (estado == C_AVOIDENCE) {
        //         estado = SLOWSTART;
        //     }
        // }
        if (estado == SLOWSTART && cwnd * 2 <= ssthreshold && recebido == ACK) {
			cwnd = cwnd * 2;
		} 

		else if (recebido == ACK && estado != SLOWSTART && (cwnd + C * Mathf.Pow ((t - k), 3) + Wmax) <= 100) {
			k = Mathf.Pow((Wmax*((B)/C)),(1.0f/3.0f));
			cwnd = cwnd + C * Mathf.Pow ((t - k), 3) + Wmax;
			Wmax = Wmax * B;
			if (cwnd >= ssthreshold - 0.01f) {
				t = t + 0.5f;
			}
			estado = C_AVOIDENCE;
		} 

		else if (recebido == TOUT) {
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
			estado = SLOWSTART;
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
			estado = FAST_RET;
		}

        return cwnd;
    }
}
