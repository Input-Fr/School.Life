using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;
public class testRelay : MonoBehaviour
{
    // Start is called before the first frame update
    public static testRelay Instance { get; private set; }
    [SerializeField]private TextMeshProUGUI textOfServ;
    [SerializeField]private GameObject _button;
    const string keyForRelay = "azertyuiop";

    [SerializeField]private GameObject Panel;
    [SerializeField]private GameObject ButtonCreate;
    [SerializeField]private GameObject ButtonJoin;
    [SerializeField]private GameObject input;
    [SerializeField]private GameObject cam;

    private Lobby _connectedLobby;
    private QueryResponse _lobbies;
    private UnityTransport _transport;
    private string _playerId;
    private Lobby hostLobby;
    private float heartBeatTimer;
    const string keyForJoinCodeRelay = "";

    private void Awake() {

        	Instance = this;
            _transport = FindObjectsOfType<UnityTransport>()[0];

    }
        

    async void Start()
    {
        // waiting for unity services to respond (await is use to not freeze the whole game while unity services is responding)
        
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        
       
        var lobbyIds = await LobbyService.Instance.GetJoinedLobbiesAsync();
        Debug.Log(lobbyIds.Count);
            
        if (lobbyIds.Count > 0)
        {
            for (int i = 0; i < lobbyIds.Count; i++)
            {
                var lobby = lobbyIds[i];
                //Lobby currentLobby = await LobbyService.Instance.ReconnectToLobbyAsync(lobby);
                string playerId = AuthenticationService.Instance.PlayerId;
                await LobbyService.Instance.RemovePlayerAsync(lobby,playerId);
            
                //Lobby currentLobby = await LobbyService.Instance.GetLobbyAsync(lobby);
                //JoinRelay(currentLobby.Data[keyForRelay].Value);    
            }
                        
        }
            
        
    }

 
    public async void CreateLobby(string code)
    {
        try{
            string lobbyName = "lobbyName" + UnityEngine.Random.Range(0,10000);
            int maxPlayer = 40;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
                IsPrivate = false,
                Data = new Dictionary<string, DataObject>{
                    {keyForRelay,new DataObject(DataObject.VisibilityOptions.Member,code)} 
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
    
    public async void CreateRelay()
    {
        // Create the relay
        try{
        Allocation alloc = await RelayService.Instance.CreateAllocationAsync(40);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
        textOfServ.text = joinCode;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
            alloc.RelayServer.IpV4,
            (ushort)alloc.RelayServer.Port,
            alloc.AllocationIdBytes,
            alloc.Key,
            alloc.ConnectionData
        );

        NetworkManager.Singleton.StartHost();
        //
        CreateLobby(joinCode);


        }catch (RelayServiceException error) {
            Debug.Log(error);
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

    public async void JoinRelay(string joinCode)
    {
        try {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                textOfServ.text = joinCode;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
            cam.SetActive(false);
            Panel.SetActive(false);
            ButtonCreate.SetActive(false);
            ButtonJoin.SetActive(false);
            input.SetActive(false);
        }catch (RelayServiceException error){

            Debug.Log(error);
            
            Panel.SetActive(true);
            ButtonCreate.SetActive(true);
            ButtonJoin.SetActive(true);
            input.SetActive(true);
            cam.SetActive(true);
        }
    }

    public async void ListLobbies()
    {
        try{
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }catch(LobbyServiceException error)
        {
            Debug.Log(error);
        }
    }

    public async void QuickJoinLobby() {
        try{
            Lobby currentLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            // turn off : the panel, the buttons and the input
            Panel.SetActive(false);
            ButtonCreate.SetActive(false);
            ButtonJoin.SetActive(false);
            input.SetActive(false);
            JoinRelay(currentLobby.Data[keyForRelay].Value);

        }catch (LobbyServiceException)
        {
            // turn off : the panel, the buttons and the input
            Panel.SetActive(false);
            ButtonCreate.SetActive(false);
            ButtonJoin.SetActive(false);
            input.SetActive(false);
            CreateRelay();
        }
    }
}
