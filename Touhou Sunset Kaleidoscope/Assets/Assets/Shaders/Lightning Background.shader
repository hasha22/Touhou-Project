Shader "Custom/LightningBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightningColor ("Tint Color", Color) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        
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
            float4 _MainTex_ST;
            float4 _LightningColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Multiply texture by tint color (darkens/tints the background)
                col.rgb *= _LightningColor.rgb;
                
                return col;
            }
            ENDCG
        }
    }
}