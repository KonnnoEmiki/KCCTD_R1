using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フェードイン,フェードアウト用クラス
public class Fade : MonoBehaviour
{
	public static readonly float DefalueFadeTime = 0.5f;

	public enum FadeType
	{
		None = -1,
		In,
		Out
	}

	[SerializeField]
	private UnityEngine.UI.Image m_FillImage = null;
	
	private float m_FadeTime = 0.0f;
	private float m_Timer = 0.0f;
	private FadeType m_FadeType;
	private bool m_IsDone = false;
	private bool m_IsPause = false;
	private Color m_OriginalColor = Color.clear;
	private Color m_StartColor = Color.clear;
	private Color m_DestColor = Color.clear;
	private Color m_DeltaColor = Color.clear;

	private System.Action m_OnDone;

	public bool IsDone { get { return m_IsDone; } }
	public bool IsPause { get { return m_IsPause; } }

	public void SetColor(Color color)
	{
		m_FillImage.color = color;

		bool isOpaque = m_FillImage.color.a > 0.01f; // 不透明か(0.01f以下なら透明と見なす)

		// 透明なら非表示に、そうでないなら表示
		m_FillImage.enabled = isOpaque;
		m_FillImage.gameObject.SetActive(isOpaque);
	}

	public Color GetColor()
	{
		if (m_FillImage) return m_FillImage.color;
		return Color.clear;
	}

	public float GetAlpha()
	{
		if (m_FillImage) return m_FillImage.color.a;
		return 0;
	}

	public void Run(Color src, Color dest, float time)
	{
		if (m_IsDone == false && m_DeltaColor == dest)
			return;
		if (m_FillImage.color == dest)
			return;

		m_IsPause = false;
		m_DestColor = dest;

		if (time <= 0.0f)
		{
			SetColor(dest);
			m_IsDone = true;
		}
		else
		{
			m_StartColor = src;
			m_DestColor = dest;
			m_DeltaColor = (m_DestColor - m_StartColor) / time;
			m_Timer = 0.0f;
			m_FadeTime = time;
			m_IsDone = false;
		}
	}

	public void Play(bool forward)
	{
		Color dst = Color.clear;
		Color src = Color.clear;
		if (forward)
		{
			src = m_OriginalColor;
			dst = Color.black;
		}
		else
		{
			src = Color.black;
			dst = m_OriginalColor;
		}

		Run(src, dst, DefalueFadeTime);
	}

	public void Play()
	{
		m_IsPause = false;
	}

	public void Pause()
	{
		m_IsPause = true;
	}

	public void AddFadeDoneEvent(System.Action callback)
	{
		m_OnDone += callback;
	}

	public void RemoveFadeDoneEvent(System.Action callback)
	{
		m_OnDone -= callback;
	}

	public void RemoveAllFadeDoneEvent()
	{
		m_OnDone = null;
	}

	private void Awake()
	{
		if (m_FillImage == null) return;

		m_OriginalColor = m_FillImage.color;
	}

	void Update()
	{
		if (m_IsDone) return;
		if (m_IsPause) return;

		m_Timer += Time.unscaledDeltaTime;
		if (m_Timer >= m_FadeTime)
		{
			SetColor(m_DestColor);
			m_IsDone = true;
			m_OnDone?.Invoke();
			return;
		}
		
		SetColor(m_StartColor + (m_DeltaColor * m_Timer));
	}
	
}