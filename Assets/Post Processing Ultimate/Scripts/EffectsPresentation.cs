using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU {
	public class EffectsPresentation : MonoBehaviour {
	
		//Profile that is used by camera
		public PostProcessProfile profile;
		
		//All profile effects
		internal List<PostProcessEffectSettings> settings;
		
		//Temporary variable used during animation
		internal float timePassed = 0f;
		
		//Selected effects
		internal Fade fx;
		
		void Start() {
			//Checks if profile is not empty
			if (profile != null){
				//Assigns profile to variable
				settings = profile.settings;
			}
		}
		
		void Update() {
			//33 effects, each 1 second, so 33 seconds
			if (Time.time > 1 && Time.time < 33){
				settings[(int) Mathf.Floor(Time.time)].active = true;
				settings[(int) Mathf.Floor(Time.time) - 1].active = false;
				if (settings[(int) Mathf.Floor(Time.time)].ToString().Contains("NTEC.PPU.Fade")){
					fx = (Fade) settings[(int) Mathf.Floor(Time.time)];
					fx.Intensity.value = (timePassed += Time.deltaTime);
				}
			}
		}
	}
}