using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class CharaterCameraController : NetworkBehaviour
{
	[SerializeField] private PlayerInputControllerAmongFall playerInputs;

	[Header("Camera")]

	[SerializeField] private Transform playerTransform = null;
	[SerializeField] private CinemachineVirtualCamera virtualCamera = null;



	private CinemachineTransposer transposer;

	public override void OnStartAuthority()
	{
		transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

		virtualCamera.gameObject.SetActive(true);
		enabled = true;

		//Binds Look to change of mouse
		playerInputs.controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
	}

	private void Look(Vector2 lookAxix)
	{
		float deltaTime = Time.deltaTime;

		//transposer.m_FollowOffset.y = 6f;

	}
}
