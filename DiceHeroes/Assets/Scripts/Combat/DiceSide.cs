using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public bool touchingGround=false;
    bool stoppedRolling;
    [SerializeField]
    public int value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground" )//|| other.gameObject.tag == "Dice")
        {
            touchingGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")// || other.gameObject.tag == "Dice")
        {
            touchingGround = false;
        }
    }

}
