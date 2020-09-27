using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayer : NetworkBehaviour
{

	[SyncVar]
	private string displayName = "Loading Name...";

	private bool _IsLeader;
	public bool IsLeader
	{
		set
		{
			_IsLeader = true;
		}
	}

	private NetworkManagerAmongFall _room;
	private NetworkManagerAmongFall Room
	{
		get
		{
			if (_room != null)
			{
				return _room;
			}
			return _room = NetworkManager.singleton as NetworkManagerAmongFall;
		}
	}


	public override void OnStartClient()
	{
		DontDestroyOnLoad(gameObject);

		Room.GamePlayers.Add(this);
	}

	public override void OnStopClient()
	{
		Room.GamePlayers.Remove(this);
	}
	
	[Server]
	public void SetDisplayName(string displayName)
	{
		this.displayName = displayName;
	}


}

