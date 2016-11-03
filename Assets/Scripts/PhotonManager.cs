using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PhotonManager : Photon.PunBehaviour
{
    public static PhotonManager Singleton
    {
        get
        {
            return instance;
        }
    }

    public event System.EventHandler MasterConnected;

    public string CharacterName
    {
        get;
        set;
    }

    public void JoinGameRoom(string characterName)
    {
        this.CharacterName = characterName;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom("Room", options, null);
    }

    static PhotonManager instance;

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("UnityChanPUN_v1.0");
    }

    public override void OnConnectedToMaster()
    {
        // 連上 MasterServer 後,  Button (UI) 就可以顯示或是執行其它後續動作.
        Debug.Log("已連上 Master Server");

        if (this.MasterConnected != null)
        {
            this.MasterConnected(this, System.EventArgs.Empty);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("您已進入遊戲室!!");
        
        // 如果是 Master Client?
        if (PhotonNetwork.isMasterClient)
        {
            
        }

        SceneManager.sceneLoaded += this.OnLevelFinishedLoading;
        PhotonNetwork.LoadLevel("Stage");
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;

        // 若不在 Photon 的房間內，則網路有問題
        if (!PhotonNetwork.inRoom)
            return;
        Debug.Log("我們已進入遊戲場景了,耶~");

        // Self
        PhotonNetwork.Instantiate(this.CharacterName, new Vector3(-8.0f, 4.5f, 0), Quaternion.identity, 0);
    }
}