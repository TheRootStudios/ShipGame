using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Boats")]
    public PlayerBoat Player;
    public Boat[] enemies;
    public GameObject lifeBarPrefab;
    public Transform[] spawnPoints;

    [Header("Match")]
    public float matchTime;
    public float spawnTime;
    float spawnCounter;
    public int points;
    bool lose = false;

    [Header("UI")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI EndPanelMsg;
    public GameObject EndPanel;

    // Start is called before the first frame update
    void Start()
    {
        Player.OnDeath += (player) =>
        {
            PlayerLose();
        };

        matchTime = PlayerPrefs.GetFloat("gameTime");
        spawnTime = PlayerPrefs.GetFloat("spawnTime");

        spawnCounter = spawnTime;
        Time.timeScale = 1;

        pointsText.text = points.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            if(EndPanel.activeInHierarchy && !lose)
			{
                Time.timeScale = 1;
                EndPanel.SetActive(false);
			}
			else
			{
                InitializePanel();
			}
        }

		if (lose) { return; }

        if(matchTime > 0)
		{
            matchTime -= Time.deltaTime;
		} else
		{
            matchTime = 0;
		}


        timeText.text = TimeSpan.FromSeconds(matchTime).ToString("mm':'ss");


        if(matchTime == 0)
		{
            lose = true;
            InitializePanel();
        }



        spawnCounter += Time.deltaTime;

        if(spawnCounter >= spawnTime)
		{
            EnemyBoat enemy = GameObject.Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length)],spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Length)].position,Quaternion.identity).GetComponent<EnemyBoat>();
            enemy.target = Player;
            spawnCounter = 0f;

            LifeBar life = GameObject.Instantiate(lifeBarPrefab).GetComponent<LifeBar>();
            life.owner = enemy;

            enemy.OnDeath += (enemy) =>
            {
                EnemyKilled(enemy);
            };
        }
    }


    public void PlayerLose()
	{
        Player.OnDeath -= (joinedroom) =>
        {
            PlayerLose();
        };

        lose = true;

        InitializePanel();
    }

    public void EnemyKilled(Boat enemy)
	{
        enemy.OnDeath -= (enemy) =>
        {
            EnemyKilled(enemy);
        };

        points += 100;

        pointsText.text = points.ToString();
    }

    public void InitializePanel()
	{
        Time.timeScale = 0;

		if (!lose)
		{
            EndPanelMsg.text = "";
		}
		else
		{
            EndPanelMsg.text = "Pontuação Final: " + points.ToString();
        }

        EndPanel.SetActive(true);

    }


    public void PlayAgain()
	{
        SceneManager.LoadScene(1);
	}

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
