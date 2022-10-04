using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0.1";

    [Header("Ÿ��Ʋ ȭ��")]
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TextMeshProUGUI logText;

    [SerializeField] Button randomJoinButton;
    [SerializeField] Button createRoomButton;

    [Header("��ũ�� ��")]
    [SerializeField] Button PlusButton;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject roomBtnPref;
    [SerializeField] Transform roomBtnParent;

    [Header("�游��� �˾�")]
    [SerializeField] GameObject createRoomPopUp;
    [SerializeField] TMP_InputField createRoomName;
    [SerializeField] Button createRoomButtonInPannel;

    private string roomName;
    int index = 0; // �� ��� ���ڰ� ���� ��

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 20
    };

    private void Awake()
    {
        createRoomName = createRoomPopUp.GetComponentInChildren<TMP_InputField>();
        createRoomPopUp.SetActive(false);

        // ��ư �̺�Ʈ �޼ҵ� ����
        randomJoinButton.onClick.AddListener(OnClickRandomJoinButton);
        createRoomButton.onClick.AddListener(OnClickCreateRoomButton);
        createRoomButtonInPannel.onClick.AddListener(OnClickcreateRoomButtonInPannel);
        PlusButton.onClick.AddListener(OnClickPlusButton);

        PhotonNetwork.GameVersion = gameVersion;

        // ������ ���� ����õ�
        PhotonNetwork.ConnectUsingSettings();


        deactivateJoinButton("������");
    }

    private void OnClickPlusButton()
    {
        GameObject roomButton = Instantiate(roomBtnPref, roomBtnParent) as GameObject;
        roomButton.GetComponent<RoomButton>().peopleCount = index; // �� ��� ����
        //roomButton.GetComponent<RoomButton>().RoomNameText = index; // �� ��� ����
    }

    private void deactivateJoinButton(string message)
    {
        randomJoinButton.interactable = false;
        logText.text = message;
    }

    private void activeJoinButton()
    {
        randomJoinButton.interactable = true;
        // randomJoinButtonText.text = "�����ϱ�";
    }

    public override void OnConnectedToMaster()
    {
        activeJoinButton();
        logText.text = "�����Ϳ� ���� ���ӵ�";
    }

    public override void OnDisconnected(DisconnectCause cause) // ConnectUsingSettings()�� ������ ������ �� ȣ��Ǵ� �ݹ��Լ���.
    {
        deactivateJoinButton("������ ����. ������ �õ� ��..");

        PhotonNetwork.ConnectUsingSettings();
    }
    
    private void createRoomUI(string _rommName)
    {
        GameObject roomButton = Instantiate(roomBtnPref, roomBtnParent) as GameObject;
        roomButton.GetComponent<RoomButton>().peopleCount = index; // �� ��� ����
        roomButton.GetComponent<RoomButton>().RoomNameText.text = _rommName;
    }

    private void OnClickRandomJoinButton()
    {
        if (nickname.text.Length == 0)
        {
            logText.text = "�г����� �Է��ϼ���";
            return;
        }

        // ������ ������ �������̶�� �� ���� ����
        if (PhotonNetwork.IsConnected)
        {
            Data data = FindObjectOfType<Data>();
            data.Nickname = nickname.text;

            //PhotonNetwork.JoinOrCreateRoom("Metavers", RandomRoomOptions, TypedLobby.Default);
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            deactivateJoinButton("������ ����. ������ �õ� ��..");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void OnClickCreateRoomButton()
    {
        if (nickname.text.Length == 0)
        {
            logText.text = "�г����� �Է��ϼ���";
            return;
        }

        createRoomPopUp.SetActive(true);
    }

    private void OnClickcreateRoomButtonInPannel()
    {
        if (createRoomName.text.Length == 0)
        {
            logText.text = "�� �̸��� �Է��ϼ���";
            return;
        }
        roomName = createRoomName.text;
        createRoomUI(roomName);

        PhotonNetwork.JoinOrCreateRoom(roomName, RandomRoomOptions, TypedLobby.Default);

        createRoomPopUp.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        logText.text = "�濡 ������";

        PhotonNetwork.LoadLevel("Main");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        logText.text = "�� ���� ����, ���ο� �� ����..";

        OnClickCreateRoomButton();
    }
}
