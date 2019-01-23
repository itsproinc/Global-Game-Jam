using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeMovement : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
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
    public int speed;

    public void Reset ()
    {
        movementJoyStick = GameObject.Find ("Movement Joystick").GetComponent<Image> ();
        movementJoyStickInternal = GameObject.Find ("Internal Joystick").GetComponent<Image> ();

        movementArea = GameObject.Find ("Movement Joystick Area").GetComponent<Image> ();

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
        speed = 4;
        movementJoyStick.gameObject.SetActive (false);
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
    }

    private void Update ()
    {
        playerObject.velocity = new Vector2 (speed * pos.x, playerObject.velocity.y);
    }
}