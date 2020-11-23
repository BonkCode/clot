using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEnemyBody : EnemyBody
{
	public GameObject bulletPrefab;

	#region Constructors
	public CircleEnemyBody(GameObject _bulletPrefab, GameObject _parent, GameObject _corpsePrefab) 
	{
		startingHp = 5;
		hp = startingHp;
		bulletPrefab = _bulletPrefab;
		parent = _parent;
		corpsePrefab = _corpsePrefab;
	}

	public CircleEnemyBody(int _startingHp, int _hp, GameObject _bulletPrefab, GameObject _parent, GameObject _corpsePrefab)
	{
		startingHp = _startingHp;
		hp = _hp;
		bulletPrefab = _bulletPrefab;
		parent = _parent;
		corpsePrefab = _corpsePrefab;
	}

	public CircleEnemyBody(int _startingHp, GameObject _bulletPrefab, GameObject _parent, GameObject _corpsePrefab)
	{
		startingHp = _startingHp;
		hp = _startingHp;
		bulletPrefab = _bulletPrefab;
		parent = _parent;
		corpsePrefab = _corpsePrefab;
	}
	#endregion

	public override IClonable Clone()
	{
		CircleEnemyBody clone;
		clone = (CircleEnemyBody)this.MemberwiseClone();
		clone.startingHp = startingHp;
		clone.hp = hp;
		clone.corpsePrefab = corpsePrefab;
		return ((IClonable)clone);
	}

	public override void DoPrimaryAction(Transform _source)
	{
		GameObject _projectile = Object.Instantiate(bulletPrefab, parent.transform.position, Quaternion.identity);
		Rigidbody2D _rb = _projectile.GetComponent<Rigidbody2D>();
		_projectile.GetComponent<Projectile>().source = _source.tag;
		_projectile.GetComponent<Projectile>().lifetime = 0.1f;
		_projectile.GetComponent<Projectile>().singleTarget = false;
		_projectile.transform.localScale = new Vector3(15, 15, 1);
	}

	public override void Discard(PlayerController _player)
	{
		if (_player == null)
			return ;
		if (hp > 0)
		{
			GameObject corpse = GameObject.Instantiate(corpsePrefab, parent.transform.position, parent.transform.rotation);
			corpse.GetComponent<Corpse>().originalBody = this;
		}
		_player.body = null;
		_player.GetComponent<SpriteRenderer>().sprite = _player.originalSprite;
		_player.GetComponent<SpriteRenderer>().material = _player.originalMaterial;
	}
}

public class CircleEnemy : Enemy
{
	[SerializeField] private GameObject bulletPrefab = null;

	private void Awake()
	{
		currentAttackCooldown = Random.Range(1, attackCooldown);
		player = GameObject.FindGameObjectWithTag("Player").transform;
		body = new CircleEnemyBody(5, bulletPrefab, gameObject, corpsePrefab);
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (afk)
			return ;

		Vector3 direction = player.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		rb.rotation = angle;
		if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
		{
			transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
		}
		if (currentAttackCooldown <= 0)
		{
			body.DoPrimaryAction(transform);
			currentAttackCooldown = attackCooldown;
		}
		else
			currentAttackCooldown -= 1f * Time.deltaTime;
	}
}
