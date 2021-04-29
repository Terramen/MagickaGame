using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    private readonly string connectionStatusMessage = "    Connection Status: ";
    private readonly string isOfflineStatus = "    Offline Status: ";

    [Header("UI References")]
    public Text ConnectionStatusText;

    public Text offlineStatus;

    #region UNITY

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
        offlineStatus.text = isOfflineStatus + PhotonNetwork.OfflineMode;
    }

    #endregion
}
