using UnityEngine;
using System.Collections;

public class MenuStateManager : MonoBehaviour
{
	[SerializeField] UITexture gameCenterIcon;
	[SerializeField] TweenAlpha titleTween;
	[SerializeField] TweenAlpha optionsTween;
	[SerializeField] TweenAlpha stageSelectTween;
	[SerializeField] TweenAlpha[] levelSelectTweens;
	[SerializeField] MenuObject[] idleMenuObjects;
	[SerializeField] MenuObject[] optionsMenuObjects;
	[SerializeField] MenuObject[] stageSelectMenuObjects;
	[SerializeField] MenuObject[] levelSelectMenuObjects;
	[SerializeField] GameObject[] lockBanners;
	[SerializeField] Collider[] stageSelectBackButtons;
	[SerializeField] Collider[] stage1Buttons;
	[SerializeField] Collider[] stage2Buttons;
	[SerializeField] GameObject[] stage2Particles;
	[SerializeField] Collider[] stage3Buttons;
	[SerializeField] GameObject[] stage3Particles;
	[SerializeField] Collider[] stage4Buttons;
	[SerializeField] GameObject[] stage4Particles;

	public enum MenuState
	{
		Idle,
		Options,
		StageSelect,
		GravitySelect,
		AntiGravitySelect,
		WormHoleSelect,
		ChaosTheorySelect
	}
	
	public MenuState menuState;
	
	public void SetState()
	{
		switch(menuState)
		{
		case MenuState.Idle:
			Idle();
			break;
		case MenuState.Options:
			Options();
			break;
		case MenuState.StageSelect:
			StageSelect();
			break;
		case MenuState.GravitySelect:
			LevelSelect(0);
			break;
		case MenuState.AntiGravitySelect:
			LevelSelect(1);
			break;
		case MenuState.WormHoleSelect:
			LevelSelect(1);
			break;
		case MenuState.ChaosTheorySelect:
			LevelSelect(1);
			break;
		}
	}

	void Awake()
	{
		gameCenterIcon.enabled = true;
		menuState = MenuState.Idle;
		FadeOutLabel(optionsTween, 0.01f);
		FadeOutLabel(stageSelectTween, 0.01f);

		foreach (TweenAlpha levelTween in levelSelectTweens)
			FadeOutLabel(levelTween, 0.01f);

		foreach (GameObject lockBanner in lockBanners)
			lockBanner.SetActive(false);

		foreach(Collider backButton in stageSelectBackButtons)
			backButton.enabled = false;

		SetState();
	}

	void Idle()
	{
		gameCenterIcon.enabled = true;
		FadeInLabel(titleTween, 1f, 0.5f);
		FadeOutLabel(optionsTween);
		FadeOutLabel(stageSelectTween);

		foreach (GameObject lockBanner in lockBanners)
			lockBanner.SetActive(false);

		foreach(Collider backButton in stageSelectBackButtons)
			backButton.enabled = false;

		foreach (MenuObject idleMenuObject in idleMenuObjects)
		{
			if (!idleMenuObject.menuLabel)
			{
				Collider objectCollider = idleMenuObject.menuObject.GetComponent<Collider>();
				TrailRenderer[] trailRenderers = idleMenuObject.menuObject.GetComponentsInChildren<TrailRenderer>();
				ParticleSystem[] particleSystems = idleMenuObject.menuObject.GetComponentsInChildren<ParticleSystem>();

				if (idleMenuObject.activate)
				{
					objectCollider.enabled = true;

					foreach (TrailRenderer trailRenderer in trailRenderers)
						trailRenderer.enabled = true;

					foreach (ParticleSystem particleSystem in particleSystems)
						particleSystem.enableEmission = true;
				}
				else
				{
					objectCollider.enabled = false;

					foreach (TrailRenderer trailRenderer in trailRenderers)
						trailRenderer.enabled = false;
					
					foreach (ParticleSystem particleSystem in particleSystems)
						particleSystem.enableEmission = false;
				}

				RepositionMenuObject repositionMenuObjectScript = idleMenuObject.menuObject.GetComponent<RepositionMenuObject>();
				repositionMenuObjectScript.Reposition(idleMenuObject.position);
			}
			else
			{
				if (idleMenuObject.activate)
					idleMenuObject.menuObject.SetActive(true);
				else
					idleMenuObject.menuObject.SetActive(false);
			}
		}
	}

