using UnityEditor;
using UnityEngine;

public class UIMenuItems : MonoBehaviour
{
    [MenuItem("Anchors/Anchors to Corners %[")]
    static void AnchorsToCorners()
    {
        RectTransform rt = Selection.activeTransform as RectTransform;
        RectTransform rtb = Selection.activeTransform.parent as RectTransform;

        if (rt == null || rtb == null)
            return;

        Vector2 newAnchorsMin = new Vector2(rt.anchorMin.x + rt.offsetMin.x / rtb.rect.width,
                                          rt.anchorMin.y + rt.offsetMin.y / rtb.rect.height);
        Vector2 newAnchorsMax = new Vector2(rt.anchorMax.x + rt.offsetMax.x / rtb.rect.width,
                                           rt.anchorMax.y + rt.offsetMax.y / rtb.rect.height);
        rt.anchorMin = newAnchorsMin;
        rt.anchorMax = newAnchorsMax;
        rt.offsetMin = rt.offsetMax = new Vector2(0, 0);
    }
}
