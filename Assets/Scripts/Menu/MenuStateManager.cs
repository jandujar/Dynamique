using UnityEngine;
using System.Collections;

public class MenuStateManager : MonoBehaviour
{
	[SerializeField] TweenAlpha titleTween;
	[SerializeField] MenuObject[] idleMenuObjects;
	[SerializeField] MenuObject[] stageSelectMenuObjects;
	[SerializeField] MenuObject[] levelSelectMenuObjects;
	[SerializeField] MenuObject[] optionsMenuObjects;

	public enum MenuState
	{
		Idle,
		Options,
		StageSelect,
		LevelSelect
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
		case MenuState.LevelSelect:
			LevelSelect();
			break;
		}
	}

	void Awake()
	{
		menuState = MenuState.Idle;
		SetState();
	}

	void Idle()
	{
		Debug.Log("Idle");
		FadeInTitle();

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
		Debug.Log("Options");
		FadeOutTitle();
	}

	void StageSelect()
	{
		Debug.Log("Stage Select");
		FadeOutTitle();

		foreach (MenuObject stageSelectMenuObject in stageSelectMenuObjects)
		{
			if (!stageSelectMenuObject.menuLabel)
			{
				Collider objectCollider = stageSelectMenuObject.menuObject.GetComponent<Collider>();
				TrailRenderer[] trailRenderers = stageSelectMenuObject.menuObject.GetComponentsInChildren<TrailRenderer>();
				ParticleSystem[] particleSystems = stageSelectMenuObject.menuObject.GetComponentsInChildren<ParticleSystem>();

				if (stageSelectMenuObject.activate)
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

	void LevelSelect()
	{
		Debug.Log("Level Select");
	}

	void FadeInTitle()
	{
		titleTween.from = titleTween.value;
		titleTween.to = 1f;
		titleTween.duration = 1f;
		titleTween.delay = 0.5f;
		titleTween.ResetToBeginning();
		titleTween.PlayForward();
	}

	void FadeOutTitle()
	{
		titleTween.from = titleTween.value;
		titleTween.to = 0f;
		titleTween.duration = 0.25f;
		titleTween.delay = 0f;
		titleTween.ResetToBeginning();
		titleTween.PlayForward();
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