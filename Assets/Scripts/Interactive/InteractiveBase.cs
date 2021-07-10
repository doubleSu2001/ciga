using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 可交互物接口
public interface IInteractiveElement
{
    // 试图交互 传入信号值 返回信号值
    int TryInteract(int InCode);

    bool CanInteract(MonoBehaviour Source);
}

public interface IInteractiveSource
{
    // 试图获取反馈
    void InteractiveAction(int Id);
}
