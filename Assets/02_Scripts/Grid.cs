using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //320*657
    public int w;
    public int h;
    public int[,] gridArr;
    public float cellSize = 1f;
    public Vector3 originPosition;
    public GameObject[,] tiles;

    [Header("Tile Object Square")]
    public GameObject square;
    public Player player;

    void Awake()
    {
        Init();
        InitGridValue();
        DrawGrid();
    }
    void Init()
    {
        // (0,0) Ÿ�� ���� ��ġ
        originPosition = new Vector3(-w/2,h/2 + 20f);
        int n_gcd = GCD(h, w);

        // �ִ����� ũ��� �� Ÿ�� ũ�� ó��, ������ �� ����� ���� W, H Ÿ�� �� ���̱�
        w /= n_gcd;
        h /= n_gcd;
        cellSize *= n_gcd;
        gridArr = new int[h, w];
        tiles = new GameObject[h, w];
        square.transform.localScale = new Vector3(cellSize, cellSize);
        originPosition -= new Vector3(0, cellSize + 0.5f);
        player.n_totalTiles = w * h;
    }

    //�ִ����� --> 3�� �ǵ��� W, H �����ϸ� ������ �ӵ� �� ũ��� ���� ���� ����
    int GCD(int n, int m)
    {
        if (m == 0) return n;
        else return GCD(m, n % m);
    }

    // ���� ���
    public void DrawGrid()
    {
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                // Ÿ�� ����, Ÿ�� ���� �迭 �� ����
                tiles[i, j] = CreateTiles(GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * 0.5f, i, j);
                SetTileValue(i, j, gridArr[i, j]);
            }
        }
    }

    //�� �迭 ��ġ ��ȯ(y, x) --> (h, w)
    public Vector3 GetWorldPosition(float i, float j)
    {
        return new Vector3(j, -i, 0) * cellSize + originPosition;
    }

    // Square ������Ʈ ������ ���� ��ġ�� ����
    public GameObject CreateTiles(Vector3 localPosition, int i, int j)
    {
        GameObject obj = Instantiate(square);
        obj.name = $"Tile({i}, {j})";
        obj.transform.position = localPosition;
        return obj;

    }

    // gridArr �� ����
    public void SetTileValue(int i, int j, int val)
    {
        gridArr[i, j] = val;
        switch (val)
        {
            case 1:
                tiles[i, j].gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 2:
                tiles[i, j].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case 3:
                Color invisable = new Color(0,0,0,0);
                tiles[i, j].gameObject.GetComponent<SpriteRenderer>().color = invisable;
                player.n_territoryTiles++;
                break;
            default:
                tiles[i, j].gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                break;
        }
        
    }

    // gridArr �� �ʱ�ȭ(�׵θ� = 1, ������ = 0)
    public void InitGridValue()
    {
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {

                if (i == 0 || i == h - 1 || j == 0 || j == w - 1)
                {
                    gridArr[i, j] = 1;
                }
                else
                {
                    gridArr[i, j] = 0;
                }
            }
        }
    }

}
