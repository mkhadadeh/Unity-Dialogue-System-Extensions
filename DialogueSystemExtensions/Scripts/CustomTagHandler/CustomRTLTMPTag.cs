using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomRTLTMPTag : ScriptableObject
{
    protected bool hasTextChanged = true;
    public abstract string tag_name { get; }
    public abstract bool needs_closing_tag { get; }
    public abstract IEnumerator applyToText(RTLTMPro.RTLTextMeshPro text, int startIndex, int length, string param);
    public CustomRTLTMPTag clone() {
        return (CustomRTLTMPTag)this.MemberwiseClone();
        }
    public void onTextChange()
    {
        hasTextChanged = true;
    }
}
