using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour {

	public void EnterClient()
	{
		Debug.Log ("EnterClient");
		SceneManager.LoadScene ("main");
	}


	public void EnterServer()
	{
		Debug.Log ("EnterServer");
		SceneManager.LoadScene ("server");
	}
}
