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
    bool bTryGetTrain = false;
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
        if(!bTryGetTrain && childBehaviour.IsArrive() && StateTick > childBehaviour.minCatchTrainTime && childBehaviour.TrainOnHand == 0)
        {
            if(Random.Range(0f, 1f) > childBehaviour.probCatchTrain)
            {
                childBehaviour.TryGetTrain();
            }
            bTryGetTrain = true;//不管拿没拿 之后不会再尝试了
        }
        if (StateTick > WaitTime)
        {
            childBehaviour.MoveToScenePoint(SceneManager.Instance.GetRandomScene(SceneType.Route));
            return new StateHang();//停留时间超过一定 则开始游荡
        }
        return base.Tick(Delta, Context);
    }
}