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
    进度条,
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
        [HideInInspector]
        public int index;
    }

    [Header("倒计时时长")]
    public int MaxTime = 30;
    [Header("初始愉悦值")]
    public int StartHappy = 10;
    [Header("总愉悦值")]
    public int MaxHappy = 100;
    [Header("最低胜利愉悦值")]
    public int MinWinHappy = 60;
    [Header("熊孩子产生需求后等待的时间")]
    public float NeedWaitTime = 10;
    [Header("表情出现时间")]
    public float FaceKeepTime = 5;

    [Header("熊孩子出现波"), SerializeField]
    public List<ChildConfig> ChildWave;
    public int WaveIndex = -1;
    [Header("熊孩子出现位置")]
    public Transform BirthPlace;
    [Header("熊孩子奖惩倍率")]
    public float ChaosMulit;

    [Header("奖励配置"), SerializeField]
    public List<GiftConfig> Gifts;

    [Header("生成体代码"), SerializeField]
    public Dictionary<ESpawn, GameObject> PrefabMap;

    public Dictionary<string, GiftConfig> GiftMap;

    [HideInInspector]
    public float CurTime;


    float curHappy;
    [HideInInspector]
    public float CurHappy
    {
        set
        {
            GetComponentInChildren<PlayerInfo>().OnScoreChange(value - curHappy);
            // OnHappyChange.Invoke(value - curHappy);
            curHappy = value;
        }
        get
        {
            return curHappy;
        }
    }

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
    [Header("熊孩子预制体 0 1 正常 2 熊")]
    public List<GameObject> children;
    [Header("进度条预制体")]
    public GameObject slider;
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
        GiftMap = new Dictionary<string, GiftConfig>();
        PrefabMap.Add(ESpawn.火车, train);
        // PrefabMap.Add(ESpawn.熊孩子, child);
        PrefabMap.Add(ESpawn.进度条, slider);

        for(int i = 0; i < Gifts.Count; i++)
        {
            var it = Gifts[i];
            if (null != it && null != it.name)
            {
                GiftMap[it.name] = it;
                it.index = i;
            }
        }
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (bGameStart)
        {
            CurTime += Time.deltaTime;
            if(WaveIndex < ChildWave.Count && ChildWave[WaveIndex].Time < CurTime)
            {
                for(int i = 0; i < ChildWave[WaveIndex].Count; i++)
                {
                    SpawnActor(ESpawn.熊孩子, BirthPlace, (int)ChildWave[WaveIndex].Type);
                }
                WaveIndex++;
            }
        }
    }

    public void StartGame()
    {
        bGameStart = true;
        bGameEnd = false;
        bGameWin = false;
        curHappy = StartHappy;
        CurTime = 0;
        WaveIndex = 0;
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
    

    // 生成物体通用逻辑
    public GameObject SpawnActor(ESpawn type, Transform transform, int ParamInfo = 0, bool bBias = true)
    {
        GameObject temp = null;
        if(type == ESpawn.熊孩子)
        {
            if((ChildType)ParamInfo == ChildType.Chaos)
            {
                temp = children[2];
            }
            else
            {
                temp = children[Random.Range(1, 3)];
            }
        }
        else if(PrefabMap.ContainsKey(type))
        {
            temp = PrefabMap[type];
        }
        var rd = transform.position;
        if (bBias)
        {
            rd += new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), 0) + transform.up;
        }
        var obj = Instantiate(temp, rd, Quaternion.identity);
        var info = obj.GetComponent<ISpawnInfo>();
        if (info != null)
        {
            info.SetParam(ParamInfo);
        }
        return obj;
    }

    public void ApplyGift(string type, ChildBehaviour Source)
    {
        print("奖励:" + type);
        if(GiftMap.ContainsKey(type))
        {
            float multi = 1.0f;
            if(Source.mType == ChildType.Chaos)
            {
                multi = ChaosMulit;
            }
            Source.PlayOverHead(GiftMap[type].index + 10);
            CurHappy += GiftMap[type].Value * multi;
            CheckGameFinished();
        }
    }

    public void OnGiveChildSuccessEvent()
    {

    }
    public void OnGiveChildErrorEvent()
    {

    }
    public void OnGiveChildFailEvent()
    {

    }

    public void OnArrangeSuccessEvent()
    {

    }
    public void OnArrangeErrorEvent()
    {

    }

}
