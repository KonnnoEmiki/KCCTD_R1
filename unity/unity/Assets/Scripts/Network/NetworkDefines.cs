using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Network関連の定数,enumなど
public static class NetworkDefines
{
    public enum ConnectionState
	{
		Connect,	// 接続中
		Disconnect	// 切断中
	}

	public enum RoomErrorType
	{
		None,				// エラーなし
		Crowded_Room,		// 部屋が満員
		Not_Exist_Room,		// 部屋が存在しない
		Failed_Create_Room,	// 部屋の作成に失敗
		Already_Join_Room,	// すでに入室済み
		Unknown_Error,		// その他のエラー
	}
	
	// 最大ルーム接続人数
	public static readonly uint MaxNumRoomPlayers = 10;

}
