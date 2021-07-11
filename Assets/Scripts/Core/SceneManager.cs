using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
        ObjMap = new Dictionary<SceneType, List<SceneBase>>();
    }
    
    public Dictionary<SceneType, List<SceneBase>> ObjMap;

    public void RigisterSceneObject(SceneBase Obj)
    {
        if(!ObjMap.ContainsKey(Obj.Type))
        {
            ObjMap.Add(Obj.Type, new List<SceneBase>());
        }
        ObjMap[Obj.Type].Add(Obj);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SceneBase GetRandomScene(SceneType Type = SceneType.None)
    {
        if(!ObjMap.ContainsKey(Type))
        {
            print("不存在的类型" + Type);
            return null;
        }
        List<SceneBase> ObjList = ObjMap[Type];
        return ObjList[Random.Range(0, ObjList.Count - 1)];
    }
}
