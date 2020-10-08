using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TaskManager : NetworkBehaviour
{

	private List<Crewmate> CrewMateTaskLists = new List<Crewmate>();
	private List<int> imposterIndexes = new List<int>();

	private NetworkManagerAmongFall _room;
	private NetworkManagerAmongFall room
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


	[Server]
	public void createRoles()
	{
		//clean up last round
		CrewMateTaskLists.Clear();
		imposterIndexes.Clear();



		imposterIndexes = IntRange.UniqueRandomIntsInRange(0, room.GamePlayers.Count, NetworkManagerAmongFall.NumOfImposters);

		DebuggingList<int>.PrintList(imposterIndexes);

		for (int i = 0; i < room.GamePlayers.Count - NetworkManagerAmongFall.NumOfImposters; i++)
		{

		}

		for (int i = 0; i < room.GamePlayers.Count; i++)
		{
			if (imposterIndexes.Contains(i))
			{

			}
		}

	}

	public void GetRole(int index)
	{

	}

}
