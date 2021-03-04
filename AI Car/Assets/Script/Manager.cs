using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int populationSize = 100;
    public float trainingDuration = 30;
    public float mutation = 5;

    public Agent agentPrefab;
    public Transform agentGroup;

    Agent agent;
    public List<Agent> agents = new List<Agent>();

    public CameraController cameraController;

    public GameObject uiNeurone;
    private bool uiIsActive = true;

    void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        NewGeneration();
        InitNeuralNetworkViewer();
        Focus();
        yield return new WaitForSeconds(trainingDuration);
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        NewGeneration();
        Focus();
        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
    }

    void NewGeneration()
    {
        agents.Sort();
        AddOrRemoveAgent();
        Mutate();
        ResetAgents();
        SetColors();
    }
    
    void AddOrRemoveAgent()
    {
        if (agents.Count != populationSize)
        {
            int dif = populationSize - agents.Count;

            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    AddAgent();
                }
            }
            else
            {
                for (int i = 0; i < -dif; i++)
                {
                    RemoveAgent();
                }
            }
        }
    }

    void AddAgent()
    {
        agent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup);
        agent.net = new NeuralNetwork(agent.net.layers);
        agents.Add(agent);
    }

    void RemoveAgent()
    {
        Destroy(agents[agents.Count - 1].transform);
        agents.RemoveAt(agents.Count - 1);
    }

    void Focus()
    {
        NeuralNetworkViewer.instance.agent = agents[0];
        NeuralNetworkViewer.instance.RefreshAxon();
        cameraController.target = (agents[0].transform);
    }

    void Mutate()
    {
        for (int i = agents.Count/2 ; i < agents.Count; i++)
        {
            agents[i].net.CopyNet(agents[i - (agents.Count / 2)].net);
            agents[i].net.Mutate(mutation);
            agents[i].SetMutateColor();
        }
    }

    void ResetAgents()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].ResetAgent();
        }
    }

    void SetColors()
    {
        for (int i = 0; i < agents.Count/2; i++)
        {
            agents[i].SetDefaultColor();
        }
        agents[0].SetFirstColor();
    }

    public void End()
    {
        StopAllCoroutines();
        StartCoroutine(Loop());
    }

    public void ResetNets()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].net = new NeuralNetwork(agent.net.layers);
        }

        End();
    }

    public void Save()
    {
        List<NeuralNetwork> nets = new List<NeuralNetwork>();

        for (int i = 0; i < agents.Count; i++)
        {
            nets.Add(agents[i].net);
        }

        DataManager.instance.Save(nets);
    }

    public void Load()
    {
        Data data = DataManager.instance.Load();

        if (data != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = data.nets[i];
            }
        }

        End();
    }

    public void ShowNeuralSysteme() 
    {
        if (!uiIsActive)
        {
            uiIsActive = true;
            uiNeurone.SetActive(true);
        }
        else
        {
            uiIsActive = false;
            uiNeurone.SetActive(false);
        }
    }

    void InitNeuralNetworkViewer()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }
}
