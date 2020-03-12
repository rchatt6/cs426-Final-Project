using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFPSEditor;
using UnityEditor;

public class MFPSGeneralDoc : TutorialWizard
{

    //required//////////////////////////////////////////////////////
    private const string ImagesFolder = "mfps2/editor/general/";
    private NetworkImages[] ServerImages = new NetworkImages[]
    {
        new NetworkImages{Name = "img-0.jpg", Image = null},
        new NetworkImages{Name = "img-1.jpg", Image = null},
        new NetworkImages{Name = "img-2.jpg", Image = null},
        new NetworkImages{Name = "img-3.jpg", Image = null},
        new NetworkImages{Name = "img-4.jpg", Image = null},
        new NetworkImages{Name = "img-5.jpg", Image = null},
        new NetworkImages{Name = "img-6.jpg", Image = null},
        new NetworkImages{Name = "img-7.jpg", Image = null},
        new NetworkImages{Name = "img-8.jpg", Image = null},
        new NetworkImages{Name = "img-9.jpg", Image = null},
        new NetworkImages{Name = "img-10.jpg", Image = null},
        new NetworkImages{Name = "img-12.jpg", Image = null},
    };
    private Steps[] AllSteps = new Steps[] {
     new Steps { Name = "Resume", StepsLenght = 0 },
    new Steps { Name = "GameData", StepsLenght = 0 },
    new Steps { Name = "Kill Feed", StepsLenght = 2 },
    new Steps { Name = "AFK", StepsLenght = 0 },
    new Steps { Name = "Kick Votation", StepsLenght = 0 },
    };

    public override void WindowArea(int window)
    {
       if(window == 0) { Resume(); }
        else if (window == 1) { GameDataDoc(); }
        else if (window == 2) { KillFeedDoc(); }
        else if (window == 3) { AfkDoc(); }
        else if (window == 4) { KickVotationDoc(); }
    }
    //final required////////////////////////////////////////////////

