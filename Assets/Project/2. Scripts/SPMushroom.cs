using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPMushroom : MonoBehaviour
{
    private Animator anim;              // Animator 컴포넌트를 사용하기 위한 레퍼런스 선언
    private Rigidbody2D rigid2D;        // Rigidbody2D 컴포넌트를 사용하기 위한 레퍼런스 선언

    public float moveSpeed = 1f;        // 슈퍼마리오 버섯의 이동속도
    public AudioClip[] powerUpClips;    // 버섯을 먹었을 때 플레이 할 수 있는 오디오 클립 배열

    private SpriteRenderer playerRen;   // SpriteRenderer 컴포넌트를 위한 레퍼런스
    private Transform frontCheck;       // 버섯 앞에 있는 오브젝트를 체크하기 위해 사용되는 gameObject의 position을 위한 Reference
    private bool spmushroom = false;            // 버섯 오브젝트의 존재 여부를 파악하기 위한 변수
    //private Score score;                // Score 스크립트를 위한 레퍼런스

    private void Awake()
    {
        // 레퍼런스 셋팅
        //playerRen = transform.Find("Player").GetComponent<SpriteRenderer>();
        frontCheck = transform.Find("frontCheck").transform;
        rigid2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //score = GameObject.Find("Score").GetComponent<Score>();
    }

    private void FixedUpdate()
    {
        // 버섯 앞에 모든 콜라이더들의 배열을 생성(Physics2D.Linecast() 함수 참고)
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1 << LayerMask.NameToLayer("Ground")); ;


        // 1은 2의 0승이므로 Default Layer을 가리킨다.

        // 콜라이더들 각각을 체크한다.
        foreach (Collider2D obs in frontHits)
        {
            // 만약 어떤 오브젝트의 콜라이더의 태그가 Obstacle 이라면
            if(obs.tag == "Obstacle")
            {
                // 다른 콜라이더들을 체크하는 것을 멈추고 버섯을 뒤집어라
                Flip();
                break;
            }
            else if (obs.tag == "Player")
            {
                Destroy(gameObject);
                int i = Random.Range(0, powerUpClips.Length);
                AudioSource.PlayClipAtPoint(powerUpClips[i], transform.position);
            }
        }

        // 버섯의 속도를 x축 방향 moveSpeed 로 셋팅
        // LocalScale을 이용해서 스케일의 크기 만큼 속도가 증가하며, 방향 전환(Flip())시 LocalScale은 x축에 (-1)이 곱해져서 반대 방향으로 이동
        rigid2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigid2D.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    //private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {

            //anim.SetBool("Collide", true);
            Debug.Log("버섯 콜라이더 확인");
            //Time.timeScale = 0;
            //Debug.Log(Time.timeScale);
            ////anim.SetTrigger("Evolution");
            //PlayerCtrl.player.GetComponent<Animator>().SetTrigger("Evolution");
            Destroy(gameObject);
            int i = Random.Range(0, powerUpClips.Length);
            AudioSource.PlayClipAtPoint(powerUpClips[i], transform.position);
            Time.timeScale = 0;

        }
    }

    public void Flip()
    {
        // -1을 transform 컴포넌트 요소 LocalScale(벡터) x축에 곱한다. (버섯 방향전환)
        Vector2 mushroomScale = transform.localScale;
        mushroomScale.x *= -1;
        transform.localScale = mushroomScale;
    }
}
