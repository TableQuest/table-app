using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thanks Tarodev for this awesome tutorial https://www.youtube.com/watch?v=kkAjpQAM-jE

public class Tile : MonoBehaviour
{
    [SerializeField] private int width = 80;
    [SerializeField] private int height = 72;
    [SerializeField] private Color firstColor, secondColor;
    [SerializeField] private SpriteRenderer tileRenderer;
    [SerializeField] private GameObject highlighter;

    public void Init(bool isOffset) {
        tileRenderer.color = isOffset ? secondColor : firstColor;
    }

    void OnMouseEnter() {
        highlighter.SetActive(true);
    }

    void OnMouseExit() {
        highlighter.SetActive(false);
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }
}
