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
        childBehaviour.MoveToScenePoint(SceneManager.Instance.GetRandomScene(SceneType.WaitPlace));
        return new StateWait();
    }
}
