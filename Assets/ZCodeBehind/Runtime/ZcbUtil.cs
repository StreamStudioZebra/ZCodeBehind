using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZcbUtil
{
    public static List<GameObject> GetDescendantList(GameObject goParent)
    {
        var goList = Resources.FindObjectsOfTypeAll<GameObject>().Where(go =>
        {
            while (true)
            {
                if (go == goParent)
                    return true;

                if (go.transform.parent == null)
                    return false;

                go = go.transform.parent.gameObject;
            }
        }).ToList();

        return goList;
    }
}