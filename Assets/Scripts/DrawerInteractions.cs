using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerInteractions : MonoBehaviour
{
  private GameObject[] drawerElements;

  private float cHeight;
  private float buttonHeight;
  private float distanceToTop;

  private GameObject upArrow;
  private GameObject downArrow;
  private int counter;
  // 1 is up, 2 is down
  private int move = 0;

  public GameObject dButton;

    // Start is called before the first frame update
    void Start()
    {
      drawerElements = GameObject.FindGameObjectsWithTag("drawGroup");

      upArrow = dButton.transform.Find("up-arrow").gameObject;
      upArrow.GetComponent<Text>().text = "^";

      downArrow = dButton.transform.Find("down-arrow").gameObject;
      downArrow.GetComponent<Text>().text = "";

      GameObject canvas = GameObject.Find("Canvas");
      cHeight = canvas.GetComponent<Canvas>().pixelRect.height;

      buttonHeight = dButton.GetComponent<RectTransform>().rect.height;

      RectTransform r = GameObject.Find("Drawer").GetComponent<RectTransform>();
      r.position = new Vector3(r.position.x, -r.position.y + buttonHeight, r.position.z);

      distanceToTop = cHeight - buttonHeight;

      counter = 0;
    }

    public void OnClickBehaviour() {
      counter++;
      if (counter % 2 == 0) {
        DrawerDown();
      }
      else DrawerUp();
    }

    // moving the drawer up + switching the arrows
    public void DrawerUp() {
      foreach (GameObject go in drawerElements) {
        RectTransform rtf = go.GetComponent<RectTransform>();
        rtf.position = new Vector3(rtf.position.x, rtf.position.y + distanceToTop, rtf.position.z);
      }
      upArrow.GetComponent<Text>().text = "";
      downArrow.GetComponent<Text>().text = "^";
    }

    // moving the drawer down + switching the arrows
    public void DrawerDown() {
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



      if (Input.touchCount > 0  && Input.GetTouch(0).phase == TouchPhase.Moved) {

        // Get movement of the finger since last frame
        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

        // // for detecting whether or not touch was on tab
        // Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        // Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
        //
        // RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
        //
        // if (hitInformation.collider != null) {
        //    //We should have hit something with a 2D Physics collider!
        //    GameObject touchedObject = hitInformation.transform.gameObject;
        //    //touchedObject should be the object someone touched.
        //    Debug.Log("Touched " + touchedObject.transform.name);
        // }

        // if (hitInformation.collider == null) {
        //   Debug.Log("hitInformation collided with null");
        // }

        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            Debug.Log("Something Hit");
            if (raycastHit.collider.name == "dButton")
            {
                Debug.Log("dButton clicked");
            }

        }



        // get info about vertical direction of swipe
        if (touchDeltaPosition.y > 60) {
          Debug.Log("++++++++++++++++++++++++");
        }

        if (touchDeltaPosition.y < -60) {
          Debug.Log("-------------------------");
        }

      }
    }
}
