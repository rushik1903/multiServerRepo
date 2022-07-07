using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using TMPro;
using System;

public enum ServerToClientId : ushort
{
    sync=1,
    playerSpawned,
    playerMovement,
}

public enum ClientToServerId : ushort
{
    name=1,
    input,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    [SerializeField] private TextMeshProUGUI input;

    public static NetworkManager Singleton{
        get=>_singleton;
        private set{
            if(_singleton==null){
                _singleton=value;
            }
            else if(_singleton!=value){
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    public Server Server {get; private set; }
    public ushort CurrentTick {get; private set;} = 0;

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Awake(){
        Singleton = this;
    }

    public void Start(){
        // port =  Convert.ToUInt16(input.text);
        CurrentTick = 2;
        Application.targetFrameRate = 30;

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }

    private void FixedUpdate() {
        Server.Tick();

        if(CurrentTick%40==0){
            SendSync();
        }

        CurrentTick++;
    }

    private void OnApplicationQuit() {
        Server.Stop();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e){
        if(Player.list.TryGetValue(e.Id,out Player player)){
            Destroy(player.gameObject);
        }
    }

    private void SendSync(){
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.sync);
        message.Add(CurrentTick);
        
        Server.SendToAll(message);
    }
}
