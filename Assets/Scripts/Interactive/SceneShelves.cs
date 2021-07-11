using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 货架
public class SceneShelves : SceneBase, IInteractiveElement
{
    [Header("货架上类型")]
    public int OriginTrainType;

    // 数量
    public int TrainNum;

    // Update is called once per frame
    void Update()
    {
        
    }
    public int TryInteract(int InCode)
    {
        if(InCode == 0)
        {
            TrainNum--;
            return OriginTrainType;
        }
        else
        {

        }
        TrainNum++;
        return 0;
    }

    public bool CanInteract(MonoBehaviour Source)
    {
        return true;// 谁都可以和货架交互
    }
}
