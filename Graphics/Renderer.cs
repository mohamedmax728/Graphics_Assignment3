using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
//include GLM library


using System.IO;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        uint pointsID, indicesID;
        mat4 modmatrix,vimatrix,promatrix,mvp;
        //3D Drawing
        int mvpid;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(0, 0, 0.4f, 1);
            float[] points = { 
		        -0.5f,  0.5f,-1,    0,1,0,
                 0.5f,  0.5f,-1,    0,1,0,
                -0.5f, -0.5f,-1,    0,1,0,
                 0.5f, -0.5f,-1,    0,1,0,

                -0.5f,  0.5f, 1,    0,0,1,
                 0.5f,  0.5f, 1,    0,0,1,
                -0.5f, -0.5f, 1,    0,0,1,
                 0.5f, -0.5f, 1,    0,0,1,
            };
            ushort[] ind =
            {
                0,1,2,
                1,2,3,
                1,5,7,
                1,7,3,
                4,5,7,
                4,7,6,
                0,4,6,
                0,6,2,
                0,4,5,
                0,5,1,
                2,6,3,
                3,6,7
            };

            pointsID = GPU.GenerateBuffer(points);
            indicesID = GPU.GenerateElementBuffer(ind);
            List<mat4> transformations = new List<mat4>()
            {
                glm.scale(new mat4(1),new vec3(2,4,1)),
                glm.rotate(45.0f/180.0f * 3.143f,new vec3(0,1,0))
            };

            modmatrix = MathHelper.MultiplyMatrices(transformations);

            vimatrix = glm.lookAt(new vec3(0, 10, 5),new vec3(0,0,0),new vec3(0,1,0));
            
            promatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.01f, 100);
            List<mat4> tm = new List<mat4>()
            {
                modmatrix,vimatrix,promatrix
            };
            mvp = MathHelper.MultiplyMatrices(tm);
            sh.UseShader();
            mvpid = Gl.glGetUniformLocation(sh.ID, "mvp");
            Gl.glUniformMatrix4fv(mvpid, 1, Gl.GL_FALSE, mvp.to_array());
        }

        public void Draw()
        {
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);
           Gl.glClear(Gl.GL_COLOR_BUFFER_BIT|Gl.GL_DEPTH_BUFFER_BIT);

            
            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);


            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, pointsID);
            Gl.glBindBuffer(Gl.GL_ELEMENT_ARRAY_BUFFER, indicesID);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6*sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3*sizeof(float)));
            

            Gl.glDrawElements(Gl.GL_TRIANGLES, 36, Gl.GL_UNSIGNED_SHORT,IntPtr.Zero);



            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
        }
        public void Update()
        {
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
