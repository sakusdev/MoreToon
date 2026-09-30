# MoreToon

MoreToon is an experimental anime-rendering extension built on top of [lilToon](https://github.com/lilxyzw/lilToon).

The goal is to keep lilToon's existing avatar-friendly feature set while adding stronger 2D/anime-style lighting controls.

## Development status

**v0.1 prototype**

The first prototype focuses on the opaque full lilToon shader and adds:

- shadow input quantization
- light-direction quantization
- diffuse lighting bands
- minimum diffuse brightness
- a master strength control

The custom processing runs before lilToon's reflection / MatCap / rim light / emission stages so those effects remain available after the MoreToon diffuse pass.

### Planned

- Cutout / Transparent variants
- Toon specular controls
- Face SDF helpers
- Hair highlight controls
- Camera-aware face/normal correction
- presets and migration helpers

## Shader

After Unity imports the custom lilToon containers, select:

`MoreToon/lilToon`

The first prototype is intentionally limited to **Opaque** while the lighting model is being validated.

## Continuous integration

GitHub Actions runs the Unity EditMode test suite on **Unity 2022.3.22f1**. The shader test imports the MoreToon `.lilcontainer` assets, verifies the generated shaders exist, and fails when Unity's shader compiler reports an error.

Required repository secrets for GameCI Personal activation:

- `UNITY_LICENSE`
- `UNITY_EMAIL`
- `UNITY_PASSWORD`

The workflow uploads the Unity Test Runner artifacts on every run, including failed runs.

## Upstream

MoreToon is based on lilToon and is not an official lilToon project.

lilToon is released under the MIT License. See `Assets/lilToon/LICENSE`.
