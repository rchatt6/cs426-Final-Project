///////////////////////////////////////////////////////////////////////////////////////
// bl_BodyPart.cs
//
// This script receives the information of the damage done by another player
// place it on a gameobject containing a collider in the hierarchy of the remote player
// use "bl_BodyPartManager.cs" to automatically configure                            
//                                 Lovatto Studio
///////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

public class bl_BodyPart : MonoBehaviour {

    public int HitBoxIdentifier = 0;
    public bl_PlayerHealthManager HealtScript;
    public bl_BodyPartManager BodyManager;

    /// <summary>
    /// Use this for receive damage local and sync for all other
    /// </summary>
    public void GetDamage(float damage, string t_from, DamageCause cause, Vector3 direction, int weapon_ID = 0)
    {
        float m_TotalDamage = damage * HitBox.DamageMultiplier;

        DamageData e = new DamageData();
        e.Damage = m_TotalDamage;
        e.Direction = direction;
        e.Cause = cause;
        e.isHeadShot = HitBox.Bone == HumanBodyBones.Head;
        e.GunID = weapon_ID;
        e.From = t_from;

        if (HealtScript != null)
        {
            HealtScript.GetDamage(e);
        }
    }

    public BodyHitBox HitBox
    {
        get
        {
            return BodyManager.GetHitBox(HitBoxIdentifier);
        }
    }
}

[System.Serializable]
public class BodyHitBox
{
    public string Name;
    public HumanBodyBones Bone;
    [Range(0.5f,10)] public float DamageMultiplier = 1.0f;
    [Header("References")]
    public Collider collider;
}