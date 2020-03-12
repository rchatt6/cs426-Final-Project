/////////////////////////////////////////////////////////////////////////////////
////////////////////////////////bl_RoomMenu.cs///////////////////////////////////
/////////////////place this in a scene for handling menus of room////////////////
/////////////////////////////////////////////////////////////////////////////////
///////////////////////////////Lovatto Studio////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System;

public class bl_RoomMenu : bl_MonoBehaviour
{
    public bool isPlaying { get; set; }
    [HideInInspector]
    public float m_sensitive = 2.0f,SensitivityAim;
    [HideInInspector]public int WeaponCameraFog = 60;
    [HideInInspector]
    public bool ShowWarningPing = false;

    [HideInInspector]
    public bool showMenu = true;
    [HideInInspector]
    public bool isFinish = false;
    [HideInInspector]
    public bool SpectatorMode, WaitForSpectator = false;
    /// <summary>
    /// Reference of player class select
    /// </summary>
    public static PlayerClass PlayerClass = PlayerClass.Assault;
    [Header("Inputs")]
    public KeyCode ScoreboardKey = KeyCode.N;
    public KeyCode PauseMenuKey = KeyCode.Escape;
    public KeyCode ChangeClassKey = KeyCode.M;
    [Header("Map Camera")]
    /// <summary>
    /// Rotate room camera?
    /// </summary>
    public bool RotateCamera = true;
    /// <summary>
    /// Rotation Camera Speed
    /// </summary>
    public float CameraRotationSpeed = 5;
    public static float m_alphafade = 3;
    [Header("LeftRoom")]
    [Range(0.0f,5)]
    public float DelayLeave = 1.5f;

    private GameObject ButtonsClassPlay = null;

    private bl_GameManager GM;  
    private bl_UIReferences UIReferences;
    private bool m_showbuttons;
#if ULSP
    private bl_DataBase DataBase;
#endif
    public Action<Team> onWaitUntilRoundFinish; //event called when a player enter but have to wait until current round finish.

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        if (!isConnected)
            return;

