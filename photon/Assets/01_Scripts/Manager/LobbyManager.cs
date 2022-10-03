using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField _nickname;
    [SerializeField] TextMeshProUGUI logText;
    [SerializeField] Button _joinButton;
    private TextMeshProUGUI _joinButtonText;

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 20
    };

    private void deactivateJoinButton(string message)
    {
        _joinButton.interactable = false;
        logText.text = message;
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
        _joinButton.onClick.AddListener(OnClickJoinButton);

        // 마스터 서버 연결시도
        PhotonNetwork.ConnectUsingSettings();

        deactivateJoinButton("접속중");
    }

    public override void OnConnectedToMaster()
    {
        logText.text = "마스터에 서버 접속됨";

        // 버튼 활성화
        activeJoinButton();
    }

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()에 연결이 끊겼을 때 호출되는 콜백함수다.
    {
        // 버튼 비활성화
        deactivateJoinButton("연결이 끊김. 재접속 시도중..");

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnClickJoinButton()
    {
        if(_nickname.text.Length ==0)
        {
            logText.text = "닉네임을 입력하세요";
            return;
        }

        Data data = FindObjectOfType<Data>();
        data.Nickname = _nickname.text;

        // Debug.Log($"입력된 닉네임 : {data.Nickname}");

        // 아무 방이나 입장한다.
        PhotonNetwork.JoinOrCreateRoom("Metavers", RandomRoomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        logText.text = "방에 입장함";

        // 세션이 성립되었다고 해서 실제로 이동하는 것이 아니다. -> 레벨 이동 함수 사용
        PhotonNetwork.LoadLevel("Main");
    }
}
