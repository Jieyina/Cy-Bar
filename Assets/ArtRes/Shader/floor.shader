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

        _blurSizeXY("BlurSizeXY", Range(0,10)) = 2
        
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		 GrabPass { }
		Pass 
        {
			Tags { "LightMode"="ForwardBase" }
            
            cull Front
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
            sampler2D _GrabTexture : register(s0);
            float _blurSizeXY;

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
                float4 screenPos:TEXCOORD4;
                
			};
			
			v2f vert(a2v v) 
            {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                o.local_uv= v.uv;
                o.screenPos = float4(o.pos.x, -o.pos.y, o.pos.z, o.pos.w);
				
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
                fixed3 BaseColor = _Color.rgb*_BasePower;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Color.rgb;
                fixed3 color = lerp(BaseColor, Emission.rgb, gradient) ;
				fixed3 diffuse = _LightColor0.rgb * color  * max(0, dot(worldNormal, worldLightDir));

                float2 screenPos = i.screenPos.xy / i.screenPos.w;
                float depth= _blurSizeXY * 0.0009;

                screenPos.x = (screenPos.x + 1) * 0.5;
                screenPos.y = (screenPos.y + 1) * 0.5;

                half4 sum = half4(0,0,0,0);

                sum += tex2D( _GrabTexture, float2(screenPos.x-5.0 * depth, screenPos.y+5.0 * depth)) * 0.025;    
                sum += tex2D( _GrabTexture, float2(screenPos.x+5.0 * depth, screenPos.y-5.0 * depth)) * 0.025;

                sum += tex2D( _GrabTexture, float2(screenPos.x-4.0 * depth, screenPos.y+4.0 * depth)) * 0.05;
                sum += tex2D( _GrabTexture, float2(screenPos.x+4.0 * depth, screenPos.y-4.0 * depth)) * 0.05;

                sum += tex2D( _GrabTexture, float2(screenPos.x-3.0 * depth, screenPos.y+3.0 * depth)) * 0.09;
                sum += tex2D( _GrabTexture, float2(screenPos.x+3.0 * depth, screenPos.y-3.0 * depth)) * 0.09;

                sum += tex2D( _GrabTexture, float2(screenPos.x-2.0 * depth, screenPos.y+2.0 * depth)) * 0.12;
                sum += tex2D( _GrabTexture, float2(screenPos.x+2.0 * depth, screenPos.y-2.0 * depth)) * 0.12;

                sum += tex2D( _GrabTexture, float2(screenPos.x-1.0 * depth, screenPos.y+1.0 * depth)) *  0.15;
                sum += tex2D( _GrabTexture, float2(screenPos.x+1.0 * depth, screenPos.y-1.0 * depth)) *  0.15;

                sum += tex2D( _GrabTexture, screenPos-5.0 * depth) * 0.025;    
                sum += tex2D( _GrabTexture, screenPos-4.0 * depth) * 0.05;
                sum += tex2D( _GrabTexture, screenPos-3.0 * depth) * 0.09;
                sum += tex2D( _GrabTexture, screenPos-2.0 * depth) * 0.12;
                sum += tex2D( _GrabTexture, screenPos-1.0 * depth) * 0.15;    
                sum += tex2D( _GrabTexture, screenPos) * 0.16; 
                sum += tex2D( _GrabTexture, screenPos+5.0 * depth) * 0.15;
                sum += tex2D( _GrabTexture, screenPos+4.0 * depth) * 0.12;
                sum += tex2D( _GrabTexture, screenPos+3.0 * depth) * 0.09;
                sum += tex2D( _GrabTexture, screenPos+2.0 * depth) * 0.05;
                sum += tex2D( _GrabTexture, screenPos+1.0 * depth) * 0.025;
                
				
				return fixed4(ambient + diffuse+sum/2, _AlphaScale);
			}
			
			ENDCG
		}
        Pass 
        {
			Tags { "LightMode"="ForwardBase" }
            
            cull back
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
            sampler2D _GrabTexture : register(s0);
            float _blurSizeXY;

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
                float4 screenPos:TEXCOORD4;
                
			};
			
			v2f vert(a2v v) 
            {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                o.local_uv= v.uv;
                o.screenPos = float4(o.pos.x, -o.pos.y, o.pos.z, o.pos.w);
				
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
                fixed3 BaseColor = _Color.rgb*_BasePower;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Color.rgb;
                fixed3 color = lerp(BaseColor, Emission.rgb, gradient) ;
				fixed3 diffuse = _LightColor0.rgb * color  * max(0, dot(worldNormal, worldLightDir));

                float2 screenPos = i.screenPos.xy / i.screenPos.w;
                float depth= _blurSizeXY * 0.0009;

                screenPos.x = (screenPos.x + 1) * 0.5;
                screenPos.y = (screenPos.y + 1) * 0.5;

                half4 sum = half4(0,0,0,0);

                sum += tex2D( _GrabTexture, float2(screenPos.x-5.0 * depth, screenPos.y+5.0 * depth)) * 0.025;    
                sum += tex2D( _GrabTexture, float2(screenPos.x+5.0 * depth, screenPos.y-5.0 * depth)) * 0.025;

                sum += tex2D( _GrabTexture, float2(screenPos.x-4.0 * depth, screenPos.y+4.0 * depth)) * 0.05;
                sum += tex2D( _GrabTexture, float2(screenPos.x+4.0 * depth, screenPos.y-4.0 * depth)) * 0.05;

                sum += tex2D( _GrabTexture, float2(screenPos.x-3.0 * depth, screenPos.y+3.0 * depth)) * 0.09;
                sum += tex2D( _GrabTexture, float2(screenPos.x+3.0 * depth, screenPos.y-3.0 * depth)) * 0.09;

                sum += tex2D( _GrabTexture, float2(screenPos.x-2.0 * depth, screenPos.y+2.0 * depth)) * 0.12;
                sum += tex2D( _GrabTexture, float2(screenPos.x+2.0 * depth, screenPos.y-2.0 * depth)) * 0.12;

                sum += tex2D( _GrabTexture, float2(screenPos.x-1.0 * depth, screenPos.y+1.0 * depth)) *  0.15;
                sum += tex2D( _GrabTexture, float2(screenPos.x+1.0 * depth, screenPos.y-1.0 * depth)) *  0.15;

                sum += tex2D( _GrabTexture, screenPos-5.0 * depth) * 0.025;    
                sum += tex2D( _GrabTexture, screenPos-4.0 * depth) * 0.05;
                sum += tex2D( _GrabTexture, screenPos-3.0 * depth) * 0.09;
                sum += tex2D( _GrabTexture, screenPos-2.0 * depth) * 0.12;
                sum += tex2D( _GrabTexture, screenPos-1.0 * depth) * 0.15;    
                sum += tex2D( _GrabTexture, screenPos) * 0.16; 
                sum += tex2D( _GrabTexture, screenPos+5.0 * depth) * 0.15;
                sum += tex2D( _GrabTexture, screenPos+4.0 * depth) * 0.12;
                sum += tex2D( _GrabTexture, screenPos+3.0 * depth) * 0.09;
                sum += tex2D( _GrabTexture, screenPos+2.0 * depth) * 0.05;
                sum += tex2D( _GrabTexture, screenPos+1.0 * depth) * 0.025;
                
				
				return fixed4(ambient + diffuse+sum/2, _AlphaScale);
			}
			
			ENDCG
		}
        
        
        
	} 
	FallBack "Transparent/VertexLit"
}