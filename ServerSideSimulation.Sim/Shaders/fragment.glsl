#version 330 core

uniform float color;

in vec2 fragCoord;

out vec4 fragColor;

void main()
{
    fragColor = vec4(color, color, color, 1.0);
}
