using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[Serializable]
struct QValue {
    public GameState S;
    public MyInput A;
    public float value;

    public QValue(GameState S, MyInput A, float value) {
        this.S = S;
        this.A = A;
        this.value = value;
    }
    public QValue(GameState S, MyInput A) {
        this.S = S;
        this.A = A;
        this.value = 0;
    }

    public override string ToString() {
        return "(S= "+ S + ", A= " + A + ", value= " + value + ")";
    }
}

[Serializable]
class QTable {
    public List<QValue> table;

    public QTable() {
        table = new List<QValue>();
    }
}

// On va devoir coder un Markov Decision Processes !
// Epsilon-greedy + Q-learning
public class StrategieMDP : Strategie
{
    public string filePath; // Le fichier où l'on va sauvegarder la QTable !
    public float facteurDeReduction; // Gamma // A quel point les frames suivantes sont de moins en moins importantes
    public float learningRate; // Alpha // A quel point on veut modifier notre modèle à chaque apprentissage
    public float explorationRate; // Epsilon // A quel point notre agent va explorer autour de lui !
    public float explorationRateDecrease; // La vitesse à laquelle on diminue notre taux d'exploration !
    public float minExplorationRate; // Le minimum d'exploration à tous temps !

    QTable qTable;

    string path;

    public override MyInput GetInput(GameState gs) {
        if(UnityEngine.Random.Range(0f, 1f) < explorationRate)
        {
            return MyInput.Random();
        } else
        {
            return GreedyPolicy(gs);
        }
    }

    float rewardFunction(GameState S, MyInput A) {
        // To CODE !
        return 0;
    }

    Dictionary<GameState, float> transitionFunction(GameState S, MyInput A)
    {
        Dictionary<GameState, float> d = new Dictionary<GameState, float>();
        return d;
    }

    public override void Load()
    {
        path = Application.persistentDataPath + "\\" + filePath;

        // On récupère notre QTable si elle existe déjà
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            qTable = (QTable)formatter.Deserialize(stream);
            stream.Close();
            stream.Dispose();

        // Si elle n'existe pas, on l'initialise :)
        } catch (Exception e) {
            Debug.LogError(e.Message);
            // Initialiser la QTable
            qTable = new QTable();
            foreach(GameState gs in GameState.AllStates()) {
                foreach(MyInput input in MyInput.AllInputs()) {
                    qTable.table.Add(new QValue(gs, input));
                }
            }
        }
        Debug.Log("Taille de la QTable = " + qTable.table.Count);
    }

    public override void Unload() {
        Debug.Log("Stratégie " + name + " : " + (GetWinrate() * 100f) + "% winrate !");

        // On enregistre tout notre apprentissage ! <3
        try
        {
            Debug.Log("Save path = " + path);
            Stream stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Write);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, qTable);
            stream.Close();
            stream.Dispose();
        } catch (Exception e) {
            Debug.LogError(e.Message);
        }
    }

    MyInput GreedyPolicy(GameState gs) {
        float currentValue = float.NegativeInfinity;
        MyInput currentInput = new MyInput(MyInput.Coup.NOTHING, MyInput.Deplacement.NOTHING);

        // On essaye d'optimiser l'itération pour ne parcourir que 15 états au lieu de 29000 :D
        int idDebut = (MyInput.nbInputs) * gs.GetId();
        int idFin = idDebut + MyInput.nbInputs;
        for(int i = idDebut; i < idFin; i++) {
            if(qTable.table[i].value > currentValue) {
                currentInput = qTable.table[i].A;
                currentValue = qTable.table[i].value;
            }
        }
        return currentInput;
    }

    public override void Reward(GameState S, MyInput A, GameState newS, float r) {
        // Q(S, A) = Q(S, A) + learningRate * (GreedyPolicy(Q(newS, ?).S) - Q(S, A));
        QValue QSA = Q(S, A);
        MyInput newA = GreedyPolicy(newS);
        QSA.value = QSA.value + learningRate * (r + facteurDeReduction * Q(newS, newA).value - QSA.value);

        // Et on remet cette valeur dans la table
        SetQTable(QSA.S, QSA.A, QSA.value);
    }

    QValue Q(GameState S, MyInput A) {
        return qTable.table[(MyInput.nbInputs) * S.GetId() + A.GetId()];
    }

    void SetQTable(GameState S, MyInput A, float value) {
        int id = (MyInput.nbInputs) * S.GetId() + A.GetId();
        qTable.table[id] = new QValue(S, A, value);
    }

    public override void RegisterVictory() {
        base.RegisterVictory();
        explorationRate = Mathf.Max(explorationRate * explorationRateDecrease, minExplorationRate);
    }
    public override void RegisterDefeat() {
        base.RegisterDefeat();
        explorationRate = Mathf.Max(explorationRate * explorationRateDecrease, minExplorationRate);
    }
}
