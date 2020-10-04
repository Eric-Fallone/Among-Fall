using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInfoHolder : NetworkBehaviour
{
	[SyncVar]
	public int playerNumber;
}
