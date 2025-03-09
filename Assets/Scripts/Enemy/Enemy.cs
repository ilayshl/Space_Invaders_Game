using UnityEngine;

/// <summary>
/// Responsible for enemies in the game and dropping pickups on death.
/// SUPPOSED TO BE THE PARENT SCRIPT FOR METEOR, SWINGENEMY AND BOSS BUT CURRENTLY
/// I DON'T KNOW HOW TO USE INHERITANCE
/// </summary>
public class Enemy : MonoBehaviour
{
    /*
    public int myScore = 50;

    [SerializeField] protected float moveSpeed = 1;
    [SerializeField] protected int initialHP = 4;
    [SerializeField] private int dmg;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathParticle;

    protected int moveDir = 1;
    protected EnemyManager enemyManager;
    protected SpriteRenderer sr;
    private HitPoints hp;
    */

    /* private void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        sr = GetComponent<SpriteRenderer>();
    } */

    /* private void Start()
    {
        hp = new HitPoints(initialHP);
        enemyManager.AddToCurrentWave(this);
    } */

    /* /// <summary>
    /// Returns the damage of the enemy.
    /// </summary>
    /// <returns></returns>
    public int Damage()
    {
        return dmg;
    } */

    /* /// <summary>
    /// Sets the direction of the object by an int value- negative is left, positive is right.
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(int direction)
    {
        moveDir *= direction;
    } */

   /*  /// <summary>
    /// Makes the enemy take x damage, if dead, triggers death events.
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void OnHit(int dmg)
    {
        Debug.Log("check1");
        this.hp.LoseHealth(dmg);
        if (hp.IsDead())
        {
            OnDeath();
        }
        else
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            enemyManager.PlaySound(hitSounds[randomIndex]);
        }
        /*if (hp.Health() == 1)
        {
            if (sr.color == Color.green) //If enemy is Berserker
            {
                float berserkerBonus = 1.5f;
                moveSpeed *= berserkerBonus;
            }
        }
    }*/

    /* protected virtual void OnDeath()
    {
        enemyManager.PlaySound(deathSound);
        enemyManager.EnemyKilled(this);
        enemyManager.AddScore(myScore);
        enemyManager.SpawnDeathParticles(this.transform, deathParticle);
        Destroy(gameObject);
    } */

    /* /// <summary>
    /// Adds moveSpeed to the enemy.
    /// </summary>
    /// <param name="addition"></param>
    public virtual void AddSpeed(float addition)
    {
        this.moveSpeed += addition;
    } */

}
