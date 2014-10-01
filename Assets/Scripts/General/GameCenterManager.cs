using UnityEngine;
using System.Collections;
// BELOW IS NEEDED FOR GAME CENTER
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterManager : MonoBehaviour
{
	// THE LEADERBOARD INSTANCE
	static ILeaderboard m_Leaderboard;

	string leaderboard1Name = "Overall Total Score";
	string leaderboard1ID = "overall_total_score";
	string leaderboard2Name = "Gravity Total Score";
	string leaderboard2ID = "gravity_total_score";
	string leaderboard3Name = "Anti-Gravity Total Score";
	string leaderboard3ID = "anti_gravity_total_score";
	string leaderboard4Name = "Worm Hole Total Score";
	string leaderboard4ID = "worm_hole_total_score";
	string leaderboard5Name = "Chaos Theory Total Score";
	string leaderboard5ID = "chaos_theory_total_score";

	GameObject newRecordObject;
	
// THIS MAKES SURE THE GAME CENTER INTEGRATION WILL ONLY WORK WHEN OPERATING ON AN APPLE IOS DEVICE (iPHONE, iPOD TOUCH, iPAD)
//#if UNITY_IPHONE
	
	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad(transform.gameObject);

		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		// AUTHENTICATE AND REGISTER A ProcessAuthentication CALLBACK
		// THIS CALL NEEDS OT BE MADE BEFORE WE CAN PROCEED TO OTHER CALLS IN THE Social API
        Social.localUser.Authenticate(ProcessAuthentication);
		
		// GET INSTANCE OF LEADERBOARD
		DoLeaderboard();

//		//Reset Achievements
//		GameCenterPlatform.ResetAllAchievements((resetResult) => {
//			Debug.Log(resetResult ? "Achievements have been Reset" : "Achievement reset failure.");
//		});

		SubmitScore(1, 0, 0);
		SubmitScore(2, 0, 0);
		SubmitScore(3, 0, 0);
		SubmitScore(4, 0, 0);
	}

	public void SubmitAchievement(string achievementID, double progress)
	{
		ReportAchievement(achievementID, progress);
	}
	
	public void SubmitScore(int stageNumber, int stageScore, int totalScore)
	{
		string stageLeaderboardID = null;

		switch (stageNumber)
		{
		case 1:
			stageLeaderboardID = leaderboard2ID;
			break;
		case 2:
			stageLeaderboardID = leaderboard3ID;
			break;
		case 3:
			stageLeaderboardID = leaderboard4ID;
			break;
		case 4:
			stageLeaderboardID = leaderboard5ID;
			break;
		default:
			Debug.LogError("Invalid stage number sent");
			break;
		}

		ReportScore(stageScore, stageLeaderboardID);
		ReportScore(totalScore, leaderboard1ID);
    }
	
	///////////////////////////////////////////////////
	// INITAL AUTHENTICATION (MUST BE DONE FIRST)
	///////////////////////////////////////////////////
	
	// THIS FUNCTION GETS CALLED WHEN AUTHENTICATION COMPLETES
	// NOTE THAT IF THE OPERATION IS SUCCESSFUL Social.localUser WILL CONTAIN DATA FROM THE GAME CENTER SERVER
    void ProcessAuthentication(bool success)
	{
        if (success)
		{
            Debug.Log ("Authenticated");

			// MAKE REQUEST TO GET LOADED ACHIEVEMENTS AND REGISTER A CALLBACK FOR PROCESSING THEM
			Social.LoadAchievements(ProcessLoadedAchievements); // ProcessLoadedAchievements FUNCTION CAN BE FOUND BELOW

			Social.LoadScores(leaderboard1Name, scores => {
    			if (scores.Length > 0) {
					// SHOW THE SCORES RECEIVED
        			Debug.Log ("Received " + scores.Length + " scores");
        			string myScores = "Leaderboard: \n";
        			foreach (IScore score in scores)
            			myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
        			Debug.Log (myScores);
    			}
    			else
        			Debug.Log ("No scores have been loaded.");
				});

			Social.LoadScores(leaderboard2Name, scores => {
				if (scores.Length > 0) {
					// SHOW THE SCORES RECEIVED
					Debug.Log ("Received " + scores.Length + " scores");
					string myScores = "Leaderboard: \n";
					foreach (IScore score in scores)
						myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
					Debug.Log (myScores);
				}
				else
					Debug.Log ("No scores have been loaded.");
			});

			Social.LoadScores(leaderboard3Name, scores => {
				if (scores.Length > 0) {
					// SHOW THE SCORES RECEIVED
					Debug.Log ("Received " + scores.Length + " scores");
					string myScores = "Leaderboard: \n";
					foreach (IScore score in scores)
						myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
					Debug.Log (myScores);
				}
				else
					Debug.Log ("No scores have been loaded.");
			});

			Social.LoadScores(leaderboard4Name, scores => {
				if (scores.Length > 0) {
					// SHOW THE SCORES RECEIVED
					Debug.Log ("Received " + scores.Length + " scores");
					string myScores = "Leaderboard: \n";
					foreach (IScore score in scores)
						myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
					Debug.Log (myScores);
				}
				else
					Debug.Log ("No scores have been loaded.");
			});

			Social.LoadScores(leaderboard5Name, scores => {
				if (scores.Length > 0) {
					// SHOW THE SCORES RECEIVED
					Debug.Log ("Received " + scores.Length + " scores");
					string myScores = "Leaderboard: \n";
					foreach (IScore score in scores)
						myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
					Debug.Log (myScores);
				}
				else
					Debug.Log ("No scores have been loaded.");
			});
        }
        else
		{
            Debug.Log ("Failed to authenticate with Game Center.");
		}

		if (Application.loadedLevel == 0)
			StartCoroutine(WaitAndLoadMainMenu());
    }

	// THIS FUNCTION GETS CALLED WHEN THE LoadAchievements CALL COMPLETES
	void ProcessLoadedAchievements (IAchievement[] achievements)
	{
		if (achievements.Length == 0)
			Debug.Log ("Error: no achievements found");
		else
			Debug.Log ("Got " + achievements.Length + " achievements");
	}
	
	///////////////////////////////////////////////////
	// GAME CENTER ACHIEVEMENT INTEGRATION
	///////////////////////////////////////////////////
	
	void ReportAchievement(string achievementId, double progress)
	{
		Social.ReportProgress(achievementId, progress, (result) => {
			Debug.Log(result ? string.Format("Successfully reported achievement {0}", achievementId) 
			          : string.Format("Failed to report achievement {0}", achievementId));
		});
	}

	#region Game Center Integration
	///////////////////////////////////////////////////
	// GAME CENTER LEADERBOARD INTEGRATION
	///////////////////////////////////////////////////
	
	/// <summary>
	/// Reports the score to the leaderboards.
	/// </summary>
	/// <param name="score">Score.</param>
	/// <param name="leaderboardID">Leaderboard I.</param>
	void ReportScore(long score, string leaderboardID)
	{
    	Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
    	Social.ReportScore (score, leaderboardID, success =>
		                    { Debug.Log(success ? "Reported score to leaderboard successfully" : "Failed to report score");});
	}
	
	/// <summary>
	/// Get the leaderboard.
	/// </summary>
	void DoLeaderboard()
	{
    	m_Leaderboard = Social.CreateLeaderboard();
    	m_Leaderboard.id = leaderboard1ID;
    	m_Leaderboard.LoadScores(result => DidLoadLeaderboard(result));
	}

	/// <summary>
	/// RETURNS THE NUMBER OF LEADERBOARD SCORES THAT WERE RECEIVED BY THE APP
	/// </summary>
	/// <param name="result">If set to <c>true</c> result.</param>
	void DidLoadLeaderboard(bool result)
	{
    	Debug.Log("Received " + m_Leaderboard.scores.Length + " scores");
    	foreach (IScore score in m_Leaderboard.scores)
		{
        	Debug.Log(score);
		}
	}
	#endregion

	IEnumerator WaitAndLoadMainMenu()
	{
		yield return new WaitForSeconds(1.0f); 
		Application.LoadLevel(1);
	}
}

//#endif
