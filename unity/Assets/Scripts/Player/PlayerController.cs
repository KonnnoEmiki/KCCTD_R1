using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonobitEngine.MonoBehaviour,IObserver<PlayerAnimationEvent>
{
	[SerializeField]
	private float m_ForwordSpeed = 5;	// 前進速度
	[SerializeField]
	private float m_BackSpeed = 1;		// 後進速度
	[SerializeField,Range(0,10)]
	private float m_RotationSpeed = 4;	// 回転速度
	[SerializeField]
	private float m_JumpPow = 4;		// ジャンプ力

	// 各種Animatorのパラメータ設定用
	[SerializeField]
	private AnimationParameter m_JumpParam = new AnimationParameter();
	[SerializeField]
	private AnimationParameter m_SpeedParam = new AnimationParameter();
	[SerializeField]
	private AnimationParameter m_DirectionParam = new AnimationParameter();

	private Player m_Player = null;
	private PlayerInput m_Input = null;
	private PlayerAnimationController m_AnimController = null;
	private Rigidbody m_RigidBody = null;
	private Camera m_Camera = null;

	private float m_JumpStartTimeMoveKeyValue = 0;

	void Start()
	{
		m_AnimController = GetComponent<PlayerAnimationController>();
		m_RigidBody = GetComponent<Rigidbody>();
		m_Player = GetComponent<Player>();
		m_Input = GetComponent<PlayerInput>();
		m_Camera = Camera.main;
		m_AnimController.AddObserver(this); // アニメーションイベント通知受け取り用
	}

	void Update()
	{
		if (GameManager.IsGameSet) return;	// 決着がついていれば
		if (monobitView.isMine == false) return;	// 所有権が無ければ
		if (m_Player.IsDown) return;				// 倒れていれば

		m_AnimController.SetAnimationParameter(m_SpeedParam, m_Input.MoveKeyVal);
		m_AnimController.SetAnimationParameter(m_DirectionParam, m_Input.RotationKeyVal);
	}

	void FixedUpdate()
    {
		if (GameManager.IsGameSet) return; // 決着がついていれば
		if (monobitView.isMine == false) return;	// 所有権が無ければ
		if (m_Player.IsDown) return;				// 倒れていれば

		if (m_Input.HasJumpKeyDown && m_Player.HasGrounded && m_Player.IsPlayPlaceAnim == false)
		{
			PlayJumpAnim();
			m_JumpStartTimeMoveKeyValue = m_Input.MoveKeyVal;
		}

		// ジャンプ中以外のその場アニメーション再生中は移動,回転処理は走らせない
		if (m_Player.IsPlayPlaceAnim && m_Player.IsJumping == false) return;

		Rotation(); // 回転
		Move();     // 移動
	}
	
	// カメラの向いている方向に回転 & カメラから見て左右方向に回転
	void Rotation()
	{
		if (m_Input.HasMoveKeyDown == false && m_Input.HasRotationKeyDown == false)
			return;
		if (m_Camera == null)
			return;

		Vector3 lookDir = Vector3.right * m_Input.RotationKeyVal + Vector3.forward;
		lookDir = m_Camera.transform.TransformVector(lookDir);
		lookDir.y = 0;
		var rotationSpeed = m_RotationSpeed * Time.deltaTime;
		// ジャンプ中なら
		if (m_Player.IsJumping)
			rotationSpeed *= 0.5f;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), rotationSpeed);
	}

	// キャラの向いている方向に移動
	void Move()
	{
		if (m_Input.HasMoveKeyDown == false)
			return;

		float moveVal = m_Input.MoveKeyVal * Time.deltaTime;

		// 前進,後進で移動速度を変更
		if (m_Input.MoveKeyVal < 0)
			moveVal *= m_BackSpeed;		// 後進なら
		else
			moveVal *= m_ForwordSpeed;  // 前進なら
		// ジャンプ中なら
		if (m_Player.IsJumping)
			moveVal *= 0.5f;

		m_RigidBody.MovePosition(transform.TransformPoint(Vector3.forward * moveVal));
	}

	// ジャンプアニメーション再生
	[MunRPC]
	private void PlayJumpAnim()
	{
		// AnimatorのTriggerは同期されないので、
		// 所有権を持っていれば自分以外のプレイヤーにRPCを飛ばし、
		// 所有権の有無を問わずAnimatorにパラメータをセットする
		if (monobitView.isMine)
			monobitView.RPC("PlayJumpAnim", MonobitEngine.MonobitTargets.Others);
		m_AnimController.SetAnimationParameter(m_JumpParam);
	}

	// アニメーションイベント受け取り用
	public void OnNotify(Observable<PlayerAnimationEvent> observer, PlayerAnimationEvent e)
	{
		EventDispatcher dispatcher = new EventDispatcher(e);
		dispatcher.Dispatch<OnJumpEvent>(OnJump);
	}
	
	// アニメーション中でキャラクターがジャンプしたタイミング
	public void OnJump()
	{
		// プレイヤーのジャンプ処理

		m_RigidBody.velocity += (Vector3.up * m_JumpPow + transform.forward * m_JumpStartTimeMoveKeyValue);
	}

	public void OnDown()
	{
		m_AnimController.SetAnimationParameter(m_SpeedParam, 0.0f);
		m_AnimController.SetAnimationParameter(m_DirectionParam, 0.0f);
	}

}
