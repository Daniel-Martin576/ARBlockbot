using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject categoryScroll;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
      // categoryScroll = GameObject.Find("Category Scroll");
      // GameObject.Find("Category Scroll").SetActive(false);
      categoryScroll.SetActive(false);
      count = 0;
    }

    public void changeButton() {
      count++;
      GameObject thisHighlight = this.gameObject.transform.GetChild(0).gameObject;
      Image thisImage = thisHighlight.GetComponent<Image>();
      Image lastImg = HighlightTracker.lastHighlight;
      if (thisImage != lastImg) {
        thisImage.enabled = true;
        lastImg.enabled = false;
      }
      HighlightTracker.lastHighlight = thisImage;
      if (categoryScroll == null) {
        Debug.Log("nooo");
      }
      if (count % 2 != 0) {
        categoryScroll.SetActive(true);
      } else {
        categoryScroll.SetActive(false);
      }

    }


    // Update is called once per frame
    void Update()
    {

    }
}
