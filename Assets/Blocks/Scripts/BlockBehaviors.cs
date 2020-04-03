using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviors : Block
{
    // Kinda don't want to seperate blocks into invidiual classes yet, because they will only be overriding one function ("execute")
    // So just go to do FLYWEIGHT Design Pattern and enum the hell out of it 

    public enum Behaviour
    {
        PRINT
    }

    public override void execute()
    {
        throw new System.NotImplementedException();
    }
}
