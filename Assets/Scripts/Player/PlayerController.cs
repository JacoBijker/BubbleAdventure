using UnityEngine;
using System.Collections;
using Assets.Scripts.Behavior;
using Assets.Scripts.Standalone;

public class PlayerController : MonoBehaviour
{
    public GameObject LevelManager;

    public Transform groundCheck;
    public float maxSpeed = 5f;
    public float jumpForce = 750;
    public GameObject bulletType;
    public float bulletVelocity = 400;
    public float shootSpeed = 2;
    public LayerMask whatIsGround;

    public GUIStyle UpButton;
    public GUIStyle LeftButton;
    public GUIStyle RightButton;
    public GUIStyle ShootButton;

    private bool facingRight = true;
    private bool grounded = false;
    private float groundRadius = 0.2f;

    private Animator animator;
    private float shootTime = 1;
    private Collider2D[] playerColliders;
    private bool isActive = true;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private Flicker flicker;

    private float btnMove;
    private bool btnShoot;
    private bool btnJump;
    private bool IsOnScreen;

    private GUIScaler guiScaler;
    void Start()
    {
        guiScaler = new GUIScaler();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        flicker = new Flicker(this.spriteRenderer);

        IsOnScreen = Application.streamingAssetsPath.Contains("://");
    }

    void OnGUI()
    {
        if (!IsOnScreen)
            return;

        guiScaler.Set(GUI.matrix);
        GUI.matrix = guiScaler.GetScaled();

        Vector2 midPoint = new Vector2(100, 500);
        Vector2 inputPoint = new Vector2(100, 30);

        var scaleX = Screen.width / 1024;
        var scaleY = Screen.height / 600;

        Rect leftButtonRect = new Rect(19 * scaleX, inputPoint.y * scaleY, 76 * scaleX, 71 * scaleY);
        Rect rightButtonRect = new Rect(181 * scaleX, inputPoint.y * scaleY, 76 * scaleX, 71 * scaleY);

        //Rect leftButtonRect = new Rect(midPoint.x - 81, inputPoint.y, 76, 71);
        //Rect rightButtonRect = new Rect(midPoint.x + 81, inputPoint.y, 76, 71);

        bool btnPressed = false;
        //var touch = Input.GetTouch(0);
        //if (touch.position) ;
        if (Input.GetMouseButton(0))
        {
            var touchpos = Input.mousePosition;
            if (leftButtonRect.Contains(touchpos))
            {
                btnMove -= 0.075f;
                btnPressed = true;
            }

            if (rightButtonRect.Contains(touchpos))
            {
                btnMove += 0.075f;
                btnPressed = true;
            }
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            var touchpos = Input.GetTouch(i).position;
            if (leftButtonRect.Contains(touchpos))
            {
                btnMove -= 0.075f;
                btnPressed = true;
            }

            if (rightButtonRect.Contains(touchpos))
            {
                btnMove += 0.075f;
                btnPressed = true;
            }
        }


        GUI.RepeatButton(new Rect(midPoint.x - 81, midPoint.y, 76, 71), "", LeftButton);
        //{
        //    btnMove -= 0.075f;
        //    btnPressed = true;
        //}

        GUI.RepeatButton(new Rect(midPoint.x + 81, midPoint.y, 76, 71), "", RightButton);
        //{
        //    btnMove += 0.075f;
        //    btnPressed = true;
        //}

        if (!btnPressed)
        {
            if (btnMove > 0.02f)
                btnMove = Mathf.Max(0, btnMove - 0.025f);
            else if (btnMove < 0.04f)
                btnMove = Mathf.Min(0, btnMove + 0.025f);
        }
        //else
        //btnMove = 0;
        btnMove = Mathf.Clamp(btnMove, -1, 1);

        if (GUI.Button(new Rect(934, midPoint.y - 135, 61, 75), "", UpButton))
            btnJump = true;
        if (GUI.Button(new Rect(924, midPoint.y, 80, 80), "", ShootButton))
            btnShoot = true;

        GUI.matrix = guiScaler.GetOriginal();
    }



    private bool GetFireInput()
    {
        if (!IsOnScreen)
            return Input.GetMouseButton(0);
        else
            return btnShoot;
    }

    private bool GetJumpInput()
    {
        if (!IsOnScreen)
            return Input.GetKeyDown(KeyCode.Space);
        else
            return btnJump;
    }

    private float GetMoveInput()
    {
        if (!IsOnScreen)
            return Input.GetAxis("Horizontal");
        else
            return btnMove;
    }

    void FixedUpdate()
    {
        if (!isActive)
            return;

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        animator.SetBool("Grounded", grounded);
        float move = GetMoveInput();
        //Debug.Log(move.ToString());

        animator.SetFloat("JumpSpeed", GetComponent<Rigidbody2D>().velocity.y);
        animator.SetFloat("MoveSpeed", Mathf.Abs(move));

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        //if (Mathf.Abs(move) > 0.01 && !audioSource.isPlaying)
        //    audioSource.PlayRandomClipFromList(Global.footStepClips);

        if ((move > 0 && !facingRight) || (move < 0 && facingRight))
            Flip();

        flicker.Update();
    }

    void Update()
    {
        if (!isActive)
            return;

        shootTime -= Time.deltaTime * shootSpeed;
        if (grounded && GetJumpInput())
        {
            btnJump = false;
            animator.SetBool("Grounded", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        }

        if (shootTime <= 0 && GetFireInput())
        {
            btnShoot = false;
            shootTime = 1;
            var xOffset = facingRight ? 0.7f : -0.7f;
            Vector3 bulletPosition = new Vector3(this.transform.position.x + xOffset, this.transform.position.y, this.transform.position.z);

            var newBullet = Instantiate(bulletType, bulletPosition, Quaternion.identity) as GameObject;
            BubbleManager.Add(newBullet);
            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(xOffset * bulletVelocity, 0));
            newBullet.GetComponent<Attack_Bubble>().SetLevelManager(LevelManager);
            audioSource.PlayRandomClipFromList(Global.attackClips);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Kill()
    {
        if (flicker.IsActive)
            return;

        audioSource.PlayRandomClipFromList(Global.dieClips);
        isActive = false;
        Invoke("Respawn", 1.5f);
        animator.SetBool("Dead", true);
        SetColliders(false);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 350f));
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Enemy")
        {
            Kill();
        }
    }

    void Respawn()
    {
        SetColliders(true);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isActive = true;
        Reset();
        LevelManager.SendMessage("Respawn");
        animator.SetBool("Dead", false);
        flicker.Start();
        Invoke("Vulnerable", 2f);
    }

    void Vulnerable()
    {
        flicker.Stop();
    }

    public void Reset()
    {
        if (!facingRight)
            Flip();
    }

    void SetColliders(bool active)
    {
        playerColliders = playerColliders ?? GetComponents<Collider2D>();
        foreach (var collider in playerColliders)
            collider.enabled = active;
    }
}
