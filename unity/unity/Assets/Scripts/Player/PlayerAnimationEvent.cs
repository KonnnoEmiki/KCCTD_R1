using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimationEventType
{
	None = 0,
	OnJumpAnimationStart,
	OnJump,
	OnLanding,
	OnJumpAnimationEnd,
	OnPlaceAnimationStart,
	OnPlaceAnimationEnd,
}

public abstract class PlayerAnimationEvent : EventBase { }

public class OnJumpAnimationStartEvent : PlayerAnimationEvent
{
	public override int GetEventType()
	{
		return (int)PlayerAnimationEventType.OnJumpAnimationStart;
	}

	public override string GetName()
	{
		return "OnJumpAnimationStartEvent";
	}
}

public class OnJumpEvent : PlayerAnimationEvent
{
	public override int GetEventType()
	{
		return (int)PlayerAnimationEventType.OnJump;
	}

	public override string GetName()
	{
		return "OnJumpEvent";
	}
}

public class OnLandingEvent : PlayerAnimationEvent
{
	public override int GetEventType()
	{
		return (int)PlayerAnimationEventType.OnLanding;
	}

	public override string GetName()
	{
		return "OnLandingEvent";
	}
}

public class OnJumpAnimationEndEvent : PlayerAnimationEvent
{
	public override int GetEventType()
	{
		return (int)PlayerAnimationEventType.OnJumpAnimationEnd;
	}

	public override string GetName()
	{
		return "OnJumpAnimationEndEvent";
	}
}

// その場アニメーション(再生中に移動,回転処理を走らせたくないアニメーション)開始
public class OnPlaceAnimationStartEvent : PlayerAnimationEvent
{
	public override int GetEventType()
	{
		return (int)PlayerAnimationEventType.OnPlaceAnimationStart;
	}

	public override string GetName()
	{
		return "OnPlaceAnimationStart";
	}
}

// その場アニメーション終了
public class OnPlaceAnimationEndEvent : PlayerAnimationEvent
{
	public override int GetEventType()
	{
		return (int)PlayerAnimationEventType.OnPlaceAnimationEnd;
	}

	public override string GetName()
	{
		return "OnPlaceAnimationEnd";
	}
}