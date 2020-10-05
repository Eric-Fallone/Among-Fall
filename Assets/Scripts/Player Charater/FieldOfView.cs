/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class FieldOfView : MonoBehaviour {

    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    private Vector3 origin;
    private float startingAngle;

	private GameObject ObjectToFollow;

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 360f;
        viewDistance = 5f;

		ObjectToFollow = gameObject.transform.parent.gameObject;
		gameObject.transform.parent = null;
		this.gameObject.transform.position = new Vector3(0,.01f,0);
	}


	private void LateUpdate() {
        int rayCount = 300;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

		origin = ObjectToFollow.transform.position;

        vertices[0] = origin ;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;

			Vector3 temp = UtilsClass.GetVectorFromAngle(angle);
			float y = temp.y;
			temp.y = 0;
			temp.z = y;
			RaycastHit raycastHit;
			Physics.Raycast(origin , temp, out raycastHit, viewDistance, layerMask);
            if (raycastHit.collider == null) {
				// No hit
				vertex = origin + temp * viewDistance;
            } else {
				// Hit object
                vertex = raycastHit.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin) {
       // this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection) {
        //startingAngle = UtilsClass.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    public void SetFoV(float fov) {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance) {
        this.viewDistance = viewDistance;
    }

}
