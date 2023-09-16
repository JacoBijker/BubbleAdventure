using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;
using Assets.Scripts.Behavior;

public class Attack_Bubble : MonoBehaviour
{
    private static float lastBubblePickup;
    private static int bubbleChainCount;
    private static PlayerAccessor playerAccessor;
    private GameObject capturedObject;
    private bool isActive = true;
    private GameObject LevelManager;
    public GameObject playerPickupPopup;
    private SpriteRenderer spriteRenderer;
    private Flicker flicker;
    private Scaling scaling;
    private static object syncRoot;

    private Vector3 originalScaling;

    public bool IsActive
    {
        get
        {
            return isActive;
        }
    }

    void Start()
    {
        playerAccessor = playerAccessor ?? new PlayerAccessor();
        syncRoot = syncRoot ?? new object();

        var upgrades = playerAccessor.Player.BubbleSize * 0.1f;
        originalScaling = new Vector3(this.transform.localScale.x + upgrades, this.transform.localScale.y + upgrades, this.transform.localScale.z);
        
        this.transform.localScale = new Vector3(originalScaling.x * 1.2f, originalScaling.y * 0.9f, originalScaling.z);

        flicker = new Flicker(this.gameObject);
        scaling = new Scaling(this.transform, 0.3f);

        scaling.SetOriginalScale(originalScaling);

        var stopTime = playerAccessor.Player.BubbleDuration * 0.15f;
        Invoke("Stop", 1.05f + stopTime);
    }

    //For wobbly effect
    private float bubbleTimer;
    private float bubbleshift = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0.25f);
        else if (bubbleTimer >= bubbleshift)
        {
            bubbleTimer = 0;
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, Random.Range(-0.5f, 0.5f));
        }
        else
            bubbleTimer += Time.deltaTime;

        flicker.Update();
        scaling.Update();
    }

    void Stop()
    {
        isActive = false;
        this.transform.localScale = originalScaling;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0.45f);
        Invoke("SelfDestruct", 20f);
    }

    void SelfDestruct()
    {
        if (capturedObject == null)
            Destroy(this.gameObject);
    }

    public void BlowUp()
    {
        if (capturedObject != null)
        {
            var newScore = 0;
            lock (syncRoot)
            {
                var currentTime = Time.time;
                //Debug.Log("Pickup at: " + currentTime.ToString());
                if (currentTime - lastBubblePickup < 1)
                    bubbleChainCount++;
                else
                    bubbleChainCount = 1;

                lastBubblePickup = Time.time;
                newScore = 500 * bubbleChainCount;
                playerAccessor.Player.Score += newScore;
            }
            var newPickupPopup = Instantiate(playerPickupPopup) as GameObject;
            var newPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            newPickupPopup.GetComponent<PlayerPickupPopup>().Setup(newPosition, "+" + newScore.ToString(), bubbleChainCount);
            LevelManager.SendMessage("Killed_Enemy");
        }
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {

        if (!isActive && !scaling.IsActive)
            scaling.Start();

        if (collisionInfo.gameObject.tag == "Player")
        {
            if (capturedObject != null)
            {
                var currentTime = Time.time;
                //Debug.Log("Pickup at: " + currentTime.ToString());
                if (currentTime - lastBubblePickup < 1)
                    bubbleChainCount++;
                else
                    bubbleChainCount = 1;

                lastBubblePickup = Time.time;
                var newScore = 500 * bubbleChainCount;
                playerAccessor.Player.Score += newScore;
                var newPickupPopup = Instantiate(playerPickupPopup) as GameObject;
                var newPosition = new Vector3(collisionInfo.gameObject.transform.position.x, collisionInfo.gameObject.transform.position.y + 1, collisionInfo.gameObject.transform.position.z);
                newPickupPopup.GetComponent<PlayerPickupPopup>().Setup(newPosition, "+" + newScore.ToString(), bubbleChainCount);
                LevelManager.SendMessage("Killed_Enemy");
            }

            playerAccessor.Player.Score += 10;
            Destroy(this.gameObject);
        }

        if (isActive)
        {
            if (collisionInfo.gameObject.tag == "Enemy")
            {
                Stop();
                capturedObject = collisionInfo.gameObject;
                collisionInfo.gameObject.SetActive(false);
                GetComponent<SpriteRenderer>().sprite = Global.BubbleTypes[1];

                Invoke("StartFlash", 6f);
                Invoke("FreeEnemy", 8f);
            }
        }
    }

    void StartFlash()
    {
        flicker.Start();
    }

    void FreeEnemy()
    {
        flicker.Stop();
        capturedObject.transform.position = this.transform.position;
        capturedObject.SetActive(true);
        Destroy(this.gameObject);
    }

    public void SetLevelManager(GameObject levelManager)
    {
        this.LevelManager = levelManager;
    }
}
