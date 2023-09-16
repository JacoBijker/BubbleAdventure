using UnityEngine;
using System.Collections;

public class BeeMove : MonoBehaviour
{
    public float Speed = 2f;

    public Transform TopCheck;
    public Transform BotCheck;
    public Transform FrontCheck;

    private int x = 1;
    private int y = 1;

    private bool Collides(Transform toCheck)
    {
        var collisionInfo = (Physics2D.OverlapCircle(toCheck.position, 0.1f));
        if (collisionInfo != null && (collisionInfo.gameObject.tag == "LevelBlock" || collisionInfo.gameObject.tag == "Enemy"))
            return true;

        return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        if (Collides(FrontCheck))
        {
            Vector3 theScale = transform.localScale;
            theScale.x = theScale.x * -1;
            transform.localScale = theScale;
            x *= -1;
        } 

        if (Collides(TopCheck))
        {
            y = -1;
        } 
        
        if (Collides(BotCheck))
        {
            y = 1;
        }

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(x * Speed, y * Speed);
    }
}
