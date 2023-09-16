using UnityEngine;
using System.Collections;

public class Attack_Spider : MonoBehaviour {

    public GameObject attackObject;
    public float attackChance = 0.05f;
	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate()
    {
        if (Random.value < attackChance)
        {
            Vector3 bulletPosition = new Vector3(this.transform.position.x, this.transform.position.y - 0.3f, this.transform.position.z);
            var newBullet = Instantiate(attackObject, bulletPosition, Quaternion.identity) as GameObject;
            //audioSource.PlayRandomClipFromList(Global.attackClips);
        }
    }
}