	void Options()
	{
		gameCenterIcon.enabled = false;
		FadeInLabel(optionsTween, 1f, 0.5f);
		FadeOutLabel(titleTween, 0.25f);

		foreach (MenuObject optionsMenuObject in optionsMenuObjects)
		{
			if (!optionsMenuObject.menuLabel)
			{
				Collider objectCollider = optionsMenuObject.menuObject.GetComponent<Collider>();
				TrailRenderer[] trailRenderers = optionsMenuObject.menuObject.GetComponentsInChildren<TrailRenderer>();
				ParticleSystem[] particleSystems = optionsMenuObject.menuObject.GetComponentsInChildren<ParticleSystem>();
				
				if (optionsMenuObject.activate)
				{
					objectCollider.enabled = true;
					
					foreach (TrailRenderer trailRenderer in trailRenderers)
						trailRenderer.enabled = true;
					
					foreach (ParticleSystem particleSystem in particleSystems)
						particleSystem.enableEmission = true;
				}
				else
				{
					objectCollider.enabled = false;
					
					foreach (TrailRenderer trailRenderer in trailRenderers)
						trailRenderer.enabled = false;
					
					foreach (ParticleSystem particleSystem in particleSystems)
						particleSystem.enableEmission = false;
				}
				
				RepositionMenuObject repositionMenuObjectScript = optionsMenuObject.menuObject.GetComponent<RepositionMenuObject>();
				repositionMenuObjectScript.Reposition(optionsMenuObject.position);
			}
			else
			{
				if (optionsMenuObject.activate)
					optionsMenuObject.menuObject.SetActive(true);
				else
					optionsMenuObject.menuObject.SetActive(false);
			}
		}
	}

	void StageSelect()
	{
		gameCenterIcon.enabled = false;
		FadeInLabel(stageSelectTween, 1f, 0.5f);
		FadeOutLabel(titleTween, 0.25f);

		foreach (TweenAlpha levelTween in levelSelectTweens)
			FadeOutLabel(levelTween);

		foreach(Collider backButton in stageSelectBackButtons)
			backButton.enabled = true;

		foreach(Collider stage1Button in stage1Buttons)
			stage1Button.enabled = true;

		int currentStars = EncryptedPlayerPrefs.GetInt("Total Stars", 0);

		if (currentStars < 21)
		{
			lockBanners[0].SetActive(true);
			lockBanners[1].SetActive(true);
			lockBanners[2].SetActive(true);

			foreach(Collider stage2Button in stage2Buttons)
				stage2Button.enabled = false;
			
			foreach(Collider stage3Button in stage3Buttons)
				stage3Button.enabled = false;

			foreach(Collider stage4Button in stage4Buttons)
				stage4Button.enabled = false;

			foreach(GameObject stage2Particle in stage2Particles)
				stage2Particle.SetActive(false);

			foreach(GameObject stage3Particle in stage3Particles)
				stage3Particle.SetActive(false);

			foreach(GameObject stage4Particle in stage4Particles)
				stage4Particle.SetActive(false);
		}
		else if (currentStars >= 21)
		{
			lockBanners[0].SetActive(false);
			lockBanners[1].SetActive(true);
			lockBanners[2].SetActive(true);

			foreach(Collider stage2Button in stage2Buttons)
				stage2Button.enabled = true;
			
			foreach(Collider stage3Button in stage3Buttons)
				stage3Button.enabled = false;
			
			foreach(Collider stage4Button in stage4Buttons)
				stage4Button.enabled = false;
			
			foreach(GameObject stage2Particle in stage2Particles)
				stage2Particle.SetActive(true);
			
			foreach(GameObject stage3Particle in stage3Particles)
				stage3Particle.SetActive(false);
			
			foreach(GameObject stage4Particle in stage4Particles)
				stage4Particle.SetActive(false);
		}
		else if (currentStars >= 46)
		{
			lockBanners[0].SetActive(false);
			lockBanners[1].SetActive(false);
			lockBanners[2].SetActive(true);

			foreach(Collider stage2Button in stage2Buttons)
				stage2Button.enabled = true;
			
			foreach(Collider stage3Button in stage3Buttons)
				stage3Button.enabled = true;
			
			foreach(Collider stage4Button in stage4Buttons)
				stage4Button.enabled = false;
			
			foreach(GameObject stage2Particle in stage2Particles)
				stage2Particle.SetActive(true);
			
			foreach(GameObject stage3Particle in stage3Particles)
				stage3Particle.SetActive(true);
			
			foreach(GameObject stage4Particle in stage4Particles)
				stage4Particle.SetActive(false);
		}
		else if (currentStars >= 72)
		{
			lockBanners[0].SetActive(false);
			lockBanners[1].SetActive(false);
			lockBanners[2].SetActive(false);

			foreach(Collider stage2Button in stage2Buttons)
				stage2Button.enabled = true;
			
			foreach(Collider stage3Button in stage3Buttons)
				stage3Button.enabled = true;
			
			foreach(Collider stage4Button in stage4Buttons)
				stage4Button.enabled = true;
			
			foreach(GameObject stage2Particle in stage2Particles)
				stage2Particle.SetActive(true);
			
			foreach(GameObject stage3Particle in stage3Particles)
				stage3Particle.SetActive(true);
			
			foreach(GameObject stage4Particle in stage4Particles)
				stage4Particle.SetActive(true);
		}

		foreach (MenuObject stageSelectMenuObject in stageSelectMenuObjects)
		{
			if (!stageSelectMenuObject.menuLabel)
			{
				Collider objectCollider = stageSelectMenuObject.menuObject.GetComponent<Collider>();
				TrailRenderer[] trailRenderers = stageSelectMenuObject.menuObject.GetComponentsInChildren<TrailRenderer>();
				ParticleSystem[] particleSystems = stageSelectMenuObject.menuObject.GetComponentsInChildren<ParticleSystem>();

				if (stageSelectMenuObject.activate)
				{
					//objectCollider.enabled = true;

					foreach (TrailRenderer trailRenderer in trailRenderers)
						trailRenderer.enabled = true;
					
					foreach (ParticleSystem particleSystem in particleSystems)
						particleSystem.enableEmission = true;
				}
				else
				{
					objectCollider.enabled = false;

					foreach (TrailRenderer trailRenderer in trailRenderers)
						trailRenderer.enabled = false;
					
					foreach (ParticleSystem particleSystem in particleSystems)
						particleSystem.enableEmission = false;
				}
				
				RepositionMenuObject repositionMenuObjectScript = stageSelectMenuObject.menuObject.GetComponent<RepositionMenuObject>();
				repositionMenuObjectScript.Reposition(stageSelectMenuObject.position);
			}
			else
			{
				if (stageSelectMenuObject.activate)
					stageSelectMenuObject.menuObject.SetActive(true);
				else
					stageSelectMenuObject.menuObject.SetActive(false);
			}
		}
	}

