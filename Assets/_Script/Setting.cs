using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Setting : MonoBehaviour
{
    private BigInteger Score = 0;
    private BigInteger PayScore = 1;
    private BigInteger DropScore = 10; // 10점씩 증가하도록 변경

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public BigInteger GetGold()
    {
        Score += DropScore; // 10점씩 증가

        return Score;
    }

    private string FormatNum(BigInteger num)
    {
        string[] unit = { "", "K", "M", "Y", "I" };
        int unitIndex = 0;

        while (num > 1000 && unitIndex < unit.Length - 1)
        {
            num /= 1000;
            unitIndex++;
        }

        string fNum = string.Format("{0}{1}", num.ToString(), unit[unitIndex]);

        return fNum;
    }

    public string StringScore()
    {
        return FormatNum(Score);
    }

    public string StringPayScore()
    {
        return FormatNum(PayScore);
    }
}
