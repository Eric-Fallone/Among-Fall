using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInfoHolder : NetworkBehaviour
{
	[SyncVar]
	public int playerNumber;

	public GameObject ImposterGameObj;

	public GameObject CrewGameObj;
	public CrewMate crewMate;
	public GameObject KillableGameObj;


	public void SetImposter()
	{
		ImposterGameObj.SetActive(true);
	}

	public void SetCrewMateLocalPlayer(List<GameObject> tasks)
	{
		CrewGameObj.SetActive(true);
		crewMate.ListOfTasks = tasks;
		KillableGameObj.SetActive(true);
	}

	public void SetCrewMateOtherPlayer()
	{
		//CrewGameObj.SetActive(true);
		KillableGameObj.SetActive(true);
	}

	public void ResetRole()
	{
		ImposterGameObj.SetActive(false);
		CrewGameObj.SetActive(false);
		KillableGameObj.SetActive(false);
	}
}
