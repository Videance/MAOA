using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string caminho =>
        Path.Combine(Application.persistentDataPath, "save.json");

    public static void Salvar()
    {
        SaveData dados = new SaveData();

        // Tutorial
        dados.emTutorial = S_controleTutorial.emTutorial;
        dados.tutorial1 = S_controleTutorial.tutorial1;

        // História
        dados.faseAtual = S_onClique.faseAtual;

        // Leaderboard
        dados.vitoriasXbot = new(S_pontos.vitoriasXbot);

        string json = JsonUtility.ToJson(dados, true);
        File.WriteAllText(caminho, json);

        Debug.Log("Salvo em: " + caminho);
    }

    public static void Carregar()
    {
        if (!File.Exists(caminho))
            return;

        string json = File.ReadAllText(caminho);
        SaveData dados = JsonUtility.FromJson<SaveData>(json);

        // Tutorial
        S_controleTutorial.emTutorial = dados.emTutorial;
        S_controleTutorial.tutorial1 = dados.tutorial1;

        // História
        S_onClique.faseAtual = dados.faseAtual;

        // Leaderboard
        S_pontos.vitoriasXbot = new(dados.vitoriasXbot);

        // Golpes
        S_modoHistoria.aprendidos.Clear();

        // 4 iniciais
        for (int i = 0; i < 4; i++)
        {
            S_modoHistoria.aprendidos.Add(S_verificaGolpe.Vgolpe.golpes[i]);
        }

        int[] fases = { 4, 6, 8, 10, 12, 14, 16, 18 };

        int indiceGolpe = 4;

        foreach (int fase in fases)
        {
            if (S_onClique.faseAtual >= fase)
            {
                S_modoHistoria.aprendidos.Add(S_verificaGolpe.Vgolpe.golpes[indiceGolpe++]);
                S_modoHistoria.aprendidos.Add(S_verificaGolpe.Vgolpe.golpes[indiceGolpe++]);
            }
        }
    }
}