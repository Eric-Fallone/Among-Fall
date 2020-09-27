using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
	private void Awake()
	{
		PlayerSpawnSystem.AddSpawnPoint(this.transform);
	}

	private void OnDestroy()
	{
		PlayerSpawnSystem.RemoveSpawnPoint(this.transform);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position, 1f);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(this.transform.position, this.transform.position + transform.forward * 2 );
	}
}
