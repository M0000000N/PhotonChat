using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField _nickname; // ������ʹ�
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
        _joinButtonText.text = "�����ϱ�";
    }

    private void Awake()
    {
        _joinButtonText = _joinButton.GetComponentInChildren<TextMeshProUGUI>();
        // ��ư �̺�Ʈ �޼ҵ� ����
        _joinButton.onClick.AddListener(OnClickJoinButton); // �����긮���� �� �ʿ䰡 ����. ���� ��� ���� �Ŷ�?
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

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 20
    };

    private void OnClickJoinButton()
    {
        if(_nickname.text.Length ==0)
        {
            Debug.Log("�г����� �Է��ϼ���");
            return;
        }

        Data data = FindObjectOfType<Data>();
        data.Nickname = _nickname.text;

        Debug.Log($"{ data.Nickname}");

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
