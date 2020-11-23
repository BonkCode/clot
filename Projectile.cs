using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BonkUtils;

public class Projectile : MonoBehaviour
{
	public GameObject impactParticlePrefab;
	public GameObject mainCam;
	public bool singleTarget = true;
	public string source;
	private Rigidbody2D rb;
	private List<Collider2D> alreadyHitObjects = new List<Collider2D>();
	public float speed = 20f;
    public float shootAngle;
	public float lifetime = 5f;

	public virtual void Start()
	{
		mainCam = GameObject.FindWithTag("MainCamera");
		rb = GetComponent<Rigidbody2D>();
		Destroy(gameObject, lifetime);
	}

	private void OnTriggerEnter2D(Collider2D _other)
	{
		if (alreadyHitObjects.Contains(_other))
			return ;
		if (source == "Player" && _other.tag == "Player")
			return ;
		else if (source != "Player" && _other.tag != "Player" && source != "Corpse")
			return ;
		alreadyHitObjects.Add(_other);
		Enemy _enemy = _other.GetComponent<Enemy>();
		Corpse _corpse = _other.GetComponent<Corpse>();
		if (_enemy != null)
		{
			if (_enemy.body.hp - 1 <= 0)
				mainCam.GetComponent<CameraShaker>().Shake();
			_enemy.TakeDamage(1);
		}
		else if (_corpse != null)
		{
			_corpse.TakeDamage(1f);
		}
		else
		{
			PlayerController _player = _other.GetComponent<PlayerController>();
			if (_player != null)
				_player.TakeDamage(1);
		}
		if (singleTarget && _other.gameObject.layer == LayerMask.NameToLayer("AI") && source == "Player")
		{
			//mainCam.GetComponent<CameraShaker>().Shake();
		}
		if (singleTarget)
		{
			Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
