using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRail : SceneBase, IInteractiveElement
{

    [Header("铁轨最多m秒内被拆一次")]
    public float M = 30;

    public bool bBroken = false; // 铁轨上小车是否损坏
    public float CurMs = 0;
    //与铁轨交互
    public int TryInteract(int InCode)
    {
        if(IsCoolingDown())
        {
            return 0;// 此时冷却中
        }
        bBroken = InCode == 0;
        int Ret = CurTrainType;
        CurTrainType = InCode;
        CurMs = 0.1f;//启动冷却
        return Ret;
    }

    public bool IsCoolingDown()
    {
        return CurMs > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurMs > 0)
        {
            CurMs += Time.deltaTime;
            if (CurMs > M)
            {
                CurMs = 0;
            }
        }
    }

    // 只有
    public bool CanInteract(MonoBehaviour Source)
    {
        ChildBehaviour childBehaviour = Source.GetComponent<ChildBehaviour>();
        PlayerController Con = Source.GetComponent<PlayerController>();
        return childBehaviour != null && childBehaviour.mType == ChildType.Chaos || Con != null;
    }
}