	void LevelSelect(int stageValue)
	{
		FadeInLabel(levelSelectTweens[stageValue], 1f, 0.5f);
		FadeOutLabel(stageSelectTween, 0.25f);

		foreach (GameObject lockBanner in lockBanners)
			lockBanner.SetActive(false);

		foreach(Collider backButton in stageSelectBackButtons)
			backButton.enabled = false;

		foreach (MenuObject levelSelectMenuObject in levelSelectMenuObjects)
		{
			if (!levelSelectMenuObject.menuLabel)
			{
				Collider objectCollider = levelSelectMenuObject.menuObject.GetComponent<Collider>();
				TrailRenderer[] trailRenderers = levelSelectMenuObject.menuObject.GetComponentsInChildren<TrailRenderer>();
				ParticleSystem[] particleSystems = levelSelectMenuObject.menuObject.GetComponentsInChildren<ParticleSystem>();
				
				if (levelSelectMenuObject.activate)
				{
					objectCollider.enabled = true;
					
					foreach (TrailRenderer trailRenderer in trailRenderers)
						trailRenderer.enabled = true;
					
					foreach (ParticleSystem particleSystem in particleSystems)
						particleSystem.enableEmission = true;
				}
				else
				{
					objectCollider.enabled = false;
					
					foreach (TrailRenderer trailRenderer in trailRenderers)
						trailRenderer.enabled = false;
					
					foreach (ParticleSystem particleSystem in particleSystems)
						particleSystem.enableEmission = false;
				}
				
				RepositionMenuObject repositionMenuObjectScript = levelSelectMenuObject.menuObject.GetComponent<RepositionMenuObject>();
				repositionMenuObjectScript.Reposition(levelSelectMenuObject.position);
			}
			else
			{
				if (levelSelectMenuObject.activate)
					levelSelectMenuObject.menuObject.SetActive(true);
				else
					levelSelectMenuObject.menuObject.SetActive(false);
			}
		}
	}

	void FadeInLabel(TweenAlpha labelTween, float fadeTime = 1f, float fadeDelay = 0f)
	{
		labelTween.from = labelTween.value;
		labelTween.to = 1f;
		labelTween.duration = 1f;
		labelTween.delay = 0.5f;
		labelTween.ResetToBeginning();
		labelTween.PlayForward();
	}

	void FadeOutLabel(TweenAlpha labelTween, float fadeTime = 1f, float fadeDelay = 0f)
	{
		labelTween.from = labelTween.value;
		labelTween.to = 0f;
		labelTween.duration = 0.25f;
		labelTween.delay = 0f;
		labelTween.ResetToBeginning();
		labelTween.PlayForward();
	}
}

[System.Serializable]
class MenuObject
{
	public string name = "Default";
	public GameObject menuObject = null;
	public bool menuLabel = false;
	public bool activate = false;
	public Vector3 position = Vector3.zero;
}