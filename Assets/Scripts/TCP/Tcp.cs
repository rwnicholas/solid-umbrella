public abstract class Tcp {
	protected string estado { get; set;}
	protected int cwnd { get; set;} //Janela de congestionamento
	protected float ssthreshold { get; set;} //Limiar de partida lenta
	protected string recebido { get; set;}
	public string nomeVariante { get; set;}
	

	public abstract int Run (string recebido);

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

	public int Cwnd {
		get {
			return cwnd;
		}
	}
}