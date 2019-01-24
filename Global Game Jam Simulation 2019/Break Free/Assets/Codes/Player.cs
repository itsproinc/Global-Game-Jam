using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image movementJoyStick;
    public Image movementJoyStickInternal;
    public Vector3 inputVector;
    public Vector3 velocity = Vector3.zero;
    public Vector2 pos;

    public Image movementArea;
    public Vector2 placePos;
    public bool updatePos;

    public Color32 inactiveJoystickColor;
    public Color32 inactiveJoystickInternalColor;
    public Color32 activeJoystickColor;
    public Color32 activeJoystickInternalColor;

    public Rigidbody2D playerObject;
    public Animator animator;
    public Transform cameraObj;
    public int speed;

    public Button interactButton;
    public Text toolTip;

    public bool isHiding;

    [Space (10)]
    public bool onLadder;
    public bool useLadder;

    [Space (10)]
    public bool onHidingDoor;
    public bool useHidingDoor;

    [Space (10)]
    public bool onCellDoor;
    public bool useCellDoor;

    public void Reset ()
    {
        movementJoyStick = GameObject.Find ("Movement Joystick").GetComponent<Image> ();
        movementJoyStickInternal = GameObject.Find ("Internal Joystick").GetComponent<Image> ();

        movementArea = GameObject.Find ("Movement Joystick Area").GetComponent<Image> ();

        interactButton = GameObject.Find ("Interact Button").GetComponent<Button> ();
        toolTip = GameObject.Find ("Tooltip").GetComponent<Text> ();

        inactiveJoystickColor = movementJoyStick.color;
        inactiveJoystickColor.a = 100;
        inactiveJoystickInternalColor = movementJoyStickInternal.color;
        inactiveJoystickInternalColor.a = 100;

        activeJoystickColor = movementJoyStick.color;
        activeJoystickColor.a = 150;
        activeJoystickInternalColor = movementJoyStickInternal.color;
        activeJoystickInternalColor.a = 150;
    }

    private void Start ()
    {
        animator = playerObject.GetComponent<Animator> ();
        cameraObj = playerObject.transform.GetChild (0);

        speed = 4;
        movementJoyStick.gameObject.SetActive (false);

        toolTip.text = "";
        interactButton.interactable = false;
    }

    public virtual void OnDrag (PointerEventData PED)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle (movementJoyStick.rectTransform, PED.position, PED.pressEventCamera, out pos))
        {
            pos.x = (pos.x / movementJoyStick.rectTransform.sizeDelta.x);
            if (pos.x > 1)
                pos.x = 1;

            if (pos.x < -1)
                pos.x = -1;

            // Flip sprite
            if (pos.x < 0)
                playerObject.GetComponent<SpriteRenderer> ().flipX = true;
            else
                playerObject.GetComponent<SpriteRenderer> ().flipX = false;

            // Sprite animation
            if (pos.x != 0)
            {
                animator.SetBool ("isWalking", true);
                animator.SetFloat ("speed", pos.x);
            }

            pos.y = (pos.y / movementJoyStick.rectTransform.sizeDelta.y);
            if (pos.y > 1)
                pos.y = 1;

            if (pos.y < -1)
                pos.y = -1;

            inputVector = new Vector3 (pos.x * 2, 0, pos.y * 2);
            if (inputVector.magnitude > 1.0f)
                inputVector = inputVector.normalized;

            movementJoyStickInternal.rectTransform.anchoredPosition = new Vector3 (inputVector.x * (movementJoyStick.rectTransform.sizeDelta.x / 2), inputVector.z * (movementJoyStick.rectTransform.sizeDelta.y / 2));
        }
    }

    public virtual void OnPointerDown (PointerEventData PED)
    {
        // Place virtual joystick
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle (movementArea.rectTransform, PED.position, PED.pressEventCamera, out placePos))
        {
            movementJoyStick.gameObject.SetActive (true);
            movementJoyStick.transform.localPosition = new Vector3 (placePos.x, placePos.y, 0);

            movementJoyStick.color = activeJoystickColor;
            movementJoyStickInternal.color = activeJoystickInternalColor;
        }

        updatePos = true;
        OnDrag (PED);
    }

    public virtual void OnPointerUp (PointerEventData PED)
    {
        pos = Vector2.zero;
        inputVector = Vector3.zero;
        movementJoyStickInternal.rectTransform.anchoredPosition = Vector3.zero;
        updatePos = false;

        movementJoyStick.color = inactiveJoystickColor;
        movementJoyStickInternal.color = inactiveJoystickInternalColor;

        animator.SetBool ("isWalking", false);
    }

    private void Update ()
    {
        playerObject.velocity = new Vector2 (speed * pos.x, playerObject.velocity.y);

        // Camera
        if (updatePos)
        {
            Vector3 newPos = new Vector3 (pos.x * 2, cameraObj.localPosition.y, cameraObj.localPosition.z);
            cameraObj.localPosition = Vector3.Lerp (cameraObj.localPosition, newPos, Time.fixedDeltaTime);
            if (pos.x > 0)
                toolTip.GetComponent<Transform> ().localPosition = Vector3.Lerp (toolTip.GetComponent<Transform> ().localPosition, new Vector3 (-newPos.x * 80, toolTip.GetComponent<Transform> ().localPosition.y, toolTip.GetComponent<Transform> ().localPosition.z), Time.fixedDeltaTime);
            else
                toolTip.GetComponent<Transform> ().localPosition = Vector3.Lerp (toolTip.GetComponent<Transform> ().localPosition, new Vector3 (Mathf.Abs (newPos.x) * 80, toolTip.GetComponent<Transform> ().localPosition.y, toolTip.GetComponent<Transform> ().localPosition.z), Time.fixedDeltaTime);

        }
        else if (!updatePos && cameraObj.localPosition.x != 0)
        {
            Vector3 newPos = new Vector3 (0, cameraObj.localPosition.y, cameraObj.localPosition.z);
            cameraObj.localPosition = Vector3.Lerp (cameraObj.localPosition, newPos, Time.fixedDeltaTime);
            toolTip.GetComponent<Transform> ().localPosition = Vector3.Lerp (toolTip.GetComponent<Transform> ().localPosition, new Vector3 (0, toolTip.GetComponent<Transform> ().localPosition.y, toolTip.GetComponent<Transform> ().localPosition.z), Time.fixedDeltaTime);
        }

        // Interactables
        // Ladder
        if (onLadder)
        {
            if (!useLadder)
            {
                toolTip.text = "Press 'Interact' to use ladder";
                interactButton.interactable = true;

                playerObject.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                toolTip.text = "";
                interactButton.interactable = false;
                playerObject.isKinematic = true;

                // Ladder move
                playerObject.velocity = new Vector2 (playerObject.velocity.x, speed * pos.y);
            }
        }
        else
        {
            if ((playerObject.constraints & RigidbodyConstraints2D.FreezePositionY) == RigidbodyConstraints2D.FreezePositionY)
                if (!isHiding)
                    playerObject.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

            // Disable ladder physics
            if (playerObject.isKinematic)
            {
                playerObject.isKinematic = false;
                useLadder = false;
            }
        }

        // Hiding door
        if (onHidingDoor)
        {
            if (!useHidingDoor)
            {
                toolTip.text = "Press 'Interact' to hide";
                interactButton.interactable = true;

            }
        }

        if (useHidingDoor && isHiding)
        {
            toolTip.text = "Press 'Interact' to unhide";
            interactButton.interactable = true;
        }

        // Cell door
        if (onCellDoor)
        {
            if (!useCellDoor)
            {
                toolTip.text = "Press 'Interact' to enter cell";
                interactButton.interactable = true;

            }
        }

        if (useCellDoor && isHiding)
        {
            toolTip.text = "Press 'Interact' to get out of cell";
            interactButton.interactable = true;
        }
    }

    public void InteractObject ()
    {
        // Ladder
        if (onLadder)
            useLadder = true;

        // Hiding door
        if (useHidingDoor && isHiding)
        {
            isHiding = false;
            useHidingDoor = false;

            playerObject.GetComponent<SpriteRenderer> ().enabled = true;

            playerObject.constraints = RigidbodyConstraints2D.None;
            playerObject.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerObject.GetComponent<BoxCollider2D> ().enabled = true;
        }

        if (onHidingDoor)
        {
            useHidingDoor = true;

            if (useHidingDoor)
            {
                isHiding = true;
                playerObject.GetComponent<SpriteRenderer> ().enabled = false;

                playerObject.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                playerObject.GetComponent<BoxCollider2D> ().enabled = false;
            }
        }

        // Cell door
        if (useCellDoor && isHiding)
        {
            isHiding = false;
            useCellDoor = false;

            playerObject.GetComponent<SpriteRenderer> ().enabled = true;

            playerObject.constraints = RigidbodyConstraints2D.None;
            playerObject.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerObject.GetComponent<BoxCollider2D> ().enabled = true;
        }

        if (onCellDoor)
        {
            useCellDoor = true;

            if (useCellDoor)
            {
                isHiding = true;

                playerObject.GetComponent<SpriteRenderer> ().enabled = false;

                playerObject.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                playerObject.GetComponent<BoxCollider2D> ().enabled = false;
            }
        }
    }
}