using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DiskManager : MonoBehaviour {

    public GameObject spawnObject;
    public Text message;
    public int spawnNumber = 0;
    public Transform[] spawnPosition;

    //儲存位置紀錄
    private Dictionary<int, Dictionary<int,GameObject>> recordTower = new Dictionary<int, Dictionary<int, GameObject>>();
    private Dictionary<int,GameObject> tower1 = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> tower2 = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> tower3 = new Dictionary<int, GameObject>();

    private int selectID = 0;
    private bool _isSelect = false;

    private void Start()
    {
        //初始化
        Init();
    }

    private void Update()
    {
        //檢查用
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(tower1.Count);
            Debug.Log(tower2.Count);
            Debug.Log(tower3.Count);
        }
    }

    /// <summary>
    /// 生成物件(初始化)
    /// </summary>
    private void Init()
    {
        if (spawnNumber > 7)
        {
            message.text = "超出顯示範圍，請設定低於8的數量";
            return;
        }

        message.text = "未選取";
        //加入至紀錄字典
        recordTower.Add(1, tower1);
        recordTower.Add(2, tower2);
        recordTower.Add(3, tower3);

        float size = 1.1f;
        for (int i = spawnNumber; i > 0; i--)
        {
            //生成物件
            GameObject obj = Instantiate(spawnObject, spawnPosition[0].transform);
            //給予編號
            obj.GetComponent<DiskObject>().ID = i;
            obj.GetComponentInChildren<Text>().text = i.ToString();
            //紀錄物件
            tower1.Add(i, obj);

            //大小調整
            size = size - 0.1f;
            obj.transform.localScale = new Vector3(size, obj.transform.localScale.y, obj.transform.localScale.z);
        }
    }

    /// <summary>
    /// 選擇物件(按鈕事件)
    /// </summary>
    /// <param name="towerID"></param>
    public void SelectDisk(int towerID)
    {
        //超出顯示範圍，請設定低於8的數量
        if (spawnNumber > 7)
        {
            return;
        }
        if (_isSelect == false)
        {
            //如果選擇對象為空
            if (recordTower[towerID].Keys.Count == 0)
            {
                message.text = "未選取";
                return;
            }
            message.text = "選取中:" + towerID;
            _isSelect = true;
            selectID = towerID;
        }
        else
        {
            message.text = "已放置:" + towerID;
            _isSelect = false;
            MoveDisk(selectID, towerID);
        }
    }

    /// <summary>
    /// 移動物件
    /// </summary>
    /// <param name="currentTower"></param>
    /// <param name="targetTower"></param>
    /// <returns></returns>
    private bool MoveDisk(int currentTower, int targetTower)
    {
        //如果目標為空或是值小於目標
        if (recordTower[targetTower].Keys.Count == 0 || recordTower[currentTower].Keys.Min() < recordTower[targetTower].Keys.Min())
        {
            GameObject obj = recordTower[currentTower][recordTower[currentTower].Keys.Min()];
            //加入最小值
            recordTower[targetTower].Add(recordTower[currentTower].Keys.Min(), obj);
            //移除最小值
            recordTower[currentTower].Remove(recordTower[currentTower].Keys.Min());
            //移動物件
            obj.transform.SetParent(spawnPosition[targetTower - 1].transform);
            return true;
        }
        else
        {
            return false;
        }
    }
}
