using UnityEngine;
using System.Collections;
using Assets.Scripts.Behavior;

public class Shield_Bubblefy : MonoBehaviour {

    public GameObject Bubble;

    private Flicker flicker;
    private Scaling scaling;

	// Use this for initialization
	void Start () {
        flicker = new Assets.Scripts.Behavior.Flicker(this.gameObject);
        scaling = new Scaling(this.transform);
        Invoke("Flicker", 5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        flicker.Update();
        scaling.Update();

        if (!scaling.IsActive)
            scaling.Start();
	}

    void Flicker()
    {
        flicker.Start();
        Invoke("Remove", 2f);
    }

    void Remove()
    {
        flicker.Stop();
        scaling.Stop();
        Destroy(transform.parent.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Enemy")
        {
            var newBullet = Instantiate(Bubble, collisionInfo.transform.position, Quaternion.identity) as GameObject;
            BubbleManager.Add(newBullet);
            newBullet.GetComponent<Attack_Bubble>().SetLevelManager(Global.levelManager);
        }
    }
}
