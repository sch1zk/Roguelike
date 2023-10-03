shader_type canvas_item;
render_mode unshaded;

void fragment(){
	vec3 clr = texture(TEXTURE,UV).rgb;
	if(clr==vec3(1,1,1)){
		COLOR = vec4(0,0,0,0.8);
	}
}