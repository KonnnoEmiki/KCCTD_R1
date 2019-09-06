// Observableから通知を受け取りたいクラスにこのインターフェースを継承し、
// ObservableにAddObserverで自身を通知対象に登録する
// (通知対象から外してもらう場合はRemoveObserver)
public interface IObserver<T>
{
	void OnNotify(Observable<T> observer,T notifyObject);
}