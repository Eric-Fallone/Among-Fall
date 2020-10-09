using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using System;

public class RoundStarter : NetworkBehaviour
{
	private TaskManager _taskManager;
	private TaskManager taskManager
	{
		get
		{
			if(_taskManager != null)
			{
				return _taskManager;
			}
			return _taskManager = TaskManager.instance;
		}
	}
	[SerializeField] private Animator animator = null;

	private NetworkManagerAmongFall _room;
	private NetworkManagerAmongFall room
	{
		get
		{
			if(_room != null)
			{
				return _room;
			}
			return _room = NetworkManager.singleton as NetworkManagerAmongFall;
		}
	}

	public void CountDownEnded()
	{
		animator.enabled = false;
	}

	#region Server

	public override void OnStartServer()
	{
		NetworkManagerAmongFall.OnServerStopped += CleanUpServer;
		NetworkManagerAmongFall.OnServerReadied += CheckToStartRound;
	}

	[ServerCallback]
	private void OnDestroy()
	{
		CleanUpServer();
	}

	[Server]
	private void CleanUpServer()
	{
		NetworkManagerAmongFall.OnServerStopped -= CleanUpServer;
		NetworkManagerAmongFall.OnServerReadied -= CheckToStartRound;
	}

	[ServerCallback]
	public void StartRound()
	{
		RpcStartRound();
	}

	[Server]
	private void CheckToStartRound(NetworkConnection conn)
	{
		if(room.GamePlayers.Count( x => x.connectionToClient.isReady ) != room.GamePlayers.Count)
		{
			return;
		}

		//start game

		CreateRoles();

		RpcGetRole();

		//start the count downs to start the game first server then client
		animator.enabled = true;
		RpcStartCountDown();
	}

	[Server]
	private void CreateRoles()
	{
		taskManager.createRoles();
	}

	#endregion

	#region Client

	[ClientRpc]
	private void RpcStartCountDown()
	{
		animator.enabled = true;
	}

	[ClientRpc]
	private void RpcStartRound()
	{
		PlayerInputControllerAmongFall.RemoveBlock(PlayerInputMapNames.Player);
	}

	[ClientRpc]
	private void RpcGetRole()
	{
		//get player 
		foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("Player"))
		{
			PlayerInfoHolder playerInfo = gameObj.GetComponent<PlayerInfoHolder>();

			//if player set up role in player prefab
			if(playerInfo != null && gameObj.GetComponent<NetworkTransform>().hasAuthority)
			{

				List<GameObject> tasksToGiveToPlayer = taskManager.GetRole(playerInfo.playerNumber);

				if (tasksToGiveToPlayer == null)
				{
					//imposter
					playerInfo.SetImposter();
				}
				else
				{
					playerInfo.SetCrewMateLocalPlayer(tasksToGiveToPlayer);
				}
				
			}
			//set all the other players up

			if (playerInfo != null && !gameObj.GetComponent<NetworkTransform>().hasAuthority)
			{
				List<GameObject> tasksToGiveToPlayer = taskManager.GetRole(playerInfo.playerNumber);

				if (tasksToGiveToPlayer == null)
				{
					//imposter
					playerInfo.SetImposter();
				}
				else
				{
					playerInfo.SetCrewMateOtherPlayer();
				}

			}

		}
	}

	#endregion
}
