﻿int bitReverse(int i, int m)
{
	unsigned int j = i;

	j = (j & 0x55555555) << 1 | (j & 0xAAAAAAAA) >> 1;
	j = (j & 0x33333333) << 2 | (j & 0xCCCCCCCC) >> 2;
	j = (j & 0x0F0F0F0F) << 4 | (j & 0xF0F0F0F0) >> 4;
	j = (j & 0x00FF00FF) << 8 | (j & 0xFF00FF00) >> 8;
	j = (j & 0x0000FFFF) << 16 | (j & 0xFFFF0000) >> 16;

	j >>= (32 - m);

	return j;
}

// Return A * exp(K*ALPHA*i)
float2 twiddle(float2 a,int k,float alpha)
{
  float cs,sn;
  sn = sincos((float)k*alpha,&cs);
  return mul(a,(float2)(cs,sn));
}

// a * e ^ (alpha)
float2 exp_alpha(float2 a, float alpha)
{
	float cs, sn;
	sn = sincos(alpha, &cs);
	return mul(a, (float2)(cs, sn));
}

// In-place DFT-2, output is (a,b). Arguments must be variables.
#define DFT2(a,b) { float2 tmp = a - b; a += b; b = tmp; }

// Compute N/2 x DFT-2.
// N = 2*t is the size of input vectors.
// X[N], Y[N]
// P is the length of input sub-sequences: 1,2,4,...,N/2.
// Each DFT-2 has input (X[I],X[I+T]), I=0..N/2-1,
// and output Y[J],Y|J+P], J = I with one 0 bit inserted at postion P. */
__kernel void fftRadix2(__global const float2 * x,__global float2 * y, int p, int n)
{
  int t = n/2; // worksize 1
  int i = get_global_id(0);   // coord X
  int i2 = get_global_id(1); // coord Y
  int k = i&(p-1);            // index in input sequence, in 0..P-1
  int j = ((i-k)<<1) + k;     // output index
  float alpha = -PI*(float)k/(float)p;

  /*
	int p = 1 << (iter - 1); // 2 ^ (iter - 1)
	int butterflyGrpDist = 1 << iter; // a ^ iter
	int butterflyGrpNum = n >> iter;
	int butterflyGrpBase = (i >> (iter - 1))*(butterflyGrpDist);
	int butterflyGrpOffset = i & (p - 1);

  */
  
  // Read and twiddle input
  x += i + i2*n;
  float2 u0 = x[0];
  float2 u1 = twiddle(x[t],1,alpha);

  // In-place DFT-2
  DFT2(u0,u1);

  // Write output
  y += j + i2*n;
  y[0] = u0;
  y[p] = u1;
}

// i,m => h,g
// i - координата по всему массиву
// m - количество областей размером 2^t
// h - номер области
// g - координата внутри области
#define HG_FROM_IM(i,m,h,g) {h=i%m; g=(i-h)/m;}

#define X_FROM_HGL(h,g,l) h*l+g;

// t <=> 2^t = l
// делим изображение шириной M * L на M областей шириной L
// [0 1 2 3 4 5 6 7 8 9 10 11] => [0 3 6 9] [1 4 7 10] [2 5 8 11]
// в кажой подобласти производится операция перестановки бит
__kernel void splitFft(__read_only image2d_t input, __global float2 * output, int n, int m, int l, int t)
{
	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate

    int2 coord = (int2)(get_global_id(0), get_global_id(1));
	
	float2 val = read_imagef(input, smp, coord).xy;
	int x;
	int y = coord.y;

	if (m > 1)
	{
		x = coord.x;
		int h = x % m; // номер области
		int g = (x - h) / m; // координата внутри области БПФ radix2^t
		//HG_FROM_IM(coord.x,m,h,g);
		g = bitReverse(g, t);

		x = X_FROM_HGL(h, g, l);
	}
	else
	{
		x = bitReverse(coord.x, t);
	}		 

	output[x + y * n] = val;
}

__kernel void mergeFft(__global const float2 * input, __write_only image2d_t output, int n, int m, int l)
{
	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate

    int2 coord = (int2)(get_global_id(0), get_global_id(1));
	if (m > 1)
	{
		int i = coord.x;
		int r = i % l;
		int s = (i - r) / l;

		float2 result = (float2)(0,0);
		float2 val;
		int x;

		float k = 2 * PI / n * (r + s * l);
	
		for (int mi = 0; mi < m; mi ++ )
		{
			x = X_FROM_HGL(mi, r, l)
			val = input[x + coord.y * n];
			result += exp_alpha(val, k * mi);
		}

		write_imagef(output, coord, (float4)(result.x, result.y, 0, 0));
	}
	else
	{
		float2 result = input[coord.x + coord.y * n];
		write_imagef(output, coord, (float4)(result.x, result.y, 0, 0));
	}
}