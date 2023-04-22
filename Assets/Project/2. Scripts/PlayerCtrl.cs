using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl player;
    private Animator anim;  // Player 객체의 Animator component 를 위한 Reference(참조)이다.
    private Rigidbody2D rigid2D; // 사용하는 객체의 Rigidbody2D component를 위한 Reference(참조)이다.
    private Vector2 tr;
    


    // 인스펙터에 노출 안됨
    [HideInInspector]
    public bool dirRight = true;        // 플레이어의 현재 바라보는 방향을 알기 위함

    [HideInInspector]
    public bool jump = false;           // 플레이어가 현재 점프중인지 아닌지를 알기 위함.
    public float jumpForce = 100f;     // 플레이어가 점프를 할 때의 추가되는 힘의 양
    public AudioClip[] jumpClips;       // 플레이어의 여럿 점프 사운드를 위한 오디오 클립 배열
    public bool spmario;        //플레이어(마리오)의 상태를 알기 위한 state bool 변수 선언
    public bool redmario;

    private bool grounded = false;      // 플레이어가 땅에 있는지 아닌지를 구별하기 위함.
    private bool overed = false;        // 플레이어가 점프를 하여 블록 콜라이더랑 부딪혔는지를 알기 위함.
    private Transform groundCheck;      // 만약 플레이어가 땅에 있을 때 position을 marking할 곳
    private Transform overCheck;
    //private Transform overCheck;        // 만약 플레이어가 점프를하여 블록 콜라이더랑 부딪혔을 때를 체크하기 위함.

    public float moveForce = 10f;      // 플레이어의 왼쪽 오른쪽 이동을 위한 추가되는 힘의 양.
    public float maxSpeed = 4f;         // 가장 빠르게 x축 안에서 플레이어가 이동 할 수 있는 최고의 속도

    //마리오는 조롱사운드가 없으므로 주석처리
    /*
        public float tauntProbability = 50f;    // 플레이어가 적을 조롱하게 기회 제공을 위한 변수
        public AudioClip[] taunts;              // 플레이어가 적을 조롱할 때를 위하는 오디오 클립 배열
        private int tauntIndex;                 // 가장 최근에 플레이된(조롱) taunts 배열의 Index의 저장을 위한 변수
        public float tauntDelay = 1f;           // 조롱이 발생할 때 딜레이를 줘야만 한다. 안그러면 사운드가 중복 된다.
    */

    void Awake()
    {
        // 레퍼런스(참조)들을 셋팅.
        groundCheck = transform.Find("groundCheck");    // groundCheck 오브젝트의 위치를 찾는다.
        //Debug.Log(groundCheck.transform);
        overCheck = transform.Find("overCheck");
        //overCheck = transform.Find("overCheck");
        tr = transform.position;

        anim = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        spmario = false;
        redmario = false;
    }


    // Update is called once per frame
    void Update()
    {
        // 플레이어 position 으로부터 groundcheck position 까지 linecast(두 점을 잇는 선을 그림)할 때
        // 만약 충돌한 어떤 객체의 Layer 값이 'Ground' Layer 라면 현재 플레이어는 땅에 있는거다.

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //overed = Physics2D.Linecast(transform.position, overCheck.position, 1 << LayerMask.NameToLayer("Bricks"));

        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;

    }

    // OnControllerColliderHit는 이동을 수행하는 중에 컨트롤러가 collider에 도달하는 경우 호출됨.
    //public void OnControllerColliderHit(ControllerColliderHit item)
    //{
    //}

    //private void OnTriggerEnter2D(Collider2D item)
    private void OnCollisionEnter2D(Collision2D item)
    {
        if (item.gameObject.tag == "item" && spmario == false)
        {
            //Time.timeScale = 0;
            Debug.Log(Time.timeScale);


            //Debug.Log("진화!!!");
            spmario = true;//state[1]=1이면 슈퍼마리오
            if (spmario)
            {
                anim.SetTrigger("Evolution");
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.125f);
                GetComponent<BoxCollider2D>().size = new Vector2(0.18f, 0.4f);
                groundCheck.transform.position = new Vector2(transform.position.x, transform.position.y - 0.3f);
                overCheck.transform.position = new Vector2(transform.position.x, transform.position.y + 0.16f);
                //overCheck.GetComponent<CapsuleCollider2D>().size = new Vector2(0.145f, 0.175f);
                anim.SetBool("spmario", true);
            }

        }
        else if (item.gameObject.tag == "item" && spmario == true)
        {
            anim.SetTrigger("redEvolution");
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.125f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.18f, 0.4f);
            groundCheck.transform.position = new Vector2(transform.position.x, transform.position.y - 0.3f);
            overCheck.transform.position = new Vector2(transform.position.x, transform.position.y + 0.16f);

            anim.SetBool("redmario", true);
        }
    }
    
   

    void timeRestart()
    {
        Time.timeScale = 1f;
    }

    //고정 시간마다 호출
    void FixedUpdate()
    {
        // Input 클래스 안에 다음 GetAxis() 함수 호출로 horizontal 입력을 캐치한다.
        float h = Input.GetAxis("Horizontal");

        // animator 컴포넌트에 parameter(매개변수)인 Speed에 horizontal(수평) 입력값의 절대값(Mathf.Abs())을 셋팅한다.
        anim.SetFloat("Speed", Mathf.Abs(h));

        // 만약 플레이어의 바라보는 방향이 바뀌거나 혹은 maxSpeed에 아직 도달하지 않을 때
        // ( h(-1.0f~1.0f)는 velocity.x를 다르게 표시한다.)
        if (h * rigid2D.velocity.x < maxSpeed)
        {
            // 플레이어 객체에 힘을 가한다.
            rigid2D.AddForce(Vector2.right * h * moveForce);
        }

        // 만약에 플레이어의 수평 속도가 maxSpeed 보다 크면
        if (Mathf.Abs(rigid2D.velocity.x) > maxSpeed)
        {
            // "Mathf.Sign(매개변수)"는 매개변수를 참조해서 1 또는 -1(float)을 반환
            // public static float Sign(float f);
            // Mathf.Sing()은 float의 부호를 반환하는 함수이다. 
            // 0이나 양수일 경우 1을, 음수일 경우 -1을 반환한다.

            // ex) Debug.Log(Mathf.Sign(-10)); => 출력 값 : -1
            // ex) Debug.Log(Mathf.Sign(10));  => 출력 값 :  1

            // 플레이어의 velocity(속도)를 x축 방향으로 maxSpeed로 셋팅해줘라 또한 기존 rigidbody2D.velocity.y도 셋팅 해 줘야 한다.
            rigid2D.velocity = new Vector2(Mathf.Sign(rigid2D.velocity.x) * maxSpeed, rigid2D.velocity.y);
        }

        // 만약 플레이어가 왼쪽을 바라볼 때 플레이어를 오른쪽으로 이동하게 입력했다면
        if (h > 0 && !dirRight)
        {
            // 플레이어를 뒤집어라
            Flip();
        }
        // 그렇지 않고 만약 플레이어가 오른쪽을 바라볼 때 플레이어를 왼쪽으로 이동하게 입력했다면
        else if (h < 0 && dirRight)
        {
            // 플레이어를 뒤집어라
            Flip();
        }

        // 만약 플레이어가 점프를 한다면
        if (jump)
        {
            // animator의 trigger(전환) parameter에 Jump를 셋팅
            anim.SetTrigger("Jump");

            // jump audio clip이 랜덤으로 플레이 된다.
            int i = Random.Range(0, jumpClips.Length);
            AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

            //플레이어에게 수직 힘이 가해진다.
            rigid2D.AddForce(new Vector2(0f, jumpForce));

            // Update에서 조건이 만족하여 점프상태가 될 때까지 확실하게 플레이어가 다시 점프를 못하게 만들어라.
            jump = false;
        }


        // 플레이어의 현재 방향을 바꿔주는 함수
        void Flip()
        {
            // 플레이어의 바라보는 방향을 바꾸자
            dirRight = !dirRight;
            anim.SetTrigger("Skid");

            // 플레이어의 local scale x에 -1을 곱하자
            Vector3 theScale = transform.localScale;
            theScale.x *= -1; //local scale x의 값에 -1을 곱했으므로 오브젝트의 방향이 반대가 된다.
            transform.localScale = theScale; // -1을 곱하여 방향이 바뀐 localScale의 값을 현재의 localScale 값에 대입한다.
        }




    }
}
