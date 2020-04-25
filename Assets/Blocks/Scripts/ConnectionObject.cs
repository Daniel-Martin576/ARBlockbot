using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionObject : MonoBehaviour
{
    public Type type;
    public bool lead;
    private ConnectionObject linkedTo;

    public enum Type { Prev, Next, Output, Input }

    public ConnectionObject(Type type)
    {
        this.type = type;
        lead = (type == Type.Next || type == Type.Input);
    }

    static public bool canConnect(ConnectionObject con1, ConnectionObject con2)
    {
        return ((con1.type == Type.Prev && con1.type == Type.Next)
            || (con1.type == Type.Next && con1.type == Type.Prev)
            || (con1.type == Type.Output && con1.type == Type.Input)
            || (con1.type == Type.Input && con1.type == Type.Output))
            && (!con1.linked() && !con2.linked());
    }

    public void highlight() => this.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);
    

    public void unhighlight() => this.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, 0f);

    public void join(ConnectionObject othConnection) => linkedTo = othConnection;

    public void unjoin() => linkedTo = null;

    public bool linked() =>  linkedTo != null;
    

    public ConnectionObject getLinkedTo() => linkedTo;

    public BlockObject getLinkToBlock() => linkedTo.gameObject.GetComponentInParent<BlockObject>();
}
