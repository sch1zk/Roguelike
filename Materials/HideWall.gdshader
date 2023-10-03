shader_type canvas_item;
render_mode unshaded;

void fragment(){
	vec4 behind = textureLod(SCREEN_TEXTURE, SCREEN_UV, 0.0);
	vec4 txtr = texture(TEXTURE, UV);
	if (behind.rgb == vec3(1,0,0) && txtr.rgb != vec3(1,0,1) && txtr.rgb != vec3(0,1,0)) {
		COLOR.rgb = vec3(0,0,1);
	}
}