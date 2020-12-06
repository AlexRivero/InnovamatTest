using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProblemData
{
    public string wording;
    public int correctAnswer;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ProblemScriptableObject", order = 1)]
public class ProblemScriptableObject : ScriptableObject
{
    public List<ProblemData> Problems;
}
