using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ネットワークイベントが発生した際にログを出力するクラス
public class NetworkLogger : MonoBehaviour,IObserver<NetworkEvent>
{
	void Start()
    {
		NetworkManager.Instance.AddNetworkEventObserver(this);
    }
	
	// ネットワークイベント通知受け取り
	public void OnNotify(Observable<NetworkEvent> observer, NetworkEvent notifyObject)
	{
		Debug.Log("OnNotify : " + notifyObject.GetName());
	}
	
}
