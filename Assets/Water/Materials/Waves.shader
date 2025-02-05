Shader "Custom/Waves"
{
    Properties
    {
        _WaveHeight("Wave Height", Range(0, 1)) = 0.1
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float3 worldPos;
        };

        float _WaveHeight;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // No need to calculate wave effect here, it will be set from script
            o.Albedo = o.Albedo + _WaveHeight;
        }
        ENDCG
    }
        FallBack "Diffuse"
}
