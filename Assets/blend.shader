// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Example/Blend2Textures"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
		_NextTex ("Next Texture", 2D) = "white" {}
        _Percentage ("Percentage Blend", Range(0,1)) = 0
        _UVScale ("UVScale", Float) = 1.0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // to use texture arrays we need to target DX10/OpenGLES3 which
            // is shader model 3.5 minimum
            #pragma target 3.5
            
            #include "UnityCG.cginc"

            /*struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
			};*/

			struct vertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct vertexOutput {
				float2 uv_MainTex : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

            float _Percentage;		
            float _UVScale;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NextTex;
			

			
            vertexOutput vert (vertexInput i)
            {
                vertexOutput o;
                o.pos = UnityObjectToClipPos(i.vertex);
				o.uv_MainTex = TRANSFORM_TEX(i.uv, _MainTex);
                return o;
            }
            
            float4 frag (vertexOutput i) : SV_Target
            {
				float4 mainCol = tex2D(_MainTex, i.uv_MainTex);
				float4 texTwoCol = tex2D(_NextTex, i.uv_MainTex);

				float4 blend = lerp(mainCol, texTwoCol, _Percentage);
				
				return blend;
            }
            ENDCG
        }
    }
}