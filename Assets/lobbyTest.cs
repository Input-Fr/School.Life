using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class lobbyTest : MonoBehaviour
{
    const string keyForJoinCodeRelay = "";

    async void Start()
    {
        // waiting for unity services to respond (await is use to not freeze the whole game while unity services is responding)
        
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
    }
    public async void CreateLobby()
    {
        try{
            string lobbyName = "new lobby";
        int maxPlayers = 4;
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = false;
                    
        options.Data = new Dictionary<string, DataObject>()
        {
            {
                keyForJoinCodeRelay, new DataObject(
                    visibility: DataObject.VisibilityOptions.Public, // Visible publicly.
                    value: "FGHHYG")
            },
        };


        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        }
        catch (LobbyServiceException error)
        {
            Debug.Log(error);
        }
        
    }

    public async void QuickJoinLobby()
    {
        try
        {
     		// Quick-join a random lobby with a maximum capacity of 1 or more players.
    		 QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

            options.Filter = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.LT,
                    value: "40")
            };

            var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    
}
