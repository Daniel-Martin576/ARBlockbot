using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject categoryScroll;
    // public GameObject package;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
      // categoryScroll = GameObject.Find("Category Scroll");
      // GameObject.Find("Category Scroll").SetActive(false);
      categoryScroll.SetActive(false);
      // package.SetActive(false);
      count = 0;
    }

    public void changeButton() {
      count++;
      GameObject thisHighlight = this.gameObject.transform.GetChild(0).gameObject;
      Image thisImage = thisHighlight.GetComponent<Image>();
      Image lastImg = HighlightTracker.lastHighlight;
      // GameObject lastPack = PackageTracker.lastPack;
      GameObject lastScroll = ScrollTracker.lastScroll;

      if (thisImage != lastImg) {
        thisImage.enabled = true;
        count = 1;
        // package.SetActive(true);
        if (lastImg != null) {
          lastImg.enabled = false;
        }
        if (lastScroll != null) {
          lastScroll.SetActive(false);
        }
      }
      else {
        if (count % 2 == 0) {
          thisImage.enabled = false;
          categoryScroll.SetActive(false);
          // package.SetActive(false);
        }

        else {
          thisImage.enabled = true;
          categoryScroll.SetActive(true);
          // package.SetActive(true);
        }

      }
      HighlightTracker.lastHighlight = thisImage;
      ScrollTracker.lastScroll = categoryScroll;
      // PackageTracker.lastPack = package;
      // if (categoryScroll == null) {
      //   Debug.Log("nooo");
      // }
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
