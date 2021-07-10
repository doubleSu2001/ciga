using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour, IInteractiveElement
{
    [Header("火车类型")]
    public int TrainType = 1;

    public bool CanInteract(MonoBehaviour Source)
    {
        return Source.GetComponent<PlayerController>() != null;
    }

    public int TryInteract(int InCode)
    {
        if(InCode == 0)
        {
            return TrainType;
        }
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
