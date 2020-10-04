using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class IntRange
{
	public int min;
	public int max;
	[Tooltip("If true, GetValue will always return the same value")]
	public bool DoesKeepValue;

	private int Value = 0;
	private bool isSet = false;

	public IntRange(int minI, int maxI, bool keepVal)
	{
		min = minI;
		max = maxI;
		DoesKeepValue = keepVal;
	}

	public int GetValue()
	{
		if (min == max)
		{
			return min;
		}

		if (isSet == false || DoesKeepValue == false)
		{
			Value = UnityEngine.Random.Range(min, max);
			isSet = true;
		}

		return Value;
	}


	public static List<int> UniqueRandomIntsInRange(int min, int max, int amount)
	{
		List<int> usedValues = new List<int>();
		List<int> output = new List<int>();
		int val;

		// case of not enough numbers for unique numbers
		if (max - min < amount)
		{
			for (int i = 0; i < max - min; i++)
			{
				val = UnityEngine.Random.Range(min, max);

				while (usedValues.Contains(val))
				{
					val = UnityEngine.Random.Range(min, max);
				}
				usedValues.Add(val);
				output.Add(val);
			}
			return output;
		}


		for (int i = 0; i < amount; i++)
		{
			val = UnityEngine.Random.Range(min, max);

			while (usedValues.Contains(val))
			{
				val = UnityEngine.Random.Range(min, max);
			}
			usedValues.Add(val);
			output.Add(val);
		}

		return output;
	}

}
