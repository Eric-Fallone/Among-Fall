using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharaterControllerMovement : NetworkBehaviour
{
	[Header("Customizable Variables")]
	[SerializeField] private float speed = 10f;

	private float inputSqrMagnitude;
	private Vector3 movePosHelper = new Vector3();

	public override void OnStartAuthority()
	{
		enabled = true;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		Step();
    }

	private void Step()
	{

		inputSqrMagnitude = PlayerInputControllerAmongFall.prevWASDInput.sqrMagnitude;
		if (inputSqrMagnitude > .1f)
		{
			movePosHelper.x = PlayerInputControllerAmongFall.prevWASDInput.x;
			movePosHelper.z = PlayerInputControllerAmongFall.prevWASDInput.y;


			Vector3 newPostion = transform.position + (movePosHelper * Time.deltaTime * speed);
			NavMeshHit hit;
			bool isValid = NavMesh.SamplePosition(newPostion, out hit, .1f, NavMesh.AllAreas);
			if (isValid)
			{
				if ((transform.position - hit.position).magnitude >= .02f)
				{
					transform.position = hit.position;
				}
				else
				{
					//movement stopped this frame
				}
			}

		}
		else
		{
			//no Movement
		}
	}
}
