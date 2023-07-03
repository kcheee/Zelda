using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// monohavior 보다 경량화된 스크립트.
// 자세한 설명은 노션에 있음.
public abstract class ItemEffect : ScriptableObject
{
    public abstract bool ExecuteRole();
}
