using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 movement;
    public Animator manim;
    public Rigidbody2D mrigidbody;
    public float movespeed;
    //0代表空手，1代表红车，2代表黄车，3代表绿车
    public int handsitem = 0;
    private GameObject itemonfloor = null;
    private GameObject childonfloor = null;
    //private GameObject itemonhand = null;
    public SpriteRenderer HandsSprite;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        if (movement == new Vector2(0, 0))
        {
            manim.SetBool("run", false);
        }
        else
        {
            manim.SetBool("run", true);
        }
        if(HandsSprite)
        {
            if(GameMode.Instance.TrainSpriteMap.Count>handsitem)
            {
                HandsSprite.sprite = GameMode.Instance.TrainSpriteMap[handsitem];
            }
            else
            {
                print("error:");
                print(handsitem);
                print(GameMode.Instance.TrainSpriteMap.Count);
                print(GameMode.Instance.gameObject);
            }
        }
        var targetpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = (targetpos - transform.position).normalized;
        dir.z = 0;
        transform.up = dir;
        TryInteractive();
        // if (itemonfloor != null && handsitem == 0 && Input.GetMouseButtonDown(0))
        // {
        //     if (itemonfloor.name == "红车") handsitem = 1;
        //     else if(itemonfloor.name == "黄车") handsitem = 2;
        //     else if(itemonfloor.name == "绿车") handsitem = 3;
        //     Destroy(itemonfloor);
        //     itemonfloor = null;
        // }
        // else if (handsitem != 0 && Input.GetMouseButtonDown(0) && childonfloor == null)
        // {
        //     if(handsitem == 1)
        //         Instantiate(GameMode.Instance.redtrain, transform.position + transform.up, Quaternion.identity);
        //     else if (handsitem == 2)
        //         Instantiate(GameMode.Instance.yellowtrain, transform.position + transform.up, Quaternion.identity);
        //     else if (handsitem == 3)
        //         Instantiate(GameMode.Instance.greentrain, transform.position + transform.up, Quaternion.identity);
        //     handsitem = 0;
        // }
    }
    private void FixedUpdate()
    {
        mrigidbody.MovePosition(mrigidbody.position + movement * movespeed * Time.fixedDeltaTime);
        // checkitems();
    }
    bool bBtnCoolDown;
    void TryInteractive()
    {
        if(bBtnCoolDown && Input.GetMouseButtonDown(0))
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position + transform.up, 1.5f);
            for (int i = 0; i < cols.Length; i++)
            {
                IInteractiveElement Elem = cols[i].GetComponent<IInteractiveElement>();
                if (Elem != null)
                {
                    var dir = (cols[i].transform.position - transform.position).normalized;
                    dir.z = 0;
                    if (Vector3.Angle(transform.up, dir) <= 60)
                    {
                        if(Elem.CanInteract(this))
                        {
                            handsitem = Elem.TryInteract(handsitem);
                            bBtnCoolDown = false;
                            return;
                        }
                    }
                }
            }
            if(handsitem != 0)
            {
                GameMode.Instance.SpawnActor(ESpawn.火车, transform, handsitem);
                handsitem = 0;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            bBtnCoolDown = true;
        }
    }

    void checkitems()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2,0 << 8|1<<9);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].CompareTag("Item"))
                {
                    var dir =  (cols[i].transform.position - transform.position).normalized;
                    if (Vector3.Angle(transform.up, dir) <= 60)
                    {
                        itemonfloor = cols[i].gameObject;
                        return;
                    }
                }
            }
        }


    }
    void checkchild()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2, 0 << 8 | 1 << 10);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].CompareTag("Child"))
                {
                    var dir = (cols[i].transform.position - transform.position).normalized;
                    if (Vector3.Angle(transform.up, dir) <= 60)
                    {
                        childonfloor = cols[i].gameObject;
                        return;
                    }
                }
            }
        }
    }
}
