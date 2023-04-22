using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlower : MonoBehaviour
{

    private Rigidbody2D rigid2D;        // Rigidbody2D 컴포넌트를 사용하기 위한 레퍼런스 선언
    public AudioClip[] powerUpClips;    // 버섯을 먹었을 때 플레이 할 수 있는 오디오 클립 배열


    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    //private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {

            //anim.SetBool("Collide", true);
            Debug.Log("플라워 콜라이더 확인");
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
}
