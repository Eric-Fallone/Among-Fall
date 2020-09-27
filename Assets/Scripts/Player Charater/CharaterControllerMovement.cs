using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharaterControllerMovement : MonoBehaviour 
{
	public float speed = 10f;

	public Vector3 inputValue = Vector3.zero;
	private float inputSqrMagnitude;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		SetInput();
		Step();
    }

	private void Step()
	{
		inputSqrMagnitude = inputValue.sqrMagnitude;
		if (inputSqrMagnitude > .1f)
		{
			Vector3 newPostion = transform.position + (inputValue * Time.deltaTime * speed);
			NavMeshHit hit;
			bool isValid = NavMesh.SamplePosition(newPostion, out hit, .3f, NavMesh.AllAreas);
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

	private void SetInput()
	{
		inputValue.x = Input.GetAxis("Horizontal");
		inputValue.z = Input.GetAxis("Vertical");
	}
}
