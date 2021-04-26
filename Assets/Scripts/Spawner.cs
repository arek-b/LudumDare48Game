using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int targetCreatureCount = 10;
    [SerializeField] private CreatureAI[] preexistingCreatures = default;
    [SerializeField] private bool activateWhenGameStarts = false;
    [SerializeField] private CreatureAI creaturePrefab = null;

    private List<CreatureAI> creatures = new List<CreatureAI>();

    private bool activated = false;

    private void Awake()
    {
        CreatureAI.CreatureHasDied += CreatureHasDied;

        foreach (var item in preexistingCreatures)
        {
            Add(item);
        }
    }

    private void CreatureHasDied(CreatureAI creature)
    {
        Remove(creature);
    }

    private void Start()
    {
        if (activateWhenGameStarts)
            Activate();
    }

    public void Add(CreatureAI creature)
    {
        if (!creatures.Contains(creature))
            creatures.Add(creature);
    }

    public void Remove(CreatureAI creature)
    {
        if (creatures.Contains(creature))
            creatures.Remove(creature);
    }

    public void Activate()
    {
        activated = true;
        foreach (var item in CreatureManager.Instance.Creatures)
        {
            Add(item);
        }
    }

    private void Update()
    {
        if (!activated)
            return;

        if (creatures.Count >= targetCreatureCount)
            return;

        int toSpawn = targetCreatureCount - creatures.Count;

        Debug.Log($"Spawning {toSpawn} creatures");

        for (int i = 0; i < toSpawn; i++)
        {
            CreatureAI instance = Instantiate(creaturePrefab, transform.position + new Vector3(Random.value, 0, Random.value), Quaternion.identity);

            creatures.Add(instance);
        }
    }
}