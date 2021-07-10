using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StateWait : StateBase
{
    public StateWait()
    {
        name = "观赏等待状态";
    }
    float WaitTime;
    public override void Enter(MonoBehaviour Context)
    {
        base.Enter(Context);
        ChildBehaviour childBehaviour = Context as ChildBehaviour;
        WaitTime = Random.Range(childBehaviour.minWaitTime, childBehaviour.maxWaitTime);
    }
    public override StateBase Tick(float Delta, MonoBehaviour Context)
    {
        if (StateTick > WaitTime)
        {
            ChildBehaviour childBehaviour = Context as ChildBehaviour;
            childBehaviour.MoveToScenePoint(SceneManager.Instance.GetRandomScene(SceneType.Route));
            return new StateHang();//停留时间超过一定 则开始游荡
        }
        return base.Tick(Delta, Context);
    }
}