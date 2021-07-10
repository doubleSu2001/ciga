﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateHang : StateBase
{
    public StateHang()
    {
        name = "闲逛状态";
    }
    float HangTime = 0;
    public override void Enter(MonoBehaviour Context)
    {
        base.Enter(Context);
        ChildBehaviour childBehaviour = Context as ChildBehaviour;
        HangTime = Random.Range(childBehaviour.minHangTime, childBehaviour.maxHangTime);
    }

    public override StateBase Tick(float Delta, MonoBehaviour Context)
    {
        base.Tick(Delta, Context);
        ChildBehaviour childBehaviour = Context as ChildBehaviour;
        if (StateTick > HangTime)//游荡20秒重新回归或者去看展
        {
            if(Random.Range(0,1) > 0.5)
            {
                childBehaviour.MoveToScenePoint(SceneManager.Instance.GetRandomScene(SceneType.Exit));
                return new StateOut();
            }
            else
            {
                childBehaviour.MoveToScenePoint(SceneManager.Instance.GetRandomScene(SceneType.WaitPlace));
                return new StateWait();
            }
        }
        else
        {
            if(childBehaviour.IsStopped())
            {
                childBehaviour.MoveToScenePoint(SceneManager.Instance.GetRandomScene(SceneType.Route));
            }
        }
        return this;
    }
}
