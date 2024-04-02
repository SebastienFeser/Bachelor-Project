using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingJS : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject thisGameObject;
    float time = 5f;

    private void Update()
    {
        if(Time.timeSinceLevelLoad > time)
        {
            gameManager.JumpScarePlayer();
            Destroy(thisGameObject);
        }
    }
}
