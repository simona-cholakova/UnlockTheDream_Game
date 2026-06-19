using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenLink : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(
            textMeshPro,
            Input.mousePosition,
            null);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string url = linkInfo.GetLinkID();

            Application.OpenURL(url);
        }
    }
}