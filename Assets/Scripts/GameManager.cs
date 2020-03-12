using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager> {
	[SerializeField] float videoQualityDegradation;
	[SerializeField] float matchDuration;
	[SerializeField] float maxEnjoyment = 10;
	[SerializeField] float maxVideoQuality = 5;
	[SerializeField] float bonusVideoQuality = 2;
	[SerializeField] AnimationCurve criticalHitBonusCurve;
	[SerializeField] AnimationCurve punishCurve;
	[SerializeField] AnimationCurve enjoymentResponseCurve;
	[SerializeField] TV[] playerTVs;
	[SerializeField] UnityEvent onEnterIngame;
	[SerializeField] UnityEvent onMatchEnd;
	[SerializeField] UnityEvent onResetGame;

	public bool IsIngame { get; private set; }
	public bool IsMatchEnded { get; private set; }
	public float BonusVideoQuality => bonusVideoQuality;
	public float MaxVideoQuality => maxVideoQuality;
	public float MaxEnjoyment => maxEnjoyment;
	public float MatchDuration => matchDuration;

	public int PlayerCount { get; private set; } = 2;
	public int PlayerWonIndex { get; private set; }
	public float MatchTime { get; private set; }
	public float[] PlayerEnjoyment { get; private set; }
	public float[] VideoQuality { get; private set; }

	public AnimationCurve CriticalHitBounusCurve => criticalHitBonusCurve;
	public AnimationCurve PunishCurve => punishCurve;
	public UnityEvent OnEnterIngame => onEnterIngame;
	public UnityEvent OnMatchEnd => onMatchEnd;
	public UnityEvent OnResetGame => onResetGame;

	private void Start() {
		PlayerEnjoyment = new float[PlayerCount];
		VideoQuality = new float[PlayerCount];
		for (int i = 0; i < PlayerCount; i++) {
			VideoQuality[i] = MaxVideoQuality;
		}

		for (int i = 0; i < 2; i++) {
			PlayerEnjoyment[i] = 0;
		}
	}

	public void StartGame() {
		IsIngame = true;

		for (int i = 0; i < PlayerCount; i++) {
			VideoQuality[i] = MaxVideoQuality;
		}

		for (int i = 0; i < 2; i++) {
			PlayerEnjoyment[i] = 0;
		}
		OnEnterIngame.Invoke();
	}

	private void EndMatch() {

		IsIngame = false;
		IsMatchEnded = true;
		OnMatchEnd.Invoke();
	}

	private void ResetGame() {
		IsMatchEnded = false;
		IsIngame = false;
		OnResetGame.Invoke();
	}

	private void Update() {
		if (IsIngame) {
			IngameUpdate();
		}
		else if(IsMatchEnded) {
			if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.KeypadEnter)){
				ResetGame();
			}
		}
		else {
			if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.KeypadEnter)){
				StartGame();
			}
		}
	}

	private void IngameUpdate() {
		MatchTime = Mathf.MoveTowards(MatchTime, matchDuration, Time.deltaTime);
		for (int i = 0; i < PlayerCount; i++) {
			VideoQuality[i] = playerTVs[i].VideoQuality = Mathf.MoveTowards(playerTVs[i].VideoQuality, 0, Time.deltaTime * videoQualityDegradation);
			PlayerEnjoyment[i] += enjoymentResponseCurve.Evaluate(VideoQuality[i] / MaxVideoQuality) * Time.deltaTime;
		}

		float bestScore = 0;
		for (int i = 0; i < PlayerCount; i++) {
			if (PlayerEnjoyment[i] > bestScore) {
				bestScore = PlayerEnjoyment[i];
				PlayerWonIndex = i;
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape) ||
			MatchTime == matchDuration || bestScore >= MaxEnjoyment) {
			EndMatch();
		}
	}

}
