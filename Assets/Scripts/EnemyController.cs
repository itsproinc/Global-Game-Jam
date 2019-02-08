using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	private Movement movement;
	private AttackController attackController;

	// Use this for initialization
	void Start () {
		movement = GetComponent<Movement>();
		attackController = GetComponent<AttackController>();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, attackController.attackRange, attackController.targetLayer);
		movement.Move(Vector2.zero);
		if(hit.collider != null)
		{
			attackController.Attack();
		}
		else if(Vector2.Distance(PlayerController.instance.transform.position, transform.position) > 1)
		{
			movement.Move(PlayerController.instance.transform.position - transform.position);

		}
		

	}
}
