using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManger : MonoBehaviour {

    /// <summary>
    /// 围墙的数组
    /// </summary>
    public GameObject[] outWallArray;

    /// <summary>
    /// 地板的数组
    /// </summary>
    public GameObject[] floorArray;

    /// <summary>
    /// 障碍物数组
    /// </summary>
    public GameObject[] wallArray;

    /// <summary>
    /// 食物数组
    /// </summary>
    public GameObject[] foodArray;

    /// <summary>
    /// 僵尸数组
    /// </summary>
    public GameObject[] zombieArray;

    /// <summary>
    /// 出口的实例
    /// </summary>
    public GameObject exitPrefeb;

    /// <summary>
    /// 行数
    /// </summary>
    public const int ROWS = 10;

    /// <summary>
    /// 列数
    /// </summary>
    public const int CLOS = 10;

    /// <summary>
    /// 地图控制器
    /// </summary>
    private Transform mapHolder;

    /// <summary>
    /// 最小障碍物数量
    /// </summary>
    public const int MIN_WALL_COUNT = 2;

    /// <summary>
    /// 最大障碍物数量
    /// </summary>
    public const int MAX_WALL_COUNT = 8;

    /// <summary>
    /// 游戏管理器
    /// </summary>
    private GameManager gameManager;

    /// <summary>
    /// 墙的位置
    /// </summary>
    private List<Vector2> positionList = new List<Vector2>();

	// Use this for initialization
	void Start () {
        gameManager = this.GetComponent<GameManager>();
        this.InitMap();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 初始化地图
    /// </summary>
    private void InitMap()
    {
        mapHolder = new GameObject("Map").transform;

        //创建围墙和地板
        for (int i = 0; i < CLOS; i++)
        {
            for (int j = 0; j < ROWS; j++)
            {
                if (i == 0 || j == 0 || i == CLOS - 1 || j == ROWS - 1)
                {

                    GameObject go = GameObject.Instantiate(
                        this.getRandomGameObject(outWallArray),
                        new Vector3(i, j, 0),
                        Quaternion.identity) as GameObject;
                    go.transform.SetParent(mapHolder);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(
                         this.getRandomGameObject(floorArray),
                        new Vector3(i, j, 0),
                        Quaternion.identity) as GameObject;
                    go.transform.SetParent(mapHolder);
                }
            }
        }

        //创建障碍物
        positionList.Clear();
        int wallCount = Random.Range(MapManger.MIN_WALL_COUNT, MapManger.MAX_WALL_COUNT + 1);

        for (int i = 2; i < CLOS - 2; i++)
        {
            for (int j = 2; j < ROWS - 2; j++)
            {
                positionList.Add(new Vector2(i, j));
            }
        }

        //根据障碍物数量生成所有障碍物
        InstantiateItems(wallCount, wallArray);

        //生成食物 [2, level * 2]
        int foodCount = Random.Range(2, gameManager.gameLevel * 2 + 1);
        InstantiateItems(foodCount, foodArray);

        //生成敌人 [0,level/2]
        int zombieCount = Random.Range(0, gameManager.gameLevel / 2);
        InstantiateItems(zombieCount, zombieArray);

        //创建出口

        GameObject exit = GameObject.Instantiate(
                exitPrefeb,
                new Vector3(CLOS - 2, ROWS - 2, 0),
                Quaternion.identity) as GameObject;
        exit.transform.SetParent(mapHolder);

    }

    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="count"></param>
    /// <param name="sorData"></param>
    private void InstantiateItems(int count , GameObject[] sorData)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 location = this.getRandomLoc(positionList);
            GameObject go = GameObject.Instantiate(
                this.getRandomGameObject(sorData),
                new Vector3(location.x, location.y, 0),
                Quaternion.identity) as GameObject;
            go.transform.SetParent(mapHolder);
        }
    }

    /// <summary>
    /// 获取随机点
    /// </summary>
    /// <param name="sorData"></param>
    /// <returns></returns>
    private Vector2 getRandomLoc(List<Vector2> sorData)
    {
        int index = Random.Range(0, sorData.Count);
        Vector2 result = sorData[index];
        sorData.RemoveAt(index);
        return result;
    }

    /// <summary>
    /// 获取随机的物件
    /// </summary>
    /// <param name="sorData"></param>
    /// <returns></returns>
    private GameObject getRandomGameObject(GameObject[] sorData)
    {
        int index = Random.Range(0, sorData.Length);
        return sorData[index];
    }
}
