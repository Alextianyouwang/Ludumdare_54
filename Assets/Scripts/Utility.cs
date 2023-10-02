
using UnityEngine;

public static class Utility 
{
    public static Bounds GetObjectBound(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            for (int i = 1, ni = renderers.Length; i < ni; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            return bounds;
        }
        else
        {
            return new Bounds();
        }
    }

    public static Vector3 ScreenToWorldPoint(Vector2 screenPoint, string mask)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, 100f, LayerMask.GetMask(mask)))
            return hit.point;
        else return Vector3.zero;
    }
}
