using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using System;

namespace MFPSEditor.Addons
{
    public class MFPSAddonsData : ScriptableObject
    {
      //  [Reorderable]
        public List<MFPSAddonsInfo> Addons = new List<MFPSAddonsInfo>();
        public bool AutoUpdate = false;

        [ContextMenu("Get Version Json")]
        void VersionJson()
        {
            AddonsVersionList avl = new AddonsVersionList();
            for (int i = 0; i < Addons.Count; i++)
            {
                MFPSAddonVersion version = new MFPSAddonVersion();
                version.Name = Addons[i].NiceName;
                version.Version = Addons[i].Info == null ? Addons[i].LastVersion : Addons[i].Info.Version;
                version.ChangeLog = Addons[i].ChangeLog;
                avl.Data.Add(version);
            }
            string json = JsonUtility.ToJson(avl);
            TextEditor te = new TextEditor();
            te.text = json;
            te.SelectAll();
            te.Copy();
        }

        private static MFPSAddonsData m_Data;
        public static MFPSAddonsData Instance
        {
            get
            {
                if (m_Data == null)
                {
                    m_Data = Resources.Load("MFPSAddonsData", typeof(MFPSAddonsData)) as MFPSAddonsData;
                }
                return m_Data;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MFPSAddonsData))]
    public class MFPSAddonsDataEditor : Editor
    {
        private MFPSAddonsData script;
        private SerializedProperty list;

        private EditorWWW WWW;
        private AddonsVersionList VersionData;
        private int State = 0;
        public const string VersionURL = "http://www.lovattostudio.com/game-system/mfps-addons-info/addons-versions.txt";
        private const string lastCheckKey = "addons.version.lastcheck";
        private string lastCheckTime = "--";
        Version MFPSVersion;
       
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            script = (MFPSAddonsData)target;
            list = serializedObject.FindProperty("Addons");
            WWW = new EditorWWW();
            float lastCheck = PlayerPrefs.GetFloat(lastCheckKey, 0);
            float nextTime = (float)EditorApplication.timeSinceStartup - lastCheck;
            bool checkNow = (lastCheck <= 0 || nextTime > 3600);
            MFPSVersion = new Version(AssetData.Version);

