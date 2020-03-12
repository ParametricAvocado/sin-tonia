Shader "Custom/Stencil/Unlit"
{
    Properties
    {
        _Color ("Color",Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        
        [Space]

        [Toggle]_ZWrite ("Z Write", Int) = 0
	    [Enum(UnityEngine.Rendering.CompareFunction)]_ZTest("Z Test", Int) = 4
	    [Enum(UnityEngine.Rendering.CullMode)]_CullMode ("Cull Mode", Int) = 2
        _ColorMask ("Color Mask", Float) = 15

        [Header(Stencil)]

	    [Enum(UnityEngine.Rendering.CompareFunction)]_StencilComp ("Stencil Comparison", Int) = 8
        _Stencil ("Stencil ID", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]_StencilOp ("Stencil Operation", Int) = 0
        _StencilWriteMask ("Stencil Write Mask", Int) = 255
        _StencilReadMask ("Stencil Read Mask", Int) = 255
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        ZWrite [_ZWrite]
	    ZTest [_ZTest]
        ColorMask [_ColorMask]
        Cull[_CullMode]

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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
            float _Displacement;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

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
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
