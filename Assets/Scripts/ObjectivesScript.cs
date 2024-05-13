using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObjectivesScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Objectives;

    private int currentObjectiveIndex = 0;

    public void AddNewObjectives(List<GameObject> newObjectives, int currentLevel)
    {
        foreach (var objective in Objectives)
        {
            objective.GetComponentInChildren<Light2D>().intensity = 0;
        }

        Objectives = newObjectives;
        SetObjectiveNumbers(currentLevel);
        currentObjectiveIndex = -1;
        SetNextObjective();
    }

    public void ResetObjectives()
    {
        foreach (var objective in Objectives)
        {
            objective.GetComponentInChildren<Light2D>().intensity = 0;
        }

        currentObjectiveIndex = -1;
        SetNextObjective();
    }

    public void SetNextObjective()
    {
        currentObjectiveIndex++;
        Objectives.Where(o => o.layer == 6).First().layer = 7;
        Objectives[currentObjectiveIndex].layer = 6;
        StartCoroutine(Flash());
    }

    private void Awake()
    {
        SetObjectiveNumbers(1);
    }

    private void SetObjectiveNumbers(int currentNumber)
    {
        foreach (GameObject obj in Objectives)
        {
            var numberText = obj.GetComponentInChildren<TextMeshPro>();
            numberText.text = currentNumber.ToString();
            currentNumber++;
        }
    }

    private void Start()
    {
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        var nextLight = Objectives[currentObjectiveIndex].GetComponentInChildren<Light2D>();

        if (currentObjectiveIndex > 0)
        {
            var previousLight = Objectives[currentObjectiveIndex-1].GetComponentInChildren<Light2D>();
            previousLight.intensity = 0;
        }

        for(int i = 0; i <= 3; i++)
        {
            nextLight.intensity = 1;
            yield return new WaitForSeconds(0.3f);

            nextLight.intensity = 0;
            yield return new WaitForSeconds(0.3f);
        }

        nextLight.intensity = 1f;
    }
}
