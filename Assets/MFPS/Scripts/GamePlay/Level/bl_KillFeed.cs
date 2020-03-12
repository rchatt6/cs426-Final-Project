using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class bl_KillFeed : bl_PhotonHelper
{
    public LocalKillDisplay m_LocalKillDisplay = LocalKillDisplay.Individual;
    [Range(1, 7)] public float IndividualShowTime = 3;
    public Color SelfColor = Color.green;
    //private
    private bl_UIReferences UIReference;
    private List<KillInfo> localKillsQueque = new List<KillInfo>();

#if LOCALIZATION
    private int[] LocaleTextIDs = new int[] { 28,17, };
    private string[] LocaleStrings;
#endif
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        UIReference = FindObjectOfType<bl_UIReferences>();
        if (PhotonNetwork.InRoom)
        {
#if LOCALIZATION
            LocaleStrings = bl_Localization.Instance.GetTextArray(LocaleTextIDs);
#endif
            OnJoined();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        bl_EventHandler.onLocalKill += this.NewLocalKill;
        bl_PhotonCallbacks.PlayerLeftRoom += OnPhotonPlayerDisconnected;
    }
    /// <summary>
    /// 
    /// </summary>
    void OnDisable()
    {
        bl_EventHandler.onLocalKill -= this.NewLocalKill;
        bl_PhotonCallbacks.PlayerLeftRoom -= OnPhotonPlayerDisconnected;
    }


    /// <summary>
    /// Player Joined? sync
    /// </summary>
    void OnJoined()
    {
#if LOCALIZATION
        photonView.RPC("AddNewKillFeed", RpcTarget.Others, PhotonNetwork.NickName, LocaleStrings[1], string.Empty, string.Empty, 777, 20);
#else
        SendMessageEvent(string.Format("{0} {1}", PhotonNetwork.NickName, bl_GameTexts.JoinedInMatch));
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public void SendKillMessageEvent(string killer, string killed, int gunID, Team killerTeam, bool byHeadshot)
    {
        HashTable data = new HashTable();
        data.Add("killer", killer);
        data.Add("killed", killed);
        data.Add("gunid", gunID);
        data.Add("team", killerTeam);
        data.Add("headshot", byHeadshot);
        data.Add("mt", KillFeedMessageType.WeaponKillEvent);
        SendMessageOverNetwork(data);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SendMessageEvent(string message, bool localOnly = false)
    {
        HashTable data = new HashTable();
        data.Add("message", message);
        data.Add("mt", KillFeedMessageType.Message);
        if (localOnly) { ReceiveMessage(data); return; }

        SendMessageOverNetwork(data);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SendTeamHighlightMessage(string teamHighlightMessage, string normalMessage, Team playerTeam)
    {
        HashTable data = new HashTable();
        data.Add("killer", teamHighlightMessage);
        data.Add("message", normalMessage);
        data.Add("team", playerTeam);
        data.Add("mt", KillFeedMessageType.TeamHighlightMessage);
        SendMessageOverNetwork(data);
    }

    /// <summary>
    /// 
    /// </summary>
    void SendMessageOverNetwork(HashTable data)
    {
        bl_PhotonNetwork.Instance.SendDataOverNetwork(PropertiesKeys.KillFeedEvent, data);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnMessageReceive(HashTable data)
    {
        KillFeedMessageType mtype = (KillFeedMessageType)data["mt"];
        switch (mtype)
        {
            case KillFeedMessageType.WeaponKillEvent:
                ReceiveWeaponKillEvent(data);
                break;
            case KillFeedMessageType.Message:
                ReceiveMessage(data);
                break;
            case KillFeedMessageType.TeamHighlightMessage:
                ReceiveOnePlayerMessage(data);
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void ReceiveWeaponKillEvent(HashTable data)
    {
        KillFeed kf = new KillFeed();
        kf.Killer = (string)data["killer"];
        kf.Killed = (string)data["killed"];
        kf.GunID = (int)data["gunid"];
        kf.HeadShot = (bool)data["headshot"];
        kf.KillerTeam = (Team)data["team"];
        kf.messageType = KillFeedMessageType.WeaponKillEvent;

        UIReference.SetKillFeed(kf);
    }

    /// <summary>
    /// 
    /// </summary>
    void ReceiveMessage(HashTable data)
    {
        KillFeed kf = new KillFeed();
        kf.Message = (string)data["message"];
        kf.messageType = KillFeedMessageType.Message;

        UIReference.SetKillFeed(kf);
    }

    /// <summary>
    /// 
    /// </summary>
    void ReceiveOnePlayerMessage(HashTable data)
    {
        KillFeed kf = new KillFeed();
        kf.Killer = (string)data["killer"];
        kf.Message = (string)data["message"];
        kf.KillerTeam = (Team)data["team"];
        kf.messageType = KillFeedMessageType.TeamHighlightMessage;

        UIReference.SetKillFeed(kf);
    }

    public void OnPhotonPlayerDisconnected(Player otherPlayer)
    {
#if LOCALIZATION
        SendMessageEvent(string.Format("{0} {1}", otherPlayer.NickName, bl_Localization.Instance.GetText(18)), true);
#else
        SendMessageEvent(string.Format("{0} {1}", otherPlayer.NickName, bl_GameTexts.LeftOfMatch), true);
#endif
    }

    /// <summary>
    /// Show a local ui when out killed other player
    /// </summary>
    private void NewLocalKill(KillInfo localKill)
    {
        if (localKillsQueque.Count <= 0)
        {
            bl_UIReferences.Instance.SetLocalKillFeed(localKill, m_LocalKillDisplay);
        }
        localKillsQueque.Add(localKill);
    }

    /// <summary>
    /// 
    /// </summary>
    public void LocalDisplayDone()
    {
        localKillsQueque.RemoveAt(0);
        if(localKillsQueque.Count > 0)
        {
            bl_UIReferences.Instance.SetLocalKillFeed(localKillsQueque[0], m_LocalKillDisplay);
        }
    }

    private static bl_KillFeed _instance;
    public static bl_KillFeed Instance
    {
        get
        {
            if (_instance == null) { _instance = FindObjectOfType<bl_KillFeed>(); }
            return _instance;
        }
    }

    [System.Serializable]
    public enum LocalKillDisplay
    {
        Individual,
        Multiple,
    }
}