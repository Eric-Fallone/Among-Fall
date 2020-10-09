using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
	[SerializeField] private GameObject KillIndicator;

	public void ShowIndicator()
	{
		KillIndicator.SetActive(true);
	}

	public void HideIndicator()
	{
		KillIndicator.SetActive(false);
	}
}
