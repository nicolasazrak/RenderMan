// ---------------------------------------------------------
// Ejemplo shader Minimo:
// ---------------------------------------------------------

/**************************************************************************************/
/* Variables comunes */
/**************************************************************************************/

//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

//Textura para DiffuseMap
texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
	Texture = (texDiffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

float time = 0;
float vientoX = 0;
float vientoZ = 0;
float coefY = 0;
int variacionViento = 0;

/**************************************************************************************/
/* RenderScene */
/**************************************************************************************/

//Input del Vertex Shader
struct VS_INPUT 
{
   float4 Position : POSITION0;
   float4 Color : COLOR0;
   float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT 
{
   float4 Position :        POSITION0;
   float2 Texcoord :        TEXCOORD0;
   float4 Color :			COLOR0;
};



//Vertex Shader
VS_OUTPUT vs_main( VS_INPUT Input )
{
   VS_OUTPUT Output;
   //Proyectar posicion
   Output.Position = mul( Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   Output.Texcoord = Input.Texcoord;

   //Propago el color x vertice
   Output.Color = Input.Color;

   return( Output );
   
}


// Ejemplo de un vertex shader que anima la posicion de los vertices 
// ------------------------------------------------------------------
VS_OUTPUT vs_main2( VS_INPUT Input )
{
   VS_OUTPUT Output;

   float Y = Input.Position.y;
   float Z = Input.Position.z;
   float X = Input.Position.x;
	
   float seno = sin(time* 0.01*variacionViento);
   if (seno <= 0){
	   seno = sin((time* 0.01*variacionViento) + 3.141592);
   }

   Input.Position.x = X + (seno * Y * vientoX);
   Input.Position.z = Z + (seno * Y * vientoZ);
   Input.Position.y = Y - (coefY * Input.Position.x * Input.Position.z);

   //Proyectar posicion
   Output.Position = mul( Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   Output.Texcoord = Input.Texcoord;

	// Animar color
   Input.Color.r = abs(sin(time));
   Input.Color.g = abs(cos(time));
   
   //Propago el color x vertice
   Output.Color = Input.Color;

   return( Output );
   
}

//Pixel Shader
float4 ps_main( float2 Texcoord: TEXCOORD0, float4 Color:COLOR0) : COLOR0
{      
	float4 fvBaseColor = Color;
	return tex2D(diffuseMap, Texcoord);

}


// ------------------------------------------------------------------
technique VientoArbol
{
   pass Pass_0
   {
	  VertexShader = compile vs_2_0 vs_main2();
	  PixelShader = compile ps_2_0 ps_main();
   }

}
