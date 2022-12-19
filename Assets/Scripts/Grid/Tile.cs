using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thanks Tarodev for this awesome tutorial https://www.youtube.com/watch?v=kkAjpQAM-jE

public class Tile : MonoBehaviour
{
    public static int WIDTH = 77;
    public static int HEIGHT = 77;
    
    [SerializeField] private int width = 77;
    [SerializeField] private int height = 77;
    [SerializeField] private Color firstColor, secondColor;
    [SerializeField] public SpriteRenderer tileRenderer;
    [SerializeField] private GameObject highlighter;

    public Vector2 tilePos;

    private Color _baseColor;
    
    public void Init(bool isOffset) {
        tileRenderer.color = isOffset ? secondColor : firstColor;
        _baseColor = tileRenderer.color;
    }

    public void Highlight(Color color)
    {
        tileRenderer.color = color;
    }
    
    public void PaintBaseColor()
    {
        tileRenderer.color = _baseColor;
    }
    
    void OnMouseEnter() {
        // highlighter.SetActive(true);
    }

    void OnMouseExit() {
        // highlighter.SetActive(false);
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }
}
