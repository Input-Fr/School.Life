using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class testLobby : MonoBehaviour
{

    private Lobby hostLobby;
    private float timer;
	[SerializeField]private Button startBtn;
    public event EventHandler<EventArgs> OnGameStarted;

    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Awake() {
        startBtn.onClick.AddListener(() => {
            StartGame();
        });
    }

    private void Update() {
        lobbytimer();
    }

    private async void lobbytimer() {
        if (hostLobby != null)
        {
            timer -= Time.deltaTime;
            if (timer < 0f) {
                float timerMax = 30;
                timer = timerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_START_GAME = "";
    private string playerName = AuthenticationService.Instance.PlayerId;

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
        });
    }
    public async void CreateLobby()
    {
        Unity.Services.Lobbies.Models.Player player = GetPlayer();
        string lobbyName = "mulobby";
        
        CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
            Player = player,
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>{
                {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member,"0")}
            }
        }; 
        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,39, createLobbyOptions);
        hostLobby = lobby;
        
        Debug.Log("Created lobby : " + lobby.Name + " " + lobby.MaxPlayers + " Here is your code : " + lobby.LobbyCode);
        
    }

    public async void ListLobbies() {
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder> {
                    new QueryOrder(false,QueryOrder.FieldOptions.MaxPlayers)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results){
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException  error) {
            Debug.Log(error);    
        }
    }

    public async void JoinLobbyCode(string code){
        try {
            await Lobbies.Instance.JoinLobbyByCodeAsync(code);

            Debug.Log("Joined Lobby with code " + code);
        }catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoinLobby() {
        try {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }
    public bool IsLobbyHost()
    {
        return hostLobby != null && hostLobby.HostId == AuthenticationService.Instance.PlayerId;
    }
    public async void StartGame()
    {
        if (IsLobbyHost()){
            try{
                Debug.Log("Joining Relay !");
                string relayCode = "";//await testRelay.Instance.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
                    Data = new Dictionary<string, DataObject>{
                        {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode)}
                    }
                });
                hostLobby = lobby;
            }catch (LobbyServiceException e){
                Debug.Log(e);
            }
        }
    }
}
