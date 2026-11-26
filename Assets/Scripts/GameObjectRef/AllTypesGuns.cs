using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllTypesGuns", menuName = "Scriptable Objects/AllTypesGuns")]
public class AllTypesGuns : ScriptableObject
{
    public Dictionary<GunsArray, GunRef> AllGunArray;
    public GunRef ShotGun;
    public GunRef Automatic_rifle;
    public GunRef Handgun;

    public Dictionary<GunsArray, GunRef> GetAllGunArray()
    {
        if (AllGunArray == null)
        {
            AllGunArray = new()
            {
                { GunsArray.shotgun, ShotGun },
                { GunsArray.automatic_rifle, Automatic_rifle },
                { GunsArray.handgun, Handgun }
            };
        }

        return AllGunArray;
    }
}
