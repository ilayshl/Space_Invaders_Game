using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private AudioClip hurtSound;
    private Base playerBase;
    private SoundManager soundManager;

    private const int PLAYER_DAMAGE = 1;

    private void Awake()
    {
        playerBase = FindFirstObjectByType<Base>();
        soundManager = GetComponentInParent<SoundManager>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            OnHit(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<BossCollider>(out BossCollider boss))
        {
            boss.OnHit(PLAYER_DAMAGE);
            OnHit(PLAYER_DAMAGE);
        }
        else if (other.TryGetComponent<SwingEnemy>(out SwingEnemy enemy))
        {
            enemy.OnHit(PLAYER_DAMAGE);
            OnHit(PLAYER_DAMAGE);
        }
        else if(other.TryGetComponent<Meteor>(out Meteor meteor))
        {
            meteor.OnHit(PLAYER_DAMAGE);
            OnHit(PLAYER_DAMAGE);
        }
    }

    public void OnHit(int damage)
    {
        playerBase.ChangeHealth(-damage);
        soundManager.PlaySound(hurtSound);
    }
}
