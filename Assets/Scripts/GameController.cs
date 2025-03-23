using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int mapSize = 20;

    public GameObject player;

    private GameObject holePrefab;
    public List<GameObject> holes = new List<GameObject>();
    private GameObject animalPrefab;
    public List<GameObject> animals = new List<GameObject>();

    public float timeLimit = 60.0f;
    public float timeRemaining;
    public int targetCount = 10;
    private int caughtCount;

    public int score = 0;
    public int level = 0;
    public bool gameOver = false;

    public static GameController instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        level = 0;
        NewLevel(level);
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        UpdateUI();

        if (timeRemaining <= 0)
        {
            gameOver = true;
        }
    }

    void NewLevel(int level)
    {
        timeRemaining = timeLimit + level * 10;

        caughtCount = 0;
        targetCount = 10 + level * 5;

        // reset player position
        player.transform.position = new Vector3(mapSize / 2, mapSize / 2, 0);

        // generate new board
        // TODO

        // generate new holes
        holes.ForEach(Destroy);
        holes.Clear();

        GenerateHoles(10);

        // start spawning new animals
        animals.ForEach(Destroy);
        animals.Clear();

        StartCoroutine(SpawnAnimals(1.0f, 3.0f));

        UpdateUI();
    }

    void RestartGame()
    {
        level = 0;
        NewLevel(level);
        score = 0;
    }

    void GenerateHoles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // generate a random position for the hole
            int holeX = Random.Range(0, mapSize);
            int holeY = Random.Range(0, mapSize);

            // instantiate the hole
            GameObject hole = Instantiate(holePrefab, new Vector3(holeX, holeY, 0), Quaternion.identity);
            holes.Add(hole);
        }
    }
    IEnumerator SpawnAnimals(float min_wait, float max_wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(min_wait, max_wait));

            int startHoleIndex = Random.Range(0, holes.Count);
            int targetHoleIndex = Random.Range(0, holes.Count);

            // make sure the start and target holes are different
            while (targetHoleIndex == startHoleIndex)
            {
                targetHoleIndex = Random.Range(0, holes.Count);
            }

            GameObject animal = Instantiate(animalPrefab, holes[startHoleIndex].transform.position, Quaternion.identity);
            AnimalController animalController = animal.GetComponent<AnimalController>();
            animalController.targetHole = holes[targetHoleIndex];
            animals.Add(animal);

            // add the animal to the list of animals
            animals.Add(animal);
        }
    }

    public void OnAnimalCaught()
    {
        caughtCount++;
        UpdateUI();

        if (caughtCount >= targetCount)
        {
            level++;
            NewLevel(level);
        }
    }

    void UpdateUI()
    {
        UI.instance.Update();
    }

}