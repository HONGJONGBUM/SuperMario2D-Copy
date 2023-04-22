using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameChecker : MonoBehaviour
{
    float deltaTime = 0.0f;

    GUIStyle style;
    Rect rect;
    float msec;
    float fps;
    float worstFps = 100f;
    string text;

    void Awake()
    {
        Application.targetFrameRate = 40;

        int w = Screen.width, h = Screen.height;

        rect = new Rect(0, 0, w, h * 4 / 100);

        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 4 / 100;
        style.normal.textColor = Color.cyan;

        StartCoroutine("WorstReset");
    }


    IEnumerator WorstReset() //코루틴으로 15초 간격으로 최저 프레임 리셋해줌.
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            worstFps = 100f;
        }
    }


    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()//소스로 GUI 표시.
    {

        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;  //초당 프레임 - 1초에

        if (fps < worstFps)  //새로운 최저 fps가 나왔다면 worstFps 바꿔줌.
            worstFps = fps;
        text = msec.ToString("F1") + "ms (" + fps.ToString("F1") + ") //worst : " + worstFps.ToString("F1");
        GUI.Label(rect, text, style);
    }
}


/*
    게임 화면에 프레임을 출력시키는 소스를 작성함. (현재 예상프레임과 최악일 때의 프레임 표시.)

    = 간혹 프레임이 떨어지는 현상(프레임 드랍) =

    이 프레임이 간혹 떨어지다보니 문제가 있어서 그냥 고정을 시켰음 (프레임은 35-40 정도라도 항상 안정적 유지)
    그리고 사람이 눈으로 구분할 수 있는 정도와 실제 기기에서 진행되는 프레임 차이가 있기 때문에
    낮아진 채로 고정시키는 것이 좋은거 같음. (날로 스마트폰이나 전자 기기들의 성능이 좋아져서 더욱 프레임이 잘 나옴)

    사실은 프레임 고정을 하지 않아도 되지만
    어느정도 안정적인 프레임을 원하여 고정하고싶을 때는 아래의 방법을 쓰면 됨.
    소스를 이용하여 적용시키는 방법은 아주 간단하다.

    Application.targetFrameRate = 40; 이 부분을 처음 게임 시작할 때 실행되는 Awake 부분.
    즉 게임이 시작되는 스크립트가 있는 부분의 Awake()에 넣어주면 된다.
    그러면 게임 시작시 프레임을 고정.



    표시되는 내용은 OnGUI에 있다. 그 중 화면에 나타내는 부분은
    text = msec.ToString ("F1") + "ms (" + fps.ToString ("F1") + ") //worst : " + worstFps.ToString ("F1");
    이 부분이다. float 수치인 msec을 문자열로 변환했다.
    여기서 ToString 옆에 ("F1") 부분이있다. 이는 표시형식인데 소수 한 자리까지 표시하겠다는 얘기이다.
    그리고 ms(밀리초) fps, worst까지 같은 형식으로 사용했다.


    그렇지만 현재는 그냥 소스만 넣어서 되지는 않는다.
    별도로 Vsync를 꺼줘야 별도로 프레임 조정이 가능하다.
    (Vsync를 사용하여 렌더링 처리를 디스플레이의 주사율과 동기화시킴으로 이미지 왜곡을 피할 수 있다.)
    (그래픽 처리하는 동안 CPU가 주기율을 맞추느라 쉬어감 - 배터리, 발열 저하)=> 중요

    = Vsync를 해제하거나 설정하는 방법 =
    1. 유니티 상단 메뉴 중 Edit을 선택하여 Project Settings - Quality 순으로 들어감.

    2. 그러면 우측에 Inspector 창에 Quality Settings 패널을 볼 수 있다.
       여기서 V Sync Count 부분을 바꿔줘야 한다.
       Don't Sync로 바꾸자. 여기서 만약 다시 사용한다면 Every V Blank를 선택해주자.
       이렇게 Don't Sync까지 선택하여줬다면 정상적으로 프레임 저하가 완료된 모습을 볼 수 있습니다.

 */
