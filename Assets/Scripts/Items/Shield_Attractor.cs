using UnityEngine;
using System.Collections;
using Assets.Scripts.Behavior;
using System.Linq;

public class Shield_Attractor : MonoBehaviour {

    private Flicker flicker;
    private Scaling scaling;

	void Start () {
        flicker = new Assets.Scripts.Behavior.Flicker(this.gameObject);
        scaling = new Scaling(this.transform);
        Invoke("Flicker", 10f);
	}

    void FixedUpdate()
    {
        flicker.Update();
        scaling.Update();
        var allBubbles = BubbleManager.GetBubbles();
        foreach(var bubble in allBubbles.Where(s => !s.IsActive))
        {
            Vector3 newVelocity = (this.transform.position - bubble.transform.position);
            newVelocity.Normalize();
            bubble.GetComponent<Rigidbody2D>().velocity = newVelocity;
        }

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
        Destroy(transform.parent.gameObject);
    }


}
