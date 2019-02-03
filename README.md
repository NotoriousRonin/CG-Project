# Procedural Generation - CG Project 

The program contains a UI to set parameters used for the procedural generation of the 2DTexture or Mesh.

**Frequency**: 

Frequency of the noise

**Seed**:

Perlin Noise gives the same Values, Seed used to create Randomness 

**Multiple Octaves**:

Adding multiple octaves makes the noise more realistic due more edges, 
$Octave# $Lacunarity $Persistance affect the noise if this is enabled

**Octave#**:

Number of Octaves

**Lacunarity**:

Controls Decrease in Amplitude of Octaves

**Persistance**:

Controls Decrease in Amplitude of Octaves

**Mesh View**: 

Enables the View of a Mesh, $MountainHeight and $Exponent only affecting if this is enabled

**MountainHeight**:

Maximum height of a Mountain

**Exponent**: 

Noise contains the curve f(x) = x^exp, Exponent influences how the values from 0..1 are affected by $MountainHeight

**Biome**:

Enables this adds a biome by setting each pixel to a certain color based on the height and the gradient.

**Gradient Editor**:

UI to edit the gradient.  

**Note**: 

- Unity 2018.3.1 was used
- Package File needed for the import of the Lightweight Rendering Pipeline and the ShaderGraph. 

Lightweight Rendering Pipeline:

https://docs.unity3d.com/Packages/com.unity.render-pipelines.lightweight@4.0/manual/index.html

ShaderGraph:

https://blogs.unity3d.com/2018/12/19/unity-2018-3-shader-graph-update-lit-master-node/
