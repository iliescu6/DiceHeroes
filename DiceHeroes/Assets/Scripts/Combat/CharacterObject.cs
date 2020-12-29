using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public string testEnemy;
    public BaseCharacter baseCharacter;
    // Start is called before the first frame update
    void Awake()
    {
        if (!string.IsNullOrEmpty(testEnemy))
        {
            baseCharacter = new BaseCharacter();
            baseCharacter.SetCharacterStats(testEnemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
