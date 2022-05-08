using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIKeys : MonoBehaviour {

	public Light sunlight;
	public ReflectionProbe[] probes;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F) && sunlight != null) {

			if (sunlight.enabled)
				sunlight.enabled = false;
			else
				sunlight.enabled = true;

			for(int i=0;i<probes.Length;i++) {
				if(probes[i] != null)
					probes[i].RenderProbe();
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha1)) {

			SceneManager.LoadScene ("scene_day");
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {

			SceneManager.LoadScene ("scene_night");
		}


	}
}
