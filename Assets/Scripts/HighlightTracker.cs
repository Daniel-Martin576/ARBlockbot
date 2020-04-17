using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightTracker : MonoBehaviour
{
  public static Image lastHighlight;
    // Start is called before the first frame update
    void Start()
    {
        GameObject highlight = GameObject.Find("Button").transform.GetChild(0).gameObject;
        Image im = highlight.GetComponent<Image>();
        im.enabled = true;
        lastHighlight = im;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
