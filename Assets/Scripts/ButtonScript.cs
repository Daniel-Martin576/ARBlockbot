using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

    public void showHighlight() {
      GameObject thisHighlight = this.gameObject.transform.GetChild(0).gameObject;
      Image thisImage = thisHighlight.GetComponent<Image>();
      Image lastImg = HighlightTracker.lastHighlight;
      if (thisImage != lastImg) {
        thisImage.enabled = true;
        lastImg.enabled = false;
      }
      HighlightTracker.lastHighlight = thisImage;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
