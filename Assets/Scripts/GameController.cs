using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int mapSizeX = 20;
    int mapSizeY = 10;

    public GameObject player;

    public GameObject holePrefab;
    private List<GameObject> holes = new List<GameObject>();
    public GameObject animalPrefab;
    private List<GameObject> animals = new List<GameObject>();

    public float timeLimit = 60.0f;
    public float timeRemaining;
    public int targetCount = 10;
    public int caughtCount;

    public int score = 0;
    public int level = 0;
    public bool gameOver = false;

    public static GameController instance;
    void Awake()
    {
        holePrefab.SetActive(false);
        animalPrefab.SetActive(false);

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
        score = 0;
        NewLevel(level);
    }

    void Update()
    {
        if (!gameOver)
        {
            timeRemaining -= Time.deltaTime;

            // check if time is up
            if (timeRemaining <= 0)
            {
                GameOver();
            }
            UpdateUI();
        }
    }

    void NewLevel(int level)
    {
        timeRemaining = timeLimit + level * 10;

        caughtCount = 0;
        targetCount = 10 + level * 5;

        // reset player position
        player.transform.position = new Vector3(0, 0, 0);

        // generate new board
        // TODO

        // // generate holes
        GenerateHoles(10);

        // set all animals to inactive
        foreach (GameObject animal in animals)
        {
            animal.SetActive(false);
        }
        // start spawning new animals
        StartCoroutine(SpawnAnimals(0.5f, 2.0f));
    }

    void GameOver()
    {
        gameOver = true;
    }

    public void RestartGame()
    {
        gameOver = false;
        Start();
    }

    void GenerateHoles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // generate a random position for the hole
            int holeX = Random.Range(-mapSizeX/2, mapSizeX/2);
            int holeY = Random.Range(-mapSizeY/2, mapSizeY/2);

            // try to get hole from list, if there is no hole in the list, create a new one
            try {
                GameObject hole = holes[i];
                hole.transform.position = new Vector3(holeX, holeY, 0);
            } catch {
                GameObject hole = Instantiate(holePrefab, new Vector3(holeX, holeY, 0), Quaternion.identity);
                holes.Add(hole);
                hole.SetActive(true);
            }
        }
    }

    IEnumerator SpawnAnimals(float min_wait, float max_wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(min_wait, max_wait));

            if (holes.Count > 0 && animals.Count < targetCount) {
                int startHoleIndex = Random.Range(0, holes.Count);
                int targetHoleIndex = Random.Range(0, holes.Count);

                // make sure the start and target holes are different
                while (targetHoleIndex == startHoleIndex)
                {
                    targetHoleIndex = Random.Range(0, holes.Count);
                }

                // try to get animal from pool and set it to the start hole
                GameObject animal = animals.Find(a => !a.activeSelf);
                if (animal == null)
                {
                    // if there is no animal in the pool, create a new one
                    animal = Instantiate(animalPrefab, holes[startHoleIndex].transform.position, Quaternion.identity); 
                    animals.Add(animal);
                }
                AnimalController animalController = animal.GetComponent<AnimalController>();

                animal.transform.position = holes[startHoleIndex].transform.position;
                animalController.SetTargetHole(holes[targetHoleIndex]);
                animalController.timeOnField = 0.0f;

                // set the animal to active
                animal.SetActive(true);
            }
        }
    }

    public void OnAnimalCaught()
    {
        caughtCount++;
        score += 100 + (int)(level * 10);
        UpdateUI();

        if (caughtCount >= targetCount)
        {
            level++;
            NewLevel(level);
        }
    }

    void UpdateUI()
    {
        UIController.instance.Update();
    }

}