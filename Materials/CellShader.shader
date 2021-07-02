Shader "Unlit/PlayerShader2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ColorActive("Color Active", Color) = (0.0627451, 0.4117647, 0.7215686, 0.4392157)
		_ColorUnactive("Color Unactive", Color) = (0, 0, 0, 0)
		_Cells("Cells", int) = 6
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

            sampler2D _MainTex;
			float4 _ColorActive;
			float4 _ColorUnactive;
			float _Cells;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

				if (col.b == 1 && col.a != 0) {
					if (_Cells / 10 >= col.a) {
						col = _ColorActive;
					} 
					else {
						col = _ColorUnactive;
					}
				}

                return col;
            }
            ENDCG
        }
    }
}
