using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ネットワーク関連イベント用のObservableクラス
public class NetworkEventObservable : Observable<NetworkEvent>
{
	public override void NotifyObservers(NetworkEvent notifyObject)
	{
		foreach (var observer in m_Observers)
		{
			observer.OnNotify(this, notifyObject);
			if (notifyObject.m_Handled) break;
		}
	}
}
