using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.InteractionSystem.Sample;

public class ScrollPopulate : MonoBehaviour
{

    public Dictionary<string, List<Func<Transform, GameObject>>> scrollBlockDict;
    public List<Blockly.Block> blocks;


    void Start()
    {
        // GameObject attached should be parent of scroll
        populateDict();
        GameObject content = GameObject.FindGameObjectWithTag("Content");
        GameObject draggedblock = null;
        blocks = new List<Blockly.Block>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObj = gameObject.transform.GetChild(i).gameObject;
            if ((scrollBlockDict.ContainsKey(childObj.name)))
            {
                GameObject scrollParent = childObj.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;
                foreach (Func<Transform, GameObject> func in scrollBlockDict[childObj.name])
                {
                    GameObject block = func(scrollParent.transform);
                    block.transform.localScale = new Vector3(4, 4, 1);
                    block.transform.SetParent(scrollParent.transform);
                    Destroy(block.GetComponent<BlockObject>());
                    block.tag = "Untagged";
                    block.GetComponent<RectTransform>().sizeDelta += new Vector2(50, 50);

                    void onBeginDrag(BaseEventData data)
                    {
                        GameObject newBlock = func(scrollParent.transform);
                        blocks.Add(newBlock.GetComponent<BlockObject>().getParentBlock());
                        newBlock.transform.localScale = new Vector3(4, 4, 1);
                        newBlock.transform.SetParent(content.transform);
                        draggedblock = newBlock;
                        newBlock.GetComponent<BlockObject>().OnBeginDrag((PointerEventData)data);
                    }

                    void onDrag(BaseEventData data)
                    {
                        if (draggedblock != null)
                            draggedblock.GetComponent<BlockObject>().OnDrag((PointerEventData)data);
                    }

                    void onEndDrag(BaseEventData data)
                    {
                        if (draggedblock != null)
                            draggedblock.GetComponent<BlockObject>().OnEndDrag((PointerEventData)data);
                    }


                    EventTrigger trigger = block.AddComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.BeginDrag;
                    entry.callback.AddListener((eventData) => { onBeginDrag(eventData); });
                    trigger.triggers.Add(entry);

                    EventTrigger.Entry entry1 = new EventTrigger.Entry();
                    entry1.eventID = EventTriggerType.Drag;
                    entry1.callback.AddListener((eventData) => { onDrag(eventData); });
                    trigger.triggers.Add(entry1);

                    EventTrigger.Entry entry2 = new EventTrigger.Entry();
                    entry2.eventID = EventTriggerType.Drag;
                    entry2.callback.AddListener((eventData) => { onEndDrag(eventData); });
                    trigger.triggers.Add(entry2);
                }
            }
        }
    }

    private Thread thr;
    public void Play()
    {
        if (thr != null)
        {
            thr.Join();
            Debug.Log("Killed last thread: " + !thr.IsAlive);
        }


        GameObject[] startInfoObjs = GameObject.FindGameObjectsWithTag("Start");
        foreach (GameObject startInfoObj in startInfoObjs)
        {
            BlockObject blockObject = startInfoObj.transform.parent.gameObject.GetComponent<BlockObject>();
            if (blockObject != null)
            {
                Blockly.Block startBlock = blockObject.getParentBlock();
                if (startBlock != null)
                {
                    Debug.Log("Updating...");
                    BuggyBuddy buggyBuddy = GetBuggy();
                    foreach (Blockly.Block block in blocks)
                        if (block != null)
                            block.buggyBuddy = buggyBuddy;

                    Debug.Log("Starting...");
                    void startFunction() => startBlock.function(null);
                    thr = new Thread(new ThreadStart(startFunction));
                    thr.Start();
                    return;
                }
            }
        }
    }

    public void killThread() {

       if (thr != null)
       {
           thr.Join();
           Debug.Log("Killed last thread: " + !thr.IsAlive);
       }

       BuggyBuddy buggyBuddy = GetBuggy();
       if (buggyBuddy != null) {
         buggyBuddy.steer = new Vector2(0f, 1f);
         buggyBuddy.throttle = 0f;
       }


    }

    public static BuggyBuddy GetBuggy()
    {
        GameObject robot = GameObject.FindGameObjectWithTag("Robot");
        if (robot != null)
            return robot.GetComponent<BuggyBuddy>();
        return null;
    }


    private void populateDict()
    {
        scrollBlockDict = new Dictionary<string, List<Func<Transform, GameObject>>>();
        scrollBlockDict.Add("logic-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createStartBlock,
                    Blockly.BlockLibrary.createIfDoBlock,
                    Blockly.BlockLibrary.createNotBlock,
                    Blockly.BlockLibrary.createTestTFBlock
                    //Blockly.BlockLibrary.createCompareBlock,

        });
        scrollBlockDict.Add("loops-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createLoopBlock,
                    Blockly.BlockLibrary.createWhileBlock,
        });
        scrollBlockDict.Add("math-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createNumberBlock,
                    Blockly.BlockLibrary.createPiBlock,
                    Blockly.BlockLibrary.createMathInputBlock
        });
        scrollBlockDict.Add("movement-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createSetThrottleBlock,
                    Blockly.BlockLibrary.createSetSteeringBlock,
                    Blockly.BlockLibrary.createSleepBlock,
        });
        scrollBlockDict.Add("variables-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createInputBlock,
                    Blockly.BlockLibrary.createPrintBlock,
        });
        scrollBlockDict.Add("functions-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createFunctionBlock,
        });
    }
}
