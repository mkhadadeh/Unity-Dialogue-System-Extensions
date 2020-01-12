# Unity Dialogue System Extensions (with RTL support)
Note: *This set of extensions does not work with Yarn 1.0. Currently, it works with Yarn 0.9.11*
 This project contains a few scripts that extend YarnSpinner, TextMesh Pro, and RTL TextMeshPro. These scripts integrate YarnSpinner with TextMesh Pro, allow the creation of custom rich text tags, and allow the YarnSpinner to display RTL text.

## YarnSpinner and TextMesh Pro
 YarnSpinner works with regular Unity UI Text, which doesn't allow for styling of specific parts of the text. For example, if you wanted to italicize a part of the text, that wouldn't work with regular old Unity UI. The `TMPDialogueUI` Component allows YarnSpinner to access TextMesh Pro Components and use them for dialogue rather than using Unity UI Text. Just make sure you aren't using the TextMesh Pro UGUI component.
 
 Note: *If you are using TextMesh Pro to color parts of the text, using Hex Codes requires a #. YarnSpinner doesn't work well with #'s because it uses them to denote comments. So I wrote a workaround for that. Just write your TextMesh Pro color styling tag without the #. After Yarn has parsed it, it will add the # in to let TextMesh Pro parse it. Ex: `<color=FFFFFF>` instead of `<color=#FFFFFF>`*

## Custom Tags
 To create a custom tag to apply to Yarn text, simply extend the `CustomTMPTag` class.
 You will need to override three things for each custom tag you create:
 - The `tag_name` property is the name the parser will be looking for when it is parsing your text
 - The `needs_closing_tag` property is whether or not the tag needs an end tag to be parsed correctly. 
 
   Note: *If false, the parser will not look for a closing tag, and therefore, if a closing tag is in the text, it will not be parsed.*
 - The 'applyToText()' function is where you specify what you want to happen to your text that is affected by the tag.
 
   Note: *If `needs_closing_tag` is false, then the affected text is the rest of the string past the tag.*
   
 After creating the script for the tag, create an object of the tag type in the editor and store it in `DialogueSystemExtensions/Resources/CustomTags/` so that the parser will know to look for the tag.
## RTL Dialogue and YarnSpinner
 Note: *If you do not need RTL support, switch to the master branch. That branch does not include RTL support.*
 
 RTL Dialogue is implemented using @mnarimani's RTL Text Mesh Pro plugin found here https://github.com/mnarimani/RTLTMPro. To use Yarn Dialogue with RTL Text Mesh Pro, use the `TMPRTLDialogueUI` Component. This will work the same way the TMPDialogueUI Component works, but with RTL Text Mesh Pro components.
 
 ### RTL Custom Tags
 You can create custom tags for RTL Dialogue. These tags must extend from `CustomRTLTMPTag`. They work the same way Custom Tags work.
 
 Note: *Tags extending from `CustomTMPTag` are not parsed for RTL Text. Tags extending from `CustomRTLTMPTag` are not parsed for text that does not use the RTL Text Mesh Pro component*
 
 After coding RTL Custom Tags, create an object of the tag type in the editor and store it in `DialogueSystemExtensions/Resources/CustomRTLTags/` so that the parser will know to look for the tag.
