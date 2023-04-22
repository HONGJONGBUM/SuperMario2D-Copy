using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rigid2D;

    private bool coin;  // 코인 오브젝트의 존재 여부를 파악하기 위한 변수

    private float coinForce =1f;
    


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        transform.position = gameObject.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        rigid2D.AddForce(new Vector2(0f, coinForce * 150f));
        // 프리펩 or 게임오브젝트 생성 명령어 Instantiate
        Destroy(gameObject, 0.6f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
    }
}
