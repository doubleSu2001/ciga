using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public interface ISpawnInfo
{
    void SetParam(int i);
}

[System.Serializable]
public enum ESpawn
{
    火车,
    熊孩子,
}



public class GameMode : MonoBehaviour
{
    // 熊孩子配置
    [System.Serializable]
    public class ChildConfig
    {
        [Header("数目"), SerializeField]
        public int Count;
        [Header("出现时间"), SerializeField]
        public float Time;
        [Header("类型"), SerializeField]
        public ChildType Type;
    }
    // 奖励配置
    [System.Serializable]
    public class GiftConfig
    {
        [Header("类型"), SerializeField]
        public string name;
        [Header("奖励愉悦值"), SerializeField]
        public int Value;
        [Header("奖励图标"), SerializeField]
        public Sprite Image;
    }

    [Header("倒计时时长")]
    public int MaxTime = 30;
    [Header("总愉悦值")]
    public int MaxHappy = 100;
    [Header("最低胜利愉悦值")]
    public int MinWinHappy = 60;

    [Header("熊孩子出现波"), SerializeField]
    public List<ChildConfig> ChildWave;
    [Header("奖励配置"), SerializeField]
    public List<GiftConfig> Gifts;

    [Header("生成体代码"), SerializeField]
    public Dictionary<ESpawn, GameObject> PrefabMap;

    Dictionary<string, GiftConfig> GiftMap;

    [HideInInspector]
    public float CurTime;
    [HideInInspector]
    public float CurHappy;

    bool bGameStart = false; // 游戏是否开始
    bool bGameEnd = false; // 游戏是否结束
    bool bGameWin = false;

    [Header("胜利事件")]
    public UnityEvent OnWinEvent;
    [Header("失败事件")]
    public UnityEvent OnLoseEvent;
    //三种火车预制体
    [Header("火车预制体")]
    public GameObject train;
    [Header("熊孩子预制体")]
    public GameObject child;
    [Header("三种火车的Sprite")]
    public List<Sprite> TrainSpriteMap;
    
    [HideInInspector]
    public static GameMode Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PrefabMap = new Dictionary<ESpawn, GameObject>();
        PrefabMap.Add(ESpawn.火车, train);
        PrefabMap.Add(ESpawn.熊孩子, child);

        foreach (var it in Gifts)
        {
            GiftMap[it.name] = it;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bGameStart)
        {
            CurTime += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        bGameStart = true;
        bGameEnd = false;
        bGameWin = false;
        CurHappy = 0;
        CurTime = 0;
    }

    public void CheckGameFinished()
    {
        if (CurTime > MaxTime)
        {
            bGameStart = false;
            bGameEnd = true;
        }
        if(bGameEnd)
        {
            if (CurHappy > MinWinHappy)
            {
                bGameWin = true;
                OnWinEvent.Invoke();
            }
            else
            {
                OnLoseEvent.Invoke();
            }
        }
    }

    public void GetGift(string name)
    {
        CurHappy += GiftMap[name].Value;
        CheckGameFinished();
        
    }

    // 生成物体通用逻辑
    public GameObject SpawnActor(ESpawn type, Transform transform, int ParamInfo = 0)
    {
        if(PrefabMap.ContainsKey(type))
        {
            var obj = Instantiate(PrefabMap[type], transform.position + transform.up, Quaternion.identity);
            var info = obj.GetComponent<ISpawnInfo>();
            if(info!= null)
            {
                info.SetParam(ParamInfo);
            }
            return obj;
        }
        else
        {
            print("生成物体" + type + "失败");
        }
        return null;
    }
}
