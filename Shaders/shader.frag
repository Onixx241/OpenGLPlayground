#version 330 core

out vec4 FragColor;
//this comes from the shader vertex, out from there in to here
in vec4 vertexColor;

uniform vec4 uniColor;

void main()
{
	FragColor = uniColor;
}