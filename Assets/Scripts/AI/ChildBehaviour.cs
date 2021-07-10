using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
public class ChildBehaviour : MonoBehaviour
{
    public NavMeshAgent mAngent;
    public Pathfinding.IAstarAI Finder;
    public Rigidbody2D mRigidbody;
    public SceneBase ScenePoint;
    ChildType mType;
    [SerializeField]
    public StateMachine mStateMachine;

    [Header("最短驻留时间")]
    public float minWaitTime;
    [Header("最长驻留时间")]
    public float maxWaitTime;
    [Header("最短閒逛时间")]
    public float minHangTime;
    [Header("最长閒逛时间")]
    public float maxHangTime;

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
    }

    // 移动到某个场地
    public void MoveToScenePoint(SceneBase To)
    {
        ScenePoint = To;
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
        return Finder.isStopped;
    }
}
