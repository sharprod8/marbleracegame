using TMPro;
using UnityEngine;

public class TitleColourWave : MonoBehaviour
{
    private TMP_Text textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        textMesh.ForceMeshUpdate();

        TMP_TextInfo textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            Color32[] colours = textInfo.meshInfo[materialIndex].colors32;

            Color colour = Color.HSVToRGB( Mathf.Repeat((i * 0.08f) + Time.time * 0.15f, 1f), 0.6f, 1f);

            colours[vertexIndex + 0] = colour;
            colours[vertexIndex + 1] = colour;
            colours[vertexIndex + 2] = colour;
            colours[vertexIndex + 3] = colour;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;

            textMesh.UpdateGeometry( textInfo.meshInfo[i].mesh, i);
        }
    }
}