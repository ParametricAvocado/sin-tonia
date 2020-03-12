using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TV : MonoBehaviour, IPointerDownHandler {
	[SerializeField] UnityEvent onPointerDown;
	[SerializeField] GameObject menuCamera;
	[SerializeField] GameObject ingameCamera;
	[SerializeField] KeyCode punchKey;
	[SerializeField] AudioSource hitSource;
	[SerializeField] AudioClip[] hitSFX;

	public float VideoQuality { get; set; }

	Animator animator;
	int hHitTrigger = Animator.StringToHash("Hit");
	int hQualityFloat = Animator.StringToHash("Quality");
	int hCritBool = Animator.StringToHash("Crit");

	GameManager GameManager => GameManager.GetInstance();

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
		Punch();
	}

	private void Punch() {
		var randomQuality = UnityEngine.Random.Range(0, GameManager.MaxVideoQuality);
		var bonusQuality = GameManager.MaxVideoQuality + GameManager.BonusVideoQuality;
		var criticalHit = GameManager.CriticalHitBounusCurve.Evaluate(VideoQuality / GameManager.MaxVideoQuality);
		VideoQuality = Mathf.Lerp(randomQuality, bonusQuality, criticalHit);

		animator.SetTrigger(hHitTrigger);
		hitSource.PlayOneShot(hitSFX[UnityEngine.Random.Range(0, hitSFX.Length)]);
		onPointerDown.Invoke();
	}

	private void Start() {
		VideoQuality = GameManager.MaxVideoQuality;
		GameManager.GetInstance().OnEnterIngame.AddListener(OnEnterIngame);
		GameManager.GetInstance().OnResetGame.AddListener(OnExitIngame);
	}

	private void OnEnterIngame() {
		ingameCamera.SetActive(true);
		menuCamera.SetActive(false);
	}

	private void OnExitIngame() {
		ingameCamera.SetActive(false);
		menuCamera.SetActive(true);
	}

	private void Update() {
		if (Input.GetKeyDown(punchKey)) {
			Punch();
		}
	}

	private void LateUpdate() {
		var criticalHit = GameManager.CriticalHitBounusCurve.Evaluate(VideoQuality / GameManager.MaxVideoQuality);

		animator.SetFloat(hQualityFloat, VideoQuality / GameManager.MaxVideoQuality);
		animator.SetBool(hCritBool, criticalHit > 0.1f);
	}
}
