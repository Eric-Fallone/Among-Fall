using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OnAuthorityStartingObjectEnabler : NetworkBehaviour
{
	[SerializeField] private GameObject[] ObjectsToEnable;

	[SerializeField] private GameObject PlayerCharaterSprite;

	public override void OnStartAuthority()
	{
		foreach (GameObject gameObj in ObjectsToEnable)
		{
			gameObj.SetActive(true);
		}

		PlayerCharaterSprite.layer = 9;

	}

}
