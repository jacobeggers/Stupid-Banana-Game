﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace opengl_test
{
    // A simple class meant to help create shaders.
    public class Shader
    {
        public readonly int Handle;

        private readonly Dictionary<string, int> _uniformLocations;

        // This is how you create a simple shader.
        // Shaders are written in GLSL, which is a language very similar to C in its semantics.
        // The GLSL source is compiled *at runtime*, so it can optimize itself for the graphics card it's currently being used on.
        // A commented example of GLSL can be found in shader.vert.
        public Shader(string vertPath, string fragPath)
        {
            // There are several different types of shaders, but the only two you need for basic rendering are the vertex and fragment shaders.
            // The vertex shader is responsible for moving around vertices, and uploading that data to the fragment shader.
            //   The vertex shader won't be too important here, but they'll be more important later.
            // The fragment shader is responsible for then converting the vertices to "fragments", which represent all the data OpenGL needs to draw a pixel.
            //   The fragment shader is what we'll be using the most here.

            // Load vertex shader and compile
            var shaderSource = File.ReadAllText(vertPath);

            // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            // Now, bind the GLSL source code
            GL.ShaderSource(vertexShader, shaderSource);

            // And then compile
            CompileShader(vertexShader);

            // We do the same for the fragment shader.
            shaderSource = File.ReadAllText(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            // These two shaders must then be merged into a shader program, which can then be used by OpenGL.
            // To do this, create a program...
            Handle = GL.CreateProgram();

            // Attach both shaders...
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            // And then link them together.
            LinkProgram(Handle);

            // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
            // Detach them, and then delete them.
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
            // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
            // later.

            // First, we have to get the number of active uniforms in the shader.
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        }

        private static void CompileShader(int shader)
        {
            // Try to compile the shader
            GL.CompileShader(shader);

            // Check for compilation errors
            //GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            //if (code != (int)All.True)
            //{
            //    // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
            //    var infoLog = GL.GetShaderInfoLog(shader);
            //    throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            //}
        }

        private static void LinkProgram(int program)
        {
            // We link the program
            GL.LinkProgram(program);

        }

        // A wrapper function that enables the shader program.
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
        // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }
    }
}
