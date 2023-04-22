using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Goomba : MonoBehaviour
{
    private Animator anim;

    public float moveSpeed = 0.5f;        // 몬스터의 이동속도
    public int HP = 1;                  // 몬스터의 생명력
    public Sprite deadEnemy;            // 몬스터가 죽었을 때 교체할 몬스터의 스프라이트
    public Sprite damagedEnemy;         // 몬스터가 데미지를 입었을 때 교체할 몬스터의 스프라이트(선택적이다.)
    public AudioClip[] deathClips;      // 몬스터가 죽었을 때 플레이 할 수 있는 오디오 클립 배열
    public GameObject hundredpointsUI;  // 몬스터가 죽었을 때 발생하는 100의 프리팹
    public float deathSpinMin = -100f;  // 몬스터가 죽었을 때 회전력의 최소량을 주기 위한 값
    public float deathSpinMax = 100f;   // 몬스터가 죽었을 때 회전력의 최대량을 주기 위한 값

    private SpriteRenderer ren;         // SpriteRenderer 컴포넌트를 위한 레퍼런스
    private Transform frontCheck;       // 만약 무엇이든 몬스터 앞에 있다면 체크를 위해 사용되는 gameObject의 position을 위한 Reference
    private bool dead = false;          // 몬스터가 죽었는지 아닌지를 알기 위한 변수
    //private Score score;                // Score 스크립트를 위한 레퍼런스
    private Rigidbody2D rigid2D;        // Rigidbody2D 컴포넌트를 위한 레퍼런스


    private void Awake()
    {
        anim = GetComponent<Animator>();
        // 레퍼런스들의 셋팅
        ren = transform.Find("Goomba").GetComponent<SpriteRenderer>();
        //Debug.Assert(ren);
        frontCheck = transform.Find("frontCheck").transform;
        //score = GameObject.Find("Score").GetComponent<Score>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // enemy 앞에 모든 콜라이더들의 배열을 생성 (PlayerCtrl 스크립트의 Physics2D.Linecast()함수 참고)
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1<<LayerMask.NameToLayer("Ground"));
        // 1은 2의 0승이므로 Default Layer을 가리킨다.
        // Collider[] frontHits = Physics.OverLapSphere(Vector3 position, float radus); //3D 게임일 때
        // 위와 같은 코드를 사용하면 드럼통 같은 오브젝트를 쏘면 그 주변이 폭발하는 코드를 구현할 수 있다.

        // 콜라이더들 각각을 체크
        foreach (Collider2D c in frontHits)
        {
            // 만약 어떤 콜라이더의 태그가 Obstacle 이라면...
            if (c.tag == "Obstacle" || c.tag == "Enemy")
            {
                // 다른 콜라이더들을 체크하는 것을 멈추고 몬스터를 뒤집어라
                Flip();
                Debug.Log("버섯플립");
                break;
            }
        }

        // 몬스터의 속도를 x축 방향 moveSpeed 으로 셋팅
        // localScale을 이용하여 스케일의 크기 만큼 속도가 증가하며, 방향 전환(Flip())시 localScale은 x축에 (-1)이 곱해져서 반대 방향으로 이동
        rigid2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigid2D.velocity.y);

        // 만약 몬스터의 HP가 1이고 damagedEnemy 스프라이트가 연결되어 있을 때
        if (HP == 1 && damagedEnemy != null)
        {
            // SpriteRenderer 컴포넌트에 멤버 sprite에 damagedEnemy 스프라이트 연결
            ren.sprite = damagedEnemy;
        }
        // 만약 몬스터의 HP가 0 또는 0 미만이고 아직 살아있다면 죽이자..
        if ( HP <= 0 && !dead)
        {
            // Death() 함수 호출
            Death();
        }
    }
    
    public void Hurt()
    {
        // 몬스터의 생명력을 1 만큼 줄인다.
        HP--;
    }

    void Death()
    {
        // 이 게임오브젝트를 포함하여 하위로 있는 자식 게임오브젝트에서 SpriteRenderer 컴포넌트들을 모두 찾는다.
        SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

        // 모든 SpriteRenderer 컴포넌트를 Disable 한다.
        foreach(SpriteRenderer s in otherRenderers)
        {
            s.enabled = false;
        }

        // 메인(Mario)인 ren이 가리키는 SpriteRenderer를 다시 활성화 하고, SpriteRenderer 컴포넌트의 멤버 Sprite에 deadEnemy 스프라이트로 셋팅
        ren.enabled = true;
        ren.sprite = deadEnemy;

        // 100 포인트의 스코어 증가
        //score.score += 100;

        // dead를 true 로 셋팅
        dead = true;

        // 몬스터의 회전을 위해 fixedAngel을 false로 하고 회전력의 추가에 의해서 몬스터를 회전시키자.
        rigid2D.fixedAngle = false;
        // z축으로 회전
        rigid2D.AddTorque(Random.Range(deathSpinMin, deathSpinMax));

        // 게임오브젝트에 collider2D 컴포넌트들을 모두 찾은 다음 Collider2D 컴포넌트가 모두 trigger가 되게 셋팅 하자.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        // deathClips 배열로부터 랜덤하게 audioclip을 플레이 하자
        int i = Random.Range(0, deathClips.Length);
        AudioSource.PlayClipAtPoint(deathClips[i], transform.position);

        // 몬스터 바로 위에 벡터를 생성
        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;

        // 이 벡터지점에서 100포인트 프리팹을 인스턴스로 만들자.
        Instantiate(hundredpointsUI, scorePos, Quaternion.identity);
    }

    public void Flip()
    {
        // -1을 transform 컴포넌트에 요소 localScale(벡터)의 x축에 곱하자.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }    
}
