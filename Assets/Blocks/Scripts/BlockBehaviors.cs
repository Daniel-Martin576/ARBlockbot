using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehaviors : BlockObject
{
    // Kinda don't want to seperate blocks into invidiual classes yet, because they will only be overriding one function ("execute")
    // So just go to do FLYWEIGHT Design Pattern (kind) and enum the hell out of it 
    public Behaviour behaviour;

    public enum Behaviour
    {
        Nothing, Print, TextBox, IF

    }
    /*
    public override void execute()
    {
        Connection executeCon;

        switch (behaviour)
        {
            case Behaviour.Print:
                if ((executeCon = getConnectionType(Connection.Type.Execute)) != null)
                    print(executeCon.getLinkToBlock().getReturn());
                    break;

            case Behaviour.IF:
                if ((executeCon = getConnectionType(Connection.Type.Check)) != null)
                    if (convertStringToBool(executeCon.getLinkToBlock().getReturn()))
                        if ((executeCon = getConnectionType(Connection.Type.Execute)) != null)
                            executeCon.getLinkToBlock().execute();
                
                break;

            default:
                break;

        }

        if ((executeCon = getConnectionType(Connection.Type.Next)) != null)
            executeCon.getLinkToBlock().execute();
    }

    public static bool convertStringToBool(object obj)
    {
        if (obj == null) return false;

        bool check = false;
        double x;

        // string of True and != 0 numbers work
        if (obj is string && Boolean.TryParse((string) obj, out check)) { }
        else if (obj is string && Double.TryParse((string) obj, out x))
            check = Convert.ToBoolean(x);

        return check;
    }

    */
}

