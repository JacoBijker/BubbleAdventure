using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{

    private static bool shaking = false;

    private float shakeInterval = 0.2f;
    private float currentTime = 0f;
    private float range = 0.5f;

    void Start()
    { }

    private Vector3 destination;

    void Update()
    {
        //currentTime += Time.deltaTime;

        //if (currentTime > shakeInterval)
        //{
        //    var x = Random.Range(-range, range);
        //    var y = Random.Range(-range, range);
            
        //    destination = new Vector3(x, y, this.transform.position.z);
        //    currentTime = 0;
        //}

        //var progress = currentTime / shakeInterval;
        //transform.position = new Vector3(destination.x * progress, destination.y * progress, 0);
    }

    public static void Shake()
    {

    }
}
