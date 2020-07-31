public abstract class Tcp {
	protected string estado { get; set;}
	protected float cwnd { get; set;} //Janela de congestionamento
	protected float ssthreshold { get; set;} //Limiar de partida lenta
	protected string recebido { get; set;}
	public string nomeVariante { get; set;} //nome da variante do tcp (tahoe, cubic, etc)
	

	public abstract float Run (string recebido);
    public abstract Tcp Init();

    protected Tcp() {
		cwnd = 1;
		ssthreshold = 25.0f;
		estado = "Partida Lenta";
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
}