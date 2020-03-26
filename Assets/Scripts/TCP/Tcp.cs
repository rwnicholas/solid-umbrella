public abstract class Tcp {
	protected string estado { get; set;}
	protected float cwnd { get; set;}
	protected float ssthresh { get; set;}
	protected string recebido { get; set;}

	public abstract void Run (string recebido);

}