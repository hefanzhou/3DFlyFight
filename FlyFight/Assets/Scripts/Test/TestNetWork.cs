using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Test
{

	public class PlayerInfo
	{
		public NetworkPlayer networkPlayer;
		public string name;
		public PlayerInfo(NetworkPlayer _networkPlayer, string _name = "")
		{
			networkPlayer = _networkPlayer;
			name = _name;
		}
	}
	public class TestNetWork : MonoBehaviour
	{

		private bool isSever = false;
		private Dictionary<NetworkPlayer, PlayerInfo> allPlayer = new Dictionary<NetworkPlayer, PlayerInfo>();
		private string tipInf = "";
		private string myName = "input your name";
		void Awake()
		{

		}

		void OnGUI()
		{

			if (GUILayout.Button("Create Server"))
			{
				InitServer();
				NetworkConnectionError err = Network.InitializeServer(32, 53214, false);
				if (err == NetworkConnectionError.NoError)
				{
					MasterServer.RegisterHost("fps", "flyFight", "this is comment");
					isSever = true;
				}
				else
				{
					tipInf = "initialize server fail err:" + err.ToString();
				}
			}

			if (GUILayout.Button("Connect to Server"))
			{
				InitClient();
				isSever = false;
			}

			if (isSever) ServerGUI();
			else ClientGUI();
		}

		void InitServer()
		{
			MasterServer.ipAddress = "127.0.0.1";
			MasterServer.port = 23466;

		}

		void ServerGUI()
		{

			if (GUILayout.Button("beginGame"))
			{
				networkView.RPC("InitiScence", RPCMode.AllBuffered);
			}
			if (GUILayout.Button("RPC"))
			{
				GameObject.Find("/Go1").GetComponent<TestPRC>().networkView.RPC("PrintSome", RPCMode.AllBuffered);
			}
			if (GUILayout.Button("clearRPC"))
			{

			}
			tipInf = "initialize server success, connect player:";
			foreach (var player in allPlayer) tipInf += "\n" + player.Value.name;
			GUILayout.TextArea(tipInf);
		}

		void InitClient()
		{
			MasterServer.ipAddress = "127.0.0.1";
			MasterServer.port = 23466;
			MasterServer.RequestHostList("fps");
		}

		void ClientGUI()
		{
			HostData[] data = MasterServer.PollHostList();
			foreach (HostData hostData in data)
			{
				GUILayout.BeginHorizontal();
				string name = hostData.gameName + " " + hostData.connectedPlayers + " " + hostData.playerLimit;
				GUILayout.Label(name);
				GUILayout.Space(5);
				string hostInfo = "";
				foreach (string hostIp in hostData.ip)
				{
					hostInfo += hostIp + ":" + hostData.port + "\n";
				}

				GUILayout.Label(hostInfo);
				GUILayout.Space(5);
				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Connect"))
				{
					var netErr = Network.Connect(hostData);
					Debug.Log(netErr);
				}

				myName = GUILayout.TextArea(myName);

				GUILayout.EndHorizontal();
			}
		}

		void OnPlayerConnected(NetworkPlayer player)
		{
			Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
			PlayerInfo playerInfo = new PlayerInfo(player);
			allPlayer.Add(player, playerInfo);
		}

		void OnConnectedToServer()
		{
			Debug.Log("Connected to server");
			networkView.RPC("SendMyInfo", RPCMode.Server, myName);
		}
		void OnFailedToConnect(NetworkConnectionError error)
		{
			Debug.Log("Could not connect to server: " + error);
		}
		[RPC]
		void InitiScence()
		{
			GameObject obj = Resources.Load("Cube") as GameObject;
			GameObject go = Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;

		}

		void OnPlayerDisconnected(NetworkPlayer player)
		{
			Debug.Log("Clean up after player " + player);
			Network.RemoveRPCs(player);
			Network.DestroyPlayerObjects(player);
			allPlayer.Remove(player);
		}

		[RPC]
		void SendMyInfo(string playerInfo, NetworkMessageInfo info)
		{
			allPlayer[info.sender].name = playerInfo;
		}

	}

}
