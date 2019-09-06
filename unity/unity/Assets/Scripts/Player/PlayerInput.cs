using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの入力管理クラス
public class PlayerInput : MonoBehaviour
{
	[SerializeField, SelectableInputAxisAttribute]
	private SystemDefines.InputAxis m_MoveKey;

	[SerializeField, SelectableInputAxisAttribute]
	private SystemDefines.InputAxis m_RotationKey;

	[SerializeField, SelectableInputAxisAttribute]
	private SystemDefines.InputAxis m_JumpKey;

	private float m_MoveKeyVal = 0;
	public float MoveKeyVal { get { return m_MoveKeyVal; } }
	private bool m_HasMoveKeyDown = false;
	public bool HasMoveKeyDown { get { return m_HasMoveKeyDown; } }
	private float m_RotationKeyVal = 0;
	public float RotationKeyVal { get { return m_RotationKeyVal; } }
	private bool m_HasRotationKeyDown = false;
	public bool HasRotationKeyDown { get { return m_HasRotationKeyDown; } }
	private bool m_HasJumpKeyDown = false;
	public bool HasJumpKeyDown { get { return m_HasJumpKeyDown; } }
	private bool m_HasJumpKeyPressed = false;
	public bool HasJumpKeyPressed { get { return m_HasJumpKeyPressed; } }

	public bool IsInputEnable { get; set; } = true;

	private void Start()
	{
		var monobitView = GetComponent<MonobitEngine.MonobitView>();
		if (monobitView.isMine == false) Destroy(this); // 所有権が無ければ削除
	}

	private void Update()
	{
		m_MoveKeyVal		= 0;
		m_RotationKeyVal	= 0;
		m_HasJumpKeyDown	= false;
		m_HasJumpKeyPressed	= false;

		// フェード中はキー入力を受け付けないようにする
		if (ApplicationManager.Fade.IsDone == false) return;

		if (IsInputEnable == false) return;
		
		// 各種キー入力情報取得
		m_MoveKeyVal			= Input.GetAxis(m_MoveKey.Name);
		m_RotationKeyVal		= Input.GetAxis(m_RotationKey.Name);
		m_HasJumpKeyDown		= Input.GetKeyDown(m_JumpKey.PositiveButton);
		m_HasJumpKeyPressed		= Input.GetKey(m_JumpKey.PositiveButton);

		// 各種キーが押されているか判定
		m_HasMoveKeyDown		= Mathf.Abs(MoveKeyVal) > 0;
		m_HasRotationKeyDown	= Mathf.Abs(RotationKeyVal) > 0;
	}


}
