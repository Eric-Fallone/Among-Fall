using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeChecker : MonoBehaviour
{
	[SerializeField] private LayerMask layermask;
	[SerializeField] private GameObject selfGameObject;
	[SerializeField] private float _radius = 0;
	 public float Radius
	{
		get
		{
			return _radius;
		}
		set
		{
			_radius = value;
		}
	}

	private List<GameObject> foundItems = new List<GameObject>();
	private List<GameObject> thisRoundItems = new List<GameObject>();


	void LateUpdate()
	{
		LookforGameobjectsInRange();
	}

	private void LookforGameobjectsInRange()
	{
		thisRoundItems.Clear();
		Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, Radius, layermask);
		foreach (var hit in hitColliders)
		{
			//ignore self
			if(hit.gameObject == selfGameObject)
			{
				return;
			}

			//checks to see if its still range 
			if ( !foundItems.Contains(hit.gameObject) )
			{
				print("Found"+ hit.gameObject.name);
				thisRoundItems.Add(hit.gameObject);
				foundItems.Add(hit.gameObject);
			}
			else
			{
				thisRoundItems.Add(hit.gameObject);
			}
		}

		//removes not found item
		for(int i = foundItems.Count - 1; i >= 0; i--)
		{
			if(foundItems[i] == null)
			{
				foundItems.RemoveAt(i);
			}

			if ( !thisRoundItems.Contains(foundItems[i]) )
			{
				print("No longer Nearby"+ foundItems[i]);
				foundItems.RemoveAt(i);
			}
		}

	}

}
