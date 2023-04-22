using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
	public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

	// 시작 딜레이
	public float DelayTime = 0.2f;
	// 총 플레이 타임(이 시간만큼 동작)
	// 테스트는 그냥 마우스로 destPos 잡고 계속 움직여보자.
	public float PlayTime = 1.0f;	
	private float delayTimer = 0.02f;
	private float playTimer = 0.0f;

	public Transform startPos;
	public Transform destPos;


	void Start () 
	{
	}
	
	void Update () 
	{
		if(delayTimer <= DelayTime)
		{
			delayTimer += Time.deltaTime;
			return;
		}

		if (playTimer <= PlayTime)
		{
			// 그래프로 속도 조절하자.
			transform.localPosition = Vector3.Lerp(startPos.localPosition, destPos.localPosition, curve.Evaluate(playTimer / PlayTime));
			playTimer += Time.deltaTime;
		}
	}
	/*
	  
		ac.Evaluate 함수에 들어가는 값은 Animation Curve에 X축 기준 값이다.(0에서 1)
		반환값은 그에 대응한 Y축 값이다.

	 */
}
