#ifndef MORETOON_CUSTOM_INSERT_INCLUDED
#define MORETOON_CUSTOM_INSERT_INCLUDED

float MoreToonQuantize01(float value, float steps)
{
    float bandCount = max(2.0, floor(steps + 0.5));
    float divisions = bandCount - 1.0;
    return floor(saturate(value) * divisions + 0.5) / divisions;
}

float3 MoreToonQuantizeDirection(float3 direction, float steps)
{
    if(steps < 1.5) return direction;

    float subdivisions = max(1.0, floor(steps + 0.5));
    float3 quantized = floor(direction * subdivisions + 0.5) / subdivisions;
    float lengthSq = dot(quantized, quantized);

    return lengthSq > 1.0e-6 ? quantized * rsqrt(lengthSq) : direction;
}

float MoreToonLuminance(float3 color)
{
    return dot(color, float3(0.2126, 0.7152, 0.0722));
}

float3 MoreToonQuantizeDiffuse(
    float3 litColor,
    float3 albedo,
    float lightSteps,
    float minLight,
    float strength
)
{
    float minimum = saturate(minLight);
    float3 source = max(litColor, albedo * minimum);

    float albedoLuminance = max(MoreToonLuminance(max(albedo, 0.0)), 1.0e-4);
    float lighting = saturate(MoreToonLuminance(max(source, 0.0)) / albedoLuminance);

    float quantizedLighting = max(minimum, MoreToonQuantize01(lighting, lightSteps));
    float scale = quantizedLighting / max(lighting, 1.0e-4);
    float3 quantizedColor = source * scale;

    return lerp(litColor, quantizedColor, saturate(strength));
}

#if defined(LIL_PASS_FORWARD)
    #define MORETOON_BEFORE_SHADOW \
        if(_MoreToonEnabled > 0.5) \
        { \
            if(_MoreToonLightDirectionSteps >= 1.5) \
            { \
                fd.L = MoreToonQuantizeDirection(fd.L, _MoreToonLightDirectionSteps); \
                fd.ln = dot(fd.L, fd.N); \
            } \
            if(_MoreToonShadowSteps >= 2.0) \
            { \
                float mtShadowInput = saturate(fd.ln * 0.5 + 0.5); \
                float mtShadowQuantized = MoreToonQuantize01(mtShadowInput, _MoreToonShadowSteps); \
                float mtQuantizedLn = mtShadowQuantized * 2.0 - 1.0; \
                fd.ln = lerp(fd.ln, mtQuantizedLn, saturate(_MoreToonShadowSharpness)); \
            } \
        }

    #define MORETOON_BEFORE_REFLECTION \
        if(_MoreToonEnabled > 0.5) \
        { \
            fd.col.rgb = MoreToonQuantizeDiffuse( \
                fd.col.rgb, \
                fd.albedo, \
                _MoreToonLightSteps, \
                _MoreToonMinLight, \
                _MoreToonStrength \
            ); \
        }
#else
    #define MORETOON_BEFORE_SHADOW
    #define MORETOON_BEFORE_REFLECTION
#endif

#endif
