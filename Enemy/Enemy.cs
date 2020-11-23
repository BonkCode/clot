using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyBody : IClonable
{
	public float hp;
	public float startingHp;
	public Sprite sprite;
	public Material material;
	public GameObject parent;
	public GameObject corpsePrefab;

	public abstract IClonable Clone();

	public abstract void Discard(PlayerController _player);

	public abstract void DoPrimaryAction(Transform _source);
}

abstract public class Enemy : MonoBehaviour
{
	[SerializeField] protected bool afk = false;
	[SerializeField] [Range(0f, 1f)]protected float bodySpawnChance = 1f;
	protected float attackCooldown = 5f;
	protected float currentAttackCooldown;
	public EnemyBody body;
	public GameObject corpsePrefab;
	public float speed;
	public float stoppingDistance;
	public Transform player;
	public Rigidbody2D rb;

	public virtual void TakeDamage(int _damage)
	{
		body.hp -= _damage;
		if (body.hp <= 0)
		{
			EnemyBody _newBody = (EnemyBody)body.Clone();
			_newBody.hp = _newBody.startingHp;
			if (Random.Range(0f, 1f) <= bodySpawnChance)
			{
				GameObject corpse = Instantiate(corpsePrefab, transform.position, Quaternion.identity);
				corpse.GetComponent<Corpse>().originalBody = _newBody;
				corpse.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rb.rotation);
			}
			Destroy(gameObject);
		}
	}
}
