using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMate : GameplayRole
{
	public List<GameObject> ListOfTasks;

	public override void SetRole()
	{
		Role = RoleType.CREWMATE;
	}
}
