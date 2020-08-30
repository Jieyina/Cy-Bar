// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/floor"
{
    Properties {
		_Color ("Base Color", Color) = (1, 1, 1, 1)
        _BasePower("BaseColor Power",Range(0,2)) = 1
		_AlphaScale ("Alpha Scale", Range(0, 1)) = 1
        _EmissionColor("Emission Color", Color) = (0,0,0,0)
        _EmissionPower("Emission Power",Range(0,2)) = 1
        _Gradient("Gradient,x:slope,y:speed,zw:gradual",vector) = (-.1,5,.3,.01)

        [Space(20)]
		_OutLineWidth ("Outline", Range(0.01, 1)) = 0.1
		_OutLineColor ("Outline Color", Color) = (0, 0, 0, 1)
        
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
		Pass 
        {
			Tags { "LightMode"="ForwardBase" }

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"
			
			fixed4 _Color;
            fixed _BasePower;
			fixed _AlphaScale;
			fixed _EmissionPower;
            fixed4 _EmissionColor;
            float4 _Gradient;
            float _Speed;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD0;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
                float2 local_uv:TEXCOORD3;
                
			};
			
			v2f vert(a2v v) 
            {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                o.local_uv= v.uv;
				
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                _Speed += _Time.x*_Gradient.y;
                _Speed += _Gradient.x*i.local_uv.y;
                float gradient = i.uv.x - (_Speed%1.5);  
                    gradient = gradient > 0 ?   
                        max(_Gradient.w - gradient, 0) / _Gradient.w:  
                        max(_Gradient.z + gradient, 0) / _Gradient.z;
				
                fixed3 Emission = _EmissionColor  * _EmissionPower ;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Color.rgb;
                fixed3 color = lerp(_Color.rgb*_BasePower, Emission.rgb, gradient) ;
				fixed3 diffuse = _LightColor0.rgb * color  * max(0, dot(worldNormal, worldLightDir));
                
				
				return fixed4(ambient + diffuse, _AlphaScale);
			}
			
			ENDCG
		}
        
	} 
	FallBack "Transparent/VertexLit"
}