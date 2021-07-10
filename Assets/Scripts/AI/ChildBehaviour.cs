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
public class ChildBehaviour : MonoBehaviour, IInteractiveElement
{
    public NavMeshAgent mAngent;
    public Pathfinding.IAstarAI Finder;
    public Rigidbody2D mRigidbody;
    public SceneBase TargetScenePoint;
    public ChildType mType;
    [SerializeField]
    public StateMachine mStateMachine;


    [Header("驻留货架多久后拿火车")]
    public float minCatchTrainTime;
    [Header("持有火车时间")]
    public float maxKeepTrainTime;


    [Header("最短驻留时间")]
    public float minWaitTime;
    [Header("最长驻留时间")]
    public float maxWaitTime;
    [Header("最短閒逛时间")]
    public float minHangTime;
    [Header("最长閒逛时间")]
    public float maxHangTime;

    public int TrainOnHand = 0;

    [Header("当前需要的火车")]
    public int NeedTrain = 0; 
    public SceneBase CurScene;
    public float CurKeepTrainTime;

    public UnityEvent SuccessEvent;
    public UnityEvent FailEvent;

    // Start is called before the first frame update
    void Start()
    {
        mAngent = GetComponent<NavMeshAgent>();
        mRigidbody = GetComponent<Rigidbody2D>();
        mStateMachine = new StateMachine(new StateStart(), this);
        Finder = GetComponent<Pathfinding.IAstarAI>();
    }

    // Update is called once per frame
    void Update()
    {
        mStateMachine.Tick(Time.deltaTime);
        if(TrainOnHand != 0)
        {
            CurKeepTrainTime += Time.deltaTime;
        }
        CheckCurScene();
    }
    void CheckCurScene()
    {
        // Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2, 0 << 8 | 1 << 9);
        if(IsStopped())
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

    // 移动到某个场地
    public void MoveToScenePoint(SceneBase To)
    {
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
        if(TrainOnHand != 0)
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
        if(NeedTrain == InCode)
        {
            SuccessEvent.Invoke();
        }
        else
        {
            FailEvent.Invoke();
        }
        NeedTrain = 0;
        return 0;
    }

    // 放下火车
    public void PushDownTrain()
    {
        if(TrainOnHand != 0)
        {
            GameMode.Instance.SpawnActor(TrainOnHand, transform);
            TrainOnHand = 0;
        }
    }

    public bool CanInteract(MonoBehaviour Source)
    {
        return Source.GetComponent<PlayerController>() != null;
    }
}
