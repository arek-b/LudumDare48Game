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

    private Vector3[] vertices;

    private System.Random random;

    private List<Transform> currentlyPosessedTransforms = new List<Transform>();

    private void Awake()
    {
        vertices = meshFilter.mesh.vertices;
        random = new System.Random();
        meshRenderer.enabled = false;
    }

    public void ReleaseTransforms()
    {
        foreach (Transform item in currentlyPosessedTransforms)
        {
            item.SetParent(null);
            item.gameObject.layer = 0;
            item.GetComponent<CanMorphIntoSphereCreature>().ActivatePhysics();
        }

        currentlyPosessedTransforms.Clear();
    }

    private Vector3 NewScale(List<Transform> transforms)
    {
        float newScale = transforms.Count / 5f;
        return new Vector3(newScale, newScale, newScale);
    }

    private Vector3 NewPosition(List<Transform> transforms)
    {
        float totalX = 0f;
        float totalZ = 0f;
        foreach (Transform item in transforms)
        {
            totalX += item.position.x;
            totalZ += item.position.z;
        }
        float centerX = totalX / transforms.Count;
        float centerZ = totalZ / transforms.Count;
        float y = transform.position.y;

        if (Physics.Raycast(meshFilter.transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            y = hitInfo.point.y + (meshFilter.mesh.bounds.size.y * meshFilter.transform.localScale.y / 2);
        }

        return new Vector3(centerX, y, centerZ);
    }

    public void BeginAssembly(List<Transform> transforms)
    {
        meshFilter.transform.localScale = NewScale(transforms);
        transform.position = NewPosition(transforms);
        var vertexAssignment = AssignTransformsToVertices(transforms);
        MoveTransformsToVertices(vertexAssignment);
        StartCoroutine(EnableControlsDelayed(assemblyDuration));
        playerCameraRotation.enabled = true;
    }

    private void MoveTransformsToVertices(Dictionary<Vector3, Transform> assignments)
    {
        const int SphereCreatureLayer = 10;

        foreach (KeyValuePair<Vector3, Transform> item in assignments)
        {
            Vector3 targetPosition = Vector3.Scale(item.Key, meshFilter.transform.localScale) + transform.position;
            item.Value.DOMove(targetPosition, assemblyDuration).SetEase(Ease.OutBack);
            item.Value.SetParent(meshFilter.transform);
            item.Value.gameObject.layer = SphereCreatureLayer;
            currentlyPosessedTransforms.Add(item.Value);
        }
    }

    private IEnumerator EnableControlsDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        sphereCreatureMovement.enabled = true;
    }

    private Dictionary<Vector3, Transform> AssignTransformsToVertices(List<Transform> transforms)
    {
        Dictionary<Vector3, Transform> keyValuePairs = new Dictionary<Vector3, Transform>();
        List<Vector3> usedVertices = new List<Vector3>();

        for (int i = 0; i < transforms.Count; i++)
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
                keyValuePairs.Add(randomVertex, transforms[i]);
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
