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
        // (0,0) 타일 시작 위치
        originPosition = new Vector3(-w/2,h/2 + 20f);
        int n_gcd = GCD(h, w);

        // 최대공약수 크기로 각 타일 크기 처리, 동일한 맵 사이즈를 위해 W, H 타일 수 줄이기
        w /= n_gcd;
        h /= n_gcd;
        cellSize *= n_gcd;
        gridArr = new int[h, w];
        tiles = new GameObject[h, w];
        square.transform.localScale = new Vector3(cellSize, cellSize);
        originPosition -= new Vector3(0, cellSize + 0.5f);
        player.n_totalTiles = w * h;
    }

    //최대공약수 --> 3이 되도록 W, H 설정하면 적당한 속도 및 크기로 게임 진행 가능
    int GCD(int n, int m)
    {
        if (m == 0) return n;
        else return GCD(m, n % m);
    }

    // 격자 출력
    public void DrawGrid()
    {
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                // 타일 생성, 타일 상태 배열 값 지정
                tiles[i, j] = CreateTiles(GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * 0.5f, i, j);
                SetTileValue(i, j, gridArr[i, j]);
            }
        }
    }

    //각 배열 위치 반환(y, x) --> (h, w)
    public Vector3 GetWorldPosition(float i, float j)
    {
        return new Vector3(j, -i, 0) * cellSize + originPosition;
    }

    // Square 오브젝트 복사해 지정 위치에 생성
    public GameObject CreateTiles(Vector3 localPosition, int i, int j)
    {
        GameObject obj = Instantiate(square);
        obj.name = $"Tile({i}, {j})";
        obj.transform.position = localPosition;
        return obj;

    }

    // gridArr 값 변경
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

    // gridArr 값 초기화(테두리 = 1, 나머지 = 0)
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
