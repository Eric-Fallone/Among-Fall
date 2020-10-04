using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingList<T> : MonoBehaviour
{
	public static void PrintList(List<T> list)
	{
		string result = "List contents: ";
		foreach (var item in list)
		{
			result += item.ToString() + ", ";
		}
		Debug.Log(result);
	}
}
