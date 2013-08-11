float4x4 WorldViewProjection;
Texture myTexture;

sampler ColoredTextureSampler=sampler_state
{
	texture=<myTexture>;
	magfilter=LINEAR;
	minfilter=LINEAR;
	mipfilter=LINEAR;
	AddressU=MIRROR;
	AddressV=MIRROR;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 textureCoordinates: TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 textureCoordinates: TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
	output.Position=mul(input.Position, WorldViewProjection);
	output.textureCoordinates=input.textureCoordinates;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(ColoredTextureSampler, input.textureCoordinates);
    return color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
