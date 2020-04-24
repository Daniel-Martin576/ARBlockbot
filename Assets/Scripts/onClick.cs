using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onClick : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button forwardButton, leftButton, rightButton, backButton, rotateLButton;
    public static bool forward = false;
    public static bool backward = false;
    public static bool left = false;
    public static bool right = false;
    public static bool rotateL = false;
    

    void Start()
    {
        //Calls the TaskOnClick method when you click the Button
        forwardButton.onClick.AddListener(TaskOnClickForward);
        leftButton.onClick.AddListener(TaskOnClickLeft);
        rightButton.onClick.AddListener(TaskOnClickRight);
        backButton.onClick.AddListener(TaskOnClickBack);
        rotateLButton.onClick.AddListener(TaskOnClickBackRotateL);
    }

    void TaskOnClickForward()
    {
        forward = true;
        backward = false;
        left = false;
        right = false;
        rotateL = false;


        Debug.Log("You have clicked forward!");
    }
    
    void TaskOnClickLeft()
    {
      
        left = true;
        right = false;
        backward = false;
        forward = false;
        rotateL = false;
    }

    void TaskOnClickRight()
    {
        
        right = true;
        left = false;
        backward = false;
        forward = false;
        rotateL = false;
    }

    void TaskOnClickBack()
    {
        //Output this to console when the Button2 is clicked
        backward = true;
        forward = false;
        left = false;
        right = false;
        rotateL = false;
    }

    void TaskOnClickBackRotateL()
    {
        rotateL = true;
        backward = false;
        forward = false;
        left = false;
        right = false;
    }


}
