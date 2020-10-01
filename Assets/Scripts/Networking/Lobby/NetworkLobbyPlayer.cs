using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkLobbyPlayer : NetworkBehaviour
{
	[SerializeField] private GameObject LobbyUI = null;
	[SerializeField] private TMP_Text[] playerNamesText = new TMP_Text[10];
	[SerializeField] private TMP_Text[] playerReadyText = new TMP_Text[10];
	[SerializeField] private Button startGameButton = null;

	[SyncVar(hook = nameof(HandleDisplayNameChanged))]
	public string DisplayName = "Loading Name...";

	[SyncVar(hook = nameof(HandleReadyStatusChanged))]
	public bool IsReady = false;


	private bool _IsLeader; 
	public bool IsLeader
	{
		set
		{
			_IsLeader = true;
			startGameButton.gameObject.SetActive(true);
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

	public override void OnStartAuthority()
	{
		CmdSetDisplayName(PlayerNameInput.DisplayName);

		LobbyUI.SetActive(true);
	}

	public override void OnStartClient()
	{
		Room.RoomPlayers.Add(this);

		UpdateDisplay();
	}

	public override void OnStopClient()
	{
		Room.RoomPlayers.Remove(this);

		UpdateDisplay();
	}

	public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

	public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();



	private void UpdateDisplay()
	{
		if (!hasAuthority)
		{
			foreach(var player in Room.RoomPlayers)
			{
				if (player.hasAuthority)
				{
					player.UpdateDisplay();
					break;
				}
			}

			return;
		}

		// can probally combine these two for loops into one

		for (int i = 0; i < playerNamesText.Length; i++)
		{
			playerNamesText[i].text = "Waiting For Player...";
			playerReadyText[i].text = string.Empty;
		}

		for (int i = 0; i < Room.RoomPlayers.Count ; i++)
		{
			playerNamesText[i].text = Room.RoomPlayers[i].DisplayName;
			playerReadyText[i].text = Room.RoomPlayers[i].IsReady ?
				"Ready" :
				"Not Ready";
		}
	}

	public void HandleReadyToStart(bool readyToStart)
	{
		if (!_IsLeader)
		{
			return;
		}

		startGameButton.interactable = readyToStart;
	}

	[Command]
	private void CmdSetDisplayName(string displayNameIn)
	{
		DisplayName = displayNameIn;
	}

	[Command]
	public void CmdReadyUp()
	{
		IsReady = !IsReady;

		Room.NotifyPlayersOfReadyState();
	}

	[Command]
	public void CmdStartGame()
	{
		if(Room.RoomPlayers[0].connectionToClient != connectionToClient)
		{
			return;
		}

		Room.StartGame();
	}

	[Client]
	public void OpenFriendsListToInvite()
	{
		SteamLobby.OpenFriendsListInvite();
	}

	[Client]
	public void StopClient()
	{
		SteamLobby.LeaveSteamLobby();
	}
}
