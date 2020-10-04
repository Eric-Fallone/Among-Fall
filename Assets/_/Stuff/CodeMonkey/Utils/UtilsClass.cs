/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeMonkey.Utils {

	/*
     * Various assorted utilities functions
     * */
	public static class UtilsClass
	{



		public static Vector3 GetVectorFromAngle(float angle)
		{
			// angle = 0 -> 360
			float angleRad = angle * (Mathf.PI / 180f);
			return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
		}

	}
}