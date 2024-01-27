using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public GameObject associatedFlag;
    bool hasFlag = false;
    void Start()
    {
       if( associatedFlag != null )
            hasFlag = true;
    }

    public bool getHasFlag()
    {
        return hasFlag;
    }

    public GameObject getAssociatedFlag()
    {
        return associatedFlag;
    }
}
