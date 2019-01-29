using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationHelper : MonoBehaviour {
	public UnityEvent[] listFunction;

	public void AnimationFunction (int functionIndex)
	{
		listFunction[functionIndex].Invoke();
	}

}
