using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerAmongFall : NetworkManager
{
	[SerializeField] private int MinPlayers = 2;
	[Scene] [SerializeField] private string MenuScene = string.Empty;

	[Header("Room")]
	[SerializeField] private NetworkRoomPlayerLobby RoomPlayerPrefab = null;

	public static event Action OnClientConnected;
	public static event Action OnClientDisconnected;

	public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

	#region Server

	public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();


	public override void OnStopServer()
	{
		RoomPlayers.Clear();
	}

	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		if ( "Assets/Scenes/" + SceneManager.GetActiveScene().name +".unity" == MenuScene)
		{
			bool isLeader = RoomPlayers.Count == 0;

			NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(RoomPlayerPrefab);

			roomPlayerInstance.IsLeader = isLeader;

			NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
		}
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		if (numPlayers > maxConnections)
		{
			conn.Disconnect();
			return;
		}
		if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" != MenuScene)
		{
			print("Game is in session while a player connected");
			return;
		}
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		if (conn.identity != null)
		{
			var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
			RoomPlayers.Remove(player);


			NotifyPlayersOfReadyState();
		}

		base.OnServerDisconnect(conn);
	}

	public void NotifyPlayersOfReadyState()
	{
		foreach(var player in RoomPlayers)
		{
			player.HandleReadyToStart(IsReadyToStart());
		}
	}

	private bool IsReadyToStart()
	{
		if(numPlayers < MinPlayers) {
			return false;
		}

		foreach(var player in RoomPlayers)
		{
			if (!player.IsReady)
			{
				return false;
			}
		}

		return true;
	}

	#endregion

	#region Client

	public override void OnStartClient()
	{
		var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

		foreach (var prefab in spawnablePrefabs)
		{
			ClientScene.RegisterPrefab(prefab);
		}
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
		OnClientConnected?.Invoke();
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);

		OnClientDisconnected?.Invoke();
	}
	#endregion
}
