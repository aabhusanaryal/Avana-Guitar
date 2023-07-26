using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchLocation
{
    public int touchId;
    public GameObject tcollider;

    public touchLocation(int newTouchId, GameObject newtcollider)
    {
        touchId = newTouchId;
        tcollider = newtcollider;
    }
}