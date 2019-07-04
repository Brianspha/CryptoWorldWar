using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    // Start is called before the first frame update
    public Color HoverColor = Color.red;
    public Color DefaultColor = Color.white;

    private void OnMouseOver()
    {
        GetComponent<MeshRenderer>().material.color = HoverColor;
    }
    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.color = DefaultColor;
        
    }
    private void OnMouseEnter()
    {
        GetComponent<Material>().color = HoverColor;

    }
}
