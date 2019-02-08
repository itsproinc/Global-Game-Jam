using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public float speed = 2;
	public float jumpForce = 3;
	private Rigidbody2D rigidbody;
	private BoxCollider2D col;
	private Animator animator;

	// Use this for initialization
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
		animator = GetComponentInChildren<Animator> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void Move (Vector2 direction)
	{
		direction.y = 0;
		rigidbody.position += direction.normalized * speed * Time.deltaTime;
		if (direction.x < 0) transform.rotation = Quaternion.Euler (0, 180, 0);
		else if (direction.x > 0) transform.rotation = Quaternion.Euler (0, 0, 0);
		if (animator) animator.SetFloat ("speed", direction.magnitude);

	}

	bool isGrounded
	{
		get
		{
			return Physics2D.Raycast (transform.position, -Vector2.up, col.size.y / 2 + 0.05f, 1 << LayerMask.NameToLayer ("Ground"));
		}
	}

	public void Jump ()
	{
		if (isGrounded) rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpForce);
	}
}