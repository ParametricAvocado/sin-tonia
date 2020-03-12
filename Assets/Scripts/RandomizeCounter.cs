using UnityEngine;

public class RandomizeCounter : StateMachineBehaviour {
	public string counterName = "Counter";
	public int counterMaxVal = 1;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetInteger(counterName, Random.Range(0, counterMaxVal+1));
	}
}
