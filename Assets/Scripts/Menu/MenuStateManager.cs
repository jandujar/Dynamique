using UnityEngine;
using System.Collections;

public class MenuStateManager : MonoBehaviour
{
	[SerializeField] MenuObject[] idleMenuObjects;
	[SerializeField] MenuObject[] unfoldMenuObjects;
	[SerializeField] MenuObject[] levelSelectMenuObjects;
	[SerializeField] MenuObject[] optionsMenuObjects;

	public enum MenuState
	{
		Idle,
		Unfold,
		LevelSelect,
		Options
	}
	
	public MenuState menuState;
	
	public void SetState()
	{
		switch(menuState)
		{
		case MenuState.Idle:
			Idle();
			break;
		case MenuState.Unfold:
			Unfold();
			break;
		case MenuState.LevelSelect:
			LevelSelect();
			break;
		case MenuState.Options:
			Options();
			break;
		}
	}

	void Awake()
	{
		SetState();
	}

	void Idle()
	{
		Debug.Log("Idle");

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

	void Unfold()
	{
		Debug.Log("Unfold");

		foreach (MenuObject unfoldMenuObject in unfoldMenuObjects)
		{
			if (!unfoldMenuObject.menuLabel)
			{
				Collider objectCollider = unfoldMenuObject.menuObject.GetComponent<Collider>();
				TrailRenderer[] trailRenderers = unfoldMenuObject.menuObject.GetComponentsInChildren<TrailRenderer>();
				ParticleSystem[] particleSystems = unfoldMenuObject.menuObject.GetComponentsInChildren<ParticleSystem>();

				if (unfoldMenuObject.activate)
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
				
				RepositionMenuObject repositionMenuObjectScript = unfoldMenuObject.menuObject.GetComponent<RepositionMenuObject>();
				repositionMenuObjectScript.Reposition(unfoldMenuObject.position);
			}
			else
			{
				if (unfoldMenuObject.activate)
					unfoldMenuObject.menuObject.SetActive(true);
				else
					unfoldMenuObject.menuObject.SetActive(false);
			}
		}
	}

	void LevelSelect()
	{
		Debug.Log("Level Select");

		foreach (MenuObject levelSelectMenuObject in levelSelectMenuObjects)
		{
			if (levelSelectMenuObject.activate)
				levelSelectMenuObject.menuObject.SetActive(true);
			else
				levelSelectMenuObject.menuObject.SetActive(false);
		}
	}

	void Options()
	{
		Debug.Log("Options");

		foreach (MenuObject optionsMenuObject in optionsMenuObjects)
		{
			if (optionsMenuObject.activate)
				optionsMenuObject.menuObject.SetActive(true);
			else
				optionsMenuObject.menuObject.SetActive(false);
		}
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