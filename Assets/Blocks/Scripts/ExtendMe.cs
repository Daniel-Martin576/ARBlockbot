using UnityEngine;

public class ExtendMe : MonoBehaviour {

    public int ID;

    private const float boxBuffer = 15;
    private RectTransform BoxRectTran;
    private Vector2 boxRect;


    public void Start()
    {
        RectTransform extendMes = gameObject.GetComponent<RectTransform>();
    }

    public void resizeBox(int ID, float width)
    {
        BoxRectTran.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width + 2 * boxBuffer);
    }

}
