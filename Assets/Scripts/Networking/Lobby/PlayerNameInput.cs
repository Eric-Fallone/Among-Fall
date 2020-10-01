using ParrelSync;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private TMP_InputField NameInputField = null;
	[SerializeField] private Button ConfirmButton = null;

	public static string DisplayName { get; private set; }

	public const string PlayerPrefsNameKey = "PlayerName";

	private void Start()
	{
		SetUpInputField();
	}

	private void SetUpInputField()
	{
		if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
		{
			return;
		}

		string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

		//dev only fix remove this if statement
		if (ClonesManager.GetArgument() != "" )
		{
			print("asd");
			NameInputField.text = ClonesManager.GetArgument();
		}
		else
		{
			NameInputField.text = defaultName;
		}

		CheckNameValid(defaultName);
	}

	public void CheckNameValid(string nameToCheck)
	{
		ConfirmButton.interactable = !string.IsNullOrEmpty(nameToCheck);
	}

	public void SavePlayerName()
	{
		DisplayName = NameInputField.text;

		//PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
	}
}
