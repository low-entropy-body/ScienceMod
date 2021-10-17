sampler uImage0 : register(s0);
float uTime;

float ffabs(float val)
{
    return sqrt(val * val);
}
float ffmod(float v, float m)
{
    int f = floor(v / m);
    return v - f * m;
}
float3 HUEtoRGB(float H)
{
    float R = ffabs(H * 6 - 3) - 1;
    float G = 2 - ffabs(H * 6 - 2);
    float B = 2 - ffabs(H * 6 - 4);
    return saturate(float3(R, G, B));
}

float4 rainbow(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
    return float4(HUEtoRGB(ffmod(uTime * 0.01 + coords.y, 1)), color.a);
}

technique Technique1
{
    pass RainBow
    {
        PixelShader = compile ps_2_0 rainbow();
    }
}