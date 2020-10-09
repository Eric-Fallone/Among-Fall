using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TaskManager : NetworkBehaviour
{
	public static TaskManager instance; 

	[Header("Tasks Lists")]
	[SerializeField] private GameObject[] TaskCommunityMasterList;
	[SerializeField] private GameObject[] TaskVisualMasterList;
	[SerializeField] private GameObject[] TaskIncrementalMasterList;

	[SerializeField] private List<int> CommonTaskIndex;
	[SerializeField] private GameObject[] TaskCommonMasterList;
	[SerializeField] private GameObject[] TaskLongMasterList;
	[SerializeField] private GameObject[] TaskShortMasterList;


	private List<List<GameObject>> CrewMateTaskLists = new List<List<GameObject>>();
	private List<int> imposterIndexes = new List<int>();
	private List<int> taskIndexes;

	private List<int> PlayerIndexRoles = new List<int>(); 

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

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(this.gameObject);
			print("Warning two Task mangagers");
		}
	}


	[Server]
	public void createRoles()
	{
		//clean up last round
		CrewMateTaskLists.Clear();
		PlayerIndexRoles.Clear();
		imposterIndexes.Clear();

		imposterIndexes = IntRange.UniqueRandomIntsInRange(0, room.GamePlayers.Count, room.NumOfImposters);

		CommonTaskIndex = IntRange.UniqueRandomIntsInRange(0, TaskCommonMasterList.Length, room.NumOfCommonTasks);


		//create task lists for everycrew member tasks
		for (int i = 0; i < room.GamePlayers.Count; i++)
		{

			//Create a new task list
			CrewMateTaskLists.Add(new List<GameObject>());


			//common

			foreach (int commonIndex in CommonTaskIndex)
			{
				CrewMateTaskLists[i].Add( TaskCommonMasterList[commonIndex] );
			}

			//Long


			List<int> taskIndex = IntRange.UniqueRandomIntsInRange(0, TaskLongMasterList.Length, room.NumOfLongTasks);
			foreach (int c in taskIndex)
			{
				CrewMateTaskLists[i].Add(TaskLongMasterList[c]);
			}

			//Short

			taskIndex = IntRange.UniqueRandomIntsInRange(0, TaskShortMasterList.Length, room.NumOfShortTasks);
			foreach (int c in taskIndex)
			{
				CrewMateTaskLists[i].Add(TaskShortMasterList[c]);
			}

			DebuggingList<GameObject>.PrintList(CrewMateTaskLists[i]);
		}

		//add bonus tasks


		//assign player index to roles
		int CrewMateTaskListIndex = 0;
		for (int i = 0; i < room.GamePlayers.Count; i++)
		{
			if (imposterIndexes.Contains(i))
			{
				//imposter
				PlayerIndexRoles.Add(-1);
			}
			else
			{
				//crewmate
				PlayerIndexRoles.Add(CrewMateTaskListIndex);
				CrewMateTaskListIndex++;
			}

		}

	}

	public List<GameObject> GetRole(int index)
	{
		if (PlayerIndexRoles[index] == -1)
		{
			return null;
		}
		return CrewMateTaskLists[ PlayerIndexRoles[index] ];
	}

}
