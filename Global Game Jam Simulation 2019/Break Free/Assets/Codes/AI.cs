using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject [] patrolPoint;

    public Rigidbody2D enemyObject;
    public Transform enemyView;
    public Animator animator;
    public float speed;

    public bool move;
    public bool isMoving;
    public bool arrived;

    public int gotoWaypoint;
    public int currentWaypoint;

    private void Start ()
    {
        enemyObject = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
        enemyView = enemyObject.transform.GetChild (0);
        currentWaypoint = -1;

        speed = 1;
    }

    public void Update ()
    {
        // Sprite animation
        if (move)
        {
            animator.SetBool ("isWalking", true);
            animator.SetFloat ("speed", 0.5f);
        }
        else
            animator.SetBool ("isWalking", false);

        if (!isMoving)
        {
            gotoWaypoint = UnityEngine.Random.Range (0, patrolPoint.Length);

            if (gotoWaypoint != currentWaypoint)
            {
                isMoving = true;
                move = true;
                arrived = false;

                // Flip sprite
                if (gotoWaypoint < currentWaypoint)
                {
                    enemyObject.GetComponent<SpriteRenderer> ().flipX = true; // Left
                    enemyView.localPosition = new Vector3 (-1.96f, enemyView.localPosition.y, enemyView.localPosition.z);
                    enemyView.localEulerAngles = new Vector3 (enemyView.rotation.x, enemyView.rotation.y, 180f);
                }
                else
                {
                    enemyObject.GetComponent<SpriteRenderer> ().flipX = false; // Right
                    enemyView.localPosition = new Vector3 (1.96f, enemyView.localPosition.y, enemyView.localPosition.z);
                    enemyView.localEulerAngles = new Vector3 (enemyView.rotation.x, enemyView.rotation.y, 0f);
                }
            }
        }

        // Go waypoint
        if (isMoving && !arrived)
        {
            // Move (Left)
            if (gotoWaypoint < currentWaypoint)
            {
                if (enemyObject.position.x > patrolPoint [gotoWaypoint].transform.position.x)
                {
                    enemyObject.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                    enemyObject.velocity = new Vector2 (speed * -1, enemyObject.velocity.y);
                }
                else
                {
                    if (!arrived)
                    {
                        arrived = true;
                        StartCoroutine (wait (UnityEngine.Random.Range (3, 7)));
                    }
                }
            }
            else
            {
                if (enemyObject.position.x < patrolPoint [gotoWaypoint].transform.position.x)
                {
                    enemyObject.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                    enemyObject.velocity = new Vector2 (speed * 1, enemyObject.velocity.y);
                }
                else
                {
                    if (!arrived)
                    {
                        arrived = true;
                        StartCoroutine (wait (UnityEngine.Random.Range (3, 7)));
                    }
                }
            }
        }
    }

    IEnumerator wait (int length)
    {
        enemyObject.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        move = false;
        yield return new WaitForSeconds (length);
        currentWaypoint = gotoWaypoint;
        isMoving = false;
    }
}