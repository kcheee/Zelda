using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeParticles : MonoBehaviour
{
    public float scaleMultiplier = 2f;

    private void Start()
    {
        // 부모 오브젝트의 파티클 시스템을 크기 조정합니다.
        var parentParticleSystem = GetComponent<ParticleSystem>();
        if (parentParticleSystem != null)
        {
            var parentMain = parentParticleSystem.main;
            parentMain.startSizeMultiplier *= scaleMultiplier;
            parentMain.startSpeedMultiplier *= scaleMultiplier;
            parentMain.startLifetimeMultiplier *= Mathf.Pow(scaleMultiplier, 0.5f);
        }

        // 자식 오브젝트들의 파티클 시스템을 크기 조정합니다.
        var childParticleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var childParticleSystem in childParticleSystems)
        {
            var childMain = childParticleSystem.main;
            childMain.startSizeMultiplier *= scaleMultiplier;
            childMain.startSpeedMultiplier *= scaleMultiplier;
            childMain.startLifetimeMultiplier *= Mathf.Pow(scaleMultiplier, 0.5f);
        }
    }
}