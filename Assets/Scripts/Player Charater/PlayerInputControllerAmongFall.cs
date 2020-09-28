using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputControllerAmongFall : NetworkBehaviour
{
	private Controls _controls;
	public Controls controls
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

	private Vector2 _prevWASDInput;
	public Vector2 prevWASDInput
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


	}

	[ClientCallback]
	private void OnEnable()
	{
		controls.Enable();
	}

	[ClientCallback]
	private void OnDisable()
	{
		controls.Disable();
	}

	[Client]
	private void SetMovement(Vector2 movement)
	{
		prevWASDInput = movement;
	}

	[Client]
	private void ResetMovement()
	{
		prevWASDInput = Vector2.zero;
	}

}
