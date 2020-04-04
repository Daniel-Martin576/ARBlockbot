using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject trash;
    private const float maxTrashDist = 1000f;

    protected Connection[] myConnections;
    private const float maxConnectionDist = 300.0f;
    private Tuple<Connection, Connection> lastPotConnection;
    private Transform OrigParent;

    private Color[] beforeColors;


    public void Start()
    {
        trash = GameObject.FindGameObjectWithTag("Trash");
        myConnections = this.gameObject.GetComponentsInChildren<Connection>();
        OrigParent = transform.parent;

    }

    public Connection[] getConnections()
    {
        return myConnections;
    }

    private Tuple<Connection, Connection> findPotentialConnections()
    {
        Connection myHighestPotenital = null;
        Connection othHighestPotenital = null;
        float minDistance = maxConnectionDist;

        GameObject[] blocksObjects = GameObject.FindGameObjectsWithTag("Block");

        foreach(GameObject blocksObject in blocksObjects)
        {
            if (blocksObject != this.gameObject)
            {
                Connection[] othConnections = blocksObject.GetComponent<Block>().getConnections();
                foreach (Connection othConnection in othConnections)
                {
                    foreach (Connection myConnection in myConnections)
                    {
                        if (Connection.canConnect(myConnection, othConnection))
                        {
                            float dist = (myConnection.transform.position - othConnection.transform.position).sqrMagnitude;
                            if (dist < minDistance)
                            {
                                myHighestPotenital = myConnection;
                                othHighestPotenital = othConnection;
                                minDistance = dist;
                            }
                        }
                    }
                }
            }
        }

        return Tuple.Create(myHighestPotenital, othHighestPotenital);
    }

    private void connect(Connection myCon, Connection othCon)
    {
        Vector3 offset = othCon.transform.position - myCon.transform.position;
        transform.position = transform.position + offset;
        if (myCon.authority == Connection.Authority.Leader)
            othCon.transform.parent.SetParent(transform);
        else
            transform.SetParent(othCon.transform.parent);
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
            if (myCon.linked() && myCon.authority == Connection.Authority.Follower)
            {
                transform.SetParent(OrigParent);
                myCon.getLinkedTo().unjoin();
                myCon.unjoin();
            }
        }

        lastPotConnection = Tuple.Create<Connection, Connection>(null, null);
    }

    public void OnDrag(PointerEventData data)
    {
        // Movement
        transform.position = Input.mousePosition;

        // Trash
        this.checkIfOnTrash();

        // Highlight
        Tuple<Connection, Connection> potConnections = findPotentialConnections();
        if (potConnections.Item2 != lastPotConnection.Item2)
        {
            if (lastPotConnection.Item2 != null)
                lastPotConnection.Item2.unhighlight();
            if (potConnections.Item2 != null)
                potConnections.Item2.highlight();
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
            lastPotConnection.Item2.unhighlight();
            this.connect(lastPotConnection.Item1, lastPotConnection.Item2);
        }
    }

    public bool checkIfOnTrash()
    {
        bool closeEnough = (trash.transform.position - transform.position).sqrMagnitude < maxTrashDist;
        // Kinda hacky; disabledSprite = Open Trash Sprite, selectedSprite = Close Trash Sprite
        if (closeEnough)
            trash.GetComponent<Image>().sprite = trash.GetComponent<Button>().spriteState.disabledSprite;
        else
            trash.GetComponent<Image>().sprite = trash.GetComponent<Button>().spriteState.selectedSprite;

        return closeEnough;
    }

    public Connection getConnectionType(Connection.Type type)
    {
        foreach (Connection myCon in myConnections)
        {
            if (myCon.type == type && myCon.linked())
            {
                return myCon;
            }
        }
        return null;
    }

    // public abstract void execute();

    public virtual void execute() { }
    public virtual object getReturn() => null;
}
