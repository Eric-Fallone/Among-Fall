﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWhenClose : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		print(collision.gameObject.layer);
	}
}
