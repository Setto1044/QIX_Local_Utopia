using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Grid Map GameObject's Script")]
    public Grid grid;

    [Header("Player GameObject's child Square")]
    PlayerStateImg state;
    public GameObject img_player;

    [Header("Grid Map's (i, j) location")]
    public int i;
    public int j;

    [Header("Player Square Image size")]
    public int playerSize = 3;

    public bool VillainGameOver = false;
    public bool LineGameOver = false;
    
    public bool StepWeb = false;
    public bool inMyArea = true;

    // �� ������ �� �� ���,�����ϴ� ����
    int starti = 0;
    int startj = 0;
    int endi = 0;
    int endj = 0;

    int returni = 0;
    int returnj = 0;


    int[] di = { -1, 1, 0, 0 };
    int[] dj = { 0, 0, -1, 1 };

    [Header("Territory Score")]
    public int n_totalTiles;
    public int n_territoryTiles;
    public int completePercent = 0;


    [Header("\n")]
    public AudioSource GetTerritory;

    // Struct (i, j) for BFS Search
    struct IJ
    {
        public int i;
        public int j;
        public IJ(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
    }



    /// <summary>
    /// 0: �ȸ��� ��
    /// 1: ���� �� �׵θ�
    /// 2: ��ô�� �� �׵θ�
    /// 3: ���� ��
    /// </summary>

    void Awake()
    {
        Init();
        state = img_player.GetComponent<PlayerStateImg>();
    }
    void Init()
    {
        i = 0;
        j = 0;
        //this.transform.position = grid.GetWorldPosition(i, j);
        img_player.transform.position += new Vector3(grid.cellSize, grid.cellSize);
        img_player.transform.localScale = new Vector3(grid.cellSize * 30 * playerSize, grid.cellSize * 30 * playerSize);
        n_territoryTiles = 0;
    }

    /// <summary>
    /// int[] di = { -1, 1, 0, 0 };
    /// int[] dj = { 0, 0, -1, 1 };
    /// </summary>
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.W))
        {
            Move(di[0], dj[0]);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Move(di[2], dj[2]);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Move(di[1], dj[1]);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(di[3], dj[3]);
        }
    }

    /// <summary>
    /// 0: �ȸ��� ��
    /// 1: ���� �� �׵θ�
    /// 2: ��ô�� �� �׵θ�
    /// 3: ���� ��
    /// </summary>
    
    // �� Tile �̵��ø��� ����
    public void Move(int di, int dj)
    {
        if (!StepWeb)
        {
            int pi = i;
            int pj = j;

            // �ش� �������� �̵� �����ϸ� �̵�
            if (Novable(i + di, j + dj))
            {
                this.transform.position = grid.GetWorldPosition(i, j);
                i += di;
                j += dj;
            }
            // �� ������ ����ġ�� ���ư���
            if (grid.gridArr[i, j] == 2)
            {
                inMyArea = true;
                state.PlayerSafe();
                i = returni;
                j = returnj;
                this.transform.position = grid.GetWorldPosition(returni, returnj);
                ReturnLine();
            }

            // �̵��ؼ� �� ĭ�� ���� ���� ���� ���� ���
            else if (grid.gridArr[i, j] == 0)
            {
                // �׵θ����� �� ����� �����̸�
                if (grid.gridArr[pi, pj] == 1)
                {
                    // ������ǥ(�ǵ��ƿ� ��ǥ �ٷ� ���� ��ǥ��ġ) ��ġ ���
                    starti = i;
                    startj = j;
                    // �ǵ��ƿ� ��ǥ ��ġ ���
                    returni = pi;
                    returnj = pj;
                    //Debug.Log("Start: (" + starti + ", " + startj + ") ==> " + grid.gridArr[starti, startj]);
                    grid.SetTileValue(starti, startj, 2);
                }
                //��� �׸���
                grid.SetTileValue(i, j, 2);

                // ���ݴ��� �� �ֵ��� �÷��̾� ���º��� ����
                inMyArea = false;
                state.PlayerAdventure();
            }
            // �� �� �԰� ���ƿ� ��
            else if (grid.gridArr[i, j] == 1 && grid.gridArr[pi, pj] == 2)
            {
                // ������ǥ ��ġ ���
                endi = i;
                endj = j;

                // ���� �ľ� �� ���� �� ó�� �Լ�
                CompleteArea();

                // SFX ���� 
                GetTerritory.Play();

                // �÷��̾� ���� ����
                inMyArea = true;
                state.PlayerSafe();
            }
        }
        

    }

    void CompleteArea()
    {
        /// 1. ������-���� �ִܰŸ� ã�� ����
        /// 2. ���� ���� ��ĵ�������� ���� �� �Ǻ�
        /// 3. �׷������̾�� ���� ä���
        /// 4. ��ô�� �׵θ� --> ������ �׵θ��� ��ȯ
        /// 5. ū �׵θ� ����� ���� ������ ��ȯ

        bool isBindedTerritory = false;
        //1 BFS�� ���� �׵θ� ����
        isBindedTerritory = BfsBinding();
        //2 ��ĵ���� ���� ���� �� ��ǥ ��ȯ
        if (isBindedTerritory)
        {
            (int i_gf, int j_gf) = FindInterPoint();
            
        //3 Grass Fire �˰������� �׵θ� ���� ä���, �׵θ� ����, ���� ���� �µθ� ó��
            if (i_gf != -1 && j_gf != -1)
            {
                GrassFire(i_gf, j_gf);
                SetTerritoryBoundary();
                DeleteInnerLines();
            }
            else
            {
                DeleteInnerLines();
            }
        }

        //4 ���� Ÿ��, ��ü Ÿ�� ���� �ۼ�Ʈ ���
        completePercent = (int)(n_territoryTiles / (float)n_totalTiles * 100);


    }

    /// <summary>
    /// 0: �ȸ��� ��
    /// 1: ���� �� �׵θ�
    /// 2: ��ô�� �� �׵θ�
    /// 3: ���� ��
    /// </summary>

    bool BfsBinding()
    {
        int[,] pathArr = new int[grid.h, grid.w];

        // 0�迭 ����� ��� ����
        for (int i = 0; i < grid.h; i++)
        {
            for (int j = 0; j < grid.w; j++)
            {
                pathArr[i, j] = 0;
            }
        }
        // ���� ���� 1 ����
        pathArr[starti, startj] = 1;
        Queue<IJ> q = new Queue<IJ>();
        q.Enqueue(new IJ(starti, startj));

        // start --> end���� ��� bfs Ž�� �� �ǵ��ư��� ó��
        while (q.Count != 0)
        {
            int ti = q.Peek().i;
            int tj = q.Peek().j;
            q.Dequeue();
            // �����ϸ�
            if (ti == endi && tj == endj)
            {
                q.Clear();
                // �ִܰ�� ã�� --> ���ư��鼭 gridArr ��� �� ��ô�׵θ��� ��ȯ 
                SetCompleteSurroundedArea(pathArr);

                return true;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    int nexti = ti + di[i];
                    int nextj = tj + dj[i];
                    if (Novable(nexti, nextj))
                    {
                        if (grid.gridArr[nexti, nextj] == 1 && pathArr[nexti, nextj] == 0)
                        {
                            pathArr[nexti, nextj] = pathArr[ti, tj] + 1;
                            q.Enqueue(new IJ(nexti, nextj));
                        }
                    }
                }
            }
        }
        return false;
    }

    void SetCompleteSurroundedArea(int[,] arr)
    {
        grid.SetTileValue(starti, startj, 2);
        grid.SetTileValue(endi, endj, 2);
        int ci = endi;
        int cj = endj;
        int cur = arr[endi, endj];
        while (arr[ci, cj] != 1)
        {
            for (int i = 0; i < 4; i++)
            {
                int ii = ci + di[i];
                int jj = cj + dj[i];

                if (Novable(ii, jj) && arr[ii, jj] == cur - 1 && grid.gridArr[ii, jj] != 0)
                {
                    ci = ii;
                    cj = jj;
                    // ���� �� �׵θ� ������� �����ֱ�
                    grid.SetTileValue(ci, cj, 2);
                    cur--;
                }
            }
        }
    }



    // ���� �� �Ǻ�
    (int i_gf, int j_gf) FindInterPoint()
    {
        // ���� ��ĵ����
        for (int i = 1; i < grid.h; i++)
        {
            for (int j = 1; j < grid.w; j++)
            {
                int cnt = 0;
                for (int cur = j; cur < grid.w; cur++)
                {
                    // ���� ��踦 �����µ�
                    if (grid.gridArr[i, cur] == 2)
                    {
                        if (cur < grid.w - 1)
                        {
                            // ���� �׵θ����� �ɸ��� Ž�� �н�
                            if (grid.gridArr[i, cur + 1] == 2)
                            {
                                i += 1;
                                j = 0;
                                break;
                            }
                        }
                        // �Ȱɸ��� ���� ī��Ʈ
                        cnt++;
                    }
                }
                // �ش� �� �������� ���� �������� Ȧ�� ������ŭ ���� �׵θ� ������ ���� ���� �� 
                if (cnt % 2 == 1)
                {
                    if(grid.gridArr[i,j] == 0)
                    {
                        Debug.Log("seed: " + i.ToString() + " " + j.ToString());
                        return (i, j);
                    }
                }
            }

        }
        return (-1, -1);
    }

    // ���� �� ä���
    void GrassFire(int i, int j)
    {
        grid.gridArr[i, j] = 3;
        grid.SetTileValue(i, j, 3);

        if (grid.gridArr[i - 1, j] == 0)
        {
            GrassFire(i - 1, j);
        }
        if (grid.gridArr[i + 1, j] == 0)
        {
            GrassFire(i + 1, j);
        }
        if (grid.gridArr[i, j - 1] == 0)
        {
            GrassFire(i, j - 1);
        }
        if (grid.gridArr[i, j + 1] == 0)
        {
            GrassFire(i, j + 1);
        }
    }

    //�� int �迭 �� ����
    void SetTerritoryBoundary()
    {
        for (int i = 0; i < grid.h; i++)
        {
            for (int j = 0; j < grid.w; j++)
            {
                if (grid.gridArr[i, j] == 2)
                {
                    grid.SetTileValue(i, j, 1);
                }
            }
        }
    }

    // �ڱ� �� �ڱⰡ ������ ���ڸ��� ���ư��� Ÿ�� �ʱ�ȭ
    public void ReturnLine()
    {
        for (int i = 1; i < grid.h - 1; i++)
        {
            for (int j = 1; j < grid.w - 1; j++)
            {
                if (grid.gridArr[i, j] == 2)
                {
                    grid.SetTileValue(i, j, 0);
                }
            }
        }
    }

    // �� 80% �̻� ������ ��� �� �����ϰ� �ϰ� �̹��� ���̰��ϱ�
    public void ClearImageViewer()
    {
        for (int i = 1; i < grid.h-1; i++)
        {
            for (int j = 1; j < grid.w-1; j++)
            {
                if( grid.gridArr[i,j] != 3){
                    grid.SetTileValue(i, j, 3);
                }
            }
        }
    }


    // ���� ���ο� �����ϴ� �׵θ� ����
    void DeleteInnerLines()
    {
        int[] di8 = { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] dj8 = { -1, 0, 1, 1, 1, 0, -1, -1 };
        for (int i = 1; i < grid.h - 1; i++)
        {
            for (int j = 1; j < grid.w - 1; j++)
            {
                if (grid.gridArr[i, j] == 1)
                {
                    int cnt = 0;
                    for (int k = 0; k < 8; k++)
                    {
                        int ii = i + di8[k];
                        int jj = j + dj8[k];
                        if (ArrExist(ii, jj) && grid.gridArr[ii, jj] == 0)
                        {
                            cnt++;
                        }
                    }
                    if (cnt == 0)
                    {
                        grid.SetTileValue(i, j, 3);
                    }
                }
            }
        }
    }
    bool Novable(int i, int j)
    {
        return (0 <= i && i < grid.h && 0 <= j && j < grid.w && grid.gridArr[i, j] != 3);
    }

    bool ArrExist(int i, int j)
    {
        return (0 <= i && i < grid.h && 0 <= j && j < grid.w);
    }

    void OnCollisionEnter(Collision col)
    {
        print("Collision");
        if(!inMyArea && col.gameObject.tag == "Enermy")
        {
            VillainGameOver = true;
            Debug.Log("collision with Enermy!");
        }
        
        if(col.gameObject.tag == "Web")
        {
            print("Web Tag!");
            StartCoroutine(SpiderWebpenalty(col.gameObject));
        }
    }

    // �Ź��� ������ ����Ǵ� �Լ�
    IEnumerator SpiderWebpenalty(GameObject web)
    {
        StepWeb = true;
        state.Playerweb();
        yield return new WaitForSeconds(3);
        Destroy(web);
        StepWeb = false;
        state.PlayerAdventure();
    }

}
