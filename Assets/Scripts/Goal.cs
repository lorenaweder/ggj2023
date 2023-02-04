using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public Transform image;

    Camera _mainCamera;
    Bounds _viewportBounds;

    private void Start()
    {
        _viewportBounds = new Bounds(Vector3.zero, Vector3.one * 2f);
        _mainCamera = Camera.main;
    }

    void Update()
    {
        var rawPosInViewport = _mainCamera.WorldToViewportPoint(transform.position);

        // Remaping to compare with bounds, since bounds is -1 to 1
        var remappedPosInViewport = new Vector3(rawPosInViewport.x.Remap(0f, 1f, -1f, 1f), rawPosInViewport.y.Remap(0f, 1f, -1f, 1f));

        if (!_viewportBounds.Contains(remappedPosInViewport))
        {   
            var pointOnEdge = _viewportBounds.ClosestPoint(remappedPosInViewport);
            // Back to viewport coordinates, random depth so it's not stuck in front of the camera
            var remappedPointOnEdge = new Vector3(pointOnEdge.x.Remap(-1f, 1f, 0f, 1f), pointOnEdge.y.Remap(-1f, 1f, 0f, 1f), 20f);
            var edgePointInWorld = _mainCamera.ViewportToWorldPoint(remappedPointOnEdge);
            
            image.position = edgePointInWorld;
            
            if (!image.gameObject.activeInHierarchy) image.gameObject.SetActive(true);
        }
        else if (image.gameObject.activeInHierarchy) image.gameObject.SetActive(false);
    }
}

public static class Helper
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
