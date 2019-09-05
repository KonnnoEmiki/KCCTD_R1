using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 偏差打ち用台座回転クラス
[RequireComponent(typeof(BallLauncher))]
public class DeviationalShotRotator : MonoBehaviour
{
	private Player m_TargetPlayer = null; // ターゲット
	private BallLauncher m_BallLauncher = null; // 台座

	[SerializeField]
	private Vector3 m_TargetOffset;
	
	[SerializeField]
	private float m_RotationSpeed = 2;

	// 一番近いプレイヤーの検索インターバル(毎フレーム検索する必要無いので)
	[SerializeField]
	private float m_SerchPlayerInterval = 0.5f;

	// ランダムなオフセット座標(1 -> Vector3(-1,-1,-1) ~ Vector3(1,1,1))
	[SerializeField]
	private float m_RandomOffset = 1;

	private Vector3 m_RandomOffsetPos;
	private bool m_IsFollowPlayer = false;
	
    void Start()
    {
		m_BallLauncher = GetComponent<BallLauncher>();
		StartCoroutine(SerchNearestPlayer());
		m_RandomOffsetPos = MathUtil.RandomRange(-m_RandomOffset, m_RandomOffset);
    }

	void Update()
	{
		if (m_TargetPlayer == null) return;

		if (m_IsFollowPlayer == false) return;

		transform.rotation = CalcLookAtPlayerRotation();
	}

	// ランダムなオフセット座標算出(オフセット0だとかなり当たるので)
	public void ReCalcRandomOffset()
	{
		m_RandomOffsetPos = MathUtil.RandomRange(-m_RandomOffset, m_RandomOffset);
	}

	// プレイヤー追尾
	public void FollowPlayer()
	{
		m_IsFollowPlayer = true;
	}

	// プレイヤー追尾解除
	public void UnFollowPlayer()
	{
		m_IsFollowPlayer = false;
	}

	// プレイヤーにボールが当たる射出方向を計算し砲台の回転値を算出
	Quaternion CalcLookAtPlayerRotation()
	{
		// プレイヤーの位置 + オフセット
		var playerPos = m_TargetPlayer.transform.position + m_TargetOffset;
		
		// プレイヤーの速度取得
		var playerVelocityXZ = m_TargetPlayer.VelocityXZ;
		var playerVelocityY = m_TargetPlayer.VelocityY;

		//　ボールの到達時間算出
		var arrivalTime = Vector3.Distance(playerPos, transform.position) / m_BallLauncher.BallInitialSpeed * m_BallLauncher.BallMass;
		//　プレイヤーの横の移動予測値算出
		var predictionPosXZ = m_TargetPlayer.transform.forward * playerVelocityXZ.magnitude * arrivalTime;
		//　プレイヤーの縦の移動予測値算出
		var predictionPosY = m_TargetPlayer.transform.up * playerVelocityY * arrivalTime;

		//　プレイヤーがそのまま移動すると仮定した際に弾がプレイヤーに衝突する位置を算出
		var predictionPlayerPoint = playerPos + predictionPosXZ + predictionPosY;

		//　砲台とプレイヤーの予測移動先の距離算出
		var adjacent = Vector3.Distance(transform.position, predictionPlayerPoint);
		//　落下距離を計算
		var fallingDistance = 0.5f * Physics.gravity.y * (arrivalTime * arrivalTime);

		//　砲台の回転値を算出
		var targetPos = predictionPlayerPoint - Vector3.up * fallingDistance + m_RandomOffsetPos;
		Vector3 targetDir = targetPos - transform.position;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, m_RotationSpeed * Time.deltaTime, 0f);

		return Quaternion.LookRotation(newDir);
	}

	// 一番近いプレイヤー検索
	IEnumerator SerchNearestPlayer()
	{
		while (true)
		{
			if (GameManager.IsGameSet) yield break;
			if (PlayerManager.HasInstance == false) yield break;

			m_TargetPlayer = PlayerManager.Instance.GetNearestPlayer(transform.position);
			yield return new WaitForSeconds(m_SerchPlayerInterval);
		}
	}

}
