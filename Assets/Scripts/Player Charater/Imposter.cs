using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imposter : GameplayRole
{
	[SerializeField] private LayerMask layermasks; 
	//[SerializeField] GameObject objectsInRange;

	private Collider[] foundKillable;

	private List<Killable> killablesFound = new List<Killable>();
	private List<Killable> killablesPrev = new List<Killable>();

	private void Update()
	{
		killablesFound.Clear();

		foundKillable = Physics.OverlapSphere(this.gameObject.transform.position, NetworkManagerAmongFall.ViewDistance/2, layermasks);

		foreach (Collider col in foundKillable)
		{
			Killable killComponent = col.gameObject.GetComponent<Killable>();
			if(killComponent != null && !killablesFound.Contains(killComponent) && col.gameObject.transform.parent != this.transform.parent)
			{
				if ( !killablesPrev.Contains(killComponent) )
				{
					KillableInRange();
					killablesPrev.Add(killComponent);
				}

				killablesFound.Add(killComponent);
			}
		}

		for(int i = killablesPrev.Count - 1 ; i >= 0; i--)
		{
			if ( !killablesFound.Contains(killablesPrev[i]) )
			{
				KillableOutOfRange();
				killablesPrev.RemoveAt(i);
			}
		}
	}

	private void KillableInRange()
	{
		print("Found");
	}

	private void KillableOutOfRange()
	{
		print("Leaving Found");
	}

}
