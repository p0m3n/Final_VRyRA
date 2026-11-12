//using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class Runes : MonoBehaviour
{
    public UnityEvent uEvent;

    [SerializeField] private Renderer runeRenderer;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Color newRuneColor;
    [SerializeField] private float channelR, channelG, channelB;

    private void Start()
    {
        runeRenderer = GetComponent<Renderer>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = true;
    }

    public void StoneShoted()
    {
        channelR = 1f;
        channelG = 1f;
        channelB = 1f;

        newRuneColor = new Color(channelR, channelG, channelB);

        runeRenderer.material.SetColor("_Color", newRuneColor);

        boxCollider.enabled = false;
    }
}
