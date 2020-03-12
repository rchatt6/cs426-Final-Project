using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MFPSEditor;

public class AddWeaponTutorial : TutorialWizard
{
    //required//////////////////////////////////////////////////////
    private const string ImagesFolder = "mfps2/editor/";
    private NetworkImages[] ServerImages = new NetworkImages[]
    {
        new NetworkImages{Name = "img-1.jpg", Image = null},
        new NetworkImages{Name = "img-2.jpg", Image = null},
        new NetworkImages{Name = "img-3.jpg", Image = null},
        new NetworkImages{Name = "img-4.jpg", Image = null},
        new NetworkImages{Name = "img-0.jpg", Image = null},
        new NetworkImages{Name = "img-5.png", Image = null},
        new NetworkImages{Name = "img-6.jpg", Image = null},
        new NetworkImages{Name = "img-7.jpg", Image = null},
        new NetworkImages{Name = "img-8.jpg", Image = null},
        new NetworkImages{Name = "img-9.jpg", Image = null},
        new NetworkImages{Name = "img-10.jpg", Image = null},
        new NetworkImages{Name = "img-11.jpg", Image = null},
        new NetworkImages{Name = "img-12.jpg", Image = null},
        new NetworkImages{Name = "img-13.jpg", Image = null},
        new NetworkImages{Name = "img-14.jpg", Image = null},
        new NetworkImages{Name = "img-15.jpg", Image = null},
        new NetworkImages{Name = "img-16.jpg", Image = null},
        new NetworkImages{Name = "img-17.jpg", Image = null},
        new NetworkImages{Name = "img-18.jpg", Image = null},
        new NetworkImages{Name = "img-19.jpg", Image = null},
        new NetworkImages{Name = "img-20.jpg", Image = null},
        new NetworkImages{Name = "img-21.jpg", Image = null},
        new NetworkImages{Name = "img-22.jpg", Image = null},
         new NetworkImages{Name = "img-23.jpg", Image = null},
    };
    private Steps[] AllSteps = new Steps[] {
     new Steps { Name = "Weapon Model", StepsLenght = 0 },
    new Steps { Name = "Create Info", StepsLenght = 3 },
    new Steps { Name = "FPV Weapon", StepsLenght = 9 },
    new Steps { Name = "TPV Weapon", StepsLenght = 3 },
    new Steps { Name = "PickUp Prefab", StepsLenght = 2 },
    };
    //final required////////////////////////////////////////////////

    private GameObject PlayerInstantiated;
    private int animationType = 0;

    public override void OnEnable()
    {
        base.OnEnable();
        base.Initizalized(ServerImages, AllSteps, ImagesFolder);
        GUISkin gs = Resources.Load<GUISkin>("content/MFPSEditorSkin") as GUISkin;
        if (gs != null)
        {
            base.SetTextStyle(gs.customStyles[2]);
        }
    }

    public override void WindowArea(int window)
    {
        if (window == 0)
        {
            DrawWeaponModel();
        }
        else if (window == 1)
        {
            DrawCreateInfo();
        }
        else if (window == 2)
        {
            DrawFPWeapon();
        }
        else if (window == 3)
        {
            DrawTPWeapon();
        }
        else if (window == 4)
        {
            DrawPickUpPrefab();
        }
    }

    void DrawWeaponModel()
    {
        if (subStep == 0)
        {
            DrawText("In order to add a new weapon you need of course a Weapon 3D model. Now, there are a few variables. Some just replace the weapon model and use the MFPS default hands model and animations." +
                " Although this is not prohibited or wrong, it definitely is not the best solution because the hand model and animations of MFPS are placeholders as examples only. It is highly recommended that you use " +
                "your own models and animations. To do this, follow this tutorial:");

            DrawText("To begin, you need a 3D Animated Weapon model (include the hands). You also need 4 animations: TakeIn, TakeOut, Fire and Reload,\n" +
                "the animations should be in 'Legacy' mode to work with 'Animation' Component.\nBelow you will find a link to a list with some Asset Store packages of weapons models that meet the requirements" +
                "to use in MFPS");
            GUILayout.Space(5);
            if (GUILayout.Button("<color=yellow>Weapon Assets Packs</color>", EditorStyles.label))
            {
                Application.OpenURL("https://www.lovattostudio.com/en/weapon-packs-for-mfps/");
            }
        }
    }

