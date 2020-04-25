using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TextBlock : BlockObject
{
    private const float rectBuffer = 40;
    private const float boxBuffer = 15;

    private InputField inputField;
    private float startingWidth;

    private RectTransform InputRectTran;
    private Vector2 inputRect;

    private RectTransform BoxRectTran;
    private Vector2 boxRect;

    new void Start()
    {
        base.Start();
        inputField = this.gameObject.GetComponentInChildren<InputField>();
        startingWidth = CalculateLengthOfMessage(inputField.textComponent, inputField.text);

        //InputRectTran = inputField.GetComponentInParent<RectTransform>();
        //inputRect = InputRectTran.sizeDelta;
        

        //BoxRectTran = this.gameObject.GetComponentInChildren<ExtendMe>().GetComponentInParent<RectTransform>();
        //boxRect = BoxRectTran.sizeDelta;

        resizeBox();
    }

    // Ugh should put this in another file 
    public void resizeBox()
    {
        float prefWidth = CalculateLengthOfMessage(inputField.textComponent, inputField.text);
        //print(prefWidth);
       // float width = (inputRect.x - rectBuffer) * System.Math.Max(prefWidth / startingWidth, 1) + rectBuffer;

        //InputRectTran.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, boxBuffer, width);
        //BoxRectTran.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width + 2 * boxBuffer);
    }

    private int CalculateLengthOfMessage(Text chatText, string message)
    {
        int totalLength = 0;

        Font myFont = chatText.font;  //chatText is my Text component
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = message.ToCharArray();

        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, chatText.fontSize);

            totalLength += characterInfo.advance;
        }

        return totalLength;
    }

    public override object getReturn()
    {
        return this.gameObject.GetComponentInChildren<Text>().text;
    }

}
