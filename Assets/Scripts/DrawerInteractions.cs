using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerInteractions : MonoBehaviour
{
  private GameObject[] drawerElements;
  private float cHeight;
  private float buttonHeight;
  private float distanceToTop = (float) 300.0;
  private GameObject upArrow;
  private GameObject downArrow;

    // Start is called before the first frame update
    void Start()
    {
      upArrow = GameObject.Find("dButton").transform.Find("up-arrow").gameObject;
      upArrow.GetComponent<Text>().text = "^";

      downArrow = GameObject.Find("dButton").transform.Find("down-arrow").gameObject;
      downArrow.GetComponent<Text>().text = "";

      GameObject canvas = GameObject.Find("Canvas");
      cHeight = canvas.GetComponent<Canvas>().pixelRect.height;

      buttonHeight = GameObject.Find("dButton").GetComponent<RectTransform>().rect.height;

      RectTransform r = GameObject.Find("Drawer").GetComponent<RectTransform>();
      r.position = new Vector3(r.position.x, -r.position.y + buttonHeight, r.position.z);

      distanceToTop = cHeight - buttonHeight;

      Debug.Log(cHeight);
    }

    // moving the drawer up + switching the arrows
    public void DrawerUp() {

      drawerElements = GameObject.FindGameObjectsWithTag("drawGroup");
      foreach (GameObject go in drawerElements) {
        RectTransform rtf = go.GetComponent<RectTransform>();
        rtf.position = new Vector3(rtf.position.x, rtf.position.y + distanceToTop, rtf.position.z);
      }
      upArrow.GetComponent<Text>().text = "";
      downArrow.GetComponent<Text>().text = "^";
    }

    // moving the drawer down + switching the arrows
    public void DrawerDown() {
      drawerElements = GameObject.FindGameObjectsWithTag("drawGroup");
      foreach (GameObject go in drawerElements) {
        RectTransform rtf = go.GetComponent<RectTransform>();
        rtf.position = new Vector3(rtf.position.x, rtf.position.y - distanceToTop, rtf.position.z);
      }
      upArrow.GetComponent<Text>().text = "^";
      downArrow.GetComponent<Text>().text = "";
    }


    // Update is called once per frame
    void Update()
    {

    }
}
