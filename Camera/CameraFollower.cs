using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    #region Fields
	Vector3 originalPos;
	public Transform target;
	public CameraShaker shake;
	private float zPos;
	public float lerpSmoothness;
	#endregion

	#region MonoBehaviour methods
	private void Start()
	{
		originalPos = transform.localPosition;
		zPos = transform.position.z;
	}

	private void FixedUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, target.position, lerpSmoothness);
		transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
	}
	#endregion

	#region Custom methods
	#endregion
}
