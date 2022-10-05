using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomButton : MonoBehaviourPunCallbacks
{
    public int peopleCount;
    [SerializeField] TextMeshProUGUI RoomNameText;
    [SerializeField] TextMeshProUGUI memberText;

    public RoomInfo RoomInfo { get; private set; }
    public void SetRoomInfo(RoomInfo _roomInfo)
    {
        RoomInfo = _roomInfo;
        RoomNameText.text = _roomInfo.Name;
        memberText.text = $"{_roomInfo.PlayerCount} / {_roomInfo.MaxPlayers}";
    }
}
