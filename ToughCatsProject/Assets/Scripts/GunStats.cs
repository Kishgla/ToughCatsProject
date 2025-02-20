using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create new gun")]
public class GunStats : ScriptableObject
{
    [Header("----- Weapon Attributes -----")]
    [Range(2, 300)] public int shootDistance;
    [Range(0.1f, 3)] public float shootRate;
    [Range(1, 10)] public int shootDamage;
    [Range(1, 25)] public int magazineSize;
    [Range(1, 10)] public int numOfMag;
    [Range(0.1f, 3)] public float reloadSpeed;


    [Header("----- Components -----")]
    public GameObject model;
    public GameObject hitEffect;
    public GameObject muzzleFlash;

    [Header("----- Audio -----")]
    public AudioClip gunShotAud;
    [Range(0, 1)] public float gunShotAudVol;
}
