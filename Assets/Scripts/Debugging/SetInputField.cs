using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetInputField : MonoBehaviour
{
	[SerializeField] private TMP_InputField NameInputField = null;
	public string toSet = "";
	// Start is called before the first frame update
	void Start()
    {
		NameInputField.text = toSet;
    }
}
