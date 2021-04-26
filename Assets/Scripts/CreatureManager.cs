using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour
{
    private static CreatureManager instance;
    public static CreatureManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError(nameof(CreatureManager) + " instance is null");
            }
            return instance;
        }
    }

    private List<CreatureAI> creatures;

    public int Count => creatures.Count;

    public List<CreatureAI> Creatures => creatures;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        creatures = new List<CreatureAI>();
    }

    public void RegisterCreature(CreatureAI creature)
    {
        if (!creatures.Contains(creature))
            creatures.Add(creature);
    }

    public void UnregisterCreature(CreatureAI creature)
    {
        if (creatures.Contains(creature))
            creatures.Remove(creature);
    }
}
