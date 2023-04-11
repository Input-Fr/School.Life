using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;


public class lobbySys : MonoBehaviour
{

    private Lobby hostLobby;
    private float heartBeatTimer;
    const string keyForRelay = "azertyuiop";

    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby()
    {
        try{
            string lobbyName = "lobbyName";
            int maxPlayer = 40;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
                IsPrivate = false,
                Data = new Dictionary<string, DataObject>{
                    {keyForRelay,new DataObject(DataObject.VisibilityOptions.Member,"Data Shared !!")} 
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,maxPlayer, createLobbyOptions);

            hostLobby = lobby;
            Debug.Log("Created Lobby! Lobby name: " + lobby.Name + " Slots: " + lobby.MaxPlayers);
        }catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void ListLobbies()
    {
        try{
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Number of lobbies : " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results){
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    private async void keepLobbyAlive(){
        if (hostLobby != null){
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer < 0f)
            {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        keepLobbyAlive();
    }

    public async void QuickJoinLobby() {
        try{
            Lobby currentLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            Debug.Log(currentLobby.Data[keyForRelay].Value);
        }catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
