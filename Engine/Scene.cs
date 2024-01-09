using MonoGame.Extended.Input;

namespace Engine;

public abstract class Scene
{
    public BaseGame Game { get; private set; }
    public Scene(BaseGame game)
    {
        Game = game;
    }
    public abstract void Load();
    public abstract void Update(float dt, KeyboardStateExtended keyState, MouseStateExtended mouseState);
    public abstract void Draw(float dt);
}
