using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour, IInteractable
{
	public EnemyBody originalBody = null;
	[SerializeField] GameObject parentPrefab = null;
	[SerializeField] GameObject bulletPrefab = null;
	[SerializeField] GameObject corpsePrefab = null;

	private void Start()
	{
		if (originalBody == null)
		{
			GameObject _parent = Instantiate(parentPrefab, transform.position, Quaternion.identity);
			originalBody = (EnemyBody)_parent.GetComponent<Enemy>().body.Clone();
			Destroy(_parent);
		}
		if (originalBody == null)
			originalBody = new TriangleEnemyBody(5, bulletPrefab, parentPrefab, corpsePrefab);
		originalBody.sprite = GetComponent<SpriteRenderer>().sprite;
		originalBody.material = GetComponent<SpriteRenderer>().material;
	}

	public void Interact(PlayerController _player)
	{
		Material _newMaterial = new Material(originalBody.material);
		EnemyBody _newBody = (EnemyBody)originalBody.Clone();
		_newBody.parent = _player.gameObject;
		_newMaterial.color = _player.originalMaterial.color;
		if (_player.body != null)
			_player.body.Discard(_player);
		_player.body = _newBody;
		_player.GetComponent<SpriteRenderer>().sprite = originalBody.sprite;
		_player.GetComponent<SpriteRenderer>().material = _newMaterial;
		_player.transform.position = transform.position;
		_player.transform.rotation = transform.rotation;
		Destroy(gameObject);
	}

	public void TakeDamage(float _damage)
	{
		originalBody.hp -= _damage;
		if (originalBody.hp <= 0)
		{
			GameObject _projectile = Object.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			Rigidbody2D _rb = _projectile.GetComponent<Rigidbody2D>();
			_projectile.GetComponent<Projectile>().source = this.tag;
			_projectile.GetComponent<Projectile>().lifetime = 0.1f;
			_projectile.GetComponent<Projectile>().singleTarget = false;
			_projectile.transform.localScale = new Vector3(15, 15, 1);
			Destroy(gameObject);
		}
	}
}
