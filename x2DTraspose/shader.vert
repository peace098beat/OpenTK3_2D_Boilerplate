#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main(void)
{
	mat4 coordinate = model * view * projection;
	//mat4 coordinate = model * view ;

	texCoord = aTexCoord;
	gl_Position = coordinate * vec4(aPosition, 1.0);
}