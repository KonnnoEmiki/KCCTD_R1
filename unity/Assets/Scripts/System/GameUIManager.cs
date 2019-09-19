using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

// InGameのUI管理用クラス
public class GameUIManager : MonoBehaviour,IObserver<PlayerEvent>,IObserver<NetworkEvent>
{
	[SerializeField]
	private InGameUITexts m_InGameUITexts;

	[SerializeField]
	private Text m_SurvivingPlayersText = null;

	[SerializeField]
	private Text m_GameSetText = null;

	[SerializeField]
	private Text m_WinText = null;

	[SerializeField]
	private Text m_LoseText = null;

	[SerializeField]
	private Text m_NetworkErrorText = null;

    [SerializeField]
    private Text m_GameStartText = null;

    [SerializeField]
    private Text m_LifeText = null;

    [SerializeField]
    private Text m_DamageEffect = null;

    private int Life=-1;

    private static bool starttext = true;

    private static bool damageflag = true;

    void Awake()
	{
		NetworkManager.Instance.AddNetworkEventObserver(this);
	}

	void Start()
    {
		if (m_InGameUITexts == null) return;
		UpdateSurvivingPlayersTextUI();
        starttext = true;
        damageflag = true;
        PlayerManager.Instance.AddObserver(this);
	}

    static async void DelayStart()
    {
            await Task.Delay(3000);
            starttext = false;
    }

    static async void DelayDamage()
    {
        await Task.Delay(30);
        damageflag = false;
    }

    private void Update()
    {
        if (NetworkGUI.gs == true)
        {
            if (starttext == true)
            {
                m_GameStartText.gameObject.SetActive(true);
                m_GameStartText.text = m_InGameUITexts.m_GameStartText;
                DelayStart();
            }
            if (Life != Player.LifeCount)
            {
                m_LifeText.gameObject.SetActive(true);
                Life = Player.LifeCount;
                m_LifeText.text = m_InGameUITexts.m_LifeText + Player.LifeCount;
                if (Life != Player.Stamina)
                {
                    m_DamageEffect.gameObject.SetActive(true);
                    m_DamageEffect.text = m_InGameUITexts.m_DamageEffect;
                    DelayDamage();
                }
            }
            if (damageflag == false)
                m_DamageEffect.gameObject.SetActive(false);
            damageflag = true;
            if (starttext == false)
                m_GameStartText.gameObject.SetActive(false);
        }
    }

    // プレイヤー関連のイベント受信用
    public void OnNotify(Observable<PlayerEvent> observer, PlayerEvent e)
	{
		EventDispatcher dispatcher = new EventDispatcher(e);
		dispatcher.Dispatch<OnSpawnPlayerEvent>(UpdateSurvivingPlayersTextUI);
		dispatcher.Dispatch<OnDownPlayerEvent>(UpdateSurvivingPlayersTextUI);
	}

	// ネットワーク関連のイベント受信用
	public void OnNotify(Observable<NetworkEvent> observer, NetworkEvent e)
	{
		NetworkEventDispatcher dispatcher = new NetworkEventDispatcher(e);
		dispatcher.Dispatch<OtherPlayerDisconnectedEvent>(OnDisconnectedOtherPlayer);
	}

	// 誰かが切断した
	private bool OnDisconnectedOtherPlayer(OtherPlayerDisconnectedEvent e)
	{
		UpdateSurvivingPlayersTextUI();
		return false;
	}

	// 残りプレイヤー表示更新
	private void UpdateSurvivingPlayersTextUI()
    {
		if (m_SurvivingPlayersText == null) return;
	
		int numPlayers = RoomManager.NumPlayers;
		int survivingPlayers = PlayerManager.Instance.NumSurvivingPlayers;

		m_SurvivingPlayersText.text = m_InGameUITexts.m_SurvivingPlayersTest + survivingPlayers + "/" + numPlayers;
	}

	// ネットワーク関連のエラー表示
	public void OnNetworkError(string errorText)
	{
		if (m_NetworkErrorText == null) return;
		m_NetworkErrorText.gameObject.SetActive(true);
		m_NetworkErrorText.text = m_InGameUITexts.m_NetworkErrorText + errorText;
	}

	// 決着がついた時のUI表示
	public void OnGameSet()
	{
		m_GameSetText.gameObject.SetActive(true);
		m_GameSetText.text = m_InGameUITexts.m_GameSetText;

		// 生き残ったプレイヤーの所有権を持っていれば勝ち判定
		if (PlayerManager.Instance.SurvivingPlayers[0].monobitView.isMine)
		{
			m_WinText.gameObject.SetActive(true);
			m_WinText.text = m_InGameUITexts.m_WinText;
		}
		else
		{
			m_LoseText.gameObject.SetActive(true);
			m_LoseText.text = m_InGameUITexts.m_LoseText;
		}
	}
}
