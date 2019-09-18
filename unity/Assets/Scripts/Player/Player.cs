using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(Rigidbody),typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerController), typeof(PlayerAnimationController))]
public class Player : MonobitEngine.MonoBehaviour,IObserver<PlayerAnimationEvent>
{
	private string m_OwnerName;
	public string OwnerName { get { return m_OwnerName; } }

	private bool m_HasGrounded = false;
	public bool HasGrounded { get { return m_HasGrounded; } }

	private bool m_IsJumping = false;
	public bool IsJumping { get { return m_IsJumping; } }

	private bool m_IsPlayJumpAnim = false;

	private bool m_IsPlayPlaceAnim = false;
	public bool IsPlayPlaceAnim { get { return m_IsPlayPlaceAnim; } }

	private bool m_IsDown = false;
	public bool IsDown { get { return m_IsDown; } }

    public Vector3 Velocity
	{
		get
		{
			if (m_RigidBody == null)
				return Vector3.zero;
			return m_RigidBody.velocity;
		}
	}

	public Vector2 VelocityXZ
	{
		get
		{
			if (m_RigidBody == null)
				return Vector2.zero;
			var vel = m_RigidBody.velocity;
			return new Vector2(vel.x, vel.z);
		}
	}

	public float VelocityY
	{
		get
		{
			if (m_RigidBody == null)
				return 0;
			var vel = m_RigidBody.velocity;
			return vel.y;
		}
	}

	[SerializeField,Tag]
	public string m_BallTag;

	[SerializeField]
	private AnimationParameter m_GrandedParam = new AnimationParameter();

	[SerializeField]
	private AnimationParameter m_InFallParam = new AnimationParameter();

	[SerializeField]
	private AnimationParameter m_InLndingParam = new AnimationParameter();

	[SerializeField]
	private AnimationParameter m_OnJumpParam = new AnimationParameter();

	[SerializeField]
	private AnimationParameter m_OnDownParam = new AnimationParameter();

	[SerializeField]
	private AnimationParameter m_WinParam = new AnimationParameter();

	[SerializeField]
	private float m_FootColliderRadius;
	[SerializeField]
	private float m_CastDistance;

	[SerializeField]
	private float m_DurableValue = 1; // 耐久値(どれくらいのスビードのボールまで当たっても耐えるか)

	private Rigidbody m_RigidBody = null;
	private CapsuleCollider m_Collider = null;
	private PlayerController m_PlayerController = null;
	private PlayerAnimationController m_AnimController = null;

	private float m_HalfCastDistance;

	private Vector3 m_CastOffset;

    public static int Stamina = 3;

    public static int LifeCount = Stamina;

    public static bool sibouflag = false;
    public static int HighScore = 0;

    void Start()
    {
		m_RigidBody = GetComponent<Rigidbody>();
		m_Collider = GetComponent<CapsuleCollider>();
		m_PlayerController = GetComponent<PlayerController>();
		m_AnimController = GetComponent<PlayerAnimationController>();

        LifeCount = 3;

        sibouflag = false;

		Init();
    }

    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はkillerタグですか？
        if (hit.CompareTag("killer"))
        {
            // Unityちゃん is 死
            if (NetworkGUI.gs == true)
                if (monobitView.isMine == false)
                {
                    ScoreCounter.scoreflag = 3;
                    return;    // 所有権が無ければ
                }
                else LifeCount--;
            if (LifeCount == 0)
            {
                if (monobitView.isMine == false)
                sibouflag = true;
                OnDown();
            }
        }

