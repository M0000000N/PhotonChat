using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class NicknameTextUI : MonoBehaviourPunCallbacks, IPunObservable
{
    private TextMeshProUGUI _ui;
    int clickCount = 0;
    private string _nickname;

    public int ClickCount
    {
        get { return clickCount; }
        set
        { 
            clickCount = value;
            Nickname = $"{_nickname} : {clickCount}";
        }
    }
    public string Nickname
    {
        get { return _ui.text; }
        set { _ui.text = value; }
    }
    public void OnPhotonSerializedView(PhotonStream stream, PhotonMessageInfo info)
    {

        // 직렬화 -> 서버에게 데이터를 보내는 것
        if (stream.IsWriting)
        {
            stream.SendNext(ClickCount);
            //stream.SendNext(Nickname); 닉네임을 안넣은 이유 : 네트워크 비용을 줄이기 위해
        }
        else // 역직렬화 -> 서버로부터 데이터를 받은 것
        {
            ClickCount = (int)stream.ReceiveNext();
            //Nickname = (string)stream.ReceiveNext();
        }
    }


    private void Awake()
    {
        _ui = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        Data data = FindObjectOfType<Data>();
        Nickname = data.Nickname;
        Nickname = Nickname;
        photonView.RPC("SetNickname", RpcTarget.All, Nickname);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ++ClickCount;
        }
    }

    [PunRPC]
    public void SetNickname(string nickname)
    {
        Nickname = nickname;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("SetNickname", newPlayer, Nickname);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
