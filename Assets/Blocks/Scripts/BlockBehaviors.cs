using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviors : Block
{
    // Kinda don't want to seperate blocks into invidiual classes yet, because they will only be overriding one function ("execute")
    // So just go to do FLYWEIGHT Design Pattern (kind) and enum the hell out of it 
    public Behaviour behaviour;

    public enum Behaviour
    {
        Print, Nothing, IF

    }

    public override void execute()
    {
        switch (behaviour)
        {
            case Behaviour.Print:
                Connection executeCon;
                if ((executeCon = getConnectionType(Connection.Type.Execute)) != null) {
                    executeCon.getLinkToBlock().execute();
                }
                print("asdasd");
                break;
            case Behaviour.Nothing:
                break;
            default:
                throw new System.NotImplementedException("Behavior not implemented");
        }
    }



}