    public override void OnEnable()
    {
        base.OnEnable();
        base.Initizalized(ServerImages, AllSteps, ImagesFolder);
        GUISkin gs = Resources.Load<GUISkin>("content/MFPSEditorSkin") as GUISkin;
        if (gs != null)
        {
            base.SetTextStyle(gs.customStyles[2]);
            base.m_GUISkin = gs;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Resume()
    {
        DrawTitleText("MFPS 2.0");
        GUILayout.Label("Version: " + AssetData.Version);
        DownArrow();
        DrawTitleText("Custom Integration Tutorials");
        if(GUILayout.Button("<color=#3987D6>Integrate Loading Screen to MFPS</color>", EditorStyles.label))
        {
            Application.OpenURL("https://www.lovattostudio.com/en/integrate-loading-screen-to-mfps-2-0/");
        }
        if (GUILayout.Button("<color=#3987D6>Integrate DestroyIt to MFPS</color>", EditorStyles.label))
        {
            Application.OpenURL("https://www.lovattostudio.com/en/integrate-destroyit-to-mfps-2-0/");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void GameDataDoc()
    {
        DrawTitleText("GameData");
        DrawText("In this and other MFPS tutorials you will notice that <b>GameData</b> is mentioned or referenced a lot, if you still doesn't understand what it's or which is the function of this 'object', keep reading this.\n \n" +
            "- In essence, GameData is a scriptable object of bl_GameData.cs script which containing a lot of front-end settings to tweak MFPS as dev's needed, contain from simply settings like show blood in game or not to " +
            "all weapons and game modes information.\n \nBy default this GameData is located in the <b>Resources</b> folder of MFPS.");
        DrawServerImage(3);
    }

    /// <summary>
    /// 
    /// </summary>
    void KillFeedDoc()
    {
        if (subStep == 0)
        {
            DrawTitleText("KILL FEED");
            DrawText("Kill Feed or Who Kill Who is the UI text notification panel where display events of the match like but not limited to Kills events, showing the player that kill and the player that get killed.   Normally this panel " +
                "is set in a corner of the screen to not interfere with the game play but begin accessible in any moment.\n \nIn this tutorial I'll show you a few options that you have to customize it without touch code and how " +
                "you can display your own events on it.");
            DownArrow();
            DrawText("• MFPS come with two modes to display kills events on the kill feed, in a kill event there is 3 slots of information to display: the killer name, the killed name and the weapon with which the kill was perpetrated," +
                "so the choose for you here is how you wanna display the weapon, you have to option:\n \n<b>Weapon Name:</b>");
            DrawServerImage(0);
            DrawText("<b>Weapon Icon:</b>");
            DrawServerImage(1);
            DrawText("By default <b>Weapon Icon</b> is the default option, you can change that in <b>GameData</b> -> KillFeedWeaponShowMode.\n \nAnother option to customize is the color to highlight the local player name when this " +
                "appear in the kill feed, for the context player names in kill feed are represented by the color of his Team but in order to the local player easily knows when an event that include him appear in the kill feed, his name " +
                "should be highlight with a different color, <b>to choose that color</b> go to GameData -> <b>HighLightColor.</b>\n \nOkay, that are the front-end customize options, if you want to customize the way that the UI looks" +
                " you have to do the in the UI prefab which is located in: <i>Assets -> MFPS -> Content -> Prefabs -> UI -> Instances -> <b>KillFeed</b></i>, drag this prefab inside the kill feed panel in canvas which is located by default in: ");
            DrawServerImage(2);
            DrawText("Right, these are all customize options that you have in front end, if you wanna create your own events to display, check the next step.");
        }
        else if (subStep == 1)
        {
            DrawTitleText("CREATE KILLFEED EVENTS");
            DrawText("The kill feed system various type of events to display, use the one that fits your event:\n \n<b>Kill Event:</b>\n \n• This is should use when of course a kill event happen, but a kill that include two actors " +
                "the killer and the killed, to show that you have to call this:");
            DrawCodeText("bl_KillFeed.Instance.SendKillMessageEvent(string killer, string killed, int gunID, Team killerTeam, bool byHeadshot);");
            DrawText("<b>Message:</b>\n \n• If you want to show a simple text of an event in concrete that doesn't include a player in specific, use:");
            DrawCodeText("bl_KillFeed.Instance.SendMessageEvent(string message);");
            DrawText("<b>Team Highlight:</b>\n \n• If you want to show a text of an event in concrete that as subject have a team in specific and you wanna highlight a part of the text with the tam color, use:");
            DrawCodeText("bl_KillFeed.Instance.SendTeamHighlightMessage(string teamHighlightMessage, string normalMessage, Team playerTeam);");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void AfkDoc()
    {
        DrawTitleText("AFK");
        DrawText("AFK is an abbreviation for <i>away from keyboard</i>, players are called AFK when they have not interact with the game in a extended period of time.  in multi player games AFK players could be a problem," +
            "like for example in MFPS where players play in teams, an AFK player represent free points for the enemy team, or in different context AFK player are used to leveling up, so that is way some games count with a system " +
            "to detect these AFK players and kick out of the server/room after a certain period of time begin AFK.  MFPS include this system but <b>is disable by default</b>.\n \n" +
            "In order to enable AFK detection, go to GameData -> turn on <b>Detect AFK</b>, -> set the seconds before kick out players after detected as AFK in <b>AFK Time Limit</b>");
    }

    /// <summary>
    /// 
    /// </summary>
    void KickVotationDoc()
    {
        DrawTitleText("KICK VOTATION");
        DrawText("In order to give an option to players to get rip of toxic, hackers, non-rules players in a democratic way where a player put the option on the table and the majority of the players in room " +
            "decide to kick out or cancel the petition, MFPS include a voting system.\n \nTo start a vote in game, players have to open the menu -> in the scoreboard click / touch over the player that they want to request the vote -> " +
            "in the PopUp menu that will appear -> Click on <b>Request Kick</b> button.\n \nBy default the keys to vote are F1 for Yes and F2 for No, you can change these keys in bl_KickVotation.cs which is attached in <b>GameManager</b> " +
            "in maps scenes.");
        DownArrow();
        DrawText("If you want to implement your own way to start a voting request, you can do it by calling:");
        DrawCodeText("bl_KickVotation.Instance.RequestKick(Photon.Realtime.Player playerToKick);");
    }

    [MenuItem("MFPS/Tutorials/MFPS General")]
    private static void ShowWindowMFPS()
    {
        EditorWindow.GetWindow(typeof(MFPSGeneralDoc));
    }
}