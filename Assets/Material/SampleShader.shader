Shader "Unlit/SampleShader"
{
    Properties
    {
		_Color ("BaseColor", Color) = (1,0,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color; //vector나 color처럼 나열된 4개의 숫자를 저장할 수 있는 타입

			struct vertexInput {
				float3 positionOnObjectSpace : POSITION;
			};

			struct fragmentInput {
				float4 positionOnClipSpace : SV_POSITION;
			};

			fragmentInput vert(vertexInput input){
				float4 positionOnClipSpace = UnityObjectToClipPos(input.positionOnObjectSpace);

				fragmentInput output;
				output.positionOnClipSpace = positionOnClipSpace;

				return output;
			}

			fixed4 frag(fragmentInput input) : SV_TARGET {
				return _Color;
			
			}

            ENDCG
        }
    }
}
