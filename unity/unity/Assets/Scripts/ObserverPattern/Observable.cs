using System.Collections.Generic;

// 複数のオブジェクト(Observer)に通知を送りたいオブジェクトは
// このObservableを、通知を受け取りたいオブジェクトはIObserverを継承し使う
public class Observable<T>
{
	// 通知対象
	protected List<IObserver<T>> m_Observers= new List<IObserver<T>>();
	
	// 通知対象へ追加
	public void AddObserver(IObserver<T> observer)
	{
		m_Observers.Add(observer);
	}

	// 通知対象から除外
	public void RemoveObserver(IObserver<T> observer)
	{
		m_Observers.Remove(observer);
	}

	// 削除された等でnullになっているObserverを除外
	public void RemoveMissingObservers()
	{
		for (var i = m_Observers.Count - 1; i > -1; i--)
		{
			if (m_Observers[i].Equals(null))
				m_Observers.RemoveAt(i);
		}
	}

	// 通知送信
	public virtual void NotifyObservers(T notifyObject)
	{
		foreach (var observer in m_Observers)
			observer.OnNotify(this,notifyObject);
	}
}
