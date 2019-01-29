using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	
	[HideInInspector]
	public Unit unit;
	private Movement movement;
	public static PlayerController instance;
	private Animator animator;
	private AttackController attackController;
	public GameObject wall;
	public GameObject gameOverWindow;

	public int enemyBaseMoney = 200;

	void Start () {
		unit = GetComponent<Unit>();
		movement = GetComponent<Movement>();
		instance = this;
		animator = GetComponentInChildren<Animator>();
		attackController = GetComponent<AttackController>();
		attackController.attackDelay = 1/(1/attackController.attackDelay * Upgradeables.attackSpeed);
		print(attackController.attackDelay);
		attackController.damage = (int) (attackController.damage * Upgradeables.attackDamage);
		unit.maxHealth = (int)(unit.health * Upgradeables.mental);
		unit.health = unit.maxHealth;
		animator.SetFloat("aspd", Upgradeables.attackSpeed);
		animator.SetFloat("speed", 2);
		enabled = false;
		unit.die = Die;
		transform.DOMove(new Vector3(0, transform.position.y, transform.position.z), movement.speed * 1.5f).SetSpeedBased().OnComplete(()=>
		{
			enabled = true;
			wall.SetActive(true);
		});
	}
	
	// Update is called once per frame
	void Update () {
		float deltaMove = Input.GetAxisRaw("Horizontal");
		movement.Move(new Vector2(deltaMove, 0));
		if(Input.GetButtonDown("Jump")) movement.Jump();
		if(Input.GetButtonDown("Fire1"))
        {
            attackController.Attack();
        }
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Money")
		{
			Upgradeables.curMoney += 200;
			Destroy(other.gameObject);
		}
	}
	
	

	void Die()
	{
		GetComponent<AttackController>().enabled = false;
		movement.enabled = false;
		gameObject.SetActive(false);
		gameOverWindow.SetActive(true);
		PlayerPrefs.SetInt ("money", Upgradeables.curMoney);
	}

	public void GoToScene(string sName)
	{
		SceneManager.LoadScene(sName);
	}
}
