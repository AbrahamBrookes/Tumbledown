using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassArrow : MonoBehaviour
{
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        // Scale up the arrow when the mouse enters
        transform.localScale = originalScale * 1.2f;
    }

    private void OnMouseExit()
    {
        // Scale back down when the mouse exits
        transform.localScale = originalScale;
    }
}
