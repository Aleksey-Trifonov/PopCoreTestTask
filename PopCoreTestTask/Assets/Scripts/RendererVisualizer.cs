using UnityEngine;

public class RendererVisualizer : MonoBehaviour
{
    void Start()
    {
        var parent = transform.parent;

        var renderer = GetComponent<Renderer>();
        var parentRenderer = parent.GetComponent<Renderer>();
        if (parentRenderer != null)
        {
            renderer.sortingLayerID = parentRenderer.sortingLayerID;
            renderer.sortingOrder = parentRenderer.sortingOrder;
        }
        else
        {
            renderer.sortingLayerID = SortingLayer.NameToID("GameplayObjects");
            renderer.sortingOrder = 0;
        }
    }
}