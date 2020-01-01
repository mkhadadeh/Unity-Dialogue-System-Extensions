using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomTMPTag : ScriptableObject
{
    // Todo Implement no_closing_tag property
    protected bool hasTextChanged = true;
    public abstract string tag_name { get; }
    public abstract IEnumerator applyToText(TMPro.TextMeshPro text, int startIndex, int length, string param);
    public CustomTMPTag clone() {
        return (CustomTMPTag)this.MemberwiseClone();
        }
    public void onTextChange()
    {
        hasTextChanged = true;
    }
}
