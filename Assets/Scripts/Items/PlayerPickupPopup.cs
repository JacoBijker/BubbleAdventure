using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Standalone;

public class PlayerPickupPopup : MonoBehaviour
{
    private Vector3 RealWorldPosition;
    private string Text;
    private AudioSource audioSource;
    private Text textComponent; // Reference to the Text component

    // Use this for initialization
    void Start()
    {
        textComponent = GetComponent<Text>();

        if (!textComponent)
        {
            Debug.Log("HUDFPS needs a Text component!");
            enabled = false;
            return;
        }
    }

    void Awake()
    {
        Invoke("Stop", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        float yOffset = 0.2f * Time.deltaTime;
        RealWorldPosition = new Vector3(RealWorldPosition.x, RealWorldPosition.y + yOffset, RealWorldPosition.z);
        this.transform.position = Camera.main.WorldToViewportPoint(RealWorldPosition);

    }

    public void Setup(Vector3 Position, string Text, int bubbleChainCount)
    {
        RealWorldPosition = Position;

        if (!textComponent)
            textComponent = GetComponent<Text>();

        textComponent.text = Text;

        var currentFontSize = textComponent.fontSize;
        var aspect = Mathf.Max(Screen.width / 1024f, Screen.height / 600f);

        textComponent.fontSize = (int)Mathf.Round(currentFontSize * aspect);

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayClipFromList(Global.pickupClips, bubbleChainCount);
    }

    private void Stop()
    {
        Destroy(this.gameObject);
    }
}
