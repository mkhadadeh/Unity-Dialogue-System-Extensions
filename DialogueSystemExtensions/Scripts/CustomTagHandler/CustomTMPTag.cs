using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomTMPTag : ScriptableObject
{
    protected bool hasTextChanged = true;
    public abstract string tag_name { get; }
    public abstract bool needs_closing_tag { get; }
    public abstract IEnumerator applyToText(TMPro.TextMeshPro text, int startIndex, int length, string param);
    public CustomTMPTag clone() {
        return (CustomTMPTag)this.MemberwiseClone();
        }
    public void onTextChange()
    {
        hasTextChanged = true;
    }
}
