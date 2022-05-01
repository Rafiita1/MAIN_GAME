using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTEC.PPU{
	public class Tooltips {
	
		public static string List(string name){
			switch (name){
				//=====Input=====
				case "_Int":
				return "Int global variable";
				case "_Float":
				return "Float global variable";
				case "_IntSlider":
				return "Int global variable";
				case "_FloatSlider":
				return "Float global variable";
				case "_Color":
				return "Color global variable";
				case "_Vector2":
				return "Vector2 global variable";
				case "_Vector3":
				return "Vector3 global variable";
				case "_Vector4":
				return "Vector4 global variable";
				case "_Texture2D":
				return "Texture2D global variable";
				case "_Spline":
				return "Spline global variable";
				case "_Bool":
				return "Bool global variable";
				//=====Camera=====
				case "CameraOutput":
				return "Single pixel color value";
				case "CameraInput":
				return "Main texture sampler";
				case "CameraDepth":
				return "Main texture depth";
				case "DefaultUV":
				return "Default texture coordinates";
				case "StereoUV":
				return "Stereo texture coordinates";
				case "SpecialTex":
				return "Special texture sampler";
				case "TempTex":
				return "Temp texture sampler";
				case "TexelSize":
				return "Texture pixel size";
				case "WorldPosition":
				return "Global transform pixel position";
				//=====FE=====
				case "Output":
				return "Function output";
				case "Input1":
				return "Single index argument";
				case "Input2":
				return "Double index argument";
				case "Input3":
				return "Triple index argument";
				case "Input4":
				return "Quadruple index argument";
				//=====Value=====
				case "Value1":
				return "Single index value (1 + LMB)";
				case "Value2":
				return "Double index value (2 + LMB)";
				case "Value3":
				return "Triple index value (3 + LMB)";
				case "Value4":
				return "Quadruple index value (4 + LMB)";
				//=====HLSL Function=====
				case "Abs":
				return "Returns the absolute value of the specified value";
				case "Acos":
				return "Returns the arccosine of the specified value";
				case "All":
				return "Determines if all components of the specified value are non-zero";
				case "Any":
				return "Determines if any components of the specified value are non-zero";
				case "Asin":
				return "Returns the arcsine of the specified value";
				case "Atan":
				return "Returns the arctangent of the specified value";
				case "Atan2":
				return "Returns the arctangent of two values (x,y)";
				case "Ceil":
				return "Returns the smallest integer value that is greater than or equal to the specified value";
				case "Clamp":
				return "Clamps the specified value to the specified minimum and maximum range (LAlt + C + LMB)";
				case "Cos":
				return "Returns the cosine of the specified value";
				case "Cosh":
				return "Returns the hyperbolic cosine of the specified value";
				case "Cross":
				return "Returns the cross product of two floating-point, 3D vectors";
				case "Ddx":
				return "Returns the partial derivative of the specified value with respect to the screen-space x-coordinate";
				case "Ddy":
				return "Returns the partial derivative of the specified value with respect to the screen-space y-coordinate";
				case "Degrees":
				return "Converts the specified value from radians to degrees";
				case "Distance":
				return "Returns a distance scalar between two vectors";
				case "Dot":
				return "Returns the dot product of two vectors (LAlt + D + LMB)";
				case "Exp":
				return "Returns the base-e exponential, or ex, of the specified value (E + LMB)";
				case "Exp2":
				return "Returns the base 2 exponential, or 2x, of the specified value (LAlt + E + LMB)";
				case "Floor":
				return "Returns the greatest integer which is less than or equal to x (F + LMB)";
				case "Fmod":
				return "Returns the floating-point remainder of x/y";
				case "Frac":
				return "Returns the fractional (or decimal) part of x, which is greater than or equal to 0 and less than 1";
				case "Fwidth":
				return "Returns the absolute value of the partial derivatives of the specified value (LAlt + F + LMB)";
				case "Isfinite":
				return "Determines if the specified floating-point value is finite";
				case "Isinf":
				return "Determines if the specified value is infinite";
				case "Isnan":
				return "Determines if the specified value is NAN or QNAN";
				case "Ldexp":
				return "Returns the result of multiplying the specified value by two, raised to the power of the specified exponent";
				case "Length":
				return "Returns the length of the specified floating-point vector (LAlt + L + LMB)";
				case "Lerp":
				return "Performs a linear interpolation (L + LMB)";
				case "Log":
				return "Returns the base-e logarithm of the specified value";
				case "Log2":
				return "Returns the base-2 logarithm of the specified value";
				case "Log10":
				return "Returns the base-10 logarithm of the specified value";
				case "Max":
				return "Selects the greater of x and y";
				case "Min":
				return "Selects the lesser of x and y";
				case "Normalize":
				return "Normalizes the specified floating-point vector according to x / length(x) (N + LMB)";
				case "Pow":
				return "Returns the specified value raised to the specified power (P + LMB)";
				case "Radians":
				return "Converts the specified value from degrees to radians";
				case "Reflect":
				return "Returns a reflection vector using an incident ray and a surface normal (LAlt + R + LMB)";
				case "Refract":
				return "Returns a refraction vector using an entering ray, a surface normal, and a refraction index";
				case "Round":
				return "Rounds the specified value to the nearest integer (R + LMB)";
				case "Rsqrt":
				return "Returns the reciprocal of the square root of the specified value";
				case "Saturate":
				return "Clamps the specified value within the range of 0 to 1";
				case "Sign":
				return "Returns the sign of x";
				case "Sin":
				return "Returns the sine of the specified value (LAlt + S + LMB)";
				case "Sinh":
				return "Returns the hyperbolic sine of the specified value";
				case "Smoothstep":
				return "Returns a smooth Hermite interpolation between 0 and 1, if x is in the range [min, max]";
				case "Sqrt":
				return "Returns the square root of the specified floating-point value, per component";
				case "Step":
				return "Compares two values, returning 0 or 1 based on which value is greater";
				case "Tan":
				return "Returns the tangent of the specified value (T + LMB)";
				case "Tanh":
				return "Returns the hyperbolic tangent of the specified value";
				case "Trunc":
				return "Truncates a floating-point value to the integer component (LAlt + T + LMB)";
				//=====Arithmetic=====
				case "Add":
				return "Combines numbers (A + LMB)";
				case "Av":
				return "Arithmetic mean, average of inputs";
				case "Sub":
				return "Finds difference between numbers (S + LMB)";
				case "Mul":
				return "Combines multipliers and the multiplicands (M + LMB)";
				case "Div":
				return "Finds the quotient of numbers (D + LMB)";
				case "Mod":
				return "Finds the remainder after division (LAlt + M + LMB)";
				//=====Constant=====
				case "HALF_MAX":
				return "Constant value: 65504.0";
				case "EPSILON":
				return "Constant value: 1.0e-4";
				case "PI":
				return "Constant value: 3.14159265359";
				case "TWO_PI":
				return "Constant value: 6.28318530718";
				case "FOUR_PI":
				return "Constant value: 12.56637061436";
				case "INV_PI":
				return "Constant value: 0.31830988618";
				case "INV_TWO_PI":
				return "Constant value: 0.15915494309";
				case "INV_FOUR_PI":
				return "Constant value: 0.07957747155";
				case "HALF_PI":
				return "Constant value: 1.57079632679";
				case "INV_HALF_PI":
				return "Constant value: 0.636619772367";
				case "FLT_EPSILON":
				return "Smallest positive number, such that 1.0 + FLT_EPSILON != 1.0: 1.192092896e-07";
				case "FLT_MIN":
				return "Minimum representable positive floating-point number: 1.175494351e-38";
				case "FLT_MAX":
				return "Maximum representable floating-point number: 3.402823466e+38";
				//=====Data=====
				case "WorldSpace":
				return "_WorldSpaceCameraPos";
				case "Projection":
				return "_ProjectionParams";
				case "Luminance":
				return "unity_ColorSpaceLuminance";
				case "DeltaTime":
				return "unity_DeltaTime";
				case "Ortho":
				return "unity_OrthoParams";
				case "ZBuffer":
				return "_ZBufferParams";
				case "Screen":
				return "_ScreenParams";
				case "Time":
				return "_Time";
				case "SinTime":
				return "_SinTime";
				case "CosTime":
				return "_CosTime";
				//=====Logic=====
				case "If":
				return "Checks if statement is true or false and returns values (I + LMB)";
				case "Compare":
				return "Compares two values";
				case "And":
				return "Handles AND statement";
				case "Or":
				return "Handles OR statement";
				case "Not":
				return "Handles NOT statement";
				//=====Variable=====
				case "Variable1":
				return "Single index variable (LAlt + 1 + LMB)";
				case "Variable2":
				return "Double index variable (LAlt + 2 + LMB)";
				case "Variable3":
				return "Triple index variable (LAlt + 3 + LMB)";
				case "Variable4":
				return "Quadruple index variable (LAlt + 4 + LMB)";
				case "Iterator":
				return "Increasing value in a loop";
				case "VarLoop1":
				return "Performs arithmetic operations on a single value";
				case "VarLoop2":
				return "Performs arithmetic operations on a double value";
				case "VarLoop3":
				return "Performs arithmetic operations on a triple value";
				case "VarLoop4":
				return "Performs arithmetic operations on a quadruple value";
				//=====Custom=====
				case "Custom":
				return "Custom function (C + LMB)";
				case "Custom1":
				return "Single index custom function";
				case "Custom2":
				return "Double index custom function";
				case "Custom3":
				return "Triple index custom function";
				case "Custom4":
				return "Quadruple index custom function";
				//=====PPS Function=====
				case "AnyIsNan":
				return "Determines if any components of the specified value are non-zero";
				case "DecodeStereo":
				return "Decodes normals stored in _CameraDepthNormalsTexture";
				case "FastSign":
				return "Returns the sign of x";
				case "GradientNoise":
				return "Interleaved gradient function from Jimenez 2014";
				case "IsNan":
				return "Determines if the specified value is NAN";
				case "Linear01Depth":
				return "Handles orthographic projection correctly";
				case "LinearEyeDepth":
				return "Computes linear eye depth space from value";
				case "Max3":
				return "Selects the greatest of x, y and z";
				case "Min3":
				return "Selects the least of x, y and z";
				case "PositivePow":
				return "PositivePow remove this warning when you know the value is positive and avoid inf/NAN";
				case "Rcp":
				return "Returns 1.0 / value";
				case "SafeHDR":
				return "Clamps HDR value within a safe range";
				case "TriangleVertToUV":
				return "Vertex manipulation";
				//=====Predefined Macro=====
				case "Checker":
				return "Checks if destined macro is true or false";
				case "NearClipValue":
				return "Defined to the value of near clipping plane. Direct3D-like platforms use 0.0 while OpenGL-like platforms use –1.0";
				case "StartsAtTop":
				return "Always defined with value of 1 or 0. A value of 1 is on platforms where Texture V coordinate is 0 at the \"top\" of the Texture. Direct3D-like platforms use value of 1; OpenGL-like platforms use value of 0";
				case "Target":
				return "Defined to a numeric value that matches the Shader target compilation model";
				case "Version":
				return "Contains the numeric value of the Unity version";
				//=====Default=====
				default:
				return "PPU Node";
			}
		}
	}
}