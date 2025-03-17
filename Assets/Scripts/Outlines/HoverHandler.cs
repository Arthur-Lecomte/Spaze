using UnityEngine;

public class HoverHandler : MonoBehaviour
{
    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError("Outline component not found on the object.");
        }
    }

    private void OnMouseEnter()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}