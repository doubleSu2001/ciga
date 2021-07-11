using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour, IInteractiveElement, ISpawnInfo
{
    [Header("火车类型")]
    public int TrainType = 1;
    public SpriteRenderer mSprite;


    public bool CanInteract(MonoBehaviour Source)
    {
        return Source.GetComponent<PlayerController>() != null;
    }

    public void SetParam(int i)
    {
        TrainType = i;
        if(GameMode.Instance.TrainSpriteMap.Count > i)
        {
            mSprite.sprite = GameMode.Instance.TrainSpriteMap[i];
        }
    }

    public int TryInteract(int InCode)
    {
        if(InCode == 0)
        {
            Destroy(gameObject);
            return TrainType;
        }
        return 0;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
