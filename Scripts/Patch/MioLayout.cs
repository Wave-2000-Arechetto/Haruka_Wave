using Godot;
using MinionLib.Layout;


namespace Haruka.Scripts.Patch;
public class MioLayout : IMinionLayout
{
    public bool IsActive => true;

    public void ApplyLayout(MinionLayoutContext context)
    {
        foreach (var node in context.Positions.Keys.ToList())
        {
            if (node.Entity.Monster is Monster.Mio)
                context.Positions[node] += new Vector2(100, 0);
        }
    }
}