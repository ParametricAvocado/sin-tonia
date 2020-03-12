using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	[SerializeField] GameObject menuUI;
	[SerializeField] GameObject ingameUI;
	[SerializeField] GameObject endMatchUI;
	[SerializeField] Animator[] playerWonTexts;

	[SerializeField] Text timerText;
	[SerializeField] Slider timeSlider;
	[SerializeField] Slider[] qualitySliders;
	[SerializeField] Slider[] enjoymentSlider;

	GameManager GameManager => GameManager.GetInstance();
	TimeSpan matchSpan;
	private void Start() {
		GameManager.OnEnterIngame.AddListener(OnEnterIngame);
		GameManager.OnMatchEnd.AddListener(OnMatchEnd);
		GameManager.OnResetGame.AddListener(OnResetGame);
	}

	private void OnMatchEnd() {
		endMatchUI.SetActive(true);
		for (int i = 0; i < GameManager.PlayerCount; i++) {
			playerWonTexts[i].SetBool("Won", i == GameManager.PlayerWonIndex);
		}
	}

	private void OnEnterIngame() {
		menuUI.SetActive(false);
		endMatchUI.SetActive(false);
		ingameUI.SetActive(true);
	}

	private void OnResetGame() {
		menuUI.SetActive(true);
		endMatchUI.SetActive(false);
		ingameUI.SetActive(false);
	}

	private void LateUpdate() {
		if (GameManager.IsIngame) {
			UpdateIngame();
		}
	}

	private void UpdateIngame() {
		timeSlider.value = GameManager.MatchTime / GameManager.MatchDuration;

		for (int i = 0; i < GameManager.PlayerCount; i++) {
			qualitySliders[i].value = GameManager.VideoQuality[i] / GameManager.MaxVideoQuality;
			enjoymentSlider[i].value = GameManager.PlayerEnjoyment[i] / GameManager.MaxEnjoyment;
		}

		matchSpan = TimeSpan.FromSeconds(GameManager.MatchDuration - GameManager.MatchTime);

		timerText.text = string.Format("{0}:{1}", matchSpan.Minutes, matchSpan.Seconds.ToString("D2"));
	}
}
