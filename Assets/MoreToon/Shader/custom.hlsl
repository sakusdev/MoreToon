//----------------------------------------------------------------------------------------------------------------------
// MoreToon custom hooks

#define LIL_CUSTOM_PROPERTIES \
    float _MoreToonEnabled; \
    float _MoreToonStrength; \
    float _MoreToonShadowSteps; \
    float _MoreToonShadowSharpness; \
    float _MoreToonLightDirectionSteps; \
    float _MoreToonLightSteps; \
    float _MoreToonMinLight;

#define LIL_CUSTOM_TEXTURES

// The actual bodies are defined in custom_insert.hlsl so pass-specific
// macros such as LIL_PASS_FORWARD are already available when expanded.
#define BEFORE_SHADOW MORETOON_BEFORE_SHADOW
#define BEFORE_REFLECTION MORETOON_BEFORE_REFLECTION
