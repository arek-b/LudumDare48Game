using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class SphereCreature : MonoBehaviour
{
    [SerializeField] private float minDistanceBetweenElements = 0.1f;
    [SerializeField] private float assemblyDuration = 1f;
    [SerializeField] private MeshFilter meshFilter = null;
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] public SphereCreatureMovement sphereCreatureMovement = null;
    [SerializeField] public PlayerCameraRotation playerCameraRotation = null;

    public Transform SphereTransform => meshFilter.transform;

    private Vector3[] vertices;

    private System.Random random;

    private List<CanMorphIntoSphereCreature> currentlyPosessedMorphables = new List<CanMorphIntoSphereCreature>();

    private void Awake()
    {
        vertices = meshFilter.mesh.vertices;
        random = new System.Random();
        meshRenderer.enabled = false;
    }

    public void ReleaseTransforms()
    {
        foreach (CanMorphIntoSphereCreature item in currentlyPosessedMorphables)
        {
            item.transform.SetParent(null);
            item.gameObject.layer = 0;
            item.SetNotMorphed();
        }

        currentlyPosessedMorphables.Clear();
    }

    private Vector3 NewScale(List<CanMorphIntoSphereCreature> morphables)
    {
        float newScale = 0.5f + morphables.Count / 20f;
        return new Vector3(newScale, newScale, newScale);
    }

    private Vector3 NewPosition(List<CanMorphIntoSphereCreature> morphables)
    {
        float totalX = 0f;
        float totalZ = 0f;
        foreach (CanMorphIntoSphereCreature item in morphables)
        {
            totalX += item.transform.position.x;
            totalZ += item.transform.position.z;
        }
        float centerX = totalX / morphables.Count;
        float centerZ = totalZ / morphables.Count;
        float y = transform.position.y;

        if (Physics.Raycast(meshFilter.transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            y = hitInfo.point.y + (meshFilter.mesh.bounds.size.y * meshFilter.transform.localScale.y / 2);
        }

        return new Vector3(centerX, y, centerZ);
    }

    public void BeginAssembly(List<CanMorphIntoSphereCreature> morphables)
    {
        foreach (CanMorphIntoSphereCreature item in morphables)
        {
            item.SetMorphed(this);
        }
        meshFilter.transform.localScale = NewScale(morphables);
        transform.position = NewPosition(morphables);
        var vertexAssignment = AssignMorphablesToVertices(morphables);
        MoveTransformsToVertices(vertexAssignment);
        StartCoroutine(EnableControlsDelayed(assemblyDuration));
        playerCameraRotation.enabled = true;
    }

    private void MoveTransformsToVertices(Dictionary<Vector3, CanMorphIntoSphereCreature> assignments)
    {
        const int SphereCreatureLayer = 10;

        foreach (KeyValuePair<Vector3, CanMorphIntoSphereCreature> item in assignments)
        {
            Vector3 targetPosition = Vector3.Scale(item.Key, meshFilter.transform.localScale) + transform.position;
            item.Value.transform.DOMove(targetPosition, assemblyDuration).SetEase(Ease.OutBack);
            item.Value.transform.SetParent(meshFilter.transform);
            item.Value.gameObject.layer = SphereCreatureLayer;
            currentlyPosessedMorphables.Add(item.Value);
        }
    }

    private IEnumerator EnableControlsDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        sphereCreatureMovement.enabled = true;
    }

    private Dictionary<Vector3, CanMorphIntoSphereCreature> AssignMorphablesToVertices(List<CanMorphIntoSphereCreature> morphables)
    {
        Dictionary<Vector3, CanMorphIntoSphereCreature> keyValuePairs = new Dictionary<Vector3, CanMorphIntoSphereCreature>();
        List<Vector3> usedVertices = new List<Vector3>();

        for (int i = 0; i < morphables.Count; i++)
        {
            bool found = false;
            List<Vector3> availableVertices = new List<Vector3>(vertices);
            while (!found && availableVertices.Count > 0)
            {
                int index = random.Next(availableVertices.Count);
                Vector3 randomVertex = availableVertices[index];

                if (usedVertices.Contains(randomVertex))
                {
                    availableVertices.Remove(randomVertex);
                    continue;
                }

                if (usedVertices.Any(v => Vector3.Distance(v, randomVertex) < minDistanceBetweenElements))
                {
                    availableVertices.Remove(randomVertex);
                    continue;
                }

                usedVertices.Add(randomVertex);
                keyValuePairs.Add(randomVertex, morphables[i]);
                found = true;
            }

            if (!found)
            {
                // if we couldn't find a vertex for this transform, we won't be
                // able to find vertices for any more transforms
                break;
            }
        }

        return keyValuePairs;
    }
}
