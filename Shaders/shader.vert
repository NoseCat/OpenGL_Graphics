#version 460 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;

uniform mat4 model; //posrotscale of object
uniform mat4 view; //posrotscale of camera
uniform mat4 projection; //posrotscale to project on screen

out vec3 FragColor;

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    
    FragColor = aColor;
}