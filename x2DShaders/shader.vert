#version 330 core

layout(location = 0) in vec3 aPosition;

uniform float time;
uniform vec2  mouse;
uniform vec2 resolution;

void main(void)
{
	

	gl_Position = vec4(aPosition, 1.0);
}