    void DrawCreateInfo()
    {
        if (subStep == 0)
        {
            DrawText("The first step to add a new weapon is create the weapon info,\n for it you need go to the <b>GameData</b> and add a new field in\n the 'AllWeapons' list.");
            GUILayout.Space(10);
            if (DrawButton("Open GameData"))
            {
                bl_GameData gm = bl_GameData.Instance;
                Selection.activeObject = gm;
                EditorGUIUtility.PingObject(gm);
                subStep++;
            }
        }
        else if (subStep == 1)
        {
            DrawText("Now the GameData should be open in the inspector window,\n-There in the inspector of GameData in the bottom you will see a 'Weapon' section with a 'AllWeapons' list, open it and <b>Add</b> a new field to the list. \n<i>(use the button bellow)</i>");
            if (DrawButton("Add Field Automatically"))
            {
                bl_GameData gm = bl_GameData.Instance;
                Selection.activeObject = gm;
                EditorGUIUtility.PingObject(gm);
                bl_GunInfo info = new bl_GunInfo();
                info.Name = "New Weapon";
                gm.AllWeapons.Add(info);
                subStep++;
            }
        }
        else if (subStep == 2)
        {
            DrawText("Now in the 'AllWeapons' list you should see a new field called <b>'New Weapon'</b>, open it and fill the information as required by the type of weapon you are adding.");
            DownArrow();
            DrawPropertieInfo("Name", "string", "The name of this weapon, use a Unique name for each weapon so you can easily identified they.");
            DrawPropertieInfo("Type", "enum", "The type of this weapon, is a sniper, rifle, knife, etc...");
            DrawPropertieInfo("Damage", "int", "The amount of damage that will cause this weapon with every hit.");
            DrawPropertieInfo("Fire Rate", "float", "Minimum time between shots.");
            DrawPropertieInfo("Reload Time", "float", "Time that take reload the weapon.");
            DrawPropertieInfo("Range", "int", "The maximum distance that the bullet of this weapon can travel (and hit something) before get destroyed.");
            DrawPropertieInfo("Accuracy", "int", "The spread of the bullet.");
            DrawPropertieInfo("Weight", "int", "The 'weight' of this weapon, the weight affect the player speed when this gun is enabled");
            DrawPropertieInfo("Pick Up Prefab", "bl_GunPickUp", "The pick up prefab of this weapon, leave empty at the moment, you will set up later in this tutorial.");
            DrawPropertieInfo("Gun Icon", "Sprite", "A sprite icon that represent this weapon.");
            DownArrow();
            DrawImage(GetServerImage(4));
            GUILayout.Label("With this you have setup the weapon info, you're ready for the next step.");
        }
    }

