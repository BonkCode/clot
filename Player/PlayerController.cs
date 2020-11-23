using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomErrors;

public class PlayerController : MonoBehaviour
{
	#region Fields
	public Camera cam;
	public float shootAngle;
	public GameObject bulletPrefab; // delete
	public Sprite originalSprite;
	public Material originalMaterial;
	public EnemyBody body;
	private List<Corpse> interactedObjects = new List<Corpse>();
	private Vector2 input;
	private Vector2 moveVelocity;
	public float speed;
	private Rigidbody2D rb;
	#endregion

	#region MonoBehaviour methods
	private void Start()
	{
		originalSprite = GetComponent<SpriteRenderer>().sprite;
		originalMaterial = GetComponent<SpriteRenderer>().material;
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		HandleInput();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void OnTriggerEnter2D(Collider2D _other)
	{
		if (_other.gameObject.GetComponent<Corpse>() != null)
			interactedObjects.Add(_other.gameObject.GetComponent<Corpse>());
	}

	private void OnTriggerExit2D(Collider2D _other)
	{
		if (_other.gameObject.GetComponent<Corpse>() != null)
			interactedObjects.Remove(_other.gameObject.GetComponent<Corpse>());
	}
	#endregion

	#region Custom methods
	private void HandleInput()
	{
		Vector2 _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		Vector2 _lookDir = _mousePos - rb.position;
		shootAngle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
		rb.rotation = shootAngle;
		input = new Vector2( Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		moveVelocity = input.normalized * speed;
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (interactedObjects.Count > 0)
			{
				interactedObjects[0].Interact(this);
			}
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (body != null)
				body.Discard(this);
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (body != null)
				body.DoPrimaryAction(transform);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	private void TestShoot()
	{
		GameObject _projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		Rigidbody2D _rb = _projectile.GetComponent<Rigidbody2D>();
		_rb.AddForce(transform.right * _projectile.GetComponent<Projectile>().speed, ForceMode2D.Impulse);
		_rb.AddTorque(1000);
		_projectile.GetComponent<Projectile>().source = "Player";
	}

	public void TakeDamage(int _damage)
	{
		if (body == null)
		{
			return ; // TODO: fucking death
		}
		body.hp -= _damage;
		if (body.hp <= 0)
		{
			body.Discard(this);
		}
	}

	private void Move() 
	{
		rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
	}
	#endregion
}