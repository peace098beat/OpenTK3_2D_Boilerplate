﻿using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace x2DShaderEBO
{
    public class Shader : System.IDisposable
    {
        int Handle;

        public Shader(string vertPath, string fragPath)
        {
            int VertexShader;
            int FragmentShader;

            // -------


            string VertexShaderSource = LoadSource(vertPath);
            VertexShader = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
            {
                System.Console.WriteLine(infoLogVert);
            }

            // -------

            string FragmentShaderSource = LoadSource(fragPath);
            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);
            if (infoLogFrag != System.String.Empty)
            {
                System.Console.WriteLine(infoLogFrag);
            }

            // -------

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            string infoLogLink = GL.GetProgramInfoLog(Handle);
            if( infoLogLink != System.String.Empty)
            {
                System.Console.WriteLine(infoLogLink);
            }

            //When the shader program is linked, it no longer needs the individual shaders attacked to it; 
            //the compiled code is copied into the shader program.

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        private string LoadSource(string path)
        {
            string readContents;

            using(StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }

            return readContents;
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state
                }

                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
