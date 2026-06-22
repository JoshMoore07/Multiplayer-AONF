using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class multiplayer_manager : MonoBehaviour
{
    [SerializeField] private int maxPlayers = 1;
    private Lobby currentLobby;

    public static multiplayer_manager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public async Task<string> JoinLobbyAndRelay(string lobbyCode)
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            string relayCode = joinedLobby.Data["RelayCode"].Value;
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);
            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "wss");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            return lobbyCode;
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e.Message);
        }
        return null;
    }

    public async Task<string> CreateLobbyAndRelay(string lobbyName)
    {
        try
        {
            //Set up relay server
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            //Conects player to relay server as a host
            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "wss");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();


            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,

            };

            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "RelayCode", new DataObject(
                    visibility: DataObject.VisibilityOptions.Public,
                    value: joinCode
                    )
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            string lobbyCode = lobby.LobbyCode;
            Debug.Log($"Created lobby with code {lobbyCode}");
            return lobbyCode;

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e.Message);
        }
        return null;
    }

    public async void LeaveLobbyAndRelay()
    {
        try
        {
            if(currentLobby != null)
            {
                if (currentLobby.HostId == AuthenticationService.Instance.PlayerId)
                {
                    await LobbyService.Instance.DeleteLobbyAsync(currentLobby.Id);
                }
                else
                {
                    await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, AuthenticationService.Instance.PlayerId);
                }
                currentLobby = null;
            }

            if(NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.Shutdown();
            }

            
            
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e.Message);
        }
    }


}
