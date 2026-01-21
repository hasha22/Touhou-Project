Shader "Custom/LightningBackground"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _BlendTex ("Blend Texture", 2D) = "white" {}
        _Blend ("Blend Factor", Range(0,1)) = 0
        _LightningColor ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        //Tags { "Queue"="Background" "RenderType"="Opaque" }
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
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
                float2 uv2 : TEXCOORD1;  
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            sampler2D _BlendTex;
            float4 _MainTex_ST;
            float4 _BlendTex_ST;
            float _Blend;
            float4 _LightningColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);      
                o.uv2 = TRANSFORM_TEX(v.uv, _BlendTex);    
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                /*
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                fixed4 blendCol = tex2D(_BlendTex, i.uv2);  
                
                fixed4 col = lerp(baseCol, blendCol, _Blend);
                col.rgb *= _LightningColor.rgb;
                
                return col;*/

                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= _LightningColor.rgb;
                col.a *= _LightningColor.a;  
                return col;
            }
            ENDCG
        }
    }
}