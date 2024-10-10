#version 330

uniform float angle;

in vec3 position;

out vec2 fragCoord;

void main() {
     float tilt = 0.5;
     mat4 tiltMatrix = mat4(
        1.0, 0.0, 0.0, 0.0,
        0.0, cos(tilt), -sin(tilt), 0.0,
        0.0, sin(tilt), cos(tilt), 0.0,
        0.0, 0.0, 0.0, 1.0
    );

    mat4 rotationMatrix = mat4(
        cos(angle), 0.0, sin(angle), 0.0,
        0.0, 1.0, 0.0, 0.0,
        -sin(angle), 0.0, cos(angle), 0.0,
        0.0, 0.0, 0.0, 1.0
    );

    fragCoord = position.xy;
    gl_Position = tiltMatrix * rotationMatrix * vec4(position, 1.0);
}
