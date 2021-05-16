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
            if (!stopped)
            {
                RollDice();//Is it's sleeping in a bad position, move it again
            }
        }
        else if (rigidBody.IsSleeping() && rigidBody.isKinematic == true)//&&
                                                                         //Vector3.Distance(transform.position, spawningPoint)<.1f)
        {
            MoveDiceToScreen();
        }
    }

    public void MoveDiceToScreen()
    {
        transform.position = Vector3.MoveTowards(transform.position, spawningPoint, Time.deltaTime * 10);
        var vec = transform.eulerAngles;
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;//TODO have 90 as a variable and set it based on dice type
        transform.eulerAngles = vec;
    }

    public void RollDice()
    {
        rigidBody.isKinematic = false;
        rigidBody.AddRelativeForce(Random.Range(0, 400), Random.Range(0, 500), Random.Range(0, 500));
        rigidBody.AddTorque(Random.Range(0, 100), Random.Range(0, 500), Random.Range(0, 500));
    }

    public void ResetDice()
    {
        rigidBody.isKinematic = true;
        stopped = false;
    }

}
