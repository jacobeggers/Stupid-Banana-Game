using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace opengl_test
{
    internal class Graphics
    {
        private float x;
        private float y;
        private float width;
        private float height;

        private float[] _vertices =
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // top right
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // bottom right
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // bottom left
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        private Shader _shader;

        private Texture _texture;
        public Graphics()
        {
            GL.ClearColor(0.15f, 0.15f, 0.15f, 0.15f);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _texture = Texture.LoadFromFile("Resources/tilemap.png");
            _texture.Use(TextureUnit.Texture0);
        }

        public void drawRect(float x, float y, float width, float height, string objectType, OpenTK.Mathematics.Vector2i windowSize)
        {
            (this.x, this.y, this.width, this.height) = convertToScreenSpace(x, y, width, height, windowSize[0], windowSize[1]);
            drawObject(objectType);
        }

        private (float, float, float, float) convertToScreenSpace(float x, float y, float width, float height, float windowWidth, float windowHeight)
        {
            x = (x - ((windowWidth - 16) / 2)) * (1 / ((windowWidth - 16) / 2));
            y = (y - ((windowHeight - 39) / 2)) * (1 / ((windowHeight - 39) / 2));
            width = width * (1 / ((windowWidth - 16) / 2));
            height = height * (1 / ((windowHeight - 39) / 2));
            return (x, y, width, height);
        }

        public void drawObject(string objectType)
        {
            if (objectType == "wall")
            {
                _vertices = new float[] {
                    x + width, y, 0.0f, 0.5f, 0.5f, // top right ^>
                    x + width, y + height, 0.0f, 0.5f, 0.0f, // bottom right v>
                    x, y + height, 0.0f, 0.0f, 0.0f, // bottom left <v
                    x, y, 0.0f, 0.0f, 0.5f // top left <^
                };

                GL.BufferSubData(BufferTarget.ArrayBuffer, 0, _vertices.Length * sizeof(float), _vertices);

                GL.BindVertexArray(_vertexArrayObject);

                _texture.Use(TextureUnit.Texture0);
                _shader.Use();

                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            } else if (objectType == "player")
            {
                _vertices = new float[] {
                    x + width, y, 0.0f, 1.0f, 0.5f, // top right ^>
                    x + width, y + height, 0.0f, 1.0f, 0.0f, // bottom right v>
                    x, y + height, 0.0f, 0.5f, 0.0f, // bottom left <v
                    x, y, 0.0f, 0.5f, 0.5f // top left <^
                };

                GL.BufferSubData(BufferTarget.ArrayBuffer, 0, _vertices.Length * sizeof(float), _vertices);

                GL.BindVertexArray(_vertexArrayObject);

                _texture.Use(TextureUnit.Texture0);
                _shader.Use();

                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }
    }
}
