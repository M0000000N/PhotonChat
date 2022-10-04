using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomButton : MonoBehaviour
{
    public int peopleCount;
    public TextMeshProUGUI RoomNameText;
    [SerializeField] TextMeshProUGUI memberText;
    void Start()
    {
        peopleCount = 0;
        memberText.text = $"{peopleCount} / 8";
    }
}
