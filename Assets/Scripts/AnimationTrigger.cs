using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			GetComponent<Animator>().SetTrigger("Hit");
		}
	}
}
