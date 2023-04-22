using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    private Animator anim;
    private bool boxOn = false;

    public Vector2 offset;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {   // 현재 스크립트가 적용된 오브젝트 태그가 r 라면
        // 그리고 boxOn은 현재 true 이므로 처음에는 조건이 성립되어 if문을 들어가지만 한번 실행 후
        // boxOn이 false가 되므로 아래 if문은 동작하지 않는다. 즉 Collide 콜라이더도 1번 실행되고,
        // 코인도 한번만 생성되는 것이다.
        Debug.Log("확인");
        if (gameObject.tag == "Bricks" /*&& !boxOn*/) //현재 게임 오브젝트의 Tag가 "CoinBox"면        
        {
            //애니메이션 collide를 true로 바꾼다.
            anim.SetTrigger("BricksCollide");
            
            // 코인의 생성 위치를 정하기 위한 vector2 변수 pos를 선언하고 코인의 y값의 위치만 변경할 것이기 때문에,
            // transform.positon.y 에만 + offset.y 를 추가한다.
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + offset.y);

            // 객체를 생성하는 함수Instantiate 를 사용하여 코인 프리팹을 생성 및 위치값 변수 pos의 위치로 코인 프리팹 오브젝트를 생성한다.
            //Instantiate(coin, pos, Quaternion.identity);
            //boxOn = true;
        }
        

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
