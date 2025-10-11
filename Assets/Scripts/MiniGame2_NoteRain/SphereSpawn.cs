using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SphereSpawner : MonoBehaviour
{
    [Header("Sphere Prefab")]
    [SerializeField] private NoteSphereController spherePrefab; // Prefab maestro con el SphereController

    [Header("Spawn Boundaries")]
    [SerializeField] private Transform boundaryLeft;      // Límite izquierdo del área de spawn
    [SerializeField] private Transform boundaryRight;     // Límite derecho del área de spawn
    [SerializeField] private Transform boundaryTop;       // Límite superior del área de spawn
    [SerializeField] private Transform boundaryBottom;    // Límite inferior del área de spawn    

    // Target Notes
    private string[] targetNoteNames;     // Nombres de las notas que el jugador debe atrapar
    private float spawnIntervalTargetNotes ; // Intervalo entre spawns de notas objetivo
    private float initialDelayTargetNotes;  // Retraso inicial para notas objetivo

    // Obstacle Notes
    private float spawnIntervalObstacleNotes; // Intervalo entre spawns de notas obstáculo
    private float initialDelayObstacleNotes;  // Retraso inicial para notas obstáculo

    // Ajustes de dificultad
    [SerializeField] private float elapsedTime = 0f;      

    private List<NotePair> obstacleNotes;     // Notas que NO debe atrapar el jugador
    private List<NotePair> collectibleNotes;  // Notas que SÍ debe atrapar el jugador

    private Vector3 lastSpawnPosition = Vector3.zero;
    [SerializeField] private float minDistanceX = 1.5f; // distancia mínima entre spawns en X


    public void ConfigureSpawner( string[] targetNotes, float targetInterval, float targetDelay, float obstacleInterval, float obstacleDelay)
    {
        targetNoteNames = targetNotes;
        spawnIntervalTargetNotes = targetInterval;
        initialDelayTargetNotes = targetDelay;
        spawnIntervalObstacleNotes = obstacleInterval;
        initialDelayObstacleNotes = obstacleDelay;

        // Obtenemos la lista desde el prefab
        List<NotePair> prefabsSphere = spherePrefab.prefabsSphere;

         // Clasificamos las notas
        obstacleNotes = prefabsSphere
            .Where(note => !targetNoteNames.Contains(note.noteName))
            .ToList();

        collectibleNotes = prefabsSphere
            .Where(note => targetNoteNames.Contains(note.noteName))
            .ToList();

        StartCoroutine(SpawnObstacleNotesRoutine());
        StartCoroutine(SpawnCollectibleNotesRoutine());
    }

    
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        int safetyCounter = 0;

        do
        {
            spawnPosition = new Vector3(
                Random.Range(boundaryLeft.position.x, boundaryRight.position.x),
                boundaryTop.position.y, // siempre desde arriba
                0f
            );
            safetyCounter++;
            if (safetyCounter > 20) break;

        } while (Mathf.Abs(spawnPosition.x - lastSpawnPosition.x) < minDistanceX);

        lastSpawnPosition = spawnPosition;
        return spawnPosition;
    }       


    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // Ajuste progresivo de dificultad
        if (elapsedTime > 10 && elapsedTime < 20)
        {
            spawnIntervalObstacleNotes = 2f;
            spawnIntervalTargetNotes = 3.5f;
        }
        else if (elapsedTime > 20 && elapsedTime < 30)
        {
            spawnIntervalObstacleNotes = 1f;
            spawnIntervalTargetNotes = 4f;
        }
        else if (elapsedTime > 30 && elapsedTime < 50)
        {
            spawnIntervalObstacleNotes = 0.75f;
            spawnIntervalTargetNotes = 4.5f;
        }
        else if (elapsedTime > 50)
        {
            spawnIntervalObstacleNotes = 0.25f;
            spawnIntervalTargetNotes = 5f;
        }
    }

    private IEnumerator SpawnObstacleNotesRoutine()
    {
        yield return new WaitForSeconds(initialDelayObstacleNotes);

        while (true)
        {
            SpawnObstacleNote();
            yield return new WaitForSeconds(spawnIntervalObstacleNotes);
        }
    }

    private IEnumerator SpawnCollectibleNotesRoutine()
    {
        yield return new WaitForSeconds(initialDelayTargetNotes);

        while (true)
        {
            SpawnCollectibleNote();
            yield return new WaitForSeconds(spawnIntervalTargetNotes);
        }
    }

    private void SpawnObstacleNote()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        NoteSphereController sphereObj = Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
        int randomType = UnityEngine.Random.Range(0, obstacleNotes.Count);
        sphereObj.InitSphereType(obstacleNotes[randomType].cardType, false);
    }

    private void SpawnCollectibleNote()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        NoteSphereController sphereObj = Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
        int randomType = UnityEngine.Random.Range(0, collectibleNotes.Count);
        sphereObj.InitSphereType(collectibleNotes[randomType].cardType, true);
    }

    /* private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(boundaryLeft.position.x, boundaryRight.position.x),
            Random.Range(boundaryBottom.position.y, boundaryTop.position.y),
            0f
        );
    } */
}
