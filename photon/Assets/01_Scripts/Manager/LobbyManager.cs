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
    [SerializeField] Button _joinButton;
    private TextMeshProUGUI _joinButtonText;

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 20
    };

    private void deactivateJoinButton(string message)
    {
        _joinButton.interactable = false;
        _joinButtonText.text = message;
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
        Debug.Log("�����Ϳ� ���� ���ӵ�");

        // ��ư Ȱ��ȭ
        activeJoinButton();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // ��ư ��Ȱ��ȭ
        deactivateJoinButton("������ ����.\n������ �õ�");

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnClickJoinButton()
    {
        if(_nickname.text.Length ==0)
        {
            Debug.Log("�г����� �Է��ϼ���");
            return;
        }

        Data data = FindObjectOfType<Data>();
        data.Nickname = _nickname.text;

        Debug.Log($"�Էµ� �г��� : {data.Nickname}");

        // �ƹ� ���̳� �����Ѵ�.
        PhotonNetwork.JoinOrCreateRoom("Metavers", RandomRoomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 ������");

        // ������ �����Ǿ��ٰ� �ؼ� ������ �̵��ϴ� ���� �ƴϴ�. -> ���� �̵� �Լ� ���
        PhotonNetwork.LoadLevel("Main");
    }
}
