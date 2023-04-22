using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFollow : MonoBehaviour
{
    public Vector3 offset;

    private Transform block;

    private void Awake()
    {
        block = GameObject.FindGameObjectWithTag("Block").transform;

        // offset 과 함께 block 의 position으로 현재 게임오브젝트의 position을 셋팅
        transform.position = block.position + offset;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
