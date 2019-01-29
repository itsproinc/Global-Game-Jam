using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DropChance
{
	[Range(0,1)]
	public float chance;
	public GameObject drop;
}

public class Unit : MonoBehaviour {
	
	public Image healthUI;
	public Text healthText;
	[SerializeField] int _health = 3;
	public GameObject dieEffect;
	[SerializeField] DropChance[] dropChances;
	
	
	public int maxHealth;
	public System.Action die;
	Animator animator;

	
	public int health
	{
		
		get
		{
			return _health;
		}
		set
		{
			if(health > 0 && value <= 0)
			{
				if(dieEffect != null)Instantiate(dieEffect, transform.position + Vector3.up, Quaternion.identity);
				if(die != null)
				{					
					die();
					RandomDrop();
				}
			}
			else if(value < health) //damaged
			{
				if(animator) animator.SetTrigger("damaged");
			}
			_health = Mathf.Clamp(value, 0, maxHealth);
		
			if(healthUI != null) healthUI.transform.localScale = new Vector3(Mathf.Clamp01((float)health/maxHealth), 1, 1);
			if(healthText != null) healthText.text = health.ToString() + "/" + maxHealth.ToString();
			

		}
	}


	void RandomDrop()
	{
		for(int i = 0; i < dropChances.Length;i ++)
		{
			float r = Random.Range(0f, 1f);
			if(r <= dropChances[i].chance)
			{
				Instantiate(dropChances[i].drop, transform.position + Random.insideUnitSphere * 0.3f, Quaternion.identity);
				
			}
		}

		// float currentChance = 0;
		// for(int i = 0; i < dropChances.Length;i ++)
		// {
		// 	currentChance += dropChances[i].chance;
		// 	if(currentChance > r)
		// 	{
		// 		Instantiate(dropChances[i].drop, transform.position, Quaternion.identity);
		// 		break;
		// 	}
		// }
	}


	void Awake () {
		maxHealth = health;
		health = health;
		animator = GetComponentInChildren<Animator>();
		if(die == null) die = () => Destroy(gameObject);
	}
	
}