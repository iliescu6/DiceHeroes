using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    public DiceSide[] sides;
    public bool stopped = false;
    [SerializeField]
    Rigidbody rigidBody;
    public int diceValue;
    public Vector3 spawningPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidBody.IsSleeping() && rigidBody.isKinematic == false)
        {
            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i].touchingGround)
                {
                    diceValue = sides[i].value;
                    stopped = true;
                    rigidBody.isKinematic = true;
                }
            }
        }
        else if(rigidBody.IsSleeping() && rigidBody.isKinematic == true )//&&
            //Vector3.Distance(transform.position, spawningPoint)<.1f)
        {
            MoveDiceToScreen();
        }
    }

    public void MoveDiceToScreen()
    {
        transform.position = Vector3.MoveTowards(transform.position, spawningPoint,Time.deltaTime*10);
    }

    public void RollDice()
    {
        rigidBody.isKinematic = false;
        rigidBody.AddRelativeForce(Vector3.left*400);
    }

    public void ResetDice()
    {
        rigidBody.isKinematic = true;
        stopped = false;
    }

}
