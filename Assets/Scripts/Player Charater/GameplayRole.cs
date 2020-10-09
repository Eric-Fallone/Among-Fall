using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum RoleType {CREWMATE, IMPOSTER };

public abstract class GameplayRole : NetworkBehaviour
{
	public GameObject RoleUI;

	public RoleType Role;

	private void OnEnable()
	{
		if (isLocalPlayer)
		{
			RoleUI.SetActive(true);
		}
	}

	private void OnDisable()
	{
		if (isLocalPlayer)
		{
			RoleUI.SetActive(false);
		}
	}

	public virtual void SetRole()
	{

	}

}
