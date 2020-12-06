using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    private int encerts;
    private int errades;

    public TextMeshProUGUI EncertsNum;
    public TextMeshProUGUI ErradesNum;

    int GetEncerts()
    {
        return encerts;
    }

    void SetEncerts(int encertsNum)
    {
        encerts += encertsNum;
        EncertsNum.text = encerts.ToString();
    }

    void SetEncerts()
    {
        encerts++;
        EncertsNum.text = encerts.ToString();
    }

    int GetErrades()
    {
        return errades;
    }

    void SetErrades(int erradesNum)
    {
        errades += erradesNum;
        ErradesNum.text = errades.ToString();
    }

    void SetErrades()
    {
        errades++;
        ErradesNum.text = errades.ToString();
    }
}
