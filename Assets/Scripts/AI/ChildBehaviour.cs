using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum ChildType
{
    Normal,
    Chaos,
}



/*

// 小孩的状态： 
    1. 初到 Start
    2. 停留（货架/火车轨道）
    3. 游荡
    4. 离场
     */



// 熊孩子的行为逻辑
public class ChildBehaviour : MonoBehaviour, IInteractiveElement, ISpawnInfo
{
    public NavMeshAgent mAngent;
    public Pathfinding.IAstarAI Finder;
    public Rigidbody2D mRigidbody;
    public SceneBase TargetScenePoint;
    public ChildType mType;
    public SpriteRenderer mOnHand;
    [SerializeField]
    public StateMachine mStateMachine;


    [Header("驻留货架多久后拿火车")]
    public float minCatchTrainTime = 5;
    [Header("主动拿货架火车的概率")]
    public float probCatchTrain = 0.5f;
    [Header("持有火车时间")]
    public float maxKeepTrainTime = 10;

    [Header("驻留后离开的概率")]
    public float probToLeave = 0.2f;
    [Header("最短驻留时间")]
    public float minWaitTime = 10;
    [Header("最长驻留时间")]
    public float maxWaitTime = 20;
    [Header("最短閒逛时间")]
    public float minHangTime = 10;
    [Header("最长閒逛时间")]
    public float maxHangTime = 20;
    [Header("最长生命周期")]
    public float maxLifeTime = 120;
    [Header("想要的间隔")]
    public int SepWantTrain = 30;

    int mTrainOnHand = 0;
    public int TrainOnHand
    {
        get {return mTrainOnHand; }
        set {
            if(value != mTrainOnHand)
            {
                mOnHand.sprite = GameMode.Instance.TrainSpriteMap[value];
            }
            mTrainOnHand = value;
        }
    }

    public int NeedTrain = 0;
    public SceneBase CurScene;
    public float CurKeepTrainTime;
    public float WantAcc = 0;
    public float LifeTime;

    public UnityEvent SuccessEvent;
    public UnityEvent ErrorEvent;
    public UnityEvent FailEvent;

    // Start is called before the first frame update
    void Start()
    {
        mAngent = GetComponent<NavMeshAgent>();
        mRigidbody = GetComponent<Rigidbody2D>();
        mStateMachine = new StateMachine(new StateStart(), this);
        Finder = GetComponent<Pathfinding.IAstarAI>();
        LifeTime = 0;
        SuccessEvent.AddListener(OnGiveEnd);
        ErrorEvent.AddListener(OnGiveEnd);
        FailEvent.AddListener(OnGiveEnd);
        SuccessEvent.AddListener(GameMode.Instance.OnGiveChildSuccessEvent);
        ErrorEvent.AddListener(GameMode.Instance.OnGiveChildErrorEvent);
        FailEvent.AddListener(GameMode.Instance.OnGiveChildFailEvent);
    }

    // Update is called once per frame
    void Update()
    {
        mStateMachine.Tick(Time.deltaTime);
        if (TrainOnHand != 0)
        {
            CurKeepTrainTime += Time.deltaTime;
        }
        else
        {
            CheckNeedTrain();
        }
        LifeTime += Time.deltaTime;

        CheckCurScene();
    }
    void CheckCurScene()
    {
        // Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2, 0 << 8 | 1 << 9);
        if (IsStopped())
        // if(Finder.isStopped)
        {
            CurScene = TargetScenePoint;
        }
        else
        {
            CurScene = null;
        }
        // foreach(var it in cols)
        // {
        //     CurScene = it.GetComponent<SceneBase>();
        //     if(CurScene != null)
        //     {
        //         return;
        //     }
        // }
    }



    void CheckNeedTrain()
    {
        if(NeedTrain == 0 && TrainOnHand == 0)
        {
            WantAcc += Time.deltaTime;
            if (WantAcc > SepWantTrain)
            {
                WantAcc = 0;
                NeedTrain = Random.Range(1, 4);
                PlayOverHead(NeedTrain);
            }
        }
    }
    // 移动到某个场地
    public void MoveToScenePoint(SceneBase To)
    {
        if(To == null)
        {
            return;
        }
        TargetScenePoint = To;
        To.Arrive();
        // mAngent.SetDestination(To.GetPosition());
        Finder.destination = To.GetPosition();
        Finder.SearchPath();
    }
    // 隐藏小孩
    public void Hide()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public bool IsStopped()
    {
        return Finder.velocity == Vector3.zero;
    }

    public bool IsEmptyHand()
    {
        return TrainOnHand == 0;
    }

    public bool IsArrive()
    {
        return TargetScenePoint == CurScene;
    }

    public bool TryGetTrain()
    {
        if(TrainOnHand != 0 || NeedTrain != 0)
        {
            return false;
        }
        // 试图交互的小孩类型 如果是坏孩子 会拆火车 否则只会拿货架
        SceneRail sceneRail = CurScene.GetComponent<SceneRail>();
        bool IsRail = sceneRail != null;
        if(IsRail)
        {
            if(ChildType.Normal == mType)//正常小孩 只观赏
            {
                return false;
            }
            if(sceneRail.IsCoolingDown())
            {
                return false;
            }
        }
        IInteractiveElement Elem = CurScene.GetComponent<IInteractiveElement>();
        if(Elem != null && Elem.CanInteract(this))
        {
            TrainOnHand = Elem.TryInteract(TrainOnHand);
            if(TrainOnHand != 0)
            {
                CurKeepTrainTime = 0;
                return true;
            }
        }
        return false;
    }

    // 与小孩的交互：给火车
    public int TryInteract(int InCode)
    {
        TrainOnHand = InCode;
        print("给了火车");
        if(NeedTrain == InCode)
        {
            SuccessEvent.Invoke();// 给对
            PlayOverHead(GameMode.Instance.GiftMap["递交成功"].index + 10);
        }
        else
        {
            ErrorEvent.Invoke();// 给错
            PlayOverHead(GameMode.Instance.GiftMap["递交失败"].index + 10);
        }
        NeedTrain = 0;
        return 0;
    }

    // 放下火车
    public void PushDownTrain()
    {
        if(TrainOnHand != 0)
        {
            GameMode.Instance.SpawnActor(ESpawn.火车, transform, TrainOnHand);
            TrainOnHand = 0;
        }
    }

    public void PlayOverHead(int param)
    {
        var obj = GameMode.Instance.SpawnActor(ESpawn.进度条, transform, param);
        obj.GetComponent<OverHead>().SetTarget(this);
    }

    public bool CanInteract(MonoBehaviour Source)
    {
        if (NeedTrain == 0)
            return false;
        return Source.GetComponent<PlayerController>() != null;
    }

    public void SetParam(int i)
    {
        mType = (ChildType)i;
    }
    

    public void OnGiveEnd()
    {
        NeedTrain = 0;
        WantAcc = 0;
    }
}
