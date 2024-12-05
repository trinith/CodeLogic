#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

Texture2D SpriteTexture;
sampler2D InputSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

float xIntensity = 1;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};

float4 FlashShader(VertexShaderOutput input) : COLOR
{
	// epicentre is half at bottomish
	float2 epi = float2(0.5, 0.9);
	float2 cur = input.UV;
	float2 v = float2(cur.x - epi.x, cur.y - epi.y);
	float len = sqrt(v.x * v.x + v.y * v.y);
	if (len > 0.5)
		len = 0.5;

	float4 c = tex2D(InputSampler, input.UV) * input.Color;

	float m = 1 - len / 0.5;
	m *= m;
	c.rgb *= (1 + m) * xIntensity;
	
	return c;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL FlashShader();
	}
};