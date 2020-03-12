using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IncreaseCounter : StateMachineBehaviour {
	public string counterName = "Counter";
	public int counterMaxVal = 1;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetInteger(counterName, (animator.GetInteger(counterName) + 1) % counterMaxVal);
	}
}
