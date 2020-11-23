using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	private Vector3 originalPos;
	
	private void Start()
	{
		originalPos = transform.localPosition;
	}

	public IEnumerator CameraShake (float _duration, float _magnitude)
	{
		float _elapsed = 0f;
		while (_elapsed < _duration)
		{
			float x = Random.Range(-1f, 1f) * _magnitude;
			float y = Random.Range(-1f, 1f) * _magnitude;

			transform.localPosition = new Vector3(x, y, originalPos.z);
			_elapsed += Time.deltaTime;
			yield return null;
		}
		transform.localPosition = originalPos;
	}

	public void Shake()
	{
		StartCoroutine(CameraShake(0.05f, 0.4f));
	}
}
