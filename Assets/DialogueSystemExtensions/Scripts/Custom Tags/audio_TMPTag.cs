using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioTag", menuName = "CustomTags/AudioTag")]
public class audio_TMPTag : CustomTMPTag
{

    public override string tag_name
    {
        get
        {
            return "audio";
        }
    }

    public override bool needs_closing_tag
    {
        get
        {
            return false;
        }
    }

    public override IEnumerator applyToText(TMPro.TextMeshPro text, int startIndex, int length, string param)
    {
        // Find the audio player and play the sound param from it.
        yield return new WaitForSeconds(0.1f);
    }
}
