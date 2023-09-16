using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleManager : MonoBehaviour {
    private static Transform holder;

    void Start()
    {
        holder = this.transform;
    }

    
    public static void Add(GameObject bubble)
    {
        bubble.transform.parent = holder;
    }

    public static void Clear()
    {
        foreach(var child in holder)
            Destroy((child as Transform).gameObject);
    }

    public static void BlowUp()
    {
        var allBubbles = holder.GetComponentsInChildren<Attack_Bubble>();
        foreach (var bubble in allBubbles)
            bubble.BlowUp();
    }

    public static IEnumerable<Attack_Bubble> GetBubbles()
    {
        return holder.GetComponentsInChildren<Attack_Bubble>();
    }
}
