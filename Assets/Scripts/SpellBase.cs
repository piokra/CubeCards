using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBase : MonoBehaviour
{
    public CubeObject Caster;
    public Vector3 Origin;
    public Vector3 Direction;
    public Vector3 CastPoint;
   
    
    public void InitSpell(CubeObject caster, Vector3 castPoint)
    {
        if (!caster) return;
        Caster = caster;
        Origin = caster.transform.position;
        Direction = castPoint - Origin;
        Direction.z = 0;
        Direction = Direction.normalized;
        CastPoint = castPoint;
    }
}
