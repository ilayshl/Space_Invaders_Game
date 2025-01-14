using UnityEngine;

/// <summary>
/// Responsible for enemies in the game and dropping pickups on death.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    private float xEdge = 7.5f;
    private int moveDir = 1;
    private HitPoints hp;
    private WaveManager waveManager;
    private SpriteRenderer sr;
    private Animator animator;

    private void Awake()
    {
        hp = GetComponent<HitPoints>();
        waveManager = GetComponentInParent<WaveManager>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //IncreaseDifficulty();
        Move();
        CheckForScreenEdges();
    }

    private void Move()
    {
        transform.position += new Vector3(moveDir * moveSpeed * Time.deltaTime, 0, 0);
    }
    public void SetSpeed(float addition) {
        moveSpeed+=addition;
    }

    private void CheckForScreenEdges()
    {
        if ((transform.position.x >= xEdge && moveDir > 0)
        || (transform.position.x <= -xEdge && moveDir < 0))
        {
            RowDown();
        }
    }

    private void RowDown()
    {
        moveDir *= -1;
        transform.position = new Vector3(transform.position.x, transform.position.y - 1f, 0);
    }
    
    public void OnHit(int dmg)
    {
        hp.LoseHealth(dmg);
        animator.SetTrigger("onHit");
        if (hp.IsDead())
        {
            waveManager.EnemyKilled(this.transform);
            Destroy(gameObject);
        }
        if(hp.Health() == 1){
        if(sr.color == Color.green){
            moveSpeed*=2;
        }
        }
    }
}
