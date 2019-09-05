using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボール発射台クラス
[RequireComponent(typeof(DeviationalShotRotator),typeof(RandomSphereHorizonMove))]
public class BallLauncher : MonobitEngine.MonoBehaviour
{
	[SerializeField, AssetPath]
	private string m_BallPrefab;
	
	[SerializeField]
	private float m_BallInitialSpeed = 100;
	public float BallInitialSpeed { get { return m_BallInitialSpeed; } }

	[SerializeField]
	private float m_SpawnInterval = 3;

	[SerializeField]
	private float m_ShotInterval = 1;

	[SerializeField,Range(1,100)]
	private int m_RandomMoveProbability = 20; // ランダムに移動する確率

	[SerializeField]
	private float m_MinMoveTime = 5;
	[SerializeField]
	private float m_MaxMoveTime = 8;

	private float m_BallMass;
	public float BallMass { get { return m_BallMass; } }

	private MeshCollider m_AreaCollider = null;
	private float m_AreaRadius;

	private Ball m_ShotBall = null;
	private TargetFollow m_Follow = null; // ボールを自身にピッタリ付いてくるようにするコンポーネント

	private DeviationalShotRotator m_DeviationalShotRotator = null;
	private RandomSphereHorizonMove m_CircleHorizonMove = null;

	private float m_SpawnTimer = 0;
	private float m_ShotTimer = 0;

	void Start()
    {
		if (monobitView.isMine == false) return;
	
		m_DeviationalShotRotator = GetComponent<DeviationalShotRotator>();
		m_CircleHorizonMove = GetComponent<RandomSphereHorizonMove>();
		m_AreaCollider = GameManager.Instance.AreaCollider;
		m_AreaRadius = m_AreaCollider.bounds.size.x * 0.5f * 0.9f;

		// ボールプレハブから剛体質量取得
		var ballPrefab = CachedResources.Load(m_BallPrefab) as GameObject;
		m_BallMass = ballPrefab.GetComponent<Rigidbody>().mass;
		

		RandomMove();
	}

    void Update()
    {
		if (GameManager.IsGameSet) return;
		if (monobitView.isMine == false) return;

		if (PlayerManager.Instance.NumSurvivingPlayers <= 1)
			return;

		// ボールを打ち出してから一定時間立ったらスポーンさせる
		if (m_ShotBall == null)
		{
			m_SpawnTimer += Time.deltaTime;
			if(m_SpawnTimer > m_SpawnInterval)
			{
				SpawnBall();
				m_DeviationalShotRotator.ReCalcRandomOffset();	// オフセット座標再計算
				m_DeviationalShotRotator.FollowPlayer();		// プレイヤー追尾
				m_SpawnTimer = 0;
			}
			return;
		}
		else
			m_ShotTimer += Time.deltaTime;

		// ボール生成後一定時間立ったら発射
		if (m_ShotTimer > m_ShotInterval)
		{
			ShotBall(); // 発射
			m_DeviationalShotRotator.UnFollowPlayer(); // プレイヤー追尾解除

			var rand = Random.Range(0, 101); // ランダムで移動
			if(rand <= m_RandomMoveProbability)
				RandomMove();
			m_ShotTimer = 0;
		}
	}

	// ボール生成
	private void SpawnBall()
	{
		var ball = MonobitEngine.MonobitNetwork.Instantiate(m_BallPrefab, transform.position, Quaternion.identity, 0);
		m_ShotBall = ball.GetComponent<Ball>();
		m_Follow = ball.AddComponent<TargetFollow>();
		m_Follow.m_Target = transform;
	}

	// ボール発射
	[MunRPC]
	private void ShotBall()
	{
		m_ShotBall.Shot(m_BallInitialSpeed);
		m_ShotBall.StartDestroyTimer();
		Destroy(m_Follow);
		m_ShotBall = null;
	}

	// ランダムに移動
	private void RandomMove()
	{
		if (m_CircleHorizonMove.IsMove || m_AreaCollider == null) return;

		var randMoveTime = Random.Range(m_MinMoveTime, m_MaxMoveTime);
		m_CircleHorizonMove.Move(m_AreaCollider.transform.position, m_AreaRadius, randMoveTime, true);
	}

}

