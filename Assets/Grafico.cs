using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grafico : MonoBehaviour {
	public Transform LocalReferencia;

	GameObject _ponto = null;
	LineRenderer _linha = null;

	Tcp_Tahoe tahoe = new Tcp_Tahoe ();
	Tcp_Reno reno = new Tcp_Reno ();
	Tcp_Cubic cubic = new Tcp_Cubic ();

	List <Tcp> tcps = new List<Tcp>();
	List <Text> infos = new List<Text>();
	List <Text> nomes = new List<Text>();

	string recebido = "ACK";

	float tempo = 0.5f;

	public Text infoTcp1;
	public Text infoTcp2;
	public Text infoTcp3;

	public Text NomeTcp1;
	public Text NomeTcp2;
	public Text NomeTcp3;

	bool pausar = false;

	// Use this for initialization
	void Start () {
		tcps.Add (tahoe);
		tcps.Add (reno);
		tcps.Add (cubic);

		infos.Add (infoTcp1);
		infos.Add (infoTcp2);
		infos.Add (infoTcp3);

		nomes.Add(NomeTcp1);
		nomes.Add(NomeTcp2);
		nomes.Add(NomeTcp3);

		for(var i = 0 ; i < tcps.Count; i ++){
			tcps[i].setPonto ("Ponto" + (i+1));
			tcps[i].setLinha ("Linha" + (i+1));
			nomes[i].text = tcps[i].getNomeVariante();
		}
	
		AtualizarTextos ();

		pausar = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!pausar) {
			tempo -= Time.deltaTime;
			bool sincronia = false;

			if (tempo <= 0.0f) {
				float menor = tcps [0].getDistancia ();
				for (var i = 1; i < tcps.Count; i++) {
					if (tcps [0].getDistancia () == tcps [i].getDistancia ()) {
						sincronia = true;
					}
					if (tcps [i].getDistancia () < menor) {
						menor = tcps [i].getDistancia ();
					}
				}

				if (tcps [0].getDistancia () >= (60 * 5) && sincronia) {
					var gameObjects = GameObject.FindGameObjectsWithTag ("d");

					for (var i = 0; i < gameObjects.Length; i++) {
						Destroy (gameObjects [i]);
					}

					for (var i = 0; i < tcps.Count; i++) {
						LimparListas (tcps [i]);
						tcps [i].setDistancia (0.0f);
					}
				} else {
					
					for (var i = 0; i < tcps.Count; i++) {
						if (tcps [i].getDistancia () == menor || recebido != "ACK") {
							Plotar (tcps [i], recebido);
							tcps [i].verPontos ();
						}
					}

					if (recebido != "ACK") {
						pausar = true;
						if (sincronia) {
							recebido = "ACK";
						}
					} 
				}

				tempo = 0.5f;

				AtualizarTextos ();
			}
		}
	}

	public void AtualizarTextos(){
		for(var i = 0; i < infos.Count; i++){
			if(tcps[i].getVisualizar()){
				infos [i].text = "TCP " + tcps[i].getNomeVariante() + "\nCwnd = " + Mathf.Round(tcps[i].getCwnd()).ToString() + "\nEstado = " + tcps[i].getEstado();
			}
			else{
				infos [i].text = "";
			}
		}
	}

	public void Plotar(Tcp tcp, string recebido){
		var pontos = new Vector3[2];

		pontos [0] = new Vector3 (LocalReferencia.transform.position.x + tcp.getDistancia(), LocalReferencia.transform.position.y + tcp.getCwnd(), LocalReferencia.transform.position.z);

		tcp.Calcular(recebido);

		if (recebido != "ACK" || tcp.getEstado() == "Fast Retransmit" ||  tcp.getEstado() == "Fast Retransmit/Recovery") {
			pontos [1] = new Vector3 (LocalReferencia.transform.position.x + tcp.getDistancia(), LocalReferencia.transform.position.y + tcp.getCwnd (), LocalReferencia.transform.position.z);
		} 

		else {
			pontos [1] = new Vector3 (LocalReferencia.transform.position.x + tcp.getDistancia() + 5, LocalReferencia.transform.position.y + tcp.getCwnd (), LocalReferencia.transform.position.z);
			tcp.setDistancia (tcp.getDistancia() + 5);
		}

		_linha = Instantiate(tcp.getLinha(), pontos [1], tcp.getLinha().transform.rotation) as LineRenderer;
		_linha.transform.SetParent (this.transform);
		_linha.SetPositions (pontos);
		tcp.addlistaLinhas(_linha);

		_ponto = Instantiate(tcp.getPonto() , pontos [1] , tcp.getPonto().transform.rotation) as GameObject;
		_ponto.transform.SetParent (this.transform);
		_ponto.transform.localScale = new Vector3(1, 1, 1);
		tcp.addlistaPontos(_ponto);
	}

	public void visualizarTcp(int n){
		tcps [n].ativarVisualizar ();
		tcps [n].verPontos ();
		AtualizarTextos();
	}

	public void StartPause(){
		if (pausar) {
			pausar = false;
		} 

		else {
			pausar = true;
		}
	}

	public void LimparListas(Tcp tcp){
		tcp.limparListas ();
	}

	public void Timeout(){
		recebido = "TOUT";
	}

	public void Acks_duplicados(){
		recebido = "TACK";
	}

	public void Reiniciar(){
		Application.LoadLevel(0);
	}
}
