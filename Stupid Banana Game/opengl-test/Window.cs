using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace opengl_test
{
    public class Window : GameWindow
    {
        Graphics graphics;
        int framerate;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            //WindowState = WindowState.Fullscreen;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            graphics = new Graphics();
            GameLogic.loadGame(Levels.level0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // Call only once other wise strange things begin to happen
            framerate = Framerate.getFramerate();

            // Console.WriteLine(framerate + " : FPS");
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GameLogic.setKeyStrokes(getKeyBoardInput());

            (float[] coordX, float[] coordY, float[] width, float[] height, string[] objectType) = GameLogic.executeGameLogic(framerate);

            for (int i = 0; i < coordX.Length; i++)
            {
                graphics.drawRect(coordX[i], coordY[i], width[i], height[i], objectType[i], Size);
            }

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        private char[] getKeyBoardInput()
        {
            var input = KeyboardState;

            char[] keysPressed = new char[4];

            if (input.IsKeyDown(Keys.A))
            {
                keysPressed[0] = 'A';
            }
            if (input.IsKeyDown(Keys.D))
            {
                keysPressed[0] = 'D';
            }
            if (input.IsKeyDown(Keys.Space))
            {
                keysPressed[1] = '_';
            }
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            return keysPressed;
        }
    }
}