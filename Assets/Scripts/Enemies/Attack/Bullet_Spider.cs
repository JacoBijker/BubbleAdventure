using UnityEngine;
using System.Collections;

public class Bullet_Spider : MonoBehaviour {

    public float activeTime = 7f;
    public float rotateSpeed = 250f;
	// Use this for initialization
	void Start () {
        Invoke("SelfDestruct", activeTime);
	}
	
    void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            collisionInfo.gameObject.SendMessage("Kill");
        }
    }
}
