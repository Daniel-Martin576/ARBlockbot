using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connection : MonoBehaviour
{
    public Direction direction;
    public Gender gender;
    public Authority authority;

    private Connection linkedTo;

    public enum Direction { Horz, Vert }
    public enum Gender { Male, Female }
    public enum Authority { Leader, Follower }
    public enum Type { Prev, Next, Execute, Return}

    static public bool canConnect(Connection con1, Connection con2)
    {
        return (con1.direction == con2.direction) && (con1.gender != con2.gender) && (!con1.linked() && !con2.linked());
    }

    public void highlight()
    {
        this.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);
    }

    public void unhighlight()
    {
        this.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, 0f);
    }

    public void join(Connection othConnection) => linkedTo = othConnection;

    public void unjoin() => linkedTo = null;


    public bool linked() =>  linkedTo != null;
    

    public Connection getLinkedTo() => linkedTo;

    public Block getLinkToBlock() => linkedTo.transform.parent.GetComponent<Block>();
}
