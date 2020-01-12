# Unity Dialogue System Extensions
Note: *This set of extensions does not work with Yarn 1.0. Currently, it works with Yarn 0.9.11.*

 This project contains a few scripts that extend YarnSpinner, TextMesh Pro, and RTL TextMeshPro. These scripts integrate YarnSpinner with TextMesh Pro, allow the creation of custom rich text tags, and allow YarnSpinner to display RTL text.

## YarnSpinner and TextMesh Pro
 YarnSpinner works with regular Unity UI Text, which doesn't allow for styling of specific parts of the text. For example, if you wanted to italicize a part of the text, that wouldn't work with regular old Unity UI. The `TMPDialogueUI` component in this set of extensions allows YarnSpinner to access TextMesh Pro Components and use them for dialogue rather than using Unity UI Text. Just make sure you aren't using the TextMesh Pro UGUI component.
 
 Note: *If you are using TextMesh Pro to color parts of the text, using Hex Codes requires a #. YarnSpinner doesn't work well with #'s because it uses them to denote comments. So I wrote a workaround for that. Just write your TextMesh Pro color styling tag without the #. After Yarn has parsed it, the component will add the # in to let TextMesh Pro parse it. Ex: `<color=FFFFFF>` instead of `<color=#FFFFFF>`*

## Custom Tags
 To create a custom tag to apply to Yarn text, simply extend the `CustomTMPTag` class.
 You will need to override three things for each custom tag you create:
 - The `tag_name` property is the name the parser will be looking for when it is parsing your text.
 - The `needs_closing_tag` property is whether or not the tag needs an end tag to be parsed correctly. 
 
   Note: *If false, the parser will not look for a closing tag, and therefore, if a closing tag is in the text, it will not be parsed.*
 - The 'applyToText()' function is where you specify what you want to happen to your text that is affected by the tag.
 
   Note: *If `needs_closing_tag` is false, then the affected text is the rest of the string past the tag.*
   
 After creating the script for the tag, create an object of the tag type in the editor and store it in `Resources/CustomTags/` so that the parser will know to look for the tag.
## RTL Dialogue and YarnSpinner
 Note: *Switch to the RTL branch of this repository to get access to RTL support. The master branch does not include any RTL features.*
