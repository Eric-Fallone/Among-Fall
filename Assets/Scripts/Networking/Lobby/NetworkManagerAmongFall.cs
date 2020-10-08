using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerAmongFall : NetworkManager
{
	[Header("Game")]
	[SerializeField] private int MinPlayers = 2;
	[SerializeField] public static float ViewDistance = 7f;

	public static int NumOfImposters = 2;
		
	[Header("Tasks Per Lobby")]
	 public int NumOfCommunityTasks = 1;
	 public int NumOfVisualTasks = 1;
	 public int NumOfIncrementalTasks = 1;

	[Header("Tasks Per Person")]
	public int NumOfCommonTasks = 1;
	public int NumOfLongTasks = 2;
	public int NumOfShortTasks = 5;

	[Header("Scenes")]
	[Scene] [SerializeField] private string MenuScene = string.Empty;
	[Scene] [SerializeField] private string GameSceneDruid = string.Empty;

	[Header("Room")]
	[SerializeField] private NetworkLobbyPlayer RoomPlayerPrefab = null;

	[Header("Game player prefab")]
	[SerializeField] private NetworkGamePlayer GamePlayerPrefab = null;

	[SerializeField] private GameObject PlayerSpawnSystem = null;
	[SerializeField] private GameObject RoundStarter = null;

	public static event Action OnClientConnected;
	public static event Action OnClientDisconnected;
	public static event Action<NetworkConnection> OnServerReadied;
	public static event Action OnServerStopped;

	public List<NetworkLobbyPlayer> RoomPlayers { get; } = new List<NetworkLobbyPlayer>();

	public List<NetworkGamePlayer> GamePlayers { get; } = new List<NetworkGamePlayer>();
	public List<NetworkGamePlayer> GameSpector { get; } = new List<NetworkGamePlayer>();



	#region Server

	public override void OnStartServer()
	{
		spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

		print(this.networkAddress);
	}

	public override void OnStartClient()
	{
		var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

		foreach (var prefab in spawnablePrefabs)
		{
			ClientScene.RegisterPrefab(prefab);
		}
	}


	public override void OnStopServer()
	{
		OnServerStopped?.Invoke();

		RoomPlayers.Clear();
		GamePlayers.Clear();
	}

	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		if ( "Assets/Scenes/" + SceneManager.GetActiveScene().name +".unity" == MenuScene)
		{
			bool isLeader = RoomPlayers.Count == 0;

			NetworkLobbyPlayer roomPlayerInstance = Instantiate(RoomPlayerPrefab);

			roomPlayerInstance.IsLeader = isLeader;

			NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
		}
		base.OnServerAddPlayer(conn);
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
			var player = conn.identity.GetComponent<NetworkLobbyPlayer>();
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

	public void StartGame()
	{
		if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == MenuScene)
		{
			if( !IsReadyToStart() )
			{
				return;
			}
			ServerChangeScene(GameSceneDruid);
		}
	}

	public override void ServerChangeScene(string newSceneName)
	{
		if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == MenuScene && newSceneName.StartsWith("Assets/Scenes/Level"))
		{
			for(int i = RoomPlayers.Count - 1; i >= 0; i--)
			{
				var conn = RoomPlayers[i].connectionToClient;
				var gamePlayerInstance = Instantiate(GamePlayerPrefab);
				//add to list?

				gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
				
				NetworkServer.Destroy(conn.identity.gameObject);

				NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, true);
			}
		}

		base.ServerChangeScene(newSceneName);
	}

	public override void OnServerReady(NetworkConnection conn)
	{
		base.OnServerReady(conn);

		OnServerReadied?.Invoke(conn);
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

	public override void OnServerSceneChanged(string newSceneName)
	{
		if (newSceneName.StartsWith("Assets/Scenes/Level"))
		{
			GameObject playerSpawnSystemInstance = Instantiate(PlayerSpawnSystem);
			NetworkServer.Spawn(playerSpawnSystemInstance);

			GameObject roundStarterInstance = Instantiate(RoundStarter);
			NetworkServer.Spawn(roundStarterInstance);
		}
	}


	#endregion

	public static int GetNumberOfImposters()
	{
		return NumOfImposters;
	}
}
