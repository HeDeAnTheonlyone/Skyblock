shader_type canvas_item;

uniform vec2 scroll = vec2(0.004, 0.0);
uniform float pixel_size = 400;
uniform float lower_cloud_threshold = 0.4;
uniform float upper_cloud_threshold = 0.75;

void fragment() {    
    // Calculate UV that skips pixels to make it look pixelated and scroll it over time
    vec2 pixelated_UV = floor((UV + scroll * TIME) * vec2(pixel_size)) / vec2(pixel_size);
    
    // Sample the texture using the pixelated UV to get the pixelated version of the texture
    vec4 pixelated_cloud_color = texture(TEXTURE, pixelated_UV);
	
	// Smoothly interpolates opacity for pixels between the lower and upper threshold
	pixelated_cloud_color.a *= smoothstep(lower_cloud_threshold, upper_cloud_threshold, pixelated_cloud_color.r);
	
	// Applies the pixelated texture to the color output 
	COLOR = pixelated_cloud_color;
}
