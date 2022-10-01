using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField _nickname; // 집가고싶다
    [SerializeField] Button _joinButton;
    private TextMeshProUGUI _joinButtonText;

    private void deactivateJoinButton(string message)
    {
        _joinButton.interactable = false;
        _joinButtonText.text = message;
    }

    private void activeJoinButton()
    {
        _joinButton.interactable = true;
        _joinButtonText.text = "입장하기";
    }

    private void Awake()
    {
        _joinButtonText = _joinButton.GetComponentInChildren<TextMeshProUGUI>();
        // 버튼 이벤트 메소드 연결
        _joinButton.onClick.AddListener(OnClickJoinButton); // 리무브리스너 할 필요가 없다. 왜지 계속 들을 거라서?
        // 마스터 서버 연결시도
        PhotonNetwork.ConnectUsingSettings();

        deactivateJoinButton("접속중");

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터에 서버 접속됨");

        // 버튼 활성화
        activeJoinButton();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 버튼 비활성화
        deactivateJoinButton("연결이 끊김.\n재접속 시도");

        PhotonNetwork.ConnectUsingSettings();
    }

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 20
    };

    private void OnClickJoinButton()
    {
        if(_nickname.text.Length ==0)
        {
            Debug.Log("닉네임을 입력하세요");
            return;
        }

        Data data = FindObjectOfType<Data>();
        data.Nickname = _nickname.text;

        Debug.Log($"{ data.Nickname}");

        // 아무 방이나 입장한다.
        PhotonNetwork.JoinOrCreateRoom("Metavers", RandomRoomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장함");

        // 세션이 성립되었다고 해서 실제로 이동하는 것이 아니다. -> 레벨 이동 함수 사용
        PhotonNetwork.LoadLevel("Main");
    }
}
