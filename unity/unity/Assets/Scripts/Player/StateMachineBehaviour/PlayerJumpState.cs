using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : StateMachineBehaviour
{
	private PlayerAnimationController m_AnimController = null;
	
	// アニメーション開始
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		m_AnimController = animator.GetComponent<PlayerAnimationController>();

		if (m_AnimController == null) return;

		m_AnimController.NotifyObservers(new OnJumpAnimationStartEvent());
		m_AnimController.NotifyObservers(new OnPlaceAnimationStartEvent());
	}
	
	// アニメーション終了
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (m_AnimController == null) return;

		m_AnimController.NotifyObservers(new OnJumpAnimationEndEvent());
		m_AnimController.NotifyObservers(new OnPlaceAnimationEndEvent());
	}
}
