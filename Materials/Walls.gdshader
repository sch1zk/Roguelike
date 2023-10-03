shader_type canvas_item;

uniform vec2 offset = vec2(0,0);
varying vec4 behind;

void fragment() {
	behind = textureLod(SCREEN_TEXTURE, SCREEN_UV, 0.0);
	COLOR.a = 0.0;
	if (behind.rgb == vec3(1,1,1) || behind.rgb == vec3(1,0,1)){
		COLOR.a = 1.0;
	}
	else if (behind.rgb == vec3(0,1,0)){
		COLOR.a = 1.0;
		COLOR.rgb = texture(TEXTURE, UV + (offset * TEXTURE_PIXEL_SIZE)).rgb;
	}
	else if (behind.rgb != vec3(0,0,1)){
		COLOR = vec4(0,0,0,1);
	}
}