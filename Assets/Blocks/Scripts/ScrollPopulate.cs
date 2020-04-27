using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollPopulate : MonoBehaviour
{

    public Dictionary<string, List<Func<Transform, GameObject>>> scrollBlockDict;
    void Start()
    {
        // GameObject attached should be parent of scroll
        populateDict();
        GameObject content = GameObject.FindGameObjectWithTag("Content");
        GameObject draggedblock = null;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObj = gameObject.transform.GetChild(i).gameObject;
            if ((scrollBlockDict.ContainsKey(childObj.name)))
            {
                GameObject scrollParent = childObj.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;
                foreach(Func<Transform, GameObject> func in scrollBlockDict[childObj.name])
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
                    entry.callback.AddListener((eventData) => {onBeginDrag(eventData);});
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

    public void Play()
    {
        GameObject[] startInfoObjs = GameObject.FindGameObjectsWithTag("Start");
        foreach (GameObject startInfoObj in startInfoObjs)
        {
            BlockObject blockObject = startInfoObj.transform.parent.gameObject.GetComponent<BlockObject>();
            if (blockObject != null)
            {
                Blockly.Block startBlock = blockObject.getParentBlock();
                if (startBlock != null)
                {
                    startBlock.function(null);
                    return;
                }
            }
        }
    }



    private void populateDict()
    {
        scrollBlockDict = new Dictionary<string, List<Func<Transform, GameObject>>>();
        scrollBlockDict.Add("logic-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createStartBlock,
                    Blockly.BlockLibrary.createPrintBlock,
                    Blockly.BlockLibrary.createSetThrottleBlock,
                    Blockly.BlockLibrary.createSetSteeringBlock,
                    Blockly.BlockLibrary.createSleepBlock,
                    Blockly.BlockLibrary.createLoopBlock,
                    Blockly.BlockLibrary.createInputBlock

        });
        scrollBlockDict.Add("loops-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createStartBlock,
                    Blockly.BlockLibrary.createPrintBlock,
                    Blockly.BlockLibrary.createSetThrottleBlock,
                    Blockly.BlockLibrary.createSetSteeringBlock,
                    Blockly.BlockLibrary.createSleepBlock,
                    Blockly.BlockLibrary.createLoopBlock,
                    Blockly.BlockLibrary.createInputBlock
        });
        scrollBlockDict.Add("math-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createStartBlock,
                    Blockly.BlockLibrary.createPrintBlock,
                    Blockly.BlockLibrary.createSetThrottleBlock,
                    Blockly.BlockLibrary.createSetSteeringBlock,
                    Blockly.BlockLibrary.createSleepBlock,
                    Blockly.BlockLibrary.createLoopBlock,
                    Blockly.BlockLibrary.createInputBlock
        });
        scrollBlockDict.Add("movement-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createStartBlock,
                    Blockly.BlockLibrary.createPrintBlock,
                    Blockly.BlockLibrary.createSetThrottleBlock,
                    Blockly.BlockLibrary.createSetSteeringBlock,
                    Blockly.BlockLibrary.createSleepBlock,
                    Blockly.BlockLibrary.createLoopBlock,
                    Blockly.BlockLibrary.createInputBlock
        });
        scrollBlockDict.Add("variables-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createStartBlock,
                    Blockly.BlockLibrary.createPrintBlock,
                    Blockly.BlockLibrary.createSetThrottleBlock,
                    Blockly.BlockLibrary.createSetSteeringBlock,
                    Blockly.BlockLibrary.createSleepBlock,
                    Blockly.BlockLibrary.createLoopBlock,
                    Blockly.BlockLibrary.createInputBlock
        });
        scrollBlockDict.Add("fucntions-scroll", new List<Func<Transform, GameObject>> {
                    Blockly.BlockLibrary.createStartBlock,
                    Blockly.BlockLibrary.createPrintBlock,
                    Blockly.BlockLibrary.createSetThrottleBlock,
                    Blockly.BlockLibrary.createSetSteeringBlock,
                    Blockly.BlockLibrary.createSleepBlock,
                    Blockly.BlockLibrary.createLoopBlock,
                    Blockly.BlockLibrary.createInputBlock
        });
    }
}
