using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for boss behavior. 
/// </summary>
// For now, it has no hitbox/collider and most of its attacks don't work because of that, 
// but it's just the foundation of its behavior.
// I'm having so much fun planning and coding it,
// and would apprecite your feedback on how it's played so far.
public class BossManager : MonoBehaviour
{
    public HitPoints hp = new HitPoints();
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Transform shootTransform;
    private Transform playerTransform;
    private BossStates _state = new BossStates();
    private Shoot _shoot;
    private Vector2 startingPosition;
    private int moveDir = 1;

    private WaveManager waveManager;

    private const int BOSS_HEALTH = 100;
    private const float EDGEX = 7.5f;

    void Awake()
    {
        playerTransform = FindFirstObjectByType<PlayerController>().transform;
        waveManager = FindFirstObjectByType<WaveManager>();
        _shoot = GetComponent<Shoot>();
    }

    void Start()
    {
        InitiateBoss(1);
    }

    /// <summary>
    /// For the pre-split boss- initiate the fight.
    /// </summary>
    /// <param name="healthMultiplier"></param>
    public void InitiateBoss(int healthMultiplier)
    {
        startingPosition = transform.position;
        hp.SetHealth(healthMultiplier * BOSS_HEALTH);
        transform.position += (Vector3)new Vector2(0, 10);
        ChangeState(BossStates.Intro);
    }

    /// <summary>
    /// Changes state, decided by the argument.
    /// </summary>
    /// <param name="state"></param>
    // The argument of BossStates is then converted to a string in order to call a corresponding coroutine.
    private void ChangeState(BossStates state = BossStates.Swing)
    {
        this._state = state;
        StartCoroutine(state.ToString());
        Debug.Log(state.ToString());
    }

    /// <summary>
    /// Chooeses a random attack from the pre-set indexes of BossStates.
    /// </summary>
    private void RandomAttack()
    {
        int randomIndex = Random.Range(3, 7);
        ChangeState((BossStates)randomIndex);
    }

    /// <summary>
    /// Takes the boss down from outside the screen to its starting point.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Intro()
    {
        Vector2 currentPosition = transform.position;
        while (Vector2.Distance(currentPosition, startingPosition) > 0.05f)
        {
            currentPosition = Vector2.Lerp(currentPosition, startingPosition, moveSpeed * Time.deltaTime);
            transform.position = currentPosition;
            yield return null;
        }
        ChangeState(BossStates.Swing);
    }

    private IEnumerator Swing()
    {
        int randomTime = Random.Range(3, 6);
        float timePassed = 0;
        while (_state == BossStates.Swing && timePassed < randomTime)
        {
            transform.position += new Vector3(moveDir * moveSpeed * Time.deltaTime, 0, 0);
            CheckForScreenEdges();
            timePassed += Time.deltaTime;
            yield return null;
        }
        RandomAttack();
    }

    /// <summary>
    /// Returns the object to startingPoint.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Return()
    {
        Vector2 currentPosition = transform.position;
        while (Vector2.Distance(currentPosition, startingPosition) > 0.05f)
        {
            currentPosition = Vector2.Lerp(currentPosition, startingPosition, moveSpeed * Time.deltaTime);
            transform.position = currentPosition;
            yield return null;
        }
        ChangeState();
    }

    /// <summary>
    /// Shots random amount of projectile waves at the player.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(.5f);
        int random = Random.Range(1, 4);
        for (int i = 0; i < random; i++)
        {
            ShootProjectile();
            yield return new WaitForSeconds(0.75f);
        }
        ChangeState();
    }

    /// <summary>
    /// Quickly charges towards the player position, set when the coroutine starts.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Charge()
    {
        Vector2 targetPosition = playerTransform.position;
        Vector2 currentPosition = transform.position;
        while (Vector2.Distance(currentPosition, targetPosition) > 0.1f)
        {
            currentPosition = Vector2.Lerp(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
            transform.position = currentPosition;
            yield return null;
        }
        ChangeState(BossStates.Return);
    }

    /// <summary>
    /// Goes up, then sweeps at the player's y location for a random amount of times.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Sweep()
    {
        yield return StartCoroutine(GoUp());
        int random = Random.Range(1, 3);
        Vector2 offset = new Vector3(16, 0);
        for (int i = 0; i < random; i++)
        {
            Vector2 targetPosition = (Vector2)playerTransform.position + (offset * moveDir);
            Vector2 currentPosition = (Vector2)playerTransform.position + (-offset * moveDir);
            while (Vector2.Distance(currentPosition, targetPosition) > 0.05f)
            {
                currentPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime * 7);
                transform.position = currentPosition;
                yield return null;
            }
            ChangeDirection();
        }
        ChangeState(BossStates.Return);
    }

    /// <summary>
    /// Spawns a wave of random enemies, including meteors (once I'll understand how to work with Inheritance).
    /// </summary>
    /// <returns></returns>
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(.5f);
        int random = Random.Range(1, 4);
        for (int i = 0; i < random; i++)
        {
            Vector3 spawnPosition = transform.position - new Vector3(0, 1.2f, 0);
            waveManager.SpawnEnemy(null, spawnPosition, moveDir);
            yield return new WaitForSeconds(1.5f);
        }
        ChangeState();
    }

    /// <summary>
    /// When reaching <50% health, splits into 2 bosses with smaller hitboxes.
    // These bosses have no intro animation and will attack independantly.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Split()
    {
        yield return new WaitForSeconds(3);
        ChangeState();
    }

    /// <summary>
    /// Goes up, outside of the camera bounds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoUp()
    {
        Vector2 targetPosition = transform.position + new Vector3(0, 4, 0);
        Vector2 currentPosition = transform.position;
        //Goes up from the screen
        while (Vector2.Distance(currentPosition, targetPosition) > 0.05f)
        {
            currentPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime * 3);
            transform.position = currentPosition;
            yield return null;
        }
    }

    /// <summary>
    /// Checks if x position is touching the screen edges.
    /// </summary>
    private void CheckForScreenEdges()
    {
        if ((transform.position.x >= EDGEX && moveDir > 0)
        || (transform.position.x <= -EDGEX && moveDir < 0))
        {
            ChangeDirection();
        }
    }

    /// <summary>
    /// Flips direction of movement.
    /// </summary>
    private void ChangeDirection()
    {
        moveDir *= -1;
    }

    /// <summary>
    /// Resets the object's rotation to its default.
    /// </summary>
    private void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
    }

   private void ShootProjectile()
   {
    _shoot.ShootBullet(shootTransform.position);
   }

   public void OnDeath()
   {
    Debug.Log("boss is dead");
    hp.SetHealth(100);
   }
}
