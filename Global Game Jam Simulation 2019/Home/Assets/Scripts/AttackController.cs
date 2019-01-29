using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackController:MonoBehaviour{
	public float attackRange = 2f;
	public float attackDelay = 1f;
    public int damage = 1;
	private float lastAttack;
	private Animator animator;
    public LayerMask targetLayer;
    public GameObject vfx;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }


    public void Attack()
    {
        if(Time.time - lastAttack > attackDelay)
		{
			if(animator) animator.SetTrigger("attack");
				lastAttack = Time.time;
			
		}
    }

    public void Execute()
	{
		
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, attackRange, targetLayer);
		if(hit.collider != null)
		{
			
			{
				hit.collider.GetComponent<Unit>().health -= damage;
                Instantiate(vfx, hit.collider.transform.position, Quaternion.identity);
			}
		}
	}

}