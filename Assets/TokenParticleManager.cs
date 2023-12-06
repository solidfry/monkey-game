using System;
using Enums;
using UnityEngine;

public class TokenParticleManager : MonoBehaviour
{
    public static Action<Vector2, ParticleType> OnTokenParticleSpawn;
    public ParticleSystem spawnParticle;

    private void OnEnable()
    {
        OnTokenParticleSpawn += SpawnTokenParticle;
    }
    
    private void OnDisable()
    {
        OnTokenParticleSpawn -= SpawnTokenParticle;
    }

    private void SpawnTokenParticle(Vector2 position, ParticleType particleType)
    {
        ParticleSystem particle = Instantiate(spawnParticle, position, Quaternion.identity);
        particle.Play();
        Destroy(particle.gameObject, particle.main.duration);
    }
   
}
