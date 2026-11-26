using UnityEngine;

[CreateAssetMenu(fileName = "GunRef", menuName = "Scriptable Objects/GunRef")]
public class GunRef : ScriptableObject
{
    public GunsArray gunType;
    public float damage;
    public float recoilCam;
    public float recoilMove;
    public float magazineBulletNumber;
    public float maxDistanceBulloet;
}
