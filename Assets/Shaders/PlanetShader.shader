Shader "Unlit/PlanetShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        [PowerSlider(4)] _AtmosphereWidth("Atmosphere width", Range(1,1.5)) = 1
        [PowerSlider(4)] _Ambient ("Ambient", Range(0,5)) = 0
        [PowerSlider(4)] _Intensivity ("Intensivity", Range(0,5))=1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 100
        
        blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 diff : COLOR0;
            };



            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _Color;
            float _AtmosphereWidth;
            float _Ambient;
            float _Intensivity;




            
            v2f vert (appdata v)
            {
                v2f o;

                v.vertex.xyz *= _AtmosphereWidth;
                o.vertex = UnityObjectToClipPos(v.vertex);

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);

                half n1 = max(0,dot(worldNormal, _WorldSpaceLightPos0.xyz));

                o.diff=n1*_LightColor0;
                o.diff.rgb += ShadeSH9(half4(worldNormal,1));                
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.diff*_Color*_Intensivity+_Ambient*_Color;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
