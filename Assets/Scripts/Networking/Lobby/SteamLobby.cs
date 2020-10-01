using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LobbyType {PUBLIC,PRIVATE};

public class SteamLobby : MonoBehaviour
{
	protected Callback<LobbyCreated_t> lobbyCreated;
	protected Callback<LobbyEnter_t> lobbyEntered;

	protected Callback<LobbyMatchList_t> FindLobbies;

	protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;

	private const string HostAddressKey = "HostAddress";

	private static CSteamID LobbyId;

	[SerializeField] private GameObject ButtonsHolder = null;



	private NetworkManagerAmongFall networkManager;


    // Start is called before the first frame update
    void Start()
    {
		networkManager = GetComponent<NetworkManagerAmongFall>();
		if (!SteamManager.Initialized)
		{
			return;
		}
		lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
		lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

		FindLobbies = Callback<LobbyMatchList_t>.Create(OnFindPublicLobbies);

		gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);


	}

	public void startPrivateLobby()
	{
		startLobby(LobbyType.PRIVATE);
	}

	public void startPublicLobby()
	{
		startLobby(LobbyType.PUBLIC);
	}

	private void startLobby(LobbyType lobbyType)
	{
		ButtonsHolder.SetActive(false);
		if (lobbyType == LobbyType.PRIVATE)
		{
			SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
		}
		if (lobbyType == LobbyType.PUBLIC)
		{
			SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, networkManager.maxConnections);
		}
	}

	public void FindPublicLobby()
	{
		SteamMatchmaking.AddRequestLobbyListResultCountFilter(10);
		SteamMatchmaking.RequestLobbyList();
	}

	private void OnFindPublicLobbies(LobbyMatchList_t callback)
	{
		print(callback.m_nLobbiesMatching);
		if (callback.m_nLobbiesMatching != 0)
		{
			print(callback.m_nLobbiesMatching);
			print(SteamMatchmaking.GetLobbyByIndex( (int) callback.m_nLobbiesMatching));
		}
	}

	public static void OpenFriendsListInvite()
	{
		SteamFriends.ActivateGameOverlayInviteDialog(LobbyId);	
	}

	public static void LeaveSteamLobby()
	{
		SteamMatchmaking.LeaveLobby(LobbyId);
	}


	private void OnLobbyCreated(LobbyCreated_t callback)
	{
		if(callback.m_eResult != EResult.k_EResultOK)
		{
			ButtonsHolder.SetActive(true);
			return;
		}

		networkManager.StartHost();
		SteamMatchmaking.SetLobbyData(
			new CSteamID(callback.m_ulSteamIDLobby), 
			HostAddressKey, 
			SteamUser.GetSteamID().ToString()
			);

	}

	private void OnLobbyEntered(LobbyEnter_t callback)
	{
		if (NetworkServer.active)
		{
			return;
		}

		LobbyId = new CSteamID(callback.m_ulSteamIDLobby);

		string hostAddress = SteamMatchmaking.GetLobbyData(
			LobbyId,
			HostAddressKey
			);

		networkManager.networkAddress = hostAddress;
		networkManager.StartClient();

		ButtonsHolder.SetActive(false);
	}

	private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
	{
		SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
	}
}
