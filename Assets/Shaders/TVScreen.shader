﻿Shader "Custom/TVScreen"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _TestTex ("Test (RGB)", 2D) = "white" {}
        _StaticTex ("Noise (RGB)", 2D) = "white" {}

        [Space]

		_TestBlend("Test", Range(0,1)) = 0
		_StaticBlend("Static", Range(0,1)) = 0

        [Space]

		_VBlankError("Vblank Error", Float) = 0
		_HDistorsion("Horizontal Distorsion", Float) = 0

        [Space]

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

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
        LOD 200
        
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

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _TestTex;
        sampler2D _StaticTex;

		float _TestBlend;
		float _StaticBlend;

		float _VBlankError;
		float _HDistorsion;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv =  IN.uv_MainTex;

			uv.x += sin(uv.y+_Time.y) * _HDistorsion;

			uv.y += _VBlankError;

            fixed4 c = tex2D (_MainTex, uv);
            fixed4 test = tex2D (_TestTex,  uv);
            fixed4 stat = tex2D (_StaticTex, cos(_Time.w*3 % 1) + IN.uv_MainTex * 2);

            o.Albedo = lerp(lerp(c.rgb,test,_TestBlend), stat, _StaticBlend) *_Color;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
