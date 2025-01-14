using UnityEngine;

public class PickupManager: MonoBehaviour {
    [SerializeField] private Sprite[] sprites; //0- Health, 1- Firespeed, 2-Movespeed;
    private int pickupType;
    private SpriteRenderer sr;

    private void Awake() {
        sr=GetComponent<SpriteRenderer>();
    }
    void Start() {
        pickupType=Random.Range(0, 4);
        sr.sprite=sprites[pickupType];
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<PlayerController>(out PlayerController pController)) {
            switch(pickupType) {
                case 0:
                Debug.Log("+Health");
                break;
                case 1:
                pController.IncreaseFireSpeed(5);
                Debug.Log("+Fire speed");
                break;
                case 2:
                pController.IncreaseSpeed(1);
                Debug.Log("+Movement speed");
                break;
                case 3:
                pController.IncreaseDamage(1);
                Debug.Log("+Damage");
                break;

            }
            Destroy(gameObject);
        }
    }

}
