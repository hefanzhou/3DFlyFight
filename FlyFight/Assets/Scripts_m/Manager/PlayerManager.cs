using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerManager {

	public List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
	public PlayerInfo mainPlayer = null;	
	public string roomName = "roomName";						//房间名 对应创建游戏时的gameName 
	public string myName = "myName";							//自己的名字


	private static PlayerManager m_instance = null;
	public static PlayerManager Instance
	{
		get
		{
			m_instance = m_instance == null ? new PlayerManager() : m_instance;
			return m_instance;
		}
	}


	public void UpdateClientPlayerInfo()
	{
		playerInfoList.Clear();
		foreach (var player in Network.connections)
		{
			AddPlayer(new PlayerInfo(player));
		}
		foreach (var kv in playerInfoList)
		{
			
		}

	}
	public void AddPlayer(PlayerInfo player)
	{
		playerInfoList.Add(player);
	}

	public void RemovePlayer(PlayerInfo netPlayer)
	{
		playerInfoList.Remove(netPlayer);
	}


	public PlayerInfo GetPlayerInfo(int id)
	{
		return playerInfoList[id];
	}
}


public class PlayerInfo :IComparer<PlayerInfo>
{
	public NetworkPlayer networkPlayer;
	public string name = "no name";
	public int Score = -1;
	public PlayerInfo(NetworkPlayer _networkPlayer, string _name = "")
	{
		networkPlayer = _networkPlayer;
		name = _name;
	}

	public int CompareTo(PlayerInfo other)
	{
		return this.Score - other.Score;
	}



	public int Compare(PlayerInfo x, PlayerInfo y)
	{
		return x.Score - y.Score;
	}
}