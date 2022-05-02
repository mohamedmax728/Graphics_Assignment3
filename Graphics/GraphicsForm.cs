using System.Windows.Forms;
using System.Threading;
using Tao.OpenGl;

namespace Graphics
{
    public partial class GraphicsForm : Form
    {
		bool OpenGlControlInitialized;
        Renderer renderer = new Renderer();
        Thread MainLoopThread;
        public GraphicsForm()
        {
            InitializeComponent();

			OpenGlControlInitialized = false;
            simpleOpenGlControl1.InitializeContexts();
			OpenGlControlInitialized = true;

            initialize();
            MainLoopThread = new Thread(MainLoop);
            MainLoopThread.Start();

        }
        void initialize()
        {
            renderer.Initialize();   
        }
        
        void MainLoop()
        {
            while (true)
            {
                renderer.Update();
                renderer.Draw();
                //simpleOpenGlControl1.Refresh();
            }
        }
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.CleanUp();
            MainLoopThread.Abort();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            renderer.Draw();
        }

		private void simpleOpenGlControl1_SizeChanged(object sender, System.EventArgs e)
		{
			if (OpenGlControlInitialized)
			{
				Gl.glViewport(simpleOpenGlControl1.Location.X, simpleOpenGlControl1.Location.Y, simpleOpenGlControl1.Size.Width, simpleOpenGlControl1.Size.Height);
			}
		}

    }
}
