using NUnit.Framework;
using UnityEngine;

public class Runes : MonoBehaviour
{
    [SerializeField] private Renderer runeRenderer;
    [SerializeField] private Color newRuneColor;
    [SerializeField] private float channelR, channelG, channelB;

    private void Start()
    {
        runeRenderer = GetComponent<Renderer>();
    }

    public void StoneShoted()
    {
        channelR = 1f;
        channelG = 1f;
        channelB = 1f;

        newRuneColor = new Color(channelR, channelG, channelB);

        runeRenderer.material.SetColor("_Color", newRuneColor);
    }
}
