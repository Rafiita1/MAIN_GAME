using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTEC.PPU{
	public class Creator {
	
		public static VisualElement List(string name){
			switch (name){
				//=====Input=====
				case "_Int":
				return new _Int();
				case "_Float":
				return new _Float();
				case "_IntSlider":
				return new _IntSlider();
				case "_FloatSlider":
				return new _FloatSlider();
				case "_Color":
				return new _Color();
				case "_Vector2":
				return new _Vector2();
				case "_Vector3":
				return new _Vector3();
				case "_Vector4":
				return new _Vector4();
				case "_Texture2D":
				return new _Texture2D();
				case "_Spline":
				return new _Spline();
				case "_Bool":
				return new _Bool();
				//=====Camera=====
				case "CameraOutput":
				return new CameraOutput();
				case "CameraInput":
				return new CameraInput();
				case "CameraDepth":
				return new CameraDepth();
				case "DefaultUV":
				return new DefaultUV();
				case "StereoUV":
				return new StereoUV();
				case "SpecialTex":
				return new SpecialTex();
				case "TempTex":
				return new TempTex();
				case "TexelSize":
				return new TexelSize();
				case "WorldPosition":
				return new WorldPosition();
				//=====FE=====
				case "Output":
				return new Output();
				case "Input1":
				return new Input1();
				case "Input2":
				return new Input2();
				case "Input3":
				return new Input3();
				case "Input4":
				return new Input4();
				//=====Value=====
				case "Value1":
				return new Value1();
				case "Value2":
				return new Value2();
				case "Value3":
				return new Value3();
				case "Value4":
				return new Value4();
				//=====HLSL Function=====
				case "Abs":
				return new Abs();
				case "Acos":
				return new Acos();
				case "All":
				return new All();
				case "Any":
				return new Any();
				case "Asin":
				return new Asin();
				case "Atan":
				return new Atan();
				case "Atan2":
				return new Atan2();
				case "Ceil":
				return new Ceil();
				case "Clamp":
				return new Clamp();
				case "Cos":
				return new Cos();
				case "Cosh":
				return new Cosh();
				case "Cross":
				return new Cross();
				case "Ddx":
				return new Ddx();
				case "Ddy":
				return new Ddy();
				case "Degrees":
				return new Degrees();
				case "Distance":
				return new Distance();
				case "Dot":
				return new Dot();
				case "Exp":
				return new Exp();
				case "Exp2":
				return new Exp2();
				case "Floor":
				return new Floor();
				case "Fmod":
				return new Fmod();
				case "Frac":
				return new Frac();
				case "Fwidth":
				return new Fwidth();
				case "Isfinite":
				return new Isfinite();
				case "Isinf":
				return new Isinf();
				case "Isnan":
				return new Isnan();
				case "Ldexp":
				return new Ldexp();
				case "Length":
				return new Length();
				case "Lerp":
				return new Lerp();
				case "Log":
				return new Log();
				case "Log2":
				return new Log2();
				case "Log10":
				return new Log10();
				case "Max":
				return new Max();
				case "Min":
				return new Min();
				case "Normalize":
				return new Normalize();
				case "Pow":
				return new Pow();
				case "Radians":
				return new Radians();
				case "Reflect":
				return new Reflect();
				case "Refract":
				return new Refract();
				case "Round":
				return new Round();
				case "Rsqrt":
				return new Rsqrt();
				case "Saturate":
				return new Saturate();
				case "Sign":
				return new Sign();
				case "Sin":
				return new Sin();
				case "Sinh":
				return new Sinh();
				case "Smoothstep":
				return new Smoothstep();
				case "Sqrt":
				return new Sqrt();
				case "Step":
				return new Step();
				case "Tan":
				return new Tan();
				case "Tanh":
				return new Tanh();
				case "Trunc":
				return new Trunc();
				//=====Arithmetic=====
				case "Add":
				return new Add();
				case "Av":
				return new Av();
				case "Sub":
				return new Sub();
				case "Mul":
				return new Mul();
				case "Div":
				return new Div();
				case "Mod":
				return new Mod();
				//=====Constant=====
				case "HALF_MAX":
				return new HALF_MAX();
				case "EPSILON":
				return new EPSILON();
				case "PI":
				return new PI();
				case "TWO_PI":
				return new TWO_PI();
				case "FOUR_PI":
				return new FOUR_PI();
				case "INV_PI":
				return new INV_PI();
				case "INV_TWO_PI":
				return new INV_TWO_PI();
				case "INV_FOUR_PI":
				return new INV_FOUR_PI();
				case "HALF_PI":
				return new HALF_PI();
				case "INV_HALF_PI":
				return new INV_HALF_PI();
				case "FLT_EPSILON":
				return new FLT_EPSILON();
				case "FLT_MIN":
				return new FLT_MIN();
				case "FLT_MAX":
				return new FLT_MAX();
				//=====Data=====
				case "WorldSpace":
				return new WorldSpace();
				case "Projection":
				return new Projection();
				case "Luminance":
				return new Luminance();
				case "DeltaTime":
				return new DeltaTime();
				case "Ortho":
				return new Ortho();
				case "ZBuffer":
				return new ZBuffer();
				case "Screen":
				return new Screen();
				case "Time":
				return new Time();
				case "SinTime":
				return new SinTime();
				case "CosTime":
				return new CosTime();
				//=====Logic=====
				case "If":
				return new If();
				case "Compare":
				return new Compare();
				case "And":
				return new And();
				case "Or":
				return new Or();
				case "Not":
				return new Not();
				//=====Variable=====
				case "Variable1":
				return new Variable1();
				case "Variable2":
				return new Variable2();
				case "Variable3":
				return new Variable3();
				case "Variable4":
				return new Variable4();
				case "Iterator":
				return new Iterator();
				case "VarLoop1":
				return new VarLoop1();
				case "VarLoop2":
				return new VarLoop2();
				case "VarLoop3":
				return new VarLoop3();
				case "VarLoop4":
				return new VarLoop4();
				//=====Custom=====
				case "Custom":
				return new Custom();
				case "Custom1":
				return new Custom();
				case "Custom2":
				return new Custom();
				case "Custom3":
				return new Custom();
				case "Custom4":
				return new Custom();
				//=====PPS Function=====
				case "AnyIsNan":
				return new AnyIsNan();
				case "DecodeStereo":
				return new DecodeStereo();
				case "FastSign":
				return new FastSign();
				case "GradientNoise":
				return new GradientNoise();
				case "IsNan":
				return new IsNan();
				case "Linear01Depth":
				return new Linear01Depth();
				case "LinearEyeDepth":
				return new LinearEyeDepth();
				case "Max3":
				return new Max3();
				case "Min3":
				return new Min3();
				case "PositivePow":
				return new PositivePow();
				case "Rcp":
				return new Rcp();
				case "SafeHDR":
				return new SafeHDR();
				case "TriangleVertToUV":
				return new TriangleVertToUV();
				//=====Predefined Macro=====
				case "Checker":
				return new Checker();
				case "NearClipValue":
				return new NearClipValue();
				case "StartsAtTop":
				return new StartsAtTop();
				case "Target":
				return new Target();
				case "Version":
				return new Version();
				//=====Default=====
				default:
				return new CameraInput();
			}
		}
	}
}