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
    // Start is called before the first frame update
    void Start()
    {
        Type = SceneType.WaitPlace;
    }

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
        TrainNum++;
        return 0;
    }
}
