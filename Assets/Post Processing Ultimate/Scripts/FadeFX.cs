using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU {
	public class FadeFX : MonoBehaviour {
	
		//Profile that is used by camera
		public PostProcessProfile profile;
		
		//Duration of the animation
		public float FadeSeconds = 5f;
		
		//Temporary variable used during animation
		internal float timePassed = 0f;
		
		//All profile effects
		internal List<PostProcessEffectSettings> settings;
		
		//Selected effect
		internal Fade fx;
		
		void Start() {
			//Checks if profile is not empty
			if (profile != null){
				//Assigns profile to variable
				settings = profile.settings;
				//Searches for Fade effect
				foreach (PostProcessEffectSettings i in settings){
					if (i.ToString().Contains("NTEC.PPU.Fade")){
						//Assigns effect to variable
						fx = (Fade) i;
						break;
					}
				}
			}
		}
		
		void Update() {
			//Checks if effect is not empty
			if (fx != null){
				//Fade animation
				fx.Intensity.value = (timePassed += Time.deltaTime) / FadeSeconds;
			}
		}
	}
}