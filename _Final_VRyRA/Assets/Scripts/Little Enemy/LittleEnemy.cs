using UnityEngine;

public class LittleEnemy : MonoBehaviour
{
    [SerializeField] private GameObject mainPlayer;
    public BulletPooling enemy_Bullet;
    public Transform shootingPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainPlayer = FindAnyObjectByType<CharacterMovement>().gameObject;
        //gameObject.transform.rotation = Quaternion.Euler(0,180,0);
        //detectCollider = GetComponent<SphereCollider>();
    }

    public void LookPlayer()
    {
        Vector3 playerPos = mainPlayer.transform.position;
        playerPos.y = transform.position.y;
        enemy_Bullet.GetObject(shootingPoint);
        gameObject.transform.LookAt(playerPos);
    }

    public void StopLookingPlayer()
    {
        gameObject.transform.LookAt(Vector3.zero);
    }
}
