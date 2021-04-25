using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentionMethods
{
    public static void MoveToLayer(this Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }
}
