using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caracteristicas_Graficas : MonoBehaviour {
	protected GameObject ponto {get; set;}
	protected LineRenderer linha {get; set;}

	protected string nomeVariante {get; set;}
	protected bool visualizar {get; set;}
	protected float distancia {get; set;}

	protected List <GameObject> listaPontos = new List<GameObject>();
	protected List <LineRenderer> listaLinhas = new List<LineRenderer>();

	protected Caracteristicas_Graficas(){
		visualizar = true;
		setNomeVariante ();
	}

	public void setPonto(string ponto){
		this.ponto = (GameObject)Resources.Load(ponto);
	}

	public GameObject getPonto(){
		return ponto;
	}

	public void setLinha(string linha){
		this.linha = (LineRenderer)Resources.Load (linha, typeof(LineRenderer));
	}

	public LineRenderer getLinha(){
		return linha;
	}

	public void limparListas(){
		listaLinhas.Clear ();
		listaPontos.Clear ();
	}

	public void addlistaPontos(GameObject ponto){
		this.listaPontos.Add (ponto);
	}

	public void addlistaLinhas(LineRenderer linha){
		this.listaLinhas.Add (linha);
	}

	public List <GameObject> getlistaPontos(){
		return listaPontos;
	}

	public List <LineRenderer> getListalistaLinhas(){
		return listaLinhas;
	}

	public bool getVisualizar(){
		return visualizar;
	}

	public void setVisualizar(bool visualizar){
		this.visualizar = visualizar;
	}

	public string getNomeVariante(){
		return nomeVariante;
	}

	public void verPontos(){
		for(var i = 0 ; i < listaPontos.Count ; i ++){
			listaPontos [i].SetActive (visualizar);
			listaLinhas [i].enabled = (visualizar);
		}
	}

	public void ativarVisualizar(){
		if (!visualizar) {
			visualizar = true;
		} 

		else {
			visualizar = false;
		}
	}

	public float getDistancia(){
		return distancia;
	}

	public void setDistancia(float distancia){
		this.distancia = distancia;
	}

	public virtual void setNomeVariante(){
		
	}
}
