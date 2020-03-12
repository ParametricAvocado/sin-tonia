using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	static T instance;

	public static T GetInstance() {

		if (!instance) {
			instance = FindObjectOfType<T>();
		}

		return instance;
	}

	protected virtual void Awake() {
		instance = this as T;
	}

}
