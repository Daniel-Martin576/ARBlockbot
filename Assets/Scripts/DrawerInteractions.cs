using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInteractions : MonoBehaviour
{
  private GameObject[] drawerElements;
  private float distanceToTop = (float) (105.0 + 277.1);
  private GameObject upArrow;
  private GameObject downArrow;

    // Start is called before the first frame update
    void Start()
    {
      upArrow = GameObject.Find("dButton").transform.Find("up-arrow").gameObject;
      downArrow = GameObject.Find("dButton").transform.Find("down-arrow").gameObject;
    }

    // moving the drawer up + switching the arrows
    public void DrawerUp() {

      drawerElements = GameObject.FindGameObjectsWithTag("drawGroup");
      foreach (GameObject go in drawerElements) {
        RectTransform rtf = go.GetComponent<RectTransform>();
        rtf.position = new Vector3(rtf.position.x, rtf.position.y + distanceToTop, rtf.position.z);
      }
      upArrow.GetComponent<Renderer>().enabled = false;
      downArrow.GetComponent<Renderer>().enabled = true;
    }

    // moving the drawer down + switching the arrows
    public void DrawerDown() {
      drawerElements = GameObject.FindGameObjectsWithTag("drawGroup");
      foreach (GameObject go in drawerElements) {
        RectTransform rtf = go.GetComponent<RectTransform>();
        rtf.position = new Vector3(rtf.position.x, rtf.position.y - distanceToTop, rtf.position.z);
        upArrow.GetComponent<Renderer>().enabled = true;
        downArrow.GetComponent<Renderer>().enabled = false;
      }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
