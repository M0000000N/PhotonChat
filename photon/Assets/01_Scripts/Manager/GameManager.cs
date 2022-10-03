using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameObject PlayerPrefeb;

    public void Start()
    {
        float randomPosX = Random.Range(-50f, 50f);
        float randomPosZ = Random.Range(-50f, 50f);
        Vector3 randomPos = new Vector3(randomPosX, 1f, randomPosZ);

        GameObject playerObject = PhotonNetwork.Instantiate("Player", randomPos, Quaternion.identity);
        PlayerController player = playerObject.GetComponent<PlayerController>();
    }
}
