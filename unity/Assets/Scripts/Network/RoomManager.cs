using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ルーム管理クラス
public class RoomManager : MonobitEngine.MonoBehaviour
{
	// 最後に発生したエラーのタイプ
	private NetworkDefines.RoomErrorType m_LastErrorType;
	public NetworkDefines.RoomErrorType LastErrorType
	{
		get { return m_LastErrorType; }
	}

	// ルームのホストか
	public static bool IsHost
	{
		get { return MonobitEngine.MonobitNetwork.isHost; }
	}

	// プレイヤー数
	public static int NumPlayers
	{
		get
		{
			if ( MonobitEngine.MonobitNetwork.room != null)
				return MonobitEngine.MonobitNetwork.room.playerCount;
			return 0;
		}
	}

	// ルーム作成
	public bool CreateRoom(string roomName)
	{
		bool result = true;
		m_LastErrorType = NetworkDefines.RoomErrorType.None;
		do
		{
			if (MonobitEngine.MonobitNetwork.inRoom)
			{
				result = false;
				m_LastErrorType = NetworkDefines.RoomErrorType.Already_Join_Room;
				break;
			}

			MonobitEngine.RoomSettings roomSettings = new MonobitEngine.RoomSettings
			{
				isVisible = true,
				isOpen = true,
				maxPlayers = NetworkDefines.MaxNumRoomPlayers,
				roomParameters = null,
				lobbyParameters = new string[] {}
			};
			if (MonobitEngine.MonobitNetwork.CreateRoom(roomName, roomSettings, null) == false)
			{
				result = false;
				m_LastErrorType = NetworkDefines.RoomErrorType.Failed_Create_Room;
				break;
			}

		}
		while (false);

		LogError();

		return result;
	}

	// ルームへ参加
	public bool JoinRoom(string roomName)
	{
		bool result = true;

		result = CheckJoinTheRoom(roomName);
		if (result)
			MonobitEngine.MonobitNetwork.JoinRoom(roomName);

		LogError();
		
		return result;
	}

	// ルームへ参加出来るか
	public bool CheckJoinTheRoom(string roomName)
	{
		m_LastErrorType = NetworkDefines.RoomErrorType.None;
		bool result = true;
		bool isExist = false;
		foreach(var roomData in MonobitEngine.MonobitNetwork.GetRoomData())
		{
			if (roomData.name != roomName)
				continue;
			isExist = true;
			if (roomData.playerCount >= NetworkDefines.MaxNumRoomPlayers)
			{
				m_LastErrorType = NetworkDefines.RoomErrorType.Crowded_Room;
				result = false;
			}
			break;
		}

		if(isExist == false)
			m_LastErrorType = NetworkDefines.RoomErrorType.Not_Exist_Room;

		LogError();

		return result;
	}
	
	// ルームから退室
	public void LeaveRoom()
	{
		if (MonobitEngine.MonobitNetwork.inRoom == false)
			return;
		MonobitEngine.MonobitNetwork.LeaveRoom();

    }

	// エラーログ出力 無ければ出力無し
	public void LogError()
	{
		if (m_LastErrorType == NetworkDefines.RoomErrorType.None)
			return;

		string errorStr = m_LastErrorType.ToString();
		string replacedErrorStr = errorStr.Replace('_', ' '); // アンダーバーをスペースへ

		Debug.LogWarning("Room Error :" + m_LastErrorType.ToString().Replace('_', ' '));
	}
}
