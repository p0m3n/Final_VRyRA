using UnityEngine;

public class Rune_Pink : Runes
{
    void OnTriggerEnter(Collider other)
    {
        Bullet projectile_Pink = other.GetComponent<Bullet>();
        if (projectile_Pink != null && projectile_Pink.type == "pink")
        {
            StoneShoted();
        }
    }
}
