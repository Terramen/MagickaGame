using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    private readonly string connectionStatusMessage = "    Connection Status: ";
    private readonly string isOfflineStatus = "    Offline Status: ";
    private readonly string isPingStatus = "    Ping Status: ";
    

    [Header("UI References")]
    public Text ConnectionStatusText;

    public Text offlineStatus;
    public Text pingStatus;

    #region UNITY

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
        offlineStatus.text = isOfflineStatus + PhotonNetwork.OfflineMode;
        pingStatus.text = isPingStatus + PhotonNetwork.GetPing();
    }

    #endregion
}
