
using UnityEngine;

public static class Utility 
{
    public static Bounds GetObjectBound(GameObject obj)
    {
        Bounds b = new Bounds();
        MeshRenderer[] rends = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rend in rends)
        {
            b.center = rend.bounds.center;
            b.Encapsulate(rend.bounds);
        }
        return b;
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
