using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateStart : StateBase
{
    public StateStart()
    {
        name = "初始状态";
    }
    public override StateBase Tick(float Delta, MonoBehaviour Context)
    {
        base.Tick(Delta, Context);
        ChildBehaviour childBehaviour = Context as ChildBehaviour;
        if(childBehaviour.mType == ChildType.Chaos)
        {
            SceneRail Rail = GameObject.FindObjectOfType<SceneRail>();
            if(!Rail.IsCoolingDown() && !Rail.bBroken)
            {
                childBehaviour.MoveToScenePoint(GameSceneManager.Instance.GetRandomScene(SceneType.WaitPlace));
            }
        }
        else
        {
            childBehaviour.MoveToScenePoint(GameSceneManager.Instance.GetRandomScene(SceneType.WaitPlace));
        }
        return new StateWait();
    }
}
