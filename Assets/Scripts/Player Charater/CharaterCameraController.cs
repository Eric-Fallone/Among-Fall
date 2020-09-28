using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using System;

public class CharaterCameraController : NetworkBehaviour
{
	[SerializeField] private PlayerInputControllerAmongFall playerInputs;

	[Header("Camera")]
	[SerializeField] private Vector2 ScrollHeighMixMax = new Vector2();
	[SerializeField] private float ScrollStartingHeight = 0f;
	[SerializeField] private Transform playerTransform = null;
	[SerializeField] private CinemachineVirtualCamera virtualCamera = null;



	private CinemachineTransposer transposer;

	public override void OnStartAuthority()
	{
		transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
		transposer.m_FollowOffset.y = ScrollStartingHeight;
		virtualCamera.gameObject.SetActive(true);
		enabled = true;

		//Binds Look to change of mouse
		playerInputs.controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
		playerInputs.controls.Player.Scroll.performed += inputValueScroll => SetScroll(inputValueScroll.ReadValue<float>());
	}

	private void SetScroll(float ScrollDir)
	{

		if (ScrollDir > 0)
		{
			//closer to floor
			ScrollDir = -1;
		}
		else
		{
			ScrollDir = 1;
		}
		transposer.m_FollowOffset.y = Mathf.Clamp(transposer.m_FollowOffset.y + ScrollDir, ScrollHeighMixMax.x, ScrollHeighMixMax.y);
	}

	private void Look(Vector2 lookAxix)
	{
		float deltaTime = Time.deltaTime;

		//transposer.m_FollowOffset.y = 6f;

	}
}
