using UnityEngine;
using System.Collections;

public enum SceneInformationMediumTask
{
    Play,
    None
}
public class SceneInformationMedium : MonoBehaviour {

    private static SceneInformationMediumTask action = SceneInformationMediumTask.None;
    private static string campaignName;

	void Start () {
	    if (action == SceneInformationMediumTask.Play)
        {
            Invoke("Play", 0.1f);
        }
	}

    void Play()
    {
        var campaignManager = GetComponentInChildren<CampaignManager>();
        campaignManager.LoadCampaign(campaignName);
        campaignManager.LoadNextLevel();
    }

	public static void SetCampaign(string name)
    {
        campaignName = name;
        action = SceneInformationMediumTask.Play;
    }

    public static void ClearTask()
    {
        action = SceneInformationMediumTask.None;
    }
}
