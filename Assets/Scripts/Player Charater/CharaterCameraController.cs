using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class CharaterCameraController : NetworkBehaviour
{
	[Header("Camera")]
	[SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
	[SerializeField] private Vector2 cameraVelocity = new Vector2(4f, .25f);

	[SerializeField] private Transform playerTransform = null;
	[SerializeField] private CinemachineVirtualCamera virtualCamera = null;

	private Controls _controls;
	private Controls controls
	{
		get
		{
			if(_controls != null)
			{
				return _controls;
			}
			return _controls = new Controls();
		}
	}


	private CinemachineTransposer transposer;

	public override void OnStartAuthority()
	{
		transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

		virtualCamera.gameObject.SetActive(true);
		enabled = true;

		controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
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

	private void Look(Vector2 lookAxix)
	{
		float deltaTime = Time.deltaTime;

		//transposer.m_FollowOffset.y = 6f;

	}
}
