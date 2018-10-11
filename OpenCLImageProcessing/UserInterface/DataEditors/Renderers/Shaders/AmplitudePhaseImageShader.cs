using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Processing;
using Processing.DataAttributes;
using UserInterface.DataEditors.InterfaceBinding.Attributes;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    [TargetImage(ImageFormat = ImageFormat.AmplitudePhase)]
    public class AmplitudePhaseImageShader : SimpleShaderWithProjectionViewModelMatricesBase
    {
        protected override string FragmentShaderCode => @"
#version 410 core
in vec2 TexCoord;
out vec4 color;

uniform int mode;
uniform float offset;
uniform float multiplier;
uniform sampler2D ourTexture;

void main()
{
    color = texture(ourTexture, TexCoord); // r - amplitude, g - phase
    float val;
    
    if (mode == 0) // amplitude
    {
        val = color.r;
    }
    else if (mode == 1) // phase
    {
        val = color.g;
    }
    else if (mode == 2) // real
    {
        val = color.r * cos(color.g);
    }
    else // imaginative
    {
        val = color.r * sin(color.g);
    }   
    
    val = val * multiplier + offset;
    color = vec4(val, val, val, 1);
}
";

        public enum ViewMode
        {
            Amplitude = 0,
            Phase = 1,
            Real = 2,
            Imaginative = 3
        }

        [BindToUI]
        public ViewMode Mode { get; set; }

        [BindToUI("Offset"), Range(-1, 1), Precision(2)]
        public float Offset { get; set; } = 0;

        [BindToUI("Multiplier"), Range(-10, 10), Precision(4)]
        public float Multiplier { get; set; } = 1;

        public override void Use()
        {
            base.Use();

            SetUniform1("offset", Offset);
            SetUniform1("multiplier", Multiplier);
            SetUniform1("mode", (int) Mode);
        }

        public override void SetValueRange(FloatRange channel1Range, FloatRange channel2Range)
        {
        }
    }
}