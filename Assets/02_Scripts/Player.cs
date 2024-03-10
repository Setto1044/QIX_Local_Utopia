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

    // 땅 먹으러 갈 때 출발,도착하는 지점
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
    /// 0: 안먹은 땅
    /// 1: 먹은 땅 테두리
    /// 2: 개척한 땅 테두리
    /// 3: 먹은 땅
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
    /// 0: 안먹은 땅
    /// 1: 먹은 땅 테두리
    /// 2: 개척한 땅 테두리
    /// 3: 먹은 땅
    /// </summary>
    
    // 매 Tile 이동시마다 실행
    public void Move(int di, int dj)
    {
        if (!StepWeb)
        {
            int pi = i;
            int pj = j;

            // 해당 방향으로 이동 가능하면 이동
            if (Novable(i + di, j + dj))
            {
                this.transform.position = grid.GetWorldPosition(i, j);
                i += di;
                j += dj;
            }
            // 흰선 밟으면 원위치로 돌아가기
            if (grid.gridArr[i, j] == 2)
            {
                inMyArea = true;
                state.PlayerSafe();
                i = returni;
                j = returnj;
                this.transform.position = grid.GetWorldPosition(returni, returnj);
                ReturnLine();
            }

            // 이동해서 온 칸이 아직 먹지 않은 땅일 경우
            else if (grid.gridArr[i, j] == 0)
            {
                // 테두리에서 막 출발한 상태이면
                if (grid.gridArr[pi, pj] == 1)
                {
                    // 시작좌표(되돌아올 좌표 바로 다음 좌표위치) 위치 기록
                    starti = i;
                    startj = j;
                    // 되돌아올 좌표 위치 기록
                    returni = pi;
                    returnj = pj;
                    //Debug.Log("Start: (" + starti + ", " + startj + ") ==> " + grid.gridArr[starti, startj]);
                    grid.SetTileValue(starti, startj, 2);
                }
                //경로 그리기
                grid.SetTileValue(i, j, 2);

                // 공격당할 수 있도록 플레이어 상태변수 변경
                inMyArea = false;
                state.PlayerAdventure();
            }
            // 땅 다 먹고 돌아올 때
            else if (grid.gridArr[i, j] == 1 && grid.gridArr[pi, pj] == 2)
            {
                // 도착좌표 위치 기록
                endi = i;
                endj = j;

                // 영역 파악 및 먹은 땅 처리 함수
                CompleteArea();

                // SFX 실행 
                GetTerritory.Play();

                // 플레이어 상태 변경
                inMyArea = true;
                state.PlayerSafe();
            }
        }
        

    }

    void CompleteArea()
    {
        /// 1. 시작점-끝점 최단거리 찾아 묶기
        /// 2. 묶은 도형 스캔라인으로 내부 점 판별
        /// 3. 그래스파이어로 도형 채우기
        /// 4. 개척땅 테두리 --> 먹은땅 테두리로 전환
        /// 5. 큰 테두리 남기고 전부 땅으로 전환

        bool isBindedTerritory = false;
        //1 BFS로 도형 테두리 묶기
        isBindedTerritory = BfsBinding();
        //2 스캔라인 도형 내부 점 좌표 반환
        if (isBindedTerritory)
        {
            (int i_gf, int j_gf) = FindInterPoint();
            
        //3 Grass Fire 알고리즘으로 테두리 내부 채우기, 테두리 설정, 내부 잔존 태두리 처리
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

        //4 먹은 타일, 전체 타일 비교해 퍼센트 계산
        completePercent = (int)(n_territoryTiles / (float)n_totalTiles * 100);


    }

    /// <summary>
    /// 0: 안먹은 땅
    /// 1: 먹은 땅 테두리
    /// 2: 개척한 땅 테두리
    /// 3: 먹은 땅
    /// </summary>

    bool BfsBinding()
    {
        int[,] pathArr = new int[grid.h, grid.w];

        // 0배열 만들어 경로 저장
        for (int i = 0; i < grid.h; i++)
        {
            for (int j = 0; j < grid.w; j++)
            {
                pathArr[i, j] = 0;
            }
        }
        // 시작 지점 1 저장
        pathArr[starti, startj] = 1;
        Queue<IJ> q = new Queue<IJ>();
        q.Enqueue(new IJ(starti, startj));

        // start --> end까지 경로 bfs 탐색 후 되돌아가며 처리
        while (q.Count != 0)
        {
            int ti = q.Peek().i;
            int tj = q.Peek().j;
            q.Dequeue();
            // 도착하면
            if (ti == endi && tj == endj)
            {
                q.Clear();
                // 최단경로 찾음 --> 돌아가면서 gridArr 경로 값 개척테두리로 전환 
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
                    // 도형 내 테두리 흰색으로 묶어주기
                    grid.SetTileValue(ci, cj, 2);
                    cur--;
                }
            }
        }
    }



    // 내부 점 판별
    (int i_gf, int j_gf) FindInterPoint()
    {
        // 가로 스캔라인
        for (int i = 1; i < grid.h; i++)
        {
            for (int j = 1; j < grid.w; j++)
            {
                int cnt = 0;
                for (int cur = j; cur < grid.w; cur++)
                {
                    // 도형 경계를 만났는데
                    if (grid.gridArr[i, cur] == 2)
                    {
                        if (cur < grid.w - 1)
                        {
                            // 가로 테두리선에 걸리면 탐색 패스
                            if (grid.gridArr[i, cur + 1] == 2)
                            {
                                i += 1;
                                j = 0;
                                break;
                            }
                        }
                        // 안걸리면 개수 카운트
                        cnt++;
                    }
                }
                // 해당 점 기준으로 그은 반직선과 홀수 갯수만큼 도형 테두리 만나면 도형 내부 점 
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

    // 먹은 땅 채우기
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

    //땅 int 배열 값 갱신
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

    // 자기 선 자기가 밟으면 제자리로 돌아가고 타일 초기화
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

    // 땅 80% 이상 먹으면 모든 땅 투명하게 하고 이미지 보이게하기
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


    // 영역 내부에 잔존하는 테두리 삭제
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

    // 거미줄 밟으면 실행되는 함수
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
