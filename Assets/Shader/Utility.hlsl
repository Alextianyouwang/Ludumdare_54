#ifndef UTILITY
#define UTILITY

void FlowUVW_float(float2 uv, float2 flowVector, float time, bool flowB, out float3 uvw) {
	float phaseOffset = flowB ? 0.5 : 0;
	float progress = frac(time + phaseOffset);
	uvw.xy = uv - flowVector * progress;
	uvw.z = 1 - abs(1 - 2 * progress);
	
}

#endif

