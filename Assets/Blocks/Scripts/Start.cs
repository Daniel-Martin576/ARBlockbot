using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : Block
{
    public override void execute()
    {
        if (myConnections.Length != 1 )
            throw new System.FormatException("Start Block short only have one connection");
        myConnections[0].getLinkToBlock().execute();
    }
}
