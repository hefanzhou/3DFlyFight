using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerManager {

	public Dictionary<NetworkPlayer, PlayerInfo> allPlayerDic = new Dictionary<NetworkPlayer, PlayerInfo>();
	public List<PlayerInfo> RankList = new List<PlayerInfo>();	//分数排名
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

	public void OutputLog()
	{
		Debug.Log("Received RPC to update client information");
		Debug.Log("Number of players: " + allPlayerDic.Count);
		foreach (var kv in allPlayerDic)
		{
			Debug.Log("name :[" + kv.Value.name + "] ip:[" + kv.Value.networkPlayer.ipAddress + "]");
		}
	}

	public void UpdateClientPlayerInfo()
	{
		allPlayerDic.Clear();
		foreach (var player in Network.connections)
		{
			AddPlayer(new PlayerInfo(player));
		}
		foreach (var kv in allPlayerDic)
		{
			
		}

	}
	public void AddPlayer(PlayerInfo player)
	{
		allPlayerDic.Add(player.networkPlayer, player);
	}

	public void RemovePlayer(NetworkPlayer netPlayer)
	{
		allPlayerDic.Remove(netPlayer);
	}

	public void RefreshRankList()
	{
		List<KeyValuePair<NetworkPlayer, PlayerInfo>> myList = new List<KeyValuePair<NetworkPlayer, PlayerInfo>>(allPlayerDic);
		myList.Sort(delegate(KeyValuePair<NetworkPlayer, PlayerInfo> s1, KeyValuePair<NetworkPlayer, PlayerInfo> s2)
		{
			return s2.Value.CompareTo(s1.Value);
		});
		RankList.Clear();
		foreach (var kv in myList)
		{
			RankList.Add(kv.Value);
		}
		myList.Clear();
	}

	public PlayerInfo GetPlayerInfo(NetworkPlayer netWorkPlayer)
	{
		PlayerInfo value = null;
		allPlayerDic.TryGetValue(netWorkPlayer, out value);
		return value;
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