﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {
	
	private static string gameType = "FlyFight";
	public  static string gameName = "ComeOn";
	private static string gameComment = "Network Test Run";

	
	private static HostData[] hostData;
	private static bool refreshing = false;
	private static bool singlePlayer = true;

	private static NetworkManager singletonInstance;

	private static string chosenShip = null;

	public  static int maxPlayersAllowed = 16;
	public  static List<NetworkPlayer> playerList = new List<NetworkPlayer>();
	private static int playerID = -1;
	public  static int numPlayers = 1;
	
	public static NetworkManager instance {
		get {
			// If first time accessing instance, then find it...
			if (singletonInstance == null) {
				singletonInstance = FindObjectOfType(typeof (NetworkManager)) as NetworkManager;
			}
			
			// If instance is null, then no GameManager exists in the scene, so create one.
			if (singletonInstance == null) {
				GameObject obj = new GameObject("NetworkManager");
				singletonInstance = obj.AddComponent(typeof (NetworkManager)) as NetworkManager;
				obj.name = "Network Manager";
				//Debug.Log ("Could not find a LevelManager object, so automatically generated one.");
			}
			
			return singletonInstance;
		}
	}

	public void Awake() {
		DontDestroyOnLoad(this);

		MasterServer.ipAddress = "127.0.0.1";
		MasterServer.port = 23466;


	}

	
	// Update is called once per frame
	void Update () 
	{
		if(refreshing)
		{
//			Debug.Log("Refreshing......");
			if(MasterServer.PollHostList().Length > 0)
			{
				refreshing = false;
				Debug.Log ("Number of available games: " + MasterServer.PollHostList().Length);
				hostData = MasterServer.PollHostList();
			}
			
		}
	}

	IEnumerator RefreshHostListInner()
	{
		while(true)
		{
			yield return new WaitForSeconds(2);
			if (MasterServer.PollHostList().Length > 0)
			{
				refreshing = false;
				Debug.Log("Number of available games: " + MasterServer.PollHostList().Length);
				hostData = MasterServer.PollHostList();
				break;
			}
		}
		
	}


	public static void StartServer ()
	{
		Debug.Log ("Starting server.........");
		// !Network.HavePublicAddress()); 不使用publicAdress 否则默认会去链接unity自己的穿透公共服务器
		Network.InitializeServer(maxPlayersAllowed, 52341, false);
		Random.seed = (int)Time.time;
		//改为房间名
		gameName = string.Format("Game {0}", Random.Range(100, 999));
		Debug.Log ("gameName: " + gameName);
		MasterServer.RegisterHost (gameType, gameName, gameComment);

		playerList.Clear();
		playerList.Add(Network.player); //Add host (local player) to list
		playerID = 0;
	}
	
	void OnServerInitialized ()
	{
		Debug.Log ("Server ready");
		//SpawnPlayer();
	}
	
	void OnMasterServerEvent (MasterServerEvent msevent)
	{
		Debug.Log ("Received MSE");
		if (msevent == MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log ("MSE Registration Suceeded");
		}
	}
	
	public  void RefreshHostList ()
	{
		Debug.Log ("Refreshing server list.........");
		MasterServer.RequestHostList (gameType);
		StartCoroutine( RefreshHostListInner());
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		if(LevelManager.IsMainMenu()) {
			UpdateClientPlayerInfo();
		}
	}

	[RPC]
	public void ResetToMain() {
		LevelManager.LoadMainMenu();
	}
	
	void OnConnectedToServer ()
	{
		Debug.Log ("Connected to server");
		//SpawnPlayer();
	}
	
	void OnFailedConnection (NetworkConnectionError error)
	{
		Debug.Log ("Failed to connect to server: " + error.ToString());
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Cleaning up after player: " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		
		if(LevelManager.IsMainMenu()) {
			UpdateClientPlayerInfo();
		}
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("Cleaning up in OnDisconnectedFromServer()...");
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);

		//Need to have a level loaded back to menu here
		if (!LevelManager.IsMainMenu()) {
			Debug.Log ("OnDisconnectedFromServer(), reloading level...");
			LevelManager.LoadLevel("MainMenu");
		}
	}

	public static void JoinServer(int serverIndex) {
		Network.Connect(hostData[serverIndex]);
	}
	
	public static bool IsServerListReady() {
		if(hostData != null) {
			return !refreshing && hostData.Length > 0;
		}
		return false;
	}

	public static void SetSinglePlayer(bool mode) {
		singlePlayer = mode;
	}

	public static bool IsSinglePlayer() {
		return singlePlayer;
	}

	public static HostData[] GetHostData() {
		return hostData;
	}

	public static bool IsPlayerVarsSet() {
		return chosenShip != null;
	}
	
	public static void ServerCleanup() {
		MasterServer.UnregisterHost();		
		Network.Disconnect();
	}
	
	public static int GetPlayerIndex(NetworkPlayer netWorkPlayer)
	{
		for(int i=0; i<playerList.Count; i++)
		{
			Debug.Log("In getplayerindex: given string " + netWorkPlayer);
			Debug.Log("In getplayerindex: currently looking at player " + i + " with ip " + playerList[i].ipAddress);
			if (playerList[i].Equals(netWorkPlayer))
			{
				return i;
			}
		}

		return -1;
	}

	public static NetworkPlayer GetPlayer(NetworkPlayer netWorkPlayer)
	{
		return playerList[GetPlayerIndex(netWorkPlayer)];
	}

	public static List<NetworkPlayer> GetPlayerList() {
		return playerList;
	}

	public static int GetPlayerID() {
		return playerID;
	}

	public void UpdateClientPlayerInfo() {
		playerList.Clear();
		playerList.Add(Network.player);

		playerList.AddRange(Network.connections);
		playerList.TrimExcess();
		
		numPlayers = playerList.Count;

		Debug.Log("Sending out RPPC to update client information");

		GetComponent<NetworkView>().RPC("InitializeClientPlayerInfo", RPCMode.Others);

		for(int i=0; i<playerList.Count; i++) {
			GetComponent<NetworkView>().RPC("NetworkUpdateClientPlayerInfo", RPCMode.Others, playerList[i]);
		}

		GetComponent<NetworkView>().RPC("FinalizeClientPlayerInfo", RPCMode.Others);

		Debug.Log("PlayerList finished constructing. Given player ID: " + playerID);
		Debug.Log("Received RPC to update client information");
		Debug.Log("Number of players: " + playerList.Count);
		foreach (NetworkPlayer player in playerList) {
			Debug.Log("IPAddr : " + player.ipAddress);
		}
		
	}

	[RPC]
	private void InitializeClientPlayerInfo() {
		playerList.Clear();
	}

	[RPC]
	private void NetworkUpdateClientPlayerInfo(NetworkPlayer player) {
		playerList.Add(player);
	}

	[RPC]
	private void FinalizeClientPlayerInfo() {

		Debug.Log("PlayerList size: " + playerList.Count);
		playerID = GetPlayerIndex(Network.player);

		Debug.Log("PlayerList finished constructing. Given player ID: " + playerID);
		Debug.Log("Received RPC to update client information");
		Debug.Log("Number of players: " + playerList.Count);
		foreach (NetworkPlayer player in playerList) {
			Debug.Log("IPAddr : " + player.ipAddress);
		}
		
		numPlayers = playerList.Count;
	
	}
	
	
	
	
	

























	
	
	
	
	
	
	
	
	
	
}