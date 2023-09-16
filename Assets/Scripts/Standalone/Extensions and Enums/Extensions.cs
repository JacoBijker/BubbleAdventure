using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Extensions
{
    public static void PlayRandomClipFromList(this AudioSource @this, AudioClip[] clips)
    {
        if (@this.isPlaying)
            @this.Stop();

        var index = Random.Range(0, clips.Length);
        @this.clip = clips[index];
        @this.Play();
    }

    public static void PlayClipFromList(this AudioSource @this, AudioClip[] clips, int index)
    {
        if (@this.isPlaying)
            @this.Stop();

        if (index >= clips.Length)
            index = 0;

        @this.clip = clips[index];
        @this.Play();
    }
}
