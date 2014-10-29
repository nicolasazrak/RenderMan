//Textura del Render target 2D
texture texSceneRT;
sampler SceneRT = sampler_state
{
	Texture = (texSceneRT);
	MipFilter = NONE;
	MinFilter = NONE;
	MagFilter = NONE;
};

//Offsets y weights de Gaussian Blur
static const int MAX_SAMPLES = 15;
float2 gauss_offsets[MAX_SAMPLES];
float gauss_weights[MAX_SAMPLES];


/**************************************************************************************/
/* DEFAULT */
/**************************************************************************************/


//Input del Vertex Shader
struct VS_INPUT_DEFAULT
{
	float4 Position : POSITION0;
	float2 ScreenPos : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT_DEFAULT
{
	float4 Position : POSITION0;
	float2 ScreenPos : TEXCOORD0;
};


//Vertex Shader
VS_OUTPUT_DEFAULT vs_default(VS_INPUT_DEFAULT Input)
{
	VS_OUTPUT_DEFAULT Output;

	//Proyectar posicion
	Output.Position = float4(Input.Position.xy, 0, 1);

	//Las Texcoord quedan igual
	Output.ScreenPos = Input.ScreenPos;

	return(Output);
}






//Input del Pixel Shader
struct PS_INPUT_DEFAULT
{
	float2 ScreenPos : TEXCOORD0;

};

//Pixel Shader
float4 ps_default(PS_INPUT_DEFAULT Input) : COLOR0
{
	float4 color = tex2D(SceneRT, Input.ScreenPos);
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
	float4 color = tex2D(SceneRT, Input.ScreenPos);

	//Obtener color de textura de alarma, escalado por un factor
	float4 color2 = tex2D(sampler_alarma, Input.ScreenPos) * alarmaScaleFactor;

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
/* GaussianBlurPass */
/**************************************************************************************/

//Pasada de GaussianBlur horizontal o vertical
float4 ps_gaussian_blur_pass(PS_INPUT_DEFAULT Input) : COLOR0
{
	float4 vSample = 0.0f;
	float4 vColor = 0.0f;

	float2 vSamplePosition;

	// Perform a one-directional gaussian blur
	for (int iSample = 0; iSample < MAX_SAMPLES; iSample++)
	{
		vSamplePosition = Input.ScreenPos + gauss_offsets[iSample];
		vColor = tex2D(SceneRT, vSamplePosition);
		vSample += gauss_weights[iSample] * vColor;
	}

	return vSample;
}


technique GaussianBlurPass
{
	pass Pass_0
	{
		VertexShader = compile vs_2_0 vs_default();
		PixelShader = compile ps_2_0 ps_gaussian_blur_pass();
	}
}


technique GaussianYalarma
{
	pass Pass_0
	{
		VertexShader = compile vs_2_0 vs_default();
		PixelShader = compile ps_2_0 ps_gaussian_blur_pass();
	}
	pass Pass_1
	{
		VertexShader = compile vs_2_0 vs_default();
		PixelShader = compile ps_2_0 ps_alarma();
	}
}


