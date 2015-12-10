using UnityEngine;
using System.Collections;

public class TestNetWork : MonoBehaviour {

	public bool isSever = true; 
	void Awake()
	{
		if (isSever) InitServer();
		else InitClient();
	}

	void OnGUI() {
		bool tempIsServer = isSever;
		isSever = GUILayout.Toggle(isSever, "is server");
		if (tempIsServer != isSever)
		{
			if (isSever) InitServer();
			else InitClient();
			Debug.Log("switch");
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
		if (GUI.Button(new Rect(0, 20, 150, 20), "StartLocalServer"))
		{
			Network.InitializeServer(32, 25003, !Network.HavePublicAddress());
			MasterServer.RegisterHost("fps", "flyFight", "this is comment");
		}  
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
  
            if(GUILayout.Button("Connect"))  
            {  
                Network.Connect(hostData);  
            }  
  
            GUILayout.EndHorizontal();  
        }  
    }  
}