        base.Awake();
        GM = FindObjectOfType<bl_GameManager>();
        UIReferences = FindObjectOfType<bl_UIReferences>();
        #if ULSP
        DataBase = bl_DataBase.Instance;
        if(DataBase != null) { DataBase.RecordTime(); }
#endif
        ButtonsClassPlay = UIReferences.ButtonsClassPlay;
        ShowWarningPing = false;
        showMenu = true;
        GetPrefabs();
        bl_UIReferences.Instance.PlayerUI.PlayerUICanvas.enabled = false;
    }

    protected override void OnEnable()
    {
        bl_EventHandler.onLocalPlayerSpawn += OnPlayerSpawn;
        bl_EventHandler.onLocalPlayerDeath += OnPlayerLocalDeath;
#if MFPSM
        bl_TouchHelper.OnPause += OnPause;
#endif
        bl_PhotonCallbacks.LeftRoom += OnLeftRoom;
    }

    protected override void OnDisable()
    {
        bl_EventHandler.onLocalPlayerSpawn -= OnPlayerSpawn;
        bl_EventHandler.onLocalPlayerDeath -= OnPlayerLocalDeath;
#if MFPSM
        bl_TouchHelper.OnPause -= OnPause;
#endif
        bl_PhotonCallbacks.LeftRoom -= OnLeftRoom;
    }

    void OnPlayerSpawn()
    {
        bl_UIReferences.Instance.PlayerUI.PlayerUICanvas.enabled = true;
    }

    void OnPlayerLocalDeath()
    {
        bl_UIReferences.Instance.PlayerUI.PlayerUICanvas.enabled = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnUpdate()
    {
        PauseControll();
        ScoreboardControll();

        if (RotateCamera &&  !isPlaying && !SpectatorMode)
        {
            this.transform.Rotate(Vector3.up * Time.deltaTime * CameraRotationSpeed);
        }

        if (isPlaying && Input.GetKeyDown(ChangeClassKey) && ButtonsClassPlay != null && !bl_GameData.Instance.isChating)
        {
            m_showbuttons = !m_showbuttons;
            if (m_showbuttons)
            {
                if (!ButtonsClassPlay.activeSelf)
                {
                    ButtonsClassPlay.SetActive(true);
                    bl_UtilityHelper.LockCursor(false);
                }
            }
            else
            {
                if (ButtonsClassPlay.activeSelf)
                {
                    ButtonsClassPlay.SetActive(false);
                    bl_UtilityHelper.LockCursor(true);
                }
            }
        }

        if (SpectatorMode && Input.GetKeyUp(KeyCode.Escape)) { bl_UtilityHelper.LockCursor(false); }
    }

    /// <summary>
    /// 
    /// </summary>
    void PauseControll()
    {
        bool pauseKey = Input.GetKeyDown(PauseMenuKey);
#if INPUT_MANAGER
        if (bl_Input.Instance.isGamePad)
        {
            pauseKey = bl_Input.isStartPad;
        }
#endif
        if (pauseKey && GM.alreadyEnterInGame && !isFinish && !SpectatorMode)
        {
            bool asb = UIReferences.isMenuActive;
            asb = !asb;
            UIReferences.ShowMenu(asb);
            bl_UtilityHelper.LockCursor(!asb);
            bl_UCrosshair.Instance.Show(!asb);
        }
    }

    public void OnPause()
    {
        if (GM.alreadyEnterInGame && !isFinish && !SpectatorMode)
        {
            bool asb = UIReferences.isMenuActive;
            asb = !asb;
            UIReferences.ShowMenu(asb);
            bl_UtilityHelper.LockCursor(!asb);
            bl_UCrosshair.Instance.Show(!asb);
        }
    }

    public bool isPaused { get { return UIReferences.isMenuActive; } }

    /// <summary>
    /// 
    /// </summary>
    void ScoreboardControll()
    {
        if (!UIReferences.isOnlyMenuActive && !isFinish)
        {
            if (Input.GetKeyDown(ScoreboardKey))
            {
                bool asb = UIReferences.isScoreboardActive;
                asb = !asb;
                UIReferences.ShowScoreboard(asb);
            }
            if (Input.GetKeyUp(ScoreboardKey))
            {
                bool asb = UIReferences.isScoreboardActive;
                asb = !asb;
                UIReferences.ShowScoreboard(asb);
            }
        }
    }

    public void OnSpectator(bool active)
    {
        SpectatorMode = active;
        bl_UtilityHelper.LockCursor(active);
        if (active)
        {
            this.GetComponentInChildren<Camera>().transform.rotation = Quaternion.identity;
        }
        GetComponentInChildren<bl_SpectatorCamera>().enabled = active;
    }

    /// <summary>
    /// Use for change player class for next Re spawn
    /// </summary>
    /// <param name="m_class"></param>
    public void ChangeClass()
    {
        if (isPlaying && GM.alreadyEnterInGame)
        {
            ButtonsClassPlay.SetActive(false);
            bl_UtilityHelper.LockCursor(true);
        }
        m_showbuttons = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnAutoTeam()
    {
        bl_UtilityHelper.LockCursor(true);
        showMenu = false;
        isPlaying = true;
        ButtonsClassPlay.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void JoinTeam(int id)
    {
        Team t = Team.None;
        string tn = "Team";
        if (id == 0)
        {
            t = Team.All;
        }
        else if (id == 1)
        {
            t = Team.Team1;
            tn = bl_GameData.Instance.Team1Name;
        }
        else if (id == 2)
        {
            t = Team.Team2;
            tn = bl_GameData.Instance.Team2Name;
        }
        string joinText = isOneTeamMode ? bl_GameTexts.JoinedInMatch : bl_GameTexts.JoinIn;
#if LOCALIZATION
        joinText = isOneTeamMode ? bl_Localization.Instance.GetText(17) : bl_Localization.Instance.GetText(23);
#endif
        if (isOneTeamMode)
        {
            bl_KillFeed.Instance.SendMessageEvent(string.Format("{0} {1}", PhotonNetwork.NickName, joinText));
        }
        else
        {
            string jt = string.Format("{0} {1}", joinText, tn);
            bl_KillFeed.Instance.SendTeamHighlightMessage(PhotonNetwork.NickName, jt, t);
        }
        showMenu = false;
#if !PSELECTOR
        bl_UtilityHelper.LockCursor(true);
        isPlaying = true;
#else
        if (MFPS.PlayerSelector.bl_PlayerSelectorData.Instance.PlayerSelectorMode == MFPS.PlayerSelector.bl_PlayerSelectorData.PSType.InLobby)
        {
            bl_UtilityHelper.LockCursor(true);
            isPlaying = true;
        }
#endif

        if (GetGameMode.GetGameModeInfo().onRoundStartedSpawn == bl_GameData.GameModesEnabled.OnRoundStartedSpawn.WaitUntilRoundFinish && GM.GameMatchState == MatchState.Playing)
        {
            if(onWaitUntilRoundFinish != null) { onWaitUntilRoundFinish.Invoke(t); }
            bl_GameManager.Instance.SetLocalPlayerToTeam(t);
            return;
        }

        GM.SpawnPlayer(t);
    }

    /// <summary>
    /// 
    /// </summary>
    public void LeftOfRoom()
    {
#if ULSP
        if (DataBase != null)
        {
            Player p = PhotonNetwork.LocalPlayer;
            DataBase.SaveData(p.GetPlayerScore(), p.GetKills(), p.GetDeaths());
            DataBase.StopAndSaveTime();
        }
#endif
        //Good place to save info before reset statistics
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
#if UNITY_EDITOR
            if (isApplicationQuitting) return;
#endif
            bl_UtilityHelper.LoadLevel(bl_GameData.Instance.MainMenuScene);
        }
    }

    bool isApplicationQuitting = false;
    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Suicide()
    {
        PhotonView view = PhotonView.Find(bl_GameManager.LocalPlayerViewID);
        if (view != null)
        {

            bl_PlayerHealthManager pdm = view.GetComponent<bl_PlayerHealthManager>();
            pdm.Suicide();
            bl_UtilityHelper.LockCursor(true);
            showMenu = false;
            if (view.IsMine)
            {
                bl_GameManager.SuicideCount++;
                //Debug.Log("Suicide " + bl_GameManager.SuicideCount + " times");
                //if player is a joker o abuse of suicide, them kick of room
                if (bl_GameManager.SuicideCount >= 3)//Max number of suicides  = 3, you can change
                {
                    isPlaying = false;
                    bl_GameManager.isLocalAlive = false;
                    bl_UtilityHelper.LockCursor(false);
                    LeftOfRoom();
                }
            }
        }
        else
        {
            Debug.LogError("This view " + bl_GameManager.LocalPlayerViewID + " is not found");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void GetPrefabs()
    {
        PlayerClass = PlayerClass.GetSavePlayerClass();
#if CLASS_CUSTOMIZER
        PlayerClass = bl_ClassManager.Instance.m_Class;
#endif
        UIReferences.OnChangeClass(PlayerClass);
    }

    private bool imv = false;
    public bool SetIMV
    {
        get
        {
            return imv;
        }set
        {
            imv = value;
            PlayerPrefs.SetInt(PropertiesKeys.InvertMouseVertical, (value) ? 1 : 0);
        }
    }

      private bool imh = false;
    public bool SetIMH
    {
        get
        {
            return imh;
        }
        set
        {
            imh = value;
            PlayerPrefs.SetInt(PropertiesKeys.InvertMouseHorizontal, (value) ? 1 : 0);
        }
    }

    public bool isMenuOpen
    {
        get
        {
            return UIReferences.State != bl_UIReferences.RoomMenuState.Hidde;
        }
    }

   public void OnLeftRoom()
   {
       Debug.Log("OnLeftRoom (local)");      
       this.GetComponent<bl_MatchTimeManager>().enabled = false;
       StartCoroutine(UIReferences.FinalFade(true));
   }

    private static bl_RoomMenu _instance;
    public static bl_RoomMenu Instance
    {
        get
        {
            if (_instance == null) { _instance = FindObjectOfType<bl_RoomMenu>(); }
            return _instance;
        }
    }
}