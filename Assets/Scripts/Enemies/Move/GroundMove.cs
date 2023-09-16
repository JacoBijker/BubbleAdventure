using UnityEngine;
using System.Collections;

public class GroundMove : MonoBehaviour
{
    private Transform wallCheck;
    private bool facingRight = false;

    public float moveSpeed = 3f;
    public float JumpChance = 5;
    public float JumpHeight = 12;
    // Use this for initialization
    void Start()
    {
        wallCheck = GetComponentsInChildren<Transform>()[1] as Transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var collisionInfo = (Physics2D.OverlapCircle(wallCheck.position, 0.1f));
        if (collisionInfo != null && (collisionInfo.gameObject.tag == "LevelBlock" || collisionInfo.gameObject.tag == "Enemy"))
            Flip();

        GetComponent<Rigidbody2D>().velocity = new Vector2(facingRight ? moveSpeed : -moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 0.01f)
        {
            var odds = Random.Range(0, 100);
            if (odds < JumpChance)
                GetComponent<Rigidbody2D>().velocity = new Vector2(facingRight ? moveSpeed : -moveSpeed, JumpHeight);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