    void DrawFPWeapon()
    {
        if (subStep == 0)
        {
            GUILayout.Label("Okay, to proceed with this step, lets open a new empty scene \nto make things more clear: File -> New Scene.", EditorStyles.miniLabel);
            GUILayout.Space(10);
            DrawText("Then, drag one of the player prefabs to the hierarchy.");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Drag: ", GUILayout.Width(50));
            if (DrawButton("Player 1"))
            {
                PlayerInstantiated = PrefabUtility.InstantiatePrefab(bl_GameData.Instance.Player1) as GameObject;
#if UNITY_2018_3_OR_NEWER
        PrefabUtility.UnpackPrefabInstance(PlayerInstantiated, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
#endif
                Selection.activeObject = PlayerInstantiated;
                EditorGUIUtility.PingObject(PlayerInstantiated);
                subStep++;
            }
            GUILayout.Label("Or", GUILayout.Width(25));
            if (DrawButton("Player 2"))
            {
                PlayerInstantiated = PrefabUtility.InstantiatePrefab(bl_GameData.Instance.Player2) as GameObject;
#if UNITY_2018_3_OR_NEWER
        PrefabUtility.UnpackPrefabInstance(PlayerInstantiated, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
#endif
                Selection.activeObject = PlayerInstantiated;
                EditorGUIUtility.PingObject(PlayerInstantiated);
                subStep++;
            }
            GUILayout.EndHorizontal();
        }
        else if (subStep == 1)
        {
            DrawText("Now go to where all the first person weapons are, under the WeaponManager object inside of the player prefab:" +
               " <i>Player -> Local -> Mouse -> Animations -> Main Camera -> WeaponCamera -> TilEffect -> WeaponsManager -> *</i>");
            if (DrawButton("Try open automatically"))
            {
                bl_PlayerNetwork player = FindObjectOfType<bl_PlayerNetwork>();
                if (player != null)
                {
                    bl_GunManager gm = player.transform.GetComponentInChildren<bl_GunManager>();
                    Selection.activeObject = gm;
                    EditorGUIUtility.PingObject(gm);
                    subStep++;
                }
            }
        }
        else if (subStep == 2)
        {
            DrawText("Now under the 'WeaponManager' object you will have all first person weapons already set up. So to save some work, we'll duplicate one of these to modify with the new weapon model," +
                 " so the duplicated one is of the same type. For example, if your new weapon is a sniper rifle, duplicate a sniper weapon.");
            DrawText("To duplicate, simply select the weapon in hierarchy -> Right Mouse Click -> Duplicate.");
            DrawImage(GetServerImage(0));
        }
        else if (subStep == 3)
        {
            DrawText("Drag your weapon model and put inside of the duplicated weapon. Don't delete the old model yet, just disable it for the moment and position your new weapon model as you want as default position. " +
                "\n\n Then when you have it positioned, select the root of your weapon model and go to the 'Layer' list (on top of the inspector window) and change the layer to <b>'Weapons'</b> and apply to all children.");
            DrawImage(GetServerImage(1));
        }
        else if (subStep == 4)
        {
            DrawText("Now, select the top of the duplicated weapon (where bl_Gun is) and click one time in the 'FirePoint' value (not the property name). With this, the hierarchy will foldout to where the 'FirePoint'" +
                 " and 'Muzzleflash' objects are located inside the old model. Select them (FirePoint, Muzzleflash and CartridgeEjectEffect) and put inside of your new model. Position the objects correctly (FirePoint " +
                 "and Muzzleflash on the end of the weapon barrel) and then delete the old model.");
            DownArrow();
            DrawImage(GetServerImage(2));
        }
        else if (subStep == 5)
        {
            DrawText("Select the top of new weapon model where 'Animation' or 'Animator' component is attached and add the script <b>'bl_WeaponAnimation'</b> (click the inspector button 'Add Component' and write bl_WeaponAnimation and click it).");
            DownArrow();
            if (animationType == 0)
            {
                DrawText("Now select which Animation system are your weapon model using:");
                Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Animation (Legacy)", EditorStyles.toolbarButton))
                {
                    animationType = 1;
                }
                GUILayout.Space(2);
                if (GUILayout.Button("Animator (Mecanim)", EditorStyles.toolbarButton))
                {
                    animationType = 2;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else if (animationType == 1)
            {
                DrawText("Now in the inspector of the script you will need assign the respective animations of the weapon model\n\n-Draw = TakeIn\n-Hide = Take Out\n-Fire Aim: this can be the same as normal fire " +
                   "animation but it is recommended that you use an animation with low kick back movement.\n-In case that the weapon is a Shotgun or Sniper and this have a split reload animation (Start, Insert Bullet and Finish)" +
                   " Simple check the 'SplitReloadAnimation' in bl_Gun and assign the animations in bl_WeaponAnimation script." +
                   "\n\n<b>NOTE:</b> All animations that are assigned in bl_WeaponAnimation should be listed in the 'Animations' list " +
                   "of the 'Animation' Component");
                DownArrow();
                DrawImage(GetServerImage(3));
            }
            else if (animationType == 2)
            {
                DrawText("Okay, so first make sure that the Animator component doesn't have a <b>Controller</b> assigned yet. If it already has one, remove it.\n \n" +
                    " Now in the bl_WeaponAnimation -> AnimationType, select <b>Animator</b>, you will see some empty Animation clip fields. In these, you have to assign the respective animations of the weapon model\n\n-Draw = TakeIn\n-Hide = Take Out\n-Fire Aim: This can be the same as normal fire" +
                    " animation but it is recommended that you use an animation with low kick back movement.\n-In case that the weapon is a Shotgun or Sniper and this have a split reload animation (Start, Insert Bullet and Finish)" +
                    " Simply check the 'SplitReloadAnimation' in bl_Gun and assign the animations in bl_WeaponAnimation script.\n \n" +
                    "when you have assigned all required animations, press the button <b>SetUp</b> -> a Window will open, select a folder in the project to save the Animator Controller.");
                Space(5);
                DrawImage(GetServerImage(23));
            }
        }
        else if (subStep == 6)
        {
            DrawText("Now some weapons packages comes with a 'Walk' and 'Run' animations, but in MFPS these movements are procedurally generated (by code) so you don't need these animations." +
                " What you need to do now is add the script <b>bl_WeaponMovement.cs</b> in the same object where you added the last script bl_WeaponAnimation.");
            DownArrow();
            DrawText("Then copy the current Transform Values of the weapon model.");
            DownArrow();
            DrawImage(GetServerImage(5));
            DownArrow();
            DrawText("Now position <i>(move and rotate)</i> the weapon model in the editor view simulating where will be when the player is running, like this:");
            DrawImage(GetServerImage(6));
            DownArrow();
            DrawText("When you have it set, go to the bl_WeaponMovements inspector and click on the first button <b>Get Actual Position</b>, that will get the current transform values and copy in the " +
                "respectively script values automatically.");
            DrawImage(GetServerImage(7));
            DrawText("Then do the same for the 'Run and Reload Position'. You can use the same position as normal run. Just click on the 'Get Actual Position' button.");
            DownArrow();
            DrawText("Now go again to the Transform inspector, open the Context menu and click on <b>Paste Component Values</b>, that will make the transform back to the default position.");
            DrawImage(GetServerImage(8));
        }
        else if (subStep == 7)
        {
            DrawText("Now go to the top of the new weapon where the bl_Gun script is attached and in the inspector of the script, modify the values as necessary.");
            DrawPropertieInfo("GunID", "enum", "Then select the gun name that you set up before in GameData list, note that each weapon needs to have their own weapon info. Weapons can't share the same " +
                "GunID");
            DrawPropertieInfo("Aim Position", "Vector3", "The position where the weapon will be when player aims with this weapon, To set this up:");
            DownArrow();
            DrawImage(GetServerImage(9));
            DownArrow();
            DrawText("after clicking the button a 'crosshair' should be active and visible in the game view. This will serve as a reference in the center of the screen and store the default" +
                " position while you get the aim position.\n\nNow position the weapon (the object that have bl_Gun attached)" +
                " in the center of screen making the weapon scope/iron sight to be exactly aligned with the crosshair <b>in the Game View</b>, like this:");
            DrawImage(GetServerImage(10));
            DownArrow();
            DrawText("Once you're sure that you have the right position, click the button again, to automatically assign the Aim position and return to the default position. That's it for this part.");
            DrawImage(GetServerImage(11));
            DownArrow();
            DrawPropertieInfo("Aim Smooth", "float", "the speed of the default position to the aim position transition.");
            DrawPropertieInfo("Aim Delay Movement", "float", "the amount of the delay movement effect when is aiming.");
            DrawPropertieInfo("Aim FoV", "float", "the Field Of View (Zoom) of the camera when is aiming, less value = more zooming.");
            DrawPropertieInfo("Bullet", "string", "Here you assign the name of the pooled bullet that will shoot this weapon.");
            DrawPropertieInfo("Impact Force", "int", "Force applied to rigidbody's when the bullet impact them");
            DrawPropertieInfo("Head Shake On Fire", "float", "Intensity of the random shake movement when fire.");
            DrawPropertieInfo("Recoil", "float", "'Kickback' movement amount when fire.");
            DrawPropertieInfo("Recoil Speed", "float", "Speed with which the camera will return from the kickback.");
            DrawPropertieInfo("Auto Reload", "bool", "Reload the weapon automatically when ammo = 0?");
            DrawPropertieInfo("Split Reload Animation", "bool", "Sniper/Shotgun Only, Your weapon model have the reload animation splitted (start, insert, finish)?");
            DrawPropertieInfo("Delay Fire", "float", "On grenades only, delay time to throw the projectile since input down");
            DrawPropertieInfo("Spread", "float", "the base spread of this weapon, the bullet random propagation.");
            DrawPropertieInfo("Max Spread", "float", "the max spread, cuz the spread increase while is firing");
            DrawPropertieInfo("Spread Per Second", "float", "how much increase the spread while is firing");
            DrawPropertieInfo("Decrease Spread Per Second", "float", "how much decrease the spread per second when stop firing.");
            DrawPropertieInfo("Sound Reload By Animation", "bool", "are the reload sounds play by animation key events (manual) or by time calculation (auto)?");
            DrawPropertieInfo("OnNoAmmoDesactive", "Array", "Grenade Only, Put in the list all projectile objects that is in the hands.");
            DownArrow();
            DrawText("Finally, if this weapon is a Sniper, add the script bl_SniperScope.cs too, assign the scope texture and in the list 'OnScopeDisable' add all meshes of the sniper model including hands");

        }
        else if (subStep == 8)
        {
            DrawText("To finish with this, select the <b>WeaponManager</b> object and go to -> bl_GunManager -> GunList. In the list add a new field. Into this new field, drag the new weapon.");
            DownArrow();
            DrawImage(GetServerImage(12));
            DownArrow();
            DrawText("Now if you want assign it to as the default weapon of a player class, simply (always in bl_GunManager)" +
                " open the wanted player class section (Assault, Support, Recon or Engineer) and in the slot that you want (Primary, Secondary, Knife or Projectile) select" +
                " the name of the weapon.");
            DownArrow();
            DrawImage(GetServerImage(13));
            DownArrow();
            DrawText("Next save/apply the changes to the player prefab (Don't delete the player from scene yet, as it is still required for the next step).");
            DownArrow();
            DrawText("There you go!, you have added a new first person weapon.\n\nIf you wanna make it even better and show a menu where players can select their weapons load out between all your available weapons you can use <b>Class Customization</b> addon.");
            if (DrawButton("Class Customization"))
            {
                Application.OpenURL("http://www.lovattostudio.com/en/shop/addons/class-cutomization/");
            }
        }
    }

    void DrawTPWeapon()
    {
        if (subStep == 0)
        {
            DrawText("Each weapon has two different points of view: The 'First Person View' which is what the local player sees and 'Third Person View' which is the weapon that other" +
                " players see. In this part we're going to set up this last one, Third Person Weapon, for this, what you need is the same weapon model that you use for the FPV weapon " +
                "but without the hands, just the weapon body mesh, even though <b> it is recommended to use a more optimized / less detailed model of the weapon than the first person model.</b>");
            DrawImage(GetServerImage(14));
            DownArrow();
            DrawText("if you are following from the last step, you still should have a Player Prefab in the scene. If not, drag the player where you add the FPV Weapon to the hierarchy.\n\n" +
                "Then go to bl_PlayerSync inspector and click on one of the scripts in the list 'Network Guns'. This will foldout to where all TP Weapons are. Now drag your weapon model and " +
                "put inside of the object 'RemoteWeapons'.");
            DrawImage(GetServerImage(15));
            DownArrow();
            DrawText("Then position the weapon object to simulate how the player is holding the weapon with the hands.\n\nIf your player model is in T-Pose or in a pose where it is " +
                "difficult to position the weapon in the hands, what you can do in these cases is open the <b>Animation</b> window and select the 'idle' animation and select a frame of the animation. After that" +
                " the player will be in the animation pose so you should be able to position the gun easily.");
            DrawImage(GetServerImage(17));
        }
        else if (subStep == 1)
        {
            DrawText("Now select the weapon model (the one that you just dragged to the player before) and Add the script <b>bl_NetworkGun.cs</b>");
            DownArrow();
            DrawText("In the inspector of the script, you should see an empty field called 'Local Weapon' and a Popup with the list of all FPWeapons (bl_Gun) in this player prefab." +
                " Select (by the object name) the FPWeapon that you setup before and click on the button 'Select'.");
            DrawImage(GetServerImage(16));
            DownArrow();
            DrawText("By clicking the button, it will assign the select FPWeapon automatically and some new variables will appear in the inspector of the script:");
            DrawPropertieInfo("MuzzlefFlash", "Particle System", "Fire particle effect.");
            DownArrow();
            DrawText("So for the muzzleflash, drag your particle effect and put inside of the weapon object, if you don't have your own, you have the MFPS default located in: <i>MFPS->" +
                "Content->Prefabs->Particles->WeaponsEffects->Prefabs->MuzzleFlashEffect</i>. Drag it to the hierarchy and put it inside the weapon and position at the end of the gun barrel." +
                " Then assign in the 'MuzzleFlash' field.");
            DownArrow();
            DrawText("For Grenades, the inspector will show an extra field called 'Bullet' where you need drag the 'Grenade prefab' bullet.");

        }
        else if (subStep == 2)
        {
            DrawText("Now, in the inspector of the script you have a button called <b>'SetUp Hand IK'</b>. Click it and a small editor window will open in the " +
                "scene view. You should see a sphere gizmo selected. When you move these the left hand will follow it (with IK constrains). So positioned it (move and rotate) where " +
                "the left hand will be, for making the weapon holding appear more realistic.\n When you have positioned it, click on the button of the small window previously opened called 'DONE'.");
            DownArrow();
            DrawImage(GetServerImage(18));
            DownArrow();
            DrawText("Finally click on the inspector button 'Enlist TPWeapon'. This will automatically add the weapon to the network weapons list.");
            DrawImage(GetServerImage(19));
            DownArrow();
            DrawText("That's it! You have added the TPWeapon, don't forget to save/apply the changes to the Player prefab.");
        }
    }

    void DrawPickUpPrefab()
    {
        if (subStep == 0)
        {
            DrawText("The last thing that you need set up is the 'PickUp prefab' of the weapon. This prefab is instanced when players pick up other weapons or when a player dies with the weapon active." +
                " So what you need is the weapon model. Just like for setting up the TPWeapon, you only need the weapon model mesh without hands. Again, it is recommended that you use a low poly model for this one.");
            DownArrow();
            DrawText("Drag your weapon model into the hierarchy window (You don't need the player prefab for this one). Select it and Add these Components:\n\n-<b>RigidBody</b>\n-<b>Sphere Collider</b>: in the sphere collider check" +
                " 'IsTrigger', this collider is the area where the gun will be detected, when player enter in this, so modify the position and radius if is necessary.\n-<b>Box Collider</b>: " +
                " with 'IsTrigger' unchecked, make the Bounds of this collider fit exactly with the weapon mesh.\n\nSo the inspector should look like this:");
            DrawImage(GetServerImage(20));
            DownArrow();
            DrawText("and the Colliders Bounds should look like this: ");
            DownArrow();
            DrawImage(GetServerImage(21));

        }
        else if (subStep == 1)
        {
            DrawText("Now Add the script <b>bl_GunPickUp.cs</b> and set up the variables");
            DownArrow();
            DrawPropertieInfo("GunID", "enum", "Select the Weapon ID of this weapon, the one that you set up in GameData");
            DrawPropertieInfo("Bullets", "int", "The bullets that contains this weapon when someone pickup");
            DrawPropertieInfo("DestroyAfterTime", "bool", "Will the prefab get destroyed automatically after some time since was instantiated?");
            DownArrow();
            DrawText("Good! Now create a prefab of this weapon, simple drag the object from hierarchy to a folder in the Project Window, just remember in which folder :)");
            DownArrow();
            DrawText("Finally, Go to GameData (click the button bellow)");
            if (DrawButton("Open GameData"))
            {
                bl_GameData gm = bl_GameData.Instance;
                Selection.activeObject = gm;
                EditorGUIUtility.PingObject(gm);
            }
            DownArrow();
            DrawText("in the inspector of GameData, go to the 'AllWeapons' list and open the info of your weapon previously set up. In the field 'Pick Up Prefab' drag the weapon PickUp prefab (the one from Project folder" +
                " not from hierarchy):");
            DrawImage(GetServerImage(22));
            DownArrow();
            DrawText("There you have it! You have finished integrating the new weapon :) The first time you do this, it might seem very complicated, but believe me, it gets easier next time.");
        }
    }

    [MenuItem("MFPS/Tutorials/Add Weapon")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AddWeaponTutorial));
    }
}