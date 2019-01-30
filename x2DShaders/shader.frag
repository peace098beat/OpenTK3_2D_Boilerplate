#version 330

out vec4 outputColor;

uniform float time;
uniform vec2  mouse;
uniform vec2 resolution;


void main()
{
	// 
	float ms = time;
	float sec = time/1000;

	// ---------- Screen
	float L = min(resolution.x, resolution.y);
	float Aspect = resolution.y/resolution.x;
	
	// ---------- Fragment Location
	float x = 2.0*( gl_FragCoord.x / resolution.x ) - 1.0;
	float y = 2.0*( gl_FragCoord.y / resolution.y ) - 1.0;
	
	vec2 p = vec2(x, -1.0*y *Aspect);


	// ---------- Mouse
	float mx = 2.0*( mouse.x / resolution.x)-1.0;
	float my = 2.0*( mouse.y / resolution.y )-1.0;
	
	vec2 m = vec2(mx, -1.0*my *Aspect);
	

	// ---------- Color 
	// Point
	// float t = 0.01 / length(p - m);

	// Ring
	// float d = abs(sin(time/1000.0));
	// float t = 0.01 / abs(d - length(p));

	// Gradation
	// vec2 n = vec2(sin(time/1000), cos(time/1000));
	// float t = dot(p,n);
	
	// Cone
	// vec2 n = vec2(sin(time/1000), cos(time/1000));
	// float t = dot(p, n) / (length(p) * length(n));
	
	// ArcTan
	// float theta = atan(p.x, p.y);;
	// float t = sin(theta * 10.0 + time/1000.0);
	
	// Flower
	// float u = sin((atan(p.y, p.x) + sec/2) * 6.0);
	// float t = 0.01 / abs(u - length(p));

	// Wave Ring
	// float u = sin((atan(p.y, p.x) + sec/2.0) * 20.0) * 0.11;
	// float t = 0.01 / abs(0.5 + u - length(p));

	// Flower Fan
	float u = abs(sin((atan(p.y, p.x) - length(p) + sec) *10.0) *0.5) + 0.2;
	float t = 0.01 / abs(u - length(p));


	// ----------

	outputColor = vec4(vec3(t), 1.0);
	
	// ----------

}