        // 接触対象はmuscleタグですか？
        if (hit.CompareTag("muscle"))
        {
            // Unityちゃん is 不死
            if (NetworkGUI.gs == true)
            {
                NetworkGUI.gs = false;
                Delay();
            }
        }
    }

    static async void Delay()
    {
        await Task.Delay(3000);
        NetworkGUI.gs = true;
    }

    void Update()
	{
		if (GameManager.IsGameSet) return;
		if (monobitView.isMine == false) return;

		if (Input.GetKeyDown(KeyCode.Escape))
			ApplicationManager.CursorMgr.ToggleSettings();

        SetGroundedFlag();
	}

	void FixedUpdate()
	{
		if (GameManager.IsGameSet) return;
		bool prev = m_HasGrounded;
		CheckIsGrounding();

		// ジャンプアニメーション中以外で前フレームで接地したいてかつ現在接地していなければ落下したと見なす
		if (prev == true && m_HasGrounded == false && m_IsPlayJumpAnim == false)
			OnFall();
		// 前フレームで接地していなくて現在接地していれば着地したと見なす
		if (prev == false && m_HasGrounded)
			SetLanding();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (monobitView.isMine == false) return;
		if (collision.gameObject.tag != m_BallTag) return; // ボールじゃない
		if (m_IsDown) return; // すでに倒れている
		if (GameManager.IsGameSet) return; // 決着済み

		var rb = collision.gameObject.GetComponent<Rigidbody>();
		if (rb == null) return;

		var ballSpeed = rb.velocity.magnitude;
        if (ballSpeed * 100 > m_DurableValue) // ボールの速度が耐久値を上回っていたら
            if (NetworkGUI.gs == true)
                if (monobitView.isMine == false)
                {
                    ScoreCounter.scoreflag = 3;
                    return;    // 所有権が無ければ
                }
                else LifeCount--;
        if (LifeCount == 0)
        {
            if (monobitView.isMine == false)
            sibouflag = true;
            OnDown();
        }
	}

    void Init()
	{
		// 各種アニメーション関連のイベント通知受け取り用
		m_AnimController.AddObserver(this);
		// PlayerManagerへの通知が完了するまで重力OFF
		m_RigidBody.useGravity = false;
		m_HalfCastDistance = m_CastDistance * 0.5f;
		m_CastOffset = Vector3.up * m_HalfCastDistance;

		m_OwnerName = monobitView.owner.name;

		// InGameシーンのロードよりも早くスポーンする可能性があるのでコルーチンで処理
		StartCoroutine(NotifySpawnPlayer());

		if (monobitView.isMine == false) return;

		ApplicationManager.CursorMgr.SetCursorSettings();
	}

	// 接地判定
	void CheckIsGrounding()
	{
		// 実際のコライダーの位置の少し上から少し下へカプセルキャストしてみて何かに当たったら接地している判定にする
		RaycastHit hit;
		Vector3 castOffset = Vector3.up * m_HalfCastDistance;
		Vector3 p1 = transform.position + m_CastOffset + (Vector3.up * m_FootColliderRadius);		// 足元
		float castDist = (m_CastDistance + Physics.gravity.magnitude * Time.deltaTime * 0.5f);		// 重力を加味してキャストする距離を算出
		m_HasGrounded = Physics.SphereCast(p1, m_FootColliderRadius, -transform.up, out hit, castDist);
		if (m_HasGrounded) return;

		m_HasGrounded = Physics.Raycast(transform.position + m_CastOffset, -transform.up, castDist);
	}

	// PlayerManagerにスポーンしたことを通知
	private IEnumerator NotifySpawnPlayer()
	{
		// PlayerManagerのインスタンスが生成されるのを待ってから通知
		while (true)
		{
			if (PlayerManager.HasInstance == true)
				break;
			yield return null;
		}

		PlayerManager.Instance.OnSpawnPlayer(this);
		CheckIsGrounding();
		SetGroundedFlag(); // 地面判定(一応)

		if (m_RigidBody != null) m_RigidBody.useGravity = true;
	}

	// 決着時
	public void OnGameSet()
	{
		if(m_IsDown == false) // 倒れていなければ最後の生き残り
			PlayWinAnimation();
	}

	// 勝ちアニメーション再生
	private void PlayWinAnimation()
	{
		// 勝利アニメーション再生
		m_AnimController.SetAnimationParameter(m_WinParam);
	}

	// 倒れた
	[MunRPC]
	private void OnDown()
	{
		m_IsDown = true;
		PlayerManager.Instance.OnDown(this);

		if (monobitView.isMine)
		{
			monobitView.RPC("OnDown", MonobitEngine.MonobitTargets.Others);
			m_PlayerController.OnDown();
		}
		
		m_Collider.enabled = false;
		m_RigidBody.useGravity = false;
		m_RigidBody.velocity = Vector3.zero;

		m_AnimController.SetAnimationParameter(m_OnDownParam);
	}

	// アニメーションイベント受け取り時の処理
	public void OnNotify(Observable<PlayerAnimationEvent> observer, PlayerAnimationEvent e)
	{
		EventDispatcher dispatcher = new EventDispatcher(e);
		if (monobitView.isMine)
		{
			dispatcher.Dispatch<OnJumpEvent>(OnJump);
			dispatcher.Dispatch<OnPlaceAnimationStartEvent>(OnPlaceAnimStart);
			dispatcher.Dispatch<OnPlaceAnimationEndEvent>(OnPlaceAnimEnd);
		}
		
		dispatcher.Dispatch<OnJumpAnimationStartEvent>(OnJumpAnimStart);
		dispatcher.Dispatch<OnJumpAnimationEndEvent>(OnJumpAnimEnd);
		dispatcher.Dispatch<OnLandingEvent>(OnLanding);
	}

	// ジャンプアニメーション開始
	public void OnJumpAnimStart()
	{
		m_IsPlayJumpAnim = true;
	}

	// ジャンプアニメーション終了
	public void OnJumpAnimEnd()
	{
		m_IsPlayJumpAnim = false;
	}

	// アニメーション中のジャンプしたタイミング
	public void OnJump()
	{
		m_IsJumping = true;
		m_AnimController.SetAnimationParameter(m_OnJumpParam);
		ResetLanding();
	}

	// アニメーション中の着地したタイミング
	public void OnLanding()
	{
		m_IsJumping = false;
		ResetLanding();
	}

	// その場アニメーション開始
	public void OnPlaceAnimStart()
	{
		m_IsPlayPlaceAnim = true;
	}

	// その場アニメーション終了
	public void OnPlaceAnimEnd()
	{
		m_IsPlayPlaceAnim = false;
	}

	// 落下トリガーセット
	private void OnFall()
	{
		m_AnimController.SetAnimationParameter(m_InFallParam);
	}

	// 着地フラグ -> trueへ
	private void SetLanding()
	{
		m_AnimController.SetAnimationParameter(m_InLndingParam, true);
	}

	// 着地フラグ -> falseへ
	private void ResetLanding()
	{
		m_AnimController.SetAnimationParameter(m_InLndingParam, false);
	}

	// 接地しているかのフラグセット
	private void SetGroundedFlag()
	{
		m_AnimController.SetAnimationParameter(m_GrandedParam, m_HasGrounded);
	}

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		//接地判定用スフィアキャストの確認用
		Vector3 colliderCenterPos = transform.position + Vector3.up * m_FootColliderRadius;
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(colliderCenterPos, m_FootColliderRadius);
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(colliderCenterPos, colliderCenterPos + Vector3.down * m_CastDistance);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(colliderCenterPos + Vector3.down * m_CastDistance, m_FootColliderRadius);

		Vector3 castOffset = Vector3.up * m_HalfCastDistance;
		Vector3 p1 = transform.position + castOffset + (Vector3.up * m_FootColliderRadius);      // 足元
		
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + -transform.up * (m_CastDistance + Physics.gravity.magnitude * Time.deltaTime * 0.5f));

	}
#endif

}
