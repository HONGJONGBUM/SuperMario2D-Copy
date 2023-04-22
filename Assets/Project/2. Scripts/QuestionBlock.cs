using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    private bool spmario;        //플레이어(마리오)의 상태를 알기 위한 state bool 변수 선언
    private bool redmario;      
    private bool grounded = false;         // 블록을 플레이어가 점프로 부딪혔는지 알기 위함.
    private Transform groundCheck;	 		// 만약 플레이어가 점프로 부딪혔는지 알기 위함.

    private Animator anim;
    public GameObject coin;         // 인스펙터에서 코인 프리팹을 받기 위해 선언.
    public GameObject SPMushroom;   // 인스펙터에서 SPMushroom 프리팹을 받기 위해 선언.
    public GameObject HPMushroom;   // 인스펙터에서 HPMushroom 프리팹을 받기 위해 선언.
    public GameObject Flower;       // 인스펙터에서 Flower 프리팹을 받기 위해 선언.
    public GameObject Star;         // 인스펙터에서 Star 프리팹을 받기 위해 선언.
    public AudioClip[] itemClips;   // 플레이어가 머리를 부딪혀 버섯 아이템이 나올 때 오디오 클립 배열.
    public Vector2 offset;// 코인의 생성 위치값을 조정하기 위한 offset vector 변수 선언

    private bool boxOn = true; // collide block 는 변환 후 움직이면 안되는 오브젝트 이므로 Collide가 진행되었는지 아닌지 알 수 있게 하는 변수 선언

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //groundCheck = transform.Find("")
        

        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        spmario = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>().spmario;
        redmario = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>().redmario;
        // 현재 스크립트가 적용된 오브젝트 태그가 r 라면
        // 그리고 boxOn은 현재 true 이므로 처음에는 조건이 성립되어 if문을 들어가지만 한번 실행 후
        // boxOn이 false가 되므로 아래 if문은 동작하지 않는다. 즉 Collide 콜라이더도 1번 실행되고,
        // 코인도 한번만 생성되는 것이다.


        if (gameObject.tag == "CoinBox" && boxOn) //현재 게임 오브젝트의 Tag가 "CoinBox"면
        {


            //애니메이션 collide를 true로 바꾼다.
            anim.SetBool("QuestionCollide", true);

            // 코인의 생성 위치를 정하기 위한 vector2 변수 pos를 선언하고 코인의 y값의 위치만 변경할 것이기 때문에,
            // transform.positon.y 에만 + offset.y 를 추가한다.
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + offset.y);

            // 객체를 생성하는 함수Instantiate 를 사용하여 코인 프리팹을 생성 및 위치값 변수 pos의 위치로 코인 프리팹 오브젝트를 생성한다.
            Instantiate(coin, pos, Quaternion.identity);

            boxOn = false;
        }
        else if (gameObject.tag == "MushBox" && boxOn && !spmario)
        {
            //int i = Random.Range(0, itemClips.Length);
            //AudioSource.PlayClipAtPoint(itemClips[i], transform.position);

            //애니메이션 collide를 true로 바꾼다.
            anim.SetBool("QuestionCollide", true);

            // 코인의 생성 위치를 정하기 위한 vector2 변수 pos를 선언하고 코인의 y값의 위치만 변경할 것이기 때문에,
            // transform.positon.y 에만 + offset.y 를 추가한다.
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + offset.y);

            // 객체를 생성하는 함수Instantiate 를 사용하여 코인 프리팹을 생성 및 위치값 변수 pos의 위치로 코인 프리팹 오브젝트를 생성한다.
            Instantiate(SPMushroom, pos, Quaternion.identity);

            boxOn = false;
        }
        else if (gameObject.tag=="MushBox" && boxOn && spmario)
        {
            //애니메이션 collide를 true로 바꾼다.
            anim.SetBool("QuestionCollide", true);

            // 코인의 생성 위치를 정하기 위한 vector2 변수 pos를 선언하고 코인의 y값의 위치만 변경할 것이기 때문에,
            // transform.positon.y 에만 + offset.y 를 추가한다.
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + offset.y);
            Instantiate(Flower, pos, Quaternion.identity);

            boxOn = false;
        }

    }

    //private void OnCollisionEnter2D(Collision2D col) //col은 현재 이 스크립트를 가지고 있는 오브젝트에 부딪힌 상대 오브젝트의 collider 변수
    //{
    //    // 현재 스크립트가 적용된 오브젝트 콜라이더와 부딪히는 콜라이더의 오브젝트 태그가 Player 라면
    //    // 그리고 boxOn은 현재 true 이므로 처음에는 조건이 성립되어 if문을 들어가지만 한번 실행 후
    //    // boxOn이 false가 되므로 아래 if문은 동작하지 않는다. 즉 Collide 콜라이더도 1번 실행되고,
    //    // 코인도 한번만 생성되는 것이다.
    //    if (gameObject.tag == "CoinBox" && boxOn) //현재 게임 오브젝트의 Tag가 "CoinBox"면
    //    {
    //        //애니메이션 collide를 true로 바꾼다.
    //        anim.SetBool("Collide", true);

    //        // 코인의 생성 위치를 정하기 위한 vector2 변수 pos를 선언하고 코인의 y값의 위치만 변경할 것이기 때문에,
    //        // transform.positon.y 에만 + offset.y 를 추가한다.
    //        Vector2 pos = new Vector2(transform.position.x, transform.position.y + offset.y);

    //        // 객체를 생성하는 함수Instantiate 를 사용하여 코인 프리팹을 생성 및 위치값 변수 pos의 위치로 코인 프리팹 오브젝트를 생성한다.
    //        Instantiate(coin, pos, Quaternion.identity);
    //        boxOn = false;
    //    }
    //    else if (gameObject.tag == "MushBox" && boxOn)
    //    {
    //        //애니메이션 collide를 true로 바꾼다.
    //        anim.SetBool("Collide", true);

    //        // 코인의 생성 위치를 정하기 위한 vector2 변수 pos를 선언하고 코인의 y값의 위치만 변경할 것이기 때문에,
    //        // transform.positon.y 에만 + offset.y 를 추가한다.
    //        Vector2 pos = new Vector2(transform.position.x, transform.position.y + offset.y);

    //        // 객체를 생성하는 함수Instantiate 를 사용하여 코인 프리팹을 생성 및 위치값 변수 pos의 위치로 코인 프리팹 오브젝트를 생성한다.
    //        Instantiate(SPMushroom, pos, Quaternion.identity);
    //        boxOn = false;
    //    }
        
    //}   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
