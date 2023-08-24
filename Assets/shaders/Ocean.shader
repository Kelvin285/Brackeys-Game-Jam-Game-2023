Shader "Unlit/Ocean"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float depth : COLOR0;
    float3 pos : COLOR1;
};

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    
                if (v.vertex.y > -100)
                {
                    o.depth = 0;
                }
                else
                {
                    o.depth = 4096;
                }
    o.pos = v.vertex + 4096.0f;
    
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
    float4 col = float4(0.1f, 0.5f, 1.0f, 0.25f);
    
    if (i.depth < 64.0f)
    {
        col = lerp(float4(0.25f, 0.5f, 1.0f, 0.25f), float4(0.1f, 0.25f, 1.0f, 1.0f), clamp(i.depth / 64.0f, 0, 1));
    }
    else if (i.depth < 128.0f)
    {
        col = lerp(float4(0.1f, 0.25f, 1.0f, 1.0f), float4(0.25f, 0.25f, 0.75f, 1.0f), clamp((i.depth - 64.0f) / 64.0f, 0, 1));
    }
    else if (i.depth < 512.0f)
    {
        col = lerp(float4(0.25f, 0.25f, 0.75f, 1.0f), float4(0.05f, 0.0f, 0.15f, 1.0f), clamp((i.depth - 128.0f) / 384.0f, 0, 1));
    }
    else if (i.depth < 1024.0f)
    {
        col = lerp(float4(0.05f, 0.0f, 0.15f, 1.0f), float4(0.0f, 0.0f, 0.0f, 1.0f), clamp((i.depth - 512.0f) / 512.0f, 0, 1));
    }
    else
    {
        col = float4(0.0f, 0.0f, 0.0f, 1.0f);
    }
    
        fixed4 bubbles = tex2D(_MainTex, fmod(i.pos.xy * 0.05f + _Time.x * 0.5f, 1));
    fixed4 light = tex2D(_MainTex, fmod(float2(i.pos.x * 0.05f + _Time.x * 0.5f, 0), 1));
    fixed4 light2 = tex2D(_MainTex, fmod(float2(-i.pos.x * 0.05f + 4096.0f + _Time.x * 0.5f, 0), 1));
    light *= light2;
    
    float2 coords = fmod(i.pos.xy * 0.1f * float2(-1, 1) + 4096.0f + float2(_Time.x, _Time.x) * 0.5f, 1);
    
    fixed4 bubbles2 = tex2D(_MainTex, coords);
    bubbles *= bubbles2;
    light *= bubbles2;
    
    col.xyz += bubbles.xyz * (1.0f - clamp(i.depth / 15.0f, 0, 1));
    col.xyz += light.xyz * (1.0f - clamp(i.depth / 32.0f, 0, 1));
    
    float foam_depth = 0.5f;
    if (i.depth < foam_depth)
    {
        float mix_val = clamp(i.depth / foam_depth, 0, 1);
        col.xyz += lerp(float3(0, 0, 0), float3(3, 3, 3), clamp((1.0f - mix_val) - bubbles.y * 4, 0, 1));
        float alpha = clamp((mix_val) - bubbles.x, 0, 1);
        col.w *= alpha;

    }
        return col;
}
            ENDCG
        }
    }
}
