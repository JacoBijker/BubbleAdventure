using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

public class EmptySetupScript : MonoBehaviour, ISetupable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Setup(System.Collections.Generic.Dictionary<string, string> extraInformation)
    {
    }
}
