using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class NicknameTextUI : MonoBehaviourPunCallbacks, IPunObservable
{
    private TextMeshProUGUI _ui;
    private int _clickCount = 0;
    private string _nickname;

    public int ClickCount
    {
        get { return _clickCount; }
        set
        {
            _clickCount = value;
            Nickname = $"{_nickname} : {_clickCount}";
        }
    }

    public string Nickname
    {
        get { return _ui.text; }
        set { _ui.text = value; }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 직렬화 -> 서버에게 데이터를 보내는 것
        if (stream.IsWriting)
        {
            stream.SendNext(ClickCount);
            //stream.SendNext(Nickname); 닉네임을 안넣은 이유 : 네트워크 비용을 줄이기 위해
            // 똑같은 화면을 그리려면 어떤 데이터를 주고받아야 하는지 고민해야한다.
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
        if (photonView.IsMine)
        {
            _nickname = data.Nickname;
            ClickCount = 0;
            photonView.RPC("SetNickname", RpcTarget.Others, Nickname);
            
            //Nickname = _nickname;
            //photonView.RPC("SetNickname", RpcTarget.All, Nickname);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ++ClickCount;
        }
    }

    [PunRPC]
    public void SetNickname(string nickname)
    {
        Nickname = nickname;
    }

    // 방에 입장했을 때
    public override void OnJoinedRoom()
    {
        // 방에 있는 플레이어 모두에게 내 이름을 전달한다.
        photonView.RPC("SetNickname", RpcTarget.All, Nickname);
    }

    // 새로운 플레이어가 입장했을 때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로운 플레이어에게 내 이름을 전달한다.
        photonView.RPC("SetNickname", newPlayer, Nickname);
    }
}
