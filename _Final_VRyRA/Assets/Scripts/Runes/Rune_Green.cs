using UnityEngine;

public class Rune_Green : Runes
{
    void OnTriggerEnter(Collider other)
    {
        Bullet projectile_Green = other.GetComponent<Bullet>();
        if (projectile_Green != null && projectile_Green.type == "green")
        {
            StoneShoted();
        }
    }
}
