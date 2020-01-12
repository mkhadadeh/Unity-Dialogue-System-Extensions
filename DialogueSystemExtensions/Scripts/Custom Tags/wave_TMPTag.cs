using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveTag", menuName = "CustomTags/WaveTag")]
public class wave_TMPTag : CustomTMPTag
{

    public override string tag_name
    {
        get
        {
            return "wave";
        }
    }

    public override bool needs_closing_tag
    {
        get
        {
            return true;
        }
    }

    public override IEnumerator applyToText(TMPro.TextMeshPro text, int startIndex, int length, string param)
    {
        // Set up

        // Parse param
        float speed = 0;
        float amplitude = 0.5f;
        bool multiple_params = false;
        for(int i = 0; i < param.Length; i++)
        {
            if(param[i] == ' ')
            {
                multiple_params = true;
                speed = float.Parse(param.Substring(0, i));
                amplitude = float.Parse(param.Substring(i + 1));
            }
        }
        if (!multiple_params)
        {
            speed = float.Parse(param);
        }

        var textInfo = text.textInfo;
        Matrix4x4 transformMatrix;

        // Cache the vertex data of the text object as the Jitter FX is applied to the original position of the characters.
        TMPro.TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

        while (true)
        {
            // Get new copy of vertex data if the text has changed.
            if (hasTextChanged)
            {
                // Update the copy of the vertex data for the text object.
                //cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
                hasTextChanged = false;
            }
            // Applying transformations
            for (int i = startIndex; i < startIndex + length; i++)
            {
                TMPro.TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                // Skip characters that are not visible and thus have no geometry to manipulate.
                if (!charInfo.isVisible)
                    continue;

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Get the cached vertices of the mesh used by this text element (character or sprite).
                Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

                // Determine the center point of each character.
                Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
                // This is needed so the matrix TRS is applied at the origin for each character.
                Vector3 offset = charMidBasline;

                Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
                
                destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;
                
                // This stuff is what actually matters
                Vector3 waveOffset = new Vector3(0, amplitude * Mathf.Sin(Time.fixedTime * speed + (i * 0.5f)), 0);

                transformMatrix = Matrix4x4.TRS(waveOffset, Quaternion.Euler(0, 0, 0), Vector3.one);
                
                // Back to stuff that doesn't
                destinationVertices[vertexIndex + 0] = transformMatrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                destinationVertices[vertexIndex + 1] = transformMatrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                destinationVertices[vertexIndex + 2] = transformMatrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                destinationVertices[vertexIndex + 3] = transformMatrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);
                
                destinationVertices[vertexIndex + 0] += offset;
                destinationVertices[vertexIndex + 1] += offset;
                destinationVertices[vertexIndex + 2] += offset;
                destinationVertices[vertexIndex + 3] += offset;
                
            }
            // Push changes into meshes
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
