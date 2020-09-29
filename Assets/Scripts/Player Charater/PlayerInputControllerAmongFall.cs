using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerInputControllerAmongFall : NetworkBehaviour
{
	private static readonly IDictionary<string, int> mapPlayerStates = new Dictionary<string, int>();

	private static Controls _controls;
	public static Controls controls
	{
		get
		{
			if (_controls != null)
			{
				return _controls;
			}
			return _controls = new Controls();
		}
	}

	private static Vector2 _prevWASDInput;
	public static Vector2 prevWASDInput
	{
		get
		{
			return _prevWASDInput;
		}
		private set
		{
			_prevWASDInput = value;
		}
	}

	public override void OnStartAuthority()
	{
		enabled = true;
		//bind movement functions
		controls.Player.Move.performed += inputValueWASD => SetMovement(inputValueWASD.ReadValue<Vector2>());
		controls.Player.Move.canceled += ctx => ResetMovement();

		//scroll inputs found on the camera script; 
	}


	[ClientCallback]
	private void OnDestroy()
	{
		_controls = null;
	}
	[Client]
	public static void AddBlock(string mapKey)
	{
		mapPlayerStates.TryGetValue(mapKey, out int value);
		mapPlayerStates[mapKey] = value + 1;

		UpdateMapState(mapKey);
	}

	[Client]
	public static void RemoveBlock(string mapKey)
	{
		mapPlayerStates.TryGetValue(mapKey, out int value);
		mapPlayerStates[mapKey] = Mathf.Max(value - 1, 0);

		UpdateMapState(mapKey);
	}

	[Client]
	private static void UpdateMapState(string mapKey)
	{
		int value = mapPlayerStates[mapKey];

		if(value > 0)
		{
			controls.asset.FindActionMap(mapKey).Disable();
			return;
		}
		print("Enabled");
		controls.asset.FindActionMap(mapKey).Enable();
	}


	[Client]
	private static void SetMovement(Vector2 movement)
	{
		prevWASDInput = movement;
	}

	[Client]
	private static void ResetMovement()
	{
		prevWASDInput = Vector2.zero;
	}

}
