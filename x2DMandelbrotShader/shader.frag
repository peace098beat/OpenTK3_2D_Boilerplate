#version 330

out vec4 outputColor;

uniform float time;
uniform vec2  mouse;
uniform vec2 resolution;

vec3 hsv(float h, float s, float v){
	vec4 t = vec4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
	vec3 p = abs(fract(vec3(h) + t.xyz) * 6.0 - vec3(t.w));
	return v * mix(vec3(t.x), clamp(p-vec3(t.x), 0.0, 1.0), s);
}

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
	
	// ---------- Mandelbrot
	int j = 0;
	
	vec2 C_center = vec2(0.3, -0.4444444444444445);
	//vec2 C_center = m;

	float MaxC = 2.1;
	MaxC = pow(2, -1*sec);

	vec2 C = (C_center + p * MaxC);


	vec2 z = vec2(0.0, 0.0);
	for(int i=0; i<360; i++){
		j++;
		if(length(z) > 2.0) {break;}
		z = vec2( pow(z.x, 2) - pow(z.y, 2), 2.0*z.x*z.y ) + C;
	}

	float t = float(j) / 360;
	
	float h = mod(sec*30, 360.0) / 360.0;

	// vec3 rgb = hsv(h, 1.0, 1.0);
	// outputColor = vec4(rgb*t, 1.0);
	
	// ----------
		
	vec3 rgb = hsv(h, 1-t, t);

	outputColor = vec4(rgb, 1.0);
	
}