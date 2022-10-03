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
        _joinButtonText.text = "�����ϱ�";
    }

    private void Awake()
    {
        _joinButtonText = _joinButton.GetComponentInChildren<TextMeshProUGUI>();

        // ��ư �̺�Ʈ �޼ҵ� ����
        _joinButton.onClick.AddListener(OnClickJoinButton);

        // ������ ���� ����õ�
        PhotonNetwork.ConnectUsingSettings();

        deactivateJoinButton("������");
    }

    public override void OnConnectedToMaster()
    {
        logText.text = "�����Ϳ� ���� ���ӵ�";

        // ��ư Ȱ��ȭ
        activeJoinButton();
    }

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()�� ������ ������ �� ȣ��Ǵ� �ݹ��Լ���.
    {
        // ��ư ��Ȱ��ȭ
        deactivateJoinButton("������ ����. ������ �õ���..");

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnClickJoinButton()
    {
        if(_nickname.text.Length ==0)
        {
            logText.text = "�г����� �Է��ϼ���";
            return;
        }

        Data data = FindObjectOfType<Data>();
        data.Nickname = _nickname.text;

        // Debug.Log($"�Էµ� �г��� : {data.Nickname}");

        // �ƹ� ���̳� �����Ѵ�.
        PhotonNetwork.JoinOrCreateRoom("Metavers", RandomRoomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        logText.text = "�濡 ������";

        // ������ �����Ǿ��ٰ� �ؼ� ������ �̵��ϴ� ���� �ƴϴ�. -> ���� �̵� �Լ� ���
        PhotonNetwork.LoadLevel("Main");
    }
}