            if (State == 0 && checkNow && script.AutoUpdate)
            {
                WWW.SendRequest(VersionURL, null, ReceiveInfo);
                State = 1;
            }
        }

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(lastCheckKey, 0);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            GUILayout.Space(10);
            if (State == 0)
            {
                GUILayout.Label("Data not update.");
            }
            else if (State == 1)
            {
                GUILayout.Label("Loading...");
            }
            else
            {
                GUILayout.Label("Last Check: " + lastCheckTime);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                WWW.SendRequest(VersionURL, null, ReceiveInfo);
                State = 1;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(list, true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            script.AutoUpdate = EditorGUILayout.ToggleLeft("Auto Update", script.AutoUpdate, EditorStyles.toolbarButton);
            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < script.Addons.Count; i++)
                {
                    script.Addons[i].CurrentVersion = script.Addons[i].Info == null ? "--" : script.Addons[i].Info.Version;
                }
            }
        }

        public void ReceiveInfo(string data, bool isError)
        {
            if (!isError)
            {
                VersionData = JsonUtility.FromJson<AddonsVersionList>(data);
                State = 2;
                for (int i = 0; i < script.Addons.Count; i++)
                {
                    MFPSAddonsInfo addon = script.Addons[i];
                    BuildAddon(ref addon);
                }
            }
            float lct = PlayerPrefs.GetFloat(lastCheckKey, 0);

            if (lct == 0) { lastCheckTime = DateTime.Now.ToString("HH:mm:ss"); }
            else
            {
                float secondsSince = (float)EditorApplication.timeSinceStartup - lct;
                DateTime lt = DateTime.Now.AddSeconds(-secondsSince);
                lastCheckTime = lt.ToString("HH:mm:ss");
            }
            PlayerPrefs.SetFloat(lastCheckKey, (float)EditorApplication.timeSinceStartup);

            Repaint();
        }

        void BuildAddon(ref MFPSAddonsInfo addon)
        {
            bool isFolder = AssetDatabase.IsValidFolder("Assets/Addons");
            if (isFolder)
            {
                addon.isInProject = AssetDatabase.IsValidFolder("Assets/Addons/" + addon.FolderName);
            }
            addon.isIntegrated = EditorUtils.CompilerIsDefine(addon.KeyName);
            if (addon.Info != null)
            {
                Version av = new Version(addon.Info.MinMFPSVersion);
                int result = MFPSVersion.CompareTo(av);
                addon.CompatibleWithThisMFPS = result >= 0;
            }

            if (VersionData != null)
            {
                for (int i = 0; i < VersionData.Data.Count; i++)
                {
                    if (VersionData.Data[i].Name == addon.NiceName)
                    {
                        addon.LastVersion = VersionData.Data[i].Version;
                        addon.ChangeLog = VersionData.Data[i].ChangeLog;
                        return;
                    }
                }
            }
            else
            {
                addon.LastVersion = "--";
            }
        }

    }

    public class MFPSAddonsWindow : EditorWindow
    {
        private MFPSAddonsData Data;
        private Vector2 addonsList = new Vector2();
        private Vector2 changeList = new Vector2();
        private int addonID = -1;
        private EditorWWW WWW = new EditorWWW();
        private AddonsVersionList VersionData;
        private StoreData AddonsInfoData = null;
        Version MFPSVersion;
        private int WindowState = 0;
        private Texture2D whiteTexture;
        private GUIStyle desStyle = null;
        private GUIStyle TextStyleFlat = null;
        private CustomWindows contentWindow = CustomWindows.Addons;
        private float leftPanelWidth = 230;
        bool[] questionFoulds = new bool[] { false, false, false, false, false, false };
        Rect contentAreaSize;

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            Data = MFPSAddonsData.Instance;
            minSize = new Vector2(800, 400);
            MFPSVersion = new Version(AssetData.Version);
            whiteTexture = Texture2D.whiteTexture;
            WWW.SendRequest(MFPSAddonsDataEditor.VersionURL, null, ReceiveInfo);
            WindowState = 1;
            TextStyleFlat = Resources.Load<GUISkin>("content/MFPSEditorSkin").customStyles[1];
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenAddonPage(string addonName)
        {
            if(Data == null) { Data = MFPSAddonsData.Instance; }
            for (int i = 0; i < Data.Addons.Count; i++)
            {
                if(Data.Addons[i].NiceName.ToLower() == addonName.ToLower())
                {
                    addonID = i;
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnGUI()
        {
            if (Data == null) return;

            EditorStyles.toolbarButton.richText = true;
            if (desStyle == null)
            {
                desStyle = new GUIStyle(EditorStyles.whiteLabel);
                desStyle.wordWrap = true;
                desStyle.clipping = TextClipping.Clip;
            }

            GUILayout.BeginVertical();
            Header();
            GUILayout.BeginHorizontal();
            LeftPanel();
            GUILayout.Space(5);
            ContentArea();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if(WindowState == 1) { Loading(); }
        }

        /// <summary>
        /// 
        /// </summary>
        void Loading()
        {
            GUI.color = new Color(0, 0, 0, 0.5f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteTexture, ScaleMode.StretchToFill);
            GUI.color = Color.white;
        }

        /// <summary>
        /// 
        /// </summary>
        void Header()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbarButton,GUILayout.Height(20));

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Help", EditorStyles.toolbarButton))
            {
                contentWindow = CustomWindows.Help;
            }

            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
            {
                WWW.SendRequest(MFPSAddonsDataEditor.VersionURL, null, ReceiveInfo);
                WindowState = 1;
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 
        /// </summary>
        void LeftPanel()
        {
            GUILayout.BeginVertical("box", GUILayout.Width(leftPanelWidth));
            addonsList = GUILayout.BeginScrollView(addonsList);

            for (int i = 0; i < Data.Addons.Count; i++)
            {
                MFPSAddonsInfo addon = Data.Addons[i];
                Rect r = GUILayoutUtility.GetRect(205, 18);
                if (GUI.Button(r, "", EditorStyles.toolbarButton))
                {
                    addonID = i;
                    changeList = Vector2.zero;
                    contentWindow = CustomWindows.Addons;
                }
                if (i == addonID)
                {
                    Rect rs = r;
                    rs.width = 4;
                    GUI.color = Color.yellow;
                    GUI.DrawTexture(rs, whiteTexture, ScaleMode.StretchToFill);
                    GUI.color = Color.white;
                }
                r.x += 5;
                GUI.Label(r, addon.NiceName, EditorStyles.miniLabel);
                r.x += 150;
                string version = addon.isIntegrated ? addon.CurrentVersion + "  ✓" : addon.CurrentVersion;
                GUI.Label(r, version, EditorStyles.miniLabel);

                GUILayout.Space(1);
            }
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 
        /// </summary>
        void ContentArea()
        {
            contentAreaSize = EditorGUILayout.BeginVertical();
            EditorStyles.boldLabel.richText = true;
            EditorStyles.label.richText = true;
            EditorStyles.miniLabel.richText = true;
            GUI.skin.label.richText = true;
            if (contentWindow == CustomWindows.Addons)
            {
                DrawAddons();
            }
            else if (contentWindow == CustomWindows.Help) { DrawHelp(); }

            if(contentWindow != CustomWindows.Addons)
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("BACK", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    contentWindow = CustomWindows.Addons;
                }
            }
            EditorGUILayout.EndVertical();
        }

        void DrawAddons()
        {
            if (addonID >= 0)
            {
                MFPSAddonsInfo addon = Data.Addons[addonID];
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("<size=22>{0}</size> ", addon.NiceName.ToUpper()), EditorStyles.boldLabel);
                string it = addon.isIntegrated ? "<color=green>INTEGRATED</color>" : "<color=red>NO INTEGRATED</color>";
                Rect rt = GUILayoutUtility.GetRect(200, 20);
                rt.x -= 30;
                rt.y += 10;
                if (!string.IsNullOrEmpty(addon.KeyName))
                {
                    GUI.Label(rt, it, EditorStyles.miniLabel);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("<size=14>Version <b>{0}</b></size>", addon.CurrentVersion, TextStyleFlat));
                GUILayout.Space(20);
                GUILayout.Label(string.Format("<size=14>Last Version <b>{0}</b></size>", addon.LastVersion, TextStyleFlat));
                GUILayout.FlexibleSpace();
                if(addon.Info != null && !string.IsNullOrEmpty(addon.Info.TutorialScript))
                {
                    if (GUILayout.Button("<color=#FFE300FF>OPEN TUTORIAL</color>", EditorStyles.toolbarButton))
                    {
                        EditorWindow.GetWindow(System.Type.GetType(string.Format("{0}, Assembly-CSharp-Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", addon.Info.TutorialScript)));
                    }
                    GUILayout.Space(10);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                if (addon.isInProject)
                {
                    string ct = addon.CompatibleWithThisMFPS ? "Compatible with MFPS" : "No Compatible with MFPS";
                    GUILayout.Label(string.Format("{0} {1}", ct, AssetData.Version), EditorStyles.miniBoldLabel);
                }
                else
                {
                    GUILayout.Label("This Addons is not present in this project.");
                }

                changeList = GUILayout.BeginScrollView(changeList);
                StoreProduct sp = AddonInfo(addon.NiceName);
                if (sp != null)
                {
                    GUILayout.Label("<b>Description</b>", TextStyleFlat);
                    GUILayout.Label(sp.Description, TextStyleFlat);
                }
                if (addon.Info != null && !string.IsNullOrEmpty(addon.Info.Instructions))
                {
                    DrawSeparator();
                    GUILayout.Label(addon.Info.Instructions, TextStyleFlat);
                }
                if (addon.ChangeLog.Count > 0)
                {
                    GUILayout.Label("<size=14>CHANGE LOG:</size>", EditorStyles.boldLabel);
                    for (int i = 0; i < addon.ChangeLog.Count; i++)
                    {
                        GUILayout.Label(addon.ChangeLog[i].Version, desStyle);
                        GUILayout.Label(addon.ChangeLog[i].ChangeLog, desStyle);
                        GUILayout.Space(7);
                    }
                }
                GUILayout.EndScrollView();
                //footarea

                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal(GUILayout.Height(30));
                GUILayout.FlexibleSpace();
                string ipt = addon.isInProject ? "Addon is present in the project" : "Addon is not present in the Project";
                GUILayout.Label(ipt, EditorStyles.miniLabel);
                if (!addon.isInProject)
                {
                    if (GUILayout.Button("GET THIS ADDON", EditorStyles.toolbarButton))
                    {
                        if (sp != null)
                        {
                            Application.OpenURL(sp.Url);
                        }
                    }
                }
                GUILayout.Space(5);
                if (!string.IsNullOrEmpty(addon.KeyName))
                {
                    GUI.enabled = addon.isInProject;
                    string idt = addon.isIntegrated ? "DISABLE" : "ENABLE";
                    if (GUILayout.Button(idt, EditorStyles.toolbarButton))
                    {
                        EditorUtils.SetEnabled(addon.KeyName, !addon.isIntegrated);
                        WindowState = 1;
                    }
                    GUI.enabled = true;
                }
                GUILayout.Space(20);
                GUILayout.EndHorizontal();
            }
        }

        void DrawHelp()
        {
            EditorStyles.foldout.richText = true;
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            questionFoulds[0] = EditorGUILayout.Foldout(questionFoulds[0], "<i>How to report a bug/error with an addon?</i>",EditorStyles.foldout);
            if (questionFoulds[0])
            {
                EditorGUILayout.TextArea("If you have a problem with one of the addons or any relate question, you can get in touch with us in multiple ways:", TextStyleFlat);
                GUILayout.Space(10);
                if (GUILayout.Button("<color=yellow>Forum</color>",TextStyleFlat)) { Application.OpenURL("https://www.lovattostudio.com/forum/index.php"); }
                if (GUILayout.Button("<color=yellow>Email Form</color>", TextStyleFlat)) { Application.OpenURL("https://www.lovattostudio.com/en/select-support/"); }
                if (GUILayout.Button("<color=yellow>Direct Email</color>", TextStyleFlat)) { Application.OpenURL("mailto:contact.lovattostudio@gmail.com"); }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            questionFoulds[1] = EditorGUILayout.Foldout(questionFoulds[1], "<i>Where I can get these addons?</i>", EditorStyles.foldout);
            if (questionFoulds[1])
            {
                EditorGUILayout.TextArea("All addons listed here are available at lovattostudio.com store:", TextStyleFlat);
                GUILayout.Space(10);
                if (GUILayout.Button("Addons Shop", EditorStyles.toolbarButton)) { Application.OpenURL("https://www.lovattostudio.com/en/shop/"); }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            questionFoulds[2] = EditorGUILayout.Foldout(questionFoulds[2], "<i>Why addons are not include in the package?</i>", EditorStyles.foldout);
            if (questionFoulds[2])
            {
                EditorGUILayout.TextArea("basically it is to maintain a relatively low price for the main core package, if all the addons are added by default, the price of the package would rise to at least $250.00 or more for being a complete set," +
                    "so we decided to add the essential features to the core and let developers choose which extra extensions they want to integrate, that way developers just pay for what they want and we can keep improve the template and addons.", TextStyleFlat);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            questionFoulds[3] = EditorGUILayout.Foldout(questionFoulds[3], "<i>How I can get a bundle of addons already integrated?</i>", EditorStyles.foldout);
            if (questionFoulds[3])
            {
                EditorGUILayout.TextArea("If you precise of certain addons and you wanna pay for all once and get all of them integrated in one package, you can contact us and request a price for that.", TextStyleFlat);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            questionFoulds[4] = EditorGUILayout.Foldout(questionFoulds[4], "<i>How to know when there's an update of my addons?</i>", EditorStyles.foldout);
            if (questionFoulds[4])
            {
                EditorGUILayout.TextArea("If you purchase the addon in lovattostudio.com store, you should receive a email notifying about the update, as alternative you can check MFPS News in the Unity Editor <i>(MFPS -> News)</i> or " +
                    "check the forum addon page.", TextStyleFlat);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            questionFoulds[5] = EditorGUILayout.Foldout(questionFoulds[5], "<i>Is Lovatto an Alien?</i>", EditorStyles.foldout);
            if (questionFoulds[5])
            {
                EditorGUILayout.TextArea("Yes indeed.", TextStyleFlat);
            }
            EditorGUILayout.EndVertical();
        }

        void DrawSeparator()
        {
            GUILayout.Space(7);
            GUI.color = new Color(1, 1, 1, 0.33f);
            Rect r = GUILayoutUtility.GetRect(contentAreaSize.width, 2);
            GUI.DrawTexture(r, whiteTexture, ScaleMode.StretchToFill);
            GUI.color = Color.white;
            GUILayout.Space(7);
        }

        public void ReceiveInfo(string data, bool isError)
        {
            if (!isError)
            {
                VersionData = JsonUtility.FromJson<AddonsVersionList>(data);
                for (int i = 0; i < MFPSAddonsData.Instance.Addons.Count; i++)
                {
                    MFPSAddonsInfo addon = MFPSAddonsData.Instance.Addons[i];
                    BuildAddon(ref addon);
                }
                WWW.SendRequest("https://www.lovattostudio.com/game-system/mfps-addons-info/addons.txt", null, AddonsInfoDataReceive);
            }
            else { WindowState = 0; }
            Repaint();
        }

        void AddonsInfoDataReceive(string text, bool isError)
        {
            if (!isError)
            {
                AddonsInfoData = JsonUtility.FromJson<StoreData>(text);
            }
            else
            {
                Debug.LogError(text);
                Close();
            }
            WindowState = 0;
            Repaint();
        }

        void BuildAddon(ref MFPSAddonsInfo addon)
        {
            bool isFolder = AssetDatabase.IsValidFolder("Assets/Addons");
            if (isFolder)
            {
                addon.isInProject = AssetDatabase.IsValidFolder("Assets/Addons/" + addon.FolderName);
            }
            addon.isIntegrated = EditorUtils.CompilerIsDefine(addon.KeyName);
            if (addon.Info != null)
            {
                Version av = new Version(addon.Info.MinMFPSVersion);
                int result = MFPSVersion.CompareTo(av);
                addon.CompatibleWithThisMFPS = result >= 0;
            }

            if (VersionData != null)
            {
                for (int i = 0; i < VersionData.Data.Count; i++)
                {
                    if (VersionData.Data[i].Name == addon.NiceName)
                    {
                        addon.LastVersion = VersionData.Data[i].Version;
                        addon.ChangeLog = VersionData.Data[i].ChangeLog;
                        return;
                    }
                }
            }
            else
            {
                addon.LastVersion = "--";
            }
        }

        public StoreProduct AddonInfo(string addonName)
        {
            if (AddonsInfoData == null) return null;
            return AddonsInfoData.Products.Find(x => x.Name == addonName);
        }

        [MenuItem("MFPS/Addons/Addons Manager", false, -1000)]
        public static void Open()
        {
            GetWindow<MFPSAddonsWindow>("Addons");
        }

        [System.Serializable]
        public enum CustomWindows
        {
            Addons = 0,
            Help = 1,
            AddonsToolbar = 2,
            Support = 3,
        }
    }
#endif

    [System.Serializable]
    public class MFPSAddonsInfo
    {
        public string NiceName;
        public string KeyName;
        public string LastVersion;
        public MFPSAddon Info;
        public string CurrentVersion = "--";
        public string MinVersion;
        public string FolderName;
        public List<VersionHistory> ChangeLog = new List<VersionHistory>();

        [NonSerialized]
        public bool isInProject = false;
        [NonSerialized]
        public bool isIntegrated = false;
        [NonSerialized]
        public bool CompatibleWithThisMFPS = false;
    }

    [System.Serializable]
    public class MFPSAddonVersion
    {
        public string Name;
        public string Version;
        public List<VersionHistory> ChangeLog = new List<VersionHistory>();
    }

    [Serializable]
    public class VersionHistory
    {
        public string Version;
        [TextArea(3,10)]
        public string ChangeLog;
    }

    [Serializable]
    public class AddonsVersionList
    {
        public List<MFPSAddonVersion> Data = new List<MFPSAddonVersion>();
    }
}