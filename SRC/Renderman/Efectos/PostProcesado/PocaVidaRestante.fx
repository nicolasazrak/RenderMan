/*
* Shaders para efectos de Post Procesadosss
*/


/**************************************************************************************/
/* DEFAULT */
/**************************************************************************************/


//Input del Vertex Shader
struct VS_INPUT_DEFAULT
{
	float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT_DEFAULT
{
	float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
};


//Vertex Shader
VS_OUTPUT_DEFAULT vs_default(VS_INPUT_DEFAULT Input)
{
	VS_OUTPUT_DEFAULT Output;

	//Proyectar posicion
	Output.Position = float4(Input.Position.xy, 0, 1);

	//Las Texcoord quedan igual
	Output.Texcoord = Input.Texcoord;

	return(Output);
}



//Textura del Render target 2D
texture render_target2D;
sampler RenderTarget = sampler_state
{
	Texture = (render_target2D);
	MipFilter = NONE;
	MinFilter = NONE;
	MagFilter = NONE;
};


//Input del Pixel Shader
struct PS_INPUT_DEFAULT
{
	float2 Texcoord : TEXCOORD0;
};

//Pixel Shader
float4 ps_default(PS_INPUT_DEFAULT Input) : COLOR0
{
	float4 color = tex2D(RenderTarget, Input.Texcoord);
	return color;
}



technique DefaultTechnique
{
	pass Pass_0
	{
		VertexShader = compile vs_2_0 vs_default();
		PixelShader = compile ps_2_0 ps_default();
	}
}




/**************************************************************************************/
/* ALARMA */
/**************************************************************************************/

float alarmaScaleFactor = 0.1;

//Textura alarma
texture textura_alarma;
sampler sampler_alarma = sampler_state
{
	Texture = (textura_alarma);
};

//Pixel Shader de Alarma
float4 ps_alarma(PS_INPUT_DEFAULT Input) : COLOR0
{
	//Obtener color segun textura
	float4 color = tex2D(RenderTarget, Input.Texcoord);

	//Obtener color de textura de alarma, escalado por un factor
	float4 color2 = tex2D(sampler_alarma, Input.Texcoord) * alarmaScaleFactor;

	//Mezclar ambos texels
	return color + color2;
}

technique AlarmaTechnique
{
	pass Pass_0
	{
		VertexShader = compile vs_2_0 vs_default();
		PixelShader = compile ps_2_0 ps_alarma();
	}
}





/**************************************************************************************/
/* BLUR */
/**************************************************************************************/

float blur_intensity;

//Pixel Shader de Blur
float4 ps_blur(PS_INPUT_DEFAULT Input) : COLOR0
{
	//Obtener color de textura
	float4 color = tex2D(RenderTarget, Input.Texcoord);

	//Tomar samples adicionales de texels vecinos y sumarlos (formamos una cruz)
	color += tex2D(RenderTarget, float2(Input.Texcoord.x + blur_intensity, Input.Texcoord.y));
	color += tex2D(RenderTarget, float2(Input.Texcoord.x - blur_intensity, Input.Texcoord.y));
	color += tex2D(RenderTarget, float2(Input.Texcoord.x, Input.Texcoord.y + blur_intensity));
	color += tex2D(RenderTarget, float2(Input.Texcoord.x, Input.Texcoord.y - blur_intensity));

	//Promediar todos
	color = color / 5;
	return color;
}




technique BlurTechnique
{
	pass Pass_0
	{
		VertexShader = compile vs_2_0 vs_default();
		PixelShader = compile ps_2_0 ps_blur();
	}
}