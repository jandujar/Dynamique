using UnityEngine;
using System.Collections;

public class LevelSelectButton : MonoBehaviour
{
	[SerializeField] TriggerLevelLoad triggerLevelLoad;
	[SerializeField] UIPanel levelPanel;
	[SerializeField] UISprite[] starSprites;
	int levelNumber = 0;

	void Awake()
	{
		levelNumber = triggerLevelLoad.LevelToLoad;
		int levelStatus = EncryptedPlayerPrefs.GetInt("Level " + (levelNumber - 1) + " Status", 0);

		if (levelStatus == 1)
		{
			levelPanel.alpha = 1;
			int starsEarned = EncryptedPlayerPrefs.GetInt("Level " + (levelNumber - 1) + " Stars", 0);

			if (starsEarned > 0)
			{
				switch(starsEarned)
				{
				case 1:
					starSprites[0].color = Color.white;
					starSprites[1].color = Color.black;
					starSprites[2].color = Color.black;
					break;
				case 2:
					starSprites[0].color = Color.white;
					starSprites[1].color = Color.white;
					starSprites[2].color = Color.black;
					break;
				case 3:
					starSprites[0].color = Color.white;
					starSprites[1].color = Color.white;
					starSprites[2].color = Color.white;
					break;
				}
			}
			else
			{
				foreach (UISprite starSprite in starSprites)
					starSprite.color = Color.black;
			}
		}
		else
		{
			Collider buttonCollider = transform.GetComponent<Collider>();
			buttonCollider.enabled = false;

			levelPanel.alpha = 0.4f;

			foreach (UISprite starSprite in starSprites)
				starSprite.color = Color.black;
		}
	}
}
