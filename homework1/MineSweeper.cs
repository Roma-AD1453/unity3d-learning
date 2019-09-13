using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeper : MonoBehaviour
{

    public int col = 9;
    public int raw = 9;
    public int numMine = 10;
    private int countSafe;
    private float time;
    private int result;
    private int[] mine = new int[100];
    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        ReStart();
    }

    // Update is called once per frame
    void Update()
    {
        //开始游戏后计时
        if (result == 1)
            time += Time.deltaTime;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        //显示游戏时间
        int timeSec = (int)time;
        GUI.Label(new Rect(50, 75, 100, 50), "Time: " + timeSec.ToString(), style);
        if (result == 3)
            GUI.Label(new Rect(50, 125, 100, 50), "You die.", style);
        if (result == 2)
            GUI.Label(new Rect(50, 125, 100, 50), "You win.", style);
        if (GUI.Button(new Rect(100, 300, 100, 50), "Replay"))
            ReStart();

        //打印雷区
        for(int i = 0; i < col; i++)
        {
            for (int j = 0; j < raw; j++)
            {
                if (GUI.Button(new Rect(400 + i * 50, 40 + j * 50, 50, 50), Block(mine[i + col * j])))
                {
                    //准备开始状态，第一次扫雷后将地雷布在一格以外的周边
                    if (result == 0)
                    {
                        MinePlanting(i, j);
                        Sweep(i, j);
                        result = 1;
                    }
                    if (result == 1)
                        Sweep(i, j);
                }
                //游戏失败后显示所有地雷位置
                if (result == 3 && mine[i + col * j] == 9)
                    mine[i + col * j] += 10;
            }
        }
    }

    void ReStart()
    {
        //初始化
        time = 0;
        result = 0;
        countSafe = 0;
        for (int i = 0; i < numMine; i++)
        {
            mine[i] = 9;
        }
        for (int i = numMine; i < col * raw; i++)
        {
            mine[i] = 0;
        }
    }

    void MinePlanting(int x, int y)
    {
        int a = 3, b = 3; //第一处排雷点及其附近区域的边长
        if (x == 0 || x == col - 1)
            a = 2;
        if (y == 0 || y == raw - 1)
            b = 2;
        int s = a * b; //设第一处排雷点附近区域为区域s

        //随机摆放地雷
        for (int i = col * raw - s - 1; i >= 0; i--)
        {
            int t = rand.Next(0, i);
            int temp = mine[i];
            mine[i] = mine[t];
            mine[t] = temp;
        }
        //避开第一处排雷点
        for (int i = x - 1; i <= x + 1; i++)
        {
            //检测x坐标是否在雷区内
            if (i < 0 || i >= col)
                continue;
            for (int j = y - 1; j <= y + 1; j++)
            {
                //检测y坐标是否雷区内
                if (j < 0 || j >= raw)
                    continue;
                //将预留的空位交换到区域s中
                for (int k = s; k > 0; k--)
                {
                    int temp = mine[i + col * j];
                    mine[i + col * j] = mine[col * raw - k];
                    mine[col * raw - k] = temp;
                }
            }
        }
        
        //记录每个地块是否有雷或周围有多少雷，其中0~8表示周围有0~8个地雷，9表示该地块有雷
        for (int i = 0; i < col * raw; i++)
        {
            if (mine[i] != 9)
                mine[i] = CountMine(i % raw, i / raw);
        }
    }

    void Sweep(int x, int y)
    {

        //按到地雷时返回
        if (mine[x + col * y] == 9)
        {
            result = 3;
            return;
        }

        if (mine[x + col * y] / 10 == 0)
        {
            //安全区域增加
            countSafe++;
            //十位数置1以表示该地块已探查
            mine[x + col * y] += 10;
        }
        if (countSafe == col * raw - numMine)
            result = 2;

        //周围有地雷，返回
        if (mine[x + col * y] % 10 > 0)
            return;


        //周围没有地雷的话则对四周进行深度优先遍历，直到碰到数字为止
        for (int i = x - 1; i <= x + 1; i++)
        {
            //检测x坐标是否在区域内
            if (i < 0 || i >= col)
                continue;
            for (int j = y - 1; j <= y + 1; j++)
            {
                //检测y坐标是否在区域内
                if (j < 0 || j >= raw)
                    continue;
                //若该地块未探查且周围无地雷，探查该地块
                if (mine[i + col * j] % 10 != 9 && mine[i + col * j] / 10 == 0)
                    Sweep(i, j);
            }
        }
    }

    //统计该地块附近地雷数
    int CountMine(int x, int y)
    {
        int count = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            if (i < 0 || i >= col)
                continue;
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (j < 0 || j >= raw)
                    continue;
                if (mine[i + col * j] == 9)
                    count++;
            }
        }
        return count;
    }

    //地块状态显示
    string Block(int m)
    {
        int num = m % 10;
        int isOpen = m / 10;
        if (isOpen == 0)
            return "";
        if (num == 0)
            return "0";
        if (num == 9)
            return "*";
        else
            return num.ToString();
    }
}
