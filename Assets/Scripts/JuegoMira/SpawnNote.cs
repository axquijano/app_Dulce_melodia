using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class NoteSpawner : MonoBehaviour
{
    [Header("Balloon Prefab")]
    [SerializeField] private GameObject balloonNotePrefab; // Prefab maestro con el BalloonNoteController

    [Header("Target Notes")]
    [SerializeField] private string[] targetNoteNames;     // Nombres de las notas que el jugador debe atrapar
    [SerializeField] private float spawnIntervalTargetNotes = 3f; // Intervalo entre spawns de notas objetivo
    [SerializeField] private float initialDelayTargetNotes = 1f;  // Retraso inicial para notas objetivo

    [Header("Obstacle Notes")]
    [SerializeField] private float spawnIntervalObstacleNotes = 3f; // Intervalo entre spawns de notas obstáculo
    [SerializeField] private float initialDelayObstacleNotes = 1f;  // Retraso inicial para notas obstáculo


    [Header("Spawn Boundaries")]
    [SerializeField] private Transform boundaryLeft;      // Límite izquierdo del área de spawn
    [SerializeField] private Transform boundaryRight;     // Límite derecho del área de spawn
    [SerializeField] private Transform boundaryTop;       // Límite superior del área de spawn
    [SerializeField] private Transform boundaryBottom;    // Límite inferior del área de spawn    

    [Header("Difficulty Settings")]
    [SerializeField] private float elapsedTime = 0f;      

    private List<NotePair> obstacleNotes;     // Notas que NO debe atrapar el jugador
    private List<NotePair> collectibleNotes;  // Notas que SÍ debe atrapar el jugador

    private void Awake()
    {
        // Obtenemos la lista desde el prefab
        BalloonNoteController controller = balloonNotePrefab.GetComponent<BalloonNoteController>();

        // Clasificamos las notas
        obstacleNotes = controller.prefabsBallon
            .Where(note => !targetNoteNames.Contains(note.noteName))
            .ToList();

        collectibleNotes = controller.prefabsBallon
            .Where(note => targetNoteNames.Contains(note.noteName))
            .ToList();
    }

    private void Start()
    {
        StartCoroutine(SpawnObstacleNotesRoutine());
        StartCoroutine(SpawnCollectibleNotesRoutine());
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

        GameObject balloonObj = Instantiate(balloonNotePrefab, spawnPosition, Quaternion.identity);
        BalloonNoteController balloon = balloonObj.GetComponent<BalloonNoteController>();

        int randomType = UnityEngine.Random.Range(0, obstacleNotes.Count);
        balloon.InitBalloonType(obstacleNotes[randomType].cardType, false);
    }

    private void SpawnCollectibleNote()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject balloonObj = Instantiate(balloonNotePrefab, spawnPosition, Quaternion.identity);
        BalloonNoteController balloon = balloonObj.GetComponent<BalloonNoteController>();

        int randomType = UnityEngine.Random.Range(0, collectibleNotes.Count);
        balloon.InitBalloonType(collectibleNotes[randomType].cardType, true, 10f);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(boundaryLeft.position.x, boundaryRight.position.x),
            Random.Range(boundaryBottom.position.y, boundaryTop.position.y),
            0f
        );
    }
}
