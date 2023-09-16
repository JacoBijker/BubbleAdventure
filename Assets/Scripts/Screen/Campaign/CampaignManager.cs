using UnityEngine;
using System.Collections;

public class CampaignManager : MonoBehaviour {

    private Campaign current;
    private LevelManager levelManager;
    private int currentLevel = 1;

	void Start () {
        levelManager = GetComponentInChildren<LevelManager>();
        levelManager.SetCampaignManager(this);
	}

    void Awake()
    {
        Invoke("LoadDefault", 1f);
    }

    void LoadDefault()
    {
        if (current != null)
            return;

        LoadCampaign("Default.cmp");
        LoadNextLevel();
    }

    public void LoadCampaign(string fileName)
    {
        current = new Campaign(fileName);
    }

    public bool HasNextLevel()
    {
        return current.HasNextLevel();
    }

    public void LoadNextLevel()
    {
        var nextLevelName = current.NextLevel();
        levelManager.LoadLevel(nextLevelName, current.GetCurrentLevel());
    }
}
