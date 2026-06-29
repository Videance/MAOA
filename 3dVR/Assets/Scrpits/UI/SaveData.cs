using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Tutorial
    public bool emTutorial = true;
    public bool tutorial1 = true;

    // História
    public int faseAtual = 0;

    // Leaderboard
    public List<Vector3> vitoriasXbot = new();
}