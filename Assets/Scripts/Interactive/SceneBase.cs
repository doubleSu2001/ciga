using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SceneType
{
    None, 
    Route, // 閒逛地點
    WaitPlace, //等待地点 （如货架、火车轨道
    Exit
}

// 场景基类 所有的场景交互区域物体都需要包含此组件
[System.Serializable]
public class SceneBase : MonoBehaviour
{
    [Header("最大容量")]
    public int MaxCount;

    public int CurCount;
    public int CurTrainType;

    public SceneType Type = SceneType.None;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.Instance.RigisterSceneObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Arrive()
    {
        // GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public Vector3 GetPosition()
    {
        var ret = transform.position;
        ret.z = 0;
        return ret;
    }
}
