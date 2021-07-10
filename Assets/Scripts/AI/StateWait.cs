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
        ChildBehaviour childBehaviour = Context as ChildBehaviour;
        // 试图交互获取车
        if(childBehaviour.IsArrive() && StateTick > childBehaviour.minCatchTrainTime && childBehaviour.TrainOnHand == 0)
        {
            childBehaviour.TryGetTrain();
        }
        if (StateTick > WaitTime)
        {
            childBehaviour.MoveToScenePoint(SceneManager.Instance.GetRandomScene(SceneType.Route));
            return new StateHang();//停留时间超过一定 则开始游荡
        }
        return base.Tick(Delta, Context);
    }
}