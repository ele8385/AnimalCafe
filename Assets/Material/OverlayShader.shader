Shader "Unlit/OverlayShader" {
    Properties
    {
        _Color ("Overlay Color", Color) = (1, 1, 1, 1)
        _Sprite ("Sprite", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
 
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
 
            sampler2D _Sprite;
            float4 _Color;
 
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_Sprite, i.uv);
                fixed3 overlayColor = _Color.rgb * texColor.rgb;
                fixed3 finalColor = texColor.rgb + overlayColor * (1 - texColor.rgb);
                return fixed4(finalColor, texColor.a);
            }
            ENDCG
        }
    }
}