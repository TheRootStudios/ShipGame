using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField gameTime;
    public TMP_InputField spawnTime;

	private void Start()
	{

		if(PlayerPrefs.GetFloat("gameTime") <= 0)
		{
			PlayerPrefs.SetFloat("gameTime", 60);
		}

		if (PlayerPrefs.GetFloat("spawnTime") <= 0)
		{
			PlayerPrefs.SetFloat("spawnTime", 5);
		}


		gameTime.text = PlayerPrefs.GetFloat("gameTime").ToString();
		spawnTime.text = PlayerPrefs.GetFloat("spawnTime").ToString();
	}

	public void Play()
    {
        SceneManager.LoadScene(1);
    }


	public void SaveKeys()
	{
		PlayerPrefs.SetFloat("gameTime", float.Parse(gameTime.text));
		PlayerPrefs.SetFloat("spawnTime", float.Parse(spawnTime.text));
	}
}
