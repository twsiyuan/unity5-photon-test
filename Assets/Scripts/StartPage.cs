using UnityEngine;

public class StartPage : MonoBehaviour
{
    public CharacterSelector selector;
    public GameObject info_connecting;
    public GameObject info_start;

    void Awake()
    {
        // 初始化控制項
        this.selector.AllowSelect = false;
        this.info_connecting.SetActive(true);
        this.info_start.SetActive(false);
    }

    void OnMasterConnected(object sender, System.EventArgs e)
    {
        // 大廳完成連線
        this.selector.AllowSelect = true;
        this.info_connecting.SetActive(false);
        this.info_start.SetActive(true);
    }

    void Start()
    {
        PhotonManager.Singleton.MasterConnected += this.OnMasterConnected;
    }

    void Update ()
    {
	    if (this.selector.AllowSelect && Input.GetKeyDown(KeyCode.X))
        {
            // 加入房間
            PhotonManager.Singleton.JoinGameRoom(selector.Current);

            // 等待連線..
            this.selector.AllowSelect = false;
            this.info_connecting.SetActive(true);
            this.info_start.SetActive(false);
        }
	}
}
