using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds current wave data and enemy events.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject pickup;
    private List<GameObject> currentWave = new List<GameObject>();
    private WaveManager waveManager;
    private SoundManager soundManager;

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
        soundManager = GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) //Meant for play-testing
        {
            SpawnPickup(this.transform);
        }
    }

    //Removes the given object from the current wave list.
    private void RemoveFromCurrentWave(GameObject enemy)
    {
        if (currentWave.Contains(enemy))
        {
            currentWave.Remove(enemy);
        }
        if (currentWave.Count <= 0)
        {
            waveManager.StartNextWaveSequence();
        }
    }

    /// <summary>
    /// Add a GameObject to the current wave list.
    /// </summary>
    /// <param name="enemy"></param>
    public void AddToCurrentWave(GameObject enemy)
    {
        if (!currentWave.Contains(enemy))
        {
            currentWave.Add(enemy);
        }
    }

    /// <summary>
    /// Basic enemy killed- rolls for Pickup spawn after death.
    /// </summary>
    /// <param name="enemyTransform"></param>
    public void EnemyKilled(GameObject enemy)
    {
        if (Random.Range(0, 100) <= 20)
        {
            SpawnPickup(enemy.transform);
        }
        RemoveFromCurrentWave(enemy);
    }

    /// <summary>
    /// Meteor enemy destroyed- if it was a variant, spawns a corresponding pickup.
    /// </summary>
    /// <param name="meteorTransform"></param>
    /// <param name="meteorType"></param>
    public void MeteorDestroyed(GameObject meteor, int meteorType)
    {
        if (meteorType > 0)
        {
            var pickupSpawned = SpawnPickup(meteor.transform);
            PickupManager pickupManager = pickupSpawned.GetComponent<PickupManager>();
            pickupManager.SetPickupType(meteorType);
        }
        RemoveFromCurrentWave(meteor);
    }

    //Spawns a pickup in the given position and returns itself as a variable.
    private GameObject SpawnPickup(Transform transform)
    {
        return Instantiate(pickup, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Spawns GameObject at given transform.position and destroys it after.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="particleSystem"></param>
    public void SpawnDeathParticles(Transform enemy, GameObject particleSystem)
    {
        var particles = Instantiate(particleSystem, enemy.position, Quaternion.identity);
        Destroy(particles.gameObject, 2f);
    }

    /// <summary>
    /// Plays an AudioClip via SoundManager.
    /// </summary>
    /// <param name="sound"></param>
    public void PlaySound(AudioClip sound)
    {
        soundManager.PlaySound(sound);
    }
}
