using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehaviors : Block
{
    // Kinda don't want to seperate blocks into invidiual classes yet, because they will only be overriding one function ("execute")
    // So just go to do FLYWEIGHT Design Pattern (kind) and enum the hell out of it 
    public Behaviour behaviour;

    public enum Behaviour
    {
        Nothing, Print, TextBox, IF

    }

    public override void execute()
    {
        Connection executeCon;

        switch (behaviour)
        {
            case Behaviour.Print:
                if ((executeCon = getConnectionType(Connection.Type.Execute)) != null)
                    print(executeCon.getLinkToBlock().getReturn());
                    break;

            default:
                break;

        }

        if ((executeCon = getConnectionType(Connection.Type.Next)) != null)
            executeCon.getLinkToBlock().execute();
    }
}
