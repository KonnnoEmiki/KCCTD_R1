using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 多重継承できないのでメンバにObservableを持つMonoBehaviourを用意
public class ObservableMonoBehaviour<T> : MonoBehaviour
{
	protected Observable<T> m_Observable = new Observable<T>();

	public void ChengeObservable(Observable<T> observable)
	{
		if (m_Observable.GetType() == observable.GetType())
			return; // 型が同じ = 挙動は同じ
		m_Observable = observable;
	}

	// 通知対象へ追加
	public void AddObserver(IObserver<T> observer)
	{
		m_Observable.AddObserver(observer);
	}

	// 通知対象から除外
	public void RemoveObserver(IObserver<T> observer)
	{
		m_Observable.RemoveObserver(observer);
	}

	// 通知送信
	public void NotifyObservers(T notifyObject)
	{
		m_Observable.NotifyObservers(notifyObject);
	}
}

// Singleton Ver
public class ObservableMonoBehaviour<T,N>
	: MonoBehaviour<T>
	  where T : MonoBehaviour
{
	protected Observable<N> m_Observable = new Observable<N>();

	public void ChengeObservable(Observable<N> observable)
	{
		if (m_Observable.GetType() == observable.GetType())
			return; // 型が同じ = 挙動は同じ
		m_Observable = observable;
	}

	// 通知対象へ追加
	public void AddObserver(IObserver<N> observer)
	{
		m_Observable.AddObserver(observer);
	}

	// 通知対象から除外
	public void RemoveObserver(IObserver<N> observer)
	{
		m_Observable.RemoveObserver(observer);
	}

	// 通知送信
	public void NotifyObservers(N notifyObject)
	{
		m_Observable.NotifyObservers(notifyObject);
	}
}


namespace MonobitEngine
{
	// 多重継承できないのでメンバにObservableを持つMonobitEngine.MonoBehaviourを用意
	public class ObservableMonoBehaviour<T> : MonobitEngine.MonoBehaviour
	{
		protected Observable<T> m_Observable = new Observable<T>();

		public void ChengeObservable(Observable<T> observable)
		{
			if (m_Observable.GetType() == observable.GetType())
				return; // 型が同じ = 挙動は同じ
			m_Observable = observable;
		}

		// 通知対象へ追加
		public void AddObserver(IObserver<T> observer)
		{
			m_Observable.AddObserver(observer);
		}

		// 通知対象から除外
		public void RemoveObserver(IObserver<T> observer)
		{
			m_Observable.RemoveObserver(observer);
		}

		// nullになっているObserverの除外
		public void RemoveMissingObservables()
		{
			m_Observable.RemoveMissingObservers();
		}

		// 通知送信
		public void NotifyObservers(T notifyObject)
		{
			m_Observable.NotifyObservers(notifyObject);
		}
	}

	// Singleton Ver
	public class ObservableSingletonMonoBehaviour<T,N>
		: MonobitEngine.SingletonMonoBehaviour<T>
		  where T : MonobitEngine.MonoBehaviour
	{
		protected Observable<N> m_Observable = new Observable<N>();

		protected override void Awake()
		{
			base.Awake();
			if(m_DontDestoryOnLoad)
				UnityEngine.SceneManagement.SceneManager.sceneUnloaded += SceneUnloaded;
		}

		public void ChengeObservable(Observable<N> observable)
		{
			if (m_Observable.GetType() == observable.GetType())
				return; // 型が同じ = 挙動は同じ
			m_Observable = observable;
		}

		// 通知対象へ追加
		public void AddObserver(IObserver<N> observer)
		{
			m_Observable.AddObserver(observer);
		}

		// 通知対象から除外
		public void RemoveObserver(IObserver<N> observer)
		{
			m_Observable.RemoveObserver(observer);
		}

		// 通知送信
		public void NotifyObservers(N notifyObject)
		{
			m_Observable.NotifyObservers(notifyObject);
		}

		// nullになっているObserverの除外
		public void RemoveMissingObservables()
		{
			m_Observable.RemoveMissingObservers();
		}

		// シーンアンロード時に削除されてnullになったObserverを除外
		protected virtual void SceneUnloaded(Scene scene)
		{
			RemoveMissingObservables();
		}
	}

}