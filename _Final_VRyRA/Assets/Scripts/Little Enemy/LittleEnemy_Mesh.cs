using UnityEngine;

public class LittleEnemy_Mesh : MonoBehaviour
{
    [SerializeField] private GameObject mainPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainPlayer = FindAnyObjectByType<CharacterMovement>().gameObject;

        Vector3 playerPos = mainPlayer.transform.position;
        playerPos.y = transform.position.y;
    }
}
