using System;
public class Player : Entity
{
	public string globalId;

	public Player(string id, string globalId) : base(id)
	{
		this.globalId = globalId;
	}
}

