using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateOut : StateBase
{
    public StateOut()
    {
        name = "退出状态";
    }

    public override StateBase Tick(float Delta, MonoBehaviour Context)
    {
        return base.Tick(Delta, Context);
    }
}
