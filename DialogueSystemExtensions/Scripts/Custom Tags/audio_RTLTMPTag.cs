using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioRTLTag", menuName = "CustomRTLTags/AudioTag")]
public class audio_RTLTMPTag : CustomRTLTMPTag
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

    public override IEnumerator applyToText(RTLTMPro.RTLTextMeshPro text, int startIndex, int length, string param)
    {
        AudioSource source = GameObject.Find("SceneAudio").GetComponent<AudioSource>();
        source.Stop();
        AudioClip clip = (AudioClip)Resources.Load("DialogueAudio/" + param);
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(0.0f);
    }
}
