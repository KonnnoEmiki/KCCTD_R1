using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーのアニメーション管理クラス
[RequireComponent(typeof(Animator),typeof(Player))]
public class PlayerAnimationController : ObservableMonoBehaviour<PlayerAnimationEvent>
{
	private Animator m_Animator = null;
	private Player m_Player = null;

    void Awake()
    {
		m_Animator = GetComponent<Animator>();
		m_Player = GetComponent<Player>();
    }
	
	// アニメーションのパラメータ設定
	public void SetAnimationParameter(AnimationParameter param,object value = null)
	{
		switch (param.m_Type)
		{
			case AnimationParameter.Type.Float:
				m_Animator.SetFloat(param.m_Index, (float)value);
			break;
			case AnimationParameter.Type.Int:
				m_Animator.SetInteger(param.m_Index, (int)value);
			break;
			case AnimationParameter.Type.Bool:
				m_Animator.SetBool(param.m_Index, (bool)value);
			break;
			case AnimationParameter.Type.Trigger:
				m_Animator.SetTrigger(param.m_Index);
			break;
		}
	}

	// アニメーションイベントからの通知受け取り用関数

	// ジャンプした時
	public void Jump()
	{
		var e = new OnJumpEvent();
		NotifyObservers(e);
	}

	// 着地した時
	public void Landing()
	{
		var e = new OnLandingEvent();
		NotifyObservers(e);
	}

}
