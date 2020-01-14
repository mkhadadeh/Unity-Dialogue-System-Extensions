using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTagRunner : MonoBehaviour
{
    struct ParsedTagData
    {
        public int startIndex;
        public int length;
        public string tag_param;
        public string tag_name;
    }

    public TMPro.TextMeshProUGUI lineText;
    public Dictionary<string,CustomTMPTag> customTags;
    List<CustomTMPTag> clones;
    List<ParsedTagData> parsedTags;

    // Start is called before the first frame update
    void Awake()
    {
        customTags = new Dictionary<string, CustomTMPTag>();
        lineText = GetComponent<TMPro.TextMeshProUGUI>();
        parsedTags = new List<ParsedTagData>();
        clones = new List<CustomTMPTag>();
        var tags = Resources.LoadAll<CustomTMPTag>("CustomTags");
        foreach(var t in tags)
        {
            customTags.Add(t.tag_name, t);
        }
    }

    public void ClearParsedTags()
    {
        parsedTags.Clear();
    }

    public string ParseForCustomTags(string plainText)
    {
        // Returns a string without the custom tags and stores the parsed tag data in parsedTags
        string copyOfPlainText = plainText;
        int errorIndex = 0;
        string parsedText = "";

        while(plainText != "")
        {
            // Debug.Log(plainText + " and Parsed:" + parsedText);
            bool found = false;
            // Search for a start tag at this point
            foreach (var tag in customTags) {
                string param;
                int endOfStartTagIndex;
                if (plainText.StartsWith('<' + tag.Key)) // Found part of start tag
                {
                    // Assume no parameter
                    endOfStartTagIndex = tag.Key.Length + 1;
                    param = "";
                    // If there is a parameter, process it.
                    if(!plainText.StartsWith('<'+tag.Key+'>'))
                    {
                        // Param is given (or tag improperly formatted)
                        if(plainText[endOfStartTagIndex] != '=')
                        {
                            continue;
                        }
                        else
                        {
                            int realEndIndex = endOfStartTagIndex;
                            // Scan until > is found
                            while (plainText[realEndIndex] != '>')
                            {
                                realEndIndex++;
                            }
                            // Set param and end index of start tag
                            param = plainText.Substring(endOfStartTagIndex + 1, realEndIndex - endOfStartTagIndex - 1);
                            endOfStartTagIndex = realEndIndex;
                        }
                    }
                    // Check if tag type requires an end tag
                    if (tag.Value.needs_closing_tag)
                    {
                        // Found start tag of type tag. Look for end tag of same type.
                        for (int j = 0; j < plainText.Length; j++)
                        {
                            if (plainText.Substring(0, j + 1).EndsWith("</" + tag.Key + '>'))
                            {
                                // Found valid tag pair. 
                                found = true;
                                int startOfEndTagIndex = j - tag.Key.Length - 2;

                                // Create a copy of parsedText without tags
                                string noTagParsedText = removeTags(parsedText);

                                // Process and add plaintext version to parsed text.
                                string textInTag = plainText.Substring(endOfStartTagIndex + 1, startOfEndTagIndex - endOfStartTagIndex - 1);
                                parsedText += textInTag;
                                errorIndex = j + 1;


                                // Add parsed tag data to parsedTags
                                ParsedTagData parsedTag = new ParsedTagData();
                                parsedTag.tag_name = tag.Key;
                                parsedTag.tag_param = param;
                                parsedTag.startIndex = noTagParsedText.Length;
                                parsedTag.length = removeTags(textInTag).Length;
                                parsedTags.Add(parsedTag);


                                // Reset plaintext so that the parsed tag isn't a part of it
                                if (plainText.Length > j + 1)
                                {
                                    plainText = plainText.Substring(j + 1);
                                }
                                else
                                {
                                    plainText = "";
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Tag does not need start tag. Parse the tag so that the effect applies to the text following the tag.
                        found = true;
                        string textAfterTag = "";
                        if (plainText.Length > endOfStartTagIndex + 1) {
                            textAfterTag = plainText.Substring(endOfStartTagIndex + 1);
                        }
                        ParsedTagData parsedTag = new ParsedTagData();
                        parsedTag.tag_name = tag.Key;
                        parsedTag.tag_param = param;
                        parsedTag.startIndex = removeTags(parsedText).Length;
                        parsedTag.length = removeTags(textAfterTag).Length;
                        parsedTags.Add(parsedTag);
                        
                        // Reset the plaintext so that the tag isn't a part of it
                        if(plainText.Length > endOfStartTagIndex + 1)
                        {
                            plainText = plainText.Substring(endOfStartTagIndex + 1);
                        }
                        else
                        {
                            plainText = "";
                        }
                        errorIndex = endOfStartTagIndex + 1;
                    }
                }
                if(found)
                {
                    break;
                }
            }
            if(!found)
            {
                // No valid tag pair starts at this character so just add the character to the parsed text.
                parsedText += plainText[0];
                if (plainText.Length > 1)
                {
                    plainText = plainText.Substring(1);
                }
                else
                {
                    plainText = "";
                }
                errorIndex++;
            } 
        }
        return parsedText;
    }
    
    public void OnTextChange()
    {
        foreach(var clone in clones)
        {
            clone.onTextChange();
        }
    }

    public void ApplyTagEffects()
    {
        // Applies the effects of parsedTags on the text
        lineText = GetComponent<TMPro.TextMeshProUGUI>();
        lineText.ForceMeshUpdate();
        foreach(var tag in parsedTags)
        {
            var clonedTagRunner = customTags[tag.tag_name].clone();
            clones.Add(clonedTagRunner);
            StartCoroutine(clones[clones.Count-1].applyToText(lineText, tag.startIndex, tag.length, tag.tag_param));
        }
    }

    public void clearClones()
    {
        clones.Clear();
    }

    private string removeTags(string input)
    {
        // Helper function to remove all tags from text
        string noTagParsedText = "";
        bool inTag = false;
        for (int i = 0; i < input.Length; i++)
        {

            if (input[i] == '<')
            {
                inTag = true;
                continue;
            }
            if (input[i] == '>' && inTag)
            {
                inTag = false;
                continue;
            }
            if (!inTag)
            {
                noTagParsedText += input[i];
            }
        }
        return noTagParsedText;
    }
}

