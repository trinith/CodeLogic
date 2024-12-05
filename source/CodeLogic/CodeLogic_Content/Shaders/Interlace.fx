#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float2 xSize;
float4 xBGColour = float4(0, 0, 0, 255);

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};

float4 InterlacePS(VertexShaderOutput input) : COLOR
{
	float4 cOut = tex2D(SpriteTextureSampler, input.UV) * input.Color;;

	float2 xy = input.UV * xSize - 0.5;
	float y = round(xy.y);
	if (y % 2 == 0 && cOut.a != 0)
	{
		cOut.r = xBGColour.r;
		cOut.g = xBGColour.g;
		cOut.b = xBGColour.b;
	}

	return cOut;
}

technique Interlace
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL InterlacePS();
	}
};