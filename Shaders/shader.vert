#version 460 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;
layout (location = 3) in vec3 aColor;
layout (location = 4) in vec3 aTangent;

uniform mat4 model; //posrotscale of object
uniform mat4 view; //posrotscale of camera
uniform mat4 projection; //posrotscale to project on screen

out vec3 Position;
out vec3 Normal;
out vec2 TexCoord;
out vec3 Color;
out vec3 Tangent;

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    

    Position = (model * vec4(aPosition, 1.0)).xyz;
    Normal = normalize(transpose(inverse(mat3(model))) * aNormal); //inefficent
    //transform aNormal vector by transpose inverse of model (droping pos)
    TexCoord = aTexCoord;
    Color = aColor;
    Tangent = normalize(transpose(inverse(mat3(model))) * aTangent);
}