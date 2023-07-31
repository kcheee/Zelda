using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeParticles : MonoBehaviour
{
    public float scaleMultiplier = 2f;

    private void Start()
    {
        // �θ� ������Ʈ�� ��ƼŬ �ý����� ũ�� �����մϴ�.
        var parentParticleSystem = GetComponent<ParticleSystem>();
        if (parentParticleSystem != null)
        {
            var parentMain = parentParticleSystem.main;
            parentMain.startSizeMultiplier *= scaleMultiplier;
            parentMain.startSpeedMultiplier *= scaleMultiplier;
            parentMain.startLifetimeMultiplier *= Mathf.Pow(scaleMultiplier, 0.5f);
        }

        // �ڽ� ������Ʈ���� ��ƼŬ �ý����� ũ�� �����մϴ�.
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