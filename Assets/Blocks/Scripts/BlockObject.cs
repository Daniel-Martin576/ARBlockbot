using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Blockly;

// Trash, re work
public class BlockObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject trash;
    private const float maxTrashDist = 20000f;

    protected Connection[] myConnections;
    private const float maxConnectionDist =1500.0f;
    private Tuple<Connection, Connection, BlockObject> lastPotConnection;
    private Transform OrigParent;
    private Block parentBlock;

    private Color[] beforeColors;

    public Dictionary<Connection, GameObject> connectionDict;


    public void Start()
    {
        trash = GameObject.FindGameObjectWithTag("Trash");
        OrigParent = transform.parent;
    }

    public void addConnections(Dictionary<Connection, GameObject> connectionDict)
    {
        this.connectionDict = connectionDict;
        myConnections = new List<Connection>(connectionDict.Keys).ToArray();
    }

    public void addParentBlock(Block parentBlock) => this.parentBlock = parentBlock;

    public Block getParentBlock() => parentBlock;

    public Connection[] getConnections()
    {
        return myConnections;
    }

    private Tuple<Connection, Connection, BlockObject> findPotentialConnections()
    {
        Connection myHighestPotenital = null;
        Connection othHighestPotenital = null;
        BlockObject b = null;
        float minDistance = maxConnectionDist;


        GameObject[] blocksObjects = GameObject.FindGameObjectsWithTag("Block");

        foreach(GameObject blocksObject in blocksObjects)
        {
            if (blocksObject != this.gameObject)
            {
                Connection[] othConnections = blocksObject.GetComponent<BlockObject>().getConnections();
                foreach (Connection othConnection in othConnections)
                {
                    foreach (Connection myConnection in myConnections)
                    {
                        if (Connection.canConnect(myConnection, othConnection))
                        {
                            float dist = (connectionDict[myConnection].transform.position - blocksObject.GetComponent<BlockObject>().connectionDict[othConnection].transform.position).sqrMagnitude;
                            if (dist < minDistance)
                            {
                                b = blocksObject.GetComponent<BlockObject>();
                                myHighestPotenital = myConnection;
                                othHighestPotenital = othConnection;
                                minDistance = dist;
                            }
                        }
                    }
                }
            }
        }

        return Tuple.Create(myHighestPotenital, othHighestPotenital, b);
    }

    private void connect(Connection myCon, Connection othCon, BlockObject b)
    {
        Vector3 offset = b.connectionDict[othCon].transform.position - connectionDict[myCon].transform.position;
        transform.position = transform.position + offset;
        if (myCon.lead)
            b.connectionDict[othCon].transform.parent.SetParent(transform);
        else
            transform.SetParent(b.connectionDict[othCon].transform.parent);
        myCon.join(othCon);
        othCon.join(myCon);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        // Color
        Image[] images = this.gameObject.GetComponentsInChildren<Image>();
        beforeColors = new Color[images.Length];

        for (int i = 0; i < images.Length; i++)
        {
            beforeColors[i] = images[i].color;

            float H, S, V;
            Color.RGBToHSV(images[i].color, out H, out S, out V);
            Color lightColor = Color.HSVToRGB(H, S - .2f, V + .2f, false);
            lightColor.a = images[i].color.a;
            images[i].color = lightColor;
        }

        // Unlink
        foreach (Connection myCon in myConnections)
        {
            if (myCon.linked() && !myCon.lead)
            {
                transform.SetParent(OrigParent);
                myCon.pair.unjoin();
                myCon.unjoin();
            }
        }

        lastPotConnection = Tuple.Create<Connection, Connection, BlockObject>(null, null, null);
    }

    public void OnDrag(PointerEventData data)
    {
        // Movement

        transform.position = UnityEngine.Input.mousePosition;

        // Trash
        this.checkIfOnTrash();

        // Highlight
        Tuple<Connection, Connection, BlockObject> potConnections = findPotentialConnections();
        if (potConnections.Item2 != lastPotConnection.Item2)
        {
            if (lastPotConnection.Item2 != null)
                lastPotConnection.Item3.connectionDict[lastPotConnection.Item2].GetComponent<Image>().color = new Color(1f, 1f, 0f, 0f);
            if (potConnections.Item2 != null)
                potConnections.Item3.connectionDict[potConnections.Item2].GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);
        }
        lastPotConnection = potConnections;
    }

    public void OnEndDrag(PointerEventData data)
    {
        // Trash
        if (this.checkIfOnTrash())
        {
            Destroy(this.gameObject);
            trash.GetComponent<Image>().sprite = trash.GetComponent<Button>().spriteState.selectedSprite;
        }

        // Color
        Image[] images = this.gameObject.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = beforeColors[i];
        }

        // Link
        if (lastPotConnection.Item2 != null)
        {
            lastPotConnection.Item3.connectionDict[lastPotConnection.Item2].GetComponent<Image>().color = new Color(1f, 1f, 0f, 0f);
            this.connect(lastPotConnection.Item1, lastPotConnection.Item2, lastPotConnection.Item3);
        }
    }

    public bool checkIfOnTrash()
    {
        if (trash == null) return false;
        bool closeEnough = (trash.transform.position - transform.position).sqrMagnitude < maxTrashDist;
        // Kinda hacky; disabledSprite = Open Trash Sprite, selectedSprite = Close Trash Sprite
        if (closeEnough)
            trash.GetComponent<Image>().sprite = trash.GetComponent<Button>().spriteState.disabledSprite;
        else
            trash.GetComponent<Image>().sprite = trash.GetComponent<Button>().spriteState.selectedSprite;

        return closeEnough;
    }

    // public abstract void execute();

    public virtual void execute() { }
    public virtual object getReturn() => null;
